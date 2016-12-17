# PTrampert.ExeInvoke
Class and interface to wrap what I consider to be a slightly clunky native process interface in something a bit safer to use. This library is not intended for use for monitoring long running processes, but rather for running command line applications that are sure to terminate in a reasonable amount of time and process their output safely.

### Basic Usage
```C#
var invoker = new ExeInvoker();
await invoker.Invoke(someExecuteable);
```
