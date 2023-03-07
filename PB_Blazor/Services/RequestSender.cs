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


        private readonly Regex _urlRegex;

        public enum RequestType
        {
            HttpGet,
            HttpPost,
            HttpPut,
            HttpDelete
        }

        private readonly IConfiguration _configuration;

        public RequestSender(IConfiguration configuration)
        {
            _configuration = configuration;
            _urlRegex = new Regex(URL_PATTERN);
        }

        public async Task<T?> Send<T>(
            string requestUrl,
            string? authToken,
            RequestType requestType,
            object? requestData)
        {

            EnsuredUtils.EnsureStringIsNotEmptyAndMathPattern(requestUrl, _urlRegex, BAD_REQUEST_DATA);

            HttpResponseMessage? response;

            if (requestData != null)
            {
                response = await SendRequest(
                    requestUrl,
                    authToken,
                    requestType,
                    JsonContent.Create(requestData));
            }
            else
            {
                response = await SendRequest(
                    requestUrl,
                    authToken,
                    requestType,
                    null);
            }

            EnsuredUtils.EnsureNotNull(response);

            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                if (response.Headers.Any(h => h.Key == "Token-Expired"))
                {
                    throw new TokenExpiredException(TOKEN_EXPIRED_EXCEPTION);
                }

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
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
