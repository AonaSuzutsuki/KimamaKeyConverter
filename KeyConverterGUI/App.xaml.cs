using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using CommonCoreLib;
using KeyConverterGUI.Views;

namespace KeyConverterGUI
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        private IDisposable mainWindow;
        private void MyApp_Startup(object sender, StartupEventArgs e)
        {
            TaskScheduler.UnobservedTaskException += TaskSchedulerOnUnobservedTaskException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;

            var main = new MainWindow();
            mainWindow = main;
            main.Show();
        }

        private void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception exception)
            {
                ShowAndWriteException(exception);
                mainWindow.Dispose();
            }
        }

        private void TaskSchedulerOnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            ShowAndWriteException(e.Exception);
            mainWindow.Dispose();
        }

        private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            ShowAndWriteException(e.Exception);
            mainWindow.Dispose();

            e.Handled = true;
            Shutdown();
        }

        public static void ShowAndWriteException(Exception exception)
        {
            var mes =
                $"予期せぬエラーが発生しました。\r\nお手数ですが、開発者に例外内容を報告してください。\r\n\r\n---\r\n\r\n{exception.Message}\r\n\r\n{exception.StackTrace}";
            MessageBox.Show(mes, "予期せぬエラー", MessageBoxButton.OK, MessageBoxImage.Error);

            var dt = DateTime.Now;
            OutToFile(AppInfo.GetAppPath() + @"\error-" + dt.ToString("yyyy-MM-dd- HH-mm-ss") + ".log", mes);
        }

        private static void OutToFile(string filename, string text)
        {
            using var fs = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read);
            using var sw = new StreamWriter(fs, System.Text.Encoding.UTF8);
            sw.Write(text);
        }

        private void App_OnExit(object sender, ExitEventArgs e)
        {
            mainWindow?.Dispose();
        }
    }
}
