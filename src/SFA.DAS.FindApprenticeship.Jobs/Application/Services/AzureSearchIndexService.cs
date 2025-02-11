using Azure;
using Azure.Search.Documents.Indexes.Models;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Documents;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;

namespace SFA.DAS.FindApprenticeship.Jobs.Application.Services;
public class AzureSearchIndexService(IAzureSearchHelper azureSearchHelper)
{
    public async Task CreateIndex(string indexName)
    {
        await azureSearchHelper.CreateIndex(indexName);
    }

    public async Task DeleteIndex(string indexName)
    {
        await azureSearchHelper.DeleteIndex(indexName);
    }

    public async Task UploadDocuments(string indexName, IEnumerable<ApprenticeAzureSearchDocument> documents)
    {
        await azureSearchHelper.UploadDocuments(indexName, documents);
    }

    public async Task<Response<SearchIndex>> GetIndex(string indexName)
    {
        return await azureSearchHelper.GetIndex(indexName);
    }

    public async Task DeleteDocuments(string indexName, IEnumerable<string> ids)
    {
        await azureSearchHelper.DeleteDocuments(indexName, ids);
    }
}
