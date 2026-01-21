using System.ComponentModel.DataAnnotations;

namespace DTO;

public class InfoUpdateDto : UserInfoDto
{
    [Required(ErrorMessage = "L'email est obligatoire.")]
    [EmailAddress(ErrorMessage = "L'addresse e-mail est incorrect.")]
    public string Email {get; set;}

    public DateOnly? Naissance {get; set;}
    public string NouveauMotDePasse { get; set; }
}
