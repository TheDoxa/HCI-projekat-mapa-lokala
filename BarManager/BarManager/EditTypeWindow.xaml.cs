using BarManager.Model;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media.Imaging;

namespace BarManager {
	/// <summary>
	/// Interaction logic for EditTypeWindow.xaml
	/// </summary>
	public partial class EditTypeWindow : Window, INotifyPropertyChanged {
		private BarType type = null;

		public EditTypeWindow() {
			InitializeComponent();
			DataContext = this;

			Util.Util.loadTypes();
		}

		public EditTypeWindow(BarType type) : this() {
			this.type = type;
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string name) {
			if(PropertyChanged != null) {
				PropertyChanged(this, new PropertyChangedEventArgs(name));
			}
		}

		public int id {
			get {
				return this.type.Id;
			}

			set {
				if(value != this.type.Id) {
					this.type.Id = value;
					OnPropertyChanged("id");
				}
			}
		}

		public string name {
			get {
				return this.type.Name;
			}

			set {
				if(value != this.type.Name) {
					this.type.Name = value;
					if(string.IsNullOrEmpty(value))
						update.IsEnabled = false;
					else
						update.IsEnabled = true;
					OnPropertyChanged("name");
				}
			}
		}

		public string description {
			get {
				return this.type.Description;
			}

			set {
				if(value != this.type.Description) {
					this.type.Description = value;
					OnPropertyChanged("description");
				}
			}
		}

		public string iconPath {
			get {
				return this.type.IconPath;
			}

			set {
				if(value != this.type.IconPath) {
					this.type.IconPath = value;
					OnPropertyChanged("iconPath");
				}
			}
		}

		private void Browse_Click(object sender, RoutedEventArgs e) {
			OpenFileDialog op = new OpenFileDialog();
			op.Title = "Select a picture";
			op.Filter = "Images|*.jpg;*.jpeg;*.png";
			if(op.ShowDialog() == true) {
				iconPath = op.FileName;
				icon.Source = new BitmapImage(new Uri(iconPath));

				if(string.IsNullOrEmpty(iconPath))
					update.IsEnabled = false;
				else
					update.IsEnabled = true;
			} else {
				iconPath = "";
				icon.Source = null;
				update.IsEnabled = false;
			}
		}

		private void Update_Click(object sender, RoutedEventArgs e) {
			Util.Util.updateType(this.type);

			Close();
		}

		private void Cancel_Click(object sender, RoutedEventArgs e) {
			Close();
		}
	}
}
