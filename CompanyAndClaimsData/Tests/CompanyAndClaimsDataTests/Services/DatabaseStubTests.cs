using CompanyAndClaimsData.Models;
using CompanyAndClaimsData.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyAndClaimsDataTests.Services;

public class DatabaseStubTests
{
    private DatabaseStub _database;


    public DatabaseStubTests()
    {
        _database = new DatabaseStub();
    }

    #region Happy Path Tests
    [Fact]
    public void Stub_GetClaimByUCR_ReturnsExpectedResult()
    {
        var testData = StubData.claimsData[0];

        var result = _database.GetClaimByUCR("Test1").Result;

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(testData);
        result.Should().BeOfType<Claims>();
    }

    [Fact]
    public void Stub_GetClaimsByCompanyId_ReturnsExpectedResult()
    {
        var testData = StubData.claimsData[0];

        var result = _database.GetClaimsByCompanyId(1).Result;

        result.Should().NotBeNull();
        result.Should().Contain(testData);
        result.Should().BeOfType<List<Claims>>();
    }

    [Fact]
    public void Stub_GetCompanyById_ReturnsExpectedResult()
    {
        var testData = StubData.companiesData[0];

        var result = _database.GetCompanyById(1).Result;

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(testData);
        result.Should().BeOfType<Company>();
    }

    [Fact]
    public void Stub_UpdateDatabase_ReturnsExpectedResult()
    {
        var testData = new Claims()
        {
            AssuredName = null,
            ClaimDate = DateTime.UtcNow.AddDays(-27),
            closed = false,
            CompanyId = 1,
            IncurredLoss = 0,
            LossDate = StubData.claimsData[0].LossDate,
            UCR = "Test1"
        };

        var updateData = new Claims()
        {
            AssuredName = null,
            ClaimDate = DateTime.UtcNow.AddDays(-27),
            closed = true,
            CompanyId = 1,
            IncurredLoss = 0,
            LossDate = StubData.claimsData[0].LossDate,
            UCR = "Test1"
        };

        var result = _database.UpdateClaimDatabase(updateData).Result;

        var dataCheck = StubData.claimsData[0];

        result.Should().BeTrue();
        dataCheck.Should().NotBeEquivalentTo(testData);
        dataCheck.closed.Should().BeTrue();
    }
    #endregion
}
