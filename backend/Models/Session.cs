using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models;

[Table("sessions")]
public class Session
{
    [Key]
    [Column("id_session")]
    public long Id { get; set; }

    [Column("date_debut")]
    public DateTime DateDebut { get; set; }

    [Column("expiration")]
    public DateTime Expiration { get; set; }

    [Column("token")]
    public string Token { get; set; }


    [Column("id_utilisateur")]
    public long UtilisateurId { get; set; }

    [ForeignKey(nameof(UtilisateurId))]
    public Utilisateur Utilisateur { get; set; }
}
