using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models;

[Table("tentatives")]
public class Tentative
{
    [Key]
    [Column("id_tentative")]
    public long Id { get; set; }

    [Column("restant")]
    public long Restant { get; set; }


    [Column("id_session")]
    public long SessionId { get; set; }

    [ForeignKey(nameof(SessionId))]
    public Session Session { get; set; }
}
