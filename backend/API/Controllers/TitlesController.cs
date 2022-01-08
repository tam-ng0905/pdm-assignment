using System.Text.Json;
using Domain;
using Application.Core;
using Application.Titles;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Distributed;
using Seed = Application.Titles.Seed;
using cache = Microsoft.Extensions.Caching.Distributed.IDistributedCache;


//This is the file contains all the controllers for users' account

namespace API.Controllers;
[ApiController]
public class TitlesController: BaseApiController
{
    private readonly IMediator _mediator;

    public TitlesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetTitles([FromQuery] PagingParams param)
    {
        //Send back all the books in per page
        return HandlePagedResult(await Mediator.Send(new List.Query{Params = param}));
    }
    
    [HttpGet("seed")]
    public async Task<ActionResult<List<Title>>> GetSeed([FromQuery] PagingParams param)
    {
        //Send first 50 books to feed the UI initially 
        return HandlePagedResult(await Mediator.Send(new Seed.Query{Params = param}));
    }

    
    [HttpGet("search")]
    public async Task<IActionResult> GetSearch([FromQuery] PagingParams param, string query, int price)
    {
        //Use the query and price from the request to search
        return HandlePagedResult(await Mediator.Send(new Search.Query{ Params = param, query = query, price = price,}));
    }

    
    [HttpGet("{id}")] //title/id 
    public async Task<ActionResult<Title>> GetTitleById(Guid id)
    {
        return HandleResult(await Mediator.Send(new Details.Query{Id = id}));
    }

    
    [HttpPost]
    public async Task<IActionResult> CreateTitle([FromBody] Title title)
    {
        return HandleResult(await Mediator.Send(new Create.Command {Title = title}));
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> EditTitle(Guid id, Title title)
    {
        //assign the id with the newly request book object before sending it to the handler
        title.Id = id;
        return HandleResult(await Mediator.Send(new Edit.Command {Title = title}));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeteleTitle(Guid id)
    {
        return HandleResult(await Mediator.Send(new Delete.Command{Id = id}));
    }
}