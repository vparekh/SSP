using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
namespace Output
{
    public class TextHelper
    {
        public static string GetBoundedText(string startText, string endText, string Text, ref int iStart, ref int iEnd)
        {
            iStart = Text.IndexOf(startText);
            if (iStart < 0)
                return "";
            iEnd = Text.IndexOf(endText);
            if (iEnd < 0)
                return "";

            string retval = Text.Substring(iStart, iEnd - iStart + endText.Length);

            return retval;

        }
    }
}
