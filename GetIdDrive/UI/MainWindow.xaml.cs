using GetIdDrive.UI.ViewModels;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TqkLibrary.Net.CloudStorage.GoogleDrive;
using Newtonsoft.Json;
namespace GetIdDrive.UI
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    readonly MainWindowViewModel mainWindowViewModel;
    public MainWindow()
    {
      InitializeComponent();
      this.mainWindowViewModel = this.DataContext as MainWindowViewModel;
    }

    private void btn_GetChildsId_Click(object sender, RoutedEventArgs e)
    {
      SaveFileDialog saveFileDialog = new SaveFileDialog();
      saveFileDialog.InitialDirectory = Directory.GetCurrentDirectory();
      saveFileDialog.Filter = "txt file|*.txt|All file|*.*";
      if(saveFileDialog.ShowDialog() == true)
      {
        try
        {
          GetData(saveFileDialog.FileName);
        }
        catch(Exception ex)
        {
          MessageBox.Show(ex.Message + ex.StackTrace, ex.GetType().FullName);
        }
      }
    }


    //0Bx154iMNwuyWfnZJVkxQTHJJY2J5X19pUTNabkxlWVNrUE9OUTJOVFdYWE11bkpSbDlFc0k
    async void GetData(string savePath)
    {
      DriveFileListOption option = new DriveFileListOption();
      option.maxResults = 1000;
      option.CreateQueryFolder(mainWindowViewModel.ParentId);
      List<string> ids = new List<string>();
      while(true)
      {
        string json = await DriveApiNonLogin.ListPublicFolder(option);
        DriveList driveList = JsonConvert.DeserializeObject<DriveList>(json);
        ids.AddRange(driveList.items.Select(x => x.id));
        if (!string.IsNullOrEmpty(driveList.nextPageToken))
        {
          option.pageToken = driveList.nextPageToken;
        }
        else break;
      }

      File.WriteAllLines(savePath, ids);
     }
  }


  class DriveList
  {
    public bool incompleteSearch { get; set; }
    public List<DriveItem> items { get; set; }
    public string nextPageToken { get; set; }
    public string nextLink { get; set; }
  }

  class DriveItem
  {
    public string id { get; set; }
  }
}
