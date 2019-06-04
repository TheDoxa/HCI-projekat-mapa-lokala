using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for HelpWindow.xaml
    /// </summary>
    public partial class HelpWindow : Window
    {
        private JavaScriptControlHelper ch;
        private Window _parentWindow;
        public Window ParentWindow
        {
            get { return _parentWindow; }
            set { _parentWindow = value; }
        }

        public HelpWindow(Window parent)
        {

            InitializeComponent();
            ParentWindow = parent;
            string currentDirectory = Directory.GetCurrentDirectory();
            string key = "";

            if (ParentWindow is MainWindow)
            {
                ch = new JavaScriptControlHelper((MainWindow)ParentWindow);
                key = "mainWindow";
            }
			else if(ParentWindow is BarWindow) {
				ch = new JavaScriptControlHelper((BarWindow)ParentWindow);
				key = "newBarWindow";
			} else if(ParentWindow is NewTypeWindow) {
				ch = new JavaScriptControlHelper((NewTypeWindow)ParentWindow);
				key = "newTypeWindow";
			} else if(ParentWindow is NewLabelWindow) {
				ch = new JavaScriptControlHelper((NewLabelWindow)ParentWindow);
				key = "newLabelWindow";
			} else if(ParentWindow is AllBarsWindow) {
				ch = new JavaScriptControlHelper((AllBarsWindow)ParentWindow);
				key = "viewWindow";
			} else
            {
				key = "newTypeWindow";
            }

            string path = String.Format(@"{0}/Help/{1}.htm", currentDirectory, key);
            if (!File.Exists(path))
            {
                key = "error";
            }

            Uri uri = new Uri(String.Format(@"file:{0}/Help/{1}.htm", currentDirectory, key));

            wbHelp.Source = uri;
            wbHelp.ObjectForScripting = ch;
            wbHelp.Navigate(uri);
        }
    }
}
