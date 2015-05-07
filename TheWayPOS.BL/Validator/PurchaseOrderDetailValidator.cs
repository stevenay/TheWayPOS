using System;
using System.Collections.Generic;
using System.Linq;
using TheWayPOS.DAL;
using TheWayPOS.DAL.GenericInterfaces;
using Entities = TheWayPOS.Entities;

namespace TheWayPOS.BL.Validator
{
    // at start, I consider these logic can bind to PurchaseOrderValidator
    // but, entity is included in Validator
    // so One Validator for One Business Entity (or ViewModel) is good
    public class PurchaseOrderDetailValidator : IValidator<Entities.PurchaseOrderDetail>
    {
        Entities.PurchaseOrderDetail _entity;

        public PurchaseOrderDetailValidator(Entities.PurchaseOrderDetail entity)
        {
            this._entity = entity;
        }

        static readonly string[] ValidatedProperties = 
        { 
            "BuyingPrice"
        };

        public string ValidateProperty(string purchaseOrderDetailProperty, object val)
        {
            if (Array.IndexOf(ValidatedProperties, purchaseOrderDetailProperty) < 0)
                return null;

            string error = null;

            switch (purchaseOrderDetailProperty)
            {
                case "BuyingPrice":
                    error = this.ValidateBuyingPrice(val);
                    break;
                default:
                    // Debug.Fail("Unexpected property being validated on Customer: " + propertyName);
                    break;
            }

            return error;
        }
        public bool ValidateEntity(Entities.PurchaseOrderDetail p, ref List<BusinessLogicError> errors)
        {
            return true;
        }

        #region BusinessLogicValidation_Methods

        string ValidateBuyingPrice(object val)
        {
            int i;
            if (val != null) 
            {
                if (int.TryParse(val.ToString(), out i))
                {
                    if (i < 0)
                    {
                        return "Buying Price ပမာဏ သည် သုည ထက်လျော့နည်းခွင့် မရှိပါ။";
                    }
                }
                else
                {
                    return "Buying Price သည် ကိန်းဂဏန်း တစ်ခုခု ဖြစ်ရပါမည်။";
                }
            } 
            else 
            {
                return "Buying Price ရိုက်ထည့်ပေးရန် လိုအပ်ပါသည်။";
            }

            return null;
        }

        #endregion
    }
}
