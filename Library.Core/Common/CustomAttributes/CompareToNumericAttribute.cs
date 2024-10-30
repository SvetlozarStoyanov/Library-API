using Library.Core.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace Library.Core.Common.CustomAttributes;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class CompareToNumericAttribute : ValidationAttribute
{
    private readonly string comparisonProperty;
    private readonly ComparisonOperator comparisonOperator;

    public CompareToNumericAttribute(string comparisonProperty, ComparisonOperator comparisonOperator)
    {
        this.comparisonProperty = comparisonProperty;
        this.comparisonOperator = comparisonOperator;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        int currentValue = (int)value;
        System.Reflection.PropertyInfo? comparisonProperty = validationContext.ObjectType.GetProperty(this.comparisonProperty);
        if (comparisonProperty == null)
        {
            return new ValidationResult($"Unknown property: {this.comparisonProperty}");
        }
        int comparisonValue = (int)comparisonProperty.GetValue(validationContext.ObjectInstance);
        if (!IsNumericType(comparisonValue))
        {
            return new ValidationResult($"Cannot compare non-numeric properties");
        }
        string message = string.Empty;
        switch (comparisonOperator)
        {
            case ComparisonOperator.SmallerThan:
                if (currentValue < comparisonValue)
                {
                    return ValidationResult.Success;
                }
                message = $"{this.comparisonProperty} must be less than {comparisonProperty}";
                break;
            case ComparisonOperator.SmallerOrEqualTo:
                if (currentValue <= comparisonValue)
                {
                    return ValidationResult.Success;
                };
                message = $"{this.comparisonProperty} must be less than or equal to {comparisonProperty}";
                break;
            case ComparisonOperator.EqualTo:
                if (currentValue == comparisonValue)
                {
                    return ValidationResult.Success;
                };
                message = $"{this.comparisonProperty} must be equal to {comparisonProperty}";
                break;
            case ComparisonOperator.GreaterThan:
                if (currentValue > comparisonValue)
                {
                    return ValidationResult.Success;
                };
                message = $"{this.comparisonProperty} must be greater than {comparisonProperty}";
                break;
            case ComparisonOperator.GreaterThanOrEqual:
                if (currentValue >= comparisonValue)
                {
                    return ValidationResult.Success;
                };
                message = $"{this.comparisonProperty} must greater than or equal to {comparisonProperty}";
                break;
        }

        return new ValidationResult(message);
    }

    private bool IsNumericType(object value)
    {
        if (value == null)
            return false;

        TypeCode typeCode = Type.GetTypeCode(value.GetType());
        return typeCode == TypeCode.Byte ||
               typeCode == TypeCode.SByte ||
               typeCode == TypeCode.UInt16 ||
               typeCode == TypeCode.UInt32 ||
               typeCode == TypeCode.UInt64 ||
               typeCode == TypeCode.Int16 ||
               typeCode == TypeCode.Int32 ||
               typeCode == TypeCode.Int64 ||
               typeCode == TypeCode.Decimal ||
               typeCode == TypeCode.Double ||
               typeCode == TypeCode.Single;
    }
}