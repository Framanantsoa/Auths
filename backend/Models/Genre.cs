using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models;

[Table("genres")]
public class Genre
{
    [Key]
    [Column("id_genre")]
    public long id { get; set; }

    [Column("nom_genre")]
    public string nom { get; set; }
}
