using BarManager.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using System.Windows.Shapes;

namespace BarManager
{
    /// <summary>
    /// Interaction logic for AllBarsWindow.xaml
    /// </summary>
    public partial class AllBarsWindow : Window
    {

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public ObservableCollection<Bar> _allBars = new ObservableCollection<Bar>();
        public ObservableCollection<Bar> AllBars
        {
            get
            {
                return _allBars;
            }
            set
            {
                if (value != _allBars)
                {
                    _allBars = value;
                    OnPropertyChanged("AllBars");
                }
            }
        }
        public AllBarsWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            InitData();

        }

        private void InitData()
        {
            Util.Util.loadLabels();
            Util.Util.loadTypes();
            Util.Util.loadBars();

            foreach (Bar b in Util.Util.bars)
            {
                AllBars.Add(b);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FilterWindow fw = new FilterWindow();
            fw.allBarsWindow = this;
            fw.Show();
            this.DataContext = this;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {

        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            ObservableCollection<Bar> pomocna = new ObservableCollection<Bar>();
            string searchText = searchInput.Text.ToLower();
            string[] tokens = searchText.Split(' ');
            foreach (Bar b in Util.Util.bars)
            {
                if (b.Name.ToLower().Contains(searchText))
                {
                    pomocna.Add(b);
                }
                else if (b.Description.ToLower().Contains(searchText))
                {
                    pomocna.Add(b);
                }
                else if (b.Type.Name.ToLower().Contains(searchText))
                {
                    pomocna.Add(b);
                }
                else if (b.AlcStatus.ToLower().Contains(searchText))
                {
                    pomocna.Add(b);
                }
                else
                {
                    foreach (string t in tokens)
                    {
                        if (b.Name.ToLower().Contains(t))
                        {
                            pomocna.Add(b);
                            break;
                        }
                        else if (b.Description.ToLower().Contains(t))
                        {
                            pomocna.Add(b);
                            break;
                        }
                        else if (b.Type.Name.ToLower().Contains(t))
                        {
                            pomocna.Add(b);
                            break;
                        }
                        else if (b.AlcStatus.ToLower().Contains(searchText))
                        {
                            pomocna.Add(b);
                        }
                        else
                        {
                            foreach (BarLabel bl in b.Labels)
                            {
                                if (bl.Description.ToLower().Contains(t))
                                {
                                    pomocna.Add(b);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            barTble.ItemsSource = pomocna;
        }
    }
}
