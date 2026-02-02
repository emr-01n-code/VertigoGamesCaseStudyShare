using UnityEngine;

namespace VertigoCase.Features.Wheel.Data
{
    [CreateAssetMenu(menuName = "VertigoCase/Wheel/Config")]
    public class WheelConfigSO : ScriptableObject
    {
        [SerializeField] private WheelItemDataSO[] items;

        public WheelItemDataSO[] Items => items;

        public bool HasBomb
        {
            get
            {
                if (items == null) return false;
                for (int i = 0; i < items.Length; i++)
                {
                    if (items[i] != null && items[i].Type == WheelItemType.Bomb)
                        return true;
                }
                return false;
            }
        }

        public WheelItemDataSO GetBomb()
        {
            if (items == null) return null;
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i] != null && items[i].Type == WheelItemType.Bomb)
                    return items[i];
            }
            return null;
        }

        public WheelItemDataSO GetRandomReward()
        {
            if (items == null || items.Length == 0) return null;

            for (int attempt = 0; attempt < 20; attempt++)
            {
                var pick = items[Random.Range(0, items.Length)];
                if (pick != null && pick.Type == WheelItemType.Reward)
                    return pick;
            }

            return null;
        }
    }
}
