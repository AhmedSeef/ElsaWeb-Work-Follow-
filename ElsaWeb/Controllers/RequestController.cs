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

        // Display all requests
        public async Task<IActionResult> Index()
        {
            var requests = await _dbContext.Requests.ToListAsync();
            return View(requests);
        }

        // Insert a new request with "submitted" status
        [HttpPost]
        public async Task<IActionResult> InsertRequest()
        {
            var random = new Random();
            var request = new Request
            {
                Name = $"Request_{random.Next(1, 1000)}",
                Status = RequestStatus.Submitted
            };

            var workflow = new RequestWorkflow(_dbContext, request, "add");
            await _workflowRunner.RunAsync(workflow);

            return RedirectToAction(nameof(Index));
        }

        // Assign the request and update status to "Assigned"
        [HttpPost]
        public async Task<IActionResult> AssignRequest(int requestId)
        {
            var request = await _dbContext.Requests.FindAsync(requestId);
            if (request == null)
                return NotFound();

            request.Status = RequestStatus.Assigned;

            var workflow = new RequestWorkflow(_dbContext, request, "update");
            await _workflowRunner.RunAsync(workflow);

            return RedirectToAction(nameof(Index));
        }

        // Approve a request and update status to "Approved"
        [HttpPost]
        public async Task<IActionResult> ApproveRequest(int requestId)
        {
            var request = await _dbContext.Requests.FindAsync(requestId);
            if (request == null)
                return NotFound();

            request.IsApproved = true;
            request.Status = RequestStatus.Approved;

            var workflow = new RequestWorkflow(_dbContext, request, "update");
            await _workflowRunner.RunAsync(workflow);

            return RedirectToAction(nameof(Index));
        }

        // Reject a request and update status to "Rejected"
        [HttpPost]
        public async Task<IActionResult> RejectRequest(int requestId)
        {
            var request = await _dbContext.Requests.FindAsync(requestId);
            if (request == null)
                return NotFound();

            request.IsApproved = false;
            request.Status = RequestStatus.Rejected;

            var workflow = new RequestWorkflow(_dbContext, request, "update");
            await _workflowRunner.RunAsync(workflow);

            return RedirectToAction(nameof(Index));
        }
    }
}
