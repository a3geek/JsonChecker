using System;

namespace JsonChecker.Extensions
{
    public static class ArrayExtensions
    {
        public static T Get<T>(this T[] items, int index)
            => index >= 0 && index < items.Length ? items[index] : default;

        public static void ForEach<T>(this T[] items, Action<T> action, Func<T, bool> selector = null)
        {
            foreach (var item in items)
            {
                if (selector == null || selector(item))
                {
                    action(item);
                }
            }
        }

        public static void ForEach<T>(this T[] items, Action<int, T> action, Func<int, T, bool> selector = null)
        {
            for (var i = 0; i < items.Length; i++)
            {
                if (selector == null || selector(i, items[i]))
                {
                    action(i, items[i]);
                }
            }
        }
    }
}
