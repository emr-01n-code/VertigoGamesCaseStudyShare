using UnityEngine;

namespace VertigoCase.Features.Wheel.Data
{
    [System.Serializable]
    public class WheelItemInstance
    {
        public string key;
        public WheelItemType type;
        public Sprite icon;
        public int amount;

        public WheelItemInstance(WheelItemDataSO data)
        {
            key = data.Key;
            type = data.Type;
            icon = data.Icon;

            if (type == WheelItemType.Bomb)
            {
                amount = 0;
            }
            else
            {
                amount = Random.Range(data.MinAmount, data.MaxAmount + 1);
            }
        }
    }
}
