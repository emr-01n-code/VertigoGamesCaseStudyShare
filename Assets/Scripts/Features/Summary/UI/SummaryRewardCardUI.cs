using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Project.Features.Wheel.Config;
using VertigoCase.Features.Wheel.Data;

namespace Project.Features.Summary.UI
{
    public class SummaryRewardCardUI : MonoBehaviour
    {
        [Header("Refs")]
        [SerializeField] private Image bgImage;
        [SerializeField] private Image iconImage;
        [SerializeField] private TMP_Text amountText;

        [Header("Tier Colors")]
        [SerializeField] private Color bronzeColor = Color.yellow;
        [SerializeField] private Color silverColor = Color.white;
        [SerializeField] private Color goldColor = Color.yellow;

        public void Set(string itemKey, int amount)
        {
            if (amountText != null)
                amountText.text = "x" + amount;

            if (WheelItemDatabase.Instance == null) return;

            if (WheelItemDatabase.Instance.TryGet(itemKey, out var so) && so != null)
            {
                if (iconImage != null && so.Icon != null)
                {
                    iconImage.sprite = so.Icon;
                    iconImage.enabled = true;
                }

                if (bgImage != null)
                {
                    bgImage.color = GetTierColor(so.Tier);
                }
            }

        }

        private Color GetTierColor(WheelItemTier tier)
        {
            return tier switch
            {
                WheelItemTier.Bronze => bronzeColor,
                WheelItemTier.Silver => silverColor,
                WheelItemTier.Gold => goldColor,
                _ => bronzeColor
            };
        }
    }
}
