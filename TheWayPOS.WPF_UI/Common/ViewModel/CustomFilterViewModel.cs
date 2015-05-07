using System;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Xpf.Editors.Filtering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TheWayPOS.WPF_UI.Service;

namespace TheWayPOS.WPF_UI.Common.ViewModel {
    public class CustomFilterViewModel {
        public virtual IEnumerable<FilterColumn> FilterColumns { get; set; }
        public virtual bool Save { get; set; }
        public virtual FilterItemBase FilterItem { get; set; }
        protected virtual IActualFilterCriteriaService ActualFilterCriteriaService { get { return null; } }
        public void ApplyFilter() {
            FilterItem.FilterCriteria = ActualFilterCriteriaService.ActualFilterCriteria;
        }
    }
}
