using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Audit.models;
using Audit.interfaces;
using Audit.services;
using Audit.exceptions;
using Audit.objects;


namespace Audit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class dataController : ControllerBase
    {
        IRegistryService _registryService;

        public dataController(IRegistryService registryService) {
            _registryService = registryService;
        }

        [Authorize]
        [HttpPost("orders")]
        public async Task<ActionResult> GetOrders([FromBody] GetOrdersModel model)
        {
            try
            {
                if (model.dfrom.HasValue)
                    model.dfrom = model.dfrom.Value.ToLocalTime().Date;

                if (model.dto.HasValue)
                    model.dto = model.dto.Value.ToLocalTime().Date;
                return Ok(await _registryService.GetRegistryData(model));
            }
            catch (NotFoundException) {
                return NotFound();
            }
            catch(Exception ex)
            {
                return BadRequest();
            }
        }

       

        

        [Authorize]
        [HttpGet("contragents/{path}")]
        public async Task<ActionResult> GetKontragents(string path)
        {
            try
            {
                return Ok(await _registryService.GetKontragents(path));
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        

        [Authorize]
        [HttpGet("orders/{id}")]
        public async Task<ActionResult> GetOrder(int id)
        {
            try
            {
                return Ok(await _registryService.GetOrder(id));
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [Authorize]
        [HttpPut("orders/{id?}")]
        public async Task<ActionResult> UpdateOrder(int? id, RegistryModel model)
        {
            ResponseStatus resp = new ResponseStatus();

            try
            {
                await _registryService.UpdateOrder(id, model);
                return Ok(resp);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (SendEmailException)
            {
                resp.error = "Ошибка отпарввки по E-mail";
                return Ok(resp);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }


        [HttpGet("objects")]
        [Authorize]
        public async Task<ActionResult> GetBObjects()
        {
            try
            {
                return Ok(await _registryService.GetBObjects());
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpDelete("orders/{id}")]
        [Authorize]
        public async Task<ActionResult> DeleteOrder(int id)
        {
            try
            {
                await _registryService.DeleteOrder(id);
                
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpGet("heads")]
        [Authorize]
        public async Task<ActionResult> GetHeads()
        {
            try
            {
                return Ok(_registryService.GetHeadStroyUsers());
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }




    }
}
