using Cdcn.Enterprise.Library.Application.Contracts;
using Cdcn.Enterprise.Library.Domain.Primitives.Errors;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdcn.Enterprise.Library.Infrastructure.WebApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        protected BaseApiController(IMediator mediator) => Mediator = mediator;

        protected IMediator Mediator { get; }

        /// <summary>
        /// Creates an <see cref="BadRequestObjectResult"/> that produces a <see cref="StatusCodes.Status400BadRequest"/>.
        /// response based on the specified <see cref="Result"/>.
        /// </summary>
        /// <param name="error">The error.</param>
        /// <returns>The created <see cref="BadRequestObjectResult"/> for the response.</returns>
        protected IActionResult BadRequest(Error error) => BadRequest(new ErrorResponse(new[] { error }));

        /// <summary>
        /// Creates an <see cref="OkObjectResult"/> that produces a <see cref="StatusCodes.Status200OK"/>.
        /// </summary>
        /// <returns>The created <see cref="OkObjectResult"/> for the response.</returns>
        /// <returns></returns>
        protected new IActionResult Ok(object value) => base.Ok(value);

        protected IActionResult OkPdf(object value) => OkPdf(value);


        /// <summary>
        /// Creates an <see cref="NotFoundResult"/> that produces a <see cref="StatusCodes.Status404NotFound"/>.
        /// </summary>
        /// <returns>The created <see cref="NotFoundResult"/> for the response.</returns>
        protected new IActionResult NotFound() => base.NotFound();

        protected IActionResult Ok(bool value) => base.Ok(value);
    }
}
