using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Audit.models;
using Audit.interfaces;
using Audit.services;
using Audit.exceptions;


namespace Audit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class reportsController : ControllerBase
    {
        IReportsService _reportService;
        IRegistryService _registryService;

        public reportsController(IReportsService reportService,  IRegistryService registryService) {
            _reportService = reportService;
            _registryService = registryService;
        }

        [Authorize]
        [HttpPost("orders")]
        public async Task<ActionResult> GetOrders([FromBody] GetOrdersModel model)
        {
            try
            {
                if (model.dfrom.HasValue)
                    model.dfrom = model.dfrom.Value.Date;

                if (model.dto.HasValue)
                    model.dto = model.dto.Value.Date;
                
                var doc_stream = await _reportService.OrdersReport(model, await _registryService.GetRegistryData(model));
                doc_stream.Seek(0, SeekOrigin.Begin);
                return File(doc_stream, "application/vnd.ms-excel", "ЗамечанияОНОР.xlsx");
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
        [HttpPost("preport")]
        public async Task<ActionResult> PeriodReport([FromBody]PeriodReportTypeModel model)
        {
            try
            {
                var doc_stream = await _reportService.PeriodReport(model);
                doc_stream.Seek(0, SeekOrigin.Begin);
                return File(doc_stream, "application/vnd.ms-excel", _reportService.GetReportName(model.ReportId) + ".xlsx");
            }           
            catch (Exception ex)
            {
                return BadRequest();
            }
        }


        [HttpPost("emploee")]
        [Authorize]
        public async Task<ActionResult> EmploeeReport(PeriodReportEmploeeModel model)
        {
            try
            {
                var doc_stream = await _reportService.EmploeeReport(model);
                doc_stream.Seek(0, SeekOrigin.Begin);
                return File(doc_stream, "application/vnd.ms-excel", _reportService.GetReportName(3) + ".xlsx");
            }
            catch
            {
                return BadRequest();
            }
        }


    }
}
