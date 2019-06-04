using BarManager.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
namespace BarManager
{    /// <summary>    /// Interaction logic for FilterWindow.xaml    /// </summary>
    public partial class FilterWindow : Window    {
        public event PropertyChangedEventHandler PropertyChanged;        public AllBarsWindow allBarsWindow { get; set; }
        public MainWindow mainWindow { get; set; }

        protected virtual void OnPropertyChanged(string name)        {            if (PropertyChanged != null)            {                PropertyChanged(this, new PropertyChangedEventArgs(name));            }        }        public DateTime OpenedSince { get; set; }        public bool NoAlcohol;        public bool Until23;        public bool FullAlcohol;        public bool Low;        public bool Medium;        public bool High;        public bool ExtremelyHigh;
        public bool YesHandic;        public bool NoHandic;        public bool YesSmoking;        public bool NoSmoking;        public bool YesReservations;
        public bool NoReservations;
        private int _maxCapacity = 1;
        public int MaxCapacity
        {
            get
            {
                return _maxCapacity;
            }
            set
            {
                if (value != _maxCapacity)
                {
                    _maxCapacity = value;
                    OnPropertyChanged("MaxCapacity");
                }
            }
        }
        public ObservableCollection<Bar> _allBars = new ObservableCollection<Bar>();

        public ObservableCollection<Bar> AllBars
        {
            get
            {
                return _allBars;
            }
            set            {                if (value != _allBars)                {                    _allBars = value;                    OnPropertyChanged("AllBars");                }            }        }

        public ObservableCollection<BarLabel> _availableLabels = new ObservableCollection<BarLabel>();
        public ObservableCollection<BarLabel> AvailableLabels
        {
            get
            {
                return _availableLabels;
            }
            set            {                if (value != _availableLabels)                {                    _availableLabels = value;                    OnPropertyChanged("AvailableLabels");                }            }        }
        public ObservableCollection<BarLabel> _chosenLabels = new ObservableCollection<BarLabel>();
        public ObservableCollection<BarLabel> ChosenLabels        {            get            {                return _chosenLabels;            }            set            {                if (value != _chosenLabels)                {                    _chosenLabels = value;                    OnPropertyChanged("ChosenLabels");                }            }        }
        public ObservableCollection<BarType> _typeCollection = new ObservableCollection<BarType>();
        public ObservableCollection<BarType> TypeCollection        {            get            {                return _typeCollection;            }            set            {                if (value != _typeCollection)                {                    _typeCollection = value;                    OnPropertyChanged("TypeCollection");                }            }        }        public ObservableCollection<BarType> _chosenTypes = new ObservableCollection<BarType>();
        public ObservableCollection<BarType> ChosenTypes        {            get            {                return _chosenTypes;            }            set            {                if (value != _chosenTypes)                {                    _chosenTypes = value;                    OnPropertyChanged("ChosenTypes");                }            }        }
        public FilterWindow()        {
            InitializeComponent();
            getData();
            this.DataContext = this;
        }

        private void getData()
        {
            foreach (BarType t in Util.Util.BarTypes)
            {
                TypeCollection.Add(t);
            }

            foreach (BarLabel l in Util.Util.BarLabels)
            {
                AvailableLabels.Add(l);
            }


            foreach (Bar b in Util.Util.Bars)
            {
                AllBars.Add(b);
            }
        }

        private void CheckBoxZoneTypes_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox check = (CheckBox)sender;
            string typename = check.Content.ToString();
            foreach (BarType bt in TypeCollection)
            {
                if (bt.Name.Equals(typename))
                {
                    ChosenTypes.Add(bt);
                    break;
                }
            }
        }
        private void CheckBoxZoneTypes_unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox check = (CheckBox)sender;
            string typename = check.Content.ToString();
            foreach (BarType bt in ChosenTypes)
            {
                if (bt.Name.Equals(typename))
                {
                    ChosenTypes.Remove(bt);
                    break;
                }
            }
        }

        private void CheckBoxZoneLabels_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox check = (CheckBox)sender;
            foreach (BarLabel bl in AvailableLabels)
            {
                if (bl.Color == check.Content.ToString())
                {
                    ChosenLabels.Add(bl);
                    break;
                }
            }
        }
        private void CheckBoxZoneLabels_unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox check = (CheckBox)sender;
            foreach (BarLabel bl in ChosenLabels)
            {
                if (bl.Color == check.Content.ToString())
                {
                    ChosenLabels.Remove(bl);
                    break;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (allBarsWindow != null)
                FilterTable();
            else
            {
                FilterMap1();
                FilterMap2();
                FilterMap3();
                FilterMap4();
            }
            this.Close();
        }

        private void FilterTable()
        {
            var temp = new ObservableCollection<Bar>();
            var temp2 = new ObservableCollection<Bar>();
            //type
            if (ChosenTypes.Count > 0)
            {
                foreach (BarType bt in ChosenTypes)
                {
                    foreach (Bar b in AllBars)
                    {
                        if (b.Type.Id == bt.Id)
                        {
                            temp.Add(b);
                        }
                    }
                }
            }
            else
            {
                temp = AllBars;
            }
            //labels
            if (ChosenLabels.Count > 0)
            {
                foreach (BarLabel bl in ChosenLabels)
                {
                    foreach (Bar b in temp)
                    {
                        foreach (BarLabel bl2 in b.Labels)
                        {
                            if (bl.Equals(bl2))
                            {
                                temp2.Add(b);
                                break;
                            }
                        }
                    }
                }
                temp = new ObservableCollection<Bar>();
            }
            else
            {
                temp2 = temp;
                temp = new ObservableCollection<Bar>();
            }
            //date
            if (!(OpenedSince.Equals(null)))
            {
                foreach (Bar b in temp2)
                {
                    if (DateTime.Compare(OpenedSince, b.OpenDate) <= 0)
                    {
                        temp.Add(b);
                    }
                }
                temp2 = new ObservableCollection<Bar>();
            }
            else
            {
                temp = temp2;
                temp2 = new ObservableCollection<Bar>();
            }
            //alcohol 
            if (NoAlcohol || Until23 || FullAlcohol)
            {
                foreach (Bar b in temp)
                {
                    if (NoAlcohol && b.AlcStatus.Equals("Don't server"))
                    {
                        temp2.Add(b);
                    }
                    if (Until23 && b.AlcStatus.Equals("Serves up to 23h"))
                    {
                        temp2.Add(b);
                    }
                    if (FullAlcohol && b.AlcStatus.Equals("Full serve"))
                    {
                        temp2.Add(b);
                    }
                }
                temp = new ObservableCollection<Bar>();
            }
            else
            {
                temp2 = temp;
                temp = new ObservableCollection<Bar>();
            }
            if (Low || Medium || High || ExtremelyHigh)
            {
                foreach (Bar b in temp2)
                {
                    if (Low && b.PriceCategory.Equals("Low prices"))
                    {
                        temp.Add(b);
                    }
                    if (Medium && b.PriceCategory.Equals("Meduim prices"))
                    {
                        temp.Add(b);
                    }
                    if (High && b.PriceCategory.Equals("High prices"))
                    {
                        temp.Add(b);
                    }
                    if (ExtremelyHigh && b.PriceCategory.Equals("Extremely high prices"))
                    {
                        temp.Add(b);
                    }
                }
                temp2 = new ObservableCollection<Bar>();
            }
            else
            {
                temp = temp2;
                temp2 = new ObservableCollection<Bar>();
            }
            if (YesHandic || NoHandic)
            {
                foreach (Bar b in temp)
                {
                    if (YesHandic && b.Handicapped)
                    {
                        temp2.Add(b);
                    }
                    if (NoHandic && !(b.Handicapped))
                    {
                        temp2.Add(b);
                    }
                }
                temp = new ObservableCollection<Bar>();
            }
            else
            {
                temp2 = temp;
                temp = new ObservableCollection<Bar>();
            }
            if (YesSmoking || NoSmoking)
            {
                foreach (Bar b in temp2)
                {
                    if (YesSmoking && b.SmokingAllowed)
                    {
                        temp.Add(b);
                    }
                    if (NoSmoking && !(b.SmokingAllowed))
                    {
                        temp2.Add(b);
                    }
                }
                temp2 = new ObservableCollection<Bar>();
            }
            else
            {
                temp = temp2;
                temp2 = new ObservableCollection<Bar>();
            }
            if (YesReservations || NoReservations)
            {
                foreach (Bar b in temp)
                {
                    if (YesReservations && b.ReservationsAllowed)
                    {
                        temp2.Add(b);
                    }
                    if (NoReservations && !(b.ReservationsAllowed))
                    {
                        temp2.Add(b);
                    }
                }
                temp = new ObservableCollection<Bar>();
            }
            else
            {
                temp2 = temp;
                temp = new ObservableCollection<Bar>();
            }

            if (MaxCapacity != 0)
            {
                foreach (Bar b in temp2)
                {
                    if (b.MaxCapacity >= MaxCapacity)
                        temp.Add(b);
                }
                temp2 = new ObservableCollection<Bar>();
            }
            else
            {
                temp = temp2;
                temp2 = new ObservableCollection<Bar>();
            }

            allBarsWindow.barTble.ItemsSource = temp;
        }

        private void FilterMap1()
        {
            var temp = new ObservableCollection<Bar>();
            var temp2 = new ObservableCollection<Bar>();
            //type
            if (ChosenTypes.Count > 0)
            {
                foreach (BarType bt in ChosenTypes)
                {
                    foreach (BarView bw in mainWindow.BarsFromMap1.Values)
                    {
                        Bar b = bw.GetBar();
                        if (b.Type.Id == bt.Id)
                        {
                            temp.Add(b);
                        }
                    }
                }
            }
            else
            {
                foreach (BarView bw in mainWindow.BarsFromMap1.Values)
                    temp.Add(bw.GetBar());
            }
            //labels
            if (ChosenLabels.Count > 0)
            {
                foreach (BarLabel bl in ChosenLabels)
                {
                    foreach (Bar b in temp)
                    {
                        foreach (BarLabel bl2 in b.Labels)
                        {
                            if (bl.Equals(bl2))
                            {
                                temp2.Add(b);
                                break;
                            }
                        }
                    }
                }
                temp = new ObservableCollection<Bar>();
            }
            else
            {
                temp2 = temp;
                temp = new ObservableCollection<Bar>();
            }
            //date
            if (!(OpenedSince.Equals(null)))
            {
                foreach (Bar b in temp2)
                {
                    if (DateTime.Compare(OpenedSince, b.OpenDate) <= 0)
                    {
                        temp.Add(b);
                    }
                }
                temp2 = new ObservableCollection<Bar>();
            }
            else
            {
                temp = temp2;
                temp2 = new ObservableCollection<Bar>();
            }
            //alcohol 
            if (NoAlcohol || Until23 || FullAlcohol)
            {
                foreach (Bar b in temp)
                {
                    if (NoAlcohol && b.AlcStatus.Equals("Don't server"))
                    {
                        temp2.Add(b);
                    }
                    if (Until23 && b.AlcStatus.Equals("Serves up to 23h"))
                    {
                        temp2.Add(b);
                    }
                    if (FullAlcohol && b.AlcStatus.Equals("Full serve"))
                    {
                        temp2.Add(b);
                    }
                }
                temp = new ObservableCollection<Bar>();
            }
            else
            {
                temp2 = temp;
                temp = new ObservableCollection<Bar>();
            }
            
            if (Low || Medium || High || ExtremelyHigh)
            {
                foreach (Bar b in temp2)
                {
                    if (Low && b.PriceCategory.Equals("Low prices"))
                    {
                        temp.Add(b);
                    }
                    if (Medium && b.PriceCategory.Equals("Meduim prices"))
                    {
                        temp.Add(b);
                    }
                    if (High && b.PriceCategory.Equals("High prices"))
                    {
                        temp.Add(b);
                    }
                    if (ExtremelyHigh && b.PriceCategory.Equals("Extremely high prices"))
                    {
                        temp.Add(b);
                    }
                }
                temp2 = new ObservableCollection<Bar>();
            }
            else
            {
                temp = temp2;
                temp2 = new ObservableCollection<Bar>();
            }
            if (YesHandic || NoHandic)
            {
                foreach (Bar b in temp)
                {
                    if (YesHandic && b.Handicapped)
                    {
                        temp2.Add(b);
                    }
                    if (NoHandic && !(b.Handicapped))
                    {
                        temp2.Add(b);
                    }
                }
                temp = new ObservableCollection<Bar>();
            }
            else
            {
                temp2 = temp;
                temp = new ObservableCollection<Bar>();
            }
            if (YesSmoking || NoSmoking)
            {
                foreach (Bar b in temp2)
                {
                    if (YesSmoking && b.SmokingAllowed)
                    {
                        temp.Add(b);
                    }
                    if (NoSmoking && !(b.SmokingAllowed))
                    {
                        temp2.Add(b);
                    }
                }
                temp2 = new ObservableCollection<Bar>();
            }
            else
            {
                temp = temp2;
                temp2 = new ObservableCollection<Bar>();
            }
            if (YesReservations || NoReservations)
            {
                foreach (Bar b in temp)
                {
                    if (YesReservations && b.ReservationsAllowed)
                    {
                        temp2.Add(b);
                    }
                    if (NoReservations && !(b.ReservationsAllowed))
                    {
                        temp2.Add(b);
                    }
                }
                temp = new ObservableCollection<Bar>();
            }
            else
            {
                temp2 = temp;
                temp = new ObservableCollection<Bar>();
            }

            if (MaxCapacity != 0)
            {
                foreach (Bar b in temp2)
                {
                    if (b.MaxCapacity >= MaxCapacity)
                        temp.Add(b);
                }
                temp2 = new ObservableCollection<Bar>();
            }
            else
            {
                temp = temp2;
                temp2 = new ObservableCollection<Bar>();
            }

        }

        private void FilterMap2()
        {
            var temp = new ObservableCollection<Bar>();
            var temp2 = new ObservableCollection<Bar>();
            //type
            if (ChosenTypes.Count > 0)
            {
                foreach (BarType bt in ChosenTypes)
                {
                    foreach (BarView bw in mainWindow.BarsFromMap2.Values)
                    {
                        Bar b = bw.GetBar();
                        if (b.Type.Id == bt.Id)
                        {
                            temp.Add(b);
                        }
                    }
                }
            }
            else
            {
                foreach (BarView bw in mainWindow.BarsFromMap2.Values)
                    temp.Add(bw.GetBar());
            }
            //labels
            if (ChosenLabels.Count > 0)
            {
                foreach (BarLabel bl in ChosenLabels)
                {
                    foreach (Bar b in temp)
                    {
                        foreach (BarLabel bl2 in b.Labels)
                        {
                            if (bl.Equals(bl2))
                            {
                                temp2.Add(b);
                                break;
                            }
                        }
                    }
                }
                temp = new ObservableCollection<Bar>();
            }
            else
            {
                temp2 = temp;
                temp = new ObservableCollection<Bar>();
            }
            //date
            if (!(OpenedSince.Equals(null)))
            {
                foreach (Bar b in temp2)
                {
                    if (DateTime.Compare(OpenedSince, b.OpenDate) <= 0)
                    {
                        temp.Add(b);
                    }
                }
                temp2 = new ObservableCollection<Bar>();
            }
            else
            {
                temp = temp2;
                temp2 = new ObservableCollection<Bar>();
            }
            //alcohol 
            if (NoAlcohol || Until23 || FullAlcohol)
            {
                foreach (Bar b in temp)
                {
                    if (NoAlcohol && b.AlcStatus.Equals("Don't server"))
                    {
                        temp2.Add(b);
                    }
                    if (Until23 && b.AlcStatus.Equals("Serves up to 23h"))
                    {
                        temp2.Add(b);
                    }
                    if (FullAlcohol && b.AlcStatus.Equals("Full serve"))
                    {
                        temp2.Add(b);
                    }
                }
                temp = new ObservableCollection<Bar>();
            }
            else
            {
                temp2 = temp;
                temp = new ObservableCollection<Bar>();
            }
           
            if (Low || Medium || High || ExtremelyHigh)
            {
                foreach (Bar b in temp2)
                {
                    if (Low && b.PriceCategory.Equals("Low prices"))
                    {
                        temp.Add(b);
                    }
                    if (Medium && b.PriceCategory.Equals("Meduim prices"))
                    {
                        temp.Add(b);
                    }
                    if (High && b.PriceCategory.Equals("High prices"))
                    {
                        temp.Add(b);
                    }
                    if (ExtremelyHigh && b.PriceCategory.Equals("Extremely high prices"))
                    {
                        temp.Add(b);
                    }
                }
                temp2 = new ObservableCollection<Bar>();
            }
            else
            {
                temp = temp2;
                temp2 = new ObservableCollection<Bar>();
            }
            if (YesHandic || NoHandic)
            {
                foreach (Bar b in temp)
                {
                    if (YesHandic && b.Handicapped)
                    {
                        temp2.Add(b);
                    }
                    if (NoHandic && !(b.Handicapped))
                    {
                        temp2.Add(b);
                    }
                }
                temp = new ObservableCollection<Bar>();
            }
            else
            {
                temp2 = temp;
                temp = new ObservableCollection<Bar>();
            }
            if (YesSmoking || NoSmoking)
            {
                foreach (Bar b in temp2)
                {
                    if (YesSmoking && b.SmokingAllowed)
                    {
                        temp.Add(b);
                    }
                    if (NoSmoking && !(b.SmokingAllowed))
                    {
                        temp2.Add(b);
                    }
                }
                temp2 = new ObservableCollection<Bar>();
            }
            else
            {
                temp = temp2;
                temp2 = new ObservableCollection<Bar>();
            }
            if (YesReservations || NoReservations)
            {
                foreach (Bar b in temp)
                {
                    if (YesReservations && b.ReservationsAllowed)
                    {
                        temp2.Add(b);
                    }
                    if (NoReservations && !(b.ReservationsAllowed))
                    {
                        temp2.Add(b);
                    }
                }
                temp = new ObservableCollection<Bar>();
            }
            else
            {
                temp2 = temp;
                temp = new ObservableCollection<Bar>();
            }

            if (MaxCapacity != 0)
            {
                foreach (Bar b in temp2)
                {
                    if (b.MaxCapacity >= MaxCapacity)
                        temp.Add(b);
                }
                temp2 = new ObservableCollection<Bar>();
            }
            else
            {
                temp = temp2;
                temp2 = new ObservableCollection<Bar>();
            }

        }

        private void FilterMap3()
        {
            var temp = new ObservableCollection<Bar>();
            var temp2 = new ObservableCollection<Bar>();
            //type
            if (ChosenTypes.Count > 0)
            {
                foreach (BarType bt in ChosenTypes)
                {
                    foreach (BarView bw in mainWindow.BarsFromMap3.Values)
                    {
                        Bar b = bw.GetBar();
                        if (b.Type.Id == bt.Id)
                        {
                            temp.Add(b);
                        }
                    }
                }
            }
            else
            {
                foreach (BarView bw in mainWindow.BarsFromMap3.Values)
                    temp.Add(bw.GetBar());
            }
            //labels
            if (ChosenLabels.Count > 0)
            {
                foreach (BarLabel bl in ChosenLabels)
                {
                    foreach (Bar b in temp)
                    {
                        foreach (BarLabel bl2 in b.Labels)
                        {
                            if (bl.Equals(bl2))
                            {
                                temp2.Add(b);
                                break;
                            }
                        }
                    }
                }
                temp = new ObservableCollection<Bar>();
            }
            else
            {
                temp2 = temp;
                temp = new ObservableCollection<Bar>();
            }
            //date
            if (!(OpenedSince.Equals(null)))
            {
                foreach (Bar b in temp2)
                {
                    if (DateTime.Compare(OpenedSince, b.OpenDate) <= 0)
                    {
                        temp.Add(b);
                    }
                }
                temp2 = new ObservableCollection<Bar>();
            }
            else
            {
                temp = temp2;
                temp2 = new ObservableCollection<Bar>();
            }
            //alcohol 
            if (NoAlcohol || Until23 || FullAlcohol)
            {
                foreach (Bar b in temp)
                {
                    if (NoAlcohol && b.AlcStatus.Equals("Don't server"))
                    {
                        temp2.Add(b);
                    }
                    if (Until23 && b.AlcStatus.Equals("Serves up to 23h"))
                    {
                        temp2.Add(b);
                    }
                    if (FullAlcohol && b.AlcStatus.Equals("Full serve"))
                    {
                        temp2.Add(b);
                    }
                }
                temp = new ObservableCollection<Bar>();
            }
            else
            {
                temp2 = temp;
                temp = new ObservableCollection<Bar>();
            }

            if (Low || Medium || High || ExtremelyHigh)
            {
                foreach (Bar b in temp2)
                {
                    if (Low && b.PriceCategory.Equals("Low prices"))
                    {
                        temp.Add(b);
                    }
                    if (Medium && b.PriceCategory.Equals("Meduim prices"))
                    {
                        temp.Add(b);
                    }
                    if (High && b.PriceCategory.Equals("High prices"))
                    {
                        temp.Add(b);
                    }
                    if (ExtremelyHigh && b.PriceCategory.Equals("Extremely high prices"))
                    {
                        temp.Add(b);
                    }
                }
                temp2 = new ObservableCollection<Bar>();
            }
            else
            {
                temp = temp2;
                temp2 = new ObservableCollection<Bar>();
            }
            if (YesHandic || NoHandic)
            {
                foreach (Bar b in temp)
                {
                    if (YesHandic && b.Handicapped)
                    {
                        temp2.Add(b);
                    }
                    if (NoHandic && !(b.Handicapped))
                    {
                        temp2.Add(b);
                    }
                }
                temp = new ObservableCollection<Bar>();
            }
            else
            {
                temp2 = temp;
                temp = new ObservableCollection<Bar>();
            }
            if (YesSmoking || NoSmoking)
            {
                foreach (Bar b in temp2)
                {
                    if (YesSmoking && b.SmokingAllowed)
                    {
                        temp.Add(b);
                    }
                    if (NoSmoking && !(b.SmokingAllowed))
                    {
                        temp2.Add(b);
                    }
                }
                temp2 = new ObservableCollection<Bar>();
            }
            else
            {
                temp = temp2;
                temp2 = new ObservableCollection<Bar>();
            }
            if (YesReservations || NoReservations)
            {
                foreach (Bar b in temp)
                {
                    if (YesReservations && b.ReservationsAllowed)
                    {
                        temp2.Add(b);
                    }
                    if (NoReservations && !(b.ReservationsAllowed))
                    {
                        temp2.Add(b);
                    }
                }
                temp = new ObservableCollection<Bar>();
            }
            else
            {
                temp2 = temp;
                temp = new ObservableCollection<Bar>();
            }

            if (MaxCapacity != 0)
            {
                foreach (Bar b in temp2)
                {
                    if (b.MaxCapacity >= MaxCapacity)
                        temp.Add(b);
                }
                temp2 = new ObservableCollection<Bar>();
            }
            else
            {
                temp = temp2;
                temp2 = new ObservableCollection<Bar>();
            }

        }

        private void FilterMap4()
        {
            var temp = new ObservableCollection<Bar>();
            var temp2 = new ObservableCollection<Bar>();
            //type
            if (ChosenTypes.Count > 0)
            {
                foreach (BarType bt in ChosenTypes)
                {
                    foreach (BarView bw in mainWindow.BarsFromMap4.Values)
                    {
                        Bar b = bw.GetBar();
                        if (b.Type.Id == bt.Id)
                        {
                            temp.Add(b);
                        }
                    }
                }
            }
            else
            {
                foreach (BarView bw in mainWindow.BarsFromMap4.Values)
                    temp.Add(bw.GetBar());
            }
            //labels
            if (ChosenLabels.Count > 0)
            {
                foreach (BarLabel bl in ChosenLabels)
                {
                    foreach (Bar b in temp)
                    {
                        foreach (BarLabel bl2 in b.Labels)
                        {
                            if (bl.Equals(bl2))
                            {
                                temp2.Add(b);
                                break;
                            }
                        }
                    }
                }
                temp = new ObservableCollection<Bar>();
            }
            else
            {
                temp2 = temp;
                temp = new ObservableCollection<Bar>();
            }
            //date
            if (!(OpenedSince.Equals(null)))
            {
                foreach (Bar b in temp2)
                {
                    if (DateTime.Compare(OpenedSince, b.OpenDate) <= 0)
                    {
                        temp.Add(b);
                    }
                }
                temp2 = new ObservableCollection<Bar>();
            }
            else
            {
                temp = temp2;
                temp2 = new ObservableCollection<Bar>();
            }
            //alcohol 
            if (NoAlcohol || Until23 || FullAlcohol)
            {
                foreach (Bar b in temp)
                {
                    if (NoAlcohol && b.AlcStatus.Equals("Don't server"))
                    {
                        temp2.Add(b);
                    }
                    if (Until23 && b.AlcStatus.Equals("Serves up to 23h"))
                    {
                        temp2.Add(b);
                    }
                    if (FullAlcohol && b.AlcStatus.Equals("Full serve"))
                    {
                        temp2.Add(b);
                    }
                }
                temp = new ObservableCollection<Bar>();
            }
            else
            {
                temp2 = temp;
                temp = new ObservableCollection<Bar>();
            }
            
            if (Low || Medium || High || ExtremelyHigh)
            {
                foreach (Bar b in temp2)
                {
                    if (Low && b.PriceCategory.Equals("Low prices"))
                    {
                        temp.Add(b);
                    }
                    if (Medium && b.PriceCategory.Equals("Meduim prices"))
                    {
                        temp.Add(b);
                    }
                    if (High && b.PriceCategory.Equals("High prices"))
                    {
                        temp.Add(b);
                    }
                    if (ExtremelyHigh && b.PriceCategory.Equals("Extremely high prices"))
                    {
                        temp.Add(b);
                    }
                }
                temp2 = new ObservableCollection<Bar>();
            }
            else
            {
                temp = temp2;
                temp2 = new ObservableCollection<Bar>();
            }
            if (YesHandic || NoHandic)
            {
                foreach (Bar b in temp)
                {
                    if (YesHandic && b.Handicapped)
                    {
                        temp2.Add(b);
                    }
                    if (NoHandic && !(b.Handicapped))
                    {
                        temp2.Add(b);
                    }
                }
                temp = new ObservableCollection<Bar>();
            }
            else
            {
                temp2 = temp;
                temp = new ObservableCollection<Bar>();
            }
            if (YesSmoking || NoSmoking)
            {
                foreach (Bar b in temp2)
                {
                    if (YesSmoking && b.SmokingAllowed)
                    {
                        temp.Add(b);
                    }
                    if (NoSmoking && !(b.SmokingAllowed))
                    {
                        temp2.Add(b);
                    }
                }
                temp2 = new ObservableCollection<Bar>();
            }
            else
            {
                temp = temp2;
                temp2 = new ObservableCollection<Bar>();
            }
            if (YesReservations || NoReservations)
            {
                foreach (Bar b in temp)
                {
                    if (YesReservations && b.ReservationsAllowed)
                    {
                        temp2.Add(b);
                    }
                    if (NoReservations && !(b.ReservationsAllowed))
                    {
                        temp2.Add(b);
                    }
                }
                temp = new ObservableCollection<Bar>();
            }
            else
            {
                temp2 = temp;
                temp = new ObservableCollection<Bar>();
            }

            if (MaxCapacity != 0)
            {
                foreach (Bar b in temp2)
                {
                    if (b.MaxCapacity >= MaxCapacity)
                        temp.Add(b);
                }
                temp2 = new ObservableCollection<Bar>();
            }
            else
            {
                temp = temp2;
                temp2 = new ObservableCollection<Bar>();
            }

        }

        private void OpenDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            var picker = sender as DatePicker;

            DateTime? date = picker.SelectedDate;
            if (date != null)
            {
                OpenedSince = picker.SelectedDate.Value;
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox check = (CheckBox)sender;
            String checkName = check.Name;
            switch (checkName)
            {
                case "noAlc":
                    NoAlcohol = true;
                    break;
                case "till23Alc":
                    Until23 = true;
                    break;
                case "fullAlc":
                    FullAlcohol = true;
                    break;
                case "checkLow":
                    Low = true;
                    break;
                case "checkMedium":
                    Medium = true;
                    break;
                case "checkHigh":
                    High = true;
                    break;
                case "checkExtrHigh":
                    ExtremelyHigh = true;
                    break;
                case "HandYes":
                    YesHandic = true;
                    break;
                case "HandNo":
                    NoHandic = true;
                    break;
                case "SmokingYes":
                    YesSmoking = true;
                    break;
                case "SmokingNo":
                    NoSmoking = true;
                    break;
                case "ReservationsYes":
                    YesReservations = true;
                    break;
                case "ReservationsNo":
                    NoReservations = true;
                    break;

                default:
                    break;
            }
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox check = (CheckBox)sender;
            String checkName = check.Name;
            switch (checkName)
            {
                case "noAlc":
                    NoAlcohol = false;
                    break;
                case "till23Alc":
                    Until23 = false;
                    break;
                case "fullAlc":
                    FullAlcohol = false;
                    break;
                case "checkLow":
                    Low = false;
                    break;
                case "checkMedium":
                    Medium = false;
                    break;
                case "checkHigh":
                    High = false;
                    break;
                case "checkExtrHigh":
                    ExtremelyHigh = false;
                    break;
                case "HandYes":
                    YesHandic = false;
                    break;
                case "HandNo":
                    NoHandic = false;
                    break;
                case "SmokingYes":
                    YesSmoking = false;
                    break;
                case "SmokingNo":
                    NoSmoking = false;
                    break;
                case "ReservationsYes":
                    YesReservations = false;
                    break;
                case "ReservationsNo":
                    NoReservations = false;
                    break;

                default:
                    break;
            }
        }

        private void BarCapacity_TextChanged(object sender, TextChangedEventArgs e)
        {
            barCapacity.Text = Regex.Replace(barCapacity.Text, "[^0-9]+", "");
            barCapacity.CaretIndex = barCapacity.Text.Length;
            if (barCapacity.Text.Length == 0)
            {
                filterBtn.IsEnabled = false;
                MaxCapacity = 0;
            }
            else
            {
                int capacity = 9999;
                Int32.TryParse(barCapacity.Text, out capacity);
                if (capacity <= 2000 && capacity >= 1)
                {
                    filterBtn.IsEnabled = true;
                    MaxCapacity = capacity;
                }
                else
                {
                    filterBtn.IsEnabled = false;
                    MaxCapacity = 0;
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
