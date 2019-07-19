using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using UtilityDAL.Model;
using UtilityWpf.View;

namespace UtilityDAL.View
{

    public class ServerFilesControl:Control
    {
        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }


        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(ServerFilesControl), new PropertyMetadata(null,ItemsSourceChanged));

        
        private static void ItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((d as ServerFilesControl).listboxCriteria != null)
                (d as ServerFilesControl).ItemsSource = (IEnumerable)(e.NewValue);
        }

        ListBoxCriteria listboxCriteria = null;
        private ComboBox comboBox;
        private ServerFile[] files;
        private Button update;


        static ServerFilesControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ServerFilesControl), new FrameworkPropertyMetadata(typeof(ServerFilesControl)));
        }

        public override void OnApplyTemplate()
        {
             listboxCriteria = this.GetTemplateChild("listboxCriteria") as ListBoxCriteria;
            comboBox = this.GetTemplateChild("ComboBox") as ComboBox;
            listboxCriteria.ItemsSource = ItemsSource;
            update = this.GetTemplateChild("Update") as Button;
            update.Click += Update_Click;
            listboxCriteria.CriteriaMet += ListboxCriteria_CriteriaMet;
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new FilesToDownloadEventArgs(FilesToDownloadEvent) { Files = files });
        }

        private void ListboxCriteria_CriteriaMet(object sender, RoutedEventArgs e)
        {
            //var args = e as FilesToDownloadEventArgs;
            var args = e as ListBoxCriteria.CriteriaMetEventArgs;
            files = (listboxCriteria).Items.Cast<Model.ServerFile>()
                .Select((_, i) => new { _, i })
                .Where(_ => args.Indices.Contains(_.i)).Select(_ => _._).ToArray();

            comboBox.ItemsSource = files.Where(_=>_.Link!=null).ToList();

        }



        public static readonly RoutedEvent FilesToDownloadEvent = EventManager.RegisterRoutedEvent("FilesToDownload", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(CriteriaItem));

        public event RoutedEventHandler FilesToDownload
        {
            add { AddHandler(FilesToDownloadEvent, value); }
            remove { RemoveHandler(FilesToDownloadEvent, value); }
        }

        public class FilesToDownloadEventArgs : RoutedEventArgs
        {
            public Model.ServerFile[] Files;

            public FilesToDownloadEventArgs(RoutedEvent @event) : base(@event)
            { }
        }

    }
}
