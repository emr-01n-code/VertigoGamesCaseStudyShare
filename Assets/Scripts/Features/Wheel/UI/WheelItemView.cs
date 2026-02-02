using Project.Features.Wheel.Data;
using UnityEngine;
using VertigoCase.Features.Wheel.Data;

namespace Project.Features.Wheel.UI
{
    public class WheelItemView : MonoBehaviour
    {
        private WheelItemInstance _item;

        public WheelItemInstance GetItem() => _item;

        public void SetItem(WheelItemInstance item)
        {
            _item = item;

        }
    }
}
