using CompanyAndClaimsData.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CompanyAndClaimsData.Controllers;

[ApiController]
[Route("[controller]")]
public class CompanyController : ControllerBase
{
    private readonly IDatabaseService _databaseService;

    public CompanyController(IDatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    [HttpGet("Company/{id}")]
    public IActionResult Company(int id)
    {
        var data = _databaseService.GetCompanyById(id).Result;

        if (data == null)
            return NotFound(JsonConvert.SerializeObject("Company does not exist"));

        var company = new
        {
            Company = data,
            ActiveInsurance = data.InsuranceEndDate < DateTime.UtcNow ? false: true
        };

        return Ok(JsonConvert.SerializeObject(company));
    }
}
