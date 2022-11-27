using System;
using System.Collections.Generic;

namespace Code.Data.Helpers
{
    public static class DataReaderHelper
    {
        public static string GetString(this Dictionary<string, object> dataNode, string key, string defaultValue = "")
        {
            if (!dataNode.TryGetValue(key, out var objectValue))
            {
                return defaultValue;
            }

            return objectValue as string;
        }
        
        public static bool GetBool(this Dictionary<string, object> dataNode, string key, bool defaultValue = false)
        {
            if (!dataNode.TryGetValue(key, out var objectValue))
            {
                return defaultValue;
            }

            return Convert.ToBoolean(objectValue);
        }
        
        public static int GetInt(this Dictionary<string, object> dataNode, string key, int defaultValue = 0)
        {
            if (!dataNode.TryGetValue(key, out var objectValue))
            {
                return defaultValue;
            }
            return Convert.ToInt32(objectValue);
        }
        
        public static float GetFloat(this Dictionary<string, object> dataNode, string key, float defaultValue = 0)
        {
            if (!dataNode.TryGetValue(key, out var objectValue))
            {
                return defaultValue;
            }
            return Convert.ToSingle(objectValue);
        }
    
        public static Dictionary<string, object> GetDataNode(this Dictionary<string, object> dataNode, string key)
        {
            if (!dataNode.TryGetValue(key, out var objectValue))
            {
                return new Dictionary<string, object>();
            }

            return objectValue as Dictionary<string, object>;
        }
        
        public static Dictionary<string, object> ToDataNode(this object objectData)
        {
            return objectData as Dictionary<string, object>;
        }

        public static List<object> GetDataList(this Dictionary<string, object> dataNode, string key)
        {
            if (!dataNode.TryGetValue(key, out var objectValue))
            {
                return new List<object>();
            }

            return objectValue as List<object>;
        }

        public static Vector2Int GetVector2Int(this Dictionary<string, object> dataNode, string key)
        {
            if (!dataNode.TryGetValue(key, out var objectValue))
            {
                return Vector2Int.Zero;
            }

            if (!(objectValue is List<object> points))
            {
                return Vector2Int.Zero;
            }

            List<int> intPoints = new List<int>();
            for (int i = 0; i < points.Count; i++)
            {
                intPoints.Add(Convert.ToInt32(points[i]));
            }

            if (intPoints.Count == 2)
            {
                return new Vector2Int(intPoints[0], intPoints[1]);
            }

            if (intPoints.Count == 1)
            {
                return new Vector2Int(intPoints[0], 0);
            }

            return Vector2Int.Zero;
        }
    }
}
