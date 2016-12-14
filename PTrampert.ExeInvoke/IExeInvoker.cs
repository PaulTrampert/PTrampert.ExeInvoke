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
        /// Environment variables to be set for the execution of the application.
        /// </summary>
        /// <remarks>
        /// The <see cref="EnvironmentVariables"/> dictionary allows the calling code to set environment variables for the execution.
        /// Any environment variables set here will be available in the external process's environment.
        /// </remarks>
        /// <example>
        /// <code>
        /// invoker.EnvironmentVariables = new Dictionary&lt;string, string&gt;()
        /// {
        ///     { "testVar", "Something" }
        /// };
        /// invoker.Invoke("echo %testVar%");   
        /// </code>
        /// The above code would result in "Something" being printed to the console.
        /// </example>
        IDictionary<string, string> EnvironmentVariables { get; set; }

        /// <summary>
        /// Function that reads the Standard Error stream.
        /// </summary>
        /// <remarks>
        /// This function reads Standard Error. This function should not dispose of the stream, but should read the entire stream before returning.
        /// </remarks>
        Func<StreamReader, Task> StandardErrReader { get; set; }

        /// <summary>
        /// Function that reads Standard Output.
        /// </summary>
        /// <remarks>
        /// This function reads Standard Output. This function should not dispose of the stream, but should read the entire stream before returning.
        /// </remarks>
        Func<StreamReader, Task> StandardOutReader { get; set; }
        string WorkingDirectory { get; set; }

        /// <summary>
        /// Invoke <paramref name="exe"/> with <paramref name="args"/> as arguments.
        /// </summary>
        /// <param name="exe">The path to the executable to run.</param>
        /// <param name="args">An array of arguments to be passed to the executable. Arguments with spaces should be properly escaped.</param>
        /// <returns>A task that completes once the executable, <see cref="StandardErrReader"/>, and <see cref="StandardOutReader"/> have returned.</returns>
        /// <exception cref="ExternalProcessFailureException">Thrown when a non-zero exit code is set by the external process.</exception>
        Task Invoke(string exe, params string[] args);
    }
}