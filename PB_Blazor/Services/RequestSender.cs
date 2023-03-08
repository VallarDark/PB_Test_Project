using Domain.Exceptions;
using Domain.Utils;
using Newtonsoft.Json;
using PB_Blazor.Errors;
using PresentationModels.Models;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;

namespace PB_Blazor.Services
{
    public class RequestSender
    {
        #region Constants

        private const string BAD_REQUEST_DATA =
            "Bad request data";

        private const string TOKEN_EXPIRED_EXCEPTION =
            "Token has been expired";

        private const string UNAUTHORIZED_EXCEPTION =
            "You should be authorized to access resource";

        private const string LOW_PREVILEGIES_LEVEL_EXCEPTION =
            "You have too low privileges level";

        private const string URL_PATTERN =
            @"(www|http:|https:)+[^\s]+[\w]";

        #endregion

        private readonly IConfiguration _configuration;
        private readonly Regex _urlRegex;

        public event Func<Task<UserInfoDto?>>? OnTokenExpired;
        public event Func<Task>? OnTokenInvalid;
        private bool isTokenExpired;
        private bool isLoggedOut;

        #region InnerItems

        public enum RequestType
        {
            HttpGet,
            HttpPost,
            HttpPut,
            HttpDelete
        }

        public class Request
        {
            public string? Url { get; set; }

            public string? AuthToken { get; set; }

            public RequestType Type { get; set; }

            public object? Data { get; set; }
        }

        #endregion

        public RequestSender(IConfiguration configuration)
        {
            _configuration = configuration;
            _urlRegex = new Regex(URL_PATTERN);
            isTokenExpired = false;
            isLoggedOut = false;
        }

        public async Task<T?> Send<T>(Request request)
        {
            EnsuredUtils.EnsureNotNull(request);

            EnsuredUtils.EnsureStringIsNotEmptyAndMathPattern(request.Url, _urlRegex);

            var response = await SendRequest(
            request.Url,
            request.AuthToken,
            request.Type,
            JsonContent.Create(request?.Data));

            EnsuredUtils.EnsureNotNull(response);

            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                if (response.Headers.Any(h => h.Key == "Invalid-Token"))
                {
                    if (OnTokenInvalid != null && !isLoggedOut)
                    {
                        isLoggedOut = true;

                        try
                        {
                            await OnTokenInvalid.Invoke();
                        }
                        finally
                        {
                            isLoggedOut = false;
                        }
                    }

                    throw new UnauthorizedAccessException(UNAUTHORIZED_EXCEPTION);
                }

                if (response.Headers.Any(h => h.Key == "Token-Expired"))
                {
                    if (OnTokenExpired != null
                        && !isTokenExpired
                        && !isLoggedOut)
                    {
                        isTokenExpired = true;

                        var uerInfo = await OnTokenExpired.Invoke();

                        request.AuthToken = uerInfo?.TokenDto?.Token;

                        try
                        {
                            return await Send<T>(request);
                        }
                        finally
                        {
                            isTokenExpired = false;
                        }
                    }
                    else
                    {
                        throw new UnauthorizedAccessException(UNAUTHORIZED_EXCEPTION);
                    }
                }

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    if (OnTokenInvalid != null && !isLoggedOut)
                    {
                        isLoggedOut = true;

                        try
                        {
                            await OnTokenInvalid.Invoke();
                        }
                        finally
                        {
                            isLoggedOut = false;
                        }
                    }

                    throw new UnauthorizedAccessException(UNAUTHORIZED_EXCEPTION);
                }

                if (response.Headers.Any(h => h.Key == "Low-Privileges-Level"))
                {
                    throw new LowPrivilegesLevelException(LOW_PREVILEGIES_LEVEL_EXCEPTION);
                }

                var error = JsonConvert.DeserializeObject<ErrorDetails>(content);

                throw new Exception(error?.Message ?? "Something gone wrong");
            }

            var result = JsonConvert.DeserializeObject<T>(content);

            return result;
        }

        private async Task<HttpResponseMessage?> SendRequest(
            string requestEndpoint,
            string? authToken,
            RequestType requestType,
            HttpContent? request = null)
        {
            using (var client = new HttpClient())
            {
                if (!string.IsNullOrEmpty(authToken))
                {
                    client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", authToken);
                }

                switch (requestType)
                {
                    case RequestType.HttpGet:
                        {
                            return await client.GetAsync(requestEndpoint);
                        }
                    case RequestType.HttpPost:
                        {
                            if (request == null)
                            {
                                throw new BadRequest(BAD_REQUEST_DATA);
                            }

                            return await client.PostAsync(requestEndpoint, request);
                        }
                    case RequestType.HttpPut:
                        {
                            if (request == null)
                            {
                                throw new BadRequest(BAD_REQUEST_DATA);
                            }

                            return await client.PutAsync(requestEndpoint, request);
                        }
                    case RequestType.HttpDelete:
                        {
                            return await client.DeleteAsync(requestEndpoint);
                        }
                    default:
                        {
                            return null;
                        }
                }
            }
        }
    }
}
