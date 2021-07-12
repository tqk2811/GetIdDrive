using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TqkLibrary.WpfUi;

namespace GetIdDrive.UI.ViewModels
{
  class MainWindowViewModel : BaseViewModel
  {
    string _ParentId;
    public string ParentId
    {
      get { return _ParentId; }
      set { _ParentId = value; NotifyPropertyChange(); }
    }
  }
}
