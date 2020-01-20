# Mabinogi Key Converter
This is key converter for Mabinogi.  
Depending on the type of keyboard, keys such as Ctrl and Alt are far away, and your finger hang.  
At that time, this software can assign system keys to different keys.  
For example, you can convert the Ctrl key to the Alt key and the Alt key to the Ctrl key.  
This will make your mabinogi life without any discomfort.  

Although it is intended for mabinogi, it can be used as key replacement software for the entire Windows if mabinogi detection is not used.  
Since it does not use registry, restarting is unnecessary, so you can use it immediately after startup.  

# How to use
Moved to [MabinogiKeyConverter - Wiki](https://github.com/AonaSuzutsuki/MabinogiKeyConverter/wiki).  

# Notice
Because keys are operated using global hooks, there is a possibility that key operations cannot be performed at all due to unexpected crashes.  
I made it so that there is no such fatal bug, but if it happens, please let me know.  
In addition, this application capture the input key and only presses the key with software, so it will not get caught at this stage by BlackCipher, but it may be prohibited in the future.  
Please forgive because I do not take any responsibility for damage caused by such things.  

# Requirements
1. .Net Framework 4.7.1

# Check environment
| OS | 動作状況 | 備考 |
|:---|:---|:---|
|Windows 7 x86 | OK | Checked on English version |
|Windows 7 x64 | - | - |
|Windows 8.x | - | - |
|Windows 10 x86 | - | - |
|Windows 10 x64 1903 | OK | - |

# Copyright
Copyright © Aona Suzutsuki 2018-2019  

# Special thanks
1. [C#でSendInputを使う](https://gist.github.com/romichi/4971512)  
2. [Low-Level Keyboard Hook in C#](https://blogs.msdn.microsoft.com/toub/2006/05/03/low-level-keyboard-hook-in-c/)  
3. [Windows：SendInputで指定した"dwExtraInfo"をグローバルフック内で取得する](http://d.hatena.ne.jp/ken_2501jp/20130406/1365235955)  
4. [An ObservableDictionary<TKey, TValue>](https://gist.github.com/kzu/cfe3cb6e4fe3efea6d24)  

ReactiveProperty:               Copyright (c) 2018 neuecc, xin9le, okazuki  
Prism.Core:                     Copyright (c) .NET Foundation  
Microsoft.Xaml.Behaviors.Wpf:   Copyright (c) 2015 Microsoft  
NUnit:                          Copyright (c) 2019 Charlie Poole, Rob Prouse  
Newtonsoft.Json:                Copyright (c) 2007 James Newton-King  


This software includes the work that is distributed in the Apache License 2.0
