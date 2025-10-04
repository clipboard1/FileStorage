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

    [HttpGet("getall")]
    public async Task<IActionResult> GetAll(CancellationToken cancellation = default)
    {
        var getResult = await _fileService.GetAllFiles(cancellation);

        return Ok(getResult);
    }

    [HttpPost("upload")]
    public async Task<IActionResult> Upload(IFormFile file, CancellationToken cancellation = default)
    {
        var fileStream = file.OpenReadStream();
        var saveResult = await _fileService.SaveFile(fileStream, file.FileName, cancellation);

        if (saveResult != Guid.Empty)
            return Ok(saveResult);

        return BadRequest("Something went wrong");
    }

    [HttpGet("download/{id}")]
    public async Task<IActionResult> Download(string id, CancellationToken cancellation = default)
    {
        var fileResult = await _fileService.GetFile(id, cancellation);
        if (fileResult?.Stream.Length > 0)
        {
            return File(fileResult.Stream, "application/octet-stream", fileResult.FileName);
        }

        return NotFound();
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(string id, CancellationToken cancellation = default)
    {
        var deleteResult = await _fileService.DeleteFile(id, cancellation);
        if (deleteResult)
            return Ok();

        return BadRequest("Something went wrong");
    }
}