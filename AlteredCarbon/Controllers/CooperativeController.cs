using AlteredCarbon.Models;
using AlteredCarbon.Database;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using MongoDB.Driver;

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
        public IActionResult LogIn([FromBody] Cooperative cooperative)
        {
            var userObj = _cooperatives.Find(c => c.email == cooperative.email && c.password == cooperative.password).FirstOrDefault();
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
        public IActionResult AcceptKitOrder([FromRoute] string email, [FromBody] KitOrder Order)
        {

            string orderId = Order.id;
            var userObj = _cooperatives.Find(c => c.email == email).FirstOrDefault();
            if (userObj == null)
            {
                return NotFound(new { message = "Accepting order failed, user does not exist" });
            }

            var kitOrder = _kitOrders.Find(i => i.id == orderId).FirstOrDefault();
            if (kitOrder == null)
            {
                return NotFound(new { message = "Accepting order failed, order does not exist" });
            }

            var existingFarmerUser = _farmers.Find(i => i.id == kitOrder.farmer).FirstOrDefault();
            if (existingFarmerUser == null)
            {
                return NotFound(new { message = "Accepting order failed, user does not exist" });
            }


            kitOrder.datetimeOfApproval = DateTime.Now;
                kitOrder.status = "approved";
                kitOrder.approvedBy = userObj.id;
                existingFarmerUser.points -= kitOrder.orderPoints;

                _kitOrders.FindOneAndReplace(i => i.id == orderId, kitOrder);
                _farmers.FindOneAndReplace(i => i.email == email, existingFarmerUser);

                return Ok(new { kitOrder = kitOrder });
        }







        [HttpPut("acceptsaworder/{email}")]
        public IActionResult AcceptSellAgrWasteOrder([FromRoute] string email, [FromBody] SellAgrWasteOrder Order)
        {
            string orderId = Order.id;
            var userObj = _cooperatives.Find(c => c.email == email).FirstOrDefault();
            if (userObj == null)
            {
                return NotFound(new { message = "Accepting order failed, user does not exist" });
            }

            var order = _sellOrders.Find(i => i.id == orderId).FirstOrDefault();
            if (order == null)
            {
                return NotFound(new { message = "Accepting order failed, order does not exist" });
            }

            var existingFarmerUser = _farmers.Find(i => i.id == order.farmer).FirstOrDefault();
            if (existingFarmerUser == null)
            {
                return NotFound(new { message = "Accepting order failed, user does not exist" });
            }

            order.datetimeOfApproval = DateTime.Now;
            order.status = "approved";
            order.approvedBy = userObj.id;
            existingFarmerUser.points -= order.orderPoints;

            _sellOrders.FindOneAndReplace(i => i.id == orderId, order);
            _farmers.FindOneAndReplace(i => i.email == email, existingFarmerUser);

            return Ok(new { order = order });
        }






        [HttpGet("kitorders/{email}")]
        public IActionResult GetPendingKitOrders([FromRoute] string email)
        {

            var userObj = _cooperatives.Find(c => c.email == email).FirstOrDefault();
            if (userObj == null)
            {
                return NotFound(new { message = "Creating order failed, user does not exist" });
            }

            var kitOrders = _kitOrders.Find(o=>o.status=="pending").ToList();

            return Ok(new { kitOrders = kitOrders });

        }


        [HttpGet("sellagrwasteorders/{email}")]
        public IActionResult GetSellAgrWasteOrders([FromRoute] string email)
        {

            var userObj = _cooperatives.Find(c => c.email == email).FirstOrDefault();
            if (userObj == null)
            {
                return NotFound(new { message = "Creating order failed, user does not exist" });
            }

            var orders = _sellOrders.Find(o => o.status == "pending").ToList();

            return Ok(new { orders = orders });

        }







    }
}

