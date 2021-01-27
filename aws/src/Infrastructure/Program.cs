using Amazon.CDK;
using Infrastructure.Stacks;
using System;

namespace Src
{
    sealed class Program
    {
        public static void Main(string[] args)
        {
            var app = new App();

            var hash = "d12302080c640cf8b80077f0dcdeb7da"; //md5: zro17

            var frontend = "stocks-" + hash;
            new FrontendStack(app, frontend, "www.zro17.com");

            app.Synth();
        }
    }
}
