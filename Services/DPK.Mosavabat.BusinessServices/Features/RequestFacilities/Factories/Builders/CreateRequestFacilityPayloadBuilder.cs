using DPK.Mosavabat.BusinessServices.Features.RequestFacilities.Factories.Abstractions;
using DPK.Mosavabat.Models.Commands.RequestFacilities;
using DPK.Mosavabat.Models.DataModels.Bizagi;
using System.Collections.Generic;
using System.Linq;
using DPK.Mosavabat.SqlBusinessServices.RequestFacilities.Contract;
using DPK.Mosavabat.SqlBusinessServices.RequestFacilities.Service;

namespace DPK.Mosavabat.BusinessServices.Features.RequestFacilities.Factories.Builders;

internal sealed class CreateRequestFacilityPayloadBuilder
    : ICreateRequestFacilityPayloadBuilder
{
    public CreateRequestFacilityBizagiDm Build(
        CreateRequestFacilityCommand command,
        IRequestFacilitySqlService requestFacilitySqlService,
        string guid)
    {
        return new CreateRequestFacilityBizagiDm
        {
            Guid = guid,
            BranchCode = command.BranchCode,
            CustomerNo = command.CustomerNo,
            AccountNumber = command.AccountNo,
            LegalName = command.LegalName,
            RequestDate = command.RequestDate,
            RequestNo = command.RequestNo,
            RequestFacilityStatus = BuildStatus(),
            LcpCollaterals = BuildCollaterals(command),
            Guarantors = BuildGuarantors(command),
            SubjectFacilities = BuildSubjectFacilities(command),
            LcpFiles = BuildFiles(command)
        };
    }

    private static StatusBizagiDm BuildStatus()
        => new()
        {
            Code = "5",
            Title = "Registered"
        };

    private static List<CollateralBizagiDm> BuildCollaterals(
        CreateRequestFacilityCommand command,
        IRequestFacilitySqlService requestFacilitySqlService)
    {
        return command.Collaterals.Collaterals.Select(c => new CollateralBizagiDm
        {
            LcpId = command.LcpId,
            Amount = c.Amount,
            CollateralType = await requestFacilitySqlService,
            Description = c.Description
        }).ToList();
    }

    private static List<GuarantorBizagiDm> BuildGuarantors(
        CreateRequestFacilityCommand command)
    {
        return command.Guarantors.Guarantors.Select(g => new GuarantorBizagiDm
        {
            LcpId = command.LcpId,
            FirstName = g.FirstName,
            LastName = g.LastName,
            NationalCode = g.NationalCode,
            Nationality = g.Nationality,
            Country = g.Country,
            GuarantorType = g.GuarantorType,
            LegalGuarantorType = g.LegalGuarantorType,
            Description = g.Description
        }).ToList();
    }

    private static List<SubjectFacilityBizagiDm> BuildSubjectFacilities(
        CreateRequestFacilityCommand command)
    {
        return command.SubjectFacilities.SubjectFacilities.Select(sf => new SubjectFacilityBizagiDm
        {
            LcpId = command.LcpId,
            Amount = sf.Amount,
            ContractPeriod = sf.ContractPeriod,
            ContractPeriodType = sf.ContractPeriodType,
            Description = sf.Description,
            SubjectContract = sf.SubjectContract,
            SubContractType = sf.SubContractType,
            FacilityGroup = sf.FacilityGroup,

            RequestDocuments = sf.RequestDocuments.RequestDocuments.Select(d => new ContractDocumentBizagiDm
            {
                LcpId = command.LcpId,
                Amount = d.Amount,
                DueDate = d.DueDate,
                DocumentNumber = d.DocumentNumber,
                DocumentType = d.DocumentType,
                PayAmount = d.PayAmount,
                Owner = d.Owner
            }).ToList()
        }).ToList();
    }

    private static List<LcpFileBizagiDm> BuildFiles(
        CreateRequestFacilityCommand command)
    {
        return command.LcpFiles.LcpFiles.Select(f => new LcpFileBizagiDm
        {
            LcpId = command.LcpId,
            FileLink = f.FileLink,
            Name = f.Name,
            Title = f.Title
        }).ToList();
    }
}
