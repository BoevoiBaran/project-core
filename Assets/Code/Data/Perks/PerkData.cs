using System.Collections.Generic;
using Code.Data.Helpers;

namespace Code.Data.Perks
{
    public class PerkData
    {
        public readonly string Id;
        public PerkViewData ViewData;
        public bool IsBase;
        public int LearnPrice;
        
        public PerkData(string id)
        {
            Id = id;
        }

        public void ReadData(Dictionary<string, object> dataNode)
        {
            ViewData = new PerkViewData();
            ViewData.ReadData(dataNode.GetDataNode("view_settings"));
            IsBase = dataNode.GetBool("is_base");
            LearnPrice = dataNode.GetInt("learn_price");
        }
    
        public void ResolveDependencies(DataStorage storage)
        {
        
        }
    }
    
    public class PerkViewData
    {
        public string PrefabId;
        public Vector2Int Position;
        public float Size;
        public string Name;
        public void ReadData(Dictionary<string, object> dataNode)
        {
            PrefabId = dataNode.GetString("prefab_id");
            Name = dataNode.GetString("name");
            Size = dataNode.GetFloat("size");
            Position = dataNode.GetVector2Int("position");
        }
    }
}
