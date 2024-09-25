using Elsa.Workflows.Models;
using Elsa.Workflows;
using Elsa.Extensions;
using ElsaWeb.Data;

namespace ElsaWeb.Workflows.RequestWorkFollow
{
    public class RequestInsertActivity : CodeActivity<string>
    {
        private readonly AppDbContext _dbContext;
        public Input<ElsaWeb.Models.Request> RequestInput { get; set; } = default!;

        public RequestInsertActivity(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        protected override void Execute(ActivityExecutionContext context)
        {
            try
            {
                var request = RequestInput.Get(context);
                _dbContext.Requests.Add(request);
                _dbContext.SaveChanges();

                var message = request.IsApproved ? "Request Created Successfully" : "Request Creation Failed";
                context.SetResult(message);
            }
            catch (Exception ex)
            {
                context.SetResult($"Error: {ex.Message}");
            }
        }
    }
}
