using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        #region -- PRIVATE --

        #endregion
        #endregion

        #region -- CONSTRUCTOR --
        public MainWindowViewModel()
        {

            new Thread(
                () =>
                {
                    Thread.Sleep(3000);
                    Application.Current.Dispatcher.Invoke(
                        () =>
                        {
                            AutoUpdater.ReportErrors = true;
                            AutoUpdater.Synchronous = true;
                            AutoUpdater.ShowSkipButton = true;
                            AutoUpdater.ShowRemindLaterButton = true;
                            AutoUpdater.HttpUserAgent = "AutoUpdater";
                            AutoUpdater.RunUpdateAsAdmin = false;
                            AutoUpdater.DownloadPath = Directory.GetCurrentDirectory()+"/updates";
                            AutoUpdater.InstallationPath = Directory.GetCurrentDirectory();
                            AutoUpdater.ClearAppDirectory = true;
                            AutoUpdater.UpdateFormSize = new System.Drawing.Size(800, 450);
                            AutoUpdater.Start("https://raw.githubusercontent.com/leocpx/CakeFactory/main/Launcher/version.xml");
                        });
                }).Start();
        }

        #endregion
    }
}
