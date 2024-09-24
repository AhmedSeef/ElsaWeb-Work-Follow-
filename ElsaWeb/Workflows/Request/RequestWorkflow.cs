using Elsa.Workflows.Activities;
using Elsa.Workflows.Contracts;
using Elsa.Workflows.Memory;
using Elsa.Workflows.Models;
using Elsa.Workflows;
using ElsaWeb.Data;
using ElsaWeb.Workflows.Request;

namespace ElsaWeb.Workflows.RequestWorkFollow
{
    public class RequestWorkflow : WorkflowBase
    {
        private readonly AppDbContext _dbContext;
        private readonly ElsaWeb.Models.Request _request;
        private readonly string _actionType;  // "add" or "update"

        // Constructor that accepts the dbContext, Request object, and actionType (add or update)
        public RequestWorkflow(AppDbContext dbContext, ElsaWeb.Models.Request request, string actionType)
        {
            _dbContext = dbContext;
            _request = request;
            _actionType = actionType;
        }

        protected override void Build(IWorkflowBuilder builder)
        {
            // Define a variable to store the action type
            var actionTypeVar = new Variable<string>(_actionType);

            builder.Root = new Sequence
            {
                Variables = { actionTypeVar },  // Add the actionType variable to the workflow

                Activities =
                {
                    // Use an If activity to branch based on the actionType variable
                    new If(context => actionTypeVar.Get(context) == "add")
                    {
                        Then = new Sequence
                        {
                            Activities =
                            {
                                // Add new request using RequestInsertActivity
                                new RequestInsertActivity(_dbContext)
                                {
                                    RequestInput = new Input<ElsaWeb.Models.Request>(_request)
                                }
                            }
                        },
                        Else = new Sequence
                        {
                            Activities =
                            {
                                 new RequestUpdateActivity(_dbContext)
                                    {
                                        RequestIdInput = new Input<int>(_request.Id),
                                        IsApprovedInput = new Input<bool>(_request.IsApproved)
                                    }
                            }
                        }
                    }
                }
            };
        }
    }
}
