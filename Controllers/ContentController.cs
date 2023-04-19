
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.IO;
using App_Harvest_API.Models;
using App_Harvest_API.Repo;
using App_Harvest_API.Services;
using Newtonsoft.Json;
using MongoDB.Driver;
using MongoDB.Bson;

namespace App_Harvest_API.Controllers;

[ApiController]
[Route("/FileUpload")]
public class ContentController : ControllerBase
{

    private readonly ILogger<AuthController> _logger;
    


    public ContentController(ILogger<AuthController> logger)
    {
        _logger = logger;
    }

    [HttpPost("/{file}")]
    [ProducesResponseType(typeof(IFormFile), (int) HttpStatusCode.OK)]
    [ProducesResponseType((int) HttpStatusCode.Forbidden)]
    [ProducesResponseType((int) HttpStatusCode.InternalServerError)]
    [ProducesResponseType((int) HttpStatusCode.NoContent)]
    public async Task<Object> FileUpload( IFormFile file)
    {
        MongoHelper db = new();
        db.CreateBucket();
        await db.UploadAsync(file);
        return true;
    }

    [HttpGet("GetFile/{name}")]
    [ProducesResponseType(typeof(User), (int) HttpStatusCode.OK)]
    [ProducesResponseType((int) HttpStatusCode.Forbidden)]
    [ProducesResponseType((int) HttpStatusCode.InternalServerError)]
    [ProducesResponseType((int) HttpStatusCode.NoContent)]
    public async Task<IActionResult> GetFile(string name)
    {

        MongoHelper db = new();
        var result = await db.GetFileByNameAsync(name);
        byte[] byteArray = result;
        string filePath = @"outputfiles"; // path where you want to save the file
        System.IO.File.WriteAllBytes(filePath, byteArray);
        if (result != null)
            return Ok(result);
        return StatusCode(404, "File not retrieved");
    }

}

