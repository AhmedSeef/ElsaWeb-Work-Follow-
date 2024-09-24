using Elsa.Extensions;
using Elsa.Workflows;
using Elsa.Workflows.Models;

namespace ElsaWeb.Workflows
{
    public class Greeter : CodeActivity<string>
    {
        public Input<string> Name { get; set; } = default!;

        protected override void Execute(ActivityExecutionContext context)
        {
            var name = Name.Get(context);
            var message = $"Hello, {name}!";
            context.SetResult(message);  // Set the result here
        }
    }
}
