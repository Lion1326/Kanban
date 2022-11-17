using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using KanbanAPI.App_Code.Models;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace KanbanAPI.Controllers
{
    public class IssuesController : Controller
    {
        private readonly ILogger<IssuesController> _logger;
        IssuesRepository _context;
        
        public IssuesController(ILogger<IssuesController> logger, IssuesRepository context)
        {
            _logger = logger;
            _context = context;
        }
        // создание
        public IActionResult Index()
        {
            return View(_context.Issues.ToList());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost(Name = "create")]
        public IActionResult Create(KanbanAPI.App_Code.Models.Issues issues)
        {
            if (ModelState.IsValid)
            {
                _context.Issues.Add(issues);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return View(issues);
            }
        }
        // редактирование
        public async Task<IActionResult> Edit(int id)
        {

            KanbanAPI.App_Code.Models.Issues issues = await _context.Issues.FindAsync(id);
            if (issues != null)
                return View(issues);

            return RedirectToAction("Create");
        }
        [HttpPost(Name = "edit")]
        public async Task<IActionResult> Edit(KanbanAPI.App_Code.Models.Issues issues)
        {
            _context.Issues.Update(issues);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        // удаление
        [HttpDelete(Name = "delete")]
        public async Task<IActionResult> Delete(int id)
        {

            KanbanAPI.App_Code.Models.Issues issues = await _context.Issues.FindAsync(id);
            if (_context.Issues.Where(p => p.ID == id).Count() > 0)
            {
                return BadRequest("Issue working on a task");
            }

            else
            {
                if (issues != null)
                {
                    _context.Issues.Remove(issues);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }

            return NotFound();
        }

        
    }
}
