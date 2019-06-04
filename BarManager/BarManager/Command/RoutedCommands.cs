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

		public static readonly RoutedUICommand Edit = new RoutedUICommand(
			"Edit",
			"Edit",
			typeof(RoutedCommands),
			new InputGestureCollection() {
				new KeyGesture(Key.F6)
			});

		public static readonly RoutedUICommand Delete = new RoutedUICommand(
			"Delete",
			"Delete",
			typeof(RoutedCommands),
			new InputGestureCollection() {
				new KeyGesture(Key.F7)
			});

		public static readonly RoutedUICommand Escape = new RoutedUICommand(
			"Escape",
			"Escape",
			typeof(RoutedCommands),
			new InputGestureCollection() {
				new KeyGesture(Key.Escape)
			});

		public static readonly RoutedUICommand Enter = new RoutedUICommand(
			"Enter",
			"Enter",
			typeof(RoutedCommands),
			new InputGestureCollection() {
				new KeyGesture(Key.Enter)
			});
	}
}
