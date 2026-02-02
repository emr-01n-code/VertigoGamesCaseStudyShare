using System.Collections.Generic;
using UnityEngine;
using Project.Features.Wheel.Data;
using VertigoCase.Features.Wheel.Data;

namespace Project.Features.Wheel.Config
{
    public class WheelRegistry : MonoBehaviour
    {
        [System.Serializable]
        public class ConfigPair
        {
            public SpinType type;
            public WheelConfigSO config;
        }

        public static WheelRegistry Instance { get; private set; }


        [SerializeField] private List<ConfigPair> configs = new();

        private Dictionary<SpinType, WheelConfigSO> _map;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            _map = new Dictionary<SpinType, WheelConfigSO>();

            for (int i = 0; i < configs.Count; i++)
            {
                var pair = configs[i];
                if (pair.config == null) continue;

                _map[pair.type] = pair.config;
            }
        }

        void OnDestroy()
        {
            if (Instance == this)
                Instance = null;
        }

        public WheelConfigSO GetConfig(SpinType type)
        {
            if (_map != null && _map.TryGetValue(type, out var config))
                return config;

            return null;
        }

        public SpinType GetTypeByZone(int zone)
        {
            if (zone % 30 == 0) return SpinType.Gold;
            if (zone % 5 == 0) return SpinType.Silver;

            return SpinType.Bronze;
        }

        public WheelConfigSO GetConfigByZone(int zone)
        {
            return GetConfig(GetTypeByZone(zone));
        }
    }
}
