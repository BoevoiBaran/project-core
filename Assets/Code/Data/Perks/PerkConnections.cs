using System.Collections.Generic;

namespace Code.Data.Perks
{
    public class PerkConnections
    {
        public List<PerkConnectionData> Connections = new List<PerkConnectionData>();

        public void ReadData(List<object> dataNode)
        {
            foreach (var objectValue in dataNode)
            {
                var listObjects = objectValue as List<object>;
                var perkConnection = new PerkConnectionData(listObjects[0] as string, listObjects[1] as string);
                Connections.Add(perkConnection);
            }
        }
    }
}