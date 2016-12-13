﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace PTrampert.ExeInvoke
{
    public class ExeInvoker
    {
        public Func<StreamReader, Task> StandardOutReader { get; set; }

        public Func<StreamReader, Task> StandardErrReader { get; set; }

        public async Task Invoke(string exe, params string[] args)
        {
            var invocation = new ProcessStartInfo(exe, string.Join(" ", args));
            invocation.RedirectStandardError = true;
            invocation.RedirectStandardOutput = true;
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