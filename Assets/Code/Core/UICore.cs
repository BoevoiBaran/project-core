using Code.UI.Common.Loading;
using Code.UI.Meta;
using UnityEngine;

namespace Code.Core
{
    public class UICore : MonoBehaviour
    {
        [SerializeField] private MetaUICore metaUICore;
        [SerializeField] private LoadingScreen loadingScreen;
        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        public MetaUICore Meta => metaUICore;

        public void ShowLoading()
        {
            loadingScreen.Show();
        }

        public void HideLoading()
        {
            loadingScreen.Hide();
        }
    }
}
