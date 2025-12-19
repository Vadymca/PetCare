namespace PetCare.Infrastructure.Search.Elastic;

using global::Elastic.Clients.Elasticsearch;
using global::Elastic.Transport;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PetCare.Infrastructure.Search.Options;

/// <summary>
/// Configures Elasticsearch dependencies.
/// </summary>
internal static class ElasticSearchDependencyInjection
{
    /// <summary>
    /// This method adds Elasticsearch client to the service collection.
    /// </summary>
    /// <param name="services">The service collection to add the client to.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddElasticSearch(
        this IServiceCollection services)
    {
        services.AddSingleton<ElasticsearchClient>(sp =>
        {
            var options = sp
                .GetRequiredService<IOptions<ElasticSearchOptions>>()
                .Value;

            var settings = new ElasticsearchClientSettings(
                    new Uri(options.Url))
                .DefaultIndex(options.IndexName)
                .Authentication(new BasicAuthentication(
                    options.Username!,
                    options.Password!))
                .ServerCertificateValidationCallback(
                    (_, _, _, _) => true);

            return new ElasticsearchClient(settings);
        });

        return services;
    }
}
