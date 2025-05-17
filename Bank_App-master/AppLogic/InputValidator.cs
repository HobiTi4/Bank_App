using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank_App.AppLogic
{
    public class InputValidator
    {
        public bool IsValidEmail(string email)
        {
            return email.Contains("@") && email.Contains(".");
        }

        public bool IsValidPassword(string password)
        {
            return password.Any(char.IsUpper) &&
                   password.Any(char.IsLower) &&
                   password.Any(char.IsDigit);
        }
    }
}
