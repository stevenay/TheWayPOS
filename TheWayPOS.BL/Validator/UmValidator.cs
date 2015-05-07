using System;
using System.Collections.Generic;
using System.Linq;
using TheWayPOS.DAL;
using TheWayPOS.DAL.GenericInterfaces;
using Entities = TheWayPOS.Entities;

namespace TheWayPOS.BL.Validator
{
    public class UmValidator : IValidator<Entities.Um>
    {
        Entities.Um _entity;

        public UmValidator(Entities.Um entity)
        {
            _entity = entity;
        }

        static readonly string[] ValidatedProperties = 
        {
            "UmShortName",
            "UmFullName",
            "DisposedUmCode",
            "DisposedUmQuantity"
        };

        public string ValidateProperty(string umProperty, object val)
        {
            if (Array.IndexOf(ValidatedProperties, umProperty) < 0)
                return null;

            string error = null;

            switch (umProperty)
            {
                case "UmShortName":
                    error = this.ValidateUmShortName(val);
                    break;
                case "UmFullName":
                    error = this.ValidateUmFullName(val);
                    break;
                case "DisposedUmCode":
                    error = this.ValidateDisposedUmCode(val);
                    break;
                case "DisposedUmQuantity":
                    error = this.ValidateDisposedUmQuantity(val);
                    break;
                default:
                    // Debug.Fail("Unexpected property being validated on Customer: " + propertyName);
                    break;
            }

            return error;
        }

        public bool ValidateEntity(Entities.Um p, ref List<BusinessLogicError> errors)
        {
            return true;
        }

        #region ValidateProperties BusinessLogic
        string ValidateUmShortName(object val)
        {
            string _value = (string)val;

            if (ValidationHelper.IsStringMissing(_value))
                return "ယူနစ် Short Name နာမည် ရိုက်ထည့်ပေးရန် လိုအပ်ပါသည်။";
            else
            {
                if (_entity != null && _entity.mode == Entities.Mode.Add)
                {
                    IDataManager dataManager = FactoryManager.Instance().GetRepositoryManager();
                    List<Entities.Um> valueFromDb = dataManager.UmRepo.Where(u => u.um_shortname == _value).ToList();

                    if (valueFromDb.Count > 0)
                        return "ယခုဖြည့်စွက်လိုက်သော ယူနစ် Short Name သည် System ထဲတွင်ရှိပြီးသား ဖြစ်ပါသည်။";
                }
            }

            return null;
        }
        string ValidateUmFullName(object val)
        {
            string _value = (string)val;

            if (ValidationHelper.IsStringMissing(_value))
                return "ယူနစ် Full Name နာမည် ရိုက်ထည့်ပေးရန် လိုအပ်ပါသည်။";
            else
            {
                if (_entity != null && _entity.mode == Entities.Mode.Add)
                {
                    IDataManager dataManager = FactoryManager.Instance().GetRepositoryManager();
                    List<Entities.Um> valueFromDb = dataManager.UmRepo.Where(u => u.um_fullname == _value).ToList();

                    if (valueFromDb.Count > 0)
                        return "ယခုဖြည့်စွက်လိုက်သော ယူနစ် Full Name သည် System ထဲတွင်ရှိပြီးသား ဖြစ်ပါသည်။";
                }
            }

            return null;
        }
        string ValidateDisposedUmCode(object val)
        {
            if (_entity.disposable)
            {
                if (val == null)
                {
                    return "Disposed Um ကို ရွေးချယ်ပေးပါ။";
                }
            }
            else
            {
                if (val != null)
                {
                    return "Disposed Um ကို ရွေးချယ်မည်ဆိုပါက Disposable ကိုပါ အမှန်ခြစ် ခြစ်ပေးပါ။";
                }
            }
            
            return null;
        }
        string ValidateDisposedUmQuantity(object val)
        {
            if (_entity.disposable)
            {
                if (val == null)
                {
                    return "Disposed Um Quantity ကို ရွေးချယ်ပေးပါ။";
                }
            }
            else
            {
                if (val != null)
                {
                    return "Disposed Um Quantity ကို ဖြည့်စွက်မည်ဆိုပါက Disposable ကိုပါ အမှန်ခြစ် ခြစ်ပေးပါ။";
                }
            }

            return null;
        }
        #endregion
    }
}