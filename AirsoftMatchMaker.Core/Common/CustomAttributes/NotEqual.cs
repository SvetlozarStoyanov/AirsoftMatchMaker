﻿using System.ComponentModel.DataAnnotations;

/// <summary>
/// Attribute which determines whether two field have the same value
/// </summary>
public class NotEqualAttribute : ValidationAttribute
{
    private string OtherProperty { get; set; }

    public NotEqualAttribute(string otherProperty)
    {
        OtherProperty = otherProperty;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        // get other property value
        var otherPropertyInfo = validationContext.ObjectType.GetProperty(OtherProperty);
        var otherValue = otherPropertyInfo.GetValue(validationContext.ObjectInstance);

        // verify values
        if (value.ToString().Equals(otherValue.ToString()))
            return new ValidationResult("");
        /*string.Format("{0} should not be equal to {1}.", validationContext.MemberName, OtherProperty)*/
        else
            return ValidationResult.Success;
    }
}