using System;
using DevExpress.Data.Filtering;

namespace TheWayPOS.WPF_UI.Service {
    public interface IActualFilterCriteriaService {
        CriteriaOperator ActualFilterCriteria { get; }
    }
}
