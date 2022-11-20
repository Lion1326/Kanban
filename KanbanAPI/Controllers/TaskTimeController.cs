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
    public class TaskTimeController : Controller
    {
        private ITaskTimeRepository taskTimeRepository;
        private IUserRepository userRepository;
        public TaskTimeController(
            ITaskTimeRepository taskTimeRepository,
            IUserRepository userRepository
            )
        {
            this.taskTimeRepository = taskTimeRepository;
            this.userRepository = userRepository;
        }

        //Добавление и изменение списанного времени
        [HttpPost]
        public async Task<IActionResult> Push([FromBody] TaskTime request)
        {
            if (request.ID == 0)
                taskTimeRepository.AddPart(request);
            else
                taskTimeRepository.UpdatePart(request);

            await taskTimeRepository.SaveChangesAsync();
            request.User = userRepository.GetByID(request.UserID);
            return Json(request);
        }

        //Удаление списанного времени
        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] TaskTime request)
        {
            TaskTime model = taskTimeRepository.GetByID(request.ID);
            if (model != null)
            {
                taskTimeRepository.DeletePart(model);
            }

            await taskTimeRepository.SaveChangesAsync();
            return Ok();
        }
    }
}