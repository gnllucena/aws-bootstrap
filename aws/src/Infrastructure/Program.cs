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

            new FrontendStack(app, "stocks", "72b302bf297a228a75730123efef7c41", "zro17.com");

            app.Synth();
        }
    }
}
