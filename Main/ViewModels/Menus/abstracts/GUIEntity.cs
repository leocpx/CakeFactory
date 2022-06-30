using DBManager.Tables;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityCake.Events;

namespace Main.ViewModels.Menus.abstracts
{
    public abstract class GUIEntity : INotifyPropertyChanged
    {
        #region -- PROPERTIES --
        #region -- PUBLIC --
        public event PropertyChangedEventHandler PropertyChanged; 
        public EventAggregator _ea { get; set; }
        #endregion

        #region -- PRIVATE --
        public Users CurrentUser;
        #endregion 
        #endregion

        #region -- CONSTRUCTOR --
        public GUIEntity()
        {
            _ea = UnityCake.Unity.EventAggregator;

            _ea.GetEvent<ReplyLoggedUser>().Subscribe(u => CurrentUser = u);
            _ea.GetEvent<AskLoggedUser>().Publish();
        }
        #endregion

        #region -- FUNCTIONS --
        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
