using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Peabux.Domain.Dtos;
using Peabux.Domain.Entities;
using Peabux.Domain.Exceptions;
using System.ComponentModel.DataAnnotations;

namespace Peabux.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;
    private readonly IDbContextFactory<AppDBContext> _dbContext;
    private readonly IJwtFactory _jwtFactory;

    private User? _user;

    public AuthService(IMapper mapper, UserManager<User> userManager, IDbContextFactory<AppDBContext> dbContext, IJwtFactory jwtFactory)
    {
        _mapper = mapper;
        _userManager = userManager;
        _jwtFactory = jwtFactory;
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
        var claims = await _jwtFactory.GenerateClaims(_user,"");
        var accessToken = await _jwtFactory.GenerateToken(claims);
        var refreshToken = _jwtFactory.GenerateRefreshToken();

        _user.RefreshToken = refreshToken;

        if (populateExp)
            _user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);

        await _userManager.UpdateAsync(_user);
        //accessToken, refreshToken
        return new TokenDto() { AccessToken=accessToken, RefreshToken=refreshToken };
    }

    public async Task<TokenDto> RefreshToken(TokenDto tokenDto)
    {
        var principal = _jwtFactory.GetPrincipalFromExpiredToken(tokenDto.AccessToken);

        var user = await _userManager.FindByNameAsync(principal.Identity.Name);
        if (user == null || user.RefreshToken != tokenDto.RefreshToken ||
            user.RefreshTokenExpiryTime <= DateTime.Now)
        {
            return null;
        }
        else
        {
            _user = user;
            return await CreateToken(populateExp: false);
        }
        
    }



   
}
