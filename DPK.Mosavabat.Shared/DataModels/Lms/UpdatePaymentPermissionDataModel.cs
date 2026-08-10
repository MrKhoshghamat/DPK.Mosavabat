using System.ComponentModel.DataAnnotations;

namespace DPK.Mosavabat.Shared.DataModels.Lms
{
    public class UpdatePaymentPermissionDataModel
    {
        public int Id { get; set; }
        public bool IsDelete { get; set; }
        public bool IsCancel { get; set; }
    }
}