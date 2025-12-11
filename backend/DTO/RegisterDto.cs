using System.ComponentModel.DataAnnotations;
using Validations;

namespace DTO;

public class RegisterDto : UserInfoDto
{
    public DateOnly? naissance {get; set;}

    [Required(ErrorMessage = "L'email est obligatoire.")]
    [EmailAddress(ErrorMessage = "L'addresse e-mail est incorrect.")]
    [UniqueEmail]
    public string email {get; set;}
}
