using Microsoft.Data.SqlClient;

namespace DPK.Mosavabat.Database.Services.Contracts;

public interface IMainDatabase
{
    Task<object?> ExecuteScalarAsync(string connectionString, string storedProcedureName, List<SqlParameter> parameters);

    Task<int> ExecuteNonQueryAsync(string connectionString, string storedProcedureName, List<SqlParameter> parameters);

    //Task<DataTable?> ExecuteAsDataTable(string connectionString, string storedProcedureName, List<SqlParameter> parameters);
}