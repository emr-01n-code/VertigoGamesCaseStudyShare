using TMPro;
using UnityEngine;

namespace Project.Features.Zones.UI
{
    public class ZoneNumberItemUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text zoneText;

        public void Set(int zoneNumber, Color color)
        {
            if (zoneText == null) return;
            zoneText.text = zoneNumber.ToString();
            zoneText.color = color;
            
        }
    }
}
