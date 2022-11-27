using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.Meta.PerksTree.Views
{
    public class PerkSelectionView : MonoBehaviour
    {
        public event Action<PerkViewModel> OnLearnClick;
        public event Action<PerkViewModel> OnForgetClick;
        
        [SerializeField] private TextMeshProUGUI perkId;
        [SerializeField] private TextMeshProUGUI perkPrice;
        [SerializeField] private Button learnPerkButton;
        [SerializeField] private Button forgetPerkButton;

        private PerkViewModel _perkViewModel;
        private void Awake()
        {
            learnPerkButton.onClick.AddListener(OnLearnClicked);
            forgetPerkButton.onClick.AddListener(OnForgetClicked);
        }

        private void OnForgetClicked()
        {
            OnForgetClick?.Invoke(_perkViewModel);
        }

        private void OnLearnClicked()
        {
            OnLearnClick?.Invoke(_perkViewModel);
        }
        
        public void SetSelectedPerk(PerkViewModel perkViewModel)
        {
            if (_perkViewModel != null)
            {
                _perkViewModel.Updated -= UpdateView;
            }
            _perkViewModel = perkViewModel;
            if (_perkViewModel == null)
            {
                learnPerkButton.interactable = false;
                forgetPerkButton.interactable = false;
                return;
            }

            _perkViewModel.Updated += UpdateView;
            UpdateView();
        }

        private void UpdateView()
        {
            perkId.text = _perkViewModel.Name;
            perkPrice.text = _perkViewModel.LearnPrice.ToString();
            learnPerkButton.interactable = _perkViewModel.CanLearn;
            forgetPerkButton.interactable = _perkViewModel.CanForget;
        }
    }
}
