using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using KanbanAPI.App_Code;
using KanbanAPI.App_Code.Models;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authentication.OAuth;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace KanbanAPI.Controllers
{

    [Authorize]
    [Route("{controller}")]
    public class IssuesController : Controller
    {
        private IIssueRepository issueRepository;
        public IssuesController(IIssueRepository issueRepository)
        {
            this.issueRepository = issueRepository;
        }
        public class IssueRequest
        {
            public int? Id { get; set; }
            public string Name { get; set; }
            public int CreatorID { get; set; }
            public DateTime CreationDate { get; set; }
            public int? WorkerID { get; set; }
            public DateTime? StartDate { get; set; } = null;
            public DateTime? FinishDate { get; set; }
            public int StatusID { get; set; }
            public string Description { get; set; }
        }
        //Добавление и редактирование Issue
        [HttpPost]
        public async Task<ActionResult> PushIssue([FromBody] Issue request)
        {
            var getIssue = await issueRepository.GetIssueByID(request.ID);

            if (getIssue == null)
            {
                Issue issues = new()
                {
                    Name = request.Name,
                    CreatorID = request.CreatorID,
                    CreationDate = request.CreationDate,
                    WorkerID = request.WorkerID,
                    StartDate = request.StartDate,
                    FinishDate = request.FinishDate,
                    StatusID = request.StatusID,
                    Description = request.Description,
                };

                issueRepository.AddPart(issues);
            }
            else
            {
                getIssue.Name = request.Name;
                getIssue.CreatorID = request.CreatorID;
                getIssue.CreationDate = request.CreationDate;
                getIssue.WorkerID = request.WorkerID;
                getIssue.StartDate = request.StartDate;
                getIssue.FinishDate = request.FinishDate;
                getIssue.StatusID = request.StatusID;
                getIssue.Description = request.Description;

                issueRepository.UppdatePart(getIssue);
            }

            await issueRepository.SaveChangesAsync();
            return Ok();
        }
        public class IssueStatusChangeRequest
        {
            public int Id { get; set; }
            public int StatusID { get; set; }
        }
        //Изменение статуса
        [HttpPost("statuschange")]
        public async Task<ActionResult> StatusChange([FromBody] IssueStatusChangeRequest request)
        {
            var issue = await issueRepository.GetIssueByID(request.Id);
            issue.StatusID = request.StatusID;
            issueRepository.UppdatePart(issue);
            await issueRepository.SaveChangesAsync();
            return Json(issue);
        }

        //Удаление Issue
        [HttpDelete]
        public async Task<ActionResult> DeleteIssue([FromBody] IssueRequest request)
        {
            var getIssue = await issueRepository.GetIssueByID(request.Id);

            issueRepository.DeletePart(getIssue);

            await issueRepository.SaveChangesAsync();
            return Ok();
        }
        //Получение списка всех Issue из БД, добавляя к ним таблицы User и TaskTime
        [HttpPost("list")]
        public IActionResult GetListIssues()
        {
            List<Issue> issues = issueRepository.GetList()
                .Include(x => x.Worker)
                .Include(x => x.Creator)
                .Include(x => x.TaskTimes)
                .ThenInclude(x=>x.User)
                .ToList();

            return Json(issues);
        }
    }
}
