using System;
using System.Collections.Generic;
using Code.Core.Services;
using Code.Data.Perks;

namespace Code.Data
{
    public class DataStorage
    {
        public readonly PerksDataContainer PerksDataContainer = new PerksDataContainer();

        public void Init(IAssetService assetService, Action initializedCb)
        {
            Load(assetService, initializedCb);
        }

        private void Load(IAssetService assetService, Action initializedCb)
        {
            var dataReader = new DataReader();
            dataReader.GetData(assetService, (dataNodes) =>
            {
                InsertDataToContainers(dataNodes);
                ResolveDependencies(this);
                initializedCb?.Invoke();
            });
        }

        private void InsertDataToContainers(Dictionary<string, Dictionary<string, object>> dataNodes)
        {
            PerksDataContainer.ReadData(dataNodes[PerksDataContainer.DATA_FILE_NAME]);
        }

        private void ResolveDependencies(DataStorage storage)
        {
            PerksDataContainer.ResolveDependencies(storage);
        }
    }
}
