using Microsoft.AspNetCore.Mvc;
using WebAPI.Schedulers;



namespace WebAPI
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestSchedulerController(PhotoCleanupJob job) : ControllerBase
    {
        [HttpPost("run-photo-cleanup")]
        public async Task<IActionResult> RunPhotoCleanup()
        {
            await job.Execute(null); // Контекст можно передать null, если не нужен
            return Ok("Photo cleanup job executed manually.");
        }
    }

}