using System.Collections.Generic;
using UnityEngine;
using VertigoCase.Features.Wheel.Data;
using Project.Features.Wheel.UI;

namespace Project.Features.Wheel.Runtime
{
    public class WheelItemInitializer : MonoBehaviour
    {
        [SerializeField] private List<WheelItemUI> slots = new List<WheelItemUI>();

        private readonly List<WheelItemDataSO> _used = new();

        public void SetItems(WheelConfigSO config)
        {
            _used.Clear();

            for (int i = 0; i < slots.Count; i++)
            {
                var data = PickItem(config, i);
                var inst = new WheelItemInstance(data);
                slots[i].Setup(inst);
            }
        }

        private WheelItemDataSO PickItem(WheelConfigSO config, int index)
        {
            if (config.HasBomb && index == 0)
                return config.GetBomb();

            WheelItemDataSO picked;
            do
            {
                picked = config.GetRandomReward();
            } while (_used.Contains(picked));

            _used.Add(picked);
            return picked;
        }
    }
}
