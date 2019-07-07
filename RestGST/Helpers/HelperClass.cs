using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestGST.Helpers
{
    public class HelperClass
    {

        public static bool CheckBothMarkupsExist(string textToBeVerified, string markupValue)
        {
            string markupStart = "<" + markupValue + ">";
            string markupEnd = "</" + markupValue + ">";
            if (textToBeVerified.Contains(markupStart) && textToBeVerified.Contains(markupEnd))
            {
                return true;
            }
            else if (!textToBeVerified.Contains(markupStart) && !textToBeVerified.Contains(markupEnd))
            {
                return true;
            }
            else if (textToBeVerified.Contains(markupStart) && !textToBeVerified.Contains(markupEnd))
            {
                return false;
            }
            else if (!textToBeVerified.Contains(markupStart) && textToBeVerified.Contains(markupEnd))
            {
                return false;
            }
            return false;
        }

        public static string GetTextBetweenMarkups(string text, string markupValue)
        {
            string markupStart = "<" + markupValue + ">";
            string markupEnd = "</" + markupValue + ">";
            int Start, End;
            if (text.Contains(markupStart) && text.Contains(markupEnd))
            {
                Start = text.IndexOf(markupStart, 0) + markupStart.Length;
                End = text.IndexOf(markupEnd, Start);
                return text.Substring(Start, End - Start);
            }
            else
            {
                return "";
            }
        }

        public static string GetFormattedErrorMessage(string value)
        {
            return string.Format("Error : Message cannot be processed. Missing {0} markup field.", value);
        }
    }
}