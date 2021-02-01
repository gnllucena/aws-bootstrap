using Amazon.CDK;
using Amazon.CDK.AWS.CertificateManager;
using Amazon.CDK.AWS.CloudFront;
using Amazon.CDK.AWS.CloudFront.Origins;
using Amazon.CDK.AWS.Route53;
using Amazon.CDK.AWS.Route53.Targets;
using Amazon.CDK.AWS.S3;
using Amazon.CDK.AWS.S3.Deployment;
using System;

namespace Infrastructure.Stacks
{
    public class FrontendStack : Stack
    {
        public FrontendStack(Construct scope, string name, string id, string url) : base(scope, string.Concat("frontend-", name))
        {
            var bucketId = string.Concat("frontend-", name, "-bucket");
            var bucket = new Bucket(this, bucketId, new BucketProps()
            {
                BucketName = name + id,
                WebsiteIndexDocument = "index.html",
                PublicReadAccess = true,
                RemovalPolicy = RemovalPolicy.DESTROY
            });

            var deploymentId = string.Concat("frontend-", name, "-deployment");
            new BucketDeployment(this, deploymentId, new BucketDeploymentProps()
            {
                Sources = new[] { Source.Asset("../tools/frontend/") },
                DestinationBucket = bucket,
                RetainOnDelete = false,
            });

            var hostedZoneId = string.Concat("frontend-", name, "-hostedzone");
            var hostedZone = new HostedZone(this, hostedZoneId, new HostedZoneProps
            {
                ZoneName = url
            });

            var certificateId = string.Concat("frontend-", name, "-certificate");
            var certificate = new Certificate(this, certificateId, new CertificateProps
            {
                DomainName = url,
                Validation = CertificateValidation.FromDns(hostedZone)
            });

            var distributionId = string.Concat("frontend-", name, "-distribution");
            var distribution = new Distribution(this, distributionId, new DistributionProps
            {
                DefaultBehavior = new BehaviorOptions
                {
                    Origin = new S3Origin(bucket),
                    ViewerProtocolPolicy = ViewerProtocolPolicy.REDIRECT_TO_HTTPS
                },
                DomainNames = new[] { url },
                Certificate = certificate,
                DefaultRootObject = "index.html"
            });

            var aRecordId = string.Concat("frontend-", name, "arecord");
            new ARecord(this, aRecordId, new ARecordProps
            {
                Zone = hostedZone,
                RecordName = url,
                Target = RecordTarget.FromAlias(new CloudFrontTarget(distribution))
            });
        }
    }
}

