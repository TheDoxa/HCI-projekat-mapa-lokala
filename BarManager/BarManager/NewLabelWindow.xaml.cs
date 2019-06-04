using BarManager.Model;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows;

namespace BarManager {
	/// <summary>
	/// Interaction logic for NewLabel.xaml
	/// </summary>
	public partial class NewLabelWindow : Window, INotifyPropertyChanged {

		private int _id = 1;
		private string _description, _color = "Blue";

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

		public string color {
			get {
				return _color;
			}

			set {
				if(value != _color) {
					_color = value;
					OnPropertyChanged("color");
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
					if(idOfLabelTextBox.Text.Length == 0 || string.IsNullOrEmpty(value))
						addLabel.IsEnabled = false;
					else
						addLabel.IsEnabled = true;

					if(string.IsNullOrEmpty(value))
						descriptionErrorLabel.Content = "Description is required";
					else
						descriptionErrorLabel.Content = "";
					OnPropertyChanged("description");
				}
			}
		}

		private bool fromAllWindow;

		public NewLabelWindow(bool fromAllWindow) {
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

		private void AutoIDOfLabel_Click(object sender, RoutedEventArgs e) {
			int nextID = 1;
			while(true) {
				bool exists = false;
				foreach(BarLabel label in Util.Util.BarLabels)
					if(label.Id == nextID) {
						exists = true;
						break;
					}

				if(!exists)
					break;

				nextID++;
			}

			idOfLabelTextBox.Text = "" + nextID;
		}

		private void IdOfLabelTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e) {
			idOfLabelTextBox.Text = Regex.Replace(idOfLabelTextBox.Text, "[^0-9]+", "");
			if(idOfLabelTextBox.Text.Length == 0 || string.IsNullOrEmpty(description))
				addLabel.IsEnabled = false;
			else
				addLabel.IsEnabled = true;

			idOfLabelTextBox.CaretIndex = idOfLabelTextBox.Text.Length;
		}

		private void AddLabel_Click(object sender, RoutedEventArgs e) {
			BarLabel label = new BarLabel(_id, colorOfLabel.Text, _description);
			bool res = Util.Util.addLabel(label);

			if(!res) {
				System.Windows.Forms.MessageBox.Show("Label with id " + label.Id + " already exists!", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
				return;
			}

			if(fromAllWindow) {
				AllBarsWindow.AllLabels.Add(label);
			}

			Close();
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

		private void CancelLabel_Click(object sender, RoutedEventArgs e) {
			Close();
		}
	}
}
