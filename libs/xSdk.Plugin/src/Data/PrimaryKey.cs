/*
 * Copyright 2026 Roland Breitschaft
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using xSdk.Shared;

namespace xSdk.Data;

public abstract class PrimaryKey : IPrimaryKey
{
    private object _primaryKey;

    public PrimaryKey() { }

    protected PrimaryKey(object initialValue)
    {
        _primaryKey = initialValue;
    }

    public void SetValue(object value)
    {
        if (value != null)
        {
            _primaryKey = value;
        }
    }

    public object GetValue() => _primaryKey;

    public TType GetValue<TType>() => Convert<TType>(_primaryKey);

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
        return _primaryKey.ToString();
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
