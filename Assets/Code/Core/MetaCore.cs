using System.Collections;
using Code.Core.Managers;
using Code.Core.Services;
using Code.UI.Meta;
using UnityEngine;

namespace Code.Core
{
    public class MetaCore : MonoBehaviour
    {
        private MetaUICore _metaUiCore;
    
        public IEnumerator Init(
            IServicesAggregator servicesAggregator, 
            IManagersAggregator managersAggregator,
            MetaUICore metaUiCore)
        {
            _metaUiCore = metaUiCore;
            yield return metaUiCore.Init(servicesAggregator, managersAggregator);
            _metaUiCore.ShowPerksTreeDialog();
        }

        public void Dispose()
        {
            _metaUiCore.Dispose();
            _metaUiCore = null;
        }
    }
}
