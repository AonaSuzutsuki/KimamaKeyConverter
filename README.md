# Mabinogi Ctrl & Alt Reverser
マビノギ向けのキー転換器です。  
現段階では汎用性を意識して作ってないのでCtrlとAltだけを転換します。  

# 使い方
## 基本的な使い方
![main_window.png](docs/images/main_window.png)  

画面中央のデカデカとしたボタンを押すことで有効と無効の切り替えができます。  
**Now Disabledと表記されている場合は無効化**されています。  
**Now Enabledと表記されている場合は有効化**されています。  
有効化中のみキーの入れ替えを行いますので入れ替えを行う場合は有効化してください。  

## マビノギ検出
**Detect Mabinogi**のチェック（バツマーク）を入れることでマビノギが起動している場合に有効化すると自動でマビノギを検出し、マビノギのみに有効化します。  
将来的には有効化中でも自動で検知できるようにする予定ですが、まだ対応できていません。  

## キーマッピング
![main_window.png](docs/images/key_mapping1.png)  
![main_window.png](docs/images/key_mapping2.png)  
キーマッピングは**Keyboard Mapping**ボタンより起動できます。  
ここでは変換したいキーをクリックし、2枚目の設定画面で変換するキーを押すことで設定できます。  
例えば**左のCtrlキーを左のAltに変換したい**場合は**左のCtrlをクリックし、キーボード側で左のAltキー**を押します。  
なお、マッピングを削除したい場合は何も入力しないままOKを押すと削除されます。  

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