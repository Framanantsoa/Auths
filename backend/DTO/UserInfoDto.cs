using System.ComponentModel.DataAnnotations;
using Validations;

namespace DTO;

public class UserInfoDto
{
    [Required(ErrorMessage = "Le nom est obligatoire.")]
    public string nom {get; set;}

    [Required(ErrorMessage = "Le prénom est obligatoire.")]
    public string prenom {get; set;}

    [Required(ErrorMessage = "Le mot de passe est obligatoire.")]
    public string motDePasse {get; set;}

    [Required(ErrorMessage = "Le genre est obligatoire.")]
    [ExistingGender]
    public long? idGenre {get; set;}
}
