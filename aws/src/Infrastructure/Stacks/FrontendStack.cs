using Amazon.CDK;
using Amazon.CDK.AWS.CertificateManager;
using Amazon.CDK.AWS.Route53;
using Amazon.CDK.AWS.S3;
using Amazon.CDK.AWS.S3.Deployment;

namespace Infrastructure.Stacks
{
    public class FrontendStack : Stack
    {
        public FrontendStack(Construct scope, string name, string url) : base(scope, string.Concat("frontend-", name))
        {
            var bucketId = string.Concat("frontend-", name, "-bucket");
            var bucket = new Bucket(this, bucketId, new BucketProps()
            {
                BucketName = name,
                WebsiteIndexDocument = "index.html",
                PublicReadAccess = true
            });

            var deploymentId = string.Concat("frontend-", name, "-deployment");
            new BucketDeployment(this, deploymentId, new BucketDeploymentProps()
            {
                Sources = new[] { Source.Asset("../tools/frontend/") },
                DestinationBucket = bucket,
                DestinationKeyPrefix = "web/static"
            });

            HostedZone myHostedZone = new HostedZone(this, "HostedZone", new HostedZoneProps
            {
                ZoneName = "example.com"
            });

            new Certificate(this, "Certificate", new CertificateProps
            {
                DomainName = "hello.example.com",
                Validation = CertificateValidation.FromDns(myHostedZone)
            });
        }
    }
}
