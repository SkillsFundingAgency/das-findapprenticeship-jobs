using System.Collections.Generic;
using System.Threading.Tasks;
using Azure;
using Azure.Search.Documents.Indexes.Models;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Documents;

namespace SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;
public interface IAzureSearchIndexService
{
    Task CreateIndex(string indexName);
    Task<Response<SearchIndex>> GetIndex(string indexName);
    Task DeleteIndex(string indexName);
    Task UploadDocuments(IEnumerable<ApprenticeAzureSearchDocument> documents);
}
