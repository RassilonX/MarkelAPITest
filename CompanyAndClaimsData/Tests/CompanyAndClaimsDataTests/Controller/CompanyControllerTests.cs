using CompanyAndClaimsData.Controllers;
using CompanyAndClaimsData.Models;
using CompanyAndClaimsData.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CompanyAndClaimsDataTests.Controller;

public class CompanyControllerTests
{
    private readonly Mock<IDatabaseService> _databaseService;
    private readonly CompanyController _controller;

    public CompanyControllerTests()
    {
        _databaseService = new Mock<IDatabaseService>();

        _controller = new CompanyController(_databaseService.Object);
    }

    #region Happy Path Tests
    [Fact]
    public void CompanyController_Company_ReturnsExpectedData()
    {
        var testData = new Company { Id = 1, InsuranceEndDate = DateTime.UtcNow.AddDays(-5) };
        _databaseService.Setup(s => s.GetCompanyById(1)).Returns(Task.FromResult(testData));

        var returnedCompany = new
        {
            Company = testData,
            ActiveInsurance = testData.InsuranceEndDate < DateTime.UtcNow ? false : true
        };
        var testResult = JsonConvert.SerializeObject(returnedCompany);

        var result = _controller.Company(1);


        var okResult = result.Should().BeAssignableTo<OkObjectResult>();
        result.Should().NotBeNull();
        result.Should().BeOfType<OkObjectResult>();
        okResult.Should().NotBeNull();
        okResult.Subject.Value.Should().Be(testResult);
    }
    #endregion


    #region Unhappy Path Tests
    [Fact]
    public void CompanyController_Company_ReturnsNotFoundResult()
    {
        _databaseService.Setup(s => s.GetCompanyById(123)).Returns((Task<Company>)null);

        var result = _controller.Company(123);
        var testResult = JsonConvert.SerializeObject("Company does not exist");

        var notFoundResult = result.Should().BeAssignableTo<NotFoundObjectResult>();
        result.Should().NotBeNull();
        result.Should().BeOfType<NotFoundObjectResult>();
        notFoundResult.Should().NotBeNull();
        notFoundResult.Subject.Value.Should().Be(testResult);
    }
    #endregion
}
