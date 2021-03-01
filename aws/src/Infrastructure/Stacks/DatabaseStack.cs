using Amazon.CDK;
using Amazon.CDK.AWS.EC2;
using Amazon.CDK.AWS.RDS;
using Amazon.CDK.AWS.SecretsManager;

namespace Infrastructure.Stacks
{
    public class DatabaseStack : Stack
    {
        private DatabaseInstance _databaseInstance;

        public DatabaseStack(Construct scope, string name, Vpc vpc, StackProps props = null) : base(scope, $"database-{name}", props)
        {
            //  pricing - rds
            //    750 horas de uso de instâncias db.t2.micro Single-AZ do Amazon RDS para execução de 
            //    MySQL, MariaDB, PostgreSQL, Oracle BYOL ou SQL Server(executando SQL Server Express Edition) 
            //
            //    20 GB de armazenamento de banco de dados de SSD
            //
            //    20 GB de armazenamento de backup para seus backups de banco de dados automatizados e 
            //    quaisquer snapshots de banco de dados iniciados por usuário
            //
            //  pricing - secret manager
            //    0,40 USD por segredo por mês. No caso de segredos armazenados por menos de um mês, 
            //    o preço é pro-rata (com base no número de horas). 
            //    0,05 USD por 10.000 chamadas de API.

            var secret = new Secret(this, $"database-{name}-secret", new SecretProps()
            {
                Description = $"Database {name} password",
                SecretName = $"database-{name}-secret"
            });

            var databaseSecret = new DatabaseSecret(this, $"database-{name}-databasesecret", new DatabaseSecretProps()
            {
                Username = "user",
                MasterSecret = secret,
                ExcludeCharacters = "{}[]()'\"/\\"
            });

            _databaseInstance = new DatabaseInstance(this, $"database-{name}-cluster", new DatabaseInstanceProps()
            {
                InstanceIdentifier = name + "-instance",
                DatabaseName = name,
                Credentials = Credentials.FromSecret(databaseSecret),
                Engine = DatabaseInstanceEngine.Mysql(new MySqlInstanceEngineProps()
                {
                    Version = MysqlEngineVersion.VER_8_0_21
                }),
                InstanceType = InstanceType.Of(InstanceClass.BURSTABLE3, InstanceSize.MICRO),
                Vpc = vpc,
                VpcSubnets = new SubnetSelection()
                {
                    SubnetType = SubnetType.ISOLATED
                }
            });

            _databaseInstance.AddRotationSingleUser(new RotationSingleUserOptions()
            {   
                AutomaticallyAfter = Duration.Days(7),
                ExcludeCharacters = "!@#$%^&*"
            });
        }

        public DatabaseInstance GetDatabaseInstance()
        {
            return _databaseInstance;
        }
    }
}
