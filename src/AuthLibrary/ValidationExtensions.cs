using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AuthLibrary
{
    public static class ValidationExtensions
    {
        public static IEnumerable<string> ValidationErrors(this object @this)
        {
            ValidationContext context = new ValidationContext(@this, serviceProvider: null, items: null);
            List<ValidationResult> results = new List<ValidationResult>();
            Validator.TryValidateObject(@this, context, results, true);
            foreach (ValidationResult validationResult in results)
            {
                yield return validationResult.ErrorMessage;
            }
        }
    }
}
