using Azure;
using Azure.Core.Serialization;
using Azure.Identity;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Configuration;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Documents;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;

namespace SFA.DAS.FindApprenticeship.Jobs.Application.Services;
public class AzureSearchHelper : IAzureSearchHelper
{
    private readonly ILogger<AzureSearchHelper> _logger;
    private readonly SearchIndexClient _adminIndexClient;
    private readonly DefaultAzureCredential _azureKeyCredential;
    private readonly SearchClientOptions _clientOptions;
    private readonly Uri _endpoint;

    public AzureSearchHelper(FindApprenticeshipJobsConfiguration configuration, ILogger<AzureSearchHelper> logger)
    {
        _logger = logger;
        _clientOptions = new SearchClientOptions
        {
            Serializer = new JsonObjectSerializer(new System.Text.Json.JsonSerializerOptions
            {
                Converters =
                {
                    new MicrosoftSpatialGeoJsonConverter()
                }
            })
        };

        _azureKeyCredential = new DefaultAzureCredential();
        _endpoint = new Uri(configuration.AzureSearchBaseUrl);
        _adminIndexClient = new SearchIndexClient(_endpoint, _azureKeyCredential, _clientOptions);
    }

    public async Task CreateIndex(string indexName)
    {
        try
        {
            var fieldBuilder = new FieldBuilder();
            var searchFields = fieldBuilder.Build(typeof(ApprenticeAzureSearchDocument));

            var definition = new SearchIndex(indexName, searchFields);

            var suggester = new SearchSuggester("sg", new[] { "Title", "Course/Title", "Description" });
            definition.Suggesters.Add(suggester);

            await _adminIndexClient.CreateOrUpdateIndexAsync(definition);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failure returned when creating index with name {IndexName}", indexName);
            throw new RequestFailedException($"Failure returned when creating index with name {indexName}", ex);
        }
    }

    public async Task DeleteIndex(string indexName)
    {
        var index = await GetIndex(indexName);
        
        try
        {
            if (index.Value != null)
            {
                await _adminIndexClient.DeleteIndexAsync(indexName);
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failure returned when deleting index with name {IndexName}", indexName);
            throw new RequestFailedException($"Failure returned when deleting index with name {indexName}", ex);
        }
    }

    public async Task UploadDocuments(string indexName, IEnumerable<ApprenticeAzureSearchDocument> documents)
    {
        try
        {
            var searchClient = new SearchClient(_endpoint, indexName, _azureKeyCredential, _clientOptions);
            await searchClient.MergeOrUploadDocumentsAsync(documents);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "returned when uploading documents to index with name {IndexName}", indexName);
            throw new RequestFailedException("Failure returned when uploading documents to index", ex);
        }
    }

    public async Task<Response<SearchIndex>> GetIndex(string indexName)
    {
        try
        {
            return await _adminIndexClient.GetIndexAsync(indexName);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failure returned when requesting index with name {IndexName}", indexName);
            throw new RequestFailedException($"Failure returned when requesting index with name {indexName}", ex);
        }
    }


    public async Task<List<SearchIndex>> GetIndexes()
    {
        try
        {
            var result = new List<SearchIndex>();

            var indexPageable = _adminIndexClient.GetIndexesAsync();

            await foreach (var index in indexPageable)
            {
                result.Add(index);
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failure returned when requesting indexes");
            throw new RequestFailedException("Failure returned when requesting indexes", ex);
        }
    }

    public async Task<SearchAlias> GetAlias(string aliasName)
    {
        try
        {
            return await _adminIndexClient.GetAliasAsync(aliasName);
        }
        catch (RequestFailedException)
        {
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failure returned when requesting alias {AliasName}", aliasName);
            throw new RequestFailedException($"Failure returned when requesting alias {aliasName}", ex);
        }
    }

    public async Task<Response<ApprenticeAzureSearchDocument>> GetDocument(string indexName, string vacancyReference)
    {
        try
        {
            var searchClient = new SearchClient(_endpoint, indexName, _azureKeyCredential, _clientOptions);
            return await searchClient.GetDocumentAsync<ApprenticeAzureSearchDocument>(vacancyReference);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failure returned when requesting document {VacancyReference}", vacancyReference);
            throw new RequestFailedException($"Failure returned when requesting document {vacancyReference}", ex);
        }
    }

    public async Task UpdateAlias(string aliasName, string indexName)
    {
        try
        {
            var myAlias = new SearchAlias(aliasName, indexName);
            await _adminIndexClient.CreateOrUpdateAliasAsync(aliasName, myAlias);
        }
        catch (Exception ex)
        {
            throw new RequestFailedException($"Failure returned when updating alias {aliasName} for index {indexName}", ex);
        }
    }

    public async Task DeleteDocuments(string indexName, IEnumerable<string> ids)
    {
        try
        {
            var searchClient = new SearchClient(_endpoint, indexName, _azureKeyCredential, _clientOptions);
            await searchClient.DeleteDocumentsAsync("Id", ids);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, $"Failure returned when deleting document(s) with reference(s) {string.Join(", ", ids)}");
        }
    }
}
