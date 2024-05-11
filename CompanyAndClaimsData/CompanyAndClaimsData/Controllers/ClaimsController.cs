using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Http.HttpResults;
using CompanyAndClaimsData.Models;
using Microsoft.AspNetCore.Mvc;
using CompanyAndClaimsData.Services;
using Newtonsoft.Json;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Security.Claims;

namespace CompanyAndClaimsData.Controllers;

[ApiController]
[Route("[controller]")]
public class ClaimsController : ControllerBase
{
    private readonly IDatabaseService _databaseService;

    public ClaimsController(IDatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    [HttpGet("CompanyClaims/{companyId}")]
    public IActionResult CompanyClaims(int companyId)
    {
        var companyExists = _databaseService.GetCompanyById(companyId);

        if (companyExists == null)
            return NotFound(JsonConvert.SerializeObject("Company does not exist"));

        var data = _databaseService.GetClaimsByCompanyId(companyId);

        if (data?.Any() == false) 
            return NotFound(JsonConvert.SerializeObject("No claim exists for this company"));

        return Ok(JsonConvert.SerializeObject(data));
    }

    [HttpGet("Claim/{claimId}")]
    public IActionResult Claim(string claimId)
    {
        var data = _databaseService.GetClaimByUCR(claimId);

        if (data == null)
            return NotFound(JsonConvert.SerializeObject("Claim does not exist"));

        var claim = new
        {
            Claim = data,
            ClaimAgeInDays = (DateTime.UtcNow - data.ClaimDate).Days
        };

        return Ok(JsonConvert.SerializeObject(claim));
    }

    [HttpPut("UpdateClaim")]
    public IActionResult UpdateClaim(string claimId, [FromBody] Claims claimDto)
    {
        if (claimDto == null || claimDto.UCR != claimId)
            return BadRequest(JsonConvert.SerializeObject("Invalid Request"));

        var data = _databaseService.GetClaimByUCR(claimDto.UCR);

        if (data == null)
            return NotFound(JsonConvert.SerializeObject("Claim does not exist"));

        var result = _databaseService.UpdateDatabase(claimDto);

        if (!result)
            return BadRequest(JsonConvert.SerializeObject("Failed to update the database"));

        return Ok(JsonConvert.SerializeObject("Claim updated successfully"));
    }
}
