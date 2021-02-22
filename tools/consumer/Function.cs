using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]
namespace Lambda
{
    public class Function
    {
        public async Task<string> Handler(SQSEvent evnt, ILambdaContext context)
        {
            context.Logger.LogLine("LOGGING FROM APPLICATION: " + JsonSerializer.Serialize(evnt));
            
            var response = string.Join(" ---- ", evnt.Records.Select(x => x.Body));

            return response;
        }
    }
}
