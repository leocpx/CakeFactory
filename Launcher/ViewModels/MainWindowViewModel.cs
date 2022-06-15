using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using AutoUpdaterDotNET;


namespace Launcher.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        #region -- PROPERTIES --
        public event PropertyChangedEventHandler PropertyChanged;
        private string text1Label = "looking for sweet software updates...";
        public string Text1Label
        {
            get { return text1Label; }
            set
            {
                text1Label = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Text1Label)));
            }
        }
        private double progressValue = 0;

        public double ProgressValue
        {
            get { return progressValue; }
            set 
            { 
                progressValue = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ProgressValue)));
            }
        }


        #region -- PRIVATE --
        private Random rnd = new Random();
        #endregion
        #endregion

        #region -- CONSTRUCTOR --
        public MainWindowViewModel()
        {

            new Thread(
                () =>
                {
                    while (ProgressValue < 5999)
                    {
                        Thread.Sleep(rnd.Next(10,500));
                        var maxVal = 6000 - ProgressValue;
                        var max = (int)maxVal / 2 < 100 ? 100 : (int)maxVal / 2;
                        var newValue = rnd.Next(1, max);
                        ProgressValue += 100; 
                    }
                }).Start();

            new Thread(
                () =>
                {
                    Thread.Sleep(3000);
                    {
                        AutoUpdater.ReportErrors = false;
                        AutoUpdater.Synchronous = true;
                        //AutoUpdater.ShowSkipButton = true;
                        AutoUpdater.ShowRemindLaterButton = true;
                        AutoUpdater.HttpUserAgent = "AutoUpdater";
                        AutoUpdater.RunUpdateAsAdmin = false;
                        AutoUpdater.DownloadPath = Directory.GetCurrentDirectory();
                        AutoUpdater.InstallationPath = Directory.GetCurrentDirectory();
                        AutoUpdater.ClearAppDirectory = false;
                        AutoUpdater.UpdateFormSize = new System.Drawing.Size(800, 450);
                        AutoUpdater.ApplicationExitEvent += AutoUpdater_ApplicationExitEvent;
                        AutoUpdater.CheckForUpdateEvent += AutoUpdater_CheckForUpdateEvent;
                        AutoUpdater.Start("https://raw.githubusercontent.com/leocpx/CakeFactory/main/Launcher/version.xml");
                    };
                }).Start();
        }

        private void AutoUpdater_CheckForUpdateEvent(UpdateInfoEventArgs args)
        {
            if (args.IsUpdateAvailable)
            {
                Text1Label = "new update found...";

                new Thread(
                    () =>
                    {
                        Thread.Sleep(3000);
                        ProgressValue = 6000;

                        AutoUpdater.CheckForUpdateEvent -= AutoUpdater_CheckForUpdateEvent;
                        AutoUpdater.Start("https://raw.githubusercontent.com/leocpx/CakeFactory/main/Launcher/version.xml");
                    }).Start();
            }
            else
            {
                Text1Label = "up to date, launching application...";

                new Thread(() =>
                {
                    Thread.Sleep(3000);
                    ProgressValue = 6000;

                    Application.Current.Dispatcher.Invoke(App.Current.Shutdown);
                    Application.Current.Dispatcher.Invoke(() => Process.Start("main.exe"));
                }).Start();

            }
        }

        private void AutoUpdater_ApplicationExitEvent()
        {
            Text1Label = "restarting to apply updates...";

            new Thread(() =>
            {
                Thread.Sleep(3000);
                Application.Current.Dispatcher.Invoke(App.Current.Shutdown);
            }).Start();
        }

        #endregion
    }
}
