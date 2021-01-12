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

            new FrontendStack(app, "jabuticaba");

            app.Synth();
        }
    }
}
