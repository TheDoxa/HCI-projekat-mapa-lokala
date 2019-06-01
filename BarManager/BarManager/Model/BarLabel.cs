using System.Windows.Media;

namespace BarManager.Model {

	class BarLabel {

		public int Id { get; set; }
		public string Color { get; set; }
		public string Description { get; set; }

        public BarLabel() { }

		public BarLabel(int id, string color, string description) {
			this.Id = id;
			this.Color = color;
			this.Description = description;
		}
	}
}
