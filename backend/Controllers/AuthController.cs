using DTO;
using Microsoft.AspNetCore.Mvc;
using Models;
using Utils;

namespace Services;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService service;

    public AuthController(IAuthService svc)
    {
        service = svc;
    }

// ---------- REGISTER ----------
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);

        Utilisateur user = await service.registerUser(dto);

        return Ok(ResponseApi.Ok("Utilisateur créé", new { user.id, user.nom }));
    }

// ---------- LOGIN ----------
    [HttpPost("signin")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        try {
            string token = await service.logUser(dto);
            return Ok(ResponseApi.Ok("Connexion réussie", new { token }));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ResponseApi.Fail(ex.Message, null));
        }
    }

// ---------- LOGOUT ----------
    [HttpDelete("logout")]
    public async Task<IActionResult> Logout()
    {
        try {
            string token = GetToken();

            await service.logoutUser(token);
            return Ok(ResponseApi.Ok("Déconnexion réussie", null));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ResponseApi.Fail(ex.Message, null));
        }
    }

// ---------- CHECK AUTH ----------
    [HttpGet("check")]
    public async Task<IActionResult> Check() {
        try {
            string token = GetToken();

            bool ok = await service.isAuthenticated(token);
            if(ok) return Ok(ResponseApi.Ok("Connecté", null));

            return Unauthorized(ResponseApi.Fail("Non connecté", null));
        }
        catch (ArgumentException e) {
            return BadRequest(ResponseApi.Fail(e.Message, null));
        }
    }

// ---------- REFRESH TOKEN ----------
    [HttpPut("expand")]
    public async Task<IActionResult> Refresh() {
        try {
            string token = GetToken();

            await service.refreshToken(token);
            return Ok(ResponseApi.Ok("Token prolongé", null));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ResponseApi.Fail(ex.Message, null));
        }
    }

// ---------- GET PROFILE ----------
    [HttpGet("profile")]
    public async Task<IActionResult> GetProfile()
    {
        try {
            string token = GetToken();

            var infos = await service.getPersonnalInformations(token);
            return Ok(ResponseApi.Ok("Profil obtenu", infos));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ResponseApi.Fail(ex.Message, null));
        }
    }

// ---------- UPDATE PROFILE ----------
    [HttpPut("profile")]
    public async Task<IActionResult> UpdateProfile([FromBody] InfoUpdateDto dto)
    {
        try {
            string token = GetToken();

            await service.updateUserInformations(token, dto);
            return Ok(ResponseApi.Ok("Profil mis à jour", new { dto.email }));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ResponseApi.Fail(ex.Message, null));
        }
    }

// ---------- EXTRACT TOKEN FROM HEADER ----------
    private string GetToken()
    {
        string auth = Request.Headers.Authorization;

        if (string.IsNullOrEmpty(auth) || !auth.StartsWith("Bearer "))
            throw new ArgumentException("Token manquant dans la requête");

        return auth.Substring("Bearer ".Length);
    }
}
