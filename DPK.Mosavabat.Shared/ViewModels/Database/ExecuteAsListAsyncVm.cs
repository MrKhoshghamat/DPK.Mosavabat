using Microsoft.Data.SqlClient;

namespace DPK.Mosavabat.Shared.ViewModels.Database
{
    public class ExecuteAsListAsyncVm
    {
        public string ConnectionString { get; set; }
        public string StoredProcedureName { get; set; }
        public List<SqlParameter>? Parameters { get; set; }
    }
}
