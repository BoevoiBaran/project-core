using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Code.UI.Meta.PerksTree.Views
{
    public class PerkView : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Color activeColor;
        [SerializeField] private Color disableColor;
        [SerializeField] private RectTransform perkTransform;
        [SerializeField] private Image perkImage;
        [SerializeField] private TextMeshProUGUI perkIdText;
        [SerializeField] private GameObject highlight;

        public event Action<PerkViewModel> OnClick;

        private PerkViewModel _perkViewModel;

        public void Init(PerkViewModel perkViewModel)
        {
            _perkViewModel = perkViewModel;
            perkIdText.text = _perkViewModel.Name;
            
            _perkViewModel.Updated += UpdateView;
            SetupPrefabView();
            UpdateView();
        }
        
        private void SetupPrefabView()
        {
            perkTransform.localScale = Vector3.one;
            perkTransform.localPosition = new Vector3(_perkViewModel.Position.X, _perkViewModel.Position.Y, 0);
            perkTransform.sizeDelta = Vector2.one * _perkViewModel.Size;
        }

        private void UpdateView()
        {
            perkImage.color = _perkViewModel.IsLearnt ? activeColor : disableColor;
            highlight.SetActive(_perkViewModel.IsSelected);
        }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            OnClick?.Invoke(_perkViewModel);
        }
    }
}
