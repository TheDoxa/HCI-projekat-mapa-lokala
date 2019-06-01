namespace BarManager.Model {

	class BarType {

		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string IconPath { get; set; }

        public BarType() { }

		public BarType(int id, string name, string description, string iconPath) {
			Id = id;
			Name = name;
			Description = description;
			IconPath = iconPath;
		}
	}
}
