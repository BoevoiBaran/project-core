namespace Code.Data.Perks
{
    public struct PerkConnectionData
    {
        public readonly string FirstPerkId;
        public readonly string SecondPerkId;

        public PerkConnectionData(string firstPerkId, string secondPerkId)
        {
            FirstPerkId = firstPerkId;
            SecondPerkId = secondPerkId;
        }
    }
}