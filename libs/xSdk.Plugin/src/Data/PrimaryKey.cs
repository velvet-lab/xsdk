using xSdk.Shared;

namespace xSdk.Data
{
    public abstract class PrimaryKey : IPrimaryKey
    {
        private object primaryKey;

        public PrimaryKey() { }

        protected PrimaryKey(object initialValue)
        {
            primaryKey = initialValue;
        }

        public void SetValue(object value)
        {
            if (value != null)
            {
                primaryKey = value;
            }
        }

        public object GetValue() => primaryKey;

        public TType GetValue<TType>() => Convert<TType>(primaryKey);

        protected virtual TType Convert<TType>(object value)
        {
            if (value != null)
            {
                return (TType)value;
            }
            return default;
        }

        public override string ToString()
        {
            return primaryKey.ToString();
        }
    }

    public abstract class PrimaryKey<TPrimaryKeyType> : PrimaryKey, IPrimaryKey<TPrimaryKeyType>
    {
        public PrimaryKey()
            : base() { }

        protected PrimaryKey(object? initialValue)
            : base(initialValue) { }

        public static bool operator ==(PrimaryKey<TPrimaryKeyType> left, PrimaryKey<TPrimaryKeyType> right)
        {
            if (left is null)
            {
                if (right is null)
                    return true;

                return false;
            }
            else
            {
                if (right is null)
                    return false;
            }

            var leftValue = left.GetValue<TPrimaryKeyType>();
            var rightValue = right.GetValue<TPrimaryKeyType>();

            if (leftValue is null)
            {
                if (rightValue is null)
                    return true;

                return false;
            }
            else
            {
                if (rightValue is null)
                    return false;
            }

            return leftValue.GetHashCode() == rightValue.GetHashCode();
        }

        public static bool operator !=(PrimaryKey<TPrimaryKeyType> left, PrimaryKey<TPrimaryKeyType> right) => !(left == right);

        public override int GetHashCode() => ObjectHelper.CreateAutomaticHashCode(this);

        public override bool Equals(object? obj)
        {
            if (obj is null)
                return false;

            return (this == (PrimaryKey<TPrimaryKeyType>)obj);
        }
    }
}
