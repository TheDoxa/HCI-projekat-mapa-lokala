using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BarManager {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
		public MainWindow() {
			InitializeComponent();
            Util.Util.loadTypes();
            Util.Util.loadLabels();
            Util.Util.loadBars();
        }

		private void AddBarItem_Click(object sender, RoutedEventArgs e) {
            BarWindow barWindow = new BarWindow(false, null);
            barWindow.Show();
		}

		private void AddTypeItem_Click(object sender, RoutedEventArgs e) {
			NewType newType = new NewType();
			newType.Show();
		}

		private void AddLabelItem_Click(object sender, RoutedEventArgs e) {
			NewLabel newLabel = new NewLabel();
			newLabel.Show();
		}
	}
}
