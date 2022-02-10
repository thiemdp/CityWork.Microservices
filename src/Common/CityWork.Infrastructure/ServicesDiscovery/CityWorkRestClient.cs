using Polly;
using Steeltoe.Common.Discovery;
using Steeltoe.Discovery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using RestEase;
using Polly.Retry;


namespace CityWork.Infrastructure
{
    public class CityWorkRestClient : ICityWorkRestClient
    {
        private static HttpStatusCode[] httpStatusCodesWorthRetrying = {
           HttpStatusCode.RequestTimeout, // 408
           HttpStatusCode.InternalServerError, // 500
           HttpStatusCode.BadGateway, // 502
           HttpStatusCode.ServiceUnavailable, // 503
           HttpStatusCode.GatewayTimeout // 504
        };
        DiscoveryHttpClientHandler _handler;
       private readonly AsyncRetryPolicy _retryPolicy = Policy.Handle<HttpRequestException>()
              .WaitAndRetryAsync(retryCount: 3, sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(3));
        public CityWorkRestClient(IDiscoveryClient client)
        {
            _handler = new DiscoveryHttpClientHandler(client);
        }

        private ICityWorkRestClient GetClient(string url)
        {
            var httpClient = new HttpClient(_handler, false)
            {
                BaseAddress = new Uri(url)
            };
            return  RestClient.For<ICityWorkRestClient>(httpClient);
        }
        public async Task DeleteAsync(string endpoint, CancellationToken cancellationToken)
        {
            var client = GetClient(endpoint);
            await _retryPolicy.ExecuteAsync(() => client.DeleteAsync(string.Empty, cancellationToken));
        }

        public async Task<T> DeleteAsync<T>(string endpoint, CancellationToken cancellationToken)
        {
            var client = GetClient(endpoint);
            
            return await _retryPolicy.ExecuteAsync(() => client.DeleteAsync<T>(string.Empty, cancellationToken));
        }

        public async Task<string> GetAsync(string endpoint, CancellationToken cancellationToken)
        {
            var client = GetClient(endpoint);
            
            return await _retryPolicy.ExecuteAsync(() => client.GetAsync(string.Empty, cancellationToken));
        }

        public async Task<T> GetAsync<T>(string endpoint, CancellationToken cancellationToken)
        {
            //var client = new HttpClient(_handler, false);
            //string str = await client.GetStringAsync(endpoint);
            //return JsonSerializer.Deserialize<T>(str, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            var client = GetClient(endpoint);

            return await _retryPolicy.ExecuteAsync(() => client.GetAsync<T>(string.Empty, cancellationToken));
        }

        public async Task<string> PostAsync<T>(string endpoint, T item, CancellationToken cancellationToken)
        {
            var client = GetClient(endpoint);
           
            return await _retryPolicy.ExecuteAsync(() => client.PostAsync<T>(string.Empty, item, cancellationToken));
        }

        public async Task<TResult> PostAsync<T, TResult>([Path] string endpoint, [Body] T item, CancellationToken cancellationToken)
        {
            var client = GetClient(endpoint);
           
            return await _retryPolicy.ExecuteAsync(() => client.PostAsync<T, TResult>(string.Empty, item, cancellationToken));
        }

        public async Task<string> PutAsync<T>([Path] string endpoint, [Body] T item, CancellationToken cancellationToken)
        {
            var client = GetClient(endpoint);
            
            return await _retryPolicy.ExecuteAsync(() => client.PutAsync<T>(string.Empty, item, cancellationToken));
        }

        public async Task<TResult> PutAsync<T, TResult>([Path] string endpoint, [Body] T item, CancellationToken cancellationToken)
        {
            var client = GetClient(endpoint);
            return await _retryPolicy.ExecuteAsync(() => client.PutAsync<T, TResult>(string.Empty, item, cancellationToken));
        }
    }
}
