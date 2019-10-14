using System.Linq;
using System.Windows.Controls;
using UtilityDAL.Model;

namespace UtilityDAL.DemoApp
{
    /// <summary>
    /// Interaction logic for ServerFilePage.xaml
    /// </summary>
    public partial class ServerFilePage : Page
    {
        public ServerFilePage()
        {
            InitializeComponent();
            serverFilesControl.ItemsSource = Enumerable.Range(0, 20).Select(_ =>
             new ServerFile
             {
                 Download = Faker.Date.PastWithTime(),
                 Upload = Faker.Date.PastWithTime() /*File =new System.IO.FileInfo()*/,
                 Link = "www.xd.com/link"
             });
        }
    }
}