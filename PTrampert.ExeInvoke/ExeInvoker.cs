using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace PTrampert.ExeInvoke
{
    /// <summary>
    /// <see cref="IExeInvoker"/>
    /// </summary>
    public class ExeInvoker : IExeInvoker
    {
        /// <summary>
        /// Inherited from <see cref="IExeInvoker"/>
        /// </summary>
        public async Task Invoke(string exe, string[] args = null, ExeEnvironment env = null)
        {
            var invocation = new ProcessStartInfo(exe, string.Join(" ", args ?? new string[0]));
            invocation.RedirectStandardError = true;
            invocation.RedirectStandardOutput = true;
            foreach(var variable in env?.EnvironmentVariables ?? new Dictionary<string, string>())
            {
                invocation.Environment.Add(variable.Key, variable.Value);
            }
            if (!string.IsNullOrWhiteSpace(env?.WorkingDirectory))
            {
                invocation.WorkingDirectory = env?.WorkingDirectory;
            }
            using (var process = Process.Start(invocation))
            {
                var readerTasks = new List<Task>();
                var stdOut = process.StandardOutput;
                var stdErr = process.StandardError;
                if (env?.StandardOutReader != null)
                {
                    readerTasks.Add(env.StandardOutReader(stdOut));
                }
                if (env?.StandardErrReader != null)
                {
                    readerTasks.Add(env.StandardErrReader(stdErr));
                }
                await Task.Run(() => process.WaitForExit());
                await Task.WhenAll(readerTasks);
                if (process.ExitCode != 0)
                {
                    throw new ExternalProcessFailureException(invocation, process.ExitCode);
                }
            }
        }
    }
}
