using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarManager.Model
{
    class Bar
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
        public string CapacityCategory { get; set; }
        public DateTime OpenDate { get; set; }
        public List<BarLabel> Labels { get; set; }


        public Bar(int id, string name, string desc, int typeID, string alcStatus, string iconPath, bool hand, bool smoking, bool reser,
            string priceCat, string capCat, DateTime date, string labels)
        {
            Id = id;
            Name = name;
            Description = desc;
            Type = new BarType();
            AlcStatus = alcStatus;
            Icon = iconPath;
            Handicapped = hand;
            SmokingAllowed = smoking;
            ReservationsAllowed = reser;
            PriceCategory = priceCat;
            CapacityCategory = capCat;
            OpenDate = date;
            Labels = new List<BarLabel>();
        }

        public string getFileLine()
        {
            string lineToFile = this.Id + "|" + this.Name + "|" + this.Description + "|" + this.Type.Id + "|" + this.AlcStatus + "|" + this.Icon + "|" + this.Handicapped
                + "|" + this.SmokingAllowed + "|" + this.ReservationsAllowed + "|" + this.PriceCategory + "|" + this.CapacityCategory + "|" + this.OpenDate.ToString()
                + "|";
            for (int i = 0; i < Labels.Capacity; i++)
            {
                lineToFile += Labels[i];
                if (i != Labels.Capacity - 1)
                {
                    lineToFile += ";";
                }
            }

            return lineToFile;
        }


    }
}
