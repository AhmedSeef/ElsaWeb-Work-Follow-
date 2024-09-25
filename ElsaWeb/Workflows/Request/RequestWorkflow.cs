using Elsa.Workflows.Activities;
using Elsa.Workflows.Contracts;
using Elsa.Workflows.Memory;
using Elsa.Workflows.Models;
using Elsa.Workflows;
using ElsaWeb.Data;
using ElsaWeb.Workflows.Request;
using ElsaWeb.Models;

namespace ElsaWeb.Workflows.RequestWorkFollow
{
    public class RequestWorkflow : WorkflowBase
    {
        private readonly AppDbContext _dbContext;
        private readonly ElsaWeb.Models.Request _request;
        private readonly string _actionType;

        public RequestWorkflow(AppDbContext dbContext, ElsaWeb.Models.Request request, string actionType)
        {
            _dbContext = dbContext;
            _request = request;
            _actionType = actionType;
        }

        protected override void Build(IWorkflowBuilder builder)
        {
            var actionTypeVar = new Variable<string>(_actionType);

            builder.Root = new Sequence
            {
                Variables = { actionTypeVar },

                Activities =
                {
                    new If(context => actionTypeVar.Get(context) == "add")
                    {
                        Then = new Sequence
                        {
                            Activities =
                            {
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
                                    IsApprovedInput = new Input<bool?>((bool?)_request.IsApproved),
                                    NewStatusInput = new Input<RequestStatus>(_request.Status)
                                }
                            }
                        }
                    } 
                }
            };
        }
    }
}