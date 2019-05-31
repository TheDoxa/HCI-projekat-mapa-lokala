namespace BarManager.Model {
	class BarType {
		public int id { get; set; }
		public string name { get; set; }
		public string description { get; set; }
		public string iconPath { get; set; }

		public BarType(int id, string name, string description, string iconPath) {
			this.id = id;
			this.name = name;
			this.description = description;
			this.iconPath = iconPath;
		}
	}
}
