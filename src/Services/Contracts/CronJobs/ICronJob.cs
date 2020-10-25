using System.Threading.Tasks;

namespace Services.Contracts.CronJobs
{
    public interface ICronJob
    {
        public Task Work();
    }
}
