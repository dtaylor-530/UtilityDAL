using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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
