using Elsa.Workflows;
using Elsa.Workflows.Activities;
using Elsa.Workflows.Contracts;
using Elsa.Workflows.Memory;
using Elsa.Workflows.Models;

namespace ElsaWeb.Workflows
{
    public class MyWorkflow : WorkflowBase
    {
        public string GreetingResult { get; private set; } = string.Empty;
        private readonly string _name;

        public MyWorkflow(string name)
        {
            _name = name;
        }

        protected override void Build(IWorkflowBuilder builder)
        {
            var greeting = new Variable<string>();

            builder.Root = new Sequence
            {
                Variables = { greeting },

                Activities =
                {
                    // Pass the dynamic name from the controller
                    new Greeter
                    {
                        Name = new Input<string>(_name),
                        Result = new Output<string>(greeting)
                    },

                    new WriteLine(context =>
                    {
                        GreetingResult = greeting.Get(context);  
                        return $"The greeting is: {GreetingResult}";
                    })
                }
            };
        }
    }
}
