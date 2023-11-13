using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azure;
using Azure.Core.Serialization;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Configuration;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Documents;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;

namespace SFA.DAS.FindApprenticeship.Jobs.Application.Services;
public class AzureSearchIndexService : IAzureSearchIndexService
{
    private readonly SearchIndexClient _adminIndexClient;
    private readonly SearchClient _searchClient;

    public AzureSearchIndexService(FindApprenticeshipJobsConfiguration configuration)
    {
        var clientOptions = new SearchClientOptions
        {
            Serializer = new JsonObjectSerializer(new System.Text.Json.JsonSerializerOptions
            {
                Converters =
                {
                    new MicrosoftSpatialGeoJsonConverter()
                }
            })
        };

        var credential = new AzureKeyCredential(configuration.AzureSearchKey);
        var endpoint = new Uri(configuration.AzureSearchBaseUrl);
        _adminIndexClient = new SearchIndexClient(endpoint, credential, clientOptions);
        _searchClient = new SearchClient(endpoint, "vacancies", credential, clientOptions);
    }

    public async Task CreateIndex(string indexName)
    {
        var fieldBuilder = new FieldBuilder();
        var searchFields = fieldBuilder.Build(typeof(ApprenticeAzureSearchDocument));

        var definition = new SearchIndex(indexName, searchFields);

        var suggester = new SearchSuggester("sg", new[] { "Title", "Course/Title", "Description" });
        definition.Suggesters.Add(suggester);

        await _adminIndexClient.CreateOrUpdateIndexAsync(definition);
    }

    public async Task DeleteIndex(string indexName)
    {
        var indexExists = await GetIndex(indexName);
        if (indexExists.Value != null)
        {
            await _adminIndexClient.DeleteIndexAsync(indexName);
        }
    }

    public async Task UploadDocuments(IEnumerable<ApprenticeAzureSearchDocument> documents)
    {
        await _searchClient.MergeOrUploadDocumentsAsync(documents);
    }

    public async Task<Response<SearchIndex>> GetIndex(string indexName)
    {
        return await _adminIndexClient.GetIndexAsync(indexName);
    }
}
