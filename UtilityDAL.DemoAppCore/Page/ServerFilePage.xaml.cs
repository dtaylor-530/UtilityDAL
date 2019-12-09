﻿using System.Linq;
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
            serverFilesControl.ItemsSource = new Bogus.Faker<ServerFile>()
               .RuleFor(a => a.Download, f => f.Date.Past())
                       .RuleFor(a => a.Upload, f => f.Date.Past())
                               .RuleFor(a => a.Link, f => "www.xd.com/link")
                               .Generate(20);

        }
    }
}