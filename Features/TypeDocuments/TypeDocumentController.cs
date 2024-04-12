using OrderPurchase.WebApi.Features.TypeDocuments.Entities;
using OrderPurchase.WebApi.Features.TypeDocuments.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderPurchase.WebApi.Features.TypeDocuments
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class TypeDocumentController : ControllerBase
    {
        private readonly TypeDocumentServices _services;

        public TypeDocumentController(TypeDocumentServices services)
        {
            _services = services;
        }

        [HttpGet]
        public IActionResult GetTypeDocument()
        {
            try
            {
                var result = _services.GetTypeDocument();
                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("GetTypeDocumentById{id}")]
        public IActionResult GetTypeDocumentById(int id)
        {
            try
            {
                var result = _services.GetTypeDocumentById(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Add(TypeDocument request)
        {
            try
            {
                var result = _services.Add(request);
                return Ok(result);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("{userId}")]
        public IActionResult Edit(TypeDocument request, int userId)
        {
            try
            {
                var result = _services.Edit(request, userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var result = _services.Delete(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

    }
}
