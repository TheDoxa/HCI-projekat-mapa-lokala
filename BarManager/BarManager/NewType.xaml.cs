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
	public partial class NewType : Window, INotifyPropertyChanged {

		private int _id;
		private string _name, _description, _iconPath;

		public NewType() {
			InitializeComponent();
			DataContext = this;

			Util.Util.loadTypes();
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

		private void Add_Click(object sender, RoutedEventArgs e) {
			BarType type = new BarType(_id, _name, _description, _iconPath);
			bool res = Util.Util.addType(type);

			if(!res) {
				System.Windows.Forms.MessageBox.Show("Tip sa tim identifikatorom već postoji!", "Greška", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
				return;
			}

			Close();
		}

		private void Cancel_Click(object sender, RoutedEventArgs e) {
			Close();
		}
		

		private void AutoID_Click(object sender, RoutedEventArgs e) {
			int nextID = 1;
			while(true) {
				bool exists = false;
				foreach(BarType type in Util.Util.barTypes)
					if(type.id == nextID) {
						exists = true;
						break;
					}

				if(!exists)
					break;

				nextID++;
			}

			idTextBox.Text = "" + nextID;
		}

		private void NameTextBox_TextChanged(object sender, TextChangedEventArgs e) {
			if(idTextBox.Text.Length == 0 || string.IsNullOrEmpty(_name) || string.IsNullOrEmpty(_description) || string.IsNullOrEmpty(_iconPath))
				add.IsEnabled = false;
			else
				add.IsEnabled = true;
		}

		private void DescriptionTextBox_TextChanged(object sender, TextChangedEventArgs e) {
			if(idTextBox.Text.Length == 0 || string.IsNullOrEmpty(_name) || string.IsNullOrEmpty(_description) || string.IsNullOrEmpty(_iconPath))
				add.IsEnabled = false;
			else
				add.IsEnabled = true;
		}

		private void Browse_Click(object sender, RoutedEventArgs e) {
			OpenFileDialog op = new OpenFileDialog();
			op.Title = "Select a picture";
			op.Filter = "Images|*.jpg;*.jpeg;*.png";
			if(op.ShowDialog() == true) {
				_iconPath = op.FileName;
				icon.Source = new BitmapImage(new Uri(_iconPath));

				if(idTextBox.Text.Length == 0 || string.IsNullOrEmpty(_name) || string.IsNullOrEmpty(_description) || string.IsNullOrEmpty(_iconPath))
					add.IsEnabled = false;
				else
					add.IsEnabled = true;
			} else {
				_iconPath = op.FileName;
				icon.Source = null;

				add.IsEnabled = false;
			}
		}

		private void IdTextBox_TextChanged(object sender, TextChangedEventArgs e) {
			idTextBox.Text = Regex.Replace(idTextBox.Text, "[^0-9]+", "");
			if(idTextBox.Text.Length > 0) {
				if(string.IsNullOrEmpty(_name) || string.IsNullOrEmpty(_description) || string.IsNullOrEmpty(_iconPath))
					add.IsEnabled = false;
				else
					add.IsEnabled = true;

				int val = Int32.Parse(idTextBox.Text);
				if(val < 1)
					val = 1;
				else if(val > 9999)
					val = 9999;

				idTextBox.Text = "" + val;

				_id = val;
			} else {
				add.IsEnabled = false;
			}
			idTextBox.CaretIndex = idTextBox.Text.Length;
		}
	}
}
