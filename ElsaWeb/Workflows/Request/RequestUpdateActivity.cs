using Elsa.Workflows.Models;
using Elsa.Workflows;
using ElsaWeb.Data;
using Elsa.Extensions;

namespace ElsaWeb.Workflows.Request
{
    public class RequestUpdateActivity : CodeActivity<string>
    {
        private readonly AppDbContext _dbContext;
        public Input<int> RequestIdInput { get; set; } = default!;
        public Input<bool> IsApprovedInput { get; set; } = default!;

        public RequestUpdateActivity(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        protected override void Execute(ActivityExecutionContext context)
        {
            var requestId = RequestIdInput.Get(context);
            var isApproved = IsApprovedInput.Get(context);

            var existingRequest = _dbContext.Requests.Find(requestId);

            if (existingRequest != null)
            {
                existingRequest.IsApproved = isApproved;
                existingRequest.Status = isApproved ? "Approved" : "Rejected";

                _dbContext.SaveChanges();
                var resultMessage = isApproved ? "Request Approved Successfully" : "Request Rejected";
                context.SetResult(resultMessage);
            }
            else
            {
                context.SetResult("Request Not Found");
            }
        }
    }

}
