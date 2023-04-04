
using Microsoft.AspNetCore.Mvc;
using System.Net;
using App_Harvest_API.Models;
using App_Harvest_API.Services;
using Newtonsoft.Json;
using MongoDB.Driver;
using MongoDB.Bson;

namespace App_Harvest_API.Controllers;

[ApiController]
[Route("/login")]
public class AuthController : ControllerBase
{

    private readonly ILogger<AuthController> _logger;
    


    public AuthController(ILogger<AuthController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    [ProducesResponseType(typeof(Login), (int) HttpStatusCode.OK)]
    [ProducesResponseType((int) HttpStatusCode.Forbidden)]
    [ProducesResponseType((int) HttpStatusCode.InternalServerError)]
    [ProducesResponseType((int) HttpStatusCode.NoContent)]
    public async Task<bool> Login([FromBody] Login user)
    {
        User? checkUser = null;
        if ( user.email != null)
        {
           checkUser = await UserService.getUser(user.email);
        }
        if (checkUser?.password == user.password && checkUser?.email == user.email)
        {
            return true;
        }
        return false; 
    }
}

