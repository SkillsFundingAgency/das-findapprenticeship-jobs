using Azure.Search.Documents.Indexes.Models;
using Azure;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Documents;

namespace SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;
public interface IAzureSearchHelper
{
    Task CreateIndex(string indexName);
    Task<Response<SearchIndex>> GetIndex(string indexName);
    Task DeleteIndex(string indexName);
    Task UploadDocuments(string indexName, IEnumerable<ApprenticeAzureSearchDocument> documents);
    Task<List<SearchIndex>> GetIndexes();
    Task<SearchAlias> GetAlias(string aliasName);
    Task UpdateAlias(string aliasName, string indexName);
    Task<Response<ApprenticeAzureSearchDocument>?> GetDocument(string indexName, string vacancyReference);
    Task DeleteDocuments(string indexName, IEnumerable<string> ids);
}
