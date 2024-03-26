namespace BlazorAppMSAuth.Services
{
    public class JobService : IJobService
    {
        private readonly IConfiguration _config;

        public JobService(IConfiguration config)
        {
            _config = config;
        }

        public async Task<IList<string>> GetJobsAsync()
        {
            var jobs = new List<string>();
            jobs.Add("Job 1");
            jobs.Add("Job 2");
            jobs.Add("Job 3");
            jobs.Add("Job 4");
            jobs.Add("Job 5");
            jobs.Add("Job 6");
            await Task.Delay(1000);

            return jobs;
        }
    }
}
