using System.Collections.Generic;
using UnityEngine;

namespace Lustie.GameFlow
{
    public static class ListExtensions
    {
        /// <summary>
        /// Removes the element at the specified index and swaps the last element into its place. 
        /// This is more efficient than moving all elements following the removed element, but does change the order of elements in the buffer.
        /// </summary>
        internal static List<T> RemoveAtSwapBack<T>(this List<T> source, int index)
        {
            if (source == null || index < 0 || index >= source.Count)
            {
#if UNITY_EDITOR
                Debug.LogWarning($"Remove Swap Back: index is out of range: {index}");
#endif
                return source;
            }

            int count = source.Count;
            source[index] = source[count - 1];
            source.RemoveAt(count - 1);

            return source;
        }

        /// <summary>
        /// Removes the element with the specified value and swaps the last element into its place. 
        /// This is more efficient than moving all elements following the removed element, but does change the order of elements in the buffer.
        /// </summary>
        internal static List<T> RemoveSwapBack<T>(this List<T> source, T value)
        {
            int index = source.IndexOf(value);
            return RemoveAtSwapBack(source, index);
        }
    }
}