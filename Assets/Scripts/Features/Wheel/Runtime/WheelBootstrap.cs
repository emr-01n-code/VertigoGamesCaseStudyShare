using UnityEngine;
using VertigoCase.Features.Wheel.Data;

namespace Project.Features.Wheel.Runtime
{
    public class WheelBootstrap : MonoBehaviour
    {
        [SerializeField] private WheelItemInitializer initializer;
        [SerializeField] private WheelConfigSO testConfig;

        private void Start()
        {
            initializer.SetItems(testConfig);
        }
    }
}
