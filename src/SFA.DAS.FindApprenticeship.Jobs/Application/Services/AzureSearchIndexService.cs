using System.Collections.Generic;
using System.Threading.Tasks;
using Azure;
using Azure.Search.Documents.Indexes.Models;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Documents;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;

namespace SFA.DAS.FindApprenticeship.Jobs.Application.Services;
public class AzureSearchIndexService
{
    private readonly IAzureSearchHelper _azureSearchHelper;
    public AzureSearchIndexService(IAzureSearchHelper azureSearchHelper)
    {
        _azureSearchHelper = azureSearchHelper;
    }

    public async Task CreateIndex(string indexName)
    {
        await _azureSearchHelper.CreateIndex(indexName);
    }

    public async Task DeleteIndex(string indexName)
    {
        await _azureSearchHelper.DeleteIndex(indexName);
    }

    public async Task UploadDocuments(string indexName, IEnumerable<ApprenticeAzureSearchDocument> documents)
    {
        await _azureSearchHelper.UploadDocuments(indexName,documents);
    }

    public async Task<Response<SearchIndex>> GetIndex(string indexName)
    {
        return await _azureSearchHelper.GetIndex(indexName);
    }
}
