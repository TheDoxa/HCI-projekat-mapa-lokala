using BarManager.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Media;

namespace BarManager.Util {

	class Util {

        public static ObservableCollection<Bar> bars = new ObservableCollection<Bar>();
        public static ObservableCollection<BarType> barTypes = new ObservableCollection<BarType>();
		public static ObservableCollection<BarLabel> barLabels = new ObservableCollection<BarLabel>();

        private static readonly string barPath = Path.GetFullPath(@"..\..\") + @"Resources\bars.txt";
        private static readonly string typesPath = Path.GetFullPath(@"..\..\") + @"Resources\types.txt";
		private static readonly string labelsPath = Path.GetFullPath(@"..\..\") + @"Resources\labels.txt";

        public Util() { }

		public static void loadTypes() {
			if(!File.Exists(typesPath))
				return;

			barTypes.Clear();

			if(File.Exists(typesPath)) {
				string[] lines = File.ReadAllLines(typesPath);

				foreach(string line in lines) {
					string[] typeParams = line.Split('|');
					BarType type = new BarType(Int32.Parse(typeParams[0]), typeParams[1], typeParams[2], typeParams[3]);

					barTypes.Add(type);
				}
			}
		}

		public static void writeTypes() {
			if(!File.Exists(typesPath))
				File.Create(typesPath);

			File.WriteAllText(typesPath, string.Empty);

			string content = "";
			foreach(BarType type in barTypes) {
				content += type.Id + "|" + type.Name + "|" + type.Description + "|" + type.IconPath + System.Environment.NewLine;
			}

			File.WriteAllText(typesPath, content);
		}

		public static bool addType(BarType type) {
			foreach(BarType t in barTypes)
				if(t.Id == type.Id)
					return false;

			barTypes.Add(type);
			writeTypes();

			return true;
		}

		public static void removeType(BarType type) {
			barTypes.Remove(type);
			writeTypes();
		}

		public static void loadLabels() {
			if(!File.Exists(labelsPath))
				return;

			barLabels.Clear();

			if(File.Exists(labelsPath)) {
				string[] lines = File.ReadAllLines(labelsPath);

				foreach(string line in lines) {
					string[] labelParams = line.Split('|');
					BarLabel label = new BarLabel(Int32.Parse(labelParams[0]), labelParams[1], labelParams[2]);

					barLabels.Add(label);
				}
			}
		}

		public static void writeLabels() {
			if(!File.Exists(labelsPath))
				File.Create(labelsPath);

			File.WriteAllText(labelsPath, string.Empty);

			string content = "";
			foreach(BarLabel label in barLabels) {
				content += label.Id + "|" + label.Description + "|" + label.Color + System.Environment.NewLine;
			}

			File.WriteAllText(labelsPath, content);
		}

		public static bool addLabel(BarLabel label) {
			foreach(BarLabel l in barLabels)
				if(l.Id == label.Id)
					return false;

			barLabels.Add(label);
			writeLabels();

			return true;
		}

		public static void removeLabel(BarLabel label) {
			barLabels.Remove(label);
			writeLabels();
		}
	}
}
