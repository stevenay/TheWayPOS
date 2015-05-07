namespace TheWayPOS.DAL
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DomainInterfaces;
    using EfDomainManagers;
    using GenericInterfaces;

    // In original version, it's FactoryManager
    public sealed class FactoryManager : IDisposable
    {
        #region Singleton_Implementation

        private static FactoryManager _instance = null;
        private static object lockObject = new object();

        // Singleton Service Provider
        public static FactoryManager Instance()
        {
            if (_instance == null) //lazy loading
            {
                // lock so that another thread cannot enter(access) 
                // the following code block 
                // while it's executing in current thread
                lock (lockObject)
                {
                    if (_instance == null)
                    {
                        _instance = new FactoryManager();
                    }
                }
            }
            return _instance;
        }

        private FactoryManager()
        {

        }

        #endregion

        private EntityFrameworkManager _efManager = null;
        public IDataManager GetRepositoryManager()
        {
            if (_efManager == null)
                _efManager = new EntityFrameworkManager();

            if (_efManager.IsAlreadyDisposed)
                _efManager.InitializeDbContext(); // reInitialized Db Context because it's already disposed

            return _efManager;
        }

        public void Dispose()
        {
            _efManager.Dispose();
        }
    }
}
