using AutoMapper;
using DPK.Mosavabat.Database.Services.Contracts.Mosavabat;
using DPK.Mosavabat.External.Services.Contracts.Payam;
using DPK.Mosavabat.Services.Contracts.CreditProcesses;
using DPK.Mosavabat.Services.Contracts.Mosavabat;
using DPK.Mosavabat.Shared.EntitiesDto.Mosavabat;
using DPK.Mosavabat.Shared.ViewModels.CreditProcesses.PaymentPermission;
using DPK.Mosavabat.Shared.ViewModels.Mosavabat;
using DPK.Mosavabat.Shared.ViewModels.Payam;


namespace DPK.Mosavabat.Services.Mosavabat
{
    public class BeneficiaryService : IBeneficiaryService
    {
        private readonly ISamatExternalService _samatExternalService;
        private readonly IBeneficiariesDatabaseService _beneficiariesDatabaseService;
        private readonly IMapper _mapper;

        public BeneficiaryService(
            ISamatExternalService samatExternalService,
            IBeneficiariesDatabaseService beneficiariesDatabaseService,
            IMapper mapper)

        {
            _samatExternalService = samatExternalService;
            _beneficiariesDatabaseService = beneficiariesDatabaseService;
            _mapper = mapper;
        }

        public async Task<List<GetBeneficiariesDebtListDto>> GetBeneficiariesDebtList(GetBeneficiariesDebtListVm viewModel, CancellationToken cancellationToken = default)
        {

            var beneficaries = await _samatExternalService.GetBeneficiaries(new BeneficiaryVm { NationalCode=viewModel.NationalCode,BirthDate=viewModel.BirthDate});

            var tasks = beneficaries.Select(async beneficary =>
            {
                var jariInMosavabatDebt = _beneficiariesDatabaseService.GetBeneficiariesLiveTotalDebt(new GetBeneficiariesLiveTotalDebtVm {NationalCode = beneficary.NationalCode! });
                return new GetBeneficiariesDebtListDto
                {
                    Name = beneficary.Name,
                    Relation = beneficary.MainRelation +'-'+ beneficary.Relation,
                    NationalCode = beneficary.NationalCode,
                    JariInMosavabat = jariInMosavabatDebt.Result.BeneficiariesLiveTotalDebt,
                    Jari = beneficary.Jari,
                    SarResid = beneficary.SarResid,
                    Moavagh = beneficary.Moavagh,
                    Mashkook = beneficary.Mashkook,
                    MandeTaahod = beneficary.MandeTaahod,
                    TotalDebt = beneficary.TotalDebt+jariInMosavabatDebt.Result.BeneficiariesLiveTotalDebt
                };
            });

            var result =  await Task.WhenAll(tasks);
            return result.ToList();
        }
    }
}
