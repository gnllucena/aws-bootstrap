using Amazon.CDK;
using Amazon.CDK.AWS.Events;
using Amazon.CDK.AWS.Events.Targets;
using Amazon.CDK.AWS.Lambda;
using Amazon.CDK.AWS.Logs;

namespace Infrastructure.Stacks
{
    public class SchedulerStack : Stack
    {
        public SchedulerStack(Construct scope, string name, double memory, CronOptions cron) : base(scope, string.Concat("scheduler-", name))
        { 
            var lambdaId = string.Concat("scheduler-", name, "-lambda");
            var lambda = new Function(this, lambdaId, new FunctionProps()
            {
                FunctionName = name,
                MemorySize = memory,
                Runtime = Runtime.FROM_IMAGE,
                Handler = Handler.FROM_IMAGE,
                Code = Code.FromAssetImage("../tools/scheduler"),
                Timeout = Duration.Seconds(20),
                LogRetention = RetentionDays.ONE_DAY
            });

            var ruleId = string.Concat("scheduler-", name, "-rule");
            new Rule(this, ruleId, new RuleProps
            {
                RuleName = name,
                Schedule = Schedule.Cron(cron),
                Targets = new[] { new LambdaFunction(lambda) }
            });
        }
    }
}
