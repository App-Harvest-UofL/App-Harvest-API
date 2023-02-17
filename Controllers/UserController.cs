using Microsoft.AspNetCore.Mvc;
using System.Net;
using App_Harvest_API.Models;
using App_Harvest_API.Services;
using Newtonsoft.Json;


namespace PortfolioApi.Controllers;

[ApiController]
[Route("/About")]
public class UserController : ControllerBase
{


    private readonly ILogger<UserController> _logger;
    


    public UserController(ILogger<UserController> logger)
    {
        _logger = logger;
    }

    [HttpGet("GetUser/{id}")]
    [ProducesResponseType(typeof(User), (int) HttpStatusCode.OK)]
    [ProducesResponseType((int) HttpStatusCode.Forbidden)]
    [ProducesResponseType((int) HttpStatusCode.InternalServerError)]
    [ProducesResponseType((int) HttpStatusCode.NoContent)]
    public async Task<IActionResult> GetUser(string id)
    {

        User? result = await UserService.getUser(id);

        if(result != null)
            return Ok(JsonConvert.SerializeObject(result));

        return StatusCode(501, "Get Failed");
    }

    [HttpGet("GetAll")]
    [ProducesResponseType(typeof(List<User>), (int) HttpStatusCode.OK)]
    [ProducesResponseType((int) HttpStatusCode.Forbidden)]
    [ProducesResponseType((int) HttpStatusCode.InternalServerError)]
    [ProducesResponseType((int) HttpStatusCode.NoContent)]
    public async Task<IActionResult> GetAll()
    {
        List<User> result = await UserService.getAll();

        if(result != null)
            return Ok(JsonConvert.SerializeObject(result));
        return StatusCode(501, "Get all Failed");
    }

}
