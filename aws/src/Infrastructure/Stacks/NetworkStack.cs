using Amazon.CDK;
using Amazon.CDK.AWS.EC2;

namespace Infrastructure.Stacks
{
    public class NetworkStack : Stack
    {
        private Vpc _vpc;
        private NatInstanceProvider _natInstanceProvider;

        public NetworkStack(Construct scope, string name, StackProps props = null) : base(scope, $"network-{name}", props)
        {
            _natInstanceProvider = NatProvider.Instance(new NatInstanceProps
            {
                InstanceType = new InstanceType("t2.micro")
            });

            _vpc = new Vpc(this, $"network-{name}-vpc", new VpcProps()
            {
                Cidr = "10.0.0.0/20",
                MaxAzs = 2,
                NatGateways = 1,
                NatGatewayProvider = _natInstanceProvider,
                SubnetConfiguration = new ISubnetConfiguration[]
                {
                    new SubnetConfiguration
                    {
                        CidrMask = 23,
                        Name = "PUBLIC",
                        SubnetType = SubnetType.PUBLIC
                    },
                    new SubnetConfiguration
                    {
                        CidrMask = 23,
                        Name = "PRIVATE",
                        SubnetType = SubnetType.PRIVATE
                    },
                    new SubnetConfiguration
                    {
                        CidrMask = 23,
                        Name = "ISOLATED",
                        SubnetType = SubnetType.ISOLATED
                    }
                }
            });
        }

        public Vpc GetVpc()
        {
            return _vpc;
        }

        public NatInstanceProvider GetNatInstanceProvider()
        {
            return _natInstanceProvider;
        }
    }
}
