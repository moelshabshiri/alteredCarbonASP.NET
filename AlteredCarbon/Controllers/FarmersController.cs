using AlteredCarbon.Models;
using AlteredCarbon.Database;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace AlteredCarbon.Controllers
{
    [Route("api/farmer")]
    [ApiController]
    public class FarmersController : ControllerBase
    {
        private readonly IMongoCollection<Farmer> _farmers;
        private readonly IMongoCollection<KitOrder> _kitOrders;
        private readonly IMongoCollection<SellAgrWasteOrder> _sellOrders;
        public FarmersController( IDbClient dbClient)
        {
            _farmers = dbClient.GetFarmersCollection();
            _kitOrders = dbClient.GetKitOrderCollection();
            _sellOrders = dbClient.GetSellOrderCollection();
        }


        [HttpPost("login")]
        public async Task<IActionResult> LogIn([FromBody] Farmer farmer)
        {
            Console.WriteLine("HELL");
            var farmerObj =  await _farmers.Find(f => f.email == farmer.email && f.password == farmer.password).FirstOrDefaultAsync();
            Console.WriteLine(farmerObj.email);
            if (farmerObj == null)
            {

                return NotFound(new { message = "Invalid credentials, could not log you in" });
            }
            else
            {
                return Ok(new { user = farmerObj });
            }

        }


        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] Farmer farmer)
        {
            var farmerObjE = await _farmers.Find(f => f.email == farmer.email).FirstOrDefaultAsync();
            var farmerObjPN = await _farmers.Find(f => f.email == farmer.email).FirstOrDefaultAsync();
            if (farmerObjE == null && farmerObjPN == null)
            {
                if (farmer.name == null)
                {
                    return BadRequest(new { message = "Name field is required" });
                }

                if (farmer.email == null)
                {
                    return BadRequest(new { message = "Email field is required" });
                }

                if (farmer.phoneNumber == null)
                {
                    return BadRequest(new { message = "Phone Number field is required" });
                }

                if (farmer.password == null)
                {
                    return BadRequest(new { message = "Password field is required" });
                }

                farmer.accountType = "farmer";
                farmer.points = 500;
                await _farmers.InsertOneAsync(farmer);
                return Ok(new { user = farmer });
            }
            else
            {
                return UnprocessableEntity(new { message = "Email or Phone number already exist already, please try again." });
            }

        }



        [HttpPost("createorder/{email}")]
        public async Task<IActionResult> CreateOrder([FromRoute] string email, [FromBody] KitOrder kitOrder)
        {

            var farmerObj = await _farmers.Find(f => f.email == email).FirstOrDefaultAsync();
            if (farmerObj == null)
            {
                return NotFound(new { message = "Creating order failed, user does not exist" });
            }
            else
            {
                kitOrder.datetimeOfOrder = DateTime.Now;

                kitOrder.farmer = farmerObj.id;

               await _kitOrders.InsertOneAsync(kitOrder);
                farmerObj.kitOrders.Add(kitOrder.id);
                await _farmers.FindOneAndReplaceAsync(f => f.email == email, farmerObj);

                return Ok(new { createdOrder = kitOrder });
            }

        }


        [HttpPost("sellagrwaste/{email}")]
        public async Task<IActionResult> SellAgrWaste([FromRoute] string email, [FromBody] SellAgrWasteOrder order)
        {
            var farmerObj = await _farmers.Find(f => f.email == email).FirstOrDefaultAsync();
            if (farmerObj == null)
            {
                return NotFound(new { message = "Creating order failed, user does not exist" });
            }
            else
            {
                order.datetimeOfOrder = DateTime.Now;
                order.farmer = farmerObj.id;
               await _sellOrders.InsertOneAsync(order);
                farmerObj.sellAgrWasteOrders.Add(order.id);
               await _farmers.FindOneAndReplaceAsync(f => f.email == email, farmerObj);
                return Ok(new { createdOrder = order });
            }

        }


        [HttpGet("kitorders/{email}")]
        public async Task<IActionResult> GetKitOrders([FromRoute] string email)
        {

            var farmerObj = await _farmers.Find(f => f.email == email).FirstOrDefaultAsync();
            if (farmerObj == null)
            {
                return NotFound(new { message = "Getting orders failed, user does not exist" });
            }
            else
            {
                var kitOrders = await _kitOrders.Find(k => k.farmer == farmerObj.id).ToListAsync();
                return Ok(new { kitOrders = kitOrders });
            }

        }

        [HttpGet("sellagrwasteorders/{email}")]
        public async Task<IActionResult> GetSellAgrWasteOrders([FromRoute] string email)
        {

            var farmerObj = await _farmers.Find(f => f.email == email).FirstOrDefaultAsync();
            if (farmerObj == null)
            {
                return NotFound(new { message = "Getting orders failed, user does not exist" });
            }
            else
            {
                var sellAgrWasteOrders = await _sellOrders.Find(k => k.farmer == farmerObj.id).ToListAsync();
                return Ok(new { orders = sellAgrWasteOrders });
            }

        }


        [HttpGet("points/{email}")]
        public async Task<IActionResult> GetNumberOfPoints([FromRoute] string email)
        {

            var farmerObj = await _farmers.Find(f => f.email == email).FirstOrDefaultAsync();
            if (farmerObj == null)
            {
                return NotFound(new { message = "Creating order failed, user does not exist" });
            }
            else
            {
                return Ok(new { points = farmerObj.points });
            }

        }


    }
}

