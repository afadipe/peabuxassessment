using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Win32;
using Peabux.Domain.Dtos;
using Peabux.Domain.Entities;
using Peabux.Domain.Exceptions;
using Peabux.Domain.Options;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Peabux.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;
    private readonly IOptions<JwtConfigOption> _configuration;
    private readonly JwtConfigOption _jwtConfiguration;
    private readonly IDbContextFactory<AppDBContext> _dbContext;

    private User? _user;

    public AuthService(IMapper mapper, UserManager<User> userManager,
        IOptions<JwtConfigOption> configuration, IDbContextFactory<AppDBContext> dbContext)
    {
        _mapper = mapper;
        _userManager = userManager;
        _configuration = configuration;
        _jwtConfiguration = _configuration.Value;
        _dbContext = dbContext;
    }

    public async Task<IdentityResult> RegisterUser(RegistrationDto register)
    {
        await using var context = await _dbContext.CreateDbContextAsync();

        if (_userManager.Users.Any(x => x.NationalIdNumber.ToUpper() == register.NationalIdNumber.ToUpper()))
            throw new ValidationException("A user with the specified NationalId Number already exist!");

        await ValidateUserPersonalNumber(register, context);

        var user = _mapper.Map<User>(register);
        await using (var transaction = context.Database.BeginTransaction())
        {
            try
            {
                var result = await _userManager.CreateAsync(user, register.Password);

                if (result.Succeeded)
                    await _userManager.AddToRoleAsync(user, register.Role);

                //then do an insert into teacher and student table base on the role
                if(register.Role.ToUpper() == "TEACHER")
                {
                    var teacherDto = _mapper.Map<Teacher>(register);
                    teacherDto.UserId = user.Id;
                    await context.AddAsync(teacherDto);
                }
                else
                {
                    var studentDto = _mapper.Map<Student>(register);
                    studentDto.UserId = user.Id;
                    await context.AddAsync(studentDto);
                }
                await context.SaveChangesAsync();
                await transaction.CommitAsync();
                return result;

            }
            catch(Exception ex)
            {
                await transaction.RollbackAsync();
                await _userManager.RemoveFromRoleAsync(user, register.Role);
                await _userManager.DeleteAsync(user);
                throw new ValidationException(ex.Message);
            }
        }
        
    }

    public static async Task ValidateUserPersonalNumber(RegistrationDto register,AppDBContext context)
    {
        if (register.Role.ToUpper() == "TEACHER")
        {
            if (context.Teachers.AsNoTracking().Any(x => x.TeacherNumber.ToUpper() == register.TeacherNumber.ToUpper()))
                throw new ValidationException("A Teacher with the specified Teacher Number already exist!");
        }
        else
        {
            if (context.Students.AsNoTracking().Any(x => x.StudentNumber.ToUpper() == register.StudentNumber.ToUpper()))
                throw new ValidationException("A Student with the specified Student Number already exist!");
        }
    }

    public async Task<bool> ValidateUser(LoginRequestDto login)
    {
        _user = await _userManager.FindByNameAsync(login.UserName);

        var result = (_user != null && await _userManager.CheckPasswordAsync(_user, login.Password));
        if (!result)
            return false;
        return result;
    }

    public async Task<TokenDto> CreateToken(bool populateExp)
    {
        var signingCredentials = GetSigningCredentials();
        var claims = await GetClaims();
        var tokenOptions = GenerateTokenOptions(signingCredentials, claims);

        var refreshToken = GenerateRefreshToken();

        _user.RefreshToken = refreshToken;

        if (populateExp)
            _user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);

        await _userManager.UpdateAsync(_user);

        var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

        return new TokenDto(accessToken, refreshToken);
    }

    public async Task<TokenDto> RefreshToken(TokenDto tokenDto)
    {
        var principal = GetPrincipalFromExpiredToken(tokenDto.AccessToken);

        var user = await _userManager.FindByNameAsync(principal.Identity.Name);
        if (user == null || user.RefreshToken != tokenDto.RefreshToken ||
            user.RefreshTokenExpiryTime <= DateTime.Now)
            throw  new RefreshTokenBadRequest();

        _user = user;

        return await CreateToken(populateExp: false);
    }

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }

    private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfiguration.Secret)),
            ValidateLifetime = true,
            ValidIssuer = _jwtConfiguration.ValidIssuer,
            ValidAudience = _jwtConfiguration.ValidAudience
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        SecurityToken securityToken;
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);

        var jwtSecurityToken = securityToken as JwtSecurityToken;
        if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
            StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token");
        }

        return principal;
    }

    private SigningCredentials GetSigningCredentials()
    {
        var key = Encoding.UTF8.GetBytes(_jwtConfiguration.Secret);
        var secret = new SymmetricSecurityKey(key);
        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
    }

    private async Task<List<Claim>> GetClaims()
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, _user.UserName)
        };
        var roles = await _userManager.GetRolesAsync(_user);
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        return claims;
    }

    private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
    {
        var tokenOptions = new JwtSecurityToken
        (
            issuer: _jwtConfiguration.ValidIssuer,
            audience: _jwtConfiguration.ValidAudience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(_jwtConfiguration.Expires)),
            signingCredentials: signingCredentials
        );

        return tokenOptions;
    }

   
}
