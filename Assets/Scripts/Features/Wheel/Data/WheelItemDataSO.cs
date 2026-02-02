using UnityEngine;

namespace VertigoCase.Features.Wheel.Data
{
    [CreateAssetMenu(menuName = "VertigoCase/Wheel/Item")]
    public class WheelItemDataSO : ScriptableObject
    {
        [Header("Identity")]
        [SerializeField] private string key;

        [Header("Visual")]
        [SerializeField] private Sprite icon;

        [Header("Rarity / Tier")]
        [SerializeField] private WheelItemTier tier = WheelItemTier.Bronze;

        [Header("Gameplay")]
        [SerializeField] private WheelItemType type = WheelItemType.Reward;
        [SerializeField] private int minAmount = 1;
        [SerializeField] private int maxAmount = 1;

        public string Key => string.IsNullOrWhiteSpace(key) ? name : key;
        public Sprite Icon => icon;
        public WheelItemType Type => type;
        public WheelItemTier Tier => tier;
        public int MinAmount => minAmount;
        public int MaxAmount => maxAmount;

        private void OnValidate()
        {
            if (string.IsNullOrWhiteSpace(key))
                key = name;

            if (minAmount < 0) minAmount = 0;
            if (maxAmount < minAmount) maxAmount = minAmount;
        }
    }
}
