using System;
using Code.Core.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.Meta.PerksTree.Views
{
    public class PerkPointsView : MonoBehaviour
    {
        public event Action OnAddPointsClick;
        public event Action OnForgetAllPointsClick;
        
        [SerializeField] private TextMeshProUGUI perkPoints;
        [SerializeField] private Button addPointsButton;
        [SerializeField] private Button forgetAllPointsButton;

        private IPerksPointManager _perksPointManager;

        private void Awake()
        {
            addPointsButton.onClick.AddListener(OnAddPointsClicked);
            forgetAllPointsButton.onClick.AddListener(OnForgetAllPointsClicked);
        }

        private void OnForgetAllPointsClicked()
        {
            OnForgetAllPointsClick?.Invoke();
        }

        private void OnAddPointsClicked()
        {
            OnAddPointsClick?.Invoke();
        }

        public void Init(IPerksPointManager perksPointManager)
        {
            _perksPointManager = perksPointManager;
            _perksPointManager.PointsValueChanged += UpdateCount;
            UpdateCount();
        }

        private void UpdateCount()
        {
            perkPoints.text = _perksPointManager.PerksPointCount.ToString();
        }

        public void Dispose()
        {
            if (_perksPointManager != null)
            {
                _perksPointManager.PointsValueChanged -= UpdateCount;
            }

            _perksPointManager = null;
        }

        private void OnDestroy()
        {
            addPointsButton.onClick.RemoveAllListeners();
            forgetAllPointsButton.onClick.RemoveAllListeners();
            Dispose();
        }
    }
}
