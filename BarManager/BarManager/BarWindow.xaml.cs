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

        public string _imageSource = "";
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

        public BarType _selectedType = new BarType();
        public BarType SelectedType
        {
            get
            {
                return _selectedType;
            }
            set
            {
                if (value != _selectedType)
                {
                    _selectedType = value;
                    OnPropertyChanged("SelectedType");
                }
            }
        }

        public DateTime _selectedDate = new DateTime();
        public DateTime SelectedDate
        {
            get
            {
                return _selectedDate;
            }
            set
            {
                if (value != _selectedDate)
                {
                    _selectedDate = value;
                    OnPropertyChanged("SelectedDate");
                }
            }
        }

        public string _alcoholStatus = "";
        public string AlcoholStatus
        {
            get
            {
                return _alcoholStatus;
            }
            set
            {
                if (value != _alcoholStatus)
                {
                    _alcoholStatus = value;
                    OnPropertyChanged("AlcoholStatus");
                }
            }
        }

        public string _priceStatus = "";
        public string PriceStatus
        {
            get
            {
                return _priceStatus;
            }
            set
            {
                if (value != _priceStatus)
                {
                    _priceStatus = value;
                    OnPropertyChanged("PriceStatus");
                }
            }
        }

        public string _description = "";
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                if (value != _description)
                {
                    _description = value;
                    OnPropertyChanged("Description");
                }
            }
        }

        public bool _handicapped = false;
        public bool Handicapped
        {
            get
            {
                return _handicapped;
            }
            set
            {
                if (value != _handicapped)
                {
                    _handicapped = value;
                    OnPropertyChanged("Handicapped");
                }
            }
        }

        public bool _smoking = false;
        public bool Smoking
        {
            get
            {
                return _smoking;
            }
            set
            {
                if (value != _smoking)
                {
                    _smoking = value;
                    OnPropertyChanged("Smoking");
                }
            }
        }

        public bool _reservations = false;
        public bool Reservations
        {
            get
            {
                return _reservations;
            }
            set
            {
                if (value != _reservations)
                {
                    _reservations = value;
                    OnPropertyChanged("Reservations");
                }
            }
        }


        public ObservableCollection<string> AlcoholStatusCollection { get; set; }
        public ObservableCollection<string> PriceStatusCollection { get; set; }

        public bool Edit { get; set; }
        public Bar EditBar { get; set; }

		private AllBarsWindow parent = null;

		public BarWindow(bool edit, Bar editBar, AllBarsWindow parent)
        {
            InitializeComponent();
            Edit = edit;
            if (Edit)
            {
                this.Title = "Edit Bar";
                addNewBarBtn.Content = "Save changes";
                EditBar = editBar;
                initDatasEditBar();
            }
            else
            {
                this.Title = "Add bar";
                addNewBarBtn.Content = "Add bar";
                EditBar = new Bar();
                initDatasNewBar();
            }
            this.DataContext = this;

			this.parent = parent;
		}

        private void CancelNewBarBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AddNewBarBtn_Click(object sender, RoutedEventArgs e)
        {

			if(Edit) {
				Bar modifiedBar = new Bar(BarID, BarName, Description, SelectedType, AlcoholStatus, ImageSource, Handicapped, Smoking, Reservations, PriceStatus,
								MaxCapacity, SelectedDate, ChosenLabels);
				Util.Util.modifyBar(modifiedBar);

				for(int i = 0; i < AllBarsWindow.AllBars.Count; i++) {
					if(AllBarsWindow.AllBars[i].Id == modifiedBar.Id) {
						AllBarsWindow.AllBars.RemoveAt(i);
						AllBarsWindow.AllBars.Add(modifiedBar);

						break;
					}
				}

				for(int i = 0; i < AllBarsWindow.Pomocna.Count; i++) {
					if(AllBarsWindow.Pomocna[i].Id == modifiedBar.Id) {
						AllBarsWindow.Pomocna.RemoveAt(i);
						AllBarsWindow.Pomocna.Add(modifiedBar);

						break;
					}
				}
			} else {
				Bar bar = new Bar(BarID, BarName, Description, SelectedType, AlcoholStatus, ImageSource, Handicapped, Smoking, Reservations, PriceStatus,
								MaxCapacity, SelectedDate, ChosenLabels);
				bool res = Util.Util.addBar(bar);
				if(!res) {
					System.Windows.Forms.MessageBox.Show("Id already taken!", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
					return;
				}

				if(parent != null)
					AllBarsWindow.AllBars.Add(bar);
			}

			this.Close();
        }

        private void BarID_TextChanged(object sender, TextChangedEventArgs e)
        {
            barID.Text = Regex.Replace(barID.Text, "[^0-9]+", "");
            barID.CaretIndex = barID.Text.Length;
            enableAddBarBtn();
        }


        private void initDatasEditBar()
        {

            PriceStatusCollection = new ObservableCollection<string>()
            {
                "Low prices",
                "Medium prices",
                "High prices",
                "Extremely high prices"
            };

            AlcoholStatusCollection = new ObservableCollection<string>()
            {
                "Don't serve",
                "Serves up to 23h",
                "Full serve"
            };
            
            foreach (BarType t in Util.Util.BarTypes)
            {
                TypeCollection.Add(t);
            }

            AvailableLabels.Clear();
            ChosenLabels.Clear();

            barID.IsEnabled = false;
            BarID = EditBar.Id;
            autoIncBarIDBtn.Visibility = Visibility.Collapsed;
            BarName = EditBar.Name;
            SelectedType = EditBar.Type;
            SelectedDate = EditBar.OpenDate;
            AlcoholStatus = EditBar.AlcStatus;
            PriceStatus = EditBar.PriceCategory;
            Description = EditBar.Description;
            Handicapped = EditBar.Handicapped;
            Smoking = EditBar.SmokingAllowed;
            Reservations = EditBar.ReservationsAllowed;
            MaxCapacity = EditBar.MaxCapacity;
            ImageSource = EditBar.Icon;
            ChosenLabels = EditBar.Labels;
            bool checkLab = true;
            foreach(BarLabel lab in Util.Util.BarLabels)
            {
                checkLab = true;
                foreach(BarLabel l in ChosenLabels)
                {
                    if(lab.Id == l.Id)
                    {
                        checkLab = false;
                        break;
                    }
                }
                if (checkLab)
                {
                    AvailableLabels.Add(lab);
                }
            }
        }


        private void initDatasNewBar()
        {
            PriceStatusCollection = new ObservableCollection<string>()
            {
                "Low prices",
                "Medium prices",
                "High prices",
                "Extremely high prices"
            };

            AlcoholStatusCollection = new ObservableCollection<string>()
            {
                "Don't serve",
                "Serves up to 23h",
                "Full serve"
            };
            
            foreach(BarType t in Util.Util.BarTypes)
            {
                TypeCollection.Add(t);
            }
            SelectedType = Util.Util.BarTypes[0];

            foreach (BarLabel label in Util.Util.BarLabels)
            {
                AvailableLabels.Add(label);
            }
            ChosenLabels.Clear();

            AlcoholStatus = AlcoholStatusCollection[0];
            PriceStatus = PriceStatusCollection[0];
            SelectedDate = DateTime.Now;


        }

        private void BarCapacity_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            barCapacity.Text = Regex.Replace(barCapacity.Text, "[^0-9]+", "");
            barCapacity.CaretIndex = barCapacity.Text.Length;
            enableAddBarBtn();
        }

        private void BarType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedType == null || SelectedType.IconPath == null)
            {
                SelectedType = Util.Util.BarTypes[0];
            }
            setImageOfSelectedType();
            enableAddBarBtn();
        }

        private void ReturnDefaultImage_Click(object sender, RoutedEventArgs e)
        {
            ImageSource = SelectedType.IconPath;
        }

        private void setImageOfSelectedType()
        {
            if (Edit && SelectedType.Id == EditBar.Type.Id)
            {
                ImageSource = EditBar.Icon;
            }
            else
            {
                ImageSource = SelectedType.IconPath;
            }
            
        }

        private void AutoIncBarIDBtn_Click(object sender, RoutedEventArgs e)
        {
            bool check = true;
            int id = 1;
            while (check)
            {
                check = false;
                foreach (Bar bar in Util.Util.Bars)
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
            if (addNewBarBtn == null)
                return;

            if (barID.Text.Length == 0 || barName.Text.Length < 3 || barCapacity.Text.Length == 0 || Util.Util.BarTypes.Count == 0 || ImageSource == "")
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
			NewTypeWindow newTypeWindow = new NewTypeWindow(false);
            newTypeWindow.Show();
        }

		private void HelpCommand_CanExecute(object sender, System.Windows.Input.CanExecuteRoutedEventArgs e) {
			e.CanExecute = true;
		}

		private void HelpCommand_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e) {
			HelpProvider.ShowHelp(this);
		}

		private void EscapeCommand_CanExecute(object sender, System.Windows.Input.CanExecuteRoutedEventArgs e) {
			e.CanExecute = true;
		}

		private void EscapeCommand_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e) {
			Close();
		}
	}
}
