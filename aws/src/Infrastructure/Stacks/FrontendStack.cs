using Amazon.CDK;
using Amazon.CDK.AWS.CertificateManager;
using Amazon.CDK.AWS.CloudFront;
using Amazon.CDK.AWS.CloudFront.Origins;
using Amazon.CDK.AWS.Route53;
using Amazon.CDK.AWS.Route53.Targets;
using Amazon.CDK.AWS.S3;
using Amazon.CDK.AWS.S3.Deployment;

namespace Infrastructure.Stacks
{
    public class FrontendStack : Stack
    {
        private Bucket _bucket;
        private BucketDeployment _bucketDeployment;
        private HostedZone _hostedZone;
        private Certificate _certificate;
        private Distribution _distribution;
        private ARecord _aRecord;

        public FrontendStack(Construct scope, string name, string url, StackProps props = null) : base(scope, $"frontend-{name}", props)
        {
            _bucket = new Bucket(this, $"frontend-{name}-bucket", new BucketProps()
            {
                BucketName = name + "72b302bf297a228a75730123efef7c41",
                WebsiteIndexDocument = "index.html",
                PublicReadAccess = true,
                RemovalPolicy = RemovalPolicy.DESTROY
            });

            _bucketDeployment = new BucketDeployment(this, $"frontend-{name}-deployment", new BucketDeploymentProps()
            {
                Sources = new[] { Source.Asset("../tools/frontend/") },
                DestinationBucket = _bucket,
                RetainOnDelete = false
            });

            _hostedZone = new HostedZone(this, $"frontend-{name}-hostedzone", new HostedZoneProps
            {
                ZoneName = url
            });

            _certificate = new Certificate(this, $"frontend-{name}-certificate", new CertificateProps
            {
                DomainName = url,
                Validation = CertificateValidation.FromDns(_hostedZone)
            });

            _distribution = new Distribution(this, $"frontend-{name}-distribution", new DistributionProps
            {
                DefaultBehavior = new BehaviorOptions
                {
                    Origin = new S3Origin(_bucket),
                    ViewerProtocolPolicy = ViewerProtocolPolicy.REDIRECT_TO_HTTPS
                },
                DomainNames = new[] { url },
                Certificate = _certificate,
                DefaultRootObject = "index.html"
            });

            _aRecord = new ARecord(this, $"frontend-{name}-arecord", new ARecordProps
            {
                Zone = _hostedZone,
                RecordName = url,
                Target = RecordTarget.FromAlias(new CloudFrontTarget(_distribution))
            });
        }

        public BucketDeployment GetBucketDeployment()
        {
            return _bucketDeployment;
        }

        public HostedZone GetHostedZone()
        {
            return _hostedZone;
        }

        public Certificate GetCertificate()
        {
            return _certificate;
        }

        public Distribution GetDistribution()
        {
            return _distribution;
        }

        public ARecord GetARecord()
        {
            return _aRecord;
        }
    }
}

