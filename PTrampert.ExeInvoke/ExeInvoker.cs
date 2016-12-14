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
        public Func<StreamReader, Task> StandardOutReader { get; set; }

        /// <summary>
        /// Inherited from <see cref="IExeInvoker"/>
        /// </summary>
        public Func<StreamReader, Task> StandardErrReader { get; set; }

        /// <summary>
        /// Inherited from <see cref="IExeInvoker"/>
        /// </summary>
        public IDictionary<string, string> EnvironmentVariables { get; set; }

        /// <summary>
        /// Inherited from <see cref="IExeInvoker"/>
        /// </summary>
        public string WorkingDirectory { get; set; }

        /// <summary>
        /// Inherited from <see cref="IExeInvoker"/>
        /// </summary>
        public async Task Invoke(string exe, params string[] args)
        {
            var invocation = new ProcessStartInfo(exe, string.Join(" ", args));
            invocation.RedirectStandardError = true;
            invocation.RedirectStandardOutput = true;
            foreach(var variable in EnvironmentVariables ?? new Dictionary<string, string>())
            {
                invocation.Environment.Add(variable.Key, variable.Value);
            }
            if (!string.IsNullOrWhiteSpace(WorkingDirectory))
            {
                invocation.WorkingDirectory = WorkingDirectory;
            }
            using (var process = Process.Start(invocation))
            {
                var readerTasks = new List<Task>();
                var stdOut = process.StandardOutput;
                var stdErr = process.StandardError;
                if (StandardOutReader != null)
                {
                    readerTasks.Add(StandardOutReader(stdOut));
                }
                if (StandardErrReader != null)
                {
                    readerTasks.Add(StandardErrReader(stdErr));
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
