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
    public class companyController : ControllerBase
    {
        ICompanyInterface _companyInterface;

        public companyController(ICompanyInterface accountInterface)
        {

            _companyInterface = accountInterface;

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
                result.result = await _companyInterface.GetCompanies();
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
        /// Добавление компании
        /// </summary>   

        /// <response code="200">если операция успешна или произошла ошибка в бизнес-логике(например баланс может стать отрицательным, тогда status =”error”)</response>   
        /// <response code="404">если сущность не найдена</response>     
        /// <response code="500">в случае внутренних ошибок</response>  

        [HttpPost]
        public async Task<ActionResult> AddCompany(CompanyDataModel model, [FromServices]IResultStatus result)
        {
            try
            {
                await _companyInterface.UpdateCompany(null, model);
                result.status = "ok";
                return Ok(result);
            }
            catch (NotFindException ex)
            {
                result.status = "error";
                result.message = ex.Message;
                return NotFound(result);
            }
            catch
            {
                result.status = "error";
                result.message = "Внутренняя ошибка";
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Изменение компании
        /// </summary>   

        /// <response code="200">если операция успешна или произошла ошибка в бизнес-логике(например баланс может стать отрицательным, тогда status =”error”)</response>   
        /// <response code="404">если сущность не найдена</response>     
        /// <response code="500">в случае внутренних ошибок</response>  

        [HttpPut]
        public async Task<ActionResult> UpdateCompany(UpadateCompanyDataModel model, [FromServices]IResultStatus result)
        {
            try
            {
                await _companyInterface.UpdateCompany(model.Id, (CompanyDataModel)model);
                result.status = "ok";
                return Ok(result);
            }
            catch (NotFindException ex)
            {
                result.status = "error";
                result.message = ex.Message;
                return NotFound(result);
            }
            catch
            {
                result.status = "error";
                result.message = "Внутренняя ошибка";
                return StatusCode(500);
            }
        }


        /// <summary>
        /// Удаление компании
        /// </summary>   

        /// <response code="200">если операция успешна или произошла ошибка в бизнес-логике(например баланс может стать отрицательным, тогда status =”error”)</response>   
        /// <response code="404">если сущность не найдена</response>     
        /// <response code="500">в случае внутренних ошибок</response>  

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCompany(int id, [FromServices]IResultStatus result)
        {
            try
            {
                await _companyInterface.DeleteCompany(id);
                result.status = "ok";
                return Ok(result);
            }
            catch (NotFindException ex)
            {
                result.status = "error";
                result.message = ex.Message;
                return NotFound(result);
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
