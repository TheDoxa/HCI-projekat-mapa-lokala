using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace BarManager.Validation
{

    public class MinMaxValidationRule : ValidationRule
    {
        public int Min
        {
            get;
            set;
        }

        public int Max
        {
            get;
            set;
        }


        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            try
            {
                string s = value as string;
                int d;
                if (Int32.TryParse(s,out d))
                {
                    if (d < Min) return new ValidationResult(false, " Min capacity is 1!");
                    if (d > Max) return new ValidationResult(false, " Max capacity is 2000!");

                }
                return new ValidationResult(true, null);
            }
            catch
            {
                return new ValidationResult(false, " Unknown error occured.");
            }
        }
    }

    public class StringToIntValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            try
            {
                var s = value as string;
                int r;
                if (int.TryParse(s, out r))
                {
                    return new ValidationResult(true, null);
                }
                return new ValidationResult(false, " Please enter a valid number");
            }
            catch
            {
                return new ValidationResult(false, " Unknown error occured.");
            }
        }
    }
}
