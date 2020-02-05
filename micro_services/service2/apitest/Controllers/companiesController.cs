using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;

using apitest.Interfaces;
using apitest.Exceptions;
using apitest.Models;

namespace apitest.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [EnableCors("AllowAllOrigin")]
    [ApiController]
    public class companiesController : ControllerBase
    {
        ICompaniesInterface _comapniesInterface;

        public companiesController(ICompaniesInterface comapniesInterface)
        {
            _comapniesInterface = comapniesInterface;
        }


        /// <summary>
        /// Обновление спарвочника
        /// </summary>   
        /// <response code="200">если операция успешна</response>   
        /// <response code="500">в случае внутренних ошибок</response>  
        [HttpPut]
        public async Task<ActionResult> UpdateCompanies([FromServices]IResultStatus result)
        {
            try
            {
                await _comapniesInterface.UpdateCompanies();
                result.status = "ok";
                return Ok(result);
            }
            catch
            {
                result.status = "error";
                result.message = "Внутренняя ошибка";
                return StatusCode(500);
            }
        }


        /// <summary>
        /// Список компаний
        /// </summary>   
        /// <response code="500">в случае внутренних ошибок(например проблемы с доступом к серверу БД и т.п.)</response> 

        [HttpGet]
        public async Task<ActionResult> GetCompanyList([FromServices]IResultStatus result)
        {
            try
            {
                result.result = await _comapniesInterface.GetCompanies();
                result.status = "ok";
                return Ok(result);
            }
            catch
            {
                result.status = "error";
                result.message = "Внутренняя ошибка";
                return StatusCode(500);
            }
        }

    }
}
