using DPK.Mosavabat.Services.Extensions;
using DPK.Mosavabat.Shared.Templates.Bizagi.Base;

namespace DPK.Mosavabat.Services.Helper;

public class BizagiHelpers
{
    public static BizAgiWSParamCreate<T> CreateStandardModelForBizagi<T>(T model, string userName, string process)
    {
        return new BizAgiWSParamCreate<T>
        {
            Domain = "srv-mosavabat",
            UserName = userName,
            Cases = new Cases<T>
            {
                Case = new Case<T>
                {
                    Process = process,
                    Entities = new Entities<T>
                    {
                        Dynamic = model
                    }
                }
            }
        };
    }

    public static BizAgiWSParamUpdate<T> GetStandardModelUpdate<T>(T model, string userName, string idCase, string taskId)
    {
        return new BizAgiWSParamUpdate<T>
        {
            Domain = "srv-mosavabat",
            UserName = userName,
            ActivityData = new MosavabatActivityDataDataModel
            {
                IdCase = idCase,
                TaskId = taskId
            },
            Entities = new Entities<T>
            {
                Dynamic = model
            }
        };
    }

    public static BizAgiWSParamUpdate<T> GenerateSaveActivityDataModel<T>(T model, string userName, string idCase, string taskId)
    {
        return new BizAgiWSParamUpdate<T>
        {
            Domain = "srv-mosavabat",
            UserName = userName,
            ActivityData = new MosavabatActivityDataDataModel
            {
                IdCase = idCase,
                TaskId = taskId
            },
            Entities = new Entities<T>
            {
                Dynamic = model
            }
        };
    }

    public static string ConvertModelToXmlMosavabat<T>(T model, string mosavabat) where T : BizAgiWSParam
    {
        var xml = model.ConvertObjectToXml();
        return xml.Replace(nameof(Entities<int>.Dynamic), mosavabat.ToString());
    }
}