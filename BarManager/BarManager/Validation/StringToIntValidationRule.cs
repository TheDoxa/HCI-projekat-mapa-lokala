using System;
using System.Globalization;
using System.Windows.Controls;

namespace BarManager.validation {
	public class StringToIntValidationRule : ValidationRule {
		public override ValidationResult Validate(object value, CultureInfo cultureInfo) {
			try {
				var text = value as string;
				int result;
				if(Int32.TryParse(text, out result)) {
					return new ValidationResult(true, null);
				}
				return new ValidationResult(false, "Please enter a valid int value.");
			} catch {
				return new ValidationResult(false, "Unknown error occured.");
			}
		}
	}
}
