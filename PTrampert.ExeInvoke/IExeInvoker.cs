using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace PTrampert.ExeInvoke
{
    public interface IExeInvoker
    {
        IDictionary<string, string> EnvironmentVariables { get; set; }
        Func<StreamReader, Task> StandardErrReader { get; set; }
        Func<StreamReader, Task> StandardOutReader { get; set; }
        string WorkingDirectory { get; set; }

        Task Invoke(string exe, params string[] args);
    }
}