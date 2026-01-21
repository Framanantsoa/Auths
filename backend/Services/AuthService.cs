using DTO;
using Microsoft.EntityFrameworkCore;
using Models;
using Npgsql;
using Utils;

namespace Services;

public class AuthService(DbaContext ctx, IConfiguration config, 
 ILogger<AuthService> log) : IAuthService
{
    private readonly DbaContext _ctx = ctx;
    private readonly IConfiguration _config = config;
    private readonly ILogger<AuthService> _logger = log;


    public async Task<Utilisateur> registerUser(RegisterDto dto) {
        Utilisateur usr = new() {
            Nom = dto.Nom.ToUpper(),
            Prenom = dto.Prenom,
            Genre = await _ctx.Genres.FindAsync(dto.IdGenre),
            Naissance = dto.Naissance,
            Email = dto.Email,
            MotDePasse = BCrypt.Net.BCrypt.HashPassword(dto.MotDePasse)
        };
        await _ctx.Utilisateurs.AddAsync(usr);
        await _ctx.SaveChangesAsync();

        return usr;
    }


    public async Task<Utilisateur> getUserByLogin(LoginDto dto) {
        Utilisateur usr = await _ctx.Utilisateurs.FirstOrDefaultAsync(u =>
            u.Email==dto.Email);
        if(usr != null &&
         BCrypt.Net.BCrypt.Verify(dto.MotDePasse, usr.MotDePasse)) return usr;
        
        return null;
    }


    public async Task<(long, string)> logUser(LoginDto dto) {
        Utilisateur user = await this.getUserByLogin(dto);

        if(user != null) {
            int sessionTime = _config.GetValue<int>("SessionTime");
            string Token = Guid.NewGuid().ToString();

            Session tempSession = new Session {
                DateDebut = DateTime.UtcNow,
                Expiration = DateTime.UtcNow.AddMinutes(sessionTime),
                Token = Token,
                Utilisateur = user
            };

        // Ajouter la session de l'Utilisateur
            await _ctx.Sessions.AddAsync(tempSession);
                    
            await _ctx.SaveChangesAsync();
            return (user.Id, Token);
        }
        
        throw new ArgumentException("Email ou mot de passe incorrect !");
    }


    public async Task<bool> isAuthenticated(string Token) {
        _logger.LogInformation($"Token : {Token}; {DateTime.UtcNow}");

        return await _ctx.Sessions.AnyAsync(s => 
            s.Token == Token && s.Expiration >= DateTime.UtcNow
        );
    }


    public async Task logoutUser(string Token) {
        if(await this.isAuthenticated(Token) == false) {
           throw new ArgumentException("Token manquant ou éxpiré !");
        }

        Session session = await _ctx.Sessions.FirstOrDefaultAsync(s => 
            s.Token == Token
        );

        session.Expiration = DateTime.UtcNow;
        await _ctx.SaveChangesAsync();
    }
   

    public async Task refreshToken(string Token) {
        if(await this.isAuthenticated(Token) == false) {
           throw new ArgumentException("Token manquant ou éxpiré !");
        }

        Session session = await _ctx.Sessions.Include(s => s.Utilisateur)
            .FirstOrDefaultAsync(s => s.Token == Token
        );
        session.Expiration = DateTime.UtcNow.AddMinutes(
            _config.GetValue<int>("SessionTime")
        );

        await _ctx.SaveChangesAsync();
    }


    public async Task<Utilisateur> getUserByToken(string Token) {
        if(!await this.isAuthenticated(Token))
            throw new ArgumentException("Token manquant ou expiré !");

        // Charger session + Utilisateur
        Session session = await _ctx.Sessions
            .Include(s => s.Utilisateur)
            .FirstOrDefaultAsync(s => s.Token == Token);
        
        if (session == null || session.Utilisateur == null)
            throw new ArgumentException("Session ou Utilisateur introuvable !");

        return session.Utilisateur;
    }


    public async Task<PersoInfoDto> getPersonnalInformations(string token) {
        Utilisateur user = await this.getUserByToken(token);

        string sql = """
            SELECT
                u.nom        AS "Nom",
                u.prenom     AS "Prenom",
                u.naissance  AS "Naissance",
                u.email      AS "Email",
                g.nom_genre  AS "Genre"
            FROM utilisateurs u
            LEFT JOIN genres g ON u.id_genre = g.id_genre
            WHERE u.id_utilisateur = @id
            """;

        var param = new NpgsqlParameter("@id", user.Id);

        return await _ctx.Database
            .SqlQueryRaw<PersoInfoDto>(sql, param)
            .AsNoTracking()
            .FirstOrDefaultAsync()
            ?? throw new ArgumentException("Informations personnelles introuvables");
    }


    public async Task<long> updateUserInformations(string token, InfoUpdateDto dto) {
        Utilisateur user = await this.getUserByToken(token);
    
    // Vérifier l'ancien mot de passe
        LoginDto login = new LoginDto {
            Email = user.Email, MotDePasse = dto.MotDePasse
        };

        string mdp = dto.NouveauMotDePasse!=null ? dto.NouveauMotDePasse:dto.MotDePasse;

        if(user == await this.getUserByLogin(login)) {
            user.Nom = dto.Nom.ToUpper();
            user.Prenom = dto.Prenom;
            user.Email = dto.Email;
            user.MotDePasse = BCrypt.Net.BCrypt.HashPassword(mdp);
            user.Naissance = dto.Naissance;
            user.Genre = await _ctx.Genres.FindAsync(dto.IdGenre);

            await _ctx.SaveChangesAsync(); 
            return user.Id;
        }
        throw new ArgumentException("Ancien mot de passe incorrect");
    }
}
