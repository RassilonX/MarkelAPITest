using CompanyAndClaimsData.Controllers;
using CompanyAndClaimsData.Models;
using CompanyAndClaimsData.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CompanyAndClaimsDataTests.Controller;

public class ClaimsControllerTests
{
    private readonly Mock<IDatabaseService> _databaseService;
    private readonly ClaimsController _controller;

    public ClaimsControllerTests()
    {
        _databaseService = new Mock<IDatabaseService>();

        _controller = new ClaimsController(_databaseService.Object);
    }

    #region Happy Path Tests
    [Fact]
    public void ClaimsController_CompanyClaims_ReturnsExpectedResult()
    {
        var testData = new Company { Id = 1, InsuranceEndDate = DateTime.UtcNow.AddDays(-5) };
        var claimData = new Claims { CompanyId = 1, ClaimDate = DateTime.UtcNow.AddDays(-27), UCR = "Test1" };
        _databaseService.Setup(s => s.GetCompanyById(testData.Id)).Returns(Task.FromResult(testData));
        _databaseService.Setup(s => s.GetClaimsByCompanyId(testData.Id)).Returns(Task.FromResult(new List<Claims> { claimData }));

        var claimJson = JsonConvert.SerializeObject(new List<Claims> { claimData });

        var result = _controller.CompanyClaims(testData.Id);

        var okResult = result.Should().BeAssignableTo<OkObjectResult>();
        result.Should().NotBeNull();
        result.Should().BeOfType<OkObjectResult>();
        okResult.Should().NotBeNull();
        okResult.Subject.Value.Should().Be(claimJson);
    }

    [Fact]
    public void ClaimsController_Claim_ReturnsExpectedResult()
    {
        var testData = new Claims { CompanyId = 1, ClaimDate = DateTime.UtcNow.AddDays(-27), UCR = "Test1" };
        _databaseService.Setup(s => s.GetClaimByUCR(testData.UCR)).Returns(Task.FromResult(testData));

        var claim = new
        {
            Claim = testData,
            ClaimAgeInDays = (DateTime.UtcNow - testData.ClaimDate).Days
        };

        var claimJson = JsonConvert.SerializeObject(claim);

        var result = _controller.Claim(testData.UCR);

        var okResult = result.Should().BeAssignableTo<OkObjectResult>();
        result.Should().NotBeNull();
        result.Should().BeOfType<OkObjectResult>();
        okResult.Should().NotBeNull();
        okResult.Subject.Value.Should().Be(claimJson);
    }

    [Fact]
    public void ClaimsController_UpdateClaim_ReturnsExpectedResult()
    {
        var testData = new Claims { CompanyId = 1, ClaimDate = DateTime.UtcNow.AddDays(-27), UCR = "Test1" };
        _databaseService.Setup(s => s.GetClaimByUCR(testData.UCR)).Returns(Task.FromResult(testData));
        _databaseService.Setup(s => s.UpdateClaimDatabase(testData)).Returns(Task.FromResult(true));

        var result = _controller.UpdateClaim(testData.UCR, testData);

        var success = JsonConvert.SerializeObject("Claim updated successfully");

        var okResult = result.Should().BeAssignableTo<OkObjectResult>();
        result.Should().NotBeNull();
        result.Should().BeOfType<OkObjectResult>();
        okResult.Should().NotBeNull();
        okResult.Subject.Value.Should().Be(success);
    }
    #endregion


    #region Unhappy Path Tests
    [Fact]
    public void ClaimsController_CompanyClaims_ReturnsCompanyDoesNotExist()
    {
        _databaseService.Setup(s => s.GetCompanyById(It.IsAny<int>()).Result).Equals(null);
        var result = _controller.CompanyClaims(1234);

        var failJson = JsonConvert.SerializeObject("Company does not exist");

        var notFoundResult = result.Should().BeAssignableTo<NotFoundObjectResult>();
        result.Should().NotBeNull();
        result.Should().BeOfType<NotFoundObjectResult>();
        notFoundResult.Should().NotBeNull();
        notFoundResult.Subject.Value.Should().Be(failJson);
    }

    [Fact]
    public void ClaimsController_CompanyClaims_ReturnsClaimDoesNotExist()
    {
        var testData = new Company { Id = 1, InsuranceEndDate = DateTime.UtcNow.AddDays(-5) };
        var claimData = new Claims { CompanyId = 1, ClaimDate = DateTime.UtcNow.AddDays(-27), UCR = "Test1" };
        _databaseService.Setup(s => s.GetCompanyById(testData.Id)).Returns(Task.FromResult(testData));
        _databaseService.Setup(s => s.GetClaimsByCompanyId(testData.Id)).Returns(Task.FromResult(new List<Claims> {}));

        var failJson = JsonConvert.SerializeObject("No claim exists for this company");

        var result = _controller.CompanyClaims(testData.Id);

        var notFoundResult = result.Should().BeAssignableTo<NotFoundObjectResult>();
        result.Should().NotBeNull();
        result.Should().BeOfType<NotFoundObjectResult>();
        notFoundResult.Should().NotBeNull();
        notFoundResult.Subject.Value.Should().Be(failJson);
    }

    [Fact]
    public void ClaimsController_Claim_ReturnsClaimDoesNotExist()
    {
        _databaseService.Setup(s => s.GetClaimByUCR(It.IsAny<string>()).Result).Equals(null);
        var result = _controller.Claim("Wibble");

        var failJson = JsonConvert.SerializeObject("Claim does not exist");

        var notFoundResult = result.Should().BeAssignableTo<NotFoundObjectResult>();
        result.Should().NotBeNull();
        result.Should().BeOfType<NotFoundObjectResult>();
        notFoundResult.Should().NotBeNull();
        notFoundResult.Subject.Value.Should().Be(failJson);
    }

    [Fact]
    public void ClaimsController_UpdateClaim_ReturnsInvalidRequest()
    {
        var claim = new Claims { CompanyId = 1, ClaimDate = DateTime.UtcNow.AddDays(-27), UCR = "Test1" };

        var result = _controller.UpdateClaim("Wibble", claim);

        var failJson = JsonConvert.SerializeObject("Invalid Request");

        var notFoundResult = result.Should().BeAssignableTo<BadRequestObjectResult>();
        result.Should().NotBeNull();
        result.Should().BeOfType<BadRequestObjectResult>();
        notFoundResult.Should().NotBeNull();
        notFoundResult.Subject.Value.Should().Be(failJson);
    }

    [Fact]
    public void ClaimsController_UpdateClaim_WithNullClaim_ReturnsInvalidRequest()
    {
        var result = _controller.UpdateClaim("Wibble", null);

        var failJson = JsonConvert.SerializeObject("Invalid Request");

        var notFoundResult = result.Should().BeAssignableTo<BadRequestObjectResult>();
        result.Should().NotBeNull();
        result.Should().BeOfType<BadRequestObjectResult>();
        notFoundResult.Should().NotBeNull();
        notFoundResult.Subject.Value.Should().Be(failJson);
    }

    [Fact]
    public void ClaimsController_UpdateClaim_ReturnsClaimDoesNotExist()
    {
        var claim = new Claims { CompanyId = 1, ClaimDate = DateTime.UtcNow.AddDays(-27), UCR = "Test1" };
        _databaseService.Setup(s => s.GetClaimByUCR(It.IsAny<string>()).Result).Equals(null);

        var result = _controller.UpdateClaim(claim.UCR, claim);

        var failJson = JsonConvert.SerializeObject("Claim does not exist");

        var notFoundResult = result.Should().BeAssignableTo<NotFoundObjectResult>();
        result.Should().NotBeNull();
        result.Should().BeOfType<NotFoundObjectResult>();
        notFoundResult.Should().NotBeNull();
        notFoundResult.Subject.Value.Should().Be(failJson);
    }

    [Fact]
    public void ClaimsController_UpdateClaim_ReturnsFailedDatabaseUpdate()
    {
        var claim = new Claims { CompanyId = 1, ClaimDate = DateTime.UtcNow.AddDays(-27), UCR = "Test1" };
        _databaseService.Setup(s => s.GetClaimByUCR(claim.UCR)).Returns(Task.FromResult(claim));

        var result = _controller.UpdateClaim(claim.UCR, claim);

        var failJson = JsonConvert.SerializeObject("Failed to update the database");

        var notFoundResult = result.Should().BeAssignableTo<BadRequestObjectResult>();
        result.Should().NotBeNull();
        result.Should().BeOfType<BadRequestObjectResult>();
        notFoundResult.Should().NotBeNull();
        notFoundResult.Subject.Value.Should().Be(failJson);
    }
    #endregion
}