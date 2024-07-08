using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ToDoList.DAL.Entites;
using ToDoList.BL.Repositories;
using ToDoList.DTOs;
using ToDoList.BL.Repositories.AccountRepo;

namespace ToDoList.Controllers
{
    
    [ApiController]
    public class AccountController : Controller
    {
        private UserManager<ApplicationUser> _userManager;
        private IAccountRepo _accountRepo;
        public AccountController(UserManager<ApplicationUser> userManager , IAccountRepo accountRepo )
        {
            _userManager = userManager;
            _accountRepo = accountRepo;
        }

        
        [HttpPost("/AddUser")]

        public  async Task<ActionResult> add(UserRegistrationDto user)
        {
            if (!ModelState.IsValid) return BadRequest();
            else
            {
                var state = await _accountRepo.Add(user);
                if (state.Succeeded)
                    return Ok(state);
                else return BadRequest(state);
            }

        }
        [HttpPost("/UpdateUser")]

        public async Task<ActionResult> Update(UserInfo userInfo,String id)
        {
            if (!ModelState.IsValid) return BadRequest();

            else
            {
                var state = await _accountRepo.edit(userInfo, id);
                if (state.Succeeded)
                    return Ok(state);
                else return BadRequest(state);
            }

        }
        [HttpPost("/ChangePassword")]

        public async Task<ActionResult> ChangePassword(ChanagePasswordDto chanagePasswordDto)
        {
            if (!ModelState.IsValid) return BadRequest();

            else
            {
                var state = await _accountRepo.changePassword(chanagePasswordDto);
                if (state.Succeeded)
                    return Ok(state);
                else return BadRequest(state);
            }

        }
        [HttpGet("/GetUser")]

        public async Task<ActionResult> getById(String UserID)
        {
            if (!ModelState.IsValid) return BadRequest();
            else
            {
                var userInfo = await _accountRepo.getById(UserID);
                if (userInfo != null )
                    return Ok(userInfo);
                else return NotFound();
            }


        }
        [HttpGet("/Get")]

        public async Task<ActionResult> getAll()
        {
            
                var usersInfo = await _accountRepo.getAll();
                if (usersInfo != null)
                    return Ok(usersInfo);
                else return NotFound();
          

        }
        [HttpDelete("/DeleteUser")]

        public async Task<ActionResult> Delete(String USerID)
        {
            if (!ModelState.IsValid) return BadRequest();
            else
            {
                var state = await _accountRepo.delete(USerID);
                if (state.Succeeded)
                    return Ok(state.Succeeded);
                else return NotFound(state.Errors);
            }
        }
    }
}
