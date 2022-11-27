using System;
using System.Collections.Generic;
using ThirdParty;
using UnityEngine;

namespace Code.Core.Services
{
    public interface IServicesAggregator
    {
        T GetService<T>(string serviceName);
    }

    public abstract class BaseService
    {
        public abstract void Init(IServicesAggregator services, Action initializedCb);
    }

    public class ServicesAggregator : IServicesAggregator
    {
        private readonly Dictionary<string, BaseService> _services = new Dictionary<string, BaseService>();

        public ServicesAggregator AddService(string name, BaseService baseService)
        {
            _services[name] = baseService;
            return this;
        }
        
        private event Action Initialized;
        private int _leftInitializeServicesCount;
        public void Init(Action initializedCb)
        {
            Initialized = initializedCb;
            _leftInitializeServicesCount = _services.Count;
            foreach (var (_, service) in _services)
            {
                service.Init(this, ServiceInitialized);
            }
        }

        private void ServiceInitialized()
        {
            _leftInitializeServicesCount--;
            if (_leftInitializeServicesCount > 0)
            {
                return;
            }
            
            Initialized?.Invoke();
            Initialized = null;
        }

        public T GetService<T>(string serviceName)
        {
            if (!_services.TryGetValue(serviceName, out var baseService))
            {
                Debug.LogError($"[ServicesAggregator] service: {serviceName} not found");
                return default;
            }

            if (!(baseService is T service))
            {
                Debug.LogError($"[ServicesAggregator] service: {serviceName} is not {typeof(T)}");
                return default;
            }

            return service;
        }
    }
}