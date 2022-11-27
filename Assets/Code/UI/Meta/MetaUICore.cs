using System.Collections;
using Code.Constants;
using Code.Core.Managers;
using Code.Core.Services;
using Code.UI.Meta.PerksTree;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Code.UI.Meta
{
    public class MetaUICore : MonoBehaviour
    {
        [SerializeField] private GameObject root;

        private PerksTreeDialog _perksTreeDialog;
        public IEnumerator Init(IServicesAggregator servicesAggregator, IManagersAggregator managersAggregator)
        {
            yield return LoadAllDialogs(servicesAggregator, managersAggregator);
            root.SetActive(true);
        }

        private IEnumerator LoadAllDialogs(IServicesAggregator servicesAggregator, IManagersAggregator managersAggregator)
        {
            var isDialogsInitialized = false;
            
            var assetService = servicesAggregator.GetService<IAssetService>(AssetService.SERVICE_NAME);
            assetService.GetAssetAsync<PerksTreeDialog>(DialogsId.PERKS_TREE_DIALOG, (prefab) =>
            {
                _perksTreeDialog = Object.Instantiate(prefab, root.transform);
                _perksTreeDialog.Init(servicesAggregator, managersAggregator, ManagersAggregatorInitialized);
                _perksTreeDialog.gameObject.SetActive(false);
            });

            while (!isDialogsInitialized)
            {
                yield return 0;
            }
        
            void ManagersAggregatorInitialized()
            {
                isDialogsInitialized = true;
            }
        }

        public void Dispose()
        {
            root.SetActive(false);
            _perksTreeDialog.Dispose();
        }

        public void ShowPerksTreeDialog()
        {
            _perksTreeDialog.Show();
        }
        
        public void HidePerksTreeDialog()
        {
            _perksTreeDialog.Hide();
        }
    }
}
