using System;
using System.Collections.Generic;

namespace VertigoCase.Shared.Events
{
    public static class EventManager
    {
        private static readonly Dictionary<Type, List<Delegate>> _map = new();

        public static void Subscribe<T>(Action<T> callback)
        {
            var type = typeof(T);

            if (!_map.TryGetValue(type, out var list))
            {
                list = new List<Delegate>();
                _map[type] = list;
            }

            // aynı callback iki kez eklenmesin (insan gibi küçük koruma)
            if (!list.Contains(callback))
                list.Add(callback);
        }

        public static void Unsubscribe<T>(Action<T> callback)
        {
            var type = typeof(T);

            if (!_map.TryGetValue(type, out var list))
                return;

            list.Remove(callback);

            if (list.Count == 0)
                _map.Remove(type);
        }

        public static void Publish<T>(T eventData)
        {
            var type = typeof(T);

            if (!_map.TryGetValue(type, out var list))
                return;

            // publish sırasında list değişirse sorun çıkmasın
            var snapshot = list.ToArray();
            for (int i = 0; i < snapshot.Length; i++)
            {
                if (snapshot[i] is Action<T> action)
                    action.Invoke(eventData);
            }
        }

        public static void ClearAll()
        {
            _map.Clear();
        }
    }
}
