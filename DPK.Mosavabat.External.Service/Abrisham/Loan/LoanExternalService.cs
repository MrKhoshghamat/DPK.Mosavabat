using System.Globalization;
using AutoMapper;
using Dpk.UseAdanicAbrishamService.Services;
using DPK.Mosavabat.Common.Enums;
using DPK.Mosavabat.Common.Extensions;
using DPK.Mosavabat.External.Services.Contracts.Abrisham.Loan;
using DPK.Mosavabat.Shared.EntitiesDto.Abrisham.Loan;
using DPK.Mosavabat.Shared.EntitiesDto.CreditProcesses.Abrisham.Loan;
using DPK.Mosavabat.Shared.ViewModels.Abrisham.Loan;
using Microsoft.Extensions.Configuration;

namespace DPK.Mosavabat.External.Services.Abrisham.Loan
{
    public class LoanExternalService : ILoanExternalService
    {
        private readonly AdanicAbrishamService _adanicAbrishamService;
        private readonly IMapper _mapper;

        public LoanExternalService(IConfiguration configuration, IMapper mapper)
        {
            _mapper = mapper;
            AdanicAbrishamService.TenantId = configuration["Abrisham:TenantId"];
            _adanicAbrishamService = new AdanicAbrishamService();
        }

        public async Task<LoanBillDto> GetLoanBill(GetLoanBillVm viewModel, CancellationToken cancellationToken)
        {
            var loanBillVm = _mapper.Map<LoanBillVM>(viewModel);
            loanBillVm.BillType = "03";
            loanBillVm.Phase = "00";
            loanBillVm.Terminal = "1111";

            var response = await _adanicAbrishamService.LoanBillAsync(loanBillVm, cancellationToken);
            return response;
        }

        public async Task<GetSettledLoanBillDto> GetSettledLoanBill(GetSettledLoanBillVm viewModel, CancellationToken cancellationToken = default)
        {
            var loanBillVm = _mapper.Map<LoanBillVM>(viewModel);
            loanBillVm.BillType = "03";
            loanBillVm.Phase = "00";
            loanBillVm.Terminal = "1111";

            var loanBillResponse = await _adanicAbrishamService.LoanBillAsync(loanBillVm, cancellationToken);
            var response = loanBillResponse.Transactions
                .Where(t => t is
                {
                    Indicator: "0",
                    Description:
                    "تسويه نهايي امهال" or
                    "تسويهامهال درسررسيدگذشته ومعوق" or
                    "تسويه تسهيلات تضمين دولت" or
                    "تسويه بعدازسررسيد" or
                    "تسويه"
                })
                .ToList();

            return _mapper.Map<GetSettledLoanBillDto>(response);
        }

        public async Task<GetPayedLoanBillDto> GetPayedLoanBill(GetPayedLoanBillVm viewModel, CancellationToken cancellationToken = default)
        {
            var loanBillVm = _mapper.Map<LoanBillVM>(viewModel);
            loanBillVm.BillType = "03";
            loanBillVm.Phase = "00";
            loanBillVm.Terminal = "1111";

            var loanBillResponse = await _adanicAbrishamService.LoanBillAsync(loanBillVm, cancellationToken);
            var response = loanBillResponse.Transactions
                .Where(t => t is
                {
                    Indicator: "0",
                    Description:
                    "پرداخت تسهيلات" or
                    "پيش پرداخت" or
                    "افزايش سرمايه" or
                    "فروش نقدي كالاي سلف" or
                    "پرداخت سرمايهمشاركت مدني" or
                    "پرد‌اخت‌ سرمایه‌مشارکت‌ مدنی‌"
                })
                .ToList();

            return _mapper.Map<GetPayedLoanBillDto>(response);
        }

        public async Task<GetReceivedLoanBillDto> GetReceivedLoanBill(GetReceivedLoanBillVm viewModel, CancellationToken cancellationToken = default)
        {
            var loanBillVm = _mapper.Map<LoanBillVM>(viewModel);
            loanBillVm.BillType = "03";
            loanBillVm.Phase = "00";
            loanBillVm.Terminal = "1111";

            var todayFormattedDate = DateTime.Now.ToFormattedPersianDate(DateFormat.TwoDigitYear);

            var loanBillResponse = await _adanicAbrishamService.LoanBillAsync(loanBillVm, cancellationToken);
            var response = loanBillResponse.Transactions
                .Where(t => t.PaymentDate == todayFormattedDate && t is
                {
                    Indicator: "0",
                    Description:
                    "وصول قسط امهال" or
                    "وصولتدريجيمطالبات باحكم دادگاه" or
                    "وصولمانده مطالبات باحكم دادگاه" or
                    "وصولتدريجيمطالبات تسهيلات قسطي" or
                    "وصول تدريجي مطالبات غيرقسطي" or
                    "وصولماندهمطالبات تسهيلات قسطي" or
                    "وصول مانده مطالبات غير قسطي" or
                    "وصولقسطامهالدرسررسيدگذشتهمعوق" or
                    "كاهش سرمايه" or
                    "وصول قسط" or
                    "وصول‌ قسط" or
                    "بازپرداخت تدريجي" or
                    "وصول بعد از سررسيد" or
                    "وصول مرحله اي" or
                    "وصولتدريجيمطالبات" or
                    "وصولمطالباتبانك"
                })
                .ToList();

            return _mapper.Map<GetReceivedLoanBillDto>(response);
        }

        public async Task<GetLoanCustomerFacilitiesDto> GetLoanCustomerFacilities(GetLoanCustomerFacilitiesVm viewModel, CancellationToken cancellationToken)
        {
            var loanCustomerFacilityListVm = _mapper.Map<LoanCustomerFacilityListVM>(viewModel);

            var todayFormattedDate = DateTime.Now.ToFormattedPersianDate(DateFormat.FourDigitYear);

            loanCustomerFacilityListVm.StartDate = todayFormattedDate;
            loanCustomerFacilityListVm.FinishDate = todayFormattedDate;
            loanCustomerFacilityListVm.BranchNo = "0000";

            var response = await _adanicAbrishamService.LoanCustomerFacilityListAsync(loanCustomerFacilityListVm, cancellationToken);
            response.FacilityList = response.FacilityList.Where(x => x.FacilityNo != " ").ToList();

            return _mapper.Map<GetLoanCustomerFacilitiesDto>(response);
        }

        public async Task<LoanContractsAllStatementDto> GetLoanContractsAllStatement(GetLoanContractsAllStatementVm viewModel,
            CancellationToken cancellationToken = default)
        {
            var loanContractsAllStatementVm = _mapper.Map<LoanContractsAllStatementVM>(viewModel);
            loanContractsAllStatementVm.ConfirmFlag = "Y";
            loanContractsAllStatementVm.SubContractNumber = "0";
            loanContractsAllStatementVm.EndDate = DateTime.Now.ToFormattedPersianDate(DateFormat.FourDigitYear);

            var response = await _adanicAbrishamService.LoanContractsAllStatementAsync(
                loanContractsAllStatementVm,
                cancellationToken);

            response.TransactionInformationDtos = response.TransactionInformationDtos
                .Where(transaction =>
                    !string.IsNullOrWhiteSpace(transaction.DocumentRow)
                )
                .ToList();

            return response;
        }

        public async Task<LoanInfoDto> GetLoanInfo(GetLoanInfoVm viewModel, CancellationToken cancellationToken)
        {
            var loanInfoVm = _mapper.Map<LoanInfoVM>(viewModel);
            var response = await _adanicAbrishamService.LoanInfoAsync(loanInfoVm, cancellationToken);
            return response;
        }

        private static string FormattedDate()
        {
            var persianCalendar = new PersianCalendar();
            var currentDate = DateTime.Now;
            var persianYear = persianCalendar.GetYear(currentDate);
            var persianMonth = persianCalendar.GetMonth(currentDate);
            var persianDay = persianCalendar.GetDayOfMonth(currentDate);
            var formattedDate = $"{persianYear % 100:D2}{persianMonth:D2}{persianDay:D2}";
            return formattedDate;
        }
    }
}
