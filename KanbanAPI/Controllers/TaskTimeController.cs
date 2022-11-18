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
        public TaskTimeController(
            ITaskTimeRepository taskTimeRepository
            )
        {
            this.taskTimeRepository = taskTimeRepository;
        }


        [HttpPost]
        public async Task<IActionResult> Push([FromBody] TaskTime request)
        {
            if (request.ID > 0)
                taskTimeRepository.AddPart(request);
            else
                taskTimeRepository.UpdatePart(request);

            await taskTimeRepository.SaveChangesAsync();
            return Json(request);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            TaskTime model = taskTimeRepository.GetByID(id);
            if (model != null)
            {
                taskTimeRepository.DeletePart(model);
            }

            await taskTimeRepository.SaveChangesAsync();
            return Ok();
        }
    }
}