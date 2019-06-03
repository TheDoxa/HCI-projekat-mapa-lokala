using BarManager.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BarManager {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
		public Dictionary<long, Bar> BarsFromMap1;
		public Dictionary<long, Bar> BarsFromMap2;
		public Dictionary<long, Bar> BarsFromMap3;
		public Dictionary<long, Bar> BarsFromMap4;

		public MainWindow() {
			InitializeComponent();

			Util.Util.loadTypes();
			Util.Util.loadLabels();
			Util.Util.loadBars();

			foreach(Bar b in Util.Util.bars) {
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

		#region Events

		private void BarList_MouseMove(object sender, MouseEventArgs e) {
			if(e.LeftButton == MouseButtonState.Pressed) {
				var obj = new DataObject("Bar", (BarList.SelectedItem as Bar));
				DragDrop.DoDragDrop(this, obj, DragDropEffects.Copy);
			}
		}

		private void Bar_MouseMove(object sender, MouseEventArgs e) {
			if(e.LeftButton == MouseButtonState.Pressed) {
				e.GetPosition(null);

				var obj = new DataObject("BarView", sender as BarView);
				DragDrop.DoDragDrop(this, obj, DragDropEffects.Copy);
			}
		}

		private void Canvas_Drop(object sender, DragEventArgs e) {
			Canvas barCanvas = sender as Canvas;
			Dictionary<long, Bar> barsFromMapDictionary = new Dictionary<long, Bar>();

			switch(barCanvas.Name) {
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

			if(droppedBar != null) {
				BarView bw = new BarView(droppedBar);
				var newBarPosition = new Point(mousePosition.X - bw.Width / 2, mousePosition.Y - bw.Height / 2);

				bw.SetValue(Canvas.LeftProperty, newBarPosition.X);
				bw.SetValue(Canvas.TopProperty, newBarPosition.Y);
				bw.MouseMove += Bar_MouseMove;

				barCanvas.Children.Add(bw);

				if(!CheckIfBarViewsOverlap(bw, barCanvas, newBarPosition)) {
					barsFromMapDictionary.Add(droppedBar.Id, droppedBar);
					BarList.Items.Remove(droppedBar);
				} else {
					barCanvas.Children.Remove(bw);
				}
			} else if(droppedBarView != null) {
				var newBarPosition = new Point(mousePosition.X - droppedBarView.Width / 2, mousePosition.Y - droppedBarView.Height / 2);
				if(!CheckIfBarViewsOverlap(droppedBarView, barCanvas, newBarPosition)) {
					Canvas parentCanvas = (Canvas)droppedBarView.Parent;
					if(!barCanvas.Equals(parentCanvas)) {
						if(droppedBar == null)
							droppedBar = droppedBarView.GetBar();
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

		private void BarList_Drop(object sender, DragEventArgs e) {
			var droppedBar = e.Data.GetData("Bar") as Bar;
			var droppedBarView = e.Data.GetData("BarView") as BarView;
			if(droppedBar == null)
				droppedBar = droppedBarView.GetBar();

			if(BarList.SelectedItem == null || (BarList.SelectedItem as Bar).Id != droppedBar.Id) {
				BarList.Items.Add(droppedBar);

				Canvas parentCanvas = (Canvas)droppedBarView.Parent;
				parentCanvas.Children.Remove(droppedBarView);
				RemoveBarFromCanvas(parentCanvas.Name, droppedBar.Id);
			}
		}

		private void Map_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
			if(e.LeftButton == MouseButtonState.Pressed) {
				ChangeMap((sender as Image));
			}
		}

		private void Map_DragEnter(object sender, DragEventArgs e) {
			ChangeMap((sender as Image));
		}

		#endregion

		#region Custom functions

		private bool CheckIfBarViewsOverlap(BarView droppedBarView, Canvas canvas, Point barPosition) {
			bool overlaps = false;
			Rect droppedBarRect = new Rect(barPosition.X, barPosition.Y, droppedBarView.Width, droppedBarView.Height);

			foreach(BarView bw in canvas.Children) {
				if(bw.GetBar().Id != droppedBarView.GetBar().Id) {
					var bwPos = bw.TransformToAncestor(canvas).Transform(new Point(0, 0));
					Rect bwRect = new Rect(bwPos.X, bwPos.Y, bw.Width, bw.Height);
					overlaps = bwRect.IntersectsWith(droppedBarRect);
					if(overlaps)
						break;
				}
			}

			return overlaps;
		}

		public void RemoveBarFromCanvas(string canvasName, long barId) {
			switch(canvasName) {
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

		private void ChangeMap(Image map) {
			switch(map.Name) {
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

		private void FilterBtn_Click(object sender, RoutedEventArgs e) {
			FilterWindow fw = new FilterWindow();
			fw.mainWindow = this;
			fw.Show();
		}



		private void SearchBtn_Click(object sender, RoutedEventArgs e) {
			string searchText = searchInput.Text.ToLower();
			string[] tokens = searchText.Split(' ');

			searchMap1(searchText, tokens);
			searchMap2(searchText, tokens);
			searchMap3(searchText, tokens);
			searchMap4(searchText, tokens);


		}

		private void searchMap1(string searchText, string[] tokens) {
			ObservableCollection<Bar> tempBarsMap1 = new ObservableCollection<Bar>();
			//Map1
			foreach(Bar b in BarsFromMap1.Values) {
				if(b.Name.ToLower().Contains(searchText)) {
					tempBarsMap1.Add(b);
				} else if(b.Description.ToLower().Contains(searchText)) {
					tempBarsMap1.Add(b);
				} else if(b.Type.Name.ToLower().Contains(searchText)) {
					tempBarsMap1.Add(b);
				} else if(b.AlcStatus.ToLower().Contains(searchText)) {
					tempBarsMap1.Add(b);
				} else {
					foreach(string t in tokens) {
						if(b.Name.ToLower().Contains(t)) {
							tempBarsMap1.Add(b);
							break;
						} else if(b.Description.ToLower().Contains(t)) {
							tempBarsMap1.Add(b);
							break;
						} else if(b.Type.Name.ToLower().Contains(t)) {
							tempBarsMap1.Add(b);
							break;
						} else if(b.AlcStatus.ToLower().Contains(searchText)) {
							tempBarsMap1.Add(b);
						} else {
							foreach(BarLabel bl in b.Labels) {
								if(bl.Description.ToLower().Contains(t)) {
									tempBarsMap1.Add(b);
									;
									break;
								}
							}
						}
					}
				}
			}
		}

		private void searchMap2(string searchText, string[] tokens) {
			ObservableCollection<Bar> tempBarsMap1 = new ObservableCollection<Bar>();
			//Map1
			foreach(Bar b in BarsFromMap2.Values) {
				if(b.Name.ToLower().Contains(searchText)) {
					tempBarsMap1.Add(b);
				} else if(b.Description.ToLower().Contains(searchText)) {
					tempBarsMap1.Add(b);
				} else if(b.Type.Name.ToLower().Contains(searchText)) {
					tempBarsMap1.Add(b);
				} else if(b.AlcStatus.ToLower().Contains(searchText)) {
					tempBarsMap1.Add(b);
				} else {
					foreach(string t in tokens) {
						if(b.Name.ToLower().Contains(t)) {
							tempBarsMap1.Add(b);
							break;
						} else if(b.Description.ToLower().Contains(t)) {
							tempBarsMap1.Add(b);
							break;
						} else if(b.Type.Name.ToLower().Contains(t)) {
							tempBarsMap1.Add(b);
							break;
						} else if(b.AlcStatus.ToLower().Contains(searchText)) {
							tempBarsMap1.Add(b);
						} else {
							foreach(BarLabel bl in b.Labels) {
								if(bl.Description.ToLower().Contains(t)) {
									tempBarsMap1.Add(b);
									;
									break;
								}
							}
						}
					}

				}
			}
		}

		private void searchMap3(string searchText, string[] tokens) {
			ObservableCollection<Bar> tempBarsMap1 = new ObservableCollection<Bar>();
			//Map1
			foreach(Bar b in BarsFromMap3.Values) {
				if(b.Name.ToLower().Contains(searchText)) {
					tempBarsMap1.Add(b);
				} else if(b.Description.ToLower().Contains(searchText)) {
					tempBarsMap1.Add(b);
				} else if(b.Type.Name.ToLower().Contains(searchText)) {
					tempBarsMap1.Add(b);
				} else if(b.AlcStatus.ToLower().Contains(searchText)) {
					tempBarsMap1.Add(b);
				} else {
					foreach(string t in tokens) {
						if(b.Name.ToLower().Contains(t)) {
							tempBarsMap1.Add(b);
							break;
						} else if(b.Description.ToLower().Contains(t)) {
							tempBarsMap1.Add(b);
							break;
						} else if(b.Type.Name.ToLower().Contains(t)) {
							tempBarsMap1.Add(b);
							break;
						} else if(b.AlcStatus.ToLower().Contains(searchText)) {
							tempBarsMap1.Add(b);
						} else {
							foreach(BarLabel bl in b.Labels) {
								if(bl.Description.ToLower().Contains(t)) {
									tempBarsMap1.Add(b);
									;
									break;
								}
							}
						}
					}

				}
			}
		}
		private void searchMap4(string searchText, string[] tokens) {
			ObservableCollection<Bar> tempBarsMap1 = new ObservableCollection<Bar>();
			//Map1
			foreach(Bar b in BarsFromMap4.Values) {
				if(b.Name.ToLower().Contains(searchText)) {
					tempBarsMap1.Add(b);
				} else if(b.Description.ToLower().Contains(searchText)) {
					tempBarsMap1.Add(b);
				} else if(b.Type.Name.ToLower().Contains(searchText)) {
					tempBarsMap1.Add(b);
				} else if(b.AlcStatus.ToLower().Contains(searchText)) {
					tempBarsMap1.Add(b);
				} else {
					foreach(string t in tokens) {
						if(b.Name.ToLower().Contains(t)) {
							tempBarsMap1.Add(b);
							break;
						} else if(b.Description.ToLower().Contains(t)) {
							tempBarsMap1.Add(b);
							break;
						} else if(b.Type.Name.ToLower().Contains(t)) {
							tempBarsMap1.Add(b);
							break;
						} else if(b.AlcStatus.ToLower().Contains(searchText)) {
							tempBarsMap1.Add(b);
						} else {
							foreach(BarLabel bl in b.Labels) {
								if(bl.Description.ToLower().Contains(t)) {
									tempBarsMap1.Add(b);
									;
									break;
								}
							}
						}
					}
				}
			}
		}

		private void HelpCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) {
			e.CanExecute = true;
		}

		private void HelpCommand_Executed(object sender, ExecutedRoutedEventArgs e) {

		}

		private void AddBarCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) {
			e.CanExecute = true;
		}

		private void AddBarCommand_Executed(object sender, ExecutedRoutedEventArgs e) {
			if(Util.Util.barTypes.Count > 0) {
				BarWindow barWindow = new BarWindow(false, null);
				barWindow.Show();
			} else
				System.Windows.Forms.MessageBox.Show("You need at least one type of bar to add new bar.", "Warning", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
		}

		private void AddTypeCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) {
			e.CanExecute = true;
		}

		private void AddTypeCommand_Executed(object sender, ExecutedRoutedEventArgs e) {
			NewTypeWindow newTypeWindow = new NewTypeWindow();
			newTypeWindow.Show();
		}

		private void AddLabelCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) {
			e.CanExecute = true;
		}

		private void AddLabelCommand_Executed(object sender, ExecutedRoutedEventArgs e) {
			NewLabelWindow newLabelWindow = new NewLabelWindow();
			newLabelWindow.Show();
		}

		private void BarsViewCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) {
			e.CanExecute = true;
		}

		private void BarsViewCommand_Executed(object sender, ExecutedRoutedEventArgs e) {
			AllBarsWindow allBarsWindow = new AllBarsWindow();
			allBarsWindow.Show();
		}

		private void TypesViewCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) {
			e.CanExecute = true;
		}

		private void TypesViewCommand_Executed(object sender, ExecutedRoutedEventArgs e) {

		}

		private void LabelsViewCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) {
			e.CanExecute = true;
		}

		private void LabelsViewCommand_Executed(object sender, ExecutedRoutedEventArgs e) {

		}

		private void CopyCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) {
			e.CanExecute = true;
		}

		private void CopyCommand_Executed(object sender, ExecutedRoutedEventArgs e) {

		}

		private void PasteCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e) {
			e.CanExecute = true;
		}

		private void PasteCommand_Executed(object sender, ExecutedRoutedEventArgs e) {

		}
	}
}