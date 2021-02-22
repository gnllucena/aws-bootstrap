using Amazon.CDK;
using Amazon.CDK.AWS.Lambda;
using Amazon.CDK.AWS.Lambda.EventSources;
using Amazon.CDK.AWS.Logs;
using Amazon.CDK.AWS.SQS;

namespace Infrastructure.Stacks
{
    public class ConsumerStack : Stack
    {
        public ConsumerStack(Construct scope, string name, double memory) : base(scope, string.Concat("consumer-", name))
        {
            var lambdaId = string.Concat("consumer-", name, "-lambda");
            var lambda = new Function(this, lambdaId, new FunctionProps()
            {
                FunctionName = name,
                MemorySize = memory,
                Runtime = Runtime.FROM_IMAGE,
                Handler = Handler.FROM_IMAGE,
                Code = Code.FromAssetImage("../tools/consumer"),
                Timeout = Duration.Seconds(20),
                LogRetention = RetentionDays.ONE_DAY
            });

            var deadletterId = string.Concat("consumer-", name, "-deadletter");
            var deadletter = new Queue(this, deadletterId, new QueueProps()
            {
                QueueName = name + "-deadletter",
                VisibilityTimeout = Duration.Seconds(30),
                RetentionPeriod = Duration.Days(10),
            });

            var queueId = string.Concat("consumer-", name, "-queue");
            var queue = new Queue(this, queueId, new QueueProps()
            {
                QueueName = name,
                VisibilityTimeout = Duration.Seconds(30),
                RetentionPeriod = Duration.Days(10),
                DeadLetterQueue = new DeadLetterQueue()
                {
                    MaxReceiveCount = 3,
                    Queue = deadletter
                }
            });

            lambda.AddEventSource(new SqsEventSource(queue));
        }
    }
}
