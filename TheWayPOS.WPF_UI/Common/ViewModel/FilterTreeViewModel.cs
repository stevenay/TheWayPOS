using System;
using DevExpress.Data.Filtering;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm.POCO;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using TheWayPOS.WPF_UI.Service;

namespace TheWayPOS.WPF_UI.Common.ViewModel {
    public interface ISupportFiltering<TEntity> where TEntity : class {
        int GetCount(CriteriaOperator filterCriteria);
        ICustomFilterContainerService CustomFilterContainerService { get; }
        FilterTreeViewModel<TEntity> FilterTreeViewModel { get; set; }
    }

    public class FilterTreeViewModel<T> : FilterTreeViewModelBase, IFilterTreeViewModel where T : class {
        ISupportFiltering<T> viewModel;
        public FilterTreeViewModel(IFilterTreeModelPageSpecificSettings settings)
            : base(settings) {
            Updated = (o, e) => { };
            Init();
            if(SelectedItem != null) {
                ((FilterItem)SelectedItem).IsSelected = true;
            }
            FilterClosed = true;
        }

        [ServiceProperty(Key = "FilterDialogService"), Required]
        public virtual IDialogService FilterDialogService { get { return null; } }
        public virtual string StaticCategoryName { get; set; }
        public virtual bool FilterClosed { get; set; }
        public virtual FilterItem ActiveFilterItem { get; set; }
        public virtual string StaticFilterName { get; set; }
        public event EventHandler ActiveFilterChanged;
        public event EventHandler<EventArgs> Updated;

        public void Update() {
            Updated(this, EventArgs.Empty);
        }

        public void ChangeViewModel(ISupportFiltering<T> viewModel) {
            viewModel.FilterTreeViewModel = this;
            this.viewModel = viewModel;
            Updated(this, EventArgs.Empty);
        }

        [Command]
        public void ShowFilterFromGrid(object untyped) {
            FilterItem clone = ActiveFilterItem.Clone();
            FilterClosed = false;
            if(!CriteriaOperator.Equals(clone.FilterCriteria, null) && !clone.FilterCriteria.Equals(SelectedItem.FilterCriteria)) {
                clone.FilterCriteria = CriteriaOperator.Clone(SelectedItem.FilterCriteria);
            }
            var filterViewModel = CreateCustomFilterViewModel(clone, false);
            ShowFilter(filterViewModel, () => base.AddNewCustomFilter(clone));
        }

        public void ShowFilter(CustomFilterViewModel filterViewModel, Action onSave) {
            viewModel.CustomFilterContainerService.UpdateFilterColumns(filterViewModel);
            var result = FilterDialogService.ShowDialog(MessageBoxButton.OKCancel, "Create Custom Filter", "CustomFilterView", filterViewModel);
            switch (result) {
                case MessageBoxResult.OK:
                    filterViewModel.ApplyFilter();
                    FilterClosed = true;
                    ActiveFilterItem = ((FilterItem)filterViewModel.FilterItem).Clone();
                    if(filterViewModel.Save) {
                        onSave();
                        Updated(this, EventArgs.Empty);
                    }
                    break;
                case MessageBoxResult.Cancel:
                    FilterClosed = true;
                    break;
            }
        }

        public override void ModifyCustomFilter(FilterItemBase existing) {
            var filterViewModel = CreateCustomFilterViewModel(existing, true);
            ShowFilter(filterViewModel, () => base.ModifyCustomFilter(existing));
        }

        CustomFilterViewModel CreateCustomFilterViewModel(FilterItemBase existing, bool save) {
            return ViewModelSource.Create(() => new CustomFilterViewModel { FilterItem = existing, Save = save }).SetParentViewModel(this);
        }

        [Command]
        public void CreateCustomFilter() {
            FilterItemBase filterItem = ViewModelSource.Create(() => new FilterItem(this, this, "", null));
            var filterViewModel = ViewModelSource.Create(() => new CustomFilterViewModel { FilterItem = filterItem, Save = true });
            ShowFilter(filterViewModel, () => AddNewCustomFilter(filterItem));
        }

        public override FilterItemBase CreateFilterItem(string name, CriteriaOperator filterCriteria) {
            return ViewModelSource.Create(() => new FilterItem(this, this, name, filterCriteria));
        }

        public override FilterItemBase CreateStaticFilterItem(string name, CriteriaOperator filterCriteria) {
            return ViewModelSource.Create(() => new StaticFilterItem(this, this, name, filterCriteria));
        }

        public int GetEntityCount(CriteriaOperator criteria) {
            return viewModel != null ? viewModel.GetCount(criteria) : 0;
        }

        public virtual void OnSelectedItemChanged() {
            ActiveFilterItem = ((FilterItem)SelectedItem).Clone();
            if(ActiveFilterChanged != null) {
                ActiveFilterChanged(this, null);
            }
        }

        public void ResetToAll() {
            SelectedItem = StaticFilters[0];
            ((FilterItem)SelectedItem).IsSelected = true;
        }
    }
    public interface IFilterTreeViewModel {
        event EventHandler<EventArgs> Updated;
        int GetEntityCount(CriteriaOperator criteria);
    }
    public class FilterItem : FilterItemBase  {
        FilterTreeViewModelBase parent;
        IFilterTreeViewModel entityCountGetter;
        // Func<T, bool> predicate;

        public FilterItem(FilterTreeViewModelBase parent, IFilterTreeViewModel entityCountGetter, string name, CriteriaOperator filterCriteria) {
            this.Name = name;
            OnIsSelectedChanged();
            this.FilterCriteria = filterCriteria;
            this.parent = parent;
            this.entityCountGetter = entityCountGetter;
            entityCountGetter.Updated += (s, e) => OnNameChanged();
        }

        public virtual bool IsSelected { get; set; }

        public virtual string DisplayText { get { return string.Format("{0} ({1})", Name, EntitiesCount); } }
        public virtual int EntitiesCount { get { return entityCountGetter.GetEntityCount(FilterCriteria); } }

        public void OnNameChanged() {
            this.RaisePropertyChanged(fi => fi.EntitiesCount);
            this.RaisePropertyChanged(fi => fi.DisplayText);
        }

        public void OnIsSelectedChanged() {
            if(IsSelected) {
                parent.SelectedItem = this;
            }
        }

        [Command]
        public void Modify() {
            parent.ModifyCustomFilter(this);
        }

        [Command]
        public void Delete() {
            parent.DeleteCustomFilter(this);
        }

        [Command]
        public void Duplicate() {
            parent.DuplicateFilter(this);
        }

        public FilterItem Clone() {
            return ViewModelSource.Create(() => new FilterItem(parent, entityCountGetter, (string)Name.Clone(), CriteriaOperator.Clone(FilterCriteria)));
        }
    }
    public class StaticFilterItem : FilterItem
    {
        public StaticFilterItem(FilterTreeViewModelBase parent, IFilterTreeViewModel entityCountGetter, string name, CriteriaOperator filterCriteria)
            : base(parent, entityCountGetter, name, filterCriteria) { }
    }
}
