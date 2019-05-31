using System.Windows.Media;

namespace BarManager.Model {
	class BarLabel {
		public int id { get; set; }
		public string color { get; set; }
		public string description { get; set; }

		public BarLabel(int id, string color, string description) {
			this.id = id;
			this.color = color;
			this.description = description;
		}
	}
}
