using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Wrapper class for strings that will always be drawn as if they were strings 
/// with <see cref="TagSelectorAttribute"/>.
/// </summary>
/// <remarks>Lightly edited from <see href="https://stackoverflow.com/a/22912864"/>.</remarks>
[Serializable]
public struct TagString : IEnumerable<char>, IEnumerable, ICloneable, IComparable, IComparable<string>, IConvertible, IEquatable<string>
{
    public string _value;
    public TagString(string value) => this._value = value;

    public static implicit operator string(TagString ds) => ds._value;
    public static implicit operator TagString(string s) => new TagString(s);

    public bool Equals(TagString other) => _value == other._value;
    public bool Equals(string other) => _value == other;

    public int CompareTo(string other) => _value.CompareTo(other);
    public int CompareTo(object obj) => _value.CompareTo(obj);

    public override int GetHashCode() => _value.GetHashCode();

    public IEnumerator GetEnumerator() => ((IEnumerable)_value).GetEnumerator();
    IEnumerator<char> IEnumerable<char>.GetEnumerator() => ((IEnumerable<char>)_value).GetEnumerator();

    public object Clone() => _value.Clone();

    #region IConvertible Stuff
    public TypeCode GetTypeCode()
    {
        return _value.GetTypeCode();
    }

    public bool ToBoolean(IFormatProvider provider)
    {
        return ((IConvertible)_value).ToBoolean(provider);
    }

    public byte ToByte(IFormatProvider provider)
    {
        return ((IConvertible)_value).ToByte(provider);
    }

    public char ToChar(IFormatProvider provider)
    {
        return ((IConvertible)_value).ToChar(provider);
    }

    public DateTime ToDateTime(IFormatProvider provider)
    {
        return ((IConvertible)_value).ToDateTime(provider);
    }

    public decimal ToDecimal(IFormatProvider provider)
    {
        return ((IConvertible)_value).ToDecimal(provider);
    }

    public double ToDouble(IFormatProvider provider)
    {
        return ((IConvertible)_value).ToDouble(provider);
    }

    public short ToInt16(IFormatProvider provider)
    {
        return ((IConvertible)_value).ToInt16(provider);
    }

    public int ToInt32(IFormatProvider provider)
    {
        return ((IConvertible)_value).ToInt32(provider);
    }

    public long ToInt64(IFormatProvider provider)
    {
        return ((IConvertible)_value).ToInt64(provider);
    }

    public sbyte ToSByte(IFormatProvider provider)
    {
        return ((IConvertible)_value).ToSByte(provider);
    }

    public float ToSingle(IFormatProvider provider)
    {
        return ((IConvertible)_value).ToSingle(provider);
    }

    public string ToString(IFormatProvider provider)
    {
        return _value.ToString(provider);
    }

    public object ToType(Type conversionType, IFormatProvider provider)
    {
        return ((IConvertible)_value).ToType(conversionType, provider);
    }

    public ushort ToUInt16(IFormatProvider provider)
    {
        return ((IConvertible)_value).ToUInt16(provider);
    }

    public uint ToUInt32(IFormatProvider provider)
    {
        return ((IConvertible)_value).ToUInt32(provider);
    }

    public ulong ToUInt64(IFormatProvider provider)
    {
        return ((IConvertible)_value).ToUInt64(provider);
    }
    #endregion
}