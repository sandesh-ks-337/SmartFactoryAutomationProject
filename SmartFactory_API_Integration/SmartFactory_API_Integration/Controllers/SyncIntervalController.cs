using Microsoft.AspNetCore.Mvc;

namespace SmartFactory_API_Integration.Controllers
{
    [ApiController]
    [Route("api/sync-interval")]
    public class SyncIntervalController : ControllerBase
    {
        // Static variable for simplicity; could be stored in a database for production
        private static int syncInterval = 30;

        // GET api/sync-interval
        [HttpGet]
        public IActionResult GetSyncInterval()
        {
            return Ok(syncInterval);
        }

        // POST api/sync-interval
        [HttpPost]
        public IActionResult UpdateSyncInterval([FromBody] int newInterval)
        {
            if (newInterval < 0)
            {
                return BadRequest("Invalid interval");
            }
            syncInterval = newInterval;
            return Ok();
        }
    }
}