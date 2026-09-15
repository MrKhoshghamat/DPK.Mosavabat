using System.Globalization;
using DPK.Mosavabat.Contracts;
using DPK.Mosavabat.Database.Services.Contracts.CreditProcesses;
using DPK.Mosavabat.Database.Services.Contracts.Mosavabat;
using DPK.Mosavabat.External.Services.Contracts.Bizagi;
using DPK.Mosavabat.Services.Contracts.CreditProcesses;
using DPK.Mosavabat.Shared.EntitiesDto.CreditProcesses.PaymentPermission;
using DPK.Mosavabat.Shared.Templates.Bizagi;
using DPK.Mosavabat.Shared.ViewModels.CreditProcesses.PaymentPermission;
using DPK.Mosavabat.Shared.ViewModels.Mosavabat;
using Newtonsoft.Json;
using System.Xml;
using AutoMapper;
using DPK.Mosavabat.Shared.DataModels.CreditProcesses.PaymentPermission;
using DPK.Mosavabat.External.Services.Contracts.Abrisham.Loan;
using DPK.Mosavabat.Shared.DataModels.Abrisham.Loan;
using DPK.Mosavabat.Shared.DataTransferObjects.Lms;
using DPK.Mosavabat.Shared.EntitiesDto.CreditProcesses.Abrisham.Loan;
using DPK.Mosavabat.Shared.ViewModels.Abrisham.Loan;
using Dpk.UseAdanicAbrishamService.Services;
using PaymentPermissionIssuanceControlDto = DPK.Mosavabat.Shared.EntitiesDto.CreditProcesses.PaymentPermission.PaymentPermissionIssuanceControlDto;
using DPK.Mosavabat.Shared.DataModels.Lms;

namespace DPK.Mosavabat.Services.CreditProcesses;

public class CreditProcessesService : ICreditProcessService
{
    private readonly ICreditProcessesDatabaseService _creditProcessesDatabaseService;
    private readonly IContractDatabaseService _contractDatabaseService;
    private readonly ILoanExternalService _loanExternalService;
    private readonly IUserDatabaseService _userDatabaseService;
    private readonly IWorkflowEngineServices _workflowEngine;
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;
    private readonly ILoanBillProcessingService _loanBillProcessingService;
    private readonly IContractTransactionService _contractTransactionService;

    public CreditProcessesService(
        ICreditProcessesDatabaseService creditProcessesDatabaseService,
        ILoanExternalService loanExternalService,
        IUserDatabaseService userDatabaseService,
        IWorkflowEngineServices workflowEngine,
        ILoggerManager logger,
        IMapper mapper,
        IContractDatabaseService contractDatabaseService,
        ILoanBillProcessingService loanBillProcessingService,
        IContractTransactionService contractTransactionService)
    {
        _creditProcessesDatabaseService = creditProcessesDatabaseService;
        _loanExternalService = loanExternalService;
        _userDatabaseService = userDatabaseService;
        _workflowEngine = workflowEngine;
        _logger = logger;
        _mapper = mapper;
        _contractDatabaseService = contractDatabaseService;
        _loanBillProcessingService = loanBillProcessingService;
        _contractTransactionService = contractTransactionService;
    }

    public async Task<PaymentPermissionIssuanceControlDto?> PaymentPermissionIssuanceControl(PaymentPermissionIssuanceControlVm viewModel,
        CancellationToken cancellationToken = default)
    {
        _logger.Trace(JsonConvert.SerializeObject(viewModel));

        return await _creditProcessesDatabaseService.PaymentPermissionIssuanceControl(viewModel,
            cancellationToken);
    }

    public async Task<CheckAndUpdateC13InPaymentPermissionDto?> CheckAndUpdateC13InPaymentPermission(CheckAndUpdateC13InPaymentPermissionVm viewModel,
        CancellationToken cancellationToken = default)
    {
        _logger.Trace(JsonConvert.SerializeObject(viewModel));

        return await _creditProcessesDatabaseService.CheckAndUpdateC13InPaymentPermission(viewModel,
            cancellationToken);
    }

    public async Task<List<GetTermsAndConditionsApprovalRequestDto>?> GetTermsAndConditionsApprovalRequests(GetTermsAndConditionsApprovalRequestListVm viewModel,
        CancellationToken cancellationToken = default)
    {
        _logger.Trace(JsonConvert.SerializeObject(viewModel));

        return await _creditProcessesDatabaseService.GetTermsAndConditionsApprovalRequests(viewModel,
            cancellationToken);
    }

    public async Task<InsertPaymentPermissionDto?> InsertPaymentPermission(InsertPaymentPermissionVm viewModel, CancellationToken cancellationToken = default)
    {
        _logger.Trace(JsonConvert.SerializeObject(viewModel));

        return await _creditProcessesDatabaseService.InsertPaymentPermission(viewModel, cancellationToken);
    }

    public async Task<CreateCompleteContractInfoProcessDto?> CreateCompleteContractInfoProcess(CreateCompleteContractInfoProcessVm viewModel, CancellationToken cancellationToken = default)
    {
        _logger.Trace(JsonConvert.SerializeObject(viewModel));

        var branchDto = await _creditProcessesDatabaseService.GetBranchCodeByC13(new GetBranchCodeByC13Vm() { C13 = viewModel.C13 }, cancellationToken);

        var getUserNameResult = await _userDatabaseService.GetUserName(new GetUserNameVm()
        {
            BranchCode = Convert.ToInt32(branchDto.BranchCode),
            MosavabatClassName = MosavabatClassName.CompleteContractInfo
        }, cancellationToken);

        if (string.IsNullOrEmpty(getUserNameResult.Username))
            return new CreateCompleteContractInfoProcessDto()
            { MosavabatResponseDataModel = new MosavabatResponseDataModel(default, default) };

        var accountNoDto = await _creditProcessesDatabaseService.GetAccNoByC13(new GetAccNoByC13Vm() { C13 = viewModel.C13 }, cancellationToken);
        var dateGDto = await _creditProcessesDatabaseService.GetDateGByC13(new GetDateGByC13Vm() { C13 = viewModel.C13 }, cancellationToken);
        var c16Dto = await _creditProcessesDatabaseService.GetC16ByC13(new GetC16ByC13Vm() { C13 = viewModel.C13 }, cancellationToken);

        var createCompleteContractInfoProcessDataModel = new CreateCompleteContractInfoProcessDataModel()
        {
            MabAsl = viewModel.MabAsl,
            CuApprovalSubjectId = viewModel.CuApprovalSubjectId,
            C10 = viewModel.C10,
            Shob = branchDto.BranchCode,
            C13 = viewModel.C13,
            C16 = c16Dto.C16,
            Date_g = dateGDto.Date_g
        };

        var standardModel = WorkFlowEngineService.CreateStandardModelForBizagi(createCompleteContractInfoProcessDataModel, getUserNameResult.Username, "CompleteContractInfo");

        var data = WorkFlowEngineService.ConvertModelToXmlMosavabat(standardModel, "CU_Contract");

        var xDoc = new XmlDocument();

        xDoc.LoadXml(data);

        var response = await _workflowEngine.CreateCasesAsync(xDoc);

        if (response!.CaseId == 0 || response.RadNumber == 0)
            return new CreateCompleteContractInfoProcessDto() { MosavabatResponseDataModel = response };
        var c10Dto = await _userDatabaseService.GetCustomerEk(new GetCustomerEkVm() { AccountNo = accountNoDto.AccNo }, cancellationToken);
        await _contractDatabaseService.UpdateCuCustomerInCuContract(new Shared.DataModels.Mosavabat.Contract.UpdateCuCustomerInCuContractDm()
        {
            IdCu_Customer = c10Dto.IdCuCustomerEk,
            C13 = viewModel.C13!,
            C16 = c16Dto.C16,
        }, cancellationToken);

        return new CreateCompleteContractInfoProcessDto() { MosavabatResponseDataModel = response };
    }

    public async Task<CalculateApprovalRequestUnusedBalanceDto?> CalculateApprovalRequestUnusedBalance(CalculateApprovalRequestUnusedBalanceVm viewModel,
        CancellationToken cancellationToken = default)
    {
        _logger.Trace(JsonConvert.SerializeObject(viewModel));

        var getCustomerNoByMosavabeNoDto = await _creditProcessesDatabaseService
            .GetCustomerNoByMosavabeNo(new GetCustomerNoByMosavabeNoDataModel()
            {
                MosavabeNo = viewModel.MosavabeNumber
            }, cancellationToken);

        var customerNo = getCustomerNoByMosavabeNoDto.CustomerNo;
        var mosavabeNo = viewModel.MosavabeNumber;

        // Step 1: Initiate background processing
        await _loanBillProcessingService.InitiateLoanBillProcessing(customerNo, mosavabeNo, cancellationToken);

        // Step 2: Wait briefly for processing to complete (max 20 seconds)
        await _loanBillProcessingService
            .WaitForProcessingToCompleteOrTimeout(
                customerNo,
                TimeSpan.FromSeconds(20),
                cancellationToken);

        // Step 3: Get results (from cache or DB)
        var cacheKey = $"ProcessedContractsByMosavabeNo_{mosavabeNo}";
        var cached = _loanBillProcessingService
            .TryGetFromCacheByMosavabeNo(cacheKey, cancellationToken);

        var contracts = cached ?? await _loanBillProcessingService
            .GetProcessedContractsByMosavabeNo(mosavabeNo, cancellationToken);

        // Step 3: Compute result directly (same logic as before)
        var settledContracts = _contractTransactionService.GetSettledContracts(new GetSettledContractsDataModel()
        {
            LoanBills = _mapper.Map<List<GetLoanBillDm>>(contracts.Item2)
        }).C13List;

        var getContractDebtAmountsDto = await _creditProcessesDatabaseService.GetContractsDebtAmounts(
            settledContracts,
            cancellationToken);

        var settledContractsAmountsSum = getContractDebtAmountsDto.Select(x => x.DebtAmount).Sum();

        var dailyReceivedAmountsSum = _contractTransactionService.GetDailyReceivedContractsTransactionsAmounts(
            new GetReceivedContractsDataModel()
            {
                LoanBills = _mapper.Map<List<GetLoanBillDm>>(contracts.Item2)
            },
            contracts.Item1).Amount;

        var approvalRequestUnusedBalanceDto =
            await _creditProcessesDatabaseService.CalculateApprovalRequestUnusedBalance(viewModel, cancellationToken);

        var finalCalculation =
            Convert.ToInt64(approvalRequestUnusedBalanceDto!.Amount) +
            settledContractsAmountsSum +
            Convert.ToInt64(dailyReceivedAmountsSum);

        approvalRequestUnusedBalanceDto.Amount = finalCalculation.ToString();

        return approvalRequestUnusedBalanceDto;
    }

    public async Task<CreateViolationInvestigationProcessDto?> CreateViolationInvestigationProcess(CreateViolationInvestigationProcessVm viewModel,
        CancellationToken cancellationToken = default)
    {
        _logger.Trace(JsonConvert.SerializeObject(viewModel));

        var getUserNameResult = await _userDatabaseService.GetUserName(new GetUserNameVm()
        {
            BranchCode = (int)viewModel.Branches!,
            MosavabatClassName = MosavabatClassName.ViolationInvestigation
        }, cancellationToken);

        if (string.IsNullOrEmpty(getUserNameResult.Username))
            return new CreateViolationInvestigationProcessDto()
            { MosavabatResponseDataModel = new MosavabatResponseDataModel(default, default) };

        var createViolationInvestigationProcessDataModel = new CreateViolationInvestigationProcessDataModel()
        {
            Branches = viewModel.Branches,
            ContractNo = viewModel.ContractNo,
            CreateDate = viewModel.CreateDate,
            CuApprovalSubject = viewModel.CuApprovalSubject,
            InfractionTypeDataModel = viewModel.InfractionTypeDataModel,
            Inspector = viewModel.Inspector
        };

        var standardModel = WorkFlowEngineService.CreateStandardModelForBizagi(createViolationInvestigationProcessDataModel, getUserNameResult.Username, "ViolationInvestigation");

        var data = WorkFlowEngineService.ConvertModelToXmlMosavabat(standardModel, "Infraction");

        var xDoc = new XmlDocument();

        xDoc.LoadXml(data);

        var response = await _workflowEngine.CreateCasesAsync(xDoc);

        return new CreateViolationInvestigationProcessDto() { MosavabatResponseDataModel = response };
    }

    public async Task<CalculateCustomerDebtBalanceDto?> CalculateCustomerDebtBalance(CalculateCustomerDebtBalanceVm viewModel, CancellationToken cancellationToken = default)
    {
        _logger.Trace(JsonConvert.SerializeObject(viewModel));

        return await _creditProcessesDatabaseService.CalculateCustomerDebtBalance(viewModel, cancellationToken);
    }

    public async Task<UpdatePaymentPermissionDto?> UpdatePaymentPermission(UpdatePaymentPermissionVm viewModel, CancellationToken cancellationToken = default)
    {
        _logger.Trace(JsonConvert.SerializeObject(viewModel));

        return await _creditProcessesDatabaseService.UpdatePaymentPermission(viewModel, cancellationToken);
    }

    public async Task<List<GetPaymentPermissionDto>?> GetPaymentPermissions(GetPaymentPermissionListVm viewModel, CancellationToken cancellationToken = default)
    {
        _logger.Trace(JsonConvert.SerializeObject(viewModel));

        return await _creditProcessesDatabaseService.GetPaymentPermissions(viewModel, cancellationToken);
    }

    #region محاسبه مانده بدهی

    /// <summary>
    /// محاسبه مجموع مبالغ وصولی روز مشتری
    /// </summary>
    /// <param name="viewModel"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<AggregateDailyCustomerPaymentDto> AggregateDailyCustomerPayment(AggregateDailyCustomerPaymentVm viewModel,
        CancellationToken cancellationToken = default)
    {
        _logger.Trace(JsonConvert.SerializeObject(viewModel));

        var customerNo = viewModel.CustomerNo;

        // Step 1: Initiate background processing
        await _loanBillProcessingService
            .InitiateLoanBillProcessing(
                customerNo,
                cancellationToken);

        // Step 2: Wait briefly for processing to complete (max 20 seconds)
        await _loanBillProcessingService
            .WaitForProcessingToCompleteOrTimeout(
                customerNo,
                TimeSpan.FromSeconds(20),
                cancellationToken);

        // Step 3: Get results (from cache or DB)
        var cacheKey = $"ProcessedContractsByCustomerNo_{customerNo}";
        var cached = _loanBillProcessingService
            .TryGetFromCacheByCustomerNo(cacheKey, cancellationToken);

        var contracts = cached ?? await _loanBillProcessingService
            .GetProcessedContractsByCustomerNo(customerNo, cancellationToken);

        // Step 4: Compute result directly
        var dailyReceivedContractsAmounts = _contractTransactionService.GetDailyReceivedContractsTransactionsAmounts(
            new GetReceivedContractsDataModel()
            {
                LoanBills = _mapper.Map<List<GetLoanBillDm>>(contracts.Item2)
            },
            contracts.Item1).Amount;

        return new AggregateDailyCustomerPaymentDto
        {
            ReceivedDailyPaymentAmountSum = dailyReceivedContractsAmounts.ToString(CultureInfo.InvariantCulture)
        };
    }

    /// <summary>
    /// محاسبه مجموع مانده بدهی مشتری
    /// </summary>
    /// <param name="viewModel"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<CalculateCustomerTotalDebtDto> CalculateCustomerTotalDebt(CalculateCustomerTotalDebtVm viewModel, CancellationToken cancellationToken = default)
    {
        _logger.Trace(JsonConvert.SerializeObject(viewModel));

        var customerNo = viewModel.CustomerNo;

        // Step 1: Initiate background processing
        await _loanBillProcessingService
            .InitiateLoanBillProcessing(
                customerNo,
                cancellationToken);

        // Step 2: Wait briefly for processing to complete (max 20 seconds)
        await _loanBillProcessingService
            .WaitForProcessingToCompleteOrTimeout(
                customerNo,
                TimeSpan.FromSeconds(20),
                cancellationToken);

        // Step 3: Get results (from cache or DB)
        var cacheKey = $"ProcessedContractsByCustomerNo_{customerNo}";
        var cached = _loanBillProcessingService
            .TryGetFromCacheByCustomerNo(cacheKey, cancellationToken);

        var contracts = cached ?? await _loanBillProcessingService
            .GetProcessedContractsByCustomerNo(customerNo, cancellationToken);

        // Step 4: Compute result
        contracts.Item1 = _contractTransactionService.RemoveSettledContracts(new RemoveSettleContractsDataModel()
        {
            LoanBills = _mapper.Map<List<GetLoanBillDm>>(contracts.Item2),
            Contracts = contracts.Item1
        }).Contracts;

        var dailyPaidAmounts = _contractTransactionService.GetDailyPaidContractsTransactionsAmounts(
            new GetPaidContractsDataModel()
            {
                LoanBills = _mapper.Map<List<GetLoanBillDm>>(contracts.Item2)
            }).Amount;

        var dailyPaidContractsAmountsSumDto =
            await _creditProcessesDatabaseService.CalculateDailyPaidContractsAmountsSum(
                _mapper.Map<CalculateDailyPaidContractsAmountsSumVm>(viewModel),
                cancellationToken);

        // Step 5: Calculate final result
        var totalCurrentDebtAmount = contracts.Item1
                                         .Sum(facility => facility.TotalDebtBalance) +
                                     long.Parse(dailyPaidAmounts) +
                                     dailyPaidContractsAmountsSumDto.DailyPaidContractsAmountsSum -
                                     viewModel.ReceivedDailyPaymentAmountSum;

        return new CalculateCustomerTotalDebtDto()
        {
            TotalCurrentDebtAmount = totalCurrentDebtAmount
        };
    }

    #endregion

    public async Task<List<GetLoanBillDto>> GetLoanBillReport(GetLoanBillReportVm viewModel, CancellationToken cancellationToken = default)
    {
        var getCurrentFacilitiesReportVm = _mapper.Map<GetCurrentFacilitiesReportVm>(viewModel);
        var currentFacilitiesReport =
            await _creditProcessesDatabaseService.GetCurrentFacilitiesReport(getCurrentFacilitiesReportVm,
                cancellationToken);

        var loanBills = new List<LoanBillDto>();
        foreach (var facilityReportDto in currentFacilitiesReport)
        {
            loanBills.Add(await _loanExternalService.GetLoanBill(new GetLoanBillVm()
            {
                LoanId = facilityReportDto.C13.ToString()
            }, cancellationToken));
        }

        loanBills = loanBills.Where(x => x.Transactions.Count == 0).ToList();

        var getLoanBillDto = _mapper.Map<List<GetLoanBillDto>>(loanBills);

        return getLoanBillDto;
    }

    public async Task<CheckExistActivePaymentPermissionDto?> CheckExistActivePaymentPermission(CheckExistActivePaymentPermissionVm viewModel, CancellationToken cancellationToken = default)
    {
        _logger.Trace(JsonConvert.SerializeObject(viewModel));

        return await _creditProcessesDatabaseService.CheckExistActivePaymentPermission(viewModel, cancellationToken);
    }

    public async Task<List<GetCuContractDto>> GetCuContracts(GetCuContractsVm viewModel, CancellationToken cancellationToken = default)
    {
        _logger.Trace(JsonConvert.SerializeObject(viewModel));

        return await _creditProcessesDatabaseService.GetCuContracts(viewModel, cancellationToken);
    }

    public async Task<GetLiveApprovalRequestsAmountsSumDto> GetLiveApprovalRequestsAmountsSum(GetLiveApprovalRequestsAmountsSumVm viewModel, CancellationToken cancellationToken = default)
    {
        _logger.Trace(JsonConvert.SerializeObject(viewModel));
        return await _creditProcessesDatabaseService.GetLiveApprovalRequestsAmountsSum(viewModel, cancellationToken);
    }

    public async Task<IsLcpRequestDto> IsLcpRequest(IsLcpRequestVm viewModel, CancellationToken cancellationToken = default)
    {
        _logger.Trace(JsonConvert.SerializeObject(viewModel));
        return await _creditProcessesDatabaseService.IsLcpRequest(viewModel, cancellationToken);
    }

    public async Task<List<GetListOfExceptionPayReasonDto>?> GetListOfExceptionPayReason(GetListOfExceptionPayReasonVM viewModel, CancellationToken cancellationToken = default)
    {
        _logger.Trace(JsonConvert.SerializeObject(viewModel));
        return await _creditProcessesDatabaseService.GetListOfExceptionPayReason(viewModel, cancellationToken);
    }

    public async Task<List<GetListOfPaymentPermissionExceptionReasonDto>?> GetListOfPaymentPermissionExceptionReason(GetListOfPaymentPermissionExceptionReasonVM viewModel, CancellationToken cancellationToken = default)
    {
        _logger.Trace(JsonConvert.SerializeObject(viewModel));
        return await _creditProcessesDatabaseService.GetListOfPaymentPermissionExceptionReason(viewModel, cancellationToken);
    }

    public async Task<bool> AddToPaymentPermissionExceptionReason(PaymentPermissionExceptionReasonVM viewModel, CancellationToken cancellationToken = default)
    {
        _logger.Trace(JsonConvert.SerializeObject(viewModel));
        return await _creditProcessesDatabaseService.AddToPaymentPermissionExceptionReason(viewModel, cancellationToken);
    }

    public async Task<bool> DeletePaymentPermissionExceptionReason(PaymentPermissionExceptionReasonForDeleteVM viewModel, CancellationToken cancellationToken = default)
    {
        _logger.Trace(JsonConvert.SerializeObject(viewModel));
        return await _creditProcessesDatabaseService.DeletePaymentPermissionExceptionReason(viewModel, cancellationToken);
    }

    public async Task<List<GetListOfExceptionCodeByNationalCodeReasonDto>?> GetListOfExceptionCodeByNationalCode(GetListOfExceptionCodeByNationalCodeVM viewModel, CancellationToken cancellationToken = default)
    {
        _logger.Trace(JsonConvert.SerializeObject(viewModel));
        return await _creditProcessesDatabaseService.GetListOfExceptionCodeByNationalCode(viewModel, cancellationToken);
    }

    public async Task<GetDailyPaidContractsDto> GetDailyPaidContracts(GetDailyPaidContractsVm viewModel, CancellationToken cancellationToken = default)
    {
        _logger.Trace(JsonConvert.SerializeObject(viewModel));
        return await _creditProcessesDatabaseService.GetDailyPaidContracts(viewModel, cancellationToken);
    }

    public async Task<GetCustomerContractsPastDebtDto> GetCustomerContractsPastDebt(GetCustomerContractsPastDebtVm viewModel, CancellationToken cancellationToken = default)
    {
        _logger.Trace(JsonConvert.SerializeObject(viewModel));
        return await _creditProcessesDatabaseService.GetCustomerContractsPastDebt(viewModel, cancellationToken);
    }

    public async Task<GetSubjectTypeDto> GetSubjectType(GetSubjectTypeVm viewModel, CancellationToken cancellationToken = default)
    {
        _logger.Trace(JsonConvert.SerializeObject(viewModel));
        return await _creditProcessesDatabaseService.GetSubjectType(viewModel, cancellationToken);
    }

    public async Task<GetApprovalRequestInfoDto> GetApprovalRequestInfo(GetApprovalRequestInfoVm viewModel, CancellationToken cancellationToken = default)
    {
        _logger.Trace(JsonConvert.SerializeObject(viewModel));
        return await _creditProcessesDatabaseService.GetApprovalRequestInfo(viewModel, cancellationToken);
    }

    public async Task<IsAllowedRecalculateAccountBalanceAverageDto> IsAllowedRecalculateAccountBalanceAverage(IsAllowedRecalculateAccountBalanceAverageVm viewModel, CancellationToken cancellationToken = default)
    {
        _logger.Trace(JsonConvert.SerializeObject(viewModel));
        return await _creditProcessesDatabaseService.IsAllowedRecalculateAccountBalanceAverage(viewModel, cancellationToken);
    }
}