using System.ComponentModel.DataAnnotations;
using Validations;

namespace DTO;

public class UserInfoDto
{
    [Required(ErrorMessage = "Le nom est obligatoire.")]
    public string Nom {get; set;}

    [Required(ErrorMessage = "Le prénom est obligatoire.")]
    public string Prenom {get; set;}

    [Required(ErrorMessage = "Le mot de passe est obligatoire.")]
    public string MotDePasse {get; set;}

    [Required(ErrorMessage = "Le genre est obligatoire.")]
    [ExistingGender]
    public long? IdGenre {get; set;}
}
