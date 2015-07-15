﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akron.Data.DataStructures
{
    public class MeasureDefinition
    {
        public AggregateOperations Operation { get; set; }
        public DataColumnMetadata Column { get; set; }

        public bool IsDefault { get; set; }
    }
}
