using System;
using System.Collections.Generic;
using Code.Core.Services;
using Code.Data;
using Code.Data.Perks;
using ThirdParty;

namespace Code.Core.Managers.Perks
{
    public interface IPerksManager
    {
        List<PerkInstance> Perks { get; }
        PerkInstance BasePerk { get; }
        List<PerkConnection> PerksConnection { get; }
        Dictionary<string, PerkInstance> PerksById { get; }

        bool CanLearnPerk(IPerksPointManager perksPointManager, string perkId);
        void LearnPerk(IPerksPointManager perksPointManager, string perkId);
        bool CanForgetPerk(string perkId);
        void ForgetPerk(IPerksPointManager perksPointManager, string perkId);
        void ForgetAllPerks(IPerksPointManager perksPointManager);
    }
    
    public class PerksManager: BaseManager, IPerksManager
    {
        public const string MANAGER_NAME = nameof(PerksManager);
        
        private readonly List<PerkInstance> _perks = new List<PerkInstance>();
        private readonly List<PerkConnection> _perksConnection = new List<PerkConnection>();
        private readonly Dictionary<string, PerkInstance> _perksById = new Dictionary<string, PerkInstance>();

        private PerksDataContainer _perksDataContainer;
        private PerkInstance _basePerkInstance;
        
        public override void Init(IServicesAggregator services, Action initializedCb)
        {
            var dataStorage = services.GetService<IDataStorage>(DataStorageService.SERVICE_NAME);
            _perksDataContainer = dataStorage.PerksDataContainer;
            InitPerks(_perksDataContainer);
            
            initializedCb?.Invoke();
        }

        private void InitPerks(PerksDataContainer perksDataContainer)
        {
            var perksData = perksDataContainer.Perks;
            var connectionsData = _perksDataContainer.Connections;

            foreach (var connectionData in connectionsData)
            {
                var firstPoint = perksData[connectionData.FirstPerkId].ViewData.Position;
                var secondPoint = perksData[connectionData.SecondPerkId].ViewData.Position;
                var connection = new PerkConnection(connectionData, firstPoint, secondPoint);
                _perksConnection.Add(connection);
            }
            
            foreach (var (id, data) in perksData)
            {
                var perkInstance = new PerkInstance(id, data, _perksConnection);
                _perks.Add(perkInstance);
                _perksById[id] = perkInstance;
                if (perkInstance.IsBase)
                {
                    _basePerkInstance = perkInstance;
                }
            }
        }

        public bool CanLearnPerk(IPerksPointManager perksPointManager, string perkId)
        {
            var perkInstance = _perksById[perkId];
            if (perkInstance.IsLearnt)
            {
                return false;
            }

            if (perkInstance.LearnPrice > perksPointManager.PerksPointCount)
            {
                return false;
            }

            var neighbours = perkInstance.NeighbourPerks;
            foreach (var neighbourPerkId in neighbours)
            {
                if (_perksById[neighbourPerkId].IsLearnt)
                {
                    return true;
                }
            }

            return false;
        }

        public void LearnPerk(IPerksPointManager perksPointManager, string perkId)
        {
            var perkInstance = _perksById[perkId];
            perkInstance.Learn();
            perksPointManager.Pay(perkInstance.LearnPrice);
        }

        public bool CanForgetPerk(string perkId)
        {
            var perkInstance = _perksById[perkId];
            if (!perkInstance.IsLearnt)
            {
                return false;
            }

            if (perkInstance.IsBase)
            {
                return false;
            }

            var checker = new ForgetPerkNeighboursChecker(this, perkId, _basePerkInstance.Id);
            return checker.Check();
        }

        public void ForgetPerk(IPerksPointManager perksPointManager, string perkId)
        {
            var perkInstance = _perksById[perkId];
            perkInstance.ForgetPerk();
            perksPointManager.AddPoints(perkInstance.LearnPrice);
        }
        
        public void ForgetAllPerks(IPerksPointManager perksPointManager)
        {
            foreach (var perkInstance in _perks)
            {
                if (perkInstance.IsBase || !perkInstance.IsLearnt)
                {
                    continue;
                }
                perkInstance.ForgetPerk();
                perksPointManager.AddPoints(perkInstance.LearnPrice);
            }
        }

        List<PerkInstance> IPerksManager.Perks => _perks;
        List<PerkConnection> IPerksManager.PerksConnection => _perksConnection;
        Dictionary<string, PerkInstance> IPerksManager.PerksById => _perksById;
        PerkInstance IPerksManager.BasePerk => _basePerkInstance;
    }
}