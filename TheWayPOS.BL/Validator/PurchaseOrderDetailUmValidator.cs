using System;
using System.Collections.Generic;
using Entities = TheWayPOS.Entities;

namespace TheWayPOS.BL.Validator
{
    public class PurchaseOrderDetailUmValidator : IValidator<Entities.PurchaseOrderDetail_Ums>
    {
        Entities.PurchaseOrderDetail_Ums _entity;
        static readonly string[] ValidatedProperties = { };

        public PurchaseOrderDetailUmValidator(Entities.PurchaseOrderDetail_Ums entity)
        {
            _entity = entity;
        }

        public string ValidateProperty(string propertyName, object val)
        {
            return null;
        }
        public bool ValidateEntity(Entities.PurchaseOrderDetail_Ums pu, ref List<BusinessLogicError> errors)
        {
            return false;
        }
    }
}
