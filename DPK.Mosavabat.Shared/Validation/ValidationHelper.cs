using FluentValidation;

namespace DPK.Mosavabat.Shared.Validation
{
    public static class ValidationHelper
    {
        public static IRuleBuilderOptions<T, short> MaximumLength<T>(this IRuleBuilder<T, short> rule, int maximumLength) => rule.Must(n => Math.Log10(Math.Abs((double)n)) <= maximumLength);
        public static IRuleBuilderOptions<T, short?> MaximumLength<T>(this IRuleBuilder<T, short?> rule, int maximumLength) => rule.Must(n => Math.Log10(Math.Abs((double)n.Value)) <= maximumLength);
        public static IRuleBuilderOptions<T, int> MaximumLength<T>(this IRuleBuilder<T, int> rule, int maximumLength) => rule.Must(n => Math.Log10(Math.Abs((double)n)) <= maximumLength);
        public static IRuleBuilderOptions<T, int?> MaximumLength<T>(this IRuleBuilder<T, int?> rule, int maximumLength) => rule.Must(n => Math.Log10(Math.Abs((double)n.Value)) <= maximumLength);
        public static IRuleBuilderOptions<T, long> MaximumLength<T>(this IRuleBuilder<T, long> rule, int maximumLength) => rule.Must(n => Math.Log10(Math.Abs((double)n)) <= maximumLength);
        public static IRuleBuilderOptions<T, long?> MaximumLength<T>(this IRuleBuilder<T, long?> rule, int maximumLength) => rule.Must(n => Math.Log10(Math.Abs((double)n.Value)) <= maximumLength);
        public static IRuleBuilderOptions<T, decimal> MaximumLength<T>(this IRuleBuilder<T, decimal> rule, int maximumLength) => rule.Must(n => Math.Log10(Math.Abs((double)n)) <= maximumLength);
        public static IRuleBuilderOptions<T, decimal?> MaximumLength<T>(this IRuleBuilder<T, decimal?> rule, int maximumLength) => rule.Must(n => Math.Log10(Math.Abs((double)n.Value)) <= maximumLength);
        public static IRuleBuilderOptions<T, double> MaximumLength<T>(this IRuleBuilder<T, double> rule, int maximumLength) => rule.Must(n => Math.Log10(Math.Abs(n)) <= maximumLength);
        public static IRuleBuilderOptions<T, double?> MaximumLength<T>(this IRuleBuilder<T, double?> rule, int maximumLength) => rule.Must(n => Math.Log10(Math.Abs(n.Value)) <= maximumLength);
    }
}
