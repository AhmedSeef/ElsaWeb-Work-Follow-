using Elsa.Workflows.Activities;
using Elsa.Workflows.Contracts;
using Elsa.Workflows;
using Elsa.Http;


namespace ElsaWeb.Workflows
{
    public class HttpHelloWorld : WorkflowBase
    {
        protected override void Build(IWorkflowBuilder builder)
        {
            builder.Root = new Sequence
            {
                Activities =
                {
                    new WriteHttpResponse
                    {
                        Content = new("Hello world of HTTP workflows!")
                    }
                }
            };
        }
    }
}
