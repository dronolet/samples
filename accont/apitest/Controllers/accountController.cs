using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using apitest.Interfaces;
using apitest.Exceptions;
using apitest.Models;

namespace apitest.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class accountController : ControllerBase
    {
        IAccountInterface _accountInterface;

        public accountController(IAccountInterface accountInterface)
        {

            _accountInterface = accountInterface;

        }

        /// <summary>
        /// Получения полного списка операций по счет
        /// </summary>   
        /// <param name="account_id">Идентификатор счета</param>
        /// <response code="200">если операция успешна или произошла ошибка в бизнес-логике(например баланс может стать отрицательным, тогда status =”error”)</response>   
        /// <response code="400">если параметры не верны(например отрицательный параметр amount)</response>   
        /// <response code="404">если сущность не найдена</response>     
        /// <response code="500">в случае внутренних ошибок(например проблемы с доступом к серверу БД и т.п.)</response> 

        [HttpGet("{account_id}/history")]
        public async Task<ActionResult> GetAcountHistory(int account_id, [FromServices]IResultStatus result)
        {
            try
            {
                result.result =  await _accountInterface.GetAcountHistory(account_id);
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
        /// Внесение денег на счет
        /// </summary>   
        /// <param name="account_id">Идентификатор счета</param>
        /// <param name="amount">Сумма</param>
        /// <response code="200">если операция успешна или произошла ошибка в бизнес-логике(например баланс может стать отрицательным, тогда status =”error”)</response>   
        /// <response code="400">если параметры не верны(например отрицательный параметр amount)</response>   
        /// <response code="404">если сущность не найдена</response>     
        /// <response code="500">в случае внутренних ошибок(например проблемы с доступом к серверу БД и т.п.)</response>  

        [HttpPost("{account_id}/top-up/{amount}")]
        public async Task<ActionResult> TopUpAcount(int account_id, decimal amount, [FromServices]IResultStatus result)
        {
            try
            {
                await _accountInterface.TopUpAcount(account_id, amount); 
                result.status = "ok";
                return Ok(result);
            }
            catch (NotFindException ex)
            {
                result.status = "error";
                result.message = ex.Message;
                return NotFound(result);
            }
            catch (BadLogicException ex)
            {
                result.status = "error";
                result.message = ex.Message;
                return Ok(result);
            }
            catch (BadParametersException ex)
            {
                result.status = "error";
                result.message = ex.Message;
                return BadRequest(result);
            }
            catch
            {
                result.status = "error";
                result.message = "Внутренняя ошибка";
                return StatusCode(500); 
            }
        }


        /// <summary>
        /// Снятие денег со счета
        /// </summary>   
        /// <param name="account_id">Идентификатор счета</param>
        /// <param name="amount">Сумма</param>
        /// <response code="200">если операция успешна или произошла ошибка в бизнес-логике(например баланс может стать отрицательным, тогда status =”error”)</response>   
        /// <response code="400">если параметры не верны(например отрицательный параметр amount)</response>   
        /// <response code="404">если сущность не найдена</response>     
        /// <response code="500">в случае внутренних ошибок(например проблемы с доступом к серверу БД и т.п.)</response>     

        [HttpPost("{account_id}/withdraw/{amount}")]
        public async Task<ActionResult> WithdrawAcount(int account_id, decimal amount, [FromServices]IResultStatus result)
        {
            try
            {

                await _accountInterface.WithdrawAcount(account_id, amount);
                result.status = "ok";
                return Ok(result);
            }
            catch (BadLogicException ex) {
                result.status = "error";
                result.message = "Отрицательный баланс";
                return Ok(result);
            }
            catch (NotFindException ex)
            {
                result.status = "error";
                result.message = ex.Message;
                return NotFound(result);
            }
            catch (BadParametersException ex)
            {
                result.status = "error";
                result.message = ex.Message;
                return BadRequest(result);
            }
            catch
            {
                result.status = "error";
                result.message = "Внутренняя ошибка";
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Перемещение денег между счетами
        /// </summary>   
        /// <param name="source_account_id">Идентификатор счета</param>
        /// <param name="destination_account_id">Идентификатор счета</param>
        /// <param name="amount">Сумма</param>
        /// <response code="200">если операция успешна или произошла ошибка в бизнес-логике(например баланс может стать отрицательным, тогда status =”error”)</response>   
        /// <response code="400">если параметры не верны(например отрицательный параметр amount)</response>   
        /// <response code="404">если сущность не найдена</response>     
        /// <response code="500">в случае внутренних ошибок(например проблемы с доступом к серверу БД и т.п.)</response>    

        [HttpPost("{source_account_id}/transfer/{destination_account_id}/{amount}")]
        public async Task<ActionResult> TransferAcount(int source_account_id, int destination_account_id, decimal amount, [FromServices]IResultStatus result)
        {
            try
            {
                await _accountInterface.TransferAcount(source_account_id, destination_account_id, amount);
                result.status = "ok";
                return Ok(result);
            }
            catch (NotFindException ex)
            {
                result.status = "error";
                result.message = ex.Message;
                return NotFound(result);
            }
            catch (BadLogicException ex)
            {
                result.status = "error";
                result.message = ex.Message;
                return Ok(result);
            }
            catch (BadParametersException ex)
            {
                result.status = "error";
                result.message = ex.Message;
                return BadRequest(result);
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
