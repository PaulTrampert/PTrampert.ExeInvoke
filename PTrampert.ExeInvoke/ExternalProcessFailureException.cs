using System;
using System.Diagnostics;

namespace PTrampert.ExeInvoke
{
    public class ExternalProcessFailureException : Exception
    {
        public ProcessStartInfo Invocation { get; }

        public int ExitCode { get; }

        public ExternalProcessFailureException(ProcessStartInfo invocation, int exitCode) : base($"The external process, {invocation.FileName}, failed with exit code {exitCode}.")
        {
            Invocation = invocation;
            ExitCode = exitCode;
        }
    }
}