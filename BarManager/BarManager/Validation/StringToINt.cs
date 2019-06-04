using System;
using System.Globalization;
using System.Windows.Controls;

namespace BarManager.Validation {

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


        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
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

	public class StringToIntValidationRule : ValidationRule {
		public override ValidationResult Validate(object value, CultureInfo cultureInfo) {
			try {
				var text = value as string;
				int result;
				if(Int32.TryParse(text, out result)) {
					return new ValidationResult(true, null);
				}
				return new ValidationResult(false, " Please enter a valid value.");
			} catch {
				return new ValidationResult(false, " Unknown error occured.");
			}
		}
	}
}
