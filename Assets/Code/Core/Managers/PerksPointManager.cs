using System;
using Code.Core.Services;

namespace Code.Core.Managers
{
    public interface IPerksPointManager
    {
        Action PointsValueChanged { get; set; }
        int PerksPointCount { get; }
        void Pay(int price);
        void AddPoints(int value);
    }
    
    public class PerksPointManager: BaseManager, IPerksPointManager
    {
        public const string MANAGER_NAME = nameof(PerksPointManager);
        
        public Action PointsValueChanged { get; set; }
        public int PerksPointCount { get; private set; }
        
        public override void Init(IServicesAggregator services, Action initializedCb)
        {
            initializedCb?.Invoke();
        }

        public void Pay(int price)
        {
            ChangePointsValue(-price);
        }

        public void AddPoints(int value)
        {
            ChangePointsValue(value);
        }

        private void ChangePointsValue(int value)
        {
            PerksPointCount += value;
            PointsValueChanged?.Invoke();
        }
    }
}