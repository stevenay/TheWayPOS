using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheWayPOS.BL
{
    public interface IValidator<T> where T : class
    {
        string ValidateProperty(string propertyName, object propertyVal);
        bool ValidateEntity(T entity, ref List<BusinessLogicError> errors);
    }
}
