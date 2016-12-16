using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace PTrampert.ExeInvoke
{
    /// <summary>
    /// Interface for an ExeInvoker. Allows the execution of an application, as well as the setup of the execution environment.
    /// </summary>
    public interface IExeInvoker
    {
        /// <summary>
        /// Invoke <paramref name="exe"/> with <paramref name="args"/> as arguments.
        /// </summary>
        /// <param name="exe">The path to the executable to run.</param>
        /// <param name="args">An array of arguments to be passed to the executable. Arguments with spaces should be properly escaped.</param>
        /// <param name="env">The environment to start the external process with.</param>
        /// <returns>A task that completes once the executable, <see cref="ExeEnvironment.StandardErrReader"/>, and <see cref="ExeEnvironment.StandardOutReader"/> have returned.</returns>
        /// <exception cref="ExternalProcessFailureException">Thrown when a non-zero exit code is set by the external process.</exception>
        Task Invoke(string exe, string[] args = null, ExeEnvironment env = null);
    }
}