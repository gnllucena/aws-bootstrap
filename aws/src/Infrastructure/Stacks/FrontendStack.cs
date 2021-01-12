using Amazon.CDK;
using Amazon.CDK.AWS.S3;
using Amazon.CDK.AWS.S3.Deployment;
using System;

namespace Infrastructure.Stacks
{
    public class FrontendStack : Stack
    {
        public FrontendStack(Construct scope, string name) : base(scope, string.Concat(name, "-", Guid.NewGuid()))
        {
            var bucketId = string.Concat(name, "-bucket-", Guid.NewGuid());
            var bucket = new Bucket(this, bucketId, new BucketProps()
            {
                BucketName = name,
                WebsiteIndexDocument = "index.html",
                PublicReadAccess = true
            });

            var deploymentId = string.Concat(name, "-deployment-", Guid.NewGuid());
            new BucketDeployment(this, deploymentId, new BucketDeploymentProps()
            {
                Sources = new[] { Source.Asset("../tools/frontend-stock/") },
                DestinationBucket = bucket,
                DestinationKeyPrefix = "web/static"
            });
        }
    }
}
