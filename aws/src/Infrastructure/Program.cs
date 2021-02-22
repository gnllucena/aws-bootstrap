using Amazon.CDK;
using Amazon.CDK.AWS.Events;
using Infrastructure.Stacks;

namespace Src
{
    sealed class Program
    {
        public static void Main(string[] args)
        {
            var app = new App();

            new FrontendStack(app, "stocks", "zro17.com");
            new SchedulerStack(app, "regulatory-sec-update", 256, new CronOptions() { 
                Minute = "*/5",
                Hour = "*",
                Day = "*",
                Month = "*",
                Year = "*"
            });
            new ConsumerStack(app, "regulatory-sec-reader", 256);
            //new ConsumerStack(app, "regulatory-sec-parser");
            //new ConsumerStack(app, "calculations-indexes");


            app.Synth();
        }
    }
}
