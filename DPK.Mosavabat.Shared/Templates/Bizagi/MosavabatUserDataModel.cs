namespace DPK.Mosavabat.Shared.Templates.Bizagi;

public class MosavabatUserDataModel
{
    public MosavabatUserDataModel(string username)
    {
        UserName = username;
    }

    public string UserName { get; set; }
}