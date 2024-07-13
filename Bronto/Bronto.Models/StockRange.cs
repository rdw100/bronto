namespace Bronto.Models.Enums
{
    /// <summary>
    /// Allows string attributes to be applied
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public sealed class StringValueAttribute : Attribute
    {
        public string Value { get; }

        public StringValueAttribute(string value)
        {
            Value = value;
        }
    }

    /// <summary>
    /// Represents valid stock ranges used to specify the desired length of periodic data.
    /// </summary>
    public enum StockRange
    {
        [StringValue("1d")]
        OneDay,
        [StringValue("5d")]
        FiveDays,
        [StringValue("1mo")]
        OneMonth,
        [StringValue("3mo")]
        ThreeMonths,
        [StringValue("6mo")]
        SixMonths,
        [StringValue("1y")]
        OneYear,
        [StringValue("2y")]
        TwoYears,
        [StringValue("5y")]
        FiveYears,
        [StringValue("10y")]
        TenYears,
        [StringValue("ytd")]
        YearToDate,
        [StringValue("max")]
        Max
    }

    /// <summary>
    /// Extension method to get the string value
    /// </summary>
    public static class EnumExtensions
    {
        public static string GetStringValue(this Enum value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());
            var attribute = fieldInfo?.GetCustomAttributes(typeof(StringValueAttribute), false)
                .FirstOrDefault() as StringValueAttribute;

            return attribute?.Value;
        }
    }
}