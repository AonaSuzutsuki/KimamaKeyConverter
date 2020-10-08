# Kimama Key Converter
This application converts the entire Windows system key to any key.  
It uses the Win32API key conversion technology, so you don't need to reboot your system.  
It also does not rewrite the registry, so you won't have any registry-related problems.  

# How to use
Moved to [KimamaKeyConverter - Wiki](https://github.com/AonaSuzutsuki/KimamaKeyConverter/wiki).  

# Notice
Because keys are operated using global hooks, there is a possibility that key operations cannot be performed at all due to unexpected crashes.  
I made it so that there is no such fatal bug, but if it happens, please let me know.  
In addition, this application capture the input key and only presses the key with software, so it will not get caught at this stage by anticheat tool, but it may be prohibited in the future.  
Please forgive because I do not take any responsibility for damage caused by such things.  

# Requirements
1. .Net Framework 4.7.1

# Check environment
| OS | Status | Remarks |
|:---|:---|:---|
|Windows 7 x86 | OK | Checked on English version |
|Windows 7 x64 | - | - |
|Windows 8.x | - | - |
|Windows 10 x86 | - | - |
|Windows 10 x64 1903 | OK | - |

# Copyright
Copyright © Aona Suzutsuki 2018-2020  

# Special thanks
1. [C#でSendInputを使う](https://gist.github.com/romichi/4971512)  
2. [Low-Level Keyboard Hook in C#](https://blogs.msdn.microsoft.com/toub/2006/05/03/low-level-keyboard-hook-in-c/)  
3. [Windows：SendInputで指定した"dwExtraInfo"をグローバルフック内で取得する](http://d.hatena.ne.jp/ken_2501jp/20130406/1365235955)  
4. [An ObservableDictionary<TKey, TValue>](https://gist.github.com/kzu/cfe3cb6e4fe3efea6d24)  

ReactiveProperty:               Copyright (c) 2018 neuecc, xin9le, okazuki  
Prism.Core:                     Copyright (c) .NET Foundation  
Microsoft.Xaml.Behaviors.Wpf:   Copyright (c) 2015 Microsoft  
Newtonsoft.Json:                Copyright (c) 2007 James Newton-King  


This software includes the work that is distributed in the Apache License 2.0
