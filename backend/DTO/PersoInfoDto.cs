namespace DTO;

public class PersoInfoDto
{
    public string Nom { get; set; }
    public string Prenom { get; set; }
    public string Email { get; set; }
    public string Genre { get; set; }
    public DateOnly? Naissance { get; set; }
}
