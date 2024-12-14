using LGC_CodeChallenge.SDK.Exceptions;
using LGC_CodeChallenge.SDK.Interfaces;
using System.Text.Json;

namespace LGC_CodeChallenge.SDK
{
    public class SdkClient: ISdkClient
    {
        private readonly HttpClient _httpClient;
        public SdkClient(HttpClient httpClient) 
        {
            _httpClient = httpClient;
        }

        public async Task<TResponse> GetAsync<TResponse>(string endpoint)
        {
            var response = await _httpClient.GetAsync(endpoint);

            // If the response is unsuccessful, throw a ProblemDetailException
            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = await ProblemDetailException.DeserializeAsync(response);
                throw new ProblemDetailException(
                    response.StatusCode,
                    problemDetails?.Type,
                    problemDetails?.Title,
                    problemDetails?.Detail,
                    problemDetails?.Instance
                );
            }
            //response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TResponse>(content);
        }

        public async Task<List<TResponse>> GetAllAsync<TResponse>(string endpoint)
        {
            var response = await _httpClient.GetAsync(endpoint);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = await ProblemDetailException.DeserializeAsync(response);
                throw new ProblemDetailException(
                    response.StatusCode,
                    problemDetails?.Type,
                    problemDetails?.Title,
                    problemDetails?.Detail,
                    problemDetails?.Instance
                );
            }
            //response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<TResponse>>(content);
        }


        public async Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest request)
        {
            var content = JsonSerializer.Serialize(request);
            var httpContent = new StringContent(content, System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(endpoint, httpContent);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = await ProblemDetailException.DeserializeAsync(response);
                throw new ProblemDetailException(
                    response.StatusCode,
                    problemDetails?.Type,
                    problemDetails?.Title,
                    problemDetails?.Detail,
                    problemDetails?.Instance
                );
            }
            //response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TResponse>(responseBody);
        }
        public async Task<TResponse> PutAsync<TRequest, TResponse>(string endpoint, TRequest request)
        {
            var content = JsonSerializer.Serialize(request);
            var httpContent = new StringContent(content, System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync(endpoint, httpContent);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = await ProblemDetailException.DeserializeAsync(response);
                throw new ProblemDetailException(
                    response.StatusCode,
                    problemDetails?.Type,
                    problemDetails?.Title,
                    problemDetails?.Detail,
                    problemDetails?.Instance
                );
            }
            //response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TResponse>(responseBody);
        }

        public async Task DeleteAsync(string endpoint)
        {
            var response = await _httpClient.DeleteAsync(endpoint);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = await ProblemDetailException.DeserializeAsync(response);
                throw new ProblemDetailException(
                    response.StatusCode,
                    problemDetails?.Type,
                    problemDetails?.Title,
                    problemDetails?.Detail,
                    problemDetails?.Instance
                );
            }
            //response.EnsureSuccessStatusCode();
        }


    }
}
