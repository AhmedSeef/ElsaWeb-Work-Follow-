using Elsa.Workflows.Models;
using Elsa.Workflows;
using ElsaWeb.Data;
using Elsa.Extensions;
using ElsaWeb.Models;

namespace ElsaWeb.Workflows.Request
{
    public class RequestUpdateActivity : CodeActivity<string>
    {
        private readonly AppDbContext _dbContext;
        public Input<int> RequestIdInput { get; set; } = default!;
        public Input<bool?> IsApprovedInput { get; set; } = default!;
        public Input<RequestStatus> NewStatusInput { get; set; } = default!;

        public RequestUpdateActivity(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        protected override void Execute(ActivityExecutionContext context)
        {
            try
            {
                var requestId = RequestIdInput.Get(context);
                var newStatus = NewStatusInput.Get(context);
                var isApproved = IsApprovedInput.Get(context);

                var existingRequest = _dbContext.Requests.Find(requestId);

                if (existingRequest != null)
                {
                    // Update the status based on the input
                    existingRequest.Status = newStatus;

                    // If we are approving/rejecting, also update the IsApproved field
                    if (newStatus == RequestStatus.Approved || newStatus == RequestStatus.Rejected)
                    {
                        if (isApproved.HasValue)
                        {
                            existingRequest.IsApproved = isApproved.Value;
                        }
                    }

                    _dbContext.SaveChanges();

                    var resultMessage = newStatus switch
                    {
                        RequestStatus.Assigned => "Request Assigned Successfully",
                        RequestStatus.Approved => "Request Approved Successfully",
                        RequestStatus.Rejected => "Request Rejected",
                        _ => "Status Updated"
                    };

                    context.SetResult(resultMessage);
                }
                else
                {
                    context.SetResult("Request Not Found");
                }
            }
            catch (Exception ex)
            {
                context.SetResult($"Error: {ex.Message}");
            }
        }
    }
}
