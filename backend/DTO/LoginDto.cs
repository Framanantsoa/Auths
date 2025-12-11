using System.ComponentModel.DataAnnotations;

namespace DTO;

public class LoginDto
{

    [Required(ErrorMessage = "L'email est obligatoire.")]
    public string email {get; set;}
    
    [Required(ErrorMessage = "Le mot de passe est obligatoire.")]
    public string motDePasse {get; set;}
}
