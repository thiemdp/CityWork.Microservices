
using RestEase;

namespace CityWork.Infrastructure
{
    public interface ICityWorkRestClient
    {
        [Get]
        Task<string> GetAsync(string endpoint, CancellationToken cancellationToken);
        [Get]
        Task<T> GetAsync<T>( string endpoint, CancellationToken cancellationToken);
        [Post]
        Task<string> PostAsync<T>( string endpoint, [Body] T item, CancellationToken cancellationToken);
        [Post]
        Task<TResult> PostAsync<T, TResult>( string endpoint, [Body] T item, CancellationToken cancellationToken);
        [Put]
        Task<string> PutAsync<T>( string endpoint, [Body] T item, CancellationToken cancellationToken);
        [Put]
        Task<TResult> PutAsync<T, TResult>( string endpoint, [Body] T item, CancellationToken cancellationToken);
        [Delete]
        Task DeleteAsync( string endpoint, CancellationToken cancellationToken);
        [Delete]
        Task<T> DeleteAsync<T>( string endpoint, CancellationToken cancellationToken);
    }
}
