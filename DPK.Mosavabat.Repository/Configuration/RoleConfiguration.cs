using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DPK.Mosavabat.Repository.Configuration;

public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
{
    public void Configure(EntityTypeBuilder<IdentityRole> builder)
    {
        builder.HasData(
            new IdentityRole {Name = "DpkAdmin", NormalizedName = "DPKADMIN"},
            new IdentityRole {Name = "LoanManagementSystemAdmin", NormalizedName = "LOANMANAGEMENTSYSTEMADMIN"},
            new IdentityRole {Name = "LegalCustomerPortalAdmin", NormalizedName = "LEGALCUSTOMERPORTALADMIN"},
            new IdentityRole {Name = "BizagiAdmin", NormalizedName = "BIZAGIADMIN"},
            new IdentityRole {Name = "MosavabatAdmin", NormalizedName = "MOSAVABATADMIN"});
    }
}