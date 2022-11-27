using System;
using Code.Data;
using Code.Data.Perks;

namespace Code.Core.Services
{
    public interface IDataStorage
    {
        PerksDataContainer PerksDataContainer { get; }
    }

    public class DataStorageService : BaseService, IDataStorage
    {
        public const string SERVICE_NAME = nameof(DataStorageService);

        private readonly DataStorage _dataStorage = new DataStorage();
        
        public override void Init(IServicesAggregator services, Action initializedCb)
        {
            var assetService = services.GetService<IAssetService>(AssetService.SERVICE_NAME);  
            _dataStorage.Init(assetService, initializedCb);
        }
        
        #region IDataStorageService
        
        PerksDataContainer IDataStorage.PerksDataContainer => _dataStorage.PerksDataContainer;

        #endregion
    }
}