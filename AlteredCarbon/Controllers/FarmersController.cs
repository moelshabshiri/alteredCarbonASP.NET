using AlteredCarbon.Models;
using AlteredCarbon.Database;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using MongoDB.Driver;

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
        public IActionResult LogIn([FromBody] Farmer farmer)
        {
            var farmerObj = _farmers.Find(f => f.email == farmer.email && f.password == farmer.password).FirstOrDefault();
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
        public IActionResult SignUp([FromBody] Farmer farmer)
        {
            var farmerObjE = _farmers.Find(f => f.email == farmer.email).FirstOrDefault();
            var farmerObjPN = _farmers.Find(f => f.email == farmer.email).FirstOrDefault();
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
                _farmers.InsertOne(farmer);
                return Ok(new { user = farmer });
            }
            else
            {
                return UnprocessableEntity(new { message = "Email or Phone number already exist already, please try again." });
            }

        }



        [HttpPost("createorder/{email}")]
        public IActionResult CreateOrder([FromRoute] string email, [FromBody] KitOrder kitOrder)
        {

            var farmerObj = _farmers.Find(f => f.email == email).FirstOrDefault();
            if (farmerObj == null)
            {
                return NotFound(new { message = "Creating order failed, user does not exist" });
            }
            else
            {
                kitOrder.datetimeOfOrder = DateTime.Now;

                kitOrder.farmer = farmerObj.id;

                _kitOrders.InsertOne(kitOrder);
                farmerObj.kitOrders.Add(kitOrder.id);
                _farmers.FindOneAndReplace(f => f.email == email, farmerObj);

                return Ok(new { createdOrder = kitOrder });
            }

        }


        [HttpPost("sellagrwaste/{email}")]
        public IActionResult SellAgrWaste([FromRoute] string email, [FromBody] SellAgrWasteOrder order)
        {
            var farmerObj = _farmers.Find(f => f.email == email).FirstOrDefault();
            if (farmerObj == null)
            {
                return NotFound(new { message = "Creating order failed, user does not exist" });
            }
            else
            {
                order.datetimeOfOrder = DateTime.Now;
                order.farmer = farmerObj.id;
                _sellOrders.InsertOne(order);
                farmerObj.sellAgrWasteOrders.Add(order.id);
                _farmers.FindOneAndReplace(f => f.email == email, farmerObj);
                return Ok(new { createdOrder = order });
            }

        }


        [HttpGet("kitorders/{email}")]
        public IActionResult GetKitOrders([FromRoute] string email)
        {

            var farmerObj = _farmers.Find(f => f.email == email).FirstOrDefault();
            if (farmerObj == null)
            {
                return NotFound(new { message = "Getting orders failed, user does not exist" });
            }
            else
            {
                var kitOrders = _kitOrders.Find(k => k.farmer == farmerObj.id).ToList();
                return Ok(new { kitOrders = kitOrders });
            }

        }

        [HttpGet("sellagrwasteorders/{email}")]
        public IActionResult GetSellAgrWasteOrders([FromRoute] string email)
        {

            var farmerObj = _farmers.Find(f => f.email == email).FirstOrDefault();
            if (farmerObj == null)
            {
                return NotFound(new { message = "Getting orders failed, user does not exist" });
            }
            else
            {
                var sellAgrWasteOrders = _sellOrders.Find(k => k.farmer == farmerObj.id).ToList();
                return Ok(new { orders = sellAgrWasteOrders });
            }

        }


        [HttpGet("points/{email}")]
        public IActionResult GetNumberOfPoints([FromRoute] string email)
        {

            var farmerObj = _farmers.Find(f => f.email == email).FirstOrDefault();
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

