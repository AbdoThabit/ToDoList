using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ToDoList.BL.Helpers;
using ToDoList.DAL.Entites;
using ToDoList.DTOs;

namespace ToDoList.BL.Repositories.AccountRepo
{
    public class AccountRepo : IAccountRepo
    {
        private UserManager<ApplicationUser> _UserManager;
        private ApplicationUser User { get; set; }
        private readonly JWT _jwt;
        public AccountRepo(UserManager<ApplicationUser> userManager , IOptions<JWT> jwt)
        {
            _UserManager = userManager;
            _jwt = jwt.Value;
            Debug.WriteLine($"JWT Key: {_jwt.Key}");
            Debug.WriteLine($"JWT Issuer: {_jwt.Issuer}");
            Debug.WriteLine($"JWT Audience: {_jwt.Audience}");
            Debug.WriteLine($"JWT DurationInDays: {_jwt.DurationInDays}");
        }

        

    
        public async Task<IdentityResult> Add(UserRegistrationDto userRegistrationDto)
        {
             User = new ApplicationUser()
            {
                UserName = userRegistrationDto.UserName,
                Email = userRegistrationDto.Email,
                FullName = userRegistrationDto.FullName,
                PhoneNumber = userRegistrationDto.PhoneNumber,
            };
            var state = await _UserManager.CreateAsync(User, userRegistrationDto.Password);
            return state;
        }

        public async Task<UserInfo> getById(string id)
        {
            User = await _UserManager.FindByIdAsync(id);
            if (User == null) return null;
            else
            {
                UserInfo userInfo = new UserInfo()
                {
                    FullName = User.FullName,
                    UserName = User.UserName,
                    PhoneNumber = User.PhoneNumber,
                    Email = User.Email,
                };
                return userInfo;
            }
        }

        public async Task<List<UserInfo>> getAll()
        {
            var users = await _UserManager.Users.ToListAsync();
            if (users.Count == 0 )return null;
            else
            {
                List<UserInfo> usersInfoList = new List<UserInfo>();
                UserInfo userInfo;
                foreach (var user in users)
                {
                    userInfo = new UserInfo()
                    {
                        FullName = user.FullName,
                        UserName = user.UserName,
                        PhoneNumber = user.PhoneNumber,
                        Email = user.Email,
                    };
                    usersInfoList.Add(userInfo);
                }
                return usersInfoList;
            }

        }

        public async Task<IdentityResult> delete(String id)
        {
            User = User = await _UserManager.FindByIdAsync(id);
            
            if (User != null)
            {
                var state = await _UserManager.DeleteAsync(User);
                return state;
            }
            return IdentityResult.Failed(new IdentityError { Description = "User not found." });
        }

        public async Task<IdentityResult> edit(UserInfo userInfo,String id) 
        {

            User = await _UserManager.FindByIdAsync(id);
            if (User != null)
            {
                User.FullName = userInfo.FullName;
                User.PhoneNumber = userInfo.PhoneNumber;
                User.Email = userInfo.Email;
                User.UserName = userInfo.UserName;
                var state = await _UserManager.UpdateAsync(User);
                return state;
            }
            else return IdentityResult.Failed(new IdentityError { Description = "User not found." });

        }

        public async Task<IdentityResult> changePassword(ChanagePasswordDto passwordDto)
        {
            User = await _UserManager.FindByIdAsync(passwordDto.UserId);
            if (User != null)
            {
                var state = await _UserManager.ChangePasswordAsync(User, passwordDto.CurrentPassword, passwordDto.Password);
                return state;
            }
            else return IdentityResult.Failed(new IdentityError { Description = "User not found." });
        }
        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {
            var userClaims = await _UserManager.GetClaimsAsync(user);
            var roles = await _UserManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(_jwt.DurationInDays),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }

        public async Task<AuthModel> RegisterAsync(UserRegistrationDto userRegistrationDto)
        {
            if (await _UserManager.FindByEmailAsync(userRegistrationDto.Email) is not null)
                return new AuthModel { Message = "Email is already registered!" };

            if (await _UserManager.FindByNameAsync(userRegistrationDto.UserName) is not null)
                return new AuthModel { Message = "Username is already registered!" };

            var user = new ApplicationUser
            {
                UserName = userRegistrationDto.UserName,
                Email = userRegistrationDto.Email,
                FullName = userRegistrationDto.FullName,
                PhoneNumber = userRegistrationDto.PhoneNumber,
            };

            var result = await _UserManager.CreateAsync(user, userRegistrationDto.Password);

            if (!result.Succeeded)
            {
                var errors = string.Empty;

                foreach (var error in result.Errors)
                    errors += $"{error.Description},";

                return new AuthModel { Message = errors };
            }

            await _UserManager.AddToRoleAsync(user, "User");

            var jwtSecurityToken = await CreateJwtToken(user);

            return new AuthModel
            {
                Email = user.Email,
                
                ExpiresOn = jwtSecurityToken.ValidTo,
                IsAuthenticated = true,
                Roles = new List<string> { "User" },
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                Username = user.UserName
            };
        }
        public async Task<AuthModel> GetToken(LoginDto loginDto)
        {
            var authModel = new AuthModel();

            var user = await _UserManager.FindByEmailAsync(loginDto.Email);

            if (user is null || ! await _UserManager.CheckPasswordAsync(user, loginDto.Password))
            {
                authModel.Message = "Email or Password is incorrect!";
                return authModel;
            }
            var jwtSecurityToken = await CreateJwtToken(user);
            var rolesList = await _UserManager.GetRolesAsync(user);

            authModel.IsAuthenticated = true;
            authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            authModel.Email = user.Email;
            authModel.Username = user.UserName;
            authModel.ExpiresOn = jwtSecurityToken.ValidTo;
            authModel.Roles = rolesList.ToList();

            return authModel;
        }

    }
}
