using UnityEngine;
using UnityEngine.UI;
using TMPro;
using VertigoCase.Shared.Events;
using VertigoCase.Features.Wheel.Data;

namespace Project.Features.Wheel.UI
{
    public class WheelItemUI : MonoBehaviour
    {
        [Header("Refs")]
        [SerializeField] private Image iconImage;
        [SerializeField] private TMP_Text amountText;
        [SerializeField] private Collider2D col;
        [SerializeField] private WheelItemView itemView;

        private WheelItemInstance _item;
        public WheelItemInstance GetItem() => _item;

        private void OnValidate()
        {
            if (iconImage == null) iconImage = GetComponent<Image>();
            if (col == null) col = GetComponent<Collider2D>();
        }

        private void OnEnable()
        {
            EventManager.Subscribe<WheelSpinStartedEvent>(OnSpinStarted);
            EventManager.Subscribe<WheelSpinCompletedEvent>(OnSpinCompleted);
        }

        private void OnDisable()
        {
            EventManager.Unsubscribe<WheelSpinStartedEvent>(OnSpinStarted);
            EventManager.Unsubscribe<WheelSpinCompletedEvent>(OnSpinCompleted);
        }

        private void OnSpinStarted(WheelSpinStartedEvent e)
        {
            if (col != null) col.enabled = true;
        }

        private void OnSpinCompleted(WheelSpinCompletedEvent e)
        {
            if (col != null) col.enabled = false;
        }

        public void Setup(WheelItemInstance item)
        {
            if (itemView != null)
                itemView.SetItem(item);
            _item = item;

            if (iconImage != null) iconImage.sprite = item.icon;

            if (amountText != null)
            {
                if (item.type == WheelItemType.Bomb)
                    amountText.text = "BOMB";
                else
                    amountText.text = "x" + item.amount;
            }
        }
    }
}
