using Project.Features.Wheel.Config;
using Project.Features.Wheel.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VertigoCase.Shared.Events;

namespace Project.Features.Wheel.Runtime
{
    public class WheelZoneInitializer : MonoBehaviour
    {
        [Header("Refs")]
        [SerializeField] private WheelItemInitializer initializer;

        [Header("Spin Title UI")]
        [SerializeField] private TMP_Text spinTitleText;

        [Header("Spin BG Sprite")]
        [SerializeField] private Image spinBgImage;
        [SerializeField] private Sprite bronzeSpinBg;
        [SerializeField] private Sprite silverSpinBg;
        [SerializeField] private Sprite goldSpinBg;

        [Header("Spin Indicator Sprite")]
        [SerializeField] private Image spinIndicatorImage;    
        [SerializeField] private Sprite bronzeSpinIndicator;
        [SerializeField] private Sprite silverSpinIndicator;
        [SerializeField] private Sprite goldSpinIndicator;

        [Header("Title Colors")]
        [SerializeField] private Color bronzeTitleColor = new Color(0.85f, 0.65f, 0.25f, 1f);
        [SerializeField] private Color silverTitleColor = new Color(0.85f, 0.85f, 0.85f, 1f);
        [SerializeField] private Color goldTitleColor = new Color(1.00f, 0.80f, 0.20f, 1f);

        [Header("Options")]
        [SerializeField] private bool changeTitleText = true;

        private void OnEnable()
        {
            EventManager.Subscribe<WheelZoneChangedEvent>(OnZoneChanged);
        }

        private void OnDisable()
        {
            EventManager.Unsubscribe<WheelZoneChangedEvent>(OnZoneChanged);
        }

        private void OnZoneChanged(WheelZoneChangedEvent e)
        {
            ApplyZone(e.Zone);
        }

        private void ApplyZone(int zone)
        {
            var reg = WheelRegistry.Instance;
            if (reg == null || initializer == null) return;

            var type = reg.GetTypeByZone(zone);
            var cfg = reg.GetConfig(type);
            if (cfg == null) return;

            initializer.SetItems(cfg);

            ApplySpinBg(type);

            ApplyTitle(type);

            ApplySpinIndicator(type);
        }

        private void ApplySpinBg(SpinType type)
        {
            if (spinBgImage == null) return;

            Sprite s = type switch
            {
                SpinType.Bronze => bronzeSpinBg,
                SpinType.Silver => silverSpinBg,
                SpinType.Gold => goldSpinBg,
                _ => null
            };

            if (s != null)
                spinBgImage.sprite = s;
        }

        private void ApplySpinIndicator(SpinType type)
        {
            if (spinIndicatorImage == null) return;

            Sprite s = type switch
            {
                SpinType.Bronze => bronzeSpinIndicator,
                SpinType.Silver => silverSpinIndicator,
                SpinType.Gold => goldSpinIndicator,
                _ => null
            };

            if (s != null)
                spinIndicatorImage.sprite = s;
        }

        private void ApplyTitle(SpinType type)
        {
            if (spinTitleText == null) return;

            if (changeTitleText)
            {
                spinTitleText.text = type switch
                {
                    SpinType.Bronze => "BRONZE SPIN",
                    SpinType.Silver => "SILVER SPIN",
                    SpinType.Gold => "GOLD SPIN",
                    _ => "SPIN"
                };
            }

            spinTitleText.color = type switch
            {
                SpinType.Bronze => bronzeTitleColor,
                SpinType.Silver => silverTitleColor,
                SpinType.Gold => goldTitleColor,
                _ => spinTitleText.color
            };
        }
    }
}
