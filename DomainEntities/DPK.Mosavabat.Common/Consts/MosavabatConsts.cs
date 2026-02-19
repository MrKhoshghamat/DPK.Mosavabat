namespace DPK.Mosavabat.Common.Consts
{
    /// <summary>
    /// ثابتهای برنامه
    /// به ازای تمامی موارد باید داکیومنت اضافه شود
    /// </summary>
    public class MosavabatConsts
    {
        #region App

        public const bool MultiTenancyEnabled = false;

        /// <summary>
        /// با نام برنامه جابجا شود 
        /// </summary>
        public const string LocalizationSourceName = "Mosavabat";
        /// <summary>
        /// کانکشن پیش فرض
        /// </summary>
        public const string ConnectionStringName = "Default";
        /// <summary>
        /// در صورتی که دیتا بیس دیگری باشد نام پیش فرض را اضافه میکنیم
        /// </summary>
        public const string SecondConnectionStringName = "Second";
        /// <summary>
        /// برای نام گذاری مسیر پیش فرض سرویس استفاده می شود
        /// می توان نام را به تغییر داد Bussiness
        /// </summary>
        public const string ControllerModuleName = "Service";

        /// <summary>
        /// Default page size for paged requests.
        /// </summary>
        public const int DefaultPageSize = 10;
        /// <summary>
        /// Maximum allowed page size for paged requests.
        /// </summary>
        public const int MaxPageSize = 1000;

        public const int ResizedMaxProfilePictureBytesUserFriendlyValue = 1024;

        public const int MaxProfilePictureBytesUserFriendlyValue = 5;

        public static TimeSpan AccessTokenExpiration = TimeSpan.FromDays(1);

        public static TimeSpan RefreshTokenExpiration = TimeSpan.FromDays(365);
        #endregion

        #region WebConsts

        public const string SwaggerUiEndPoint = "/swagger";

        public const string HangfireDashboardEndPoint = "/hangfire";

        public static bool SwaggerUiEnabled = true;

        public static bool HangfireDashboardEnabled = false;

        public const string DefaultCorsPolicyName = "localhost";

        public static string StringDataTypeName = "string";

        #endregion

    }
}