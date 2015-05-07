using System;
using System.Collections.Generic;
using System.Linq;
using TheWayPOS.DAL;
using TheWayPOS.DAL.GenericInterfaces;
using Entities = TheWayPOS.Entities;

namespace TheWayPOS.BL.Validator
{
    public class PurchaseOrderValidator : IValidator<Entities.PurchaseOrderHeader>
    {
        Entities.PurchaseOrderHeader _entity;
        public PurchaseOrderValidator(Entities.PurchaseOrderHeader entity)
        {
            _entity = entity;
        }

        static readonly string[] ValidatedProperties = 
        {
        };

        public string ValidateProperty(string pohProperty, object val)
        {
            if (Array.IndexOf(ValidatedProperties, pohProperty) < 0)
                return null;

            string error = null;

            //switch (pohProperty)
            //{
            //    case "ProductName":
            //        error = this.ValidateProductName(val);
            //        break;
            //    case "DiscountPercentage":
            //        error = this.ValidateDiscountPercentage(val);
            //        break;
            //    case "SupplierCode":
            //        error = this.ValidateSupplierCode(val);
            //        break;
            //    default:
            //        // Debug.Fail("Unexpected property being validated on Customer: " + propertyName);
            //        break;
            //}

            return error;
        }
        public bool ValidateEntity(Entities.PurchaseOrderHeader poh, ref List<BusinessLogicError> errors)
        {
            return true;
        }
    }
}
