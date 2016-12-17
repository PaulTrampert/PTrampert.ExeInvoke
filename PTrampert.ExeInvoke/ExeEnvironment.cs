using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace PTrampert.ExeInvoke
{
    /// <summary>
    /// Environment settings for an external process.
    /// </summary>
    /// <remarks>
    /// This class provides the environment settings for the external process. Members of this class are used by <see cref="ExeInvoker"/> to setup the environment for an external process.
    /// </remarks>
    public class ExeEnvironment
    {
        /// <summary>
        /// Used to process standard out.
        /// </summary>
        /// <value>If set, will be passed the standard out stream for the external process.</value>
        public Func<StreamReader, Task> StandardOutReader { get; set; }

        /// <summary>
        /// Used to process standard error.
        /// </summary>
        /// <value>If set, will be passed the standard error stream for the external process.</value>
        public Func<StreamReader, Task> StandardErrReader { get; set; }

        /// <summary>
        /// Environment variables to be passed to the external process.
        /// </summary>
        /// <value>If set, any environment variables here will be passed to the external process's environment.</value>
        public IDictionary<string, string> EnvironmentVariables { get; set; }

        /// <summary>
        /// The working directory for the external process.
        /// </summary>
        /// <value>If set, will set the working directory for the external process.</value>
        public string WorkingDirectory { get; set; }
    }
}
