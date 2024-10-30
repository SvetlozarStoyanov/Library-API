using System.ComponentModel.DataAnnotations;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class CustomRangeAttribute : ValidationAttribute
{
    public double? Minimum { get; }
    public double? Maximum { get; }
    public string MinimumProperty { get; }
    public string MaximumProperty { get; }

    public CustomRangeAttribute(double minimum, double maximum)
    {
        Minimum = minimum;
        Maximum = maximum;
    }

    public CustomRangeAttribute(string minimumProperty, string maximumProperty)
    {
        MinimumProperty = minimumProperty;
        MaximumProperty = maximumProperty;
    }

    public CustomRangeAttribute(double minimum, string maximumProperty)
    {
        Minimum = minimum;
        MaximumProperty = maximumProperty;
    }

    public CustomRangeAttribute(string minimumProperty, double maximum)
    {
        MinimumProperty = minimumProperty;
        Maximum = maximum;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return ValidationResult.Success; // Assume null values are handled elsewhere
        }

        double doubleValue;
        try
        {
            doubleValue = Convert.ToDouble(value);
        }
        catch
        {
            return new ValidationResult($"Property {validationContext.MemberName} is not a valid number.");
        }

        double? min = Minimum, max = Maximum;

        if (!min.HasValue || !max.HasValue)
        {
            System.Reflection.PropertyInfo[] properties = validationContext.ObjectType.GetProperties();
            if (!min.HasValue && !string.IsNullOrEmpty(MinimumProperty))
            {
                System.Reflection.PropertyInfo? minProp = properties.FirstOrDefault(p => p.Name == MinimumProperty);
                if (minProp != null)
                {
                    try
                    {
                        min = Convert.ToDouble(minProp.GetValue(validationContext.ObjectInstance));
                    }
                    catch
                    {
                        return new ValidationResult($"Property {MinimumProperty} is not a valid number.");
                    }
                }
                else
                {
                    return new ValidationResult($"Property {MinimumProperty} not found.");
                }
            }

            if (!max.HasValue && !string.IsNullOrEmpty(MaximumProperty))
            {
                System.Reflection.PropertyInfo? maxProp = properties.FirstOrDefault(p => p.Name == MaximumProperty);
                if (maxProp != null)
                {
                    try
                    {
                        max = Convert.ToDouble(maxProp.GetValue(validationContext.ObjectInstance));
                    }
                    catch
                    {
                        return new ValidationResult($"Property {MaximumProperty} is not a valid number.");
                    }
                }
                else
                {
                    return new ValidationResult($"Property {MaximumProperty} not found.");
                }
            }
        }

        if (doubleValue < min || doubleValue > max)
        {
            return new ValidationResult($"Property {validationContext.MemberName} with value {doubleValue} is out of range ({min} - {max}).");
        }

        return ValidationResult.Success;
    }
}
