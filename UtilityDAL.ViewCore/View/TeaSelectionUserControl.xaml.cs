using System.Reactive.Subjects;
using System.Windows;
using System.Windows.Controls;

namespace UtilityDAL.View
{
    /// <summary>
    /// Interaction logic for TeaSelectionUserControl.xaml
    /// </summary>
    public partial class TeaSelectionUserControl : UserControl
    {
        public static readonly DependencyProperty DirectoryProperty = DependencyProperty.Register("Directory", typeof(string), typeof(TeaSelectionUserControl), new PropertyMetadata(null, DirectoryChanged));

        private static void DirectoryChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as TeaSelectionUserControl).DirectoryChanges.OnNext((string)e.NewValue);
        }

        //[DirectoryPath]
        //[AutoUpdateText]
        public string Directory
        {
            get { return (string)GetValue(DirectoryProperty); }
            set { SetValue(DirectoryProperty, value); }
        }

        protected ISubject<string> DirectoryChanges = new Subject<string>();

        public TeaSelectionUserControl()
        {
            InitializeComponent();
            //this.Loaded += SelectionUserControl_Loaded;

            //DirectoryChanges.Where(_=>_!=null).Subscribe(_ =>
            //{
            //   tea.Directory = _;
            //});
            //sp.DataContext = this;
        }

        //private void SelectionUserControl_Loaded(object sender, RoutedEventArgs e)
        //{
        //    folder.DataContext = new FolderPickerViewModel((string)GetValue(DirectoryProperty));
        //    //maincontent.DataContext = new DirectoriesViewModel(x.Output,
        //    //    new UtilityWpf.DispatcherService(Application.Current.Dispatcher),
        //    //    "tea",
        //    //    path =>
        //    //    {
        //    //        using (var tf = TeaTime.TeaFile<Price>.OpenRead(path))
        //    //        {
        //    //            return tf.Items.ToList();
        //    //        }
        //    //    });

        //}
    }
}