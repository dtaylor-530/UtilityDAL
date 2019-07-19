using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace UtilityDAL.DemoApp
{
    /// <summary>
    /// Interaction logic for GuidStringPage.xaml
    /// </summary>
    public partial class GuidStringPage : Page
    {

        public string AText { get; set; } = "hello world";

        public GuidStringPage()
        {
            InitializeComponent();

            A.DataContext = this;

            A.TextChanged += A_TextChanged;
        }


        private void A_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = A.Text.TrimEnd();

            if (text.Length <= 16)
            {
                text = text + new string(Enumerable.Range(0, 16 - text.Length).Select(_ => ' ').ToArray());
                B.Text = UtilityDAL.GUIDParse.ToGUID(text).ToString();
            }
  
        }




        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (B.Text != "")
            {
                C.Text = UtilityDAL.GUIDParse.FromGUID(Guid.Parse(B.Text)).ToString();
            }
        }


      
    }

    class LengthValidationRule : ValidationRule
    {
        public Type ValidationType { get; set; }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {


            string strValue = Convert.ToString(value).TrimEnd();

            if (string.IsNullOrEmpty(strValue))
                return new ValidationResult(false, $"Value is null or empty");

            if (strValue.Length > 16)
                return new ValidationResult(false, $"Value must be shorter or equal to 16 characters");

            return new ValidationResult(true, null);

        }
    }
}

