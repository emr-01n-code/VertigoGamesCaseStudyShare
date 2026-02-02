using UnityEngine;

namespace Project.Features.Wheel.Core
{
    public class WheelSpinLogic : IWheelSpinLogic
    {
        private readonly int _fullRotations;

        public WheelSpinLogic(int fullRotations = 6)
        {
            _fullRotations = Mathf.Max(0, fullRotations);
        }

        public float CalculateSpinTargetAngle(int segmentCount)
        {
            segmentCount = Mathf.Max(1, segmentCount);

            int randomSegment = Random.Range(0, segmentCount);
            float segmentAngle = 360f / segmentCount;

            return 360f * _fullRotations + randomSegment * segmentAngle;
        }
    }
}
