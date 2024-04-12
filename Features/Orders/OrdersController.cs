using OrderPurchase.WebApi.Features.Orders.Entitie;
using OrderPurchase.WebApi.Features.Orders.Service;
using OrderPurchase.WebApi.Features.ServiceLayer;
using OrderPurchase.WebApi.Features.ServiceLayer.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderPurchase.WebApi.Features.Orders
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly OrderServices _service;
       

        public OrdersController(OrderServices service)
        {
            _service = service;
        }

        [HttpGet("GetOrders")]
        public IActionResult GetOrder()
        {
            try
            {
                var result = _service.GetOrder();
                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("GetOrderByDate{fro}/{to}")]
        public IActionResult GetOrderByDate(DateTime fro, DateTime to)
        {
            try
            {
                var result = _service.GetOrderByDate(fro, to);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult AddOrder(Order request)
        {
            try
            {
                var result = _service.AddOrder(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
