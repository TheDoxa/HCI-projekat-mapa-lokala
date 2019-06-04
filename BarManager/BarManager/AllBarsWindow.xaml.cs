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
            

			foreach (BarLabel b in Util.Util.BarLabels)
            {
                AllLabels.Add(b);
            }
            foreach (BarType b in Util.Util.BarTypes)
            {
                AllTypes.Add(b);
            }

            foreach (Bar b in Util.Util.Bars)
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

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            ObservableCollection<Bar> pomocna = new ObservableCollection<Bar>();
            string searchText = searchInput.Text.ToLower();
            string[] tokens = searchText.Split(' ');
            foreach (Bar b in Util.Util.Bars)
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

			typesTable.UnselectAll();
			labelsTable.UnselectAll();
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            barTble.Visibility = Visibility.Collapsed;
            typesTable.Visibility = Visibility.Visible;
            labelsTable.Visibility = Visibility.Collapsed;
            FilterBtn.Visibility = Visibility.Collapsed;
            searchBtn.Visibility = Visibility.Collapsed;
            searchInput.Visibility = Visibility.Collapsed;

			barTble.UnselectAll();
			labelsTable.UnselectAll();
		}

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            barTble.Visibility = Visibility.Collapsed;
            typesTable.Visibility = Visibility.Collapsed;
            labelsTable.Visibility = Visibility.Visible;
            FilterBtn.Visibility = Visibility.Collapsed;
            searchBtn.Visibility = Visibility.Collapsed;
            searchInput.Visibility = Visibility.Collapsed;

			barTble.UnselectAll();
			typesTable.UnselectAll();
		}

		private void EditBtn_Click(object sender, RoutedEventArgs e) {
			if(typesTable.SelectedItem != null) {
				BarType selectedType = (BarType)typesTable.SelectedItem;

				EditTypeWindow editTypeWindow = new EditTypeWindow(selectedType, this);
				editTypeWindow.Show();
			} else if(labelsTable.SelectedItem != null) {
				BarLabel selectedlabel = (BarLabel)labelsTable.SelectedItem;

				EditLabelWindow editLabelWindow = new EditLabelWindow(selectedlabel, this);
				editLabelWindow.Show();
			} else if (barTble.SelectedItem != null)
            {
                Bar selectedBar = (Bar)barTble.SelectedItem;
                BarWindow editBarWindow = new BarWindow(true, selectedBar);
                editBarWindow.Show();
            }
        }

		private void DeleteBtn_Click(object sender, RoutedEventArgs e) {
			if(typesTable.SelectedItem != null) {
				BarType selectedType = (BarType)typesTable.SelectedItem;

				Util.Util.removeType(selectedType);
			} else if(labelsTable.SelectedItem != null) {
				BarLabel selectedlabel = (BarLabel)labelsTable.SelectedItem;

				Util.Util.removeLabel(selectedlabel);
			} else if (barTble.SelectedItem != null) {
                Bar selectedBar = (Bar)barTble.SelectedItem;

                Util.Util.removeBar(selectedBar);
            }
        }

		private void EditCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) {
			e.CanExecute = typesTable.SelectedItem != null || labelsTable.SelectedItem != null;
		}

		private void EditCommand_Executed(object sender, ExecutedRoutedEventArgs e) {
			if(typesTable.SelectedItem != null) {
				BarType selectedType = (BarType)typesTable.SelectedItem;

				EditTypeWindow editTypeWindow = new EditTypeWindow(selectedType, this);
				editTypeWindow.Show();
			} else if(labelsTable.SelectedItem != null) {
				BarLabel selectedlabel = (BarLabel)labelsTable.SelectedItem;

				EditLabelWindow editLabelWindow = new EditLabelWindow(selectedlabel, this);
				editLabelWindow.Show();
			}
		}

		private void DeleteCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) {
			e.CanExecute = typesTable.SelectedItem != null || labelsTable.SelectedItem != null;
		}

		private void DeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e) {
			if(typesTable.SelectedItem != null) {
				BarType selectedType = (BarType)typesTable.SelectedItem;

				Util.Util.removeType(selectedType);
			} else if(labelsTable.SelectedItem != null) {
				BarLabel selectedlabel = (BarLabel)labelsTable.SelectedItem;

				Util.Util.removeLabel(selectedlabel);
			}
		}

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if(barTble.Visibility == Visibility.Visible)
            {
                BarWindow winD = new BarWindow(false, null);
                winD.Show();
            }else if (typesTable.Visibility == Visibility.Visible)
            {
                NewTypeWindow typeD = new NewTypeWindow();
                typeD.Show();
            }else if (labelsTable.Visibility == Visibility.Visible)
            {
                NewLabelWindow labelD = new NewLabelWindow();
                labelD.Show();
            }
        }
    }
}
