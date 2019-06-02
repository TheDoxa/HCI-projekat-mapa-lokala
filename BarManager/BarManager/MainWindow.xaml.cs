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

namespace BarManager {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
    {
        private Dictionary<long, Bar> BarsFromMap1;
        private Dictionary<long, Bar> BarsFromMap2;
        private Dictionary<long, Bar> BarsFromMap3;
        private Dictionary<long, Bar> BarsFromMap4;

        public MainWindow() {
			InitializeComponent();

            Util.Util.loadTypes();
            Util.Util.loadLabels();
            Util.Util.loadBars();
            
            foreach (Bar b in Util.Util.bars)
            {
                BarList.Items.Add(b);
            }

            ImageBrush ib1 = new ImageBrush();
            ib1.ImageSource = new BitmapImage(new Uri(@"images\map1.png", UriKind.Relative));
            BarCanvas1.Background = ib1;

            ImageBrush ib2 = new ImageBrush();
            ib2.ImageSource = new BitmapImage(new Uri(@"images\map2.png", UriKind.Relative));
            BarCanvas2.Background = ib2;

            ImageBrush ib3 = new ImageBrush();
            ib3.ImageSource = new BitmapImage(new Uri(@"images\map3.png", UriKind.Relative));
            BarCanvas3.Background = ib3;

            ImageBrush ib4 = new ImageBrush();
            ib4.ImageSource = new BitmapImage(new Uri(@"images\map4.png", UriKind.Relative));
            BarCanvas4.Background = ib4;

            BarsFromMap1 = new Dictionary<long, Bar>();
            BarsFromMap2 = new Dictionary<long, Bar>();
            BarsFromMap3 = new Dictionary<long, Bar>();
            BarsFromMap4 = new Dictionary<long, Bar>();
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

        #region Events

        private void BarList_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var obj = new DataObject("Bar", (BarList.SelectedItem as Bar));
                DragDrop.DoDragDrop(this, obj, DragDropEffects.Copy);
            }
        }

        private void Bar_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                e.GetPosition(null);

                var obj = new DataObject("BarView", sender as BarView);
                DragDrop.DoDragDrop(this, obj, DragDropEffects.Copy);
            }
        }

        private void Canvas_Drop(object sender, DragEventArgs e)
        {
            Canvas barCanvas = sender as Canvas;
            Dictionary<long, Bar> barsFromMapDictionary = new Dictionary<long, Bar>();

            switch (barCanvas.Name)
            {
                case "BarCanvas1":
                    barsFromMapDictionary = BarsFromMap1;
                    break;

                case "BarCanvas2":
                    barsFromMapDictionary = BarsFromMap2;
                    break;

                case "BarCanvas3":
                    barsFromMapDictionary = BarsFromMap3;
                    break;

                case "BarCanvas4":
                    barsFromMapDictionary = BarsFromMap4;
                    break;
            }

            var droppedBar = e.Data.GetData("Bar") as Bar;
            var droppedBarView = e.Data.GetData("BarView") as BarView;
            var mousePosition = e.GetPosition(barCanvas);

            if (droppedBar != null)
            {
                BarView bw = new BarView(droppedBar);
                var newBarPosition = new Point(mousePosition.X - bw.Width / 2, mousePosition.Y - bw.Height / 2);

                bw.SetValue(Canvas.LeftProperty, newBarPosition.X);
                bw.SetValue(Canvas.TopProperty, newBarPosition.Y);
                bw.MouseMove += Bar_MouseMove;

                barCanvas.Children.Add(bw);

                if (!CheckIfBarViewsOverlap(bw, barCanvas, newBarPosition))
                {
                    barsFromMapDictionary.Add(droppedBar.Id, droppedBar);
                    BarList.Items.Remove(droppedBar);
                }
                else
                {
                    barCanvas.Children.Remove(bw);
                }
            }
            else if (droppedBarView != null)
            {
                var newBarPosition = new Point(mousePosition.X - droppedBarView.Width / 2, mousePosition.Y - droppedBarView.Height / 2);
                if (!CheckIfBarViewsOverlap(droppedBarView, barCanvas, newBarPosition))
                {
                    Canvas parentCanvas = (Canvas)droppedBarView.Parent;
                    if (!barCanvas.Equals(parentCanvas))
                    {
                        if (droppedBar == null) droppedBar = droppedBarView.GetBar();
                        RemoveBarFromCanvas(parentCanvas.Name, droppedBar.Id);

                        parentCanvas.Children.Remove(droppedBarView);
                        barCanvas.Children.Add(droppedBarView);
                        barsFromMapDictionary.Add(droppedBar.Id, droppedBar);
                    }

                    droppedBarView.SetValue(Canvas.LeftProperty, newBarPosition.X);
                    droppedBarView.SetValue(Canvas.TopProperty, newBarPosition.Y);
                }
            }
        }

        private void BarList_Drop(object sender, DragEventArgs e)
        {
            var droppedBar = e.Data.GetData("Bar") as Bar;
            var droppedBarView = e.Data.GetData("BarView") as BarView;
            if (droppedBar == null) droppedBar = droppedBarView.GetBar();

            if (BarList.SelectedItem == null || (BarList.SelectedItem as Bar).Id != droppedBar.Id)
            {
                BarList.Items.Add(droppedBar);

                Canvas parentCanvas = (Canvas)droppedBarView.Parent;
                parentCanvas.Children.Remove(droppedBarView);
                RemoveBarFromCanvas(parentCanvas.Name, droppedBar.Id);
            }
        }

        private void Map_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                ChangeMap((sender as Image));
            }
        }

        private void Map_DragEnter(object sender, DragEventArgs e)
        {
            ChangeMap((sender as Image));
        }

        #endregion

        #region Custom functions

        private bool CheckIfBarViewsOverlap(BarView droppedBarView, Canvas canvas, Point barPosition)
        {
            bool overlaps = false;
            Rect droppedBarRect = new Rect(barPosition.X, barPosition.Y, droppedBarView.Width, droppedBarView.Height);

            foreach (BarView bw in canvas.Children)
            {
                if (bw.GetBar().Id != droppedBarView.GetBar().Id)
                {
                    var bwPos = bw.TransformToAncestor(canvas).Transform(new Point(0, 0));
                    Rect bwRect = new Rect(bwPos.X, bwPos.Y, bw.Width, bw.Height);
                    overlaps = bwRect.IntersectsWith(droppedBarRect);
                    if (overlaps) break;
                }
            }

            return overlaps;
        }

        private void RemoveBarFromCanvas(string canvasName, long barId)
        {
            switch (canvasName)
            {
                case "BarCanvas1":
                    BarsFromMap1.Remove(barId);
                    break;

                case "BarCanvas2":
                    BarsFromMap2.Remove(barId);
                    break;

                case "BarCanvas3":
                    BarsFromMap3.Remove(barId);
                    break;

                case "BarCanvas4":
                    BarsFromMap4.Remove(barId);
                    break;
            }
        }

        private void ChangeMap(Image map)
        {
            switch (map.Name)
            {
                case "Map1":
                    BarCanvas1.Visibility = Visibility.Visible;
                    BarCanvas2.Visibility = Visibility.Collapsed;
                    BarCanvas3.Visibility = Visibility.Collapsed;
                    BarCanvas4.Visibility = Visibility.Collapsed;
                    break;

                case "Map2":
                    BarCanvas1.Visibility = Visibility.Collapsed;
                    BarCanvas2.Visibility = Visibility.Visible;
                    BarCanvas3.Visibility = Visibility.Collapsed;
                    BarCanvas4.Visibility = Visibility.Collapsed;
                    break;

                case "Map3":
                    BarCanvas1.Visibility = Visibility.Collapsed;
                    BarCanvas2.Visibility = Visibility.Collapsed;
                    BarCanvas3.Visibility = Visibility.Visible;
                    BarCanvas4.Visibility = Visibility.Collapsed;
                    break;

                case "Map4":
                    BarCanvas1.Visibility = Visibility.Collapsed;
                    BarCanvas2.Visibility = Visibility.Collapsed;
                    BarCanvas3.Visibility = Visibility.Collapsed;
                    BarCanvas4.Visibility = Visibility.Visible;
                    break;
            }
        }

        #endregion
    }
}
