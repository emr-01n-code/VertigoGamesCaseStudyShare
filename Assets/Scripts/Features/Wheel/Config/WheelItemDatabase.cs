using System.Collections.Generic;
using UnityEngine;
using VertigoCase.Features.Wheel.Data;

namespace Project.Features.Wheel.Config
{
    public class WheelItemDatabase : MonoBehaviour
    {
        public static WheelItemDatabase Instance { get; private set; }

        [Header("All Wheel Items (SO)")]
        [SerializeField] private List<WheelItemDataSO> items = new();

        private readonly Dictionary<string, WheelItemDataSO> _byKey = new();

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            BuildIndex();
        }

        private void OnDestroy()
        {
            if (Instance == this)
                Instance = null;
        }

        private void BuildIndex()
        {
            _byKey.Clear();

            foreach (var so in items)
            {
                if (so == null) continue;

                var key = so.Key;
                if (string.IsNullOrWhiteSpace(key)) continue;

                if (_byKey.ContainsKey(key))
                {
                    continue;
                }

                _byKey.Add(key, so);
            }
        }

        public bool TryGet(string key, out WheelItemDataSO so) => _byKey.TryGetValue(key, out so);
    }
}
