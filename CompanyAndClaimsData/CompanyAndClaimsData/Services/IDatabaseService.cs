using CompanyAndClaimsData.Models;

namespace CompanyAndClaimsData.Services
{
    public interface IDatabaseService
    {
        //Company Table
        public Task<Company> GetCompanyById(int id);

        //Claims Table
        public Task<List<Claims>> GetClaimsByCompanyId(int companyId);

        public Task<Claims> GetClaimByUCR(string claimId);

        public Task<bool> UpdateClaimDatabase(Claims claims);
    }
}
