using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DPK.Mosavabat.BusinessServices.Services.LoanBills.LoanBillProcessing.Contract;
using DPK.Mosavabat.Common.Helpers;
using DPK.Mosavabat.DomainEntities.LoanBill.Entities;
using DPK.Mosavabat.DomainEntities.LoanBill.Repositories;
using DPK.Mosavabat.Models.DataModels.CreditProcesses;
using DPK.Mosavabat.Models.EntitiesDto.CreditProcesses;
using DPK.Mosavabat.Models.EntitiesDto.LoanBills;
using DPK.Mosavabat.SqlBusinessServices.CreditProcesses.Contract;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace DPK.Mosavabat.BusinessServices.Services.LoanBills.LoanBillProcessing.Service;

/// <summary>
/// سرویس پردازش صورتحساب تسهیلات
/// </summary>
/// <param name="loanBillRequestRepository"></param>
/// <param name="loanBillDataRepository"></param>
/// <param name="creditProcessSqlService"></param>
/// <param name="cache"></param>
internal class LoanBillProcessingService(
    ILoanBillRequestRepository loanBillRequestRepository,
    ILoanBillDataRepository loanBillDataRepository,
    ICreditProcessSqlService creditProcessSqlService,
    IMemoryCache cache)
    : ILoanBillProcessingService
{
    public async Task InitiateLoanBillProcessing(
        string customerNo,
        string mosavabeNo,
        CancellationToken cancellationToken)
    {
        var facilities = await creditProcessSqlService
            .GetMosavabeContracts(
                new GetMosavabeContractsDm(
                    mosavabeNo),
                cancellationToken);

        var cutoffIranTime = IranTimeHelper
            .ToIranTimeNow()
            .AddMinutes(-10);

        var cutoffUtcTime = TimeZoneInfo
            .ConvertTimeToUtc(
                cutoffIranTime,
                IranTimeHelper.IranTimeZone);

        var existingRequests = await loanBillRequestRepository
            .GetActiveRequestsByCustomerAsync(
                customerNo,
                cutoffUtcTime,
                cancellationToken);

        var oldLoanBillData = await loanBillDataRepository
            .GetOldLoanBillsAsync(
                cutoffUtcTime,
                cancellationToken);

        if (oldLoanBillData.Count != 0)
        {
            await loanBillDataRepository
                .DeleteRangeAsync(
                    oldLoanBillData,
                    cancellationToken);
        }

        var existingLoanIds = existingRequests
            .Select(r => r.LoanId)
            .ToHashSet();

        var newFacilities = facilities
            .Where(f => !existingLoanIds
                .Contains(f.C13.ToString()))
            .ToList();

        if (newFacilities.Count == 0)
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
                RetryCount = 0
            }).ToList();

        await loanBillRequestRepository.AddRangeAsync(requests, cancellationToken);
    }

    public async Task InitiateLoanBillProcessing(
        string customerNo,
        CancellationToken cancellationToken)
    {
        var facilities = await creditProcessSqlService
            .GetCustomerCurrentFacilities(
                new GetCustomerCurrentFacilitiesDm(
                    customerNo),
                cancellationToken);

        var cutoffUtcTime = IranTimeHelper.ToIranTimeNow().AddMinutes(-10);

        var existingRequests = await loanBillRequestRepository
            .GetActiveRequestsByCustomerAsync(
                customerNo,
                cutoffUtcTime,
                cancellationToken);

        var oldLoanBillData = await loanBillDataRepository
            .GetOldLoanBillsAsync(
                cutoffUtcTime,
                cancellationToken);

        if (oldLoanBillData.Count != 0)
        {
            await loanBillDataRepository
                .DeleteRangeAsync(
                    oldLoanBillData,
                    cancellationToken);
        }

        var existingLoanIds = existingRequests
            .Select(r => r.LoanId)
            .ToHashSet();

        var newFacilities = facilities
            .Where(f => !existingLoanIds
                .Contains(f.FacilityNo.ToString()))
            .ToList();

        if (newFacilities.Count == 0)
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
                RetryCount = 0
            }).ToList();

        await loanBillRequestRepository
            .AddRangeAsync(
                requests,
                cancellationToken);
    }

    public async Task<(List<MosavabeContractsDto>, List<GetLoanBillDto>)> GetProcessedContractsByMosavabeNo(
        string mosavabeNo,
        CancellationToken cancellationToken)
    {
        var cacheKey = $"ProcessedContractsByMosavabeNo_{mosavabeNo}";

        if (cache.TryGetValue(cacheKey, out var cachedResult))
        {
            return ((List<MosavabeContractsDto>, List<GetLoanBillDto>))cachedResult;
        }

        var facilities = await creditProcessSqlService
            .GetMosavabeContracts(
                new GetMosavabeContractsDm(
                    mosavabeNo),
                cancellationToken);

        var loanIds = facilities
            .Select(f => f.C13)
            .ToList();

        var loanBillDataList = await loanBillDataRepository
            .GetProcessedLoanBillsAsync(
                loanIds,
                cancellationToken);

        var processedLoanBills = loanBillDataList
            .Select(x => JsonConvert.DeserializeObject<GetLoanBillDto>(x.JsonData))
            .ToList();

        var result = (facilities, processedLoanBills);

        cache.Set(cacheKey, result, TimeSpan.FromMinutes(10));

        return result!;
    }

    public async Task<(List<CustomerCurrentFacilityDto>, List<GetLoanBillDto>)> GetProcessedContractsByCustomerNo(
        string customerNo,
        CancellationToken cancellationToken)
    {
        var cacheKey = $"ProcessedContractsByCustomerNo_{customerNo}";

        if (cache.TryGetValue(cacheKey, out var cachedResult))
        {
            return ((List<CustomerCurrentFacilityDto>, List<GetLoanBillDto>))cachedResult;
        }

        var facilities = await creditProcessSqlService
            .GetCustomerCurrentFacilities(
                new GetCustomerCurrentFacilitiesDm(
                    customerNo),
                cancellationToken);

        var loanIds = facilities
            .Select(f => f.FacilityNo.ToString())
            .ToList();

        var loanBillDataList = await loanBillDataRepository
            .GetProcessedLoanBillsAsync(
                loanIds,
                cancellationToken);

        var processedLoanBills = loanBillDataList
            .Select(x => JsonConvert.DeserializeObject<GetLoanBillDto>(x.JsonData))
            .ToList();

        var result = (facilities, processedLoanBills);

        cache.Set(cacheKey, result, TimeSpan.FromMinutes(10));

        return result!;
    }

    public (List<CustomerCurrentFacilityDto>, List<GetLoanBillDto>)? TryGetFromCacheByCustomerNo(
        string cacheKey,
        CancellationToken cancellationToken)
    {
        var cachedObj = cache.Get(cacheKey);
        return cachedObj as (List<CustomerCurrentFacilityDto>, List<GetLoanBillDto>)?;
    }

    public (List<MosavabeContractsDto>, List<GetLoanBillDto>)? TryGetFromCacheByMosavabeNo(
        string cacheKey,
        CancellationToken cancellationToken)
    {
        var cachedObj = cache.Get(cacheKey);
        return cachedObj as (List<MosavabeContractsDto>, List<GetLoanBillDto>)?;
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

            var isProcessing = await loanBillRequestRepository
                .GetIsProcessingRequestsAsync(
                    customerNo,
                    cutoffUtcTime,
                    cancellationToken);

            if (!isProcessing)
            {
                return;
            }

            await Task
                .Delay(
                    delay,
                    cancellationToken);
        }
    }
}