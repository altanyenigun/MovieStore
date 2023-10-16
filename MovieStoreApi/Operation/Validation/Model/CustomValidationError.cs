using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieStoreApi.Operation.Validation.Model
{
    public class CustomValidationError
    {
        public string PropertyName { get; set; }
        public string ErrorMessage { get; set; }
    }
}