using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authentication.OAuth;
using KanbanAPI.App_Code.Models;
using KanbanAPI.App_Code;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace KanbanAPI.Controllers
{
    [Authorize]
    [Route("{controller}")]
    public class UsersController : Controller
    {
        private IUserRepository userRepository;
        public UsersController(
            IUserRepository userRepository
            )
        {
            this.userRepository = userRepository;
        }

        //получение списка юзеров
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Json(userRepository.GetList());
        }
    }
}