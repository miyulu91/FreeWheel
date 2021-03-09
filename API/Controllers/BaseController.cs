using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseController : ControllerBase
    {
        // Mediator injected into the BaseController so that all the controllers derived from Base will have Mediator
        private IMediator _mediator;

        // ??= means null coalescing assignment operator. means if the value is null it will assign whatever it is the right
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

        // added the error handling for the response
        protected ActionResult HandleResult<T>(Result<T> result)
        {
            if (result == null)
                return NotFound();
            if (result.IsSuccess && result.Value != null)
                return Ok(result.Value);
            if (result.IsSuccess && result.Value == null)
                return NotFound();
            return BadRequest(result.Error);
        }
    }
}
