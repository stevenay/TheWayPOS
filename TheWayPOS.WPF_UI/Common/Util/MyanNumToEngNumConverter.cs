using System;
using System.Windows.Data;

namespace TheWayPOS.WPF_UI.Common.Util
{
    public class MyanNumToEngNumConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return value;

            return MyantoEng(value.ToString());
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return value;

            return MyantoEng(value.ToString());
        }

        public static string MyantoEng(string MyanmarNumber)
        {
            char[] myanNum = { '၀', '၁', '၂', '၃', '၄', '၅', '၆', '၇', '၈', '၉' };
            char[] engNum = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

            for (int i = 0; i < myanNum.Length; i++)
            {
                MyanmarNumber = MyanmarNumber.Replace(myanNum[i], engNum[i]);
            }

            return MyanmarNumber;
        }

        public static string EngtoMyan(string EngNumber)
        {
            char[] myanNum = { '၀', '၁', '၂', '၃', '၄', '၅', '၆', '၇', '၈', '၉' };
            char[] engNum = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

            for (int i = 0; i < engNum.Length; i++)
            {
                EngNumber = EngNumber.Replace(engNum[i], myanNum[i]);
            }

            return EngNumber;
        }
    }
}
