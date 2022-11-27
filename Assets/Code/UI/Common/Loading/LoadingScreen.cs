using UnityEngine;

namespace Code.UI.Common.Loading
{
    public class LoadingScreen : MonoBehaviour
    {
        [SerializeField] 
        private GameObject root;
        [SerializeField] 
        private LoadingTextView loadingTextView;

        public void Show()
        {
            loadingTextView.Show();
            root.SetActive(true);
        }

        public void Hide()
        {
            loadingTextView.Hide();
            root.SetActive(false);
        }
    }
}
