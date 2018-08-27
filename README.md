# Mabinogi Ctrl & Alt Reverser
マビノギ向けのキー転換器です。  
現段階では汎用性を意識して作ってないのでCtrlとAltだけを転換します。  

# 使い方
[MabinogiKeyConverter - Wiki](https://github.com/AonaSuzutsuki/MabinogiKeyConverter/wiki)に移動しました。  

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
4. [An ObservableDictionary<TKey, TValue>](https://gist.github.com/kzu/cfe3cb6e4fe3efea6d24)  
