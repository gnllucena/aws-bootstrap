using Amazon.CDK;
using Amazon.CDK.AWS.EC2;
using Amazon.CDK.AWS.Events;
using Amazon.CDK.AWS.Events.Targets;
using Amazon.CDK.AWS.Lambda;
using Amazon.CDK.AWS.Logs;

namespace Infrastructure.Stacks
{
    public class SchedulerStack : Stack
    {
        private Function _function;
        private Rule _rule;

        public SchedulerStack(Construct scope, string name, double memory, CronOptions cron, Vpc vpc, StackProps props = null) : base(scope, $"scheduler-{name}", props)
        {
            //  pricing - lambda
            //    1 milhão de solicitações gratuitas por mês e 
            //    400.000 GB/segundos de tempo de computação por mês.
            //
            //  pricing - event bridge
            //    Eventos de serviços da AWS: Gratuito
            //    Eventos personalizados:  1,00 USD / milhões de eventos personalizados publicados
            //    Eventos de terceiros(SaaS): 1,00 USD / milhões de eventos publicados
            //    Eventos entre contas: 1,00 USD / milhões de eventos entre contas publicados

            _function = new Function(this, $"scheduler-{name}-lambda", new FunctionProps()
            {
                FunctionName = name,
                MemorySize = memory,
                Runtime = Runtime.DOTNET_CORE_3_1,
                Handler = "Lambda::Lambda.Function::Handler",
                Code = Code.FromAsset("../tools/scheduler"),
                Timeout = Duration.Seconds(20),
                LogRetention = RetentionDays.ONE_DAY,
                Vpc = vpc,
                VpcSubnets = new SubnetSelection()
                {
                    SubnetType = SubnetType.PRIVATE
                }
            });

            _rule = new Rule(this, $"scheduler-{name}-rule", new RuleProps
            {
                RuleName = name,
                Schedule = Schedule.Cron(cron),
                Targets = new[] { new LambdaFunction(_function) }
            });
        }

        public Function GetFunction()
        {
            return _function;
        }

        public Rule GetRule()
        {
            return _rule;
        }
    }
}
