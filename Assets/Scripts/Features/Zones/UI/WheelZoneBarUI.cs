using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VertigoCase.Shared.Events;

namespace Project.Features.Zones.UI
{
    public class WheelZoneBarUI : MonoBehaviour
    {
        [Header("Refs")]
        [SerializeField] private RectTransform numbersGroup;
        [SerializeField] private ZoneNumberItemUI itemPrefab;
        [SerializeField] private TMP_Text currentZoneText;
        [SerializeField] private Image currentZoneBg;

        [Header("Settings")]
        [SerializeField] private int totalZones = 120;
        [SerializeField] private float offsetPerZone = 145f;
        [SerializeField] private float tweenDuration = 0.25f;

        [Header("Colors")]
        [SerializeField] private Color normalColor = Color.white;
        [SerializeField] private Color safeColor = Color.green;
        [SerializeField] private Color superColor = Color.yellow;
        [SerializeField] private Color currentNumberColor = Color.black;

        private readonly List<ZoneNumberItemUI> _items = new();
        private Vector2 _initialPosition;

        private void Awake()
        {
            _initialPosition = numbersGroup.anchoredPosition;
            BuildItems();
        }

        private void Start()
        {
            // İlk başlangıçta 1. bölgeyi ayarla
            ApplyUI(1, instant: true);
        }

        private void OnEnable()
        {
            EventManager.Subscribe<WheelZoneChangedEvent>(OnZoneChanged);
        }

        private void OnDisable()
        {
            EventManager.Unsubscribe<WheelZoneChangedEvent>(OnZoneChanged);
        }

        private void BuildItems()
        {
            foreach (Transform child in numbersGroup) Destroy(child.gameObject);
            _items.Clear();

            for (int i = 1; i <= totalZones; i++)
            {
                var item = Instantiate(itemPrefab, numbersGroup);
                // Burada her sayının kendi rengini başlangıçta veriyoruz
                item.Set(i, GetZoneColor(i));
                _items.Add(item);
            }
        }

        private void OnZoneChanged(WheelZoneChangedEvent e)
        {
            //int zone = Mathf.Clamp(e.Zone, 1, totalZones); // Güvenlik kontrolü
            ApplyUI(e.Zone, instant: false);
        }

        private void ApplyUI(int zone, bool instant)
        {
            // 1. Ortadaki Sabit Sayı ve Arkaplanı Güncelle
            //if (currentZoneText != null) currentZoneText.text = zone.ToString();
            if (currentZoneText != null)
            {
                currentZoneText.text = zone.ToString();
                currentZoneText.color = currentNumberColor;
            }
            if (currentZoneBg != null) currentZoneBg.color = GetZoneColor(zone);

            // int index = zone - 1;
            // for (int i = 0; i < _items.Count; i++)
            // {
            //     bool isCurrent = (i == index);
            //     int zoneNumber = i + 1;
            //     _items[i].Set(zoneNumber, GetZoneColor(zoneNumber), isCurrent);
            // }

            float targetX = _initialPosition.x - ((zone - 1) * offsetPerZone);

            numbersGroup.DOKill(); // Önceki animasyonu durdur
            if (instant)
                numbersGroup.anchoredPosition = new Vector2(targetX, numbersGroup.anchoredPosition.y);
            else
                numbersGroup.DOAnchorPosX(targetX, tweenDuration).SetEase(Ease.OutCubic);

            // 3. (Opsiyonel) Sayıların kendi renklerini güncelleme
            // Eğer sayıların rengi kaydıkça değişsin istemiyorsan BuildItems'daki renk yeterli.
        }

        private Color GetZoneColor(int zone)
        {
            if (zone % 30 == 0) return superColor;
            if (zone % 5 == 0) return safeColor;
            return normalColor;
        }
    }
}