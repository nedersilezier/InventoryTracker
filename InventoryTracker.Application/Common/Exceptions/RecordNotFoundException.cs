using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryTracker.Application.Common.Exceptions
{
    public class RecordNotFoundException: Exception
    {
        public RecordNotFoundException(string name, object key)
            : base($"{name} with id {key} was not found.") 
        { 
        }
    }
}
