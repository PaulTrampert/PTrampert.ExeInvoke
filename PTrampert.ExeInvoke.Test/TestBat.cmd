@echo off

echo This output goes to std out

echo This output goes to std err>&2

set arg1 = %1

if [%arg1%]==[] set arg1 = 0

exit /B %1