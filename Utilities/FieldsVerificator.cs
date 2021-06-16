using System.Text.RegularExpressions;
using WPFCustomMessageBox;

namespace BAMEX.Utilities
{
    class FieldsVerificator
    {
        public static bool VerificateString(string stringData, string field)
        {
            Regex rgx = new Regex(@"^[a-zA-ZÀ-ÿ\u00f1\u00d1\s]+$");
            if (rgx.IsMatch(stringData))
                return true;
            else
            {
                CustomMessageBox.ShowOK("Asegurese de que el campo '" + field + "' solo contenga datos alfabéticos", "Error campo inválido", "Aceptar");
                return false;
            }
        }

        public static bool VerificateDate(string date, string field)
        {
            Regex rgx = new Regex(@"^(0[1-9]|[12][0-9]|3[01])[- /.](0[1-9]|1[012])[- /.](19|20)\d\d$");
            if (rgx.IsMatch(date))
                return true;
            else
            {
                CustomMessageBox.ShowOK("Asegurese de que el campo '" + field + "' contenga unicamente una fecha valida con el formato (dd/mm/aaaa)", "Error fecha inválido", "Aceptar");
                return false;
            }
        }

        public static bool VerificateDecimal(string date, string field)
        {
            Regex rgx = new Regex(@"^[0-9]$||^[0-9]\.[0-9]$");
            if (rgx.IsMatch(date))
                return true;
            else
            {
                CustomMessageBox.ShowOK("Asegurese de que el campo '" + field + "' contenga unicamente valores numericos enteros o decimales", "Error fecha inválido", "Aceptar");
                return false;
            }
        }
    }
}
