using System;
using System.Collections.Generic;
using System.Text;

namespace UtilityDAL.Model
{



    public class Log
    {
        public string Key { get; set; }

        public string Source { get; set; }

        public DateTime Date { get; set; }

        public Issue Issue { get; set; }

        public string Message { get; set; }
    }

    public enum Issue
    {
        Success,
        Information,
        Warning,
        Error
    }

}
