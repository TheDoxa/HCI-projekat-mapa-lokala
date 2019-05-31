using BarManager.Model;
using BarManager.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media;

namespace BarManager {
	/// <summary>
	/// Interaction logic for NewLabel.xaml
	/// </summary>
	public partial class NewLabel : Window, INotifyPropertyChanged {

		private int _id;
		private string _description, _color;

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
					OnPropertyChanged("description");
				}
			}
		}

		public NewLabel() {
			InitializeComponent();
			DataContext = this;

			Util.Util.loadLabels();
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
				foreach(BarLabel label in Util.Util.barLabels)
					if(label.id == nextID) {
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
			if(idOfLabelTextBox.Text.Length > 0) {
				if(string.IsNullOrEmpty(_description))
					addLabel.IsEnabled = false;
				else
					addLabel.IsEnabled = true;

				int val = Int32.Parse(idOfLabelTextBox.Text);
				if(val < 1)
					val = 1;
				else if(val > 9999)
					val = 9999;

				idOfLabelTextBox.Text = "" + val;

				_id = val;
			} else {
				addLabel.IsEnabled = false;
			}
			idOfLabelTextBox.CaretIndex = idOfLabelTextBox.Text.Length;
		}

		private void DescriptionOfLabelTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e) {
			if(idOfLabelTextBox.Text.Length == 0 || string.IsNullOrEmpty(_description))
				addLabel.IsEnabled = false;
			else
				addLabel.IsEnabled = true;
		}

		private void AddLabel_Click(object sender, RoutedEventArgs e) {
			BarLabel label = new BarLabel(_id, colorOfLabel.Text, _description);
			bool res = Util.Util.addLabel(label);

			if(!res) {
				System.Windows.Forms.MessageBox.Show("Labela sa tim identifikatorom već postoji!", "Greška", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
				return;
			}

			Close();
		}

		private void CancelLabel_Click(object sender, RoutedEventArgs e) {
			Close();
		}
	}
}
