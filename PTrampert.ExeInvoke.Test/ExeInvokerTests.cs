using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PTrampert.ExeInvoke.Test
{
    public class ExeInvokerTests
    {
        private ExeInvoker invoker = new ExeInvoker();

        private string BatFile = Path.Combine(Path.GetDirectoryName(typeof(ExeInvokerTests).GetTypeInfo().Assembly.CodeBase.Replace("file:///", "").Replace('/', Path.DirectorySeparatorChar)), "TestBat.cmd");

        [Fact]
        public async Task ItMakesStandardOutAvailable()
        {
            var env = new ExeEnvironment
            {
                StandardOutReader = async stdOut => Assert.StartsWith("This output goes to std out\r\n", await stdOut.ReadToEndAsync())
            };
            await invoker.Invoke(BatFile, env: env);
        }

        [Fact]
        public async Task ItMakesStandardErrAvailable()
        {
            var env = new ExeEnvironment { StandardErrReader = async stdErr => Assert.Equal("This output goes to std err\r\n", await stdErr.ReadToEndAsync()) };
            await invoker.Invoke(BatFile, env: env);
        }

        [Fact]
        public async Task ItThrowsWhenExitCodeIsNonZero()
        {
            var exception = await Assert.ThrowsAsync<ExternalProcessFailureException>(async () => await invoker.Invoke(BatFile, new[] { "55" }));
            Assert.Equal(55, exception.ExitCode);
            Assert.Equal(BatFile, exception.Invocation.FileName);
        }

        [Fact]
        public async Task ItSetsEnvironmentVariables()
        {
            var env = new ExeEnvironment
            {
                StandardOutReader = async stdOut => Assert.StartsWith("This output goes to std out\r\nSomething\r\n", await stdOut.ReadToEndAsync()),
                EnvironmentVariables = new Dictionary<string, string>()
                {
                    { "testVar", "Something" }
                }
            };
            await invoker.Invoke(BatFile, env: env);
        }

        [Fact]
        public async Task ItCanSetTheWorkingDirectory()
        {
            var env = new ExeEnvironment
            {
                StandardOutReader = async stdOut => Assert.Contains("Working Directory: C:\\Users", await stdOut.ReadToEndAsync()),
                WorkingDirectory = "C:\\Users"
            };
            await invoker.Invoke(BatFile);
        }
    }
}
