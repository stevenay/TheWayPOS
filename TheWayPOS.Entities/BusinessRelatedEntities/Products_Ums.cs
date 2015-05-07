//------------------------------------------------------------------------------
// This entity is just intended for using in this application and this business logic.
// This entity is not associated with Data or Database or DataAccessLayer. 
//
// This code try to Extend Products_Ums Entity in Entities Class Library
//------------------------------------------------------------------------------

namespace TheWayPOS.Entities
{
    using System;
    using System.Collections.Generic;

    public partial class Products_Ums
    {
        public bool isApply { get; set; }
        public Mode mode { get; set; }
    }
}
