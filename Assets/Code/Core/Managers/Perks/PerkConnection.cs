using Code.Data;
using Code.Data.Perks;

namespace Code.Core.Managers.Perks
{
    public struct PerkConnection
    {
        public readonly string FirstPerkId;
        public readonly string SecondPerkId;
        public readonly Vector2Int FirstPoint;
        public readonly Vector2Int SecondPoint;

        public PerkConnection(PerkConnectionData connectionData, Vector2Int firstPoint, Vector2Int secondPoint)
        {
            FirstPerkId = connectionData.FirstPerkId;
            SecondPerkId = connectionData.SecondPerkId;
            FirstPoint = firstPoint;
            SecondPoint = secondPoint;
        }
        
        public bool ContainPerkId(string perkId)
        {
            return FirstPerkId.Equals(perkId) || SecondPerkId.Equals(perkId);
        }
        
        public override bool Equals(object obj)
        {
            if (!(obj is PerkConnectionData perkConnectionId))
            {
                return false;
            }
            
            return Equals(perkConnectionId);
        }

        public bool Equals(PerkConnectionData perkConnectionDataId)
        {
            return ContainPerkId(perkConnectionDataId.FirstPerkId) && ContainPerkId(perkConnectionDataId.SecondPerkId);
        }

        public override int GetHashCode()
        {
            return (FirstPerkId != null ? FirstPerkId.GetHashCode() : 0) + (SecondPerkId != null ? SecondPerkId.GetHashCode() : 0);
        }
    }
}