using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarManager.Model
{
    public class Bar
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public BarType Type { get; set; }
        public string AlcStatus { get; set; }
        public string Icon { get; set; }
        public bool Handicapped { get; set; }
        public bool SmokingAllowed { get; set; }
        public bool ReservationsAllowed { get; set; }
        public string PriceCategory { get; set; }
        public int MaxCapacity { get; set; }
        public DateTime OpenDate { get; set; }
        public ObservableCollection<BarLabel> Labels { get; set; }


        public Bar()
        {
            Type = new BarType();
            OpenDate = new DateTime();
            Labels = new ObservableCollection<BarLabel>();
        }

        public Bar(int id, string name, string desc, BarType bartype, string alcStatus, string iconPath, bool hand, bool smoking, bool reser,
            string priceCat, int maxCat, DateTime date, ObservableCollection<BarLabel> labels)
        {
            Id = id;
            Name = name;
            Description = desc;
            Type = bartype;
            AlcStatus = alcStatus;
            Icon = iconPath;
            Handicapped = hand;
            SmokingAllowed = smoking;
            ReservationsAllowed = reser;
            PriceCategory = priceCat;
            MaxCapacity = maxCat;
            OpenDate = date;
            Labels = labels;
        }

        public string getFileLine()
        {
            string handB = Handicapped ? "1" : "0";
            string smokingB = SmokingAllowed ? "1" :"0";
            string reserB = ReservationsAllowed ? "1" : "0";
            

            string lineToFile = Id + "|" + Name + "|" + Description + "|" + Type.Id + "|" + AlcStatus + "|" + Icon + "|" + handB
                + "|" + smokingB + "|" + reserB + "|" + PriceCategory + "|" + MaxCapacity.ToString() + "|" + OpenDate.ToString("dd'/'MM'/'yyyy")
                + "|";
            for (int i = 0; i < Labels.Count; i++)
            {
                lineToFile += Labels[i].Id;
                if (i != Labels.Count - 1)
                {
                    lineToFile += ";";
                }
            }

            return lineToFile;
        }


    }
}
