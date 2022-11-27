using System;
using Code.Core.Managers.Perks;
using Code.Data;
using ThirdParty;

namespace Code.UI.Meta.PerksTree
{
    public class PerkViewModel: ISelection
    {
        public event Action Updated;
        
        public string Id;
        public string Name;
        public bool IsLearnt;
        public int LearnPrice;
        
        public string PrefabId;
        public Vector2Int Position;
        public float Size;

        public bool IsSelected;

        public bool CanLearn;
        public bool CanForget;

        public void Update(PerkInstance perkInstance, bool canLearn, bool canForget)
        {
            CanForget = canForget;
            Name = perkInstance.ViewData.Name;
            Id = perkInstance.Id;
            IsLearnt = perkInstance.IsLearnt;
            LearnPrice = perkInstance.LearnPrice;
            PrefabId = perkInstance.ViewData.PrefabId;
            Position = perkInstance.ViewData.Position;
            Size = perkInstance.ViewData.Size;
            CanLearn = canLearn;
            Updated?.Invoke();
        }

        void ISelection.Deselect()
        {
            IsSelected = false;
            Updated?.Invoke();
        }
        
        void ISelection.Select()
        {
            IsSelected = true;
            Updated?.Invoke();
        }
    }
}
