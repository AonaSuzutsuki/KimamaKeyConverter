﻿//------------------------------------------------------------------------------
// <auto-generated>
//     このコードはツールによって生成されました。
//     ランタイム バージョン:4.0.30319.42000
//
//     このファイルへの変更は、以下の状況下で不正な動作の原因になったり、
//     コードが再生成されるときに損失したりします。
// </auto-generated>
//------------------------------------------------------------------------------

namespace KeyConverterGUI.LangResource.Resources {
    using System;
    
    
    /// <summary>
    ///   ローカライズされた文字列などを検索するための、厳密に型指定されたリソース クラスです。
    /// </summary>
    // このクラスは StronglyTypedResourceBuilder クラスが ResGen
    // または Visual Studio のようなツールを使用して自動生成されました。
    // メンバーを追加または削除するには、.ResX ファイルを編集して、/str オプションと共に
    // ResGen を実行し直すか、または VS プロジェクトをビルドし直します。
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   このクラスで使用されているキャッシュされた ResourceManager インスタンスを返します。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("KeyConverterGUI.LangResource.Resources.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   すべてについて、現在のスレッドの CurrentUICulture プロパティをオーバーライドします
        ///   現在のスレッドの CurrentUICulture プロパティをオーバーライドします。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   マビノギを検出 に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string UI_Detect_Mabinogi {
            get {
                return ResourceManager.GetString("UI_Detect_Mabinogi", resourceCulture);
            }
        }
        
        /// <summary>
        ///   無効化中 に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string UI_Disabled {
            get {
                return ResourceManager.GetString("UI_Disabled", resourceCulture);
            }
        }
        
        /// <summary>
        ///   有効化中 に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string UI_Enabled {
            get {
                return ResourceManager.GetString("UI_Enabled", resourceCulture);
            }
        }
        
        /// <summary>
        ///   キーボードマッピング に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string UI_Keyboard_Mapping {
            get {
                return ResourceManager.GetString("UI_Keyboard_Mapping", resourceCulture);
            }
        }
        
        /// <summary>
        ///   除外プロセスの登録 に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string UI_Process_Setting {
            get {
                return ResourceManager.GetString("UI_Process_Setting", resourceCulture);
            }
        }
    }
}
