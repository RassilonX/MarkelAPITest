using CompanyAndClaimsData.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Collections.Generic;

namespace CompanyAndClaimsData.Services
{
    public class DatabaseStub : IDatabaseService
    {
        List<Company> companiesData = new List<Company>() { 
            new Company { Id = 1, InsuranceEndDate = DateTime.UtcNow.AddDays(-5) }, 
            new Company { Id = 2, InsuranceEndDate = DateTime.UtcNow.AddDays(5) }, 
            new Company { Id = 3, InsuranceEndDate = DateTime.UtcNow.AddDays(15) } 
        };

        List<Claims> claimsData = new List<Claims>() { 
            new Claims { CompanyId = 1, ClaimDate = DateTime.UtcNow.AddDays(-27), UCR = "Test1"},
            new Claims { CompanyId = 2, ClaimDate = DateTime.UtcNow.AddDays(-127), UCR = "Test2"},
            new Claims { CompanyId = 2, ClaimDate = DateTime.UtcNow.AddDays(-287), UCR = "Test3"}
        };

        public Claims GetClaimByUCR(string claimId)
        {
            foreach (var claim in claimsData)
            {
                if (claim.UCR == claimId)
                    return claim;
            }

            return null;
        }

        public IEnumerable<Claims> GetClaimsByCompanyId(int companyId)
        {
            var result = new List<Claims>();

            foreach (var claim in claimsData)
            {
                if (claim.CompanyId == companyId)
                    result.Add(claim);
            }

            return result;
        }

        public Company GetCompanyById(int id)
        {
            foreach (var company in companiesData)
            {
                if (company.Id == id)
                    return company;
            }

            return null;
        }

        public bool UpdateDatabase(Claims updatedClaim)
        {
            var claimIndex = claimsData.FindIndex(c => c.UCR == updatedClaim.UCR);

            if (claimIndex == -1)
                return false;
            
            Claims claim = claimsData[claimIndex];
            claim = updatedClaim;
            claimsData[claimIndex] = claim;

            return true;
        }
    }
}
