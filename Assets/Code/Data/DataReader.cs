using System;
using System.Collections.Generic;
using Code.Core.Services;
using UnityEngine;


namespace Code.Data
{
    public class DataReader
    {
        public void GetData(IAssetService assetService, Action<Dictionary<string, Dictionary<string, object>>> loadedCb)
        {
#if UNITY_EDITOR
            var data = EditorDataProvider.GetData();
            loadedCb?.Invoke(data);
#else
            DataProvider.GetData(assetService, loadedCb);
#endif
        }
    }

    public static class DataProvider
    {
        public static void GetData(IAssetService assetService, Action<Dictionary<string, Dictionary<string, object>>> loadedCb)
        {
            assetService.GetTextAssetAsync(Code.Data.Perks.PerksDataContainer.DATA_FILE_NAME, (textAsset) =>
            {
                var dataNode = (Dictionary<string, object>) fastJSON.JSON.Parse(textAsset.text);
                var result = new Dictionary<string, Dictionary<string, object>>
                {
                    { Code.Data.Perks.PerksDataContainer.DATA_FILE_NAME, dataNode }
                };
                loadedCb?.Invoke(result);
            });
        }
    }
#if UNITY_EDITOR
    public static class EditorDataProvider
    {
        private static readonly string DataFolderPath = $"{Application.dataPath}{System.IO.Path.DirectorySeparatorChar}Data{System.IO.Path.DirectorySeparatorChar}";

        public static Dictionary<string, Dictionary<string, object>> GetData()
        {
            var result = new Dictionary<string, Dictionary<string, object>>();
            if (!System.IO.Directory.Exists(DataFolderPath))
            {
                Debug.LogError($"Data folder: {DataFolderPath} not found!");
                return result;
            }

            ProcessDirectory(DataFolderPath, result);

            return result;
        }

        private static void ProcessDirectory(string path, Dictionary<string, Dictionary<string, object>> container)
        {
            string [] fileEntries = System.IO.Directory.GetFiles(path);
            string [] directoryEntries = System.IO.Directory.GetDirectories(path);
            foreach (var fileEntry in fileEntries)
            {
                if (string.Equals(System.IO.Path.GetExtension(fileEntry), ".json"))
                {
                    ReadFile(fileEntry, container);
                }
            }

            foreach (var directoryPath in directoryEntries)
            {
                ProcessDirectory(directoryPath, container);
            }
        }
    
        private static void ReadFile(string filePath, Dictionary<string, Dictionary<string, object>> container)
        {
            Dictionary<string, object> dataNode = null;
            using (var sr = new System.IO.StreamReader(filePath))
            {
                var text = sr.ReadToEnd();
                dataNode = (Dictionary<string, object>) fastJSON.JSON.Parse(text);
            }
            var fileName = System.IO.Path.GetFileName(filePath);
            container[fileName] = dataNode;
        }
    }
#endif
}