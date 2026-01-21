using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models;

[Table("utilisateurs")]
public class Utilisateur
{
    [Key]
    [Column("id_utilisateur")]
    public long Id { get; set; }

    [Column("nom")]
    public string Nom { get; set; }

    [Column("prenom")]
    public string Prenom { get; set; }

    [Column("naissance")]
    public DateOnly? Naissance { get; set; }

    [Column("email")]
    public string Email { get; set; }

    [Column("mot_de_passe")]
    public string MotDePasse { get; set; }


    [Column("id_genre")]
    public long IdGenre { get; set; }

    [ForeignKey(nameof(IdGenre))]
    public Genre Genre { get; set; }
}
