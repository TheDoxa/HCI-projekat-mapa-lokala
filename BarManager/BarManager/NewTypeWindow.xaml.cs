using BarManager.Model;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace BarManager {
	/// <summary>
	/// Interaction logic for NewType.xaml
	/// </summary>
	public partial class NewTypeWindow : Window, INotifyPropertyChanged {

		private int _id = 1;
		private string _name, _description, _iconPath;

		private bool fromAllWindow;

		public NewTypeWindow(bool fromAllWindow) {
			InitializeComponent();
			DataContext = this;

			this.fromAllWindow = fromAllWindow;
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string name) {
			if(PropertyChanged != null) {
				PropertyChanged(this, new PropertyChangedEventArgs(name));
			}
		}

		public int id {
			get {
				return _id;
			}

			set {
				if(value != _id) {
					_id = value;
					OnPropertyChanged("id");
				}
			}
		}

		public string name {
			get {
				return _name;
			}

			set {
				if(value != _name) {
					_name = value;
					if(idTextBox.Text.Length == 0 || string.IsNullOrEmpty(value) || string.IsNullOrEmpty(iconPath))
						add.IsEnabled = false;
					else
						add.IsEnabled = true;

					if(string.IsNullOrEmpty(value))
						nameLabelError.Content = "Name is required";
					else
						nameLabelError.Content = "";
					OnPropertyChanged("name");
				}
			}
		}

		public string description {
			get {
				return _description;
			}

			set {
				if(value != _description) {
					_description = value;
					OnPropertyChanged("description");
				}
			}
		}

		public string iconPath {
			get {
				return _iconPath;
			}

			set {
				if(value != _iconPath) {
					_iconPath = value;
					if(idTextBox.Text.Length == 0 || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(value))
						add.IsEnabled = false;
					else
						add.IsEnabled = true;
					OnPropertyChanged("iconPath");
				}
			}
		}

		private void Add_Click(object sender, RoutedEventArgs e) {
			BarType type = new BarType(_id, _name, _description, _iconPath);
			bool res = Util.Util.addType(type);

			if(!res) {
				System.Windows.Forms.MessageBox.Show("Type with id " + type.Id + " already exists!", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
				return;
			}

			if(fromAllWindow)
				AllBarsWindow.AllTypes.Add(type);

			Close();
		}

		private void Cancel_Click(object sender, RoutedEventArgs e) {
			Close();
		}


		private void AutoID_Click(object sender, RoutedEventArgs e) {
			int nextID = 1;
			while(true) {
				bool exists = false;
				foreach(BarType type in Util.Util.BarTypes)
					if(type.Id == nextID) {
						exists = true;
						break;
					}

				if(!exists)
					break;

				nextID++;
			}

			idTextBox.Text = "" + nextID;
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

		private void Browse_Click(object sender, RoutedEventArgs e) {
			OpenFileDialog op = new OpenFileDialog();
			op.Title = "Select a picture";
			op.Filter = "Images|*.jpg;*.jpeg;*.png";
			if(op.ShowDialog() == true) {
				iconPath = op.FileName;
				icon.Source = new BitmapImage(new Uri(iconPath));
				iconErrorLabel.Content = "";
			} else {
				iconPath = "";
				icon.Source = null;
				iconErrorLabel.Content = "Image is required";
			}
		}

		private void IdTextBox_TextChanged(object sender, TextChangedEventArgs e) {
			idTextBox.Text = Regex.Replace(idTextBox.Text, "[^0-9]+", "");
			if(idTextBox.Text.Length == 0 || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(iconPath))
				add.IsEnabled = false;
			else
				add.IsEnabled = true;

			idTextBox.CaretIndex = idTextBox.Text.Length;
		}
	}
}
