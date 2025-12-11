using System.ComponentModel.DataAnnotations;

namespace DTO;

public class InfoUpdateDto : UserInfoDto
{
    [Required(ErrorMessage = "L'email est obligatoire.")]
    [EmailAddress(ErrorMessage = "L'addresse e-mail est incorrect.")]
    public string email {get; set;}

    public DateOnly? naissance {get; set;}
    public string nouveauMotDePasse { get; set; }
}
