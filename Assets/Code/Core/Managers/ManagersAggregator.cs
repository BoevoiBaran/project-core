using System;
using System.Collections.Generic;
using Code.Core.Services;
using ThirdParty;
using UnityEngine;

namespace Code.Core.Managers
{
    public interface IManagersAggregator
    {
        T GetManager<T>(string managerName);
    }

    public abstract class BaseManager
    {
        public abstract void Init(IServicesAggregator services, Action initializedCb);
    }

    public class ManagersAggregator : IManagersAggregator
    {
        private readonly Dictionary<string, BaseManager> _managers = new Dictionary<string, BaseManager>();

        public ManagersAggregator AddManager(string name, BaseManager baseManager)
        {
            _managers[name] = baseManager;
            return this;
        }
        
        private event Action Initialized;
        private int _leftInitializeManagersCount;
        
        public void Init(IServicesAggregator services, Action initializedCb)
        {
            Initialized = initializedCb;
            _leftInitializeManagersCount = _managers.Count;
            foreach (var (_, manager) in _managers)
            {
                manager.Init(services, ManagerInitialized);
            }
        }

        private void ManagerInitialized()
        {
            _leftInitializeManagersCount--;
            if (_leftInitializeManagersCount > 0)
            {
                return;
            }
            
            Initialized?.Invoke();
            Initialized = null;
        }

        public T GetManager<T>(string managerName)
        {
            if (!_managers.TryGetValue(managerName, out var baseManager))
            {
                Debug.LogError($"[ManagersAggregator] manager: {managerName} not found");
                return default;
            }

            if (!(baseManager is T manager))
            {
                Debug.LogError($"[ManagersAggregator] manager: {managerName} is not {typeof(T)}");
                return default;
            }

            return manager;
        }
    }
}