using BarManager.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace BarManager.Util {

	public sealed class Util : INotifyPropertyChanged
    {
        private static ObservableCollection<Bar> bars = new ObservableCollection<Bar>();
        private static ObservableCollection<BarType> barTypes = new ObservableCollection<BarType>();
		private static ObservableCollection<BarLabel> barLabels = new ObservableCollection<BarLabel>();

        private static readonly Util instance = new Util();

        public static ObservableCollection<Bar> Bars
        {
            get
            {
                return bars;
            }

            set
            {
                bars = value;
                OnPropertyChangedStatic();
            }
        }

        public static ObservableCollection<BarType> BarTypes
        {
            get
            {
                return barTypes;
            }

            set
            {
                barTypes = value;
                OnPropertyChangedStatic();
            }
        }

        public static ObservableCollection<BarLabel> BarLabels
        {
            get
            {
                return barLabels;
            }

            set
            {
                barLabels = value;
                OnPropertyChangedStatic();
            }
        }

        private static readonly string barPath = Path.GetFullPath(@"..\..\") + @"Resources\Bars.txt";
        private static readonly string typesPath = Path.GetFullPath(@"..\..\") + @"Resources\types.txt";
		private static readonly string labelsPath = Path.GetFullPath(@"..\..\") + @"Resources\labels.txt";

        public event PropertyChangedEventHandler PropertyChanged;
        public static event EventHandler<PropertyChangedEventArgs> StaticPropertyChanged;

        public Util() { }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public static void OnPropertyChangedStatic([CallerMemberName] string propertyName = null)
        {
            StaticPropertyChanged?.Invoke(Util.instance, new PropertyChangedEventArgs(propertyName));
        }

        //private static void OnPropertyRaised(string propertyname)
        //{
        //    if (PropertyChanged != null)
        //    {
        //        PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
        //    }
        //}

        public static void loadTypes() {
			if(!File.Exists(typesPath))
				return;

			BarTypes.Clear();

			if(File.Exists(typesPath)) {
				string[] lines = File.ReadAllLines(typesPath);

				foreach(string line in lines) {
					string[] typeParams = line.Split('|');
					BarType type = new BarType(Int32.Parse(typeParams[0]), typeParams[1], typeParams[2], typeParams[3]);

					BarTypes.Add(type);
				}
			}
		}

		public static void writeTypes() {
			if(!File.Exists(typesPath))
				File.Create(typesPath);

			File.WriteAllText(typesPath, string.Empty);

			string content = "";
			foreach(BarType type in BarTypes) {
				content += type.Id + "|" + type.Name + "|" + type.Description + "|" + type.IconPath + System.Environment.NewLine;
			}

			File.WriteAllText(typesPath, content);
		}

		public static bool addType(BarType type) {
			foreach(BarType t in BarTypes)
				if(t.Id == type.Id)
					return false;

			BarTypes.Add(type);
			writeTypes();

			return true;
		}

		public static void updateType(BarType type) {
			foreach(BarType t in BarTypes)
				if(t.Id == type.Id) {
					t.Name = type.Name;
					t.Description = type.Description;
					t.IconPath = type.IconPath;

					break;
				}

			writeTypes();
		}

		public static void removeType(BarType type) {
			BarTypes.Remove(type);
			writeTypes();
		}

		public static void loadLabels() {
			if(!File.Exists(labelsPath))
				return;

			BarLabels.Clear();

			if(File.Exists(labelsPath)) {
				string[] lines = File.ReadAllLines(labelsPath);

				foreach(string line in lines) {
					string[] labelParams = line.Split('|');
					BarLabel label = new BarLabel(Int32.Parse(labelParams[0]), labelParams[1], labelParams[2]);

					BarLabels.Add(label);
				}
			}
		}

		public static void writeLabels() {
			if(!File.Exists(labelsPath))
				File.Create(labelsPath);

			File.WriteAllText(labelsPath, string.Empty);

			string content = "";
			foreach(BarLabel label in BarLabels) {
				content += label.Id + "|" + label.Color + "|" + label.Description + System.Environment.NewLine;
			}

			File.WriteAllText(labelsPath, content);
		}

		public static bool addLabel(BarLabel label) {
			foreach(BarLabel l in BarLabels)
				if(l.Id == label.Id)
					return false;

			BarLabels.Add(label);
			writeLabels();

			return true;
		}

		public static void updateLabel(BarLabel label) {
			foreach(BarLabel l in BarLabels) {
				if(l.Id == label.Id) {
					l.Description = label.Description;
					l.Color = label.Color;

					break;
				}
			}

			writeLabels();
		}

		public static void removeLabel(BarLabel label) {
			BarLabels.Remove(label);
			writeLabels();
		}

		public static void loadBars()
        {
            if (!File.Exists(barPath))
                return;

            Bars.Clear();

            if (File.Exists(barPath))
            {
                string[] lines = File.ReadAllLines(barPath);

                foreach (string line in lines)
                {
                    string[] barParams = line.Split('|');
                    
                    bool hand = barParams[6] == "1";
                    bool smoking = barParams[7] == "1";
                    bool reservations = barParams[8] == "1";
                    string[] barLabelsString = barParams[12].Split(';');
                    ObservableCollection<BarLabel> labels = new ObservableCollection<BarLabel>();
                    DateTime date = DateTime.ParseExact(barParams[11], "dd/MM/yyyy", CultureInfo.InvariantCulture);

                    if (barLabelsString[0] != "")
                    {
                        foreach (BarLabel br in BarLabels)
                        {
                            foreach (string lid in barLabelsString)
                            {
                                if (Int32.Parse(lid) == br.Id)
                                {
                                    labels.Add(br);
                                    break;
                                }
                            }
                        }
                    }
                    

                    Bar bar = new Bar(Int32.Parse(barParams[0]), barParams[1], barParams[2], getTypeByID(Int32.Parse(barParams[3])), barParams[4], barParams[5], hand, smoking, reservations,
                       barParams[9], Int32.Parse(barParams[10]), date, labels);

                    Bars.Add(bar);
                }
            }
        }

        public static void writeBars()
        {
            if (!File.Exists(barPath))
                File.Create(barPath);

            File.WriteAllText(barPath, string.Empty);

            string content = "";
            foreach (Bar bar in Bars)
            {
                content += bar.getFileLine() + System.Environment.NewLine;
            }

            File.WriteAllText(barPath, content);
        }

        public static bool addBar(Bar bar)
        {
            foreach (Bar b in Bars)
                if (b.Id == bar.Id)
                    return false;

            Bars.Add(bar);
            writeBars();
            return true;
        }

        public static void removeBar(Bar bar)
        {
            Bars.Remove(bar);
            writeTypes();
        }

        public static void modifyBar(Bar bar)
        {
            int id = 0;
            for(int i = 0; i < Bars.Count; i++)
            {
                if(Bars[i].Id == bar.Id)
                {
                    id = i;
                    break;
                }
            }
            Bars.RemoveAt(id);
            bool check = addBar(bar);
        }

        private static BarType getTypeByID(int id)
        {
            BarType type = new BarType();
            foreach(BarType ty in BarTypes)
            {
                if(ty.Id == id)
                {
                    type = ty;
                    break;
                }
            }
            return type;
        }
    }
}
