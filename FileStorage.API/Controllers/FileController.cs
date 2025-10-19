using FileStorage.Application.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace FileStorage.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FileController : ControllerBase
{
    private readonly IFileService  _fileService;

    public FileController(IFileService fileService)
    {
        _fileService = fileService;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> Upload(IFormFile file, CancellationToken cancellation = default)
    {
        var fileStream = file.OpenReadStream();
        var saveResult = await _fileService.SaveFile(fileStream, cancellation);

        if (saveResult.IsFailure)
            return BadRequest(new ValidationProblemDetails(saveResult.Errors));

        return Ok(saveResult.Value);
    }

    [HttpGet("download/{id}")]
    public async Task<IActionResult> Download(Guid id, string originalName, string originalExtenstion, CancellationToken cancellation = default)
    {
        var getResult = await _fileService.GetFile(id, cancellation);

        if (getResult.IsFailure)
            return BadRequest(new ValidationProblemDetails(getResult.Errors));

        var originalFilename = $"{originalName}{originalExtenstion}";

        return File(getResult.Value, "application/octet-stream", originalFilename);
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellation = default)
    {
        var deleteResult = await _fileService.DeleteFile(id, cancellation);
        if (deleteResult.IsFailure)
            return BadRequest(new ValidationProblemDetails(deleteResult.Errors));

        return Ok();
    }
}