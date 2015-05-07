using System;
using System.Collections.Generic;
using System.Linq;
using TheWayPOS.DAL;
using TheWayPOS.DAL.GenericInterfaces;
using Entities = TheWayPOS.Entities;

namespace TheWayPOS.BL.Validator
{
    public class SupplierValidator : IValidator<Entities.Supplier>
    {
        Entities.Supplier _entity;

        public SupplierValidator(Entities.Supplier entity)
        {
            _entity = entity;
        }

        static readonly string[] ValidatedProperties = 
        { 
            "SupplierName"
        };

        public string ValidateProperty(string propertyName, object val)
        {
            if (Array.IndexOf(ValidatedProperties, propertyName) < 0)
                return null;

            string error = null;

            switch (propertyName)
            {
                case "SupplierName":
                    error = this.ValidateSupplierName(val);
                    break;

                default:
                    // Debug.Fail("Unexpected property being validated on Customer: " + propertyName);
                    break;
            }

            return error;
        }

        // This method is not used currently
        public bool ValidateEntity(Entities.Supplier s, ref List<BusinessLogicError> errors)
        {
            if (String.IsNullOrEmpty(s.supplier_name.Trim()))
            {
                errors.Add(new BusinessLogicError() { Property = "Supplier Name", ErrorType = BusinessLogicErrorType.Required });
            }

            if (errors.Count > 0)
            {
                return false;
            }

            return true;
        }

        private string ValidateSupplierName(object val)
        {
            string _value = (string)val;

            if (ValidationHelper.IsStringMissing(_value))
                return "ကုမ္ပဏီ (သို့) ဆိုင် နာမည်တစ်ခုခု ရိုက်ပေးပါ။.";
            else if (_value.Length <= 1)
                return "ကုမ္ပဏီ (သို့) ဆိုင် နာမည်သည် အလွန်တိုလွန်းနေပါသည်။";
            else
            {
                if (_entity != null)
                {
                    if (_entity.mode == Entities.Mode.Add)
                    {
                        IDataManager dataManager = FactoryManager.Instance().GetRepositoryManager();
                        List<Entities.Supplier> valueFromDb = dataManager.SupplierRepo.Where(s => s.supplier_name == _value).ToList();

                        if (valueFromDb.Count > 0)
                            return "ယခုဖြည့်စွက်လိုက်သော ကုမ္ပဏီနာမည်သည် System ထဲတွင် ရှိပြီးသား ဖြစ်ပါသည်။";
                    }
                }
            }

            return null;
        }


    }
}
