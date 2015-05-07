using System;
using System.Collections.Generic;
using System.Linq;
using TheWayPOS.DAL;
using TheWayPOS.DAL.GenericInterfaces;
using MyModel = TheWayPOS.Entities;

namespace TheWayPOS.BL
{
    public class BusinessLogicManagerBase
    {
        #region Properties

        protected bool _contextAtomic;
        protected IDataManager _dataManager;

        #endregion

        #region Constructors

        /// <summary>
        /// Set true to ContextAtomic Parameter if you want to instantly dispose the Context Vairable
        /// Set false to ContextAtomic Parameter if you want to keep your Context Alive throughout the class Life
        /// </summary>
        /// <param name="contextAtomic"></param>
        public BusinessLogicManagerBase()
        {
            _contextAtomic = false;
            if (!_contextAtomic)
            {
                _dataManager = FactoryManager.Instance().GetRepositoryManager();
            }
        }

        #endregion

        #region Atomic Methods

        public void SetAtomic(bool contextAtomic)
        {
            _contextAtomic = contextAtomic;

            if (!_contextAtomic && _dataManager.IsAlreadyDisposed)
            {
                _dataManager = FactoryManager.Instance().GetRepositoryManager();
            }
            else if (_contextAtomic)
            {
                _dataManager.Dispose();
            }
        }

        public void FinishBusinessTransaction()
        {
            // It said that DbContext does not need to be Disposed
            // according to this link: http://blog.jongallant.com/2012/10/do-i-have-to-call-dispose-on-dbcontext.html
            //if (_dataManager.IsAlreadyDisposed)
            //    return;
            //else
            //    SetAtomic(true);

            _dataManager = FactoryManager.Instance().GetRepositoryManager();
            int result = _dataManager.SaveChanges();
        }

        #endregion
    }
}
