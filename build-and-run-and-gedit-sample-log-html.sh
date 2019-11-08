#!/usr/bin/env bash
msbuild /p:Configuration=Debug && mono ./ConsoleDebug/bin/Debug/ConsoleDebug.exe
gedit sample-log.html