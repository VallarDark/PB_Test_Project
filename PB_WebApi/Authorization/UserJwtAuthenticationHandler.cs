﻿using Domain.Agregates.UserAgregate;
using Domain.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace PB_WebApi.Authorization
{
    internal class UserJwtAuthenticationHandler : AuthenticationHandler<JwtBearerOptions>
    {
        private const string INVALID_AUTHORIZE_ERROR = "Invalid authorization data";

        private readonly IUserService _userService;
        private readonly IUserTokenProvider _tokenProvider;

        public UserJwtAuthenticationHandler(
            IUserService userService,
            IUserTokenProvider tokenProvider,
            IOptionsMonitor<JwtBearerOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock) : base(options, logger, encoder, clock)
        {
            _userService = userService;
            _tokenProvider = tokenProvider;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            string? token = null;
            try
            {
                var messageReceivedContext = new MessageReceivedContext(Context, Scheme, Options);

                string? authorization = Context.Request.Headers.Authorization;

                if (string.IsNullOrEmpty(authorization))
                {
                    return AuthenticateResult.Fail(INVALID_AUTHORIZE_ERROR);
                }

                var scheme = JwtBearerDefaults.AuthenticationScheme + " ";

                if (authorization.StartsWith(
                    scheme,
                    StringComparison.OrdinalIgnoreCase))
                {
                    token = authorization.Substring(scheme.Length).Trim();
                }

                if (string.IsNullOrEmpty(token))
                {
                    return AuthenticateResult.Fail(INVALID_AUTHORIZE_ERROR);
                }

                List<Exception>? validationFailures = null;
                User? result = null;
                SecurityToken? validatedToken = null;

                foreach (var validator in Options.SecurityTokenValidators)
                {
                    if (validator.CanReadToken(token))
                    {
                        ClaimsPrincipal? principal = null;

                        try
                        {
                            principal = _tokenProvider.ReadToken(
                               token,
                               Options.TokenValidationParameters,
                               out validatedToken);

                            if (principal == null)
                            {
                                continue;
                            }

                            var email = principal.Claims.FirstOrDefault(c =>
                                c.Type == ClaimType.Email.ToString())?.Value;

                            var name = principal.Claims.FirstOrDefault(c =>
                                c.Type == ClaimType.Name.ToString())?.Value;

                            var lastName = principal.Claims.FirstOrDefault(c =>
                                c.Type == ClaimType.LastName.ToString())?.Value;

                            var sid = principal.Claims.FirstOrDefault(c =>
                                c.Type == ClaimType.Sid.ToString())?.Value;

                            var role = principal.Claims.FirstOrDefault(c =>
                                c.Type == ClaimType.Role.ToString())?.Value;

                            var passworHash = principal.Claims.FirstOrDefault(c =>
                                c.Type == ClaimType.PasswordHash.ToString())?.Value;

                            if (string.IsNullOrEmpty(email)
                                || string.IsNullOrEmpty(name)
                                || string.IsNullOrEmpty(lastName)
                                || string.IsNullOrEmpty(sid)
                                || string.IsNullOrEmpty(role)
                                || string.IsNullOrEmpty(passworHash))
                            {
                                continue;
                            }

                            if (Enum.TryParse<UserRoleType>(EncodingUtils.DecodeData(role), true, out var roleType))
                            {
                                var personalData = new UserValidationDto()
                                {
                                    Email = EncodingUtils.DecodeData(email),
                                    Name = EncodingUtils.DecodeData(name),
                                    LastName = EncodingUtils.DecodeData(lastName),
                                    SessionToken = EncodingUtils.DecodeData(sid),
                                    Role = roleType,
                                    PasswordHash = passworHash
                                };

                                result = await _userService.VerifyUser(personalData);
                            }
                        }
                        catch (SecurityTokenExpiredException)
                        {
                            Context.Response.Headers.Add("Token-Expired", "true");
                        }
                        catch (Exception ex)
                        {
                            Logger.LogError(ex.Message);

                            if (Options.RefreshOnIssuerKeyNotFound && Options.ConfigurationManager != null
                                && ex is SecurityTokenSignatureKeyNotFoundException)
                            {
                                Options.ConfigurationManager.RequestRefresh();
                            }

                            if (validationFailures == null)
                            {
                                validationFailures = new List<Exception>(1);
                            }

                            validationFailures.Add(ex);

                            continue;
                        }

                        if (validatedToken != null)
                        {
                            var tokenValidatedContext = new TokenValidatedContext(Context, Scheme, Options)
                            {
                                Principal = principal,
                                SecurityToken = validatedToken
                            };

                            tokenValidatedContext.Properties.ExpiresUtc = GetSafeDateTime(validatedToken.ValidTo);
                            tokenValidatedContext.Properties.IssuedUtc = GetSafeDateTime(validatedToken.ValidFrom);

                            if (tokenValidatedContext.Result != null)
                            {
                                return tokenValidatedContext.Result;
                            }

                            if (Options.SaveToken)
                            {
                                tokenValidatedContext.Properties.StoreTokens(new[]
                                {
                                    new AuthenticationToken { Name = "access_token", Value = token }
                                });
                            }

                            if (result == null)
                            {
                                return AuthenticateResult.Fail(INVALID_AUTHORIZE_ERROR);
                            }

                            tokenValidatedContext.Success();
                            return tokenValidatedContext.Result!;
                        }
                    }
                }

                if (validationFailures != null)
                {
                    var authenticationFailedContext = new AuthenticationFailedContext(Context, Scheme, Options)
                    {
                        Exception = (validationFailures.Count == 1) ? validationFailures[0] : new AggregateException(validationFailures)
                    };

                    if (authenticationFailedContext.Result != null)
                    {
                        return authenticationFailedContext.Result;
                    }

                    return AuthenticateResult.Fail(authenticationFailedContext.Exception);
                }

                return AuthenticateResult.Fail("No SecurityTokenValidator available for token.");
            }
            catch (Exception ex)
            {
                var authenticationFailedContext = new AuthenticationFailedContext(Context, Scheme, Options)
                {
                    Exception = ex
                };

                if (authenticationFailedContext.Result != null)
                {
                    return authenticationFailedContext.Result;
                }

                throw;
            }
        }

        private static DateTime? GetSafeDateTime(DateTime dateTime)
        {
            if (dateTime == DateTime.MinValue)
            {
                return null;
            }
            return dateTime;
        }
    }
}

