using Project.Features.Rewards.Persistence;

namespace Project.Features.Economy.Runtime
{
    public static class Wallet
    {
        public const string GOLD_KEY = "wheel_item_gold";
        public const string CASH_KEY = "wheel_item_cash";

        public static int Get(string key)
        {
            var rewards = RewardSaver.Load();
            foreach (var r in rewards)
                if (r.itemKey == key) return r.amount;
            return 0;
        }

        public static bool CanSpend(string key, int cost) => Get(key) >= cost;

        public static void TrySpend(string key, int cost)
        {
            var cur = Get(key);
            RewardSaver.SetAmount(key, cur - cost);
        }
    }
}
