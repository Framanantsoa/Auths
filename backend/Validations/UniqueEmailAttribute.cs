using System.ComponentModel.DataAnnotations;
using Utils;

namespace Validations;

public class UniqueEmailAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(
        object value, ValidationContext validationContext
    ) {
        var _context = (DbaContext)validationContext.GetService(typeof(DbaContext));

        if(value == null) return ValidationResult.Success;

        var email = value.ToString();
        if(!string.IsNullOrEmpty(email)) {
            bool exists = _context.Utilisateurs.Any(u => u.email == email);
            if (exists) {
                return new ValidationResult("Cet email est déjà utilisé.");
            }
        }

        return ValidationResult.Success;
    }
}
