using DPK.Mosavabat.Contracts;
using DPK.Mosavabat.Database.Services.Contracts;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace DPK.Mosavabat.Database.Services;

public class MainDatabase : IMainDatabase
{
    public string MosavabatConnectionString { get; set; }
    public string DevProviderConnectionString { get; set; }
    public string MosavabatApiConnectionString { get; set; }

    public MainDatabase(IConfiguration configuration)
    {
        MosavabatConnectionString = configuration["ConnectionStrings:MosavabatConnectionString"];
        DevProviderConnectionString = configuration["ConnectionStrings:DevProviderConnectionString"];
        MosavabatApiConnectionString = configuration["ConnectionStrings:MosavabatApiConnectionString"];
    }

    public async Task<object?> ExecuteScalarAsync(string connectionString, string storedProcedureName, List<SqlParameter> parameters)
    {
        await using var connection = new SqlConnection(connectionString);

        await using var command = connection.CreateCommand();

        //command.CommandTimeout = 0;

        command.CommandText = storedProcedureName;

        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddRange(parameters.ToArray());

        if (connection.State != ConnectionState.Open)
            await connection.OpenAsync();

        return await command.ExecuteScalarAsync();
    }

    public async Task<int> ExecuteNonQueryAsync(string connectionString, string storedProcedureName, List<SqlParameter> parameters)
    {
        await using var connection = new SqlConnection(connectionString);

        await using var command = connection.CreateCommand();

        //command.CommandTimeout = 0;

        command.CommandText = storedProcedureName;

        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddRange(parameters.ToArray());

        if (connection.State != ConnectionState.Open)
            await connection.OpenAsync();

        return await command.ExecuteNonQueryAsync();
    }

    public async Task<List<T>?> ExecuteAsListAsync<T>(string connectionString, string storedProcedureName, List<SqlParameter> parameters) where T : new()
    {
        var resultList = new List<T>();
        await using var connection = new SqlConnection(connectionString);

        await using var command = new SqlCommand(storedProcedureName, connection);

        //command.CommandTimeout = 0;

        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.AddRange(parameters.ToArray());

        connection.Open();

        var reader = await command.ExecuteReaderAsync();

        while (reader.Read())
        {
            var item = new T();
            foreach (var prop in typeof(T).GetProperties())
            {
                if (prop.CanWrite)
                    prop.SetValue(item, Convert.ChangeType(reader[prop.Name], prop.PropertyType));
            }

            resultList.Add(item);
        }

        return resultList;
    }
}