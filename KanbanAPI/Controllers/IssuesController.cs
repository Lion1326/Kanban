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


namespace KanbanAPI.Controllers
{
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
            public DateTime? StartDate { get; set; }
            public DateTime? FinishDate { get; set; }
            public int StatusID { get; set; }
            public string Description { get; set; }
        }

        [HttpPost("issue/create")]
        public async Task<ActionResult> AddIssue([FromBody] IssueRequest request)
        {
            var getIssue = await issueRepository.GetIssueByID(request.Id);

            if (getIssue == null)
            {
                Issues issues = new()
                {
                    Name = request.Name,
                    CreatorID = request.CreatorID,
                    CreationDate = request.CreationDate,
                    WorkerID = request.WorkerID.Value,
                    StartDate = request.StartDate.Value,
                    FinishDate = request.FinishDate.Value,
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
                getIssue.WorkerID = request.WorkerID.Value;
                getIssue.StartDate = request.StartDate.Value;
                getIssue.FinishDate = request.FinishDate.Value;
                getIssue.StatusID = request.StatusID;
                getIssue.Description = request.Description;

                issueRepository.UppdatePart(getIssue);
            }

            await issueRepository.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("issue/delete")]
        public async Task<ActionResult> DeleteIssue([FromBody] IssueRequest request)
        {
            var getIssue = await issueRepository.GetIssueByID(request.Id);

            issueRepository.DeletePart(getIssue);

            await issueRepository.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("list")]
        public IActionResult GetListIssues()
        {
            List<Issues> issues = issueRepository.GetList().ToList();

            return Json(issues);
        }
     }
}
