using System;
using System.Linq.Expressions;
using Data = DevExpress.Data;

namespace TheWayPOS.WPF_UI.Common.Util
{
    public class CriteriaOperatorToExpressionConverter<T>
    {
        public Expression<Func<T, bool>> Convert(Data.Filtering.CriteriaOperator criteria)
        {
            if (object.ReferenceEquals(criteria, null))
                return null;
            try
            {
                var parameter = Expression.Parameter(typeof(T));
                var converter = new Data.Linq.Helpers.CriteriaToExpressionConverterInternal(parameter);
                Expression body = converter.Process(criteria);
                if (body == null)
                    return null;

                return Expression.Lambda<Func<T, bool>>(body, parameter);
            }
            catch { return null; }
        }
    }
}
