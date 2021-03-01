using Amazon.CDK;
using Amazon.CDK.AWS.EC2;
using Amazon.CDK.AWS.Lambda;
using Amazon.CDK.AWS.Lambda.EventSources;
using Amazon.CDK.AWS.Logs;
using Amazon.CDK.AWS.SQS;

namespace Infrastructure.Stacks
{
    public class ConsumerStack : Stack
    {
        private Function _function;
        private Queue _queue;
        private Queue _deadletter;

        public ConsumerStack(Construct scope, string name, double memory, Vpc vpc, StackProps props = null) : base(scope, $"consumer-{name}", props)
        {
            //  pricing - lambda
            //    1 milhão de solicitações gratuitas por mês e 
            //    400.000 GB/segundos de tempo de computação por mês.
            //
            //  pricing - sqs
            //    Primeiro milhão de solicitações/mês - padrao: Gratuito - fifo: Gratuito
            //    De 1 milhão a 100 bilhões de solicitações mês - padrão: 0,40 USD - fifo: 0,50 USD
            //    De 100 milhões a 200 bilhões de solicitações/mês - padrão: 0,30 USD - fifo: 0,40 USD
            //    Mais de 200 bilhões de solicitações/mês - padrão: 0,24 USD - fifo: 0,35 USD

            _function = new Function(this, $"consumer-{name}-lambda", new FunctionProps()
            {
                FunctionName = name,
                MemorySize = memory,
                Runtime = Runtime.DOTNET_CORE_3_1,
                Handler = "Lambda::Lambda.Function::Handler",
                Code = Code.FromAsset("../tools/consumer"),
                Timeout = Duration.Seconds(20),
                LogRetention = RetentionDays.ONE_DAY,
                Vpc = vpc,
                VpcSubnets = new SubnetSelection()
                {
                    SubnetType = SubnetType.PRIVATE
                }
            });

            _deadletter = new Queue(this, $"consumer-{name}-deadletter", new QueueProps()
            {
                QueueName = name + "-deadletter",
                VisibilityTimeout = Duration.Seconds(30),
                RetentionPeriod = Duration.Days(10)
            });
;
            _queue = new Queue(this, $"consumer-{name}-queue", new QueueProps()
            {
                QueueName = name,
                VisibilityTimeout = Duration.Seconds(30),
                RetentionPeriod = Duration.Days(10),
                DeadLetterQueue = new DeadLetterQueue()
                {
                    MaxReceiveCount = 3,
                    Queue = _deadletter
                }
            });

            _function.AddEventSource(new SqsEventSource(_queue));
        }

        public Function GetFunction()
        {
            return _function;
        }

        public Queue GeQueue()
        {
            return _queue;
        }

        public Queue GetDeadletter()
        {
            return _deadletter;
        }
    }
}
