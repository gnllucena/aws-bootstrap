using Amazon.CDK;
using Amazon.CDK.AWS.EC2;
using Amazon.CDK.AWS.Events;
using Infrastructure.Stacks;

namespace Src
{
    sealed class Program
    {
        public static void Main(string[] args)
        {
            var app = new App();

            var props = new StackProps()
            {
                Env = new Environment()
                {
                    Account = "353697730469",
                    Region = "us-east-1"
                }
            };

            var network = new NetworkStack(app, "companies", props);

            new FrontendStack(app, "companies", "zro17.com");

            new DatabaseStack(app, "companies", network.GetVpc(), props);
            
            new SchedulerStack(app, "regulatory-sec-update", 128, new CronOptions()
            {
                Minute = "*/5",
                Hour = "*",
                Day = "*",
                Month = "*",
                Year = "*"
            }, network.GetVpc(), props);
            
            new ConsumerStack(app, "regulatory-sec-reader", 128, network.GetVpc(), props);
            new ConsumerStack(app, "regulatory-sec-parser", 128, network.GetVpc(), props);
            new ConsumerStack(app, "calculations-indexes", 128, network.GetVpc(), props);

            app.Synth();
        }
    }
}
