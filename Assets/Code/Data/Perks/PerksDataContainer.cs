using System.Collections.Generic;
using Code.Data.Helpers;
using ThirdParty;

namespace Code.Data.Perks
{
    public class PerksDataContainer
    {
        public const string DATA_FILE_NAME = "perks.json";
    
        public Dictionary<string, PerkData> Perks = new Dictionary<string, PerkData>();
        public List<PerkConnectionData> Connections = new List<PerkConnectionData>();
        public void ReadData(Dictionary<string, object> entitiesDataNode)
        {
            var perksDataNode = entitiesDataNode.GetDataNode("perks");
            foreach (var (id, entityDataNode) in perksDataNode)
            {
                var perkData = new PerkData(id);
                perkData.ReadData(entityDataNode.ToDataNode());
                Perks.Add(id, perkData);
            }

            var perkConnections = new PerkConnections();
            perkConnections.ReadData(entitiesDataNode.GetDataList("connections"));
            Connections = perkConnections.Connections;
        }
    
        public void ResolveDependencies(DataStorage storage)
        {
            foreach (var (_, entityData) in Perks)
            {
                entityData.ResolveDependencies(storage);
            }
        }
    }
}
