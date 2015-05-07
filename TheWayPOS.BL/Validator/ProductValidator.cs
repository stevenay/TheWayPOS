using System;
using System.Collections.Generic;
using System.Linq;
using TheWayPOS.DAL;
using TheWayPOS.DAL.GenericInterfaces;
using Entities = TheWayPOS.Entities;

namespace TheWayPOS.BL.Validator
{
    public class ProductValidator : IValidator<Entities.Product>
    {
        Entities.Product _entity;
        public ProductValidator(Entities.Product entity)
        {
            _entity = entity;
        }

        static readonly string[] ValidatedProperties = 
        { 
            "ProductName",
            "DiscountPercentage",
            "SupplierCode"
        };

        public string ValidateProperty(string productProperty, object val)
        {
            if (Array.IndexOf(ValidatedProperties, productProperty) < 0)
                return null;

            string error = null;

            switch (productProperty)
            {
                case "ProductName":
                    error = this.ValidateProductName(val);
                    break;
                case "DiscountPercentage":
                    error = this.ValidateDiscountPercentage(val);
                    break;
                case "SupplierCode":
                    error = this.ValidateSupplierCode(val);
                    break;
                default:
                    // Debug.Fail("Unexpected property being validated on Customer: " + propertyName);
                    break;
            }

            return error;
        }
        public bool ValidateEntity(Entities.Product p, ref List<BusinessLogicError> errors)
        {   
            return true;

            // check product category actually exist in the database
            // I think it's overkill feature, so I will comment it out
            //FactoryManager fm = new FactoryManager();
      //var pc = dm.GetProductCategoryManager().Get(p.product_category_code ?? 0);
            //if (pc == null)
            //{
            //    errors.Add("System does not support the Product Category that you have chosen.");
            //}
        }

        #region BusinessLogicValidation_Methods

        string ValidateProductName(object val)
        {
            string _value = (string)val;

            if (ValidationHelper.IsStringMissing(_value))
                return "Product နာမည် ရိုက်ထည့်ပေးရန် လိုအပ်ပါသည်။";
            else if (_value.Length <= 1)
                return "Product နာမည်သည် အလွန်တိုလွန်းနေပါသည်။";
            else
            {
                if (_entity != null && _entity.mode == Entities.Mode.Add)
                {
                    IDataManager dataManager = FactoryManager.Instance().GetRepositoryManager();
                    List<Entities.Product> valueFromDb = dataManager.ProductRepo.Where(p => p.product_name == _value).Where(p => p.supplier_code == _entity.supplier_code).ToList();

                    if (valueFromDb.Count > 0)
                        return "ယခုဖြည့်စွက်လိုက်သော Product နှင့် ရွေးချယ်ထားသော Company သည် System ထဲတွင် တွဲလျက်ရှိပြီးသား ဖြစ်ပါသည်။";
                }
            }

            return null;
        }
        string ValidateSupplierCode(object val)
        {
            int _value = (int)val;

            if (_entity != null && _entity.mode == Entities.Mode.Add)
            {
                IDataManager dataManager = FactoryManager.Instance().GetRepositoryManager();
                List<Entities.Product> valueFromDb = dataManager.ProductRepo.Where(p => p.product_name == _entity.product_name).Where(p => p.supplier_code == _value).ToList();

                if (valueFromDb.Count > 0)
                    return "ယခုရွေးချယ်လိုက်သော Company နှင့် ဖြည့်ထားသော Product နာမည်သည် System ထဲတွင် တွဲလျက်ရှိပြီးသား ဖြစ်ပါသည်။";
            }

            return null;
        }
        string ValidateDiscountPercentage(object val)
        {
            decimal _value = (decimal)val;

            if (_value > 100)
                return "Discount Percentage ပမာဏ သည် 100 ထက်ကျော်လွန်ခွင့် မရှိပါ။";
            else if (_value <= 0)
                return "Discount Percentage ပမာဏ သည် သုည ထက်လျော့နည်းခွင့် မရှိပါ။";

            return null;
        }

        #endregion
    }
}
