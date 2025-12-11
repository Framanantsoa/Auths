using DTO;
using Models;

namespace Services;

public interface IAuthService
{
    Task<Utilisateur> registerUser(RegisterDto dto);
    Task<string> logUser(LoginDto dto);
    Task<bool> isAuthenticated(string token);
    Task logoutUser(string token);
    Task refreshToken(string token);
    Task<PersoInfoDto> getPersonnalInformations(string token);

    Task<Utilisateur> getUserByToken(string token);
    Task updateUserInformations(string token, InfoUpdateDto dto);
    Task<Utilisateur> getUserByLogin(LoginDto dto);
}
