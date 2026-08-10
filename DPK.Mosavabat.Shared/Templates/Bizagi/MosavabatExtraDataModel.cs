namespace DPK.Mosavabat.Shared.Templates.Bizagi;

public class MosavabatExtraDataModel
{
    public MosavabatExtraDataModel(string userName)
    {
        UserName = userName;
    }

    public MosavabatExtraDataModel(string userName, int jobId)
    {
        UserName = userName;
        JobId = jobId;
    }

    /// <summary>
    ///     نام کاربری شعبه
    ///     از sp
    /// </summary>
    public string UserName { get; set; }

    public int? JobId { get; set; }
}