using LGC_Code_Challenge.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LGC_CodeChallenge.SDK.Interfaces
{
    public interface ISdkClient
    {
        Task<TResponse> GetAsync<TResponse>(string endpoint);
        Task<List<TResponse>> GetAllAsync<TResponse>(string endpoint);
        Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest request);
        Task<TResponse> PutAsync<TRequest, TResponse>(string endpoint,TRequest request);
        Task DeleteAsync(string endpoint);

    }
}
