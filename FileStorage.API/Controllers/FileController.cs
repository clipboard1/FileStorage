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

    [Interaction]
    [HttpPost("upload")]
    public async Task<IActionResult> Upload(IFormFile file, CancellationToken cancellation = default)
    {
       var fileStream = file.OpenReadStream();
        var saveResult = await _fileService.SaveFile(fileStream, cancellation);

        if (saveResult.IsFailure)
            return BadRequest(new ValidationProblemDetails(saveResult.Errors));

        return Ok(saveResult.Value);
    }

    [Interaction]
    [HttpGet("download/{id}")]
    public async Task<IActionResult> Download(Guid id, string originalName, string originalExtenstion, CancellationToken cancellation = default)
    {
        var getResult = await _fileService.GetFile(id, cancellation);

        if (getResult.IsFailure)
        {
            if (getResult.Errors.Any(d => d.Value.Any(v => v.Contains("not found", StringComparison.OrdinalIgnoreCase))))
            {
                return NotFound(CreateProblemDetails(getResult.Errors));
            }

            return BadRequest(new ValidationProblemDetails(getResult.Errors));
        }

        var originalFilename = $"{originalName}{originalExtenstion}";

        return File(getResult.Value, "application/octet-stream", originalFilename);
    }

    [Interaction]
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellation = default)
    {
        var deleteResult = await _fileService.DeleteFile(id, cancellation);
        if (deleteResult.IsFailure)
        {
            if (deleteResult.Errors.Any(d => d.Value.Any(v => v.Contains("not found", StringComparison.OrdinalIgnoreCase))))
            {
                return NotFound(CreateProblemDetails(deleteResult.Errors));
            }

            return BadRequest(CreateProblemDetails(deleteResult.Errors));
        }

        return Ok();
    }

    private static ProblemDetails CreateProblemDetails(Dictionary<string, string[]> errors)
    {
        return new ProblemDetails
        {
            Title = "Operation failed",
            Detail = "One or more errors occurred",
            Extensions = { ["errors"] = errors }
        };
    }
}