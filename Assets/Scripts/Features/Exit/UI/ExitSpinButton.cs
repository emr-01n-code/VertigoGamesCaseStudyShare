using UnityEngine;
using UnityEngine.UI;
using VertigoCase.Shared.Events;
using Project.Features.Wheel.Config;
using Project.Features.Wheel.Data;

namespace Project.Features.Exit.UI
{
    public class ExitSpinButton : MonoBehaviour
    {
        [SerializeField] private Button btn;

        private void OnValidate()
        {
            if (btn == null) btn = GetComponent<Button>();
        }

        private void OnEnable()
        {
            btn.onClick.AddListener(OnClick);

            EventManager.Subscribe<WheelSpinStartedEvent>(OnSpinStarted);
            EventManager.Subscribe<WheelZoneChangedEvent>(OnZoneChanged);

            btn.interactable = false;
        }

        private void OnDisable()
        {
            btn.onClick.RemoveListener(OnClick);

            EventManager.Unsubscribe<WheelSpinStartedEvent>(OnSpinStarted);
            EventManager.Unsubscribe<WheelZoneChangedEvent>(OnZoneChanged);
        }

        private void OnSpinStarted(WheelSpinStartedEvent _)
        {
            btn.interactable = false;
        }

        private void OnZoneChanged(WheelZoneChangedEvent e)
        {
            if (e.Zone <= 1) { btn.interactable = false; return; }

            var type = WheelRegistry.Instance.GetTypeByZone(e.Zone);
            btn.interactable = type == SpinType.Silver || type == SpinType.Gold;
        }

        private void OnClick()
        {
            EventManager.Publish(new RunExitRequestedEvent());
        }
    }
}
