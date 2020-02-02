using System;
using System.IO;
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
    public class fileController : ControllerBase
    {
        IFileService _fileService;

        public fileController(IFileService fileService) {
            _fileService = fileService;
        }



        [Authorize]
        [HttpGet("list/{id}")]
        public async Task<ActionResult> GetOrderDocs(int id)
        {

            try
            {
                return Ok(await _fileService.GetOrderFiles(id));
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
        [HttpGet("{id}")]
        public async Task<ActionResult> GetOrderDoc(int id)
        {
            try
            {
                return await Task.Run(() =>
                {
                    string realname = "";
                    MemoryStream ms = new MemoryStream(System.IO.File.ReadAllBytes(_fileService.GetFileName(id, out realname)));
                    ms.Seek(0, SeekOrigin.Begin);
                    return File(ms, "application/force-download", realname);
                    
                });
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
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteOrderDoc(int id)
        {

            try
            {
                await _fileService.DeleteFile(id);
                return Ok();
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
        [HttpPost("{orderid}")]
        [DisableRequestSizeLimit]
        public async Task<ActionResult> SaveFile(int orderid) 
        {

            try
            {
                await _fileService.AddFile(orderid);
                return Ok();
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
        [HttpPost("sendmail")]
        public async Task<ActionResult> sendMail(SendMailModel model)
        {
            ResponseStatus resp = new ResponseStatus();
            try
            {
                await _fileService.sendMail(model);
                return Ok(resp);
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

    }
}
