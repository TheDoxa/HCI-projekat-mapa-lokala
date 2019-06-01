using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace BarManager.Validation
{
    public class StringMinLength : ValidationRule
    {
        public int Min
        {
            get;
            set;
        }
        
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            if (value is string)
            {
                //Name must contain at least 3 letters!
                if (value.ToString().Length < Min) return new ValidationResult(false, "");
                return new ValidationResult(true, null);
            }
            else
            {
                return new ValidationResult(false, " Unknown error occured.");
            }
        }
    }
}
