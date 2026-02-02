using UnityEngine;
using VertigoCase.Shared.Events;

namespace Project.Features.Zones.Runtime
{
    public class WheelZoneManager : MonoBehaviour
    {
        [SerializeField] private int currentZone = 1;

        public int CurrentZone => currentZone;
        

        private void Start()
        {
            EventManager.Publish(new WheelZoneChangedEvent(currentZone));
        }

        private void OnEnable()
        {
            EventManager.Subscribe<RewardGrantedEvent>(OnRewardGranted);
        }

        private void OnDisable()
        {
            EventManager.Unsubscribe<RewardGrantedEvent>(OnRewardGranted);
        }

        private void OnRewardGranted(RewardGrantedEvent e)
        {
            if (e.IsBomb) return;
            NextZone();
        }

        [ContextMenu("Next Zone")]
        public void NextZone()
        {
            currentZone++;
            EventManager.Publish(new WheelZoneChangedEvent(currentZone));
        }
    }
}
