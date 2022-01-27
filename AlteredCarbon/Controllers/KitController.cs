using AlteredCarbon.Models;
using AlteredCarbon.Database;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using MongoDB.Driver;

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
        public IActionResult GetKitValues()
        {
            var kitvalues = _kitvalues.Find(order=>true).FirstOrDefault();
            return Ok(new { kit = _kitvalues.Find(order => true).FirstOrDefault() });
        }


    }
}
