using System;
using DevExpress.Data.Filtering;

namespace TheWayPOS.WPF_UI.ViewModel
{
    public interface IActualFilterCriteriaService {
        CriteriaOperator ActualFilterCriteria { get; }
    }
}
