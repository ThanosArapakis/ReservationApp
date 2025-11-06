using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ReservationApp.core.api.Domain.Enums;

public abstract class SmartEnumeration<TEnum> : IEquatable<SmartEnumeration<TEnum>>
where TEnum : SmartEnumeration<TEnum>
{
    private static readonly Dictionary<int, TEnum> Enumerations = CreateEnumerations();

    protected SmartEnumeration(int value, string description)
    {
        Value = value;
        Description = description;
    }

    public int Value { get; protected init; }

    public string Description { get; protected init; } = string.Empty;

    public static TEnum? FromValue(int value)
    {
        return Enumerations.TryGetValue(
            value,
            out TEnum? enumeration)
                ? enumeration
                : default;
    }

    public static TEnum? FromDescription(string description)
    {
        return Enumerations.Values
            .SingleOrDefault(e => e.Description == description);
    }

    public bool Equals(SmartEnumeration<TEnum>? other)
    {
        if (other is null)
        {
            return false;
        }

        return GetType() == other.GetType() &&
            Value == other.Value;
    }

    public override bool Equals(object? obj)
    {
        return obj is SmartEnumeration<TEnum> other &&
            Equals(other);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public override string ToString()
    {
        return Description;
    }

    private static Dictionary<int, TEnum> CreateEnumerations()
    {
        Type enumerationType = typeof(TEnum);

        IEnumerable<TEnum> fieldsForType = enumerationType
            .GetFields(
                BindingFlags.Public |
                BindingFlags.Static |
                BindingFlags.FlattenHierarchy)
            .Where(fieldInfo =>
                enumerationType.IsAssignableFrom(fieldInfo.FieldType))
            .Select(fieldInfo =>
                (TEnum)fieldInfo.GetValue(default)!);

        return fieldsForType.ToDictionary(x => x.Value);
    }
}