# Mabinogi Ctrl & Alt Reverser
マビノギ向けのキー転換器です。  
現段階では汎用性を意識して作ってないのでCtrlとAltだけを転換します。  

# 使い方
画面中央のデカデカとしたボタンを押すことで有効と無効の切り替えができます。  
Now Disabledと表記されている場合は無効化されています。  
Now Enabledと表記されている場合は有効化されています。  
有効化中のみキーの入れ替えを行いますので入れ替えを行う場合は有効化してください。  

また、マビノギ(Client.exe)を起動中に有効化するとマビノギにのみキー転換が有効化されます。  
起動前に有効化した場合は全てのアプリケーションに対して有効化されるのでマビノギのみに絞る場合は起動後に有効化してください。  

# 注意
グローバルフックを用いてキーを操作するので予期せぬクラッシュなどによりキー操作が一切行えなくなる可能性があります。  
一応そういった致命的な不具合は無いように作っていますが、もしかすると起きる可能性が潰せていないのでもし発生しましたらご一報ください。  
なお、**そういった事による損害に関しては一切責任を負いません**のでご容赦ください。  

# 必須項目
1. .Net Framework 4.7.1

# 動作確認
| OS | 動作状況 | 備考 |
|:---|:---|:---|
|Windows 7 | 不明 | - |
|Windows 8.x | 不明 | - |
|Windows 10 x86 | 不明 | - |
|Windows 10 x64 1803 | 正常動作 | - |

# Copyright
Copyright © Aona Suzutsuki 2018  

# Special thanks
1. [C#でSendInputを使う](https://gist.github.com/romichi/4971512)  
2. [Low-Level Keyboard Hook in C#](https://blogs.msdn.microsoft.com/toub/2006/05/03/low-level-keyboard-hook-in-c/)  
3. [Windows：SendInputで指定した"dwExtraInfo"をグローバルフック内で取得する](http://d.hatena.ne.jp/ken_2501jp/20130406/1365235955)  