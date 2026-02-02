using DG.Tweening;
using Project.Features.Wheel.Core;
using UnityEngine;
using UnityEngine.UI;
using VertigoCase.Shared.Events;
using VertigoCase.Features.Wheel.Data;

namespace Project.Features.Wheel.Runtime
{
    public class WheelSpinner : MonoBehaviour, IWheelSpinnable
    {
        [Header("Refs")]
        [SerializeField] private Transform wheelPivot;
        [SerializeField] private Button spinButton;
        [SerializeField] private WheelIndicator indicator;

        [Header("Settings")]
        [SerializeField] private int slotCount = 8;
        [SerializeField] private float rotationDuration = 6f;

        private IWheelSpinLogic _logic;
        private bool _isSpinning;

        private Tween _spinTween;

        private void Awake()
        {
              _logic = new WheelSpinLogic(fullRotations: 6);
        }

        private void OnEnable()
        {
            spinButton.onClick.AddListener(Spin);
        }

        private void OnDisable()
        {
            spinButton.onClick.RemoveListener(Spin);

            _spinTween?.Kill();
            _spinTween = null;
            _isSpinning = false;

            if (spinButton != null)
                spinButton.interactable = true;
        }

        public bool CanSpin() => !_isSpinning;

        public void Spin()
        {
            if (!CanSpin()) return;
            BeginSpin();
        }

        private void BeginSpin()
        {
            _isSpinning = true;
            spinButton.interactable = false;

            EventManager.Publish(new WheelSpinStartedEvent());

            float angle = _logic.CalculateSpinTargetAngle(slotCount);
            StartSpinTween(angle);
        }

        private void StartSpinTween(float angle)
        {
            _spinTween?.Kill();
            wheelPivot.DOKill();

            _spinTween = wheelPivot
                .DORotate(new Vector3(0, 0, -angle), rotationDuration, RotateMode.FastBeyond360)
                .SetEase(Ease.OutQuart)
                .SetTarget(wheelPivot)
                .SetLink(gameObject)
                .OnComplete(CompleteSpin);
        }

        private void CompleteSpin()
        {
            EndSpinUI();

            var selected = indicator != null ? indicator.GetSelectedItem() : null;

            if (selected == null)
            {
                EventManager.Publish(new WheelSpinCompletedEvent());
                return;
            }

            ResolveSpinResult(selected);
        }

        private void EndSpinUI()
        {
            _isSpinning = false;
            _spinTween = null;

            if (spinButton != null)
                spinButton.interactable = true;
        }

        private void ResolveSpinResult(WheelItemInstance selected)
        {
            switch (selected.type)
            {
                case WheelItemType.Bomb:
                    EventManager.Publish(new WheelSpinFailedEvent());
                    break;

                default:
                    EventManager.Publish(new RewardGrantedEvent(
                        selected.key,
                        selected.amount,
                        false
                    ));
                    EventManager.Publish(new WheelSpinCompletedEvent());
                    break;
            }
        }

    }
}
