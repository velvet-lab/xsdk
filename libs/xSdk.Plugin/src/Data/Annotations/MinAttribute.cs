namespace xSdk.Data.Annotations
{
    /// <summary>
    /// Min primaryKey attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class MinAttribute : DataAnnotationAttribute
    {
        /// <summary>
        /// Initializes a new instance of the MinAttribute class.
        /// </summary>
        /// <param name="value">The minimum primaryKey.</param>
        public MinAttribute(object value)
            : base(value) { }

        public override bool IsValid(object value)
        {
            if (IsIntValue())
            {
                var configured = GetIntValue();
                var current = (int)Value;

                if (current < configured)
                    return false;
            }
            else if (IsDoubleValue())
            {
                var configured = GetDoubleValue();
                var current = (double)Value;

                if (current < configured)
                    return false;
            }
            else if (IsTimeSpanValue())
            {
                var configured = GetTimeSpanValue();
                var current = (TimeSpan)Value;

                if (current < configured)
                    return false;
            }

            return true;
        }
    }
}
