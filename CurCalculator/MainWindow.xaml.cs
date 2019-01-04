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
using System.Xml.Linq;

namespace CurCalculator
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            List<Currency> currencies = new List<Currency>();
            currencies.Add(new Currency() { Title = "KZT", Description = 1.0, PubDate = DateTime.Now });
                        
            cbxCur2.ItemsSource = GetDataFromXML("http://www.nationalbank.kz/rss/rates_all.xml");
            cbxCur2.SelectedIndex = 0;           
        }        
        
        private List<Currency> GetDataFromXML(string pathToXML)
        {
            List<Currency> list = new List<Currency>();
            
            try
            {
                XDocument xdoc = XDocument.Load(pathToXML);

                foreach (XElement item in xdoc.Element("rss").Element("channel").Elements("item"))
                {
                    Currency cur = new Currency();
                    cur.Title = item.Element("title").Value;
                    cur.PubDate = Convert.ToDateTime(item.Element("pubDate").Value);
                    cur.Description = Convert.ToDouble(item.Element("description").Value.Replace(".", ","));

                    list.Add(cur);
                }

            }
            catch (Exception ex)
            {
                lblMessage.Content = ex.Message;
            }

            return list;
        }
           
        private void CbxCur2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Currency cvl = (Currency)cbxCur2.SelectedItem;
            double value = Math.Round((double.Parse(tbxSum1.Text)), 2);

            lblCurrency.Content = cvl.Title;

            tbxSum2.Text = Convert.ToString(value / cvl.Description);           
        }

        private void TbxSum1_KeyUp(object sender, KeyEventArgs e)
        {
            lblCurrency.Content = "";
            double sum = Math.Round((double.Parse(tbxSum1.Text)), 2);
            Currency cur = (Currency)cbxCur2.SelectedItem;
            
            string totalSum = (sum / cur.Description).ToString();
            tbxSum2.Text = totalSum;

            lblCurrency.Content = cur.Title;
            lblTotalSum.Content = totalSum;
            
        }

        private void TbxSum2_KeyUp(object sender, KeyEventArgs e)
        {
            double sum = Math.Round((double.Parse(tbxSum2.Text)), 2);
            Currency cur = (Currency)cbxCur2.SelectedItem;

            string totalSum = (sum * cur.Description).ToString();
            tbxSum1.Text = totalSum;

            lblTotalSum.Content = totalSum;

            lblCurrency.Content = "KZT";
        }
    
    }
}
