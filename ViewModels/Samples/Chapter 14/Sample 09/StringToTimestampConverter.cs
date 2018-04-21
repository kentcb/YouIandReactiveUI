namespace Book.ViewModels.Samples.Chapter14.Sample09
{
    using System;
    using System.Globalization;
    using ReactiveUI;

    public sealed class StringToTimestampConverter : IBindingTypeConverter
    {
        public static readonly StringToTimestampConverter Instance
                = new StringToTimestampConverter();

        private StringToTimestampConverter()
        {
        }

        public int GetAffinityForObjects(Type fromType, Type toType)
        {
            if (toType == typeof(Timestamp) && fromType == typeof(string))
            {
                return 100;
            }

            return 0;
        }

        public bool TryConvert(
            object from,
            Type toType,
            object conversionHint,
            out object result)
        {
            var format = conversionHint as string ?? "HH:mm:ss";
            var str = (string)from;
            DateTime dateTime;

            if (!DateTime.TryParseExact(
                str,
                format,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out dateTime))
            {
                result = null;
                return false;
            }

            result = new Timestamp(dateTime.TimeOfDay.Ticks);

            return true;
        }
    }
}