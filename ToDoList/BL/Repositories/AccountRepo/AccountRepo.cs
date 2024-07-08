using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ToDoList.DAL.Entites;
using ToDoList.DTOs;

namespace ToDoList.BL.Repositories.AccountRepo
{
    public class AccountRepo : IAccountRepo
    {
        private UserManager<ApplicationUser> _UserManager;
        private ApplicationUser User { get; set; }
        public AccountRepo(UserManager<ApplicationUser> userManager)
        {
            _UserManager = userManager;
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
    }
}
