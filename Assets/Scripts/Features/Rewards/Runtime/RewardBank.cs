using System.Collections.Generic;

namespace Project.Features.Rewards.Runtime
{
    public class RewardBank
    {
        private readonly Dictionary<string, int> _totals = new();

        public IReadOnlyDictionary<string, int> Totals => _totals;

        public void Add(string key, int amount)
        {
            if (string.IsNullOrEmpty(key) || amount <= 0) return;

            if (_totals.TryGetValue(key, out var cur))
                _totals[key] = cur + amount;
            else
                _totals[key] = amount;
        }

        public void Clear() => _totals.Clear();

        public int GetTotal(string key)
        {
            return _totals.TryGetValue(key, out var v) ? v : 0;
        }
    }
}
