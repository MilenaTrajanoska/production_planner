using System;
using System.Collections.Generic;
using System.Text;

namespace ProductionPlanner.Domain.Exceptions
{
    public class ProductNotFoundException : Exception
    {
        public ProductNotFoundException(string message) : base(message)
        {
        }
    }
}
