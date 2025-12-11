using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models;

[Table("tentatives")]
public class Tentative
{
    [Key]
    [Column("id_tentative")]
    public long id { get; set; }

    public long restant { get; set; }

    [ForeignKey("id_session")]
    public Session session { get; set; }
}
