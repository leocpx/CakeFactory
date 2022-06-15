using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using AutoUpdateViaGitHubRelease;


namespace Launcher.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        #region -- PROPERTIES --
        public event PropertyChangedEventHandler PropertyChanged;

        #region -- PRIVATE --
        private Update update { get; set; }
        private string GitHubUser { get; set; } = "cpx4000";
        private string GitHubRepo { get; set; } = "https://github.com/leocpx/CakeFactory.git";
        private string tempDir { get; set; } 
        private Assembly assembly { get; set; }
        private Version version { get; set; }
        #endregion
        #endregion

        #region -- CONSTRUCTOR --
        public MainWindowViewModel()
        {
            tempDir = $"{Directory.GetCurrentDirectory()}/temp";
            Directory.CreateDirectory(tempDir);


            update = new Update();
            update.PropertyChanged += Update_PropertyChanged;
            assembly = Assembly.GetExecutingAssembly();
            version = assembly.GetName().Version;
            update.CheckDownloadNewVersionAsync(GitHubUser, GitHubRepo, version, tempDir);
        }

        private void Update_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (update.Available)
            {
                var destinationDir = Path.GetDirectoryName(assembly.Location);
                update.StartInstall(destinationDir);
                Application.Current.Shutdown();
            }
        }
        #endregion
    }
}
