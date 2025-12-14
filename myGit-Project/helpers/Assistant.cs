using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace FileToText_WPF.helpers
{
    public static class Assistant
    {
        public static (bool success, string name) IsAnyButtonsSelected(params RadioButton[] buttons)
        {
            var radioButton = buttons.FirstOrDefault(rd => rd.IsChecked.Value);
            if (radioButton != null)
            {
                return (true, radioButton.Name ?? string.Empty);
            }
            else
            {
                return (false, string.Empty);
            }
        }
        public static string GetNameOfSelectedRadio(List<RadioButton> radioButtonList)
        {
            string radioButtonName = "";
            foreach (var radioButton in radioButtonList)
            {
                if (radioButton.IsChecked.Value)
                {
                    radioButtonName = radioButton.Name;
                }
            }
            return radioButtonName;
        }
        public static string GetLangName(string lang)
        {
           
            var result = lang switch
            { 
                "English" => "eng",
                "Persian" => "fas",
                "Italy" => "ita",
                "Spanish" => "spa",
                "Arabic" => "ara"
            };
            
            return result;
        }
        public static string FixPersianText(string input)
        {

            try
            {
                var lines = input.Split('\n');
                StringBuilder fixedText = new StringBuilder();

                foreach (var line in lines)
                {
                    string trimmed = line.TrimEnd('\r'); // حذف کاراکترهای اضافی ویندوز
                    char[] chars = trimmed.ToCharArray();
                    Array.Reverse(chars);
                    string reversedLine = new string(chars);

                    reversedLine = reversedLine
                        .Replace('ي', 'ی')
                        .Replace('ك', 'ک');

                    fixedText.AppendLine(reversedLine);
                }
                return fixedText.ToString();
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }

          
        }
        public static bool ContainsPersian(string text)
        {

            foreach (char c in text)
            {
                if ((c >= 0x0600 && c <= 0x06FF) || // Arabic
                    (c >= 0x0750 && c <= 0x077F) || // Arabic Supplement
                    (c >= 0x08A0 && c <= 0x08FF) || // Arabic Extended-A
                    (c >= 0xFB50 && c <= 0xFDFF) || // Arabic Presentation Forms-A
                    (c >= 0xFE70 && c <= 0xFEFF))   // Arabic Presentation Forms-B
                {
                    return true;
                }
            }
            return false;
        }
    }
}
