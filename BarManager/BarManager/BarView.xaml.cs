using BarManager.Model;
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
            BarName.Text = bar.Name;
            if (File.Exists(bar.Icon))
                BarImage.Fill = new ImageBrush(new BitmapImage(new Uri(bar.Icon)));
            if (bar.Labels.Count > 0)
                BarLabel.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString(bar.Labels[0].Color));
        }

        public Bar GetBar()
        {
            return this.bar;
        }
    }
}
