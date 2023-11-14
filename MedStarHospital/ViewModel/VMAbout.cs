using MedStarHospital.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MedStarHospital.ViewModel
{
    public class VMAbout : ViewModelBase
    {
        public ICommand cmdBack { get { return new RelayCommand(fnExit); } }
        public static Action exit;
        void fnExit(object param)
        {
            LoginPageView mm =  new LoginPageView();
            mm.DataContext = new VMLogin();
            exit.Invoke();
            mm.ShowDialog();
        }
    }
}
