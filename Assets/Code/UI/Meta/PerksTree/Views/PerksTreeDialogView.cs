using System;
using System.Collections;
using System.Collections.Generic;
using Code.Constants;
using Code.Core.Managers;
using Code.Core.Managers.Perks;
using Code.Core.Services;
using ThirdParty;
using UnityEngine;

namespace Code.UI.Meta.PerksTree.Views
{
    public class PerksTreeDialogView : MonoBehaviour
    {
        public event Action OnAddPointsClick;
        public event Action OnForgetAllPointsClick;
        public event Action<PerkViewModel> OnLearnClick;
        public event Action<PerkViewModel> OnForgetClick;
        public event Action<PerkViewModel> OnPerkClick;
        
        [SerializeField] private RectTransform perksRoot;
        [SerializeField] private RectTransform linesRoot;
        [SerializeField] private PerkPointsView perkPointsView;
        [SerializeField] private PerkSelectionView perkSelectionView;

        private readonly List<PerksConnectionLineView> _lineViews = new();
        private readonly List<PerkView> _perkViews = new();

        public void Init(IPerksPointManager perksPointManager)
        {
            perkPointsView.Init(perksPointManager);
        }
        public void Dispose()
        {
            if (perkPointsView)
            {
                perkPointsView.Dispose();
            }

            foreach (var perkView in _perkViews)
            {
                perkView.OnClick -= OnPerkClicked;
            }
        }
        public void SetSelectedPerk(PerkViewModel perkViewModel)
        {
            perkSelectionView.SetSelectedPerk(perkViewModel);
        }
        public IEnumerator DrawLines(List<PerkConnection> perksConnection, IAssetService assetService)
        {
            var needDrawLinesCount = perksConnection.Count;
            
            assetService.GetAssetAsync<PerksConnectionLineView>(PrefabsId.PERK_LINE, LinePrefabLoaded);

            while (needDrawLinesCount != 0)
            {
                yield return 0;
            }

            void LinePrefabLoaded(PerksConnectionLineView prefab)
            {
                foreach (var perkConnection in perksConnection)
                {
                    var view = Instantiate(prefab, linesRoot);
                    view.Draw(perkConnection.FirstPoint, perkConnection.SecondPoint);
                    needDrawLinesCount--;
                    _lineViews.Add(view);
                }
            }
        }
        public IEnumerator DrawPerks(Dictionary<string, PerkViewModel> perkViewModels, IAssetService assetService)
        {
            var needDrawPerksCount = perkViewModels.Count;
            
            foreach (var (_, viewModel) in perkViewModels)
            {
                assetService.GetAssetAsync<PerkView>(viewModel.PrefabId, (prefab) => PerkPrefabLoaded(viewModel, prefab));
            }

            while (needDrawPerksCount != 0)
            {
                yield return 0;
            }
            
            void PerkPrefabLoaded(PerkViewModel perkViewModel, PerkView prefab)
            {
                var view = Instantiate(prefab, perksRoot);
                view.Init(perkViewModel);
                view.OnClick += OnPerkClicked;
                needDrawPerksCount--;
                _perkViews.Add(view);
            }
        }

        #region Subscribtion
        
        private void OnPerkClicked(PerkViewModel perkViewModel)
        {
            OnPerkClick?.Invoke(perkViewModel);
        }
        
        private void OnForgetClicked(PerkViewModel perkViewModel)
        {
            OnForgetClick?.Invoke(perkViewModel);
        }
        private void OnLearnClicked(PerkViewModel perkViewModel)
        {
            OnLearnClick?.Invoke(perkViewModel);
        }
        private void OnForgetAllPointsClicked()
        {
            OnForgetAllPointsClick?.Invoke();
        }
        private void OnAddPointsClicked()
        {
            OnAddPointsClick?.Invoke();
        }

        private void Subscribe()
        {
            perkPointsView.OnForgetAllPointsClick += OnForgetAllPointsClicked;
            perkPointsView.OnAddPointsClick += OnAddPointsClicked;
            perkSelectionView.OnForgetClick += OnForgetClicked;
            perkSelectionView.OnLearnClick += OnLearnClicked;
        }
        
        private void UnSubscribe()
        {
            perkPointsView.OnForgetAllPointsClick -= OnForgetAllPointsClicked;
            perkPointsView.OnAddPointsClick -= OnAddPointsClicked;
            perkSelectionView.OnForgetClick -= OnForgetClicked;
            perkSelectionView.OnLearnClick -= OnLearnClicked;
        }
        #endregion
        
        private void Awake()
        {
            Subscribe();
        }
        private void OnDestroy()
        {
            UnSubscribe();
        }
    }
}