using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Code.Core.Services
{
    public interface IAssetService
    {
        void GetAssetAsync<T>(string id, Action<T> cb);
        void GetTextAssetAsync(string id, Action<TextAsset> cb);
        void GetSceneAsync(string id, Action<SceneInstance> cb, LoadSceneMode loadMode = LoadSceneMode.Single,
            bool activateOnLoad = true);
    }

    public class AssetService : BaseService, IAssetService
    {
        public const string SERVICE_NAME = nameof(AssetService);
        
        public override void Init(IServicesAggregator services, Action initializedCb)
        {
            initializedCb?.Invoke();
        }
        
        #region IAssetService
    
        public void GetAssetAsync<T>(string id, Action<T> cb)
        {
            var loadAssetAsync = Addressables.LoadAssetAsync<GameObject>(id);
            loadAssetAsync.Completed += (operation) =>
            {
                if (operation.IsDone)
                {
                    var result = operation.Result;
                    cb?.Invoke(result.GetComponent<T>());
                }
            };
        }

        public void GetTextAssetAsync(string id, Action<TextAsset> cb)
        {
            var loadAssetAsync = Addressables.LoadAssetAsync<TextAsset>(id);
            loadAssetAsync.Completed += (operation) =>
            {
                if (operation.IsDone)
                {
                    var result = operation.Result;
                    cb?.Invoke(result);
                }
            };
        }

        public void GetSceneAsync(string id, Action<SceneInstance> cb, LoadSceneMode loadMode = LoadSceneMode.Single, bool activateOnLoad = true)
        {
            var loadAssetAsync = Addressables.LoadSceneAsync(id, loadMode, activateOnLoad);
            loadAssetAsync.Completed += (operation) =>
            {
                if (operation.IsDone)
                {
                    var result = operation.Result;
                    cb?.Invoke(result);
                }
            };
        }

        #endregion
    }
}