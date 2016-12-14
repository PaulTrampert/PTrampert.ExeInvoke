using System;
using System.Diagnostics;

namespace PTrampert.ExeInvoke
{
    /// <summary>
    /// Thrown when an external process sets a non-zero exit code.
    /// </summary>
    public class ExternalProcessFailureException : Exception
    {
        /// <summary>
        /// The <see cref="ProcessStartInfo"/> that started the external process.
        /// </summary>
        public ProcessStartInfo Invocation { get; }

        /// <summary>
        /// The exit code of the external process.
        /// </summary>
        public int ExitCode { get; }

        internal ExternalProcessFailureException(ProcessStartInfo invocation, int exitCode) : base($"The external process, {invocation.FileName}, failed with exit code {exitCode}.")
        {
            Invocation = invocation;
            ExitCode = exitCode;
        }
    }
}