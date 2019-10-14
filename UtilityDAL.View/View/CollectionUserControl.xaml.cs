using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace UtilityDAL.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class CollectionUserControl : UserControl
    {
        private ICollectionView source;
        private Type type;
        //public string[] DeveloperList { get; }

        public CollectionUserControl()
        {
            InitializeComponent();
            //var items = ModelFactory.Build();

            // this.DeveloperList = items.Select(t => t.Developer).Distinct().ToArray();
            grdMain.DataContext = this;

            //this.lvItems.DataContext = source;
            ItemsControl = new DataGrid { IsSynchronizedWithCurrentItem = true };
        }

        public Selector ItemsControl
        {
            get { return (Selector)GetValue(ItemsControlProperty); }
            set { SetValue(ItemsControlProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemsControl.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsControlProperty =
            DependencyProperty.Register("ItemsControl", typeof(Selector), typeof(CollectionUserControl), new PropertyMetadata(null));

        //  private static Selector itemsControl = ;

        public Type Type
        {
            get { return (Type)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Type.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TypeProperty =
            DependencyProperty.Register("Type", typeof(Type), typeof(CollectionUserControl), new PropertyMetadata(null));

        public IEnumerable Items
        {
            get { return (IEnumerable)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ts.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register("Items", typeof(IEnumerable), typeof(CollectionUserControl), new PropertyMetadata(null, ItemsChanged));

        private static void ItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as CollectionUserControl).type = (e.NewValue as IEnumerable).Cast<object>().First().GetType();

            (d as CollectionUserControl).source = CollectionViewSource.GetDefaultView((e.NewValue as IEnumerable));
            (d as CollectionUserControl).ItemsControl.ItemsSource = (d as CollectionUserControl).source;
            //var items = (d as CollectionUserControl).lvItems.Items;
        }

        private void ListView_Click(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader currentHeader = e.OriginalSource as GridViewColumnHeader;
            if (currentHeader != null && currentHeader.Role != GridViewColumnHeaderRole.Padding)
            {
                using (this.source.DeferRefresh())
                {
                    Func<SortDescription, bool> lamda = item => item.PropertyName.Equals(currentHeader.Column.Header.ToString());
                    if (this.source.SortDescriptions.Count(lamda) > 0)
                    {
                        SortDescription currentSortDescription = this.source.SortDescriptions.First(lamda);
                        ListSortDirection sortDescription = currentSortDescription.Direction == ListSortDirection.Ascending ? ListSortDirection.Descending : ListSortDirection.Ascending;

                        currentHeader.Column.HeaderTemplate = currentSortDescription.Direction == ListSortDirection.Ascending ?
                            this.Resources["HeaderTemplateArrowDown"] as DataTemplate : this.Resources["HeaderTemplateArrowUp"] as DataTemplate;

                        this.source.SortDescriptions.Remove(currentSortDescription);
                        this.source.SortDescriptions.Insert(0, new SortDescription(currentHeader.Column.Header.ToString(), sortDescription));
                    }
                    else
                        this.source.SortDescriptions.Add(new SortDescription(currentHeader.Column.Header.ToString(), ListSortDirection.Ascending));
                }
            }
        }

        private void buttonAppy_Click(object sender, RoutedEventArgs e)
        {
            this.source.Filter = item =>
            {
                PropertyInfo info = item.GetType().GetProperty(FilterControl.Property);
                if (info == null)
                    return false;

                return info.GetValue(item, null).ToString().Contains(FilterControl.Value);
            };
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            this.source.Filter = item => true;
        }

        private void btnGroup_Click(object sender, RoutedEventArgs e)
        {
            this.source.GroupDescriptions.Clear();

            PropertyInfo pinfo = type.GetProperty(GroupControl.Property);
            if (pinfo != null)
                this.source.GroupDescriptions.Add(new PropertyGroupDescription(pinfo.Name));
        }

        private void btnClearGr_Click(object sender, RoutedEventArgs e)
        {
            this.source.GroupDescriptions.Clear();
        }

        private void btnNavigation_Click(object sender, RoutedEventArgs e)
        {
            Button CurrentButton = sender as Button;

            switch (CurrentButton.Tag.ToString())
            {
                case "0":
                    this.source.MoveCurrentToFirst();
                    break;

                case "1":
                    this.source.MoveCurrentToPrevious();
                    break;

                case "2":
                    this.source.MoveCurrentToNext();
                    break;

                case "3":
                    this.source.MoveCurrentToLast();
                    break;
            }
        }

        private void btnEvaluate_Click(object sender, RoutedEventArgs e) =>

            MessageBox.Show(this.ItemsControl.SelectedItem.GetType()
                .GetProperties()
                .ToDictionary(a => a.Name, a => a.GetValue(this.ItemsControl.SelectedItem, null))
                .Aggregate(new StringBuilder(), (a, b) => a.Append(b.Key + " : " + b.Value + "  "))
                .ToString());

        private void LvlItems_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ListView_Click(null, null);
        }
    }
}