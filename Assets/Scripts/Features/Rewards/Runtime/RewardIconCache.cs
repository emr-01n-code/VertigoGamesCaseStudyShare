using System.Collections.Generic;
using UnityEngine;
using Project.Features.Wheel.Config;

namespace Project.Features.Rewards.Runtime
{
    public static class RewardIconCache
    {
        private static readonly Dictionary<string, Sprite> _cache = new();

        public static bool TryGet(string itemKey, out Sprite sprite)
        {
            if (string.IsNullOrEmpty(itemKey))
            {
                sprite = null;
                return false;
            }

            if (_cache.TryGetValue(itemKey, out sprite) && sprite != null)
                return true;

            var db = WheelItemDatabase.Instance;
            if (db != null && db.TryGet(itemKey, out var so) && so != null && so.Icon != null)
            {
                sprite = so.Icon;
                _cache[itemKey] = sprite;
                return true;
            }

            sprite = null;
            return false;
        }

        public static void Clear()
        {
            _cache.Clear();
        }
    }
}
