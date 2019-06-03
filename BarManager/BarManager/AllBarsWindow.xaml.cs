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
        public ObservableCollection<BarLabel> _alllabels = new ObservableCollection<BarLabel>();
        public ObservableCollection<BarLabel> AllLabels
        {
            get
            {
                return _alllabels;
            }
            set
            {
                if (value != _alllabels)
                {
                    _alllabels = value;
                    OnPropertyChanged("AllLabels");
                }
            }
        }
        public ObservableCollection<BarType> _allTypes = new ObservableCollection<BarType>();
        public ObservableCollection<BarType> AllTypes
        {
            get
            {
                return _allTypes;
            }
            set
            {
                if (value != _allTypes)
                {
                    _allTypes = value;
                    OnPropertyChanged("AllTypes");
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

            foreach (BarLabel b in Util.Util.barLabels)
            {
                AllLabels.Add(b);
            }
            foreach (BarType b in Util.Util.barTypes)
            {
                AllTypes.Add(b);
            }

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

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            barTble.Visibility = Visibility.Visible;
            typesTable.Visibility = Visibility.Collapsed;
            labelsTable.Visibility = Visibility.Collapsed;
            FilterBtn.Visibility = Visibility.Visible;
            searchBtn.Visibility = Visibility.Visible;
            searchInput.Visibility = Visibility.Visible;

        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            barTble.Visibility = Visibility.Collapsed;
            typesTable.Visibility = Visibility.Visible;
            labelsTable.Visibility = Visibility.Collapsed;
            FilterBtn.Visibility = Visibility.Collapsed;
            searchBtn.Visibility = Visibility.Collapsed;
            searchInput.Visibility = Visibility.Collapsed;
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            barTble.Visibility = Visibility.Collapsed;
            typesTable.Visibility = Visibility.Collapsed;
            labelsTable.Visibility = Visibility.Visible;
            FilterBtn.Visibility = Visibility.Collapsed;
            searchBtn.Visibility = Visibility.Collapsed;
            searchInput.Visibility = Visibility.Collapsed;

        }
    }
}
