using Elsa.Workflows.Contracts;
using ElsaWeb.Data;
using ElsaWeb.Models;
using ElsaWeb.Workflows.RequestWorkFollow;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ElsaWeb.Controllers
{
    public class RequestController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly IWorkflowRunner _workflowRunner;

        public RequestController(AppDbContext dbContext, IWorkflowRunner workflowRunner)
        {
            _dbContext = dbContext;
            _workflowRunner = workflowRunner;
        }

        public async Task<IActionResult> Index()
        {
            var requests = await _dbContext.Requests.ToListAsync();  
            return View(requests); 
        }

        [HttpPost]
        public async Task<IActionResult> InsertRequest()
        {
            var random = new Random();
            var request = new Request
            {
                Name = $"Request_{random.Next(1, 1000)}", 
                IsApproved = random.Next(2) == 1,      
                Status = "Created"
            };

           
            var workflow = new RequestWorkflow(_dbContext, request, "add");
            await _workflowRunner.RunAsync(workflow);

            return RedirectToAction(nameof(Index));
        }

        // Approve a request
        [HttpPost]
        public async Task<IActionResult> ApproveRequest(int requestId)
        {
            var request = await _dbContext.Requests.FindAsync(requestId);
            if (request == null)
                return NotFound();

            request.IsApproved = true;

            var workflow = new RequestWorkflow(_dbContext, request, "update");
            await _workflowRunner.RunAsync(workflow);

            return RedirectToAction(nameof(Index));
        }

        // Reject a request
        [HttpPost]
        public async Task<IActionResult> RejectRequest(int requestId)
        {
            var request = await _dbContext.Requests.FindAsync(requestId);
            if (request == null)
                return NotFound();

            request.IsApproved = false;
            request.Status = "Rejected";
            var workflow = new RequestWorkflow(_dbContext, request, "update");
            await _workflowRunner.RunAsync(workflow);

            return RedirectToAction(nameof(Index));
        }
    }
}
