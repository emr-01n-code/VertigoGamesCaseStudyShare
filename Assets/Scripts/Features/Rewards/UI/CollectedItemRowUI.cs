using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Project.Features.Wheel.Config;
using Project.Features.Rewards.Runtime;
using System.Globalization;

namespace Project.Features.Rewards.UI
{
    public class CollectedItemRowUI : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private TMP_Text amountText;

        private string _currentKey;

        public void Set(string itemKey, int totalAmount)
        {
            if (amountText != null)
                amountText.text = FormatAmount(totalAmount);

            if (!string.Equals(_currentKey, itemKey))
            {
                _currentKey = itemKey;
                UpdateIcon(itemKey);
            }
        }

        private void UpdateIcon(string itemKey)
        {
            if (icon == null) return;

            if (RewardIconCache.TryGet(itemKey, out var sprite) && sprite != null)
            {
                icon.sprite = sprite;
                icon.enabled = true;
            }
            else
            {
                icon.sprite = null;
                icon.enabled = false;
            }
        }

        private static string FormatAmount(int value)
        {
            double abs = Mathf.Abs(value);

            if (abs < 1000)
                return value.ToString(CultureInfo.InvariantCulture);

            if (abs < 1_000_000)
                return (value / 1000d).ToString("0.#", CultureInfo.InvariantCulture) + "K";

            if (abs < 1_000_000_000)
                return (value / 1_000_000d).ToString("0.#", CultureInfo.InvariantCulture) + "M";

            return (value / 1_000_000_000d).ToString("0.#", CultureInfo.InvariantCulture) + "B";
        }
    }
}
