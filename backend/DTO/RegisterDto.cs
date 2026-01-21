using System.ComponentModel.DataAnnotations;
using Validations;

namespace DTO;

public class RegisterDto : UserInfoDto
{
    public DateOnly? Naissance {get; set;}

    [Required(ErrorMessage = "L'email est obligatoire.")]
    [EmailAddress(ErrorMessage = "L'addresse e-mail est incorrect.")]
    [UniqueEmail]
    public string Email {get; set;}
}
