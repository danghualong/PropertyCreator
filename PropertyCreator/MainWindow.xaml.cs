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

namespace PropertyCreator
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            rtbFields.Document = new FlowDocument(new Paragraph(new Run("private string name;")));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            rtbProperties.Document.Blocks.Clear();
            TextRange range = new TextRange(rtbFields.Document.ContentStart, rtbFields.Document.ContentEnd);
            string fields = range.Text;
            if (!string.IsNullOrEmpty(fields))
            {
                string result = GetResults(fields);
                //Paragraph p = new Paragraph();
                //p.Inlines.Add(new Run(result));
                //rtbProperties.Document.Blocks.Add(p);
                rtbProperties.Document = new FlowDocument(new Paragraph(new Run(result)));
            }
        }

        public string GetResults(String items)
        {
            string[] result = items.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            var sb = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                string item = CreateProperties(result[i].Trim());
                sb.Append(item).Append("\r\n");
            }
            return sb.ToString();
        }

        public string CreateProperties(string items)
        {
            string sep = "\r\n";
            items = items.Replace(";", "");
            string[] arrItems = items.Split(' ');
            string typeName = arrItems[1];
            string fieldName = arrItems[2];
            string PropertyName = fieldName.Substring(0, 1).ToUpper() + fieldName.Substring(1);
            StringBuilder sb = new StringBuilder();
            sb.Append("public ").Append(typeName).Append(" ").
                Append(PropertyName).Append(sep);
            sb.Append("{").Append(sep);
            sb.Append("get").Append(sep);
            sb.Append("{").Append(sep);
            sb.Append("return ").Append(fieldName).Append(";").Append(sep);
            sb.Append("}").Append(sep);

            sb.Append("set").Append(sep);
            sb.Append("{").Append(sep);
            if (typeName.Equals("string"))
            {
                sb.Append("if (!string.Equals(value,").Append(fieldName).Append("))").Append(sep);
            }
            else
            {
                sb.Append("if (value!=").Append(fieldName).Append(") ").Append(sep);
            }
            sb.Append("{").Append(sep);
            sb.Append(fieldName).Append("= value;").Append(sep);
            sb.Append("RaisePropertyChanged(() => ").Append(PropertyName).Append(");").Append(sep);
            sb.Append("}").Append(sep);
            sb.Append("}").Append(sep);
            sb.Append("}").Append(sep);
            return sb.ToString();
        }
    }
}
