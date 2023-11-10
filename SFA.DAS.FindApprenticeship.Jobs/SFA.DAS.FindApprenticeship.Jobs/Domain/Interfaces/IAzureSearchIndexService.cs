using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.FindApprenticeship.Jobs.Domain.Documents;

namespace SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces;
public interface IAzureSearchIndexService
{
    Task CreateIndex(string indexName);
    Task DeleteIndex(string indexName);
    Task UploadDocuments(IEnumerable<ApprenticeAzureSearchDocument> documents);
}
