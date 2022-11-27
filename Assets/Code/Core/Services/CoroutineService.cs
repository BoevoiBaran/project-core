using System;
using System.Collections;
using UnityEngine;

namespace Code.Core.Services
{
    public interface ICoroutineService
    {
        public Coroutine StartCoroutine(IEnumerator routine);
    }

    public class CoroutineService : BaseService, ICoroutineService
    {
        public const string SERVICE_NAME = nameof(CoroutineService);

        private MonoBehaviour _coroutinesOwner;

        public override void Init(IServicesAggregator services, Action initializedCb)
        {
            var go = new GameObject(SERVICE_NAME);
            _coroutinesOwner = go.AddComponent<CoroutineServiceComponent>();
            UnityEngine.Object.DontDestroyOnLoad(go);
            initializedCb?.Invoke();
        }

        #region ICoroutineService

        public Coroutine StartCoroutine(IEnumerator routine)
        {
            return _coroutinesOwner.StartCoroutine(routine);
        }

        #endregion
    }
    
    public class CoroutineServiceComponent : MonoBehaviour {}
}