using System;
using System.Collections;
using System.Collections.Generic;
using Code.Core.Managers;
using Code.Core.Managers.Perks;
using Code.Core.Services;
using Code.UI.Meta.PerksTree.Views;
using ThirdParty;
using UnityEngine;

namespace Code.UI.Meta.PerksTree
{
    public class PerksTreeDialog : MonoBehaviour
    {
        [SerializeField] private PerksTreeDialogView perksTreeDialogView;

        private readonly Selection<PerkViewModel> _selection = new Selection<PerkViewModel>();
        
        private IPerksManager _perksManager;
        private IPerksPointManager _perksPointManager;
        private IAssetService _assetService;
        
        private readonly Dictionary<string, PerkViewModel> _perkViewModels = new Dictionary<string, PerkViewModel>();

        public void Init(IServicesAggregator servicesAggregator, IManagersAggregator managersAggregator, Action initializedCb)
        {
            _perksManager = managersAggregator.GetManager<IPerksManager>(PerksManager.MANAGER_NAME);
            _perksPointManager = managersAggregator.GetManager<IPerksPointManager>(PerksPointManager.MANAGER_NAME);
            _assetService = servicesAggregator.GetService<IAssetService>(AssetService.SERVICE_NAME);

            perksTreeDialogView.Init(_perksPointManager);
            
            InitPerksViewModel();
            
            var coroutineService = servicesAggregator.GetService<ICoroutineService>(CoroutineService.SERVICE_NAME);
            coroutineService.StartCoroutine(SpawnUI(initializedCb));
            
            _perksPointManager.PointsValueChanged += PointsValueChanged;
        }

        private void InitPerksViewModel()
        {
            var perks = _perksManager.Perks;

            foreach (var perkInstance in perks)
            {
                var viewModel = new PerkViewModel();
                _perkViewModels[perkInstance.Id] = viewModel;
            }

            UpdateAllPerksView();
        }
        private IEnumerator SpawnUI(Action initializedCb)
        {
            yield return perksTreeDialogView.DrawLines(_perksManager.PerksConnection, _assetService);
            yield return perksTreeDialogView.DrawPerks(_perkViewModels, _assetService);
            initializedCb?.Invoke();
        }
        
        public void Show()
        {
            gameObject.SetActive(true);
            var basePerkViewModel = _perkViewModels[_perksManager.BasePerk.Id];
            _selection.Select(basePerkViewModel);
        }
        public void Hide()
        {
            gameObject.SetActive(false);
        }
        public void Dispose()
        {
            if (_perksPointManager != null)
            {
                _perksPointManager.PointsValueChanged -= PointsValueChanged;
            }

            _perksPointManager = null;
            perksTreeDialogView.Dispose();
        }

        #region Subscribtion

        private void PointsValueChanged()
        {
            UpdateAllPerksView();
        }

        private void OnForgetPerkClicked(PerkViewModel perkViewModel)
        {
            _perksManager.ForgetPerk(_perksPointManager, perkViewModel.Id);
            UpdateLearntAndNeighbours(perkViewModel);
        }

        private void OnLearnClicked(PerkViewModel perkViewModel)
        {
            _perksManager.LearnPerk(_perksPointManager, perkViewModel.Id);
            UpdateLearntAndNeighbours(perkViewModel);
        }

        private void OnForgetAllPointsClicked()
        {
            _perksManager.ForgetAllPerks(_perksPointManager);
            UpdateAllPerksView();
        }

        private void OnAddPointsClicked()
        {
            _perksPointManager.AddPoints(1);
        }

        private void OnPerkClicked(PerkViewModel viewController)
        {
            _selection.Select(viewController);
        }

        private void OnSelectionChanged(PerkViewModel old, PerkViewModel current)
        {
            perksTreeDialogView.SetSelectedPerk(current);
        }

        private void Subscribe()
        {
            _selection.SelectionChanged += OnSelectionChanged;
            perksTreeDialogView.OnForgetAllPointsClick += OnForgetAllPointsClicked;
            perksTreeDialogView.OnAddPointsClick += OnAddPointsClicked;
            perksTreeDialogView.OnForgetClick += OnForgetPerkClicked;
            perksTreeDialogView.OnLearnClick += OnLearnClicked;
            perksTreeDialogView.OnPerkClick += OnPerkClicked;
        }
        
        private void UnSubscribe()
        {
            _selection.SelectionChanged -= OnSelectionChanged;
            perksTreeDialogView.OnForgetAllPointsClick -= OnForgetAllPointsClicked;
            perksTreeDialogView.OnAddPointsClick -= OnAddPointsClicked;
            perksTreeDialogView.OnForgetClick -= OnForgetPerkClicked;
            perksTreeDialogView.OnLearnClick -= OnLearnClicked;
            perksTreeDialogView.OnPerkClick -= OnPerkClicked;
        }

        #endregion
        
        private void UpdateLearntAndNeighbours(PerkViewModel perkViewModel)
        {
            UpdatePerkViewModel(perkViewModel.Id);
            var perkInstance = _perksManager.PerksById[perkViewModel.Id];

            foreach (var perkId in perkInstance.NeighbourPerks)
            {
                UpdatePerkViewModel(perkId);
            }
        }
        private void UpdateAllPerksView()
        {
            foreach (var (perkId, _) in _perkViewModels)
            {
                UpdatePerkViewModel(perkId);
            }
        }
        private void UpdatePerkViewModel(string perkId)
        {
            var perkInstance = _perksManager.PerksById[perkId];
            var viewModel = _perkViewModels[perkId];
            var canLearn = _perksManager.CanLearnPerk(_perksPointManager, perkInstance.Id);
            var canForget = _perksManager.CanForgetPerk(perkInstance.Id);
            viewModel.Update(perkInstance, canLearn, canForget);
        }
        
        private void Awake()
        {
            Subscribe();
        }
        
        private void OnDestroy()
        {
            UnSubscribe();
            Dispose();
        }
    }
}
