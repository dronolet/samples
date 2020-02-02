using System;
using System.Collections.Generic;
using System.Linq;
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
    public class authController : ControllerBase
    {
        IAuthService _authService;

        public authController(IAuthService authService) {
            _authService = authService;
        }

        [HttpPost("sign")]
        public async Task<ActionResult> Sign(LoginModel model)
        {
            try
            {
                await _authService.Sign(model.Login, model.Password);
                return Ok();
            }
            catch (NotFoundException) {
                return NotFound();
            }
            catch(Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPost("remember")]
        public async Task<ActionResult> remember(EmailModel model)
        {
            try
            {
                await _authService.Remember(model.Email);
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

        [HttpGet("rememberall")]
        public async Task<ActionResult> rememberall()
        {
            try
            {
                await _authService.RememberAll();
                return Ok();
            }           
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPost("signout")]
        [Authorize]
        public async Task<ActionResult> SignOut()
        {
            try
            {
                await _authService.SignOut();
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [Authorize]
        [HttpGet("employees")]
        public async Task<ActionResult> GetEmployees()
        {
            try
            {
                return Ok(await _authService.GetEmployeers());
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpGet("info")]
        [Authorize]
        public async Task<ActionResult> Info()
        {
            try
            {
                return Ok(await _authService.GetUserInfo());
            }
            catch
            {
                return BadRequest();
            }
        }

     

        


    }
}
