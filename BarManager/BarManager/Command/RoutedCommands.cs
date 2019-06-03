using System.Windows.Input;

namespace BarManager.Command {
	public static class RoutedCommands {
		public static readonly RoutedUICommand Help = new RoutedUICommand(
			"Help",
			"Help",
			typeof(RoutedCommands),
			new InputGestureCollection() {
				new KeyGesture(Key.F1)
			});

		public static readonly RoutedUICommand AddBar = new RoutedUICommand(
			"Add bar",
			"AddBar",
			typeof(RoutedCommands),
			new InputGestureCollection() {
				new KeyGesture(Key.F2)
			});

		public static readonly RoutedUICommand AddType = new RoutedUICommand(
			"Add type",
			"AddType",
			typeof(RoutedCommands),
			new InputGestureCollection() {
				new KeyGesture(Key.F3)
			});

		public static readonly RoutedUICommand AddLabel = new RoutedUICommand(
			"Add label",
			"AddLabel",
			typeof(RoutedCommands),
			new InputGestureCollection() {
				new KeyGesture(Key.F4)
			});

		public static readonly RoutedUICommand BarsView = new RoutedUICommand(
			"Bars view",
			"BarsView",
			typeof(RoutedCommands),
			new InputGestureCollection() {
				new KeyGesture(Key.F5)
			});

		public static readonly RoutedUICommand TypesView = new RoutedUICommand(
			"Types view",
			"TypesView",
			typeof(RoutedCommands),
			new InputGestureCollection() {
				new KeyGesture(Key.F6)
			});

		public static readonly RoutedUICommand LabelsView = new RoutedUICommand(
			"Labels view",
			"LabelsView",
			typeof(RoutedCommands),
			new InputGestureCollection() {
				new KeyGesture(Key.F7)
			});

		public static readonly RoutedUICommand Edit = new RoutedUICommand(
			"Edit",
			"Edit",
			typeof(RoutedCommands),
			new InputGestureCollection() {
				new KeyGesture(Key.F8)
			});

		public static readonly RoutedUICommand Delete = new RoutedUICommand(
			"Delete",
			"Delete",
			typeof(RoutedCommands),
			new InputGestureCollection() {
				new KeyGesture(Key.F9)
			});

		public static readonly RoutedUICommand Copy = new RoutedUICommand(
			"Copy",
			"Copy",
			typeof(RoutedCommands),
			new InputGestureCollection() {
				new KeyGesture(Key.C, ModifierKeys.Control)
			});

		public static readonly RoutedUICommand Paste = new RoutedUICommand(
			"Paste",
			"Paste",
			typeof(RoutedCommands),
			new InputGestureCollection() {
				new KeyGesture(Key.V, ModifierKeys.Control)
			});
	}
}
