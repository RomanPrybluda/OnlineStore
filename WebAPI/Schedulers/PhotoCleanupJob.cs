using BSExpPhotos.Services;

namespace WebAPI.Schedulers;

using Quartz;
using System.Threading.Tasks;

public class PhotoCleanupJob(PhotoCleanupService cleanupService) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        await cleanupService.CleanUpOutdatedPhotosAsync();
    }
}