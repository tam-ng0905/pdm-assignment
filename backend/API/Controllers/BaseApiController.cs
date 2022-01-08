using API.Extensions;
using Application.Core;
using MediatR;
using Microsoft.AspNetCore.Mvc;

//This is the base class for all api controllers

namespace API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class BaseApiController : ControllerBase
{
    private IMediator _mediator;
    protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
    
    //Handling the result for regular response, without any pagination
    protected ActionResult HandleResult<T>(Result<T> result)
    {
        if (result == null) return NotFound();
        if (result.IsSuccess && result.Value != null)
            return Ok(result.Value);
        if (result.IsSuccess && result.Value == null)
            return NotFound();
        return BadRequest(result.Error);
    }
    
    
    
    //Handling the result for pagination 
    protected ActionResult HandlePagedResult<T>(Result<PagedList<T>> result)
    {
        if (result == null) return NotFound();
        if (result.IsSuccess && result.Value != null)
        {
            //Set up the result in the page format for pagination
            Response.AddPaginationHeader(result.Value.CurrentPage, result.Value.PageSize, 
                result.Value.TotalCount, result.Value.TotalPages);
            return Ok(result.Value);
        }
        if (result.IsSuccess && result.Value == null)
            return NotFound();
        return BadRequest(result.Error);
    }
}