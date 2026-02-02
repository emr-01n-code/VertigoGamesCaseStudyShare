using TMPro;
using UnityEngine;
using VertigoCase.Shared.Events;

namespace Project.Features.Zones.UI
{
    public class ZoneBadgeUI : MonoBehaviour
    {
        [Header("Refs")]
        [SerializeField] private TMP_Text superBadgeNumberText;
        [SerializeField] private TMP_Text safeBadgeNumberText;

        [Header("Intervals")]
        [SerializeField] private int superInterval = 30;
        [SerializeField] private int safeInterval = 5;

        private int _nextSuper;
        private int _nextSafe;

        private void Awake()
        {
            _nextSuper = superInterval;
            _nextSafe = safeInterval;

            RefreshTexts();
        }

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
            var zone = e.Zone;
            if (zone <= 1) return;

            if (zone == _nextSuper)
            {
                _nextSuper += superInterval;
            }

            if (zone == _nextSafe)
            {
                if (zone == _nextSuper - 5)
                    _nextSafe += safeInterval * 2;
                else
                    _nextSafe += safeInterval;
            }

            RefreshTexts();
        }

        private void RefreshTexts()
        {
            if (superBadgeNumberText != null)
                superBadgeNumberText.text = _nextSuper.ToString();

            if (safeBadgeNumberText != null)
                safeBadgeNumberText.text = _nextSafe.ToString();
        }
    }
}
