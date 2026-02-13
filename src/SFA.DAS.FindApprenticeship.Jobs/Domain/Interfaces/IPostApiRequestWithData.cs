namespace SFA.DAS.FindApprenticeship.Jobs.Domain.Interfaces
{
    public interface IPostApiRequestWithData : IPostApiRequestWithData<object>
    {
    }

    public interface IPostApiRequestWithData<TData>
    {
        string PostUrl { get; }
        TData Data { get; set; }
    }
}