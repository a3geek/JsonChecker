using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JsonChecker.Extensions
{
    public static class ListExtensions
    {
        public static int AddAndGetIndex<T>(this List<T> items, T item)
        {
            items.Add(item);
            return items.Count - 1;
        }

        public static bool Remove<T>(this List<T> items, Func<T, bool> selector)
        {
            for (var i = 0; i < items.Count; i++)
            {
                if (selector(items[i]) == false)
                {
                    continue;
                }

                items.RemoveAt(i);
                return true;
            }

            return false;
        }

        public static void Remove<T>(this List<T> items, IEnumerable<int> indexs)
        {
            var cnt = 0;
            foreach (var index in indexs)
            {
                items.RemoveAt(index - cnt);
                cnt++;
            }
        }

        public static T TakeLast<T>(this List<T> items)
        {
            if (items.Count <= 0)
            {
                return default;
            }

            var idx = items.Count - 1;
            var item = items[idx];
            items.RemoveAt(idx);

            return item;
        }

        public static int IndexOf<T>(this List<T> items, Func<T, bool> checker)
        {
            for (var i = 0; i < items.Count; i++)
            {
                if (checker(items[i]) == true)
                {
                    return i;
                }
            }

            return -1;
        }

        public static void SafetyAdd<T>(this List<T> items, T item)
        {
            if (item != null)
            {
                items.Add(item);
            }
        }

        public static bool SafetyRemove<T>(this List<T> items, T item)
            => item != null && items.Remove(item);

        public static void SetCount<T>(this List<T> items, int count, Func<int, T> creator, Action<int, T> destroyer)
        {
            if (items.Count == count)
            {
                return;
            }

            for (var i = count; i < items.Count; i++)
            {
                destroyer?.Invoke(i, items[i]);
            }
            items.RemoveRange(Mathf.Min(count, items.Count), Mathf.Max(items.Count - count, 0));

            for (var i = items.Count; i < count; i++)
            {
                items.Add(creator(i));
            }
        }

        public static void ForEachWith<T1, T2>(this List<T1> items, List<T2> partner, Action<T1, T2> action)
        {
            for (var i = 0; i < items.Count && i < partner.Count; i++)
            {
                action(items[i], partner[i]);
            }
        }

        public static List<T2> ConvertAll<T1, T2>(this List<T1> items, Func<int, T1, T2> converter)
        {
            var list = new List<T2>();
            for (var i = 0; i < items.Count; i++)
            {
                list.Add(converter(i, items[i]));
            }

            return list;
        }

        public static void Clear<T>(this List<T> items, Action<T> cleaner)
        {
            for (var i = 0; i < items.Count; i++)
            {
                cleaner(items[i]);
            }

            items.Clear();
        }
    }
}
