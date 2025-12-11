using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models;

[Table("utilisateurs")]
public class Utilisateur
{
    [Key]
    [Column("id_utilisateur")]
    public long id { get; set; }

    public string nom { get; set; }

    public string prenom { get; set; }

    public DateOnly? naissance { get; set; }

    public string email { get; set; }

    [Column("mot_de_passe")]
    public string motDePasse { get; set; }

    [ForeignKey("id_genre")]
    public Genre genre { get; set; }
}
