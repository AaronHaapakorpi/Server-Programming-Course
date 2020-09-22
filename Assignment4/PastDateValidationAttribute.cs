using System;
using System.Globalization;
using System.ComponentModel.DataAnnotations;

public class PastDateValidationAttribute
    : ValidationAttribute
{
    
    public DateTime Date{set;get;}
    public string GetErrorMessage() =>
        $"Date {Date} is not form the past.";
    protected override ValidationResult IsValid(object value,ValidationContext validationContext)
    
    {
        var date = (DateTime)value;
        Date = date;
        if(date<DateTime.Now){
            return ValidationResult.Success;
        }
        return new ValidationResult(GetErrorMessage());
    }
}


