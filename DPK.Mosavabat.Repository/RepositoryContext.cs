using DPK.Mosavabat.Entities.Models;
using DPK.Mosavabat.Entities.Models.LoanBill;
using DPK.Mosavabat.Entities.Models.RequestFacilities;
using DPK.Mosavabat.Repository.Configuration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DPK.Mosavabat.Repository;

public class RepositoryContext(DbContextOptions options) : IdentityDbContext<User>(options)
{
    public DbSet<Log> Logs { get; set; }
    public DbSet<PaymentPermission> PaymentPermissions { get; set; }
    public DbSet<LoanBillRequest> LoanBillRequests { get; set; }
    public DbSet<LoanBillData> LoanBillDatas { get; set; }
    public DbSet<LcpRequestFacilitySubmission> LcpRequestFacilitySubmissions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new RoleConfiguration());
        modelBuilder.ApplyConfiguration(new LcpRequestFacilitySubmissionConfiguration());
    }
}