using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using System.Text.Json;
using System.Threading.Tasks;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]
namespace Lambda
{
    public class Function
    {
        public async Task Handler(SQSEvent evnt, ILambdaContext context)
        {
            context.Logger.LogLine("LOGGING FROM APPLICATION: " + JsonSerializer.Serialize(evnt));
        }
    }
}
