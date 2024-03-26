
namespace BlazorAppMSAuth.Services
{
    public interface IJobService
    {
        Task<IList<string>> GetJobsAsync();
    }
}