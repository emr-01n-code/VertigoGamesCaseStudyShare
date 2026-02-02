using System.Collections.Generic;
using System.IO;
using UnityEngine;
using VertigoCase.Shared.Events;

namespace Project.Features.Rewards.Persistence
{
    public static class RewardSaver
    {
        private static string SavePath =>
            Path.Combine(Application.persistentDataPath, "collected_rewards.json");

        [System.Serializable]
        private class RewardSaveData
        {
            public List<RewardEntry> rewards = new();
        }

        [System.Serializable]
        public class RewardEntry
        {
            public string itemKey;
            public int amount;

            public RewardEntry(string key, int amt)
            {
                itemKey = key;
                amount = amt;
            }
        }

        public static List<RewardEntry> Load()
        {
            if (!File.Exists(SavePath))
                return new List<RewardEntry>();

            var json = File.ReadAllText(SavePath);
            var data = JsonUtility.FromJson<RewardSaveData>(json);
            return data?.rewards ?? new List<RewardEntry>();
        }

        public static void SaveFromSessionTotals(IReadOnlyDictionary<string, int> sessionTotals)
        {
            var existing = Load();
            var map = new Dictionary<string, int>();

            foreach (var e in existing)
                map[e.itemKey] = e.amount;

            foreach (var kv in sessionTotals)
            {
                if (kv.Value <= 0) continue;

                if (map.ContainsKey(kv.Key)) map[kv.Key] += kv.Value;
                else map[kv.Key] = kv.Value;
            }

            WriteMap(map);
        }

        public static void SetAmount(string itemKey, int amount)
        {
            var rewards = Load();
            var existing = rewards.Find(r => r.itemKey == itemKey);

            if (existing != null)
            {
                if (amount <= 0) rewards.Remove(existing);
                else existing.amount = amount;
            }
            else
            {
                if (amount > 0) rewards.Add(new RewardEntry(itemKey, amount));
            }

            var data = new RewardSaveData { rewards = rewards };
            File.WriteAllText(SavePath, JsonUtility.ToJson(data, true));
            EventManager.Publish(new DataSavedEvent());
        }

        private static void WriteMap(Dictionary<string, int> map)
        {
            var data = new RewardSaveData();
            foreach (var kv in map)
                data.rewards.Add(new RewardEntry(kv.Key, kv.Value));

            File.WriteAllText(SavePath, JsonUtility.ToJson(data, true));
            EventManager.Publish(new DataSavedEvent());
        }

        public static void ClearAll()
        {
            if (File.Exists(SavePath))
                File.Delete(SavePath);

            EventManager.Publish(new DataSavedEvent());
        }
    }
}
