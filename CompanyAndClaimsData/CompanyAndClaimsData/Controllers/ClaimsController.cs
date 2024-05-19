using CompanyAndClaimsData.Models;
using CompanyAndClaimsData.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
        var companyExists = _databaseService.GetCompanyById(companyId).Result;

        if (companyExists == null)
            return NotFound(JsonConvert.SerializeObject("Company does not exist"));

        var data = _databaseService.GetClaimsByCompanyId(companyId).Result;

        if (data?.Any() == false) 
            return NotFound(JsonConvert.SerializeObject("No claim exists for this company"));

        return Ok(JsonConvert.SerializeObject(data));
    }

    [HttpGet("Claim/{claimId}")]
    public IActionResult Claim(string claimId)
    {
        var data = _databaseService.GetClaimByUCR(claimId).Result;

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

        var data = _databaseService.GetClaimByUCR(claimDto.UCR).Result;

        if (data == null)
            return NotFound(JsonConvert.SerializeObject("Claim does not exist"));

        var result = _databaseService.UpdateClaimDatabase(claimDto).Result;

        if (!result)
            return BadRequest(JsonConvert.SerializeObject("Failed to update the database"));

        return Ok(JsonConvert.SerializeObject("Claim updated successfully"));
    }
}
