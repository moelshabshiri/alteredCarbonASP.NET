using AlteredCarbon.Models;
using AlteredCarbon.Database;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace AlteredCarbon.Controllers
{
    [Route("api/kit")]
    [ApiController]
    public class KitController : ControllerBase
    {
        private readonly IMongoCollection<KitValues> _kitvalues;

        public KitController(IDbClient dbClient)
        {
            _kitvalues = dbClient.GetKitValuesCollection();

        }

        [HttpGet]
        public async Task<IActionResult> GetKitValues()
        {
            var kitvalues = await _kitvalues.Find(order=>true).FirstOrDefaultAsync();
            return Ok(new { kit = await _kitvalues.Find(order => true).FirstOrDefaultAsync() });
        }


    }
}
