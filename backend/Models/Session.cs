using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models;

[Table("sessions")]
public class Session
{
    [Key]
    [Column("id_session")]
    public long id { get; set; }

    [Column("date_debut")]
    public DateTime dateDebut { get; set; }

    public DateTime expiration { get; set; }

    public string token { get; set; }

    [ForeignKey("id_utilisateur")]
    public Utilisateur utilisateur { get; set; }
}
