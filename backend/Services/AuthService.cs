using DTO;
using Microsoft.EntityFrameworkCore;
using Models;
using Npgsql;
using Utils;

namespace Services;

public class AuthService : IAuthService
{
    private readonly DbaContext _ctx;
    private readonly IConfiguration _config;

    public AuthService(DbaContext ctx, IConfiguration config) {
        _ctx = ctx;
        _config = config;
    }


    public async Task<Utilisateur> registerUser(RegisterDto dto) {
        Utilisateur usr = new Utilisateur {
            nom = dto.nom.ToUpper(),
            prenom = dto.prenom,
            genre = await _ctx.Genres.FindAsync(dto.idGenre),
            naissance = dto.naissance,
            email = dto.email,
            motDePasse = BCrypt.Net.BCrypt.HashPassword(dto.motDePasse)
        };
        await _ctx.Utilisateurs.AddAsync(usr);
        await _ctx.SaveChangesAsync();

        return usr;
    }


    public async Task<Utilisateur> getUserByLogin(LoginDto dto) {
        Utilisateur usr = await _ctx.Utilisateurs.FirstOrDefaultAsync(u =>
            u.email==dto.email);
        if(usr != null &&
         BCrypt.Net.BCrypt.Verify(dto.motDePasse, usr.motDePasse)) return usr;
        
        return null;
    }


    public async Task<string> logUser(LoginDto dto) {
        Utilisateur user = await this.getUserByLogin(dto);

        if(user != null) {
            int sessionTime = _config.GetValue<int>("SessionTime");
            string token = Guid.NewGuid().ToString();

            Session tempSession = new Session {
                dateDebut = DateTime.UtcNow,
                expiration = DateTime.UtcNow.AddMinutes(sessionTime),
                token = token,
                utilisateur = user
            };

        // Ajouter la session de l'utilisateur
            await _ctx.Sessions.AddAsync(tempSession);
                    
            await _ctx.SaveChangesAsync();
            return token;
        }
        
        throw new ArgumentException("Email ou mot de passe incorrect !");
    }


    public async Task<bool> isAuthenticated(string token) {
        return await _ctx.Sessions.AnyAsync(s => 
            s.token == token && s.expiration >= DateTime.UtcNow
        );
    }


    public async Task logoutUser(string token) {
        if(await this.isAuthenticated(token) == false) {
           throw new ArgumentException("Token manquant ou éxpiré !");
        }

        Session session = await _ctx.Sessions.FirstOrDefaultAsync(s => 
            s.token == token
        );

        session.expiration = DateTime.UtcNow;
        await _ctx.SaveChangesAsync();
    }
   

    public async Task refreshToken(string token) {
        if(await this.isAuthenticated(token) == false) {
           throw new ArgumentException("Token manquant ou éxpiré !");
        }

        Session session = await _ctx.Sessions.Include(s => s.utilisateur)
            .FirstOrDefaultAsync(s => s.token == token
        );
        session.expiration = DateTime.UtcNow.AddMinutes(
            _config.GetValue<int>("SessionTime")
        );

        await _ctx.SaveChangesAsync();
    }


    public async Task<Utilisateur> getUserByToken(string token) {
        if(!await this.isAuthenticated(token))
            throw new ArgumentException("Token manquant ou expiré !");

        // Charger session + utilisateur
        Session session = await _ctx.Sessions
            .Include(s => s.utilisateur)
            .FirstOrDefaultAsync(s => s.token == token);
        
        if (session == null || session.utilisateur == null)
            throw new ArgumentException("Session ou utilisateur introuvable !");

        return session.utilisateur;
    }


    public async Task<PersoInfoDto> getPersonnalInformations(string token) {
        Utilisateur user = await this.getUserByToken(token);

        string query = @"
            SELECT u.nom, u.prenom, u.email, u.naissance, g.nom_genre AS genre
            FROM utilisateurs u
            LEFT JOIN genres g ON u.id_genre = g.id_genre
            WHERE u.id_utilisateur = @arg1 ORDER BY u.id_utilisateur
        ";
        var param = new NpgsqlParameter("@arg1", user.id);

        PersoInfoDto infoDto = await _ctx.Database
            .SqlQueryRaw<PersoInfoDto>(query, param)
            .FirstOrDefaultAsync();

        if(infoDto == null)
            throw new InvalidOperationException("Informations personnelles introuvables");

        return infoDto;
    }


    public async Task updateUserInformations(string token, InfoUpdateDto dto) {
        Utilisateur user = await this.getUserByToken(token);
    
    // Vérifier l'ancien mot de passe
        LoginDto login = new LoginDto {
            email = user.email, motDePasse = dto.motDePasse
        };

        string mdp = dto.nouveauMotDePasse!=null ? dto.nouveauMotDePasse:dto.motDePasse;

        if(user == await this.getUserByLogin(login)) {
            user.nom = dto.nom.ToUpper();
            user.prenom = dto.prenom;
            user.email = dto.email;
            user.motDePasse = BCrypt.Net.BCrypt.HashPassword(mdp);
            user.naissance = dto.naissance;
            user.genre = await _ctx.Genres.FindAsync(dto.idGenre);

            await _ctx.SaveChangesAsync(); return;
        }
        throw new ArgumentException("Ancien mot de passe incorrect");
    }
}
