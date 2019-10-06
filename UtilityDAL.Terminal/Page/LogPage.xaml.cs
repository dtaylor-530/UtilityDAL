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
    /// Interaction logic for LogPage.xaml
    /// </summary>
    public partial class LogPage : Page
    {

        public LogPage()
        {
            InitializeComponent();

            var xx = new Bogus.Faker<Model.Log>().
                RuleFor(o => o.Date, f => f.Date.Recent())
                .RuleFor(o => o.Issue, f => f.PickRandom<Issue>())
                .RuleFor(o => o.Key, f => f.Random.String(10))
                .RuleFor(o => o.Source, f => f.Random.String(16))
                .RuleFor(o => o.Message, f => f.Lorem.Sentence()).Generate(19);

            CollectionUserControl.Items = xx;
        }

    }
}
