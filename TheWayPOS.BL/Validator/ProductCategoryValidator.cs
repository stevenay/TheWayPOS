using System;
using System.Collections.Generic;
using System.Linq;
using TheWayPOS.DAL;
using TheWayPOS.DAL.GenericInterfaces;
using Entities = TheWayPOS.Entities;

namespace TheWayPOS.BL.Validator
{
    public class ProductCategoryValidator : IValidator<Entities.ProductCategory>
    {
        Entities.ProductCategory _entity;

        public ProductCategoryValidator(Entities.ProductCategory entity)
        {
            this._entity = entity;
        }

        static readonly string[] ValidatedProperties = 
        { 
            "CategoryName"
        };

        public string ValidateProperty(string productName, object val)
        {
            if (Array.IndexOf(ValidatedProperties, productName) < 0)
                return null;

            string error = null;

            switch (productName)
            {
                case "ProductName":
                    error = this.ValidateCategoryName(val);
                    break;
                default:
                    // Debug.Fail("Unexpected property being validated on Customer: " + propertyName);
                    break;
            }

            return error;
        }

        public bool ValidateEntity(Entities.ProductCategory p, ref List<BusinessLogicError> errors)
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

        string ValidateCategoryName(object val)
        {
            string _value = (string)val;

            if (ValidationHelper.IsStringMissing(_value))
                return "Please enter Category Name";
            else if (_value.Length <= 1)
                return "Category နာမည်သည် အလွန်တိုလွန်းနေပါသည်။";
            else
            {
                if (_entity != null)
                {
                    if (_entity.mode == Entities.Mode.Add)
                    {
                        IDataManager dataManager = FactoryManager.Instance().GetRepositoryManager();
                        List<Entities.ProductCategory> valueFromDb = dataManager.ProductCategoryRepo.Where(pc => pc.category_name == _value).ToList();

                        if (valueFromDb.Count > 0)
                            return "ယခုဖြည့်စွက်လိုက်သော ပစ္စည်းအမျိုးအစားသည် System ထဲတွင် ရှိပြီးသား ဖြစ်ပါသည်။";
                    }
                }
            }

            return null;
        }

        #endregion
    
    }
}
