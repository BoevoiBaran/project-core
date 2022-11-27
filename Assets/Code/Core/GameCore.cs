using System.Collections;
using Code.Constants;
using Code.Core.Managers;
using Code.Core.Managers.Perks;
using Code.Core.Services;
using UnityEngine;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Code.Core
{
    public class GameCore : MonoBehaviour
    {
        [SerializeField] private UICore uiCore;
    
        private IServicesAggregator _services;
        private IManagersAggregator _managers;

        private void Awake()
        {
            uiCore.ShowLoading();
            DontDestroyOnLoad(this);
        }
        
        private IEnumerator Start()
        {
            yield return InitServices();
            yield return InitManagers();
            
            var assetService = _services.GetService<IAssetService>(AssetService.SERVICE_NAME);
            assetService.GetSceneAsync(ScenesAddressableId.META_SCENE, OnMetaSceneLoaded);
        }
        private IEnumerator InitServices()
        {
            var isServiceAggregatorInitialized = false;
            var services = new ServicesAggregator();
            services
                .AddService(AssetService.SERVICE_NAME, new AssetService())
                .AddService(CoroutineService.SERVICE_NAME, new CoroutineService())
                .AddService(DataStorageService.SERVICE_NAME, new DataStorageService());
            services.Init(ServiceAggregatorInitialized);
            _services = services;

            while (!isServiceAggregatorInitialized)
            {
                yield return 0;
            }
        
            void ServiceAggregatorInitialized()
            {
                isServiceAggregatorInitialized = true;
            }
        }
        private IEnumerator InitManagers()
        {
            var isManagersAggregatorInitialized = false;
            var managers = new ManagersAggregator();
            managers
                .AddManager(PerksManager.MANAGER_NAME, new PerksManager())
                .AddManager(PerksPointManager.MANAGER_NAME, new PerksPointManager());
            managers.Init(_services, ManagersAggregatorInitialized);
            _managers = managers;

            while (!isManagersAggregatorInitialized)
            {
                yield return 0;
            }
        
            void ManagersAggregatorInitialized()
            {
                isManagersAggregatorInitialized = true;
            }
        }
        
        private MetaCore _metaCore;
        private void OnMetaSceneLoaded(SceneInstance sceneInstance)
        {
            SceneManager.SetActiveScene(sceneInstance.Scene);
            _metaCore = GameObject.FindObjectOfType<MetaCore>();
        
            StartCoroutine(InitMetaCore());
        }

        IEnumerator InitMetaCore()
        {
            yield return _metaCore.Init(_services, _managers, uiCore.Meta);
            uiCore.HideLoading();
        }
        
        private void OnApplicationQuit()
        {
            if (_metaCore)
            {
                _metaCore.Dispose();
            }
        }
    }
}
