using DPK.Mosavabat.Contracts;
using DPK.Mosavabat.Database.Services.Contracts.CreditProcesses;
using DPK.Mosavabat.Database.Services.Contracts.Mosavabat;
using DPK.Mosavabat.Entities.Exceptions;
using DPK.Mosavabat.Entities.Helper;
using DPK.Mosavabat.Entities.Models.LoanBill;
using DPK.Mosavabat.Services.Contracts.CreditProcesses;
using DPK.Mosavabat.Shared.DataModels.Mosavabat;
using DPK.Mosavabat.Shared.EntitiesDto.CreditProcesses.Abrisham.Loan;
using DPK.Mosavabat.Shared.EntitiesDto.CreditProcesses.PaymentPermission;
using DPK.Mosavabat.Shared.EntitiesDto.Mosavabat;
using DPK.Mosavabat.Shared.ViewModels.CreditProcesses.PaymentPermission;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace DPK.Mosavabat.Services.Helper;

public class LoanBillProcessingService : ILoanBillProcessingService
{
    private readonly ICreditProcessesDatabaseService _creditProcessesDatabaseService;
    private readonly IRequestFacilityDatabaseService _requestFacilityDatabaseService;
    private readonly ILoanBillRequestRepository _loanBillRequestRepository;
    private readonly ILoanBillDataRepository _loanBillDataRepository;
    private readonly IMemoryCache _cache;
    private readonly TimeSpan _cacheExpiry = TimeSpan.FromMinutes(10);

    public LoanBillProcessingService(
        ICreditProcessesDatabaseService creditProcessesDatabaseService,
        IRequestFacilityDatabaseService requestFacilityDatabaseService,
        ILoanBillRequestRepository loanBillRequestRepository,
        ILoanBillDataRepository loanBillDataRepository,
        IMemoryCache cache)
    {
        _creditProcessesDatabaseService = creditProcessesDatabaseService;
        _requestFacilityDatabaseService = requestFacilityDatabaseService;
        _loanBillRequestRepository = loanBillRequestRepository;
        _loanBillDataRepository = loanBillDataRepository;
        _cache = cache;
    }

    public async Task InitiateLoanBillProcessing(
        string customerNo,
        string mosavabeNo,
        CancellationToken cancellationToken)
    {
        var hasRecord = await _creditProcessesDatabaseService.HasRecordTashilatVaziat(cancellationToken);

        if (!hasRecord)
        {
            throw new LoanBillDataNotReadyException();
        }

        var facilities = await _creditProcessesDatabaseService
            .GetMosavabeContracts(
                new GetMosavabeContractsVm()
                {
                    MosavabeNo = mosavabeNo
                }, cancellationToken);

        if (facilities.Count > 0)
        {
            var hasMissingUpdateInfo = facilities
            .Any(f => f.DateUpdate is 0 && f.TimeUpdate is 0);

            if (hasMissingUpdateInfo)
            {
                throw new LoanBillDataNotReadyException();
            }
        }

        var cutoffIranTime = IranTimeHelper.ToIranTimeNow().AddMinutes(-10);
        var cutoffUtcTime = TimeZoneInfo.ConvertTimeToUtc(cutoffIranTime, IranTimeHelper.IranTimeZone);

        var existingRequests = await _loanBillRequestRepository
            .FindByCondition(r => r.CustomerNo == customerNo &&
                                  !r.IsCompleted &&
                                  r.CreatedAt >= cutoffUtcTime,
                false)
            .ToListAsync(cancellationToken);

        var oldLoanBillData = await _loanBillDataRepository
            .FindByCondition(ld => ld.CreatedAt < cutoffUtcTime,
                false)
            .ToListAsync(cancellationToken);

        if (oldLoanBillData.Any())
        {
            _loanBillDataRepository.DeleteRange(oldLoanBillData);
            await _loanBillDataRepository.SaveChangesAsync(cancellationToken);
        }

        var existingLoanIds = existingRequests
            .Select(r => r.LoanId)
            .ToHashSet();

        var newFacilities = facilities
            .Where(f => !existingLoanIds
                .Contains(f.C13.ToString()))
            .ToList();

        if (!newFacilities.Any())
        {
            return;
        }

        var iranNow = IranTimeHelper.ToIranTimeNow();

        var requests = facilities
            .Select(f => new LoanBillRequest
            {
                CustomerNo = customerNo,
                LoanId = f.C13,
                IsCompleted = false,
                IsProcessing = false,
                CreatedAt = iranNow,
                RetryCount = 0,
                DateUpdate = f.DateUpdate,
                TimeUpdate = f.TimeUpdate
            }).ToList();

        await _loanBillRequestRepository.AddRangeAsync(requests, cancellationToken);
        await _loanBillRequestRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task InitiateLoanBillProcessing(
        string customerNo,
        CancellationToken cancellationToken)
    {
        var hasRecord = await _creditProcessesDatabaseService.HasRecordTashilatVaziat(cancellationToken);

        if (!hasRecord)
        {
            throw new LoanBillDataNotReadyException();
        }

        var facilities = await _creditProcessesDatabaseService
            .GetCustomerCurrentFacilities(new GetCustomerCurrentFacilitiesVm()
            {
                CustomerNo = customerNo
            }, cancellationToken);

        if (facilities.Count > 0)
        {
            var hasMissingUpdateInfo = facilities
            .Any(f => f.DateUpdate is 0 && f.TimeUpdate is 0);

            if (hasMissingUpdateInfo)
            {
                throw new LoanBillDataNotReadyException();
            }
        }

        var cutoffIranTime = IranTimeHelper.ToIranTimeNow().AddMinutes(-10);

        var existingRequests = await _loanBillRequestRepository
            .FindByCondition(r => r.CustomerNo == customerNo &&
                                  !r.IsCompleted &&
                                  r.CreatedAt >= cutoffIranTime, false)
            .ToListAsync(cancellationToken);

        var oldLoanBillData = await _loanBillDataRepository
            .FindByCondition(ld => ld.CreatedAt < cutoffIranTime, false)
            .ToListAsync(cancellationToken);

        if (oldLoanBillData.Any())
        {
            _loanBillDataRepository.DeleteRange(oldLoanBillData);
            await _loanBillDataRepository.SaveChangesAsync(cancellationToken);
        }

        var existingLoanIds = existingRequests
            .Select(r => r.LoanId)
            .ToHashSet();

        var newFacilities = facilities
            .Where(f => !existingLoanIds
                .Contains(f.FacilityNo.ToString()))
            .ToList();

        if (!newFacilities.Any())
        {
            return;
        }

        var iranNow = IranTimeHelper.ToIranTimeNow();

        var requests = facilities
            .Select(f => new LoanBillRequest
            {
                CustomerNo = customerNo,
                LoanId = f.FacilityNo.ToString(),
                IsCompleted = false,
                IsProcessing = false,
                CreatedAt = iranNow,
                RetryCount = 0,
                DateUpdate = f.DateUpdate,
                TimeUpdate = f.TimeUpdate,
            }).ToList();

        await _loanBillRequestRepository.AddRangeAsync(requests, cancellationToken);
        await _loanBillRequestRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task InitiateAllLoanBillProcessing(
        string customerNo,
        CancellationToken cancellationToken)
    {
        var hasRecord = await _creditProcessesDatabaseService.HasRecordTashilatVaziat(cancellationToken);

        if (!hasRecord)
        {
            throw new LoanBillDataNotReadyException();
        }

        var facilities = await _requestFacilityDatabaseService
            .GetCustomerDebtDetails(new GetCustomerDebtDetailsDataModel()
            {
                CustomerNumber = customerNo
            }, cancellationToken);

        if (facilities.Count > 0)
        {
            var hasMissingUpdateInfo = facilities
            .Any(f => f.DateUpdate is 0 && f.TimeUpdate is 0);

            if (hasMissingUpdateInfo)
            {
                throw new LoanBillDataNotReadyException();
            }
        }

        var cutoffIranTime = IranTimeHelper.ToIranTimeNow().AddMinutes(-10);

        var existingRequests = await _loanBillRequestRepository
            .FindByCondition(r => r.CustomerNo == customerNo &&
                                  !r.IsCompleted &&
                                  r.CreatedAt >= cutoffIranTime, false)
            .ToListAsync(cancellationToken);

        var oldLoanBillData = await _loanBillDataRepository
            .FindByCondition(ld => ld.CreatedAt < cutoffIranTime, false)
            .ToListAsync(cancellationToken);

        if (oldLoanBillData.Any())
        {
            _loanBillDataRepository.DeleteRange(oldLoanBillData);
            await _loanBillDataRepository.SaveChangesAsync(cancellationToken);
        }

        var existingLoanIds = existingRequests
            .Select(r => r.LoanId)
            .ToHashSet();

        var newFacilities = facilities
            .Where(f => !existingLoanIds
                .Contains(f.C13.ToString()))
            .ToList();

        if (!newFacilities.Any())
        {
            return;
        }

        var iranNow = IranTimeHelper.ToIranTimeNow();

        var requests = facilities
            .Select(f => new LoanBillRequest
            {
                CustomerNo = customerNo,
                LoanId = f.C13.ToString(),
                IsCompleted = false,
                IsProcessing = false,
                CreatedAt = iranNow,
                RetryCount = 0,
                DateUpdate = f.DateUpdate,
                TimeUpdate = f.TimeUpdate
            }).ToList();

        await _loanBillRequestRepository.AddRangeAsync(requests, cancellationToken);
        await _loanBillRequestRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task<(List<GetMosavabeContractsDto>?, List<GetLoanBillDto>)> GetProcessedContractsByMosavabeNo(
        string mosavabeNo,
        CancellationToken cancellationToken)
    {
        var cacheKey = $"ProcessedContractsByMosavabeNo_{mosavabeNo}";

        if (_cache.TryGetValue(cacheKey, out var cachedResult))
        {
            return ((List<GetMosavabeContractsDto>?, List<GetLoanBillDto>))cachedResult;
        }

        var facilities = await _creditProcessesDatabaseService
            .GetMosavabeContracts(
                new GetMosavabeContractsVm()
                {
                    MosavabeNo = mosavabeNo
                }, cancellationToken);

        var loanIds = facilities
            .Select(f => f.C13)
            .ToList();

        var loanBillDataList = await _loanBillDataRepository
            .GetProcessedLoanBillsAsync(
                loanIds,
                cancellationToken);

        var processedLoanBills = loanBillDataList
            .Select(x => JsonConvert.DeserializeObject<GetLoanBillDto>(x.JsonData))
            .ToList();

        var result = (facilities, processedLoanBills);

        _cache.Set(cacheKey, result, _cacheExpiry);

        return result!;
    }

    public async Task<(List<GetCustomerCurrentFacilityDto>?, List<GetLoanBillDto>)> GetProcessedContractsByCustomerNo(
        string customerNo,
        CancellationToken cancellationToken)
    {
        var cacheKey = $"ProcessedContractsByCustomerNo_{customerNo}";

        if (_cache.TryGetValue(cacheKey, out var cachedResult))
        {
            return ((List<GetCustomerCurrentFacilityDto>?, List<GetLoanBillDto>))cachedResult;
        }

        var facilities = await _creditProcessesDatabaseService
            .GetCustomerCurrentFacilities(new GetCustomerCurrentFacilitiesVm()
            {
                CustomerNo = customerNo
            }, cancellationToken);

        var loanIds = facilities
            .Select(f => f.FacilityNo.ToString())
            .ToList();

        var loanBillDataList = await _loanBillDataRepository
            .GetProcessedLoanBillsAsync(
                loanIds,
                cancellationToken);

        var processedLoanBills = loanBillDataList
            .Select(x => JsonConvert.DeserializeObject<GetLoanBillDto>(x.JsonData))
            .ToList();

        var result = (facilities, processedLoanBills);

        _cache.Set(cacheKey, result, _cacheExpiry);

        return result!;
    }

    public async Task<(List<GetCustomerDebtDetailsDto>, List<GetLoanBillDto>)> GetProcessedAllContractsByCustomerNo(
    string customerNo,
    CancellationToken cancellationToken)
    {
        var cacheKey = $"ProcessedAllContractsByCustomerNo_{customerNo}";

        if (_cache.TryGetValue(cacheKey, out var cachedResult))
        {
            return ((List<GetCustomerDebtDetailsDto>, List<GetLoanBillDto>))cachedResult;
        }

        var facilities = await _requestFacilityDatabaseService
            .GetCustomerDebtDetails(new GetCustomerDebtDetailsDataModel()
            {
                CustomerNumber = customerNo
            }, cancellationToken);

        var loanIds = facilities
            .Select(f => f.C13.ToString())
            .ToList();

        var loanBillDataList = await _loanBillDataRepository
            .GetProcessedLoanBillsAsync(
                loanIds,
                cancellationToken);

        var processedLoanBills = loanBillDataList
            .Select(x => JsonConvert.DeserializeObject<GetLoanBillDto>(x.JsonData))
            .ToList();

        var result = (facilities, processedLoanBills);

        _cache.Set(cacheKey, result, _cacheExpiry);

        return result!;
    }

    public (List<GetCustomerCurrentFacilityDto>, List<GetLoanBillDto>)? TryGetFromCacheByCustomerNo(string cacheKey,
        CancellationToken cancellationToken)
    {
        var cachedObj = _cache.Get(cacheKey);
        return cachedObj as (List<GetCustomerCurrentFacilityDto>, List<GetLoanBillDto>)?;
    }

    public (List<GetMosavabeContractsDto>, List<GetLoanBillDto>)? TryGetFromCacheByMosavabeNo(string cacheKey,
        CancellationToken cancellationToken)
    {
        var cachedObj = _cache.Get(cacheKey);
        return cachedObj as (List<GetMosavabeContractsDto>, List<GetLoanBillDto>)?;
    }

    public (List<GetCustomerDebtDetailsDto>, List<GetLoanBillDto>)? TryGetCustomerDebtDetailsFromCacheByCustomerNo(string cacheKey,
       CancellationToken cancellationToken)
    {
        var cachedObj = _cache.Get(cacheKey);
        return cachedObj as (List<GetCustomerDebtDetailsDto>, List<GetLoanBillDto>)?;
    }

    public async Task WaitForProcessingToCompleteOrTimeout(
        string customerNo,
        TimeSpan timeout,
        CancellationToken cancellationToken)
    {
        var startTime = IranTimeHelper.ToIranTimeNow();
        var delay = TimeSpan.FromMilliseconds(500);

        while (IranTimeHelper.ToIranTimeNow() - startTime < timeout)
        {
            var cutoffIranTime = IranTimeHelper
                .ToIranTimeNow()
                .AddMinutes(-10);

            var cutoffUtcTime = TimeZoneInfo
                .ConvertTimeToUtc(
                    cutoffIranTime,
                    IranTimeHelper.IranTimeZone);

            var isProcessing = await _loanBillRequestRepository
                .FindByCondition(r =>
                        r.CustomerNo == customerNo &&
                        !r.IsCompleted &&
                        r.CreatedAt >= cutoffUtcTime,
                    true)
                .AnyAsync(cancellationToken);

            if (!isProcessing)
            {
                return;
            }

            await Task.Delay(delay, cancellationToken);
        }
    }
}