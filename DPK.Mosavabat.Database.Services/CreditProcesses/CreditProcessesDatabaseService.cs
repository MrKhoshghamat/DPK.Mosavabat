using System.Data;
using DPK.Mosavabat.Contracts;
using DPK.Mosavabat.Database.Services.Contracts.CreditProcesses;
using DPK.Mosavabat.Database.Services.Database;
using DPK.Mosavabat.Shared.DataModels.Lms;
using DPK.Mosavabat.Shared.DataTransferObjects.Lms;
using DPK.Mosavabat.Shared.EntitiesDto.CreditProcesses.PaymentPermission;
using DPK.Mosavabat.Shared.ViewModels.CreditProcesses;
using DPK.Mosavabat.Shared.ViewModels.CreditProcesses.PaymentPermission;
using DPK.Mosavabat.Shared.ViewModels.Database;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PaymentPermissionIssuanceControlDto = DPK.Mosavabat.Shared.EntitiesDto.CreditProcesses.PaymentPermission.PaymentPermissionIssuanceControlDto;

namespace DPK.Mosavabat.Database.Services.CreditProcesses
{
    public class CreditProcessesDatabaseService : MainDatabaseService, ICreditProcessesDatabaseService
    {
        private readonly ILoggerManager _logger;

        public CreditProcessesDatabaseService(
            ILoggerManager logger,
            IConfiguration configuration) : base(configuration)
        {
            _logger = logger;
        }

        public async Task<PaymentPermissionIssuanceControlDto?> PaymentPermissionIssuanceControl(PaymentPermissionIssuanceControlVm viewModel,
            CancellationToken cancellationToken)
        {
            var parameters = new List<SqlParameter>
            {
                new("@MosavabeNumber", SqlDbType.VarChar) {Value = viewModel.MosavabeNo},
                new("@ContractAmount", SqlDbType.BigInt) {Value = viewModel.ContractAmount},
                new("@PrepayAmount", SqlDbType.BigInt) {Value = viewModel.CustomerSharePrice},
                new("@SubContractType", SqlDbType.Int) {Value = viewModel.SubContractType},
                new("@CustomerNo", SqlDbType.VarChar) {Value = viewModel.CustomerNo}
            };

            var result = await ExecuteScalarAsync(new ExecuteScalarAsyncVm()
            {
                ConnectionString = DevProviderConnectionString,
                StoredProcedureName = "[CP].[PaymentPermissionIssuanceControl]",
                Parameters = parameters
            }, cancellationToken);

            _logger.Trace(JsonConvert.SerializeObject(result));

            return new PaymentPermissionIssuanceControlDto() { Result = result!.Result!.ToString() };
        }

        public async Task<CheckAndUpdateC13InPaymentPermissionDto?> CheckAndUpdateC13InPaymentPermission(CheckAndUpdateC13InPaymentPermissionVm viewModel,
            CancellationToken cancellationToken)
        {
            var parameters = new List<SqlParameter>
            {
                new("@CustomerNo", SqlDbType.NVarChar) {Value = viewModel.CustomerNo},
                new("@BranchCode", SqlDbType.Int) {Value = viewModel.BranchCode},
                new("@ContractType", SqlDbType.Int) {Value = viewModel.ContractType},
                new("@SubContractType", SqlDbType.Int) {Value = viewModel.SubContractType},
                new("@ContractAmount", SqlDbType.BigInt) {Value = viewModel.ContractAmount},
                new("@PrepaidAmount", SqlDbType.BigInt) {Value = viewModel.PrepaidAmount},
                new("@C13", SqlDbType.BigInt) {Value = viewModel.C13}
            };

            var result = await ExecuteScalarAsync(new ExecuteScalarAsyncVm()
            {
                ConnectionString = DevProviderConnectionString,
                StoredProcedureName = "[CP].[CheckAndUpdateC13InPaymentPermission]",
                Parameters = parameters
            }, cancellationToken);

            _logger.Trace(JsonConvert.SerializeObject(result));

            return new CheckAndUpdateC13InPaymentPermissionDto() { Result = result!.Result!.ToString() };
        }

        public async Task<List<GetTermsAndConditionsApprovalRequestDto>?> GetTermsAndConditionsApprovalRequests(GetTermsAndConditionsApprovalRequestListVm viewModel,
            CancellationToken cancellationToken)
        {
            var parameters = new List<SqlParameter>
            {
                new("@MosavabeNumber", SqlDbType.VarChar) {Value = viewModel.MosavabeNumber}
            };

            var result =
                await ExecuteAsListAsync<GetTermsAndConditionsApprovalRequestDto>(new ExecuteAsListAsyncVm()
                {
                    ConnectionString = DevProviderConnectionString,
                    StoredProcedureName = "[CP].[GetListTermsAndConditionsApprovalRequest]",
                    Parameters = parameters
                }, cancellationToken);

            _logger.Trace(JsonConvert.SerializeObject(result));

            return result.List;
        }

        public async Task<InsertPaymentPermissionDto?> InsertPaymentPermission(InsertPaymentPermissionVm viewModel, CancellationToken cancellationToken)
        {
            var parameters = new List<SqlParameter>
            {
                new("@Code", SqlDbType.NVarChar) {Value = viewModel.Code},
                new("@MosavabeNo", SqlDbType.NVarChar) {Value = viewModel.MosavabeNo},
                new("@BranchCode", SqlDbType.NVarChar) {Value = viewModel.BranchCode},
                new("@CustomerNo", SqlDbType.NVarChar) {Value = viewModel.CustomerNo},
                new("@ContractType", SqlDbType.NVarChar) {Value = viewModel.ContractType},
                new("@SubContractType", SqlDbType.NVarChar) {Value = viewModel.SubContractType},
                new("@ContractAmount", SqlDbType.Decimal) {Value = viewModel.ContractAmount},
                new("@PrepaidAmount", SqlDbType.Decimal) {Value = viewModel.PrepaidAmount}
            };

            var result = await ExecuteScalarAsync(new ExecuteScalarAsyncVm()
            {
                ConnectionString = MosavabatApiConnectionString,
                StoredProcedureName = "[CP].[InsertPaymentPermission]",
                Parameters = parameters
            }, cancellationToken);

            _logger.Trace(JsonConvert.SerializeObject(result));

            return new InsertPaymentPermissionDto() { Result = result!.Result!.ToString() };
        }

        public async Task<CalculateApprovalRequestUnusedBalanceDto?> CalculateApprovalRequestUnusedBalance(CalculateApprovalRequestUnusedBalanceVm viewModel,
            CancellationToken cancellationToken)
        {
            var parameters = new List<SqlParameter>
            {
                new("@MosavabeNo", SqlDbType.VarChar) {Value = viewModel.MosavabeNumber}
            };

            var result = await ExecuteReaderAsync(new ExecuteReaderAsyncVm()
            {
                ConnectionString = DevProviderConnectionString,
                StoredProcedureName = "[CP].[CalculateApprovalRequestUnusedBalance_V2]",
                Parameters = parameters
            }, cancellationToken);

            _logger.Trace(JsonConvert.SerializeObject(result));

            return new CalculateApprovalRequestUnusedBalanceDto() { Amount = result.Result!.ToString(), RemainingContractsCount = result.Column2! == null ? null : Convert.ToInt64(result.Column2!) };
        }

        public async Task<CalculateCustomerDebtBalanceDto?> CalculateCustomerDebtBalance(CalculateCustomerDebtBalanceVm viewModel, CancellationToken cancellationToken)
        {
            var parameters = new List<SqlParameter>
            {
                new("@MosavabeNumber", SqlDbType.NVarChar) {Value = viewModel.MosavabeNumber}
            };

            var result = await ExecuteScalarAsync(new ExecuteScalarAsyncVm()
            {
                ConnectionString = DevProviderConnectionString,
                StoredProcedureName = "[CP].[CalculationCustomerDebtBalance]",
                Parameters = parameters
            }, cancellationToken);

            _logger.Trace(JsonConvert.SerializeObject(result));

            return new CalculateCustomerDebtBalanceDto() { Result = result!.Result!.ToString() };
        }

        public async Task<UpdatePaymentPermissionDto?> UpdatePaymentPermission(UpdatePaymentPermissionVm viewModel, CancellationToken cancellationToken)
        {
            var parameters = new List<SqlParameter>
            {
                new("@MosavabeNumber", SqlDbType.NVarChar) {Value = viewModel.MosavabeNo},
                new("@IsCancel", SqlDbType.Bit) {Value = viewModel.IsCancel},
                new("@IsDelete", SqlDbType.Bit) {Value = viewModel.IsDelete},
            };

            var result = await ExecuteScalarAsync(new ExecuteScalarAsyncVm()
            {
                ConnectionString = MosavabatApiConnectionString,
                StoredProcedureName = "[CP].[UpdatePaymentPermission]",
                Parameters = parameters
            }, cancellationToken);

            _logger.Trace(JsonConvert.SerializeObject(result));
            return new UpdatePaymentPermissionDto() { Result = result!.Result!.ToString() };
        }

        public async Task<List<GetPaymentPermissionDto>?> GetPaymentPermissions(GetPaymentPermissionListVm viewModel, CancellationToken cancellationToken)
        {
            var parameters = new List<SqlParameter>
            {
                new("@MosavabeNumber", SqlDbType.NVarChar) {Value = viewModel.MosavabeNumber}
            };

            var result = await ExecuteAsListAsync<GetPaymentPermissionDto>(new ExecuteAsListAsyncVm()
            {
                ConnectionString = MosavabatApiConnectionString,
                StoredProcedureName = "[CP].[GetListOfPaymentPermission]",
                Parameters = parameters
            }, cancellationToken);

            _logger.Trace(JsonConvert.SerializeObject(result));

            return result.List;
        }

        public async Task<List<GetCustomerCurrentFacilityDto>> GetCustomerCurrentFacilities(GetCustomerCurrentFacilitiesVm viewModel, CancellationToken cancellationToken)
        {
            var parameters = new List<SqlParameter>
            {
                new("@CustomerNo", SqlDbType.NVarChar) { Value = viewModel.CustomerNo }
            };

            var result = await ExecuteAsListAsync<GetCustomerCurrentFacilityDto>(new ExecuteAsListAsyncVm()
            {
                ConnectionString = DevProviderConnectionString,
                StoredProcedureName = "[CP].[GetCurrentFacilities]",
                Parameters = parameters
            }, cancellationToken);

            _logger.Trace(JsonConvert.SerializeObject(result));

            return result.List;
        }

        public async Task<List<GetCustomerCurrentLetterCreditDto>> GetCustomerCurrentLetterCredits(GetCustomerCurrentLetterCreditsVm viewModel, CancellationToken cancellationToken)
        {
            var parameters = new List<SqlParameter>
            {
                new("@CustomerNo", SqlDbType.NVarChar) { Value = viewModel.CustomerNo }
            };

            var result = await ExecuteAsListAsync<GetCustomerCurrentLetterCreditDto>(new ExecuteAsListAsyncVm()
            {
                ConnectionString = DevProviderConnectionString,
                StoredProcedureName = "[CP].[GetCurrentLetterCredits]",
                Parameters = parameters
            }, cancellationToken);

            _logger.Trace(JsonConvert.SerializeObject(result));

            return result.List;
        }

        public async Task<List<GetCustomerCurrentCreditCardDto>> GetCustomerCurrentCreditCards(GetCustomerCurrentCreditCardsVm viewModel, CancellationToken cancellationToken)
        {
            var parameters = new List<SqlParameter>
            {
                new("@CustomerNo", SqlDbType.NVarChar) { Value = viewModel.CustomerNo }
            };

            var result = await ExecuteAsListAsync<GetCustomerCurrentCreditCardDto>(new ExecuteAsListAsyncVm()
            {
                ConnectionString = DevProviderConnectionString,
                StoredProcedureName = "[CP].[GetCustomerCurrentCreditCards]",
                Parameters = parameters
            }, cancellationToken);

            _logger.Trace(JsonConvert.SerializeObject(result));

            return result.List;
        }

        public async Task<List<GetCurrentFacilityReportDto>> GetCurrentFacilitiesReport(GetCurrentFacilitiesReportVm viewModel, CancellationToken cancellationToken)
        {
            var parameters = new List<SqlParameter>
            {
                new("@StartDate", SqlDbType.Int) { Value = viewModel.StartDate },
                new("@EndDate", SqlDbType.Int) { Value = viewModel.EndDate }
            };

            var result = await ExecuteAsListAsync<GetCurrentFacilityReportDto>(new ExecuteAsListAsyncVm()
            {
                ConnectionString = DevProviderConnectionString,
                StoredProcedureName = "[CP].[GetCurrentFacilitiesReport]",
                Parameters = parameters
            }, cancellationToken);

            _logger.Trace(JsonConvert.SerializeObject(result));

            var getCurrentFacilityReportDtoList = result.List.Select(dto => new GetCurrentFacilityReportDto() { C13 = dto.C13 }).ToList();

            return getCurrentFacilityReportDtoList;
        }

        public async Task<CheckExistActivePaymentPermissionDto?> CheckExistActivePaymentPermission(CheckExistActivePaymentPermissionVm viewModel, CancellationToken cancellationToken = default)
        {
            var parameters = new List<SqlParameter>
            {
                new("@MosavabeNo", SqlDbType.NVarChar) {Value = viewModel.MosavabeNo}
            };

            var result = await ExecuteScalarAsync(new ExecuteScalarAsyncVm()
            {
                ConnectionString = DevProviderConnectionString,
                StoredProcedureName = "[CP].[CheckExistActivePaymentPermission]",
                Parameters = parameters
            }, cancellationToken);

            _logger.Trace(JsonConvert.SerializeObject(result));
            return new CheckExistActivePaymentPermissionDto() { Result = (bool)result!.Result };
        }

        public async Task<List<GetCuContractDto>> GetCuContracts(GetCuContractsVm viewModel, CancellationToken cancellationToken)
        {
            var parameters = new List<SqlParameter>
            {
                new("@MosavabeNo", SqlDbType.Int) {Value = Convert.ToInt32(viewModel.MosavabeNo)}
            };

            var result =
                await ExecuteAsListAsync<GetCuContractDto>(new ExecuteAsListAsyncVm()
                {
                    ConnectionString = DevProviderConnectionString,
                    StoredProcedureName = "[CP].[GetCuContracts]",
                    Parameters = parameters
                }, cancellationToken);

            _logger.Trace(JsonConvert.SerializeObject(result));

            return result.List;
        }

        public async Task<GetAccNoByC13Dto> GetAccNoByC13(GetAccNoByC13Vm viewModel, CancellationToken cancellationToken = default)
        {
            var parameters = new List<SqlParameter>
            {
                new("@C13", SqlDbType.NVarChar) {Value = viewModel.C13}
            };

            var result = await ExecuteScalarAsync(new ExecuteScalarAsyncVm()
            {
                ConnectionString = DevProviderConnectionString,
                StoredProcedureName = "[CP].[GetAccNoByC13]",
                Parameters = parameters
            }, cancellationToken);

            _logger.Trace(JsonConvert.SerializeObject(result));

            return new GetAccNoByC13Dto() { AccNo = result!.Result!.ToString() };
        }

        public async Task<GetDateGByC13Dto> GetDateGByC13(GetDateGByC13Vm viewModel, CancellationToken cancellationToken = default)
        {
            var parameters = new List<SqlParameter>
            {
                new("@C13", SqlDbType.NVarChar) {Value = viewModel.C13}
            };

            var result = await ExecuteScalarAsync(new ExecuteScalarAsyncVm()
            {
                ConnectionString = DevProviderConnectionString,
                StoredProcedureName = "[CP].[GetDateGByC13]",
                Parameters = parameters
            }, cancellationToken);

            _logger.Trace(JsonConvert.SerializeObject(result));

            return new GetDateGByC13Dto() { Date_g = Convert.ToDateTime(result!.Result!) };
        }

        public async Task<GetC16ByC13Dto> GetC16ByC13(GetC16ByC13Vm viewModel, CancellationToken cancellationToken = default)
        {
            var parameters = new List<SqlParameter>
            {
                new("@C13", SqlDbType.NVarChar) {Value = viewModel.C13}
            };

            var result = await ExecuteScalarAsync(new ExecuteScalarAsyncVm()
            {
                ConnectionString = DevProviderConnectionString,
                StoredProcedureName = "[CP].[GetC16ByC13]",
                Parameters = parameters
            }, cancellationToken);

            _logger.Trace(JsonConvert.SerializeObject(result));

            return new GetC16ByC13Dto() { C16 = result!.Result!.ToString() };
        }

        public async Task<CalculateLiveApprovalRequestsAmountsSumDto> CalculateLiveApprovalRequestsAmountsSum(CalculateLiveApprovalRequestsAmountsSumVm viewModel, CancellationToken cancellationToken = default)
        {
            var parameters = new List<SqlParameter>
            {
                new("@CustNo", SqlDbType.VarChar) {Value = viewModel.CustNo}
            };

            var result = await ExecuteScalarAsync(new ExecuteScalarAsyncVm()
            {
                ConnectionString = DevProviderConnectionString,
                StoredProcedureName = "[dbo].[CalculateLiveApprovalRequestsAmountsSum]",
                Parameters = parameters
            }, cancellationToken);

            _logger.Trace(JsonConvert.SerializeObject(result));

            return new CalculateLiveApprovalRequestsAmountsSumDto() { ApprovalSubjectsAmountsSum = Convert.ToInt64(result!.Result!) };
        }

        public async Task<GetBranchCodeByC13Dto> GetBranchCodeByC13(GetBranchCodeByC13Vm viewModel, CancellationToken cancellationToken = default)
        {
            var parameters = new List<SqlParameter>
            {
                new("@C13", SqlDbType.NVarChar) {Value = viewModel.C13}
            };

            var result = await ExecuteScalarAsync(new ExecuteScalarAsyncVm()
            {
                ConnectionString = DevProviderConnectionString,
                StoredProcedureName = "[CP].[GetBranchCodeByC13]",
                Parameters = parameters
            }, cancellationToken);

            _logger.Trace(JsonConvert.SerializeObject(result));

            return new GetBranchCodeByC13Dto() { BranchCode = result!.Result!.ToString() };
        }

        public async Task<GetLiveApprovalRequestsAmountsSumDto> GetLiveApprovalRequestsAmountsSum(GetLiveApprovalRequestsAmountsSumVm viewModel, CancellationToken cancellationToken = default)
        {
            var parameters = new List<SqlParameter>
            {
                new("@CustNo", SqlDbType.VarChar) {Value = viewModel.CustNo},
                new("@IdApprovalRequest", SqlDbType.Int) {Value = viewModel.IdApprovalRequest}
            };

            var result = await ExecuteScalarAsync(new ExecuteScalarAsyncVm()
            {
                ConnectionString = DevProviderConnectionString,
                StoredProcedureName = "[dbo].[CalculateLiveApprovalRequestsAmountsSum]",
                Parameters = parameters
            }, cancellationToken);

            _logger.Trace(JsonConvert.SerializeObject(result));

            return new GetLiveApprovalRequestsAmountsSumDto() { ApprovalSubjectsAmountsSum = Convert.ToInt64(result!.Result!) };
        }

        public Task<GetLiveApprovalRequestsDto> CalculateLiveApprovalRequestsAmountsSum(GetLiveApprovalRequestsVm viewModel, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<CalculateDailyPaidContractsAmountsSumDto> CalculateDailyPaidContractsAmountsSum(CalculateDailyPaidContractsAmountsSumVm viewModel, CancellationToken cancellationToken = default)
        {
            var parameters = new List<SqlParameter>
            {
                new("@CustNo", SqlDbType.VarChar) {Value = viewModel.CustNo}
            };

            var result = await ExecuteScalarAsync(new ExecuteScalarAsyncVm()
            {
                ConnectionString = DevProviderConnectionString,
                StoredProcedureName = "[CP].[CalculateDailyPaidContractsAmountsSum]",
                Parameters = parameters
            }, cancellationToken);

            _logger.Trace(JsonConvert.SerializeObject(result));

            return new CalculateDailyPaidContractsAmountsSumDto() { DailyPaidContractsAmountsSum = Convert.ToInt64(result!.Result!) };
        }

        public async Task<IsLcpRequestDto> IsLcpRequest(IsLcpRequestVm viewModel, CancellationToken cancellationToken = default)
        {
            var parameters = new List<SqlParameter>
            {
                new("@MosavabeNo", SqlDbType.VarChar) {Value = viewModel.MosavabeNo}
            };

            var result = await ExecuteScalarAsync(new ExecuteScalarAsyncVm()
            {
                ConnectionString = DevProviderConnectionString,
                StoredProcedureName = "[CP].[IsLcpRequest]",
                Parameters = parameters
            }, cancellationToken);

            _logger.Trace(JsonConvert.SerializeObject(result));

            return new IsLcpRequestDto() { IsLcpRequest = Convert.ToBoolean(result!.Result!) };
        }

        public async Task<List<GetListOfExceptionPayReasonDto>> GetListOfExceptionPayReason(GetListOfExceptionPayReasonVM viewModel, CancellationToken cancellationToken)
        {
            var parameters = new List<SqlParameter>
            {
                new("@CustomerNo", SqlDbType.NVarChar) { Value = viewModel.CustomerNo },
            };

            var result = await ExecuteAsListAsync<GetListOfExceptionPayReasonDto>(new ExecuteAsListAsyncVm()
            {
                ConnectionString = DevProviderConnectionString,
                StoredProcedureName = "[CP].[GetListOfExceptionPayReason]",
                Parameters = parameters
            }, cancellationToken);

            _logger.Trace(JsonConvert.SerializeObject(result));

            return result.List;
        }

        public async Task<List<GetListOfPaymentPermissionExceptionReasonDto>?> GetListOfPaymentPermissionExceptionReason(GetListOfPaymentPermissionExceptionReasonVM viewModel,
            CancellationToken cancellationToken = default)
        {
            var parameters = new List<SqlParameter>
            {
                new("@CustomerNo", SqlDbType.NVarChar) { Value = viewModel.CustomerNo },
            };

            var result = await ExecuteAsListAsync<GetListOfPaymentPermissionExceptionReasonDto>(new ExecuteAsListAsyncVm()
            {
                ConnectionString = DevProviderConnectionString,
                StoredProcedureName = "[CP].[GetListOfPaymentPermissionExceptionReason]",
                Parameters = parameters
            }, cancellationToken);

            _logger.Trace(JsonConvert.SerializeObject(result));

            var getListOfPaymentPermissionExceptionReasonDto = result.List.Select(dto => new GetListOfPaymentPermissionExceptionReasonDto
            {
                Id = dto.Id,
                CustomerNumber = dto.CustomerNumber,
                BranchCode = dto.BranchCode,
                CreateDate = dto.CreateDate,
                ExpireDate = dto.ExpireDate,
                IssuanceLocation = dto.IssuanceLocation,
                IssuanceExceptionId = dto.IssuanceExceptionId,
                LetterDate = dto.LetterDate,
                LetterNumber = dto.LetterNumber,
                ExceptionCode = dto.ExceptionCode,
                RadNumber = dto.RadNumber,
                IsDeleted = dto.IsDeleted,
                Creator = dto.Creator,
                NationalCode = dto.NationalCode,
            }).ToList();

            return getListOfPaymentPermissionExceptionReasonDto;
        }

        public async Task<bool> AddToPaymentPermissionExceptionReason(PaymentPermissionExceptionReasonVM viewModel, CancellationToken cancellationToken = default)
        {
            var parameters = new List<SqlParameter>
            {
                new("@CustomerNumber", SqlDbType.NVarChar) {Value = viewModel.CustomerNumber},
                new("@BranchCode", SqlDbType.Int) {Value = viewModel.BranchCode},
                new("@CreateDate", SqlDbType.DateTime2) {Value = viewModel.CreateDate},
                new("@ExpireDate", SqlDbType.DateTime2) {Value = viewModel.ExpireDate},
                new("@IssuanceLocation", SqlDbType.Int) {Value = viewModel.IssuanceLocation},
                new("@IssuanceExceptionId", SqlDbType.Int) {Value = viewModel.IssuanceExceptionId},
                new("@LetterDate", SqlDbType.DateTime2) {Value = viewModel.LetterDate},
                new("@LetterNumber", SqlDbType.NVarChar) {Value = viewModel.LetterNumber},
                new("@ExceptionCode", SqlDbType.Int) {Value = viewModel.ExceptionCode},
                new("@RadNumber", SqlDbType.NVarChar) {Value = viewModel.RadNumber},
                new("@IsDeleted", SqlDbType.Bit) {Value = viewModel.IsDeleted},
                new("@Creator", SqlDbType.Int) {Value = viewModel.Creator},
                new("@NationalCode", SqlDbType.NVarChar) {Value = viewModel.NationalCode},
            };

            var result = await ExecuteNonQueryAsync(new ExecuteNonQueryAsyncVm()
            {
                ConnectionString = DevProviderConnectionString,
                StoredProcedureName = "[CP].[AddToPaymentPermissionExceptionReason]",
                Parameters = parameters
            }, cancellationToken);

            _logger.Trace(JsonConvert.SerializeObject(result));

            return (int)result.Result! == 1;
        }

        public async Task<bool> DeletePaymentPermissionExceptionReason(PaymentPermissionExceptionReasonForDeleteVM viewModel, CancellationToken cancellationToken = default)
        {
            var parameters = new List<SqlParameter>
            {
                new("@Id", SqlDbType.NVarChar) {Value = viewModel.Id},
            };

            var result = await ExecuteNonQueryAsync(new ExecuteNonQueryAsyncVm()
            {
                ConnectionString = DevProviderConnectionString,
                StoredProcedureName = "[CP].[DeletePaymentPermissionExceptionReason]",
                Parameters = parameters
            }, cancellationToken);

            _logger.Trace(JsonConvert.SerializeObject(result));

            return (int)result.Result! == 1;
        }

        public async Task<GetDailyPaidContractsDto> GetDailyPaidContracts(GetDailyPaidContractsVm viewModel, CancellationToken cancellationToken = default)
        {
            var parameters = new List<SqlParameter>
            {
                new("@CustNo", SqlDbType.VarChar) {Value = viewModel.CustNo}
            };

            var result = await ExecuteAsListAsync<DailyPaidContractDto>(new ExecuteAsListAsyncVm()
            {
                ConnectionString = DevProviderConnectionString,
                StoredProcedureName = "[CP].[GetDailyPaidContracts]",
                Parameters = parameters
            }, cancellationToken);

            _logger.Trace(JsonConvert.SerializeObject(result));

            return new GetDailyPaidContractsDto() { DailyPaidContracts = result.List };
        }

        public async Task<GetCustomerContractsPastDebtDto> GetCustomerContractsPastDebt(GetCustomerContractsPastDebtVm viewModel, CancellationToken cancellationToken = default)
        {
            var parameters = new List<SqlParameter>
            {
                new("@CustNo", SqlDbType.VarChar) {Value = viewModel.CustNo}
            };

            var result = await ExecuteAsListAsync<CustomerContractPastDebtDto>(new ExecuteAsListAsyncVm()
            {
                ConnectionString = DevProviderConnectionString,
                StoredProcedureName = "[CP].[GetCustomerContractsPastDebt]",
                Parameters = parameters
            }, cancellationToken);

            _logger.Trace(JsonConvert.SerializeObject(result));

            return new GetCustomerContractsPastDebtDto() { CustomerContractsPastDebts = result.List };
        }
        public async Task<List<GetListOfExceptionCodeByNationalCodeReasonDto>?> GetListOfExceptionCodeByNationalCode(GetListOfExceptionCodeByNationalCodeVM viewModel,
            CancellationToken cancellationToken = default)
        {
            var parameters = new List<SqlParameter>
            {
                new("@NationalCode", SqlDbType.NVarChar) { Value = viewModel.NationalCode },
            };

            var result = await ExecuteAsListAsync<GetListOfExceptionCodeByNationalCodeReasonDto>(new ExecuteAsListAsyncVm()
            {
                ConnectionString = DevProviderConnectionString,
                StoredProcedureName = "[CP].[GetListOfExceptionCodeByNationalCode]",
                Parameters = parameters
            }, cancellationToken);

            _logger.Trace(JsonConvert.SerializeObject(result));

            var getListOfPaymentPermissionExceptionReasonDto = result.List.Select(dto => new GetListOfExceptionCodeByNationalCodeReasonDto
            {
                ExceptionCode = dto.ExceptionCode,
            }).ToList();

            return getListOfPaymentPermissionExceptionReasonDto;
        }

        public async Task<GetSubjectTypeDto> GetSubjectType(GetSubjectTypeVm viewModel, CancellationToken cancellationToken = default)
        {
            var parameters = new List<SqlParameter>
            {
                new("@MosavabeNo", SqlDbType.VarChar) { Value = viewModel.MosavabeNo }
            };

            var result = await ExecuteScalarAsync(new ExecuteScalarAsyncVm()
            {
                ConnectionString = DevProviderConnectionString,
                StoredProcedureName = "[CP].[GetSubjectTypeByMosavabeNo]",
                Parameters = parameters
            }, cancellationToken);

            _logger.Trace(JsonConvert.SerializeObject(result));

            return new GetSubjectTypeDto() { SubjectType = Convert.ToInt32(result.Result) };
        }

        public async Task<GetApprovalRequestInfoDto> GetApprovalRequestInfo(GetApprovalRequestInfoVm viewModel, CancellationToken cancellationToken = default)
        {
            var parameters = new List<SqlParameter>
            {
                new("@MosavabeNo", SqlDbType.VarChar) { Value = viewModel.MosavabeNo }
            };

            var result = await ExecuteReaderAsync(new ExecuteReaderAsyncVm()
            {
                ConnectionString = DevProviderConnectionString,
                StoredProcedureName = "[CP].[GetApprovalRequestInfoByMosavabeNo]",
                Parameters = parameters
            }, cancellationToken);

            _logger.Trace(JsonConvert.SerializeObject(result));

            return new GetApprovalRequestInfoDto()
            {
                ApprovalRequestId = Convert.ToInt32(result.Result),
                CaseId = Convert.ToInt32(result.Column2),
                RadNumber = result.Column3 == null ? null : result.Column3.ToString()
            };
        }

        public async Task<GetCustomerNoByMosavabeNoDto> GetCustomerNoByMosavabeNo(GetCustomerNoByMosavabeNoDataModel dataModel, CancellationToken cancellationToken = default)
        {
            var parameters = new List<SqlParameter>
            {
                new("@mosavabeNo", SqlDbType.Int) { Value = dataModel.MosavabeNo }
            };

            var result = await ExecuteScalarAsync(new ExecuteScalarAsyncVm()
            {
                ConnectionString = DevProviderConnectionString,
                StoredProcedureName = "[CP].[GetGetCustomerNoByMosavabeNo]",
                Parameters = parameters
            }, cancellationToken);

            _logger.Trace(JsonConvert.SerializeObject(result));

            return new GetCustomerNoByMosavabeNoDto()
            {
                CustomerNo = result.Result.ToString()
            };
        }

        public async Task<List<GetMosavabeContractsDto>> GetMosavabeContracts(GetMosavabeContractsVm viewModel, CancellationToken cancellationToken)
        {
            var parameters = new List<SqlParameter>
            {
                new("@mosavabeNo", SqlDbType.Int) { Value = viewModel.MosavabeNo }
            };

            var result = await ExecuteAsListAsync<GetMosavabeContractsDto>(new ExecuteAsListAsyncVm()
            {
                ConnectionString = DevProviderConnectionString,
                StoredProcedureName = "[CP].[GetMosavabeContracts]",
                Parameters = parameters
            }, cancellationToken);

            _logger.Trace(JsonConvert.SerializeObject(result));

            return result.List;
        }

        public async Task<IsAllowedRecalculateAccountBalanceAverageDto> IsAllowedRecalculateAccountBalanceAverage(IsAllowedRecalculateAccountBalanceAverageVm viewModel, CancellationToken cancellationToken)
        {
            var parameters = new List<SqlParameter>
            {
                new("@mosavabeNo", SqlDbType.VarChar) { Value = viewModel.MosavabeNo },
                new("@contractAmount", SqlDbType.BigInt) { Value = viewModel.MosavabeNo }
            };

            var result = await ExecuteScalarAsync(new ExecuteScalarAsyncVm()
            {
                ConnectionString = DevProviderConnectionString,
                StoredProcedureName = "[CP].[IsAllowedRecalculateAccountBalanceAverage]",
                Parameters = parameters
            }, cancellationToken);

            return new IsAllowedRecalculateAccountBalanceAverageDto() { IsAllowed = Convert.ToBoolean(result.Result) };
        }

        public async Task<List<ContractDebtDto>> GetContractsDebtAmounts(List<string> contractIds, CancellationToken cancellationToken)
        {
            if (contractIds == null || !contractIds.Any())
            {
                return new List<ContractDebtDto>();
            }

            var commaSeparatedIds = string.Join(",", contractIds);

            var parameters = new List<SqlParameter>
            {
                new("@ContractIdsList", SqlDbType.NVarChar) 
                {
                    Value = commaSeparatedIds
                }
            };

            var result =
                await ExecuteAsListAsync<ContractDebtDto>(new ExecuteAsListAsyncVm()
                {
                    ConnectionString = DevProviderConnectionString,
                    StoredProcedureName = "[dbo].[GetContractsDebtAmounts]",
                    Parameters = parameters
                }, cancellationToken);

            _logger.Trace(JsonConvert.SerializeObject(result));

            return result.List;
        }

        public async Task<GetRequestFacilityDetailsDto> GetRequestFacilityDetails(GetRequestFacilityDetailsVm viewModel, CancellationToken cancellationToken)
        {
            var parameters = new List<SqlParameter>
            {
                new("@CaseId", SqlDbType.Int) { Value = viewModel.CaseId }
            };

            var result = await ExecuteReaderAsync(new ExecuteReaderAsyncVm()
            {
                ConnectionString = DevProviderConnectionString,
                StoredProcedureName = "[CP].[GetRequestFacilityDetails]",
                Parameters = parameters
            }, cancellationToken);

            _logger.Trace(JsonConvert.SerializeObject(result));

            return new GetRequestFacilityDetailsDto()
            {
                BranchCode = Convert.ToInt32(result?.Result),
                BranchId = Convert.ToInt32(result?.Column2),
                Customer =  Convert.ToString(result?.Column3),
                MosavabeNo = Convert.ToInt32(result?.Column4)
            };
        }

        public async Task<string?> UpdateCaseIdAndRadNumberInIssuancePayment(UpdateCaseIdAndRadNumberInIssuancePaymentVm viewModel, CancellationToken cancellationToken = default)
        {
            var parameters = new List<SqlParameter>
            {
                new("@CaseId", SqlDbType.Int) {Value = viewModel.CaseId},
                new("@RadNumber", SqlDbType.Int) {Value = viewModel.RadNumber},
                new("@Guid", SqlDbType.NVarChar) {Value = viewModel.Guid}
            };

            var rowsAffected = await ExecuteNonQueryAsync(new ExecuteNonQueryAsyncVm()
            {
                ConnectionString = DevProviderConnectionString,
                StoredProcedureName = "[CP].[UpdateCaseIdAndRadNumberInIssuancePayment]",
                Parameters = parameters
            }, cancellationToken);

            _logger.Trace(JsonConvert.SerializeObject(rowsAffected));
            return rowsAffected.ToString();

        }
    }
}
