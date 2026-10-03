using DPK.Mosavabat.Entities.Models.RequestFacilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DPK.Mosavabat.Repository.Configuration;

public sealed class LcpRequestFacilitySubmissionConfiguration
    : IEntityTypeConfiguration<LcpRequestFacilitySubmission>
{
    public void Configure(EntityTypeBuilder<LcpRequestFacilitySubmission> builder)
    {
        builder.ToTable("LcpRequestFacilitySubmission", "dbo");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder.Property(x => x.SourceSystem)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.ProcessType)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.LcpId)
            .IsRequired();

        builder.Property(x => x.RequestNo)
            .HasMaxLength(50);

        builder.Property(x => x.CorrelationId)
            .IsRequired();

        builder.Property(x => x.PayloadHash)
            .IsRequired()
            .HasColumnType("varbinary(32)");

        builder.Property(x => x.Status)
            .IsRequired()
            .HasConversion<byte>();

        builder.Property(x => x.OwnerToken)
            .IsRequired();

        builder.Property(x => x.LeaseUntil);

        builder.Property(x => x.AttemptCount)
            .IsRequired()
            .HasDefaultValue(1);

        builder.Property(x => x.CaseId);

        builder.Property(x => x.RadNumber);

        builder.Property(x => x.LastErrorCode)
            .HasMaxLength(50);

        builder.Property(x => x.LastErrorMessage)
            .HasMaxLength(1000);

        builder.Property(x => x.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("SYSDATETIME()");

        builder.Property(x => x.UpdatedAt)
            .IsRequired()
            .HasDefaultValueSql("SYSDATETIME()");

        builder.Property(x => x.SendingAt);
        builder.Property(x => x.BizagiCreatedAt);
        builder.Property(x => x.CompletedAt);

        // Concurrency Token — پایه‌ی کل منطق Optimistic Concurrency در Repository.
        // بدون این خط، DbUpdateConcurrencyException که در Repository صید می‌کنیم
        // هرگز پرتاب نمی‌شه و race condition دوباره ممکن می‌شه.
        builder.Property(x => x.RowVersion)
            .IsRowVersion();

        // کلید کسب‌وکاری: تضمین سطح دیتابیس که هیچ‌وقت دو Submission با همین
        // ترکیب هم‌زمان ساخته نشن (پشتیبان اصلی منطق ReserveAsync در برابر race condition)
        builder.HasIndex(x => new { x.SourceSystem, x.ProcessType, x.LcpId })
            .IsUnique()
            .HasDatabaseName("UX_LcpRequestFacilitySubmission_BusinessKey");

        // ایندکس پشتیبان کوئری Reconciliation (GetRecoveryCandidatesAsync)
        // که دنبال رکوردهای Sending/Unknown با Lease منقضی می‌گرده
        builder.HasIndex(x => new { x.Status, x.LeaseUntil })
            .HasDatabaseName("IX_LcpRequestFacilitySubmission_Recovery");
    }
}