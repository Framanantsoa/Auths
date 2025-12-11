namespace DTO;

public class PersoInfoDto
{
    public string nom { get; set; }
    public string prenom { get; set; }
    public string email { get; set; }
    public string genre { get; set; }
    public DateOnly? naissance { get; set; }
}
