using System.ComponentModel.DataAnnotations;
using Utils;

namespace Validations;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class ExistingGenderAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(
        object value, ValidationContext validationContext
    ) {
        if(value == null) return ValidationResult.Success;
        
        var _context = (DbaContext)validationContext.GetService(typeof(DbaContext));

        if (value is long idGenre) {
            bool exists = _context.Genres.Any(g => g.id==idGenre);
            if (!exists) {
                return new ValidationResult("Le genre spécifié n'existe pas.");
            }
        }

        return ValidationResult.Success;
    }
}
