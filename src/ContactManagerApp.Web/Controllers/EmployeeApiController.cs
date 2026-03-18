using Microsoft.AspNetCore.Mvc;
using ContactManagerApp.Application.Interfaces;
using ContactManagerApp.Shared.Exceptions;
using ContactManagerApp.Application.DTOs;

namespace ContactManagerApp.Web.Controllers.Api;

[ApiController]
[Route("api/employees")]
public class EmployeeApiController : ControllerBase
{
    private readonly IEmployeeService _employeeService;

    public EmployeeApiController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadCsv(IFormFile file)
    {
        if (file == null || file.Length == 0)
            throw new InvalidFileFormatException("File is empty or not provided.");

        if (!file.FileName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
            throw new UnsupportedFileTypeException("Only CSV files are allowed.");

        await using var stream = file.OpenReadStream();
        var processedCount = await _employeeService.ProcessAndSaveAsync(stream);

        return Ok(new { message = "File processed successfully", count = processedCount });
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var employees = await _employeeService.GetAllEmployeesAsync();
        return Ok(employees);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] EmployeeUpdateDto updateDto)
    {
        await _employeeService.UpdateEmployeeAsync(id, updateDto);
        return Ok(new { message = "Record updated successfully" });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _employeeService.DeleteEmployeeAsync(id);

        return NoContent();
    }
}