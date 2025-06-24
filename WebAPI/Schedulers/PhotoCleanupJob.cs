using BSExpPhotos.Interfaces;


namespace WebAPI.Schedulers;

using Quartz;
using System.Threading.Tasks;

public class PhotoCleanupJob(IImageCleanupService cleanupService) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        
        await cleanupService.CleanUpOutdatedPhotosAsync();
    }
}