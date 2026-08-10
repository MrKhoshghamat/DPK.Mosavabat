using DPK.Mosavabat.Shared.EntitiesDto.Database;
using DPK.Mosavabat.Shared.ViewModels.Database;

namespace DPK.Mosavabat.Database.Services.Contracts.Database
{
    public interface IMainDatabaseService
    {
        Task<ExecuteScalarAsyncDto> ExecuteScalarAsync(ExecuteScalarAsyncVm viewModel, CancellationToken cancellationToken);
        Task<ExecuteReaderAsyncDto> ExecuteReaderAsync(ExecuteReaderAsyncVm viewModel, CancellationToken cancellationToken);
        Task<ExecuteNonQueryAsyncDto> ExecuteNonQueryAsync(ExecuteNonQueryAsyncVm viewModel, CancellationToken cancellationToken);
        Task<ExecuteAsListAsyncDto<T>> ExecuteAsListAsync<T>(ExecuteAsListAsyncVm viewModel, CancellationToken cancellationToken) where T : new();
    }
}
