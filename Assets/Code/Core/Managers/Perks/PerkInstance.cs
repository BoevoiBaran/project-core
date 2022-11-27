using System.Collections.Generic;
using Code.Data.Perks;

namespace Code.Core.Managers.Perks
{
    public class PerkInstance
    {
        public readonly PerkViewData ViewData;
        public readonly string Id;
        public readonly HashSet<string> NeighbourPerks = new HashSet<string>();
        public bool IsLearnt => IsBase || _isLearnt;
        private bool _isLearnt;
        public readonly bool IsBase;
        public readonly int LearnPrice;

        public PerkInstance(string id, PerkData data, List<PerkConnection> perkConnections)
        {
            ViewData = data.ViewData;
            Id = id;
            IsBase = data.IsBase;
            LearnPrice = data.LearnPrice;
            PopulateNeighbours(perkConnections);
        }

        private void PopulateNeighbours(List<PerkConnection> perkConnections)
        {
            foreach (var perkConnection in perkConnections)
            {
                if (!perkConnection.ContainPerkId(Id))
                {
                    continue;
                }
            
                var anotherPerkId = perkConnection.FirstPerkId.Equals(Id)
                    ? perkConnection.SecondPerkId
                    : perkConnection.FirstPerkId;
                NeighbourPerks.Add(anotherPerkId);
            }
        }

        public void Learn()
        {
            _isLearnt = true;
        }
        
        public void ForgetPerk()
        {
            _isLearnt = false;
        }
    }
}
