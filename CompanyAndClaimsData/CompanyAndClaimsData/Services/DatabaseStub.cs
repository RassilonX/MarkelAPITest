using CompanyAndClaimsData.Models;

namespace CompanyAndClaimsData.Services;

public class DatabaseStub : IDatabaseService
{
    public Task<Claims> GetClaimByUCR(string claimId)
    {
        foreach (var claim in StubData.claimsData)
        {
            if (claim.UCR == claimId)
                return Task.FromResult(claim);
        }

        return null;
    }

    public Task<List<Claims>> GetClaimsByCompanyId(int companyId)
    {
        var result = new List<Claims>();

        foreach (var claim in StubData.claimsData)
        {
            if (claim.CompanyId == companyId)
                result.Add(claim);
        }

        return Task.FromResult(result);
    }

    public Task<Company> GetCompanyById(int id)
    {
        foreach (var company in StubData.companiesData)
        {
            if (company.Id == id)
                return Task.FromResult(company);
        }

        return null;
    }

    public Task<bool> UpdateDatabase(Claims updatedClaim)
    {
        var claimIndex = StubData.claimsData.FindIndex(c => c.UCR == updatedClaim.UCR);

        if (claimIndex == -1)
            return Task.FromResult(false);
        
        StubData.claimsData[claimIndex] = updatedClaim;

        return Task.FromResult(true);
    }
}

public static class StubData
{
    public static List<Company> companiesData { get; set; } = new List<Company>() {
        new Company { Id = 1, InsuranceEndDate = DateTime.UtcNow.AddDays(-5) },
        new Company { Id = 2, InsuranceEndDate = DateTime.UtcNow.AddDays(5) },
        new Company { Id = 3, InsuranceEndDate = DateTime.UtcNow.AddDays(15) }
    };

    public static List<Claims> claimsData { get; set; } = new List<Claims>() {
        new Claims { CompanyId = 1, ClaimDate = DateTime.UtcNow.AddDays(-27), UCR = "Test1"},
        new Claims { CompanyId = 2, ClaimDate = DateTime.UtcNow.AddDays(-127), UCR = "Test2"},
        new Claims { CompanyId = 2, ClaimDate = DateTime.UtcNow.AddDays(-287), UCR = "Test3"}
    };
}
