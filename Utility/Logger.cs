using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web;

namespace SSPWebUI.Utility
{
    public class Logger
    {
        private string sLogFormat;
        private string sErrorTime;
        private string sFileName;
        public Logger()
        {

            //sLogFormat used to create log files format :
            // dd/mm/yyyy hh:mm:ss AM/PM ==> Log Message
            sLogFormat = DateTime.Now.ToShortDateString().ToString() + " " + DateTime.Now.ToLongTimeString().ToString() + " ==> ";

            //this variable used to create log filename format "
            //for example filename : ErrorLogYYYYMMDD
            string sYear = DateTime.Now.Year.ToString();
            string sMonth = DateTime.Now.Month.ToString();
            string sDay = DateTime.Now.Day.ToString();
            sErrorTime = sYear + sMonth + sDay;

            sFileName=HttpContext.Current.Server.MapPath("\\Logs") + "\\Log_" + sErrorTime + ".txt";
            StreamWriter writer = new StreamWriter(sFileName);
            
        }

        public void Write(string Message)
        {
            StreamWriter sw = new StreamWriter(sFileName, true);
            sw.WriteLine(sLogFormat + Message);
            sw.Flush();
            sw.Close();
        }
    }
}