using BarManager.Model;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System;
using Microsoft.Win32;
using System.Collections.ObjectModel;

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

        private int _maxCapacity = 1;
        public int MaxCapacity
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

        private int _barID = 1;
        public int BarID
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

        private string _barName = "";
        public string BarName
        {
            get
            {
                return _barName;
            }
            set
            {
                if (value != _barName)
                {
                    _barName = value;
                    OnPropertyChanged("BarName");
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

        public ObservableCollection<BarLabel> _availableLabels = new ObservableCollection<BarLabel>();
        public ObservableCollection<BarLabel> AvailableLabels
        {
            get
            {
                return _availableLabels;
            }
            set
            {
                if(value != _availableLabels)
                {
                    _availableLabels = value;
                    OnPropertyChanged("AvailableLabels");
                }
            }
        }

        public ObservableCollection<BarLabel> _chosenLabels = new ObservableCollection<BarLabel>();
        public ObservableCollection<BarLabel> ChosenLabels
        {
            get
            {
                return _chosenLabels;
            }
            set
            {
                if (value != _chosenLabels)
                {
                    _chosenLabels = value;
                    OnPropertyChanged("ChosenLabels");
                }
            }
        }
       
        public ObservableCollection<BarType> _typeCollection = new ObservableCollection<BarType>();
        public ObservableCollection<BarType> TypeCollection
        {
            get
            {
                return _typeCollection;
            }
            set
            {
                if (value != _typeCollection)
                {
                    _typeCollection = value;
                    OnPropertyChanged("TypeCollection");
                }
            }
        }


        public List<string> AlcoholStatusCollection { get; set; }
        public List<string> PriceStatusCollection { get; set; }
        

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

            Bar bar = new Bar(BarID, BarName, barDescription.Text,(BarType) barType.SelectedItem, barAlcStatus.SelectedItem.ToString(), ImageSource, (bool) barHandicapped.IsChecked,
                (bool) barSmoking.IsChecked, (bool) barReservation.IsChecked, barPriceStatus.SelectedItem.ToString(), MaxCapacity, barDate.SelectedDate.Value.Date, ChosenLabels);
            bool res = Util.Util.addBar(bar);
            if (!res)
            {
                System.Windows.Forms.MessageBox.Show("Id already taken!", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return;
            }
            this.Close();
        }

        private void BarID_TextChanged(object sender, TextChangedEventArgs e)
        {
            barID.Text = Regex.Replace(barID.Text, "[^0-9]+", "");
            barID.CaretIndex = barID.Text.Length;
            enableAddBarBtn();
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
            

            Util.Util.loadTypes();

            foreach(BarType t in Util.Util.barTypes)
            {
                TypeCollection.Add(t);
            }

            Util.Util.loadLabels();

            foreach (BarLabel label in Util.Util.barLabels)
            {
                AvailableLabels.Add(label);
            }
            ChosenLabels.Clear();

            Util.Util.loadBars();

        }

        private void BarCapacity_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            barCapacity.Text = Regex.Replace(barCapacity.Text, "[^0-9]+", "");
            barCapacity.CaretIndex = barCapacity.Text.Length;
            enableAddBarBtn();
        }

        private void BarType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            setImageOfSelectedType();
            enableAddBarBtn();
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
                else
                {
                    id++;
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

        private void AddlabelBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int idx = availableLabelsList.SelectedIndex;
                BarLabel barLabel = (BarLabel)availableLabelsList.SelectedItem;
                AvailableLabels.RemoveAt(idx);
                ChosenLabels.Add(barLabel);
            }catch
            {
                //dialog maybe?
            }

        }

        private void RemoveLabelBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int idx = chosenLabelsList.SelectedIndex;
                BarLabel barLabel = (BarLabel)chosenLabelsList.SelectedItem;
                ChosenLabels.RemoveAt(idx);
                AvailableLabels.Add(barLabel);
            }
            catch
            {
                //dialog maybe?
            }
        }

        private void BarName_TextChanged(object sender, TextChangedEventArgs e)
        {
            enableAddBarBtn();
        }

        private void enableAddBarBtn()
        {
            if (barID.Text.Length == 0 || barName.Text.Length < 3 || barCapacity.Text.Length == 0 || Util.Util.barTypes.Count == 0)
            {
                addNewBarBtn.IsEnabled = false;
            }
            else
            {
                int capacity = 9999;
                Int32.TryParse(barCapacity.Text, out capacity);
                if (capacity <= 2000 && capacity >=1)
                {
                    addNewBarBtn.IsEnabled = true;
                }
                else
                {
                    addNewBarBtn.IsEnabled = false;
                }
            }
        }

        private void AddNewTypeFromBarFormBtn_Click(object sender, RoutedEventArgs e)
        {
            NewType newTypeWindow = new NewType();
            newTypeWindow.Show();
        }
    }
}
