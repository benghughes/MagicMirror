using System;
using System.Collections.Generic;
using System.Text;

namespace MagicMirror.DataAccess.Entities.Traffic
{
    public class Element
    {
        public string Status { get; set; }
        public Duration Duration { get; set; }
        public Distance Distance { get; set; }
    }
}
