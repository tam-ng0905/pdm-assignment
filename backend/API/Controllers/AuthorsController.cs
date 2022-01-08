namespace API.Controllers;
using Domain;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Authors;
using Microsoft.AspNetCore.Mvc;
using MediatR;

//This is the file contains all the controllers for users' account
[ApiController]
public class AuthorsController : BaseApiController
{
    private readonly IMediator _mediator;

    public AuthorsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<Author>>> GetAuthors(CancellationToken ct)
    {
        //Send back a list of all of the information
        //Use the second param for CancellationToken so that the handler knows and cancels a request when the user cancels it
        return await Mediator.Send(new List.Query(), ct);
    }
    
    
    [HttpGet("{id}")] //title/id 
    public async Task<ActionResult<Author>> GetAuthorById(Guid id)
    {
        return await Mediator.Send(new Details.Query{Id = id});
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateAuthor([FromBody] Author author)
    {
        return Ok(await Mediator.Send(new Create.Command {Author = author}));
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> EditAuthor(Guid id, Author author)
    {
        //assign the id with the newly request activity object before sending it to the handler
        author.Id = id;
        return Ok(await Mediator.Send(new Edit.Command {Author = author}));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeteleAuthor(Guid id)
    {
        return Ok(await Mediator.Send(new Delete.Command{Id = id}));
    }
}