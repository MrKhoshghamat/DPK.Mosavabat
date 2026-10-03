namespace DPK.Mosavabat.Shared.DataModels.Bsi;

public class PagingDataModel
{
    private readonly int _pageNumber;
    private readonly int _pageSize;

    public PagingDataModel(int pageNumber, int pageSize)
    {
        _pageNumber = pageNumber;
        _pageSize = pageSize;
    }

    public List<string> GetPageParametersForBsi()
    {
        var result = new List<string>
        {
            $"page={_pageNumber}",
            $"size={_pageSize}"
        };

        return result;
    }
}