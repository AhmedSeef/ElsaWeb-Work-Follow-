using Elsa.Workflows.Activities;
using Elsa.Workflows.Contracts;
using ElsaWeb.Workflows;
using Microsoft.AspNetCore.Mvc;

namespace ElsaWeb.Controllers
{
    public class RunWorkflowController : Controller
    {
        private readonly IWorkflowRunner _workflowRunner;

        public RunWorkflowController(IWorkflowRunner workflowRunner)
        {
            _workflowRunner = workflowRunner;
        }

        [HttpGet]
        public async Task<IActionResult> RunWorkflow()
        {
            await _workflowRunner.RunAsync<HttpHelloWorld>();
            return Content("Workflow executed successfully. Check the response in the browser.");
        }

        [HttpGet]
        public async Task<IActionResult> RunWorkflow1(string name)
        {
            name = string.IsNullOrEmpty(name) ? "World" : name;

            // Create and run the workflow, passing the dynamic name
            var workflow = new MyWorkflow(name);
            await _workflowRunner.RunAsync(workflow);

            // Retrieve the result from the workflow's GreetingResult property
            var greeting = workflow.GreetingResult;

            return Content(greeting);
        }


    }
}
