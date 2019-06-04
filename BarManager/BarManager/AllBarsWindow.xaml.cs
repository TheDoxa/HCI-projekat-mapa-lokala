using BarManager.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
		public static event EventHandler<PropertyChangedEventArgs> StaticPropertyChanged;

		public static AllBarsWindow instance;

		protected virtual void OnPropertyChanged(string name) {
			if(PropertyChanged != null) {
				PropertyChanged(this, new PropertyChangedEventArgs(name));
			}
		}

		public static void OnPropertyChangedStatic([CallerMemberName] string propertyName = null) {
			StaticPropertyChanged?.Invoke(AllBarsWindow.instance, new PropertyChangedEventArgs(propertyName));
		}

		private static ObservableCollection<Bar> _allBars = new ObservableCollection<Bar>();
		public static ObservableCollection<Bar> AllBars {
			get {
				return _allBars;
			}
			set {
				if(value != _allBars) {
					_allBars = value;
					OnPropertyChangedStatic();
				}
			}
		}
		private static ObservableCollection<BarLabel> _alllabels = new ObservableCollection<BarLabel>();
		public static ObservableCollection<BarLabel> AllLabels {
			get {
				return _alllabels;
			}
			set {
				_alllabels = value;
				OnPropertyChangedStatic();
			}
		}
		private static ObservableCollection<BarType> _allTypes = new ObservableCollection<BarType>();
		public static ObservableCollection<BarType> AllTypes {
			get {
				return _allTypes;
			}
			set {
				_allTypes = value;
				OnPropertyChangedStatic();
			}
		}

		private static ObservableCollection<Bar> pomocna = new ObservableCollection<Bar>();

		public static ObservableCollection<Bar> Pomocna {
			get {
				return pomocna;
			}
			set {
				pomocna = value;
				OnPropertyChangedStatic();
			}
		}

		public AllBarsWindow() {
			InitializeComponent();
			this.DataContext = this;
			InitData();
			AllBarsWindow.instance = this;
		}

		private void InitData()
        {
			AllLabels.Clear();
			AllTypes.Clear();
			AllBars.Clear();

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

        private void Search_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
                return;

            string searchText = searchInput.Text.ToLower();
            SearchFun(searchText);
        }

            private void Search_Click(object sender, RoutedEventArgs e)
        {
            string searchText = searchInput.Text.ToLower();
            SearchFun(searchText);
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

		private void EditCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) {
			e.CanExecute = true;
		}

		private void EditCommand_Executed(object sender, ExecutedRoutedEventArgs e) {
			if(typesTable.SelectedItem != null) {
				BarType selectedType = (BarType)typesTable.SelectedItem;

				EditTypeWindow editTypeWindow = new EditTypeWindow(selectedType);
				editTypeWindow.Show();
			} else if(labelsTable.SelectedItem != null) {
				BarLabel selectedlabel = (BarLabel)labelsTable.SelectedItem;

				EditLabelWindow editLabelWindow = new EditLabelWindow(selectedlabel);
				editLabelWindow.Show();
			} else if(barTble.SelectedItem != null) {
				Bar selectedBar = (Bar)barTble.SelectedItem;
				BarWindow editBarWindow = new BarWindow(true, selectedBar, this);
				editBarWindow.Show();
			}

			barTble.UnselectAll();
			typesTable.UnselectAll();
			labelsTable.UnselectAll();
		}

		private void DeleteCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) {
			e.CanExecute = true;
		}

		private void DeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e) {
			if(typesTable.SelectedItem != null) {
				BarType selectedType = (BarType)typesTable.SelectedItem;

				bool res = Util.Util.removeType(selectedType);

				if(res)
					for(int i = 0; i < AllBarsWindow.AllTypes.Count; i++) {
						if(AllBarsWindow.AllTypes[i].Id == selectedType.Id) {
							AllBarsWindow.AllTypes.RemoveAt(i);

							break;
						}
					}
			} else if(labelsTable.SelectedItem != null) {
				BarLabel selectedlabel = (BarLabel)labelsTable.SelectedItem;

				bool res = Util.Util.removeLabel(selectedlabel);

				if(res)
					for(int i = 0; i < AllBarsWindow.AllLabels.Count; i++) {
						if(AllBarsWindow.AllLabels[i].Id == selectedlabel.Id) {
							AllBarsWindow.AllLabels.RemoveAt(i);

							break;
						}
					}
			} else if(barTble.SelectedItem != null) {
				Bar selectedBar = (Bar)barTble.SelectedItem;

				Util.Util.removeBar(selectedBar);

				for(int i = 0; i < AllBarsWindow.AllBars.Count; i++) {
					if(AllBarsWindow.AllBars[i].Id == selectedBar.Id) {
						AllBarsWindow.AllBars.RemoveAt(i);

						break;
					}
				}
			}

			barTble.UnselectAll();
			typesTable.UnselectAll();
			labelsTable.UnselectAll();
		}

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
			if(barTble.Visibility == Visibility.Visible) {
				BarWindow winD = new BarWindow(false, null, this);
				winD.Show();
			} else if(typesTable.Visibility == Visibility.Visible) {
				NewTypeWindow typeD = new NewTypeWindow(true);
				typeD.Show();
			} else if(labelsTable.Visibility == Visibility.Visible) {
				NewLabelWindow labelD = new NewLabelWindow(true);
				labelD.Show();
			}
		}

        private void SearchFun(string searchText)
        {
            Pomocna.Clear();
            string[] tokens = searchText.Split(' ');
            foreach (Bar b in Util.Util.Bars)
            {
                if (b.Name.ToLower().Contains(searchText))
                {
					Pomocna.Add(b);
                }
                else if (b.Description.ToLower().Contains(searchText))
                {
					Pomocna.Add(b);
                }
                else if (b.Type.Name.ToLower().Contains(searchText))
                {
					Pomocna.Add(b);
                }
                else if (b.AlcStatus.ToLower().Contains(searchText))
                {
					Pomocna.Add(b);
                }
                else
                {
                    foreach (string t in tokens)
                    {
                        if (b.Name.ToLower().Contains(t))
                        {
							Pomocna.Add(b);
                            break;
                        }
                        else if (b.Description.ToLower().Contains(t))
                        {
							Pomocna.Add(b);
                            break;
                        }
                        else if (b.Type.Name.ToLower().Contains(t))
                        {
							Pomocna.Add(b);
                            break;
                        }
                        else if (b.AlcStatus.ToLower().Contains(searchText))
                        {
							Pomocna.Add(b);
                        }
                        else
                        {
                            foreach (BarLabel bl in b.Labels)
                            {
                                if (bl.Description.ToLower().Contains(t))
                                {
									Pomocna.Add(b);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            barTble.ItemsSource = Pomocna;
        }

		private void EscapeCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) {
			e.CanExecute = true;
		}

		private void EscapeCommand_Executed(object sender, ExecutedRoutedEventArgs e) {
			Close();
		}

		private void HelpCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) {
			e.CanExecute = true;
		}

		private void HelpCommand_Executed(object sender, ExecutedRoutedEventArgs e) {
			HelpProvider.ShowHelp(this);
		}
	}
}
