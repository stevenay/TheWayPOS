using System;
using System.Collections.Generic;
using Entities = TheWayPOS.Entities;

namespace TheWayPOS.BL.Validator
{
    public class ProductUmValidator : IValidator<Entities.Products_Ums>
    {
        // There is no need to check any Business Validation for this Entity Currently
        static readonly string[] ValidatedProperties = {};

        public string ValidateProperty(string propertyName, object val)
        {
            return null;
        }

        public bool ValidateEntity(Entities.Products_Ums pu, ref List<BusinessLogicError> errors)
        {
            return false;
        }
    }
}
