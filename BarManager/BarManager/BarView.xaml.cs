using BarManager.Model;
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

namespace BarManager
{
    /// <summary>
    /// Interaction logic for BarView.xaml
    /// </summary>
    public partial class BarView : UserControl
    {
        private Bar bar;

        public BarView(Bar bar)
        {
            InitializeComponent();

            this.bar = bar;
            Naziv.Content = bar.Name;
            Opis.Content = bar.Description;
        }

        public Bar GetBar()
        {
            return this.bar;
        }
    }
}
