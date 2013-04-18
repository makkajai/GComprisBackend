call setEnv.bat
msbuild build.proj @%RESPONSE_FILE% /t:RunTests /l:FileLogger,Microsoft.Build.Engine;logfile=TestLog.log
