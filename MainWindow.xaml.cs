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
using System.IO;
using System.Windows.Controls.Primitives;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Organization> organizationList = new List<Organization>();
        private void Betoltes(string fileName)
        {
            foreach (var sor in File.ReadLines(fileName).Skip(1))
                organizationList.Add(new Organization(sor.Split(";")));
        }

        public MainWindow()
        {
            InitializeComponent();
            Betoltes("organizations-100000.csv");
            dgAdatok.ItemsSource = organizationList;
            cbOrszag.ItemsSource = organizationList.Select(x => x.Country).OrderBy(x => x).Distinct().ToList();
            cbEv.ItemsSource = organizationList.Select(x => x.Founded).OrderBy(x => x).Distinct().ToList();
            
            labTotalEmpl.Content = organizationList.Sum(x => x.EmployeesNumber);
        }


        private void dgAdatok_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgAdatok.SelectedItem is Organization)
            {
                Organization selected = dgAdatok.SelectedItem as Organization;
                labId.Content = selected.Id.ToString();
                labWeb.Content = selected.Website.ToString();
                labDescription.Content = selected.Description.ToString();
            }
            else if(dgAdatok.SelectedItem is null)
            {
                labId.Content = "";
                labWeb.Content = "";
                labDescription.Content = "";
            }
            else
                MessageBox.Show("Hiba!");
        }

        private void Filter(object sender, SelectionChangedEventArgs e)
        {
            List<Organization> FilteredList = new List<Organization>();

            if (cbOrszag.SelectedIndex != -1 && cbEv.SelectedIndex != -1)
            {
                string orszagFilter = cbOrszag.SelectedItem.ToString();
                string evFilter = cbEv.SelectedItem.ToString();
                foreach (Organization item in organizationList)
                {
                    if (item.Country == orszagFilter && item.Founded == Convert.ToInt32(evFilter))
                        FilteredList.Add(item);
                }
            }
            else if(cbOrszag.SelectedIndex != -1)
            {
                string orszagFilter = cbOrszag.SelectedItem.ToString();
                foreach (Organization item in organizationList)
                {
                    if (item.Country == orszagFilter)
                        FilteredList.Add(item);
                }
            }
            else if(cbEv.SelectedIndex != -1)
            {
                string evFilter = cbEv.SelectedItem.ToString();
                foreach (Organization item in organizationList)
                {
                    if (item.Founded == Convert.ToInt32(evFilter))
                        FilteredList.Add(item);
                }
            }
            
            dgAdatok.ItemsSource = FilteredList;
            labTotalEmpl.Content = FilteredList.Sum(x => x.EmployeesNumber);
        }

        private void ResetFilters(object sender, RoutedEventArgs e)
        {  
            cbEv.SelectedIndex = -1;
            cbOrszag.SelectedIndex = -1;

            dgAdatok.ItemsSource = organizationList;
            labTotalEmpl.Content = organizationList.Sum(x => x.EmployeesNumber);

            labId.Content = "";
            labWeb.Content = "";
            labDescription.Content = "";
        }
    }
}
