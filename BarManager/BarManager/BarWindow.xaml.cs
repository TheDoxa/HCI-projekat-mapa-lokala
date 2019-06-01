using BarManager.Model;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System;
using Microsoft.Win32;


namespace BarManager
{
    /// <summary>
    /// Interaction logic for BarWindow.xaml
    /// </summary>
    public partial class BarWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        private double _maxCapacity = 1;

        public double MaxCapacity
        {
            get
            {
                return _maxCapacity;
            }
            set
            {
                if (value != _maxCapacity)
                {
                    _maxCapacity = value;
                    OnPropertyChanged("MaxCapacity");
                }
            }
        }

        private double _barID = 1;

        public double BarID
        {
            get
            {
                return _barID;
            }
            set
            {
                if (value != _barID)
                {
                    _barID = value;
                    OnPropertyChanged("BarID");
                }
            }
        }

        public string _imageSource = "Images/barView.jpg";
        public string ImageSource
        {
            get
            {
                return _imageSource;
            }
            set
            {
                if (value != _imageSource)
                {
                    _imageSource = value;
                    OnPropertyChanged("ImageSource");
                }
            }
        }

        public List<string> AlcoholStatusCollection { get; set; }
        public List<string> PriceStatusCollection { get; set; }
        public List<string> TypeCollection { get; set; }

        public BarWindow()
        {
            InitializeComponent();
            initDatas();
            this.DataContext = this;
        }

        private void CancelNewBarBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AddNewBarBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BarID_TextChanged(object sender, TextChangedEventArgs e)
        {
            barID.Text = Regex.Replace(barID.Text, "[^0-9]+", "");
        }


        private void initDatas()
        {
            AlcoholStatusCollection = new List<string>()
            {
                "Low prices",
                "Medium prices",
                "High prices",
                "Extremely high prices"
            };

            PriceStatusCollection = new List<string>()
            {
                "Don't server",
                "Serves up to 23h",
                "Full serve"
            };

            TypeCollection = new List<string>();

            Util.Util.loadTypes();

            foreach (BarType type in Util.Util.barTypes)
            {
                TypeCollection.Add(type.Name);
            }

            Util.Util.loadLabels();

            foreach (BarLabel label in Util.Util.barLabels)
            {
                allLabelsList.Items.Add(label.Id + " " + label.Color);
            }

        }

        private void BarCapacity_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            barCapacity.Text = Regex.Replace(barCapacity.Text, "[^0-9]+", "");
        }

        private void BarType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            setImageOfSelectedType();
        }

        private void ReturnDefaultImage_Click(object sender, RoutedEventArgs e)
        {
            setImageOfSelectedType();
        }

        private void setImageOfSelectedType()
        {
            string currentType = barType.SelectedValue.ToString();
            foreach (BarType type in Util.Util.barTypes)
            {
                if (type.Name == currentType)
                {
                    ImageSource = type.IconPath;
                }
            }
        }

        private void AutoIncBarIDBtn_Click(object sender, RoutedEventArgs e)
        {
            bool check = true;
            int id = 1;
            while (check)
            {
                check = false;
                foreach (Bar bar in Util.Util.bars)
                {
                    if (bar.Id == id)
                    {
                        check = true;
                        break;
                    }
                }
                if (!check)
                {
                    BarID = id;
                }
            }
        }

        private void ChooseBarImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Choose a picture";
            op.Filter = "Images|*.jpg;*.jpeg;*.png";
            
            if (op.ShowDialog() == true)
            {
                ImageSource = op.FileName;                
            }
            else
            {
                setImageOfSelectedType();
            }
        }
    }
}
