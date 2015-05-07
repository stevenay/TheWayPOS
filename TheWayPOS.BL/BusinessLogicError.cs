using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheWayPOS.BL
{
    public enum BusinessLogicErrorType
    {
        Required,
        Chosen,
        Invalid,
        NonExist,
        GreaterThan,
        LessThan,
        Between
    }

    public class BusinessLogicError
    {
        public string Property { get; set; }
        public string Message { get; set; }
        public int MinimumValue { get; set; }
        public int MaximumValue { get; set; }
        public BusinessLogicErrorType ErrorType { get; set; }
    }
}
