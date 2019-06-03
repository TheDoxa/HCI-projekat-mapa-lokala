using BarManager.Model;
using System.ComponentModel;
using System.Windows;

namespace BarManager {
	/// <summary>
	/// Interaction logic for EditLabelWindow.xaml
	/// </summary>
	public partial class EditLabelWindow : Window, INotifyPropertyChanged {
		private BarLabel label = null;

		public EditLabelWindow() {
			InitializeComponent();
			DataContext = this;

			Util.Util.loadLabels();
		}

		public EditLabelWindow(BarLabel label) : this() {
			this.label = label;
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string name) {
			if(PropertyChanged != null) {
				PropertyChanged(this, new PropertyChangedEventArgs(name));
			}
		}

		public int id {
			get {
				return this.label.Id;
			}

			set {
				if(value != this.label.Id) {
					this.label.Id = value;
				}
			}
		}

		public string color {
			get {
				return this.label.Color;
			}

			set {
				if(value != this.label.Color) {
					this.label.Color = value;
					OnPropertyChanged("color");
				}
			}
		}

		public string description {
			get {
				return this.label.Description;
			}

			set {
				if(value != this.label.Description) {
					this.label.Description = value;
					OnPropertyChanged("description");
				}
			}
		}

		private void UpdateLabel_Click(object sender, RoutedEventArgs e) {
			Util.Util.updateLabel(label);

			Close();
		}

		private void CancelLabel_Click(object sender, RoutedEventArgs e) {
			Close();
		}
	}
}
