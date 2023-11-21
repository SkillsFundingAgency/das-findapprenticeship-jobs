﻿using Azure.Core.Serialization;
using Azure.Search.Documents;
using Azure;
using System;
using Azure.Search.Documents.Indexes;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Configuration;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;
using Azure.Search.Documents.Indexes.Models;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Documents;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFA.DAS.FindApprenticeship.Jobs.Application.Services;
public class AzureSearchHelper : IAzureSearchHelper
{
    private readonly SearchIndexClient _adminIndexClient;
    private readonly SearchClient _searchClient;
    public AzureSearchHelper(FindApprenticeshipJobsConfiguration configuration)
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
        _searchClient = new SearchClient(endpoint, "apprenticeships", credential, clientOptions);
        // Use for 'vacancies' index:
        //_searchClient = new SearchClient(endpoint, "vacancies", credential, clientOptions);
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
            throw new RequestFailedException($"Failure returned when deleting index with name {indexName}", ex);
        }
    }

    public async Task UploadDocuments(IEnumerable<ApprenticeAzureSearchDocument> documents)
    {
        try
        {
            await _searchClient.MergeOrUploadDocumentsAsync(documents);
        }
        catch (Exception ex)
        {
            throw new RequestFailedException($"Failure returned when uploading documents to index", ex);
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
            throw new RequestFailedException($"Failure returned when requesting index with name {indexName}", ex);
        }
    }
}
