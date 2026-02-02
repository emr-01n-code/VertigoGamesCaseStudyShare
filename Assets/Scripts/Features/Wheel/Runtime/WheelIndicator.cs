using DG.Tweening;
using Project.Features.Wheel.UI;
using UnityEngine;
using VertigoCase.Features.Wheel.Data;

namespace Project.Features.Wheel.Runtime
{
    public class WheelIndicator : MonoBehaviour
    {
        [Header("Kick Settings")]
        [SerializeField] private float kickAngle = 20f;
        [SerializeField] private float kickDuration = 0.15f;

        [Header("Refs")]
        [SerializeField] private Transform indicatorRoot;

        private bool _isAnimating;
        private WheelItemInstance _selected;

        private Sequence _kickSeq;

        public WheelItemInstance GetSelectedItem() => _selected;

        private void Awake()
        {
            if (indicatorRoot == null)
                indicatorRoot = transform.parent;
        }

        private void OnDisable()
        {
            _kickSeq?.Kill();
            _kickSeq = null;
            _isAnimating = false;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_isAnimating) return;

            if (!other.TryGetComponent(out WheelItemView itemView)) return;

            _selected = itemView.GetItem();

            PlayKick();
        }

        private void PlayKick()
        {
            _isAnimating = true;

            _kickSeq?.Kill();
            _kickSeq = DOTween.Sequence()
                .SetTarget(indicatorRoot)
                .Append(indicatorRoot.DORotate(new Vector3(0, 0, kickAngle), kickDuration))
                .Append(indicatorRoot.DORotate(Vector3.zero, kickDuration))
                .OnComplete(() => _isAnimating = false);
        }
    }
}
