using System.Data;
using DPK.Mosavabat.Database.Services.Contracts.Database;
using DPK.Mosavabat.Shared.EntitiesDto.Database;
using DPK.Mosavabat.Shared.ViewModels.Database;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace DPK.Mosavabat.Database.Services.Database
{
    public class MainDatabaseService : IMainDatabaseService
    {
        public string MosavabatConnectionString { get; set; }
        public string DevProviderConnectionString { get; set; }
        public string MosavabatApiConnectionString { get; set; }

        public MainDatabaseService(IConfiguration configuration)
        {
            MosavabatConnectionString = configuration["ConnectionStrings:MosavabatConnectionString"];
            DevProviderConnectionString = configuration["ConnectionStrings:DevProviderConnectionString"];
            MosavabatApiConnectionString = configuration["ConnectionStrings:MosavabatApiConnectionString"];
        }

        public async Task<ExecuteScalarAsyncDto> ExecuteScalarAsync(ExecuteScalarAsyncVm viewModel, CancellationToken cancellationToken)
        {
            await using var connection = new SqlConnection(viewModel.ConnectionString);

            await using var command = connection.CreateCommand();

            command.CommandText = viewModel.StoredProcedureName;

            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddRange(viewModel.Parameters.ToArray());

            if (connection.State != ConnectionState.Open)
                await connection.OpenAsync(cancellationToken);

            return new ExecuteScalarAsyncDto() { Result = await command.ExecuteScalarAsync(cancellationToken) };
        }

        public async Task<ExecuteReaderAsyncDto> ExecuteReaderAsync(ExecuteReaderAsyncVm viewModel, CancellationToken cancellationToken)
        {
            await using var connection = new SqlConnection(viewModel.ConnectionString);

            await using var command = connection.CreateCommand();

            command.CommandText = viewModel.StoredProcedureName;
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddRange(viewModel.Parameters.ToArray());

            if (connection.State != ConnectionState.Open)
                await connection.OpenAsync(cancellationToken);

            await using var reader = await command.ExecuteReaderAsync(cancellationToken);

            var dto = new ExecuteReaderAsyncDto
            {
                Result = null,
                Column2 = null,
                Column3 = null,
                Column4 = null
            };

            if (!await reader.ReadAsync(cancellationToken)) return dto;

            if (reader.FieldCount == 1)
            {
                dto.Result = reader.IsDBNull(0) ? null : reader.GetValue(0);
            }

            if (reader.FieldCount == 2)
            {
                dto.Result = reader.IsDBNull(0) ? null : reader.GetValue(0);
                dto.Column2 = reader.IsDBNull(1) ? null : reader.GetValue(1);
            }

            if (reader.FieldCount == 3)
            {
                dto.Result = reader.IsDBNull(0) ? null : reader.GetValue(0);
                dto.Column2 = reader.IsDBNull(1) ? null : reader.GetValue(1);
                dto.Column3 = reader.IsDBNull(2) ? null : reader.GetValue(2);
            }

            if (reader.FieldCount == 4)
            {
                dto.Result = reader.IsDBNull(0) ? null : reader.GetValue(0);
                dto.Column2 = reader.IsDBNull(1) ? null : reader.GetValue(1);
                dto.Column3 = reader.IsDBNull(2) ? null : reader.GetValue(2);
                dto.Column4 = reader.IsDBNull(3) ? null : reader.GetValue(3);
            }

            return dto;
        }

        public async Task<ExecuteNonQueryAsyncDto> ExecuteNonQueryAsync(ExecuteNonQueryAsyncVm viewModel, CancellationToken cancellationToken)
        {
            await using var connection = new SqlConnection(viewModel.ConnectionString);

            await using var command = connection.CreateCommand();

            command.CommandText = viewModel.StoredProcedureName;

            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddRange(viewModel.Parameters.ToArray());

            if (connection.State != ConnectionState.Open)
                await connection.OpenAsync(cancellationToken);

            return new ExecuteNonQueryAsyncDto() { Result = await command.ExecuteNonQueryAsync(cancellationToken) };
        }

        public async Task<ExecuteAsListAsyncDto<T>> ExecuteAsListAsync<T>(ExecuteAsListAsyncVm viewModel, CancellationToken cancellationToken) where T : new()
        {
            try
            {
                var resultList = new ExecuteAsListAsyncDto<T>()
                {
                    List = new List<T>()
                };

                await using var connection = new SqlConnection(viewModel.ConnectionString);

                await using var command = new SqlCommand(viewModel.StoredProcedureName, connection);

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddRange(viewModel.Parameters.ToArray());

                await connection.OpenAsync(cancellationToken);

                var reader = await command.ExecuteReaderAsync(cancellationToken);

                while (await reader.ReadAsync(cancellationToken))
                {
                    var item = new T();
                    foreach (var prop in typeof(T).GetProperties())
                    {
                        if (prop.CanWrite)
                            prop.SetValue(item, Convert.ChangeType(reader[prop.Name], prop.PropertyType));
                    }

                    resultList.List.Add(item);
                }

                return resultList;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
