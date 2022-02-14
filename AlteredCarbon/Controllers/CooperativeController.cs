using AlteredCarbon.Models;
using AlteredCarbon.Database;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace AlteredCarbon.Controllers
{
    [Route("api/cooperative")]
    [ApiController]
    public class CooperativeController : ControllerBase
    {
        private readonly IMongoCollection<Farmer> _farmers;
        private readonly IMongoCollection<Cooperative> _cooperatives;
        private readonly IMongoCollection<KitOrder> _kitOrders;
        private readonly IMongoCollection<SellAgrWasteOrder> _sellOrders;
        public CooperativeController(IDbClient dbClient)
        {
            _farmers = dbClient.GetFarmersCollection();
            _cooperatives = dbClient.GetCooperativesCollection();
            _kitOrders = dbClient.GetKitOrderCollection();
            _sellOrders = dbClient.GetSellOrderCollection();
        }


        [HttpPost("login")]
        public async Task<IActionResult> LogIn([FromBody] Cooperative cooperative)
        {
            var userObj = await _cooperatives.Find(c => c.email == cooperative.email && c.password == cooperative.password).FirstOrDefaultAsync();
            if (userObj == null)
            {
                return NotFound(new { message = "Invalid credentials, could not log you in" });
            }
            else
            {
                return Ok(new { user = userObj });
            }

        }



        [HttpPut("acceptkitorder/{email}")]
        public async Task<IActionResult> AcceptKitOrder([FromRoute] string email, [FromBody] KitOrder Order)
        {

            string orderId = Order.id;
            var userObj = await _cooperatives.Find(c => c.email == email).FirstOrDefaultAsync();
            if (userObj == null)
            {
                return NotFound(new { message = "Accepting order failed, user does not exist" });
            }

            var kitOrder = await _kitOrders.Find(i => i.id == orderId).FirstOrDefaultAsync();
            if (kitOrder == null)
            {
                return NotFound(new { message = "Accepting order failed, order does not exist" });
            }

            var existingFarmerUser =await _farmers.Find(i => i.id == kitOrder.farmer).FirstOrDefaultAsync();
            if (existingFarmerUser == null)
            {
                return NotFound(new { message = "Accepting order failed, user does not exist" });
            }


            kitOrder.datetimeOfApproval = DateTime.Now;
                kitOrder.status = "approved";
                kitOrder.approvedBy = userObj.id;
                existingFarmerUser.points -= kitOrder.orderPoints;

                await _kitOrders.FindOneAndReplaceAsync(i => i.id == orderId, kitOrder);
                await _farmers.FindOneAndReplaceAsync(i => i.email == email, existingFarmerUser);

                return Ok(new { kitOrder = kitOrder });
        }







        [HttpPut("acceptsaworder/{email}")]
        public async Task<IActionResult> AcceptSellAgrWasteOrder([FromRoute] string email, [FromBody] SellAgrWasteOrder Order)
        {
            string orderId = Order.id;
            var userObj = await _cooperatives.Find(c => c.email == email).FirstOrDefaultAsync();
            if (userObj == null)
            {
                return NotFound(new { message = "Accepting order failed, user does not exist" });
            }

            var order = await _sellOrders.Find(i => i.id == orderId).FirstOrDefaultAsync();
            if (order == null)
            {
                return NotFound(new { message = "Accepting order failed, order does not exist" });
            }

            var existingFarmerUser = await _farmers.Find(i => i.id == order.farmer).FirstOrDefaultAsync();
            if (existingFarmerUser == null)
            {
                return NotFound(new { message = "Accepting order failed, user does not exist" });
            }

            order.datetimeOfApproval = DateTime.Now;
            order.status = "approved";
            order.approvedBy = userObj.id;
            existingFarmerUser.points -= order.orderPoints;

            await _sellOrders.FindOneAndReplaceAsync(i => i.id == orderId, order);
            await _farmers.FindOneAndReplaceAsync(i => i.email == email, existingFarmerUser);

            return Ok(new { order = order });
        }






        [HttpGet("kitorders/{email}")]
        public async Task<IActionResult> GetPendingKitOrders([FromRoute] string email)
        {

            var userObj = await _cooperatives.Find(c => c.email == email).FirstOrDefaultAsync();
            if (userObj == null)
            {
                return NotFound(new { message = "Creating order failed, user does not exist" });
            }

            var kitOrders = await _kitOrders.Find(o=>o.status=="pending").ToListAsync();

            return Ok(new { kitOrders = kitOrders });

        }


        [HttpGet("sellagrwasteorders/{email}")]
        public async Task<IActionResult> GetSellAgrWasteOrders([FromRoute] string email)
        {

            var userObj = await _cooperatives.Find(c => c.email == email).FirstOrDefaultAsync();
            if (userObj == null)
            {
                return NotFound(new { message = "Creating order failed, user does not exist" });
            }

            var orders = await _sellOrders.Find(o => o.status == "pending").ToListAsync();

            return Ok(new { orders = orders });

        }







    }
}

