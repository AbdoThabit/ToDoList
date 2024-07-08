using Microsoft.AspNetCore.Identity;
using ToDoList.DTOs;

namespace ToDoList.BL.Repositories.AccountRepo
{
    public interface IAccountRepo

    {
        public Task<List<UserInfo>> getAll();
        public Task<UserInfo> getById(String id);
        public Task<IdentityResult>  Add(UserRegistrationDto userRegistrationDto);
        public Task<IdentityResult> edit(UserInfo userInfo, String Id);
        public Task<IdentityResult> delete(String id);
        public Task<IdentityResult> changePassword(ChanagePasswordDto password);

    }
}
