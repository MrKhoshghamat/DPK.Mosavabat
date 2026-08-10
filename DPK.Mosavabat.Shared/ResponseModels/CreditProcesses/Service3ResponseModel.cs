using DPK.Mosavabat.Shared.DataModels.CreditProcesses;
using DPK.Mosavabat.Shared.DataModels.CreditProcesses.Condition;
using DPK.Mosavabat.Shared.DataModels.CreditProcesses.Provision;

namespace DPK.Mosavabat.Shared.ResponseModels.CreditProcesses
{
    public class Service3ResponseModel
    {
        public List<ConditionDataModel>? Conditions { get; set; }
        public List<ProvisionDataModel>? Provisions { get; set; }
    }
}