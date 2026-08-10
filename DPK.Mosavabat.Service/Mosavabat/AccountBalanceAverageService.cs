using DPK.Mosavabat.Contracts;
using DPK.Mosavabat.Database.Services.Contracts.Mosavabat;
using DPK.Mosavabat.Services.Contracts.Mosavabat;
using DPK.Mosavabat.Shared.EntitiesDto.Mosavabat;
using DPK.Mosavabat.Shared.ViewModels.Mosavabat;
using Newtonsoft.Json;

namespace DPK.Mosavabat.Services.Mosavabat
{
    public class AccountBalanceAverageService : IAccountBalanceAverageService
    {
        private readonly IAccountBalanceAverageDatabaseService _accountBalanceAverageDatabaseService;
        private readonly ILoggerManager _logger;

        public AccountBalanceAverageService(
            IAccountBalanceAverageDatabaseService accountBalanceAverageDatabaseService,
            ILoggerManager logger)
        {
            _accountBalanceAverageDatabaseService = accountBalanceAverageDatabaseService;
            _logger = logger;
        }

        public async Task<List<GetAccountBalanceAverageDto>?> GetAccountBalanceAverage(GetAccountBalanceAverageVm viewModel, CancellationToken cancellationToken = default)
        {
            _logger.Trace(JsonConvert.SerializeObject(viewModel));
            return await _accountBalanceAverageDatabaseService.GetAccountBalanceAverage(viewModel, cancellationToken);
        }

        public async Task<List<GetAccountBalanceAverageTarhJariZendegiDto>> GetAccountBalanceAverageTarhJariZendegi(GetAccountBalanceAverageTarhJariZendegiVm viewModel, CancellationToken cancellationToken = default)
        {
            _logger.Trace(JsonConvert.SerializeObject(viewModel));
            return await _accountBalanceAverageDatabaseService.GetAccountBalanceAverageTarhJariZendegi(viewModel, cancellationToken);
        }

        public async Task<List<GetAccountBalanceAverageDto>?> GetRelatedAccountBalanceAverage(GetRelatedAccountBalanceAverageVM viewModel, CancellationToken cancellationToken = default)
        {
            _logger.Trace(JsonConvert.SerializeObject(viewModel));
            return await _accountBalanceAverageDatabaseService.GetRelatedAccountBalanceAverage(viewModel, cancellationToken);
        }

        public async Task<List<GetAccountBalanceAverageDto>> GetSharedAccountBalanceAverage(GetSharedAccountBalanceAverageVM viewModel, CancellationToken cancellationToken = default)
        {
            var accountBalanceAverageList = new List<GetAccountBalanceAverageDto>();
            _logger.Trace(JsonConvert.SerializeObject(viewModel));
            var sharedCustomerNumbers = await _accountBalanceAverageDatabaseService.GetSharedCustomerNumbers(new GetSharedCustomerNumbersVM { CustomerNumber = viewModel.CustomerNo }, cancellationToken);

            foreach (var sharedCustomerNumber in sharedCustomerNumbers)
            {
                var model = new GetAccountBalanceAverageVm
                {
                    CustomerNo = sharedCustomerNumber.ShareCustomerNumber,
                    ApprovalRequestId = viewModel.ApprovalRequestId,
                    CuMosavabeId = viewModel.CuMosavabeId,
                    TypeOfContractLength = viewModel.TypeOfContractLength,
                    MaxContractLength = viewModel.MaxContractLength,
                    Code = viewModel.Code,
                    CurrentMonth = viewModel.CurrentMonth,
                    FacilityGroup = viewModel.FacilityGroup,
                };

                var accountBalanceAverage = await GetAccountBalanceAverage(model, cancellationToken);

                foreach (var item in accountBalanceAverage)
                {
                    item.MaxGrant = (double.Parse(item.MaxGrant) * sharedCustomerNumber.SharePercentage / 100).ToString("0.00");
                    accountBalanceAverageList.Add(item);
                }
            }

            return accountBalanceAverageList;
        }

        public async Task<List<GetRawAccountBalanceAverageDto>> GetRawAccountBalanceAverage(GetRawAccountBalanceAverageVm viewModel, CancellationToken cancellationToken)
        {
            _logger.Trace(JsonConvert.SerializeObject(viewModel));
            return await _accountBalanceAverageDatabaseService.GetRawAccountBalanceAverage(viewModel,
                cancellationToken);
        }

        public async Task<List<ProvidingApprovedAmountFromCreditsDto>?> ProvidingApprovedAmountFromCredits(ProvidingApprovedAmountFromCreditsVm viewModel,
            CancellationToken cancellationToken = default)
        {
            _logger.Trace(JsonConvert.SerializeObject(viewModel));
            return await _accountBalanceAverageDatabaseService.ProvidingApprovedAmountFromCredits(viewModel,
                cancellationToken);
        }
    }
}
