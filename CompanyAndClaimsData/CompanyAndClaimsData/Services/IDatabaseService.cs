using CompanyAndClaimsData.Models;

namespace CompanyAndClaimsData.Services
{
    public interface IDatabaseService
    {
        //Company Table
        public Company GetCompanyById(int id);

        //Claims Table
        public IEnumerable<Claims> GetClaimsByCompanyId(int companyId);

        public Claims GetClaimByUCR(string claimId);

        public bool UpdateDatabase(Claims claims);
    }
}
