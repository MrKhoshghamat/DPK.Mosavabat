using System.Data;

namespace DPK.Mosavabat.Shared.Tvps;

public class SettledContractTvpRow
{
    public string C13 { get; init; } = null!;


    public static DataTable BuildSettledContractsTvp(IEnumerable<string> c13List)
    {
        var table = new DataTable();
        table.Columns.Add("C13", typeof(string));

        foreach (var c13 in c13List.Distinct())
            table.Rows.Add(c13);

        return table;
    }
}