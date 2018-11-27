using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace Hertzole.HertzLib
{
    public static class Extensions
    {
        /// <summary>
        /// Gets a component or adds it if it's null.
        /// </summary>
        public static T GetOrAddComponent<T>(this GameObject target) where T : Component
        {
            T component = target.GetComponent<T>();

            if (component == null)
                component = target.AddComponent<T>();

            return component;
        }

        /// <summary>
        /// Returns true if the list is null or has 0 objects.
        /// </summary>
        public static bool IsNullOrEmpty<T>(this List<T> list)
        {
            return list == null || list.Count == 0;
        }

        /// <summary>
        /// Returns true if the array is null or has 0 objects.
        /// </summary>
        public static bool IsNullOrEmpty<T>(this T[] array)
        {
            return array == null || array.Length == 0;
        }

        /// <summary>
        /// Randomly shuffles the list.
        /// </summary>
        public static void Shuffle<T>(this List<T> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                T temp = list[i];
                int randomIndex = Random.Range(i, list.Count);
                list[i] = list[randomIndex];
                list[randomIndex] = temp;
            }
        }

        /// <summary>
        /// Randomly shuffles the array.
        /// </summary>
        public static void Shuffle<T>(this T[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                T temp = array[i];
                int randomIndex = Random.Range(i, array.Length);
                array[i] = array[randomIndex];
                array[randomIndex] = temp;
            }
        }

        /// <summary>
        /// Executes an action for every object in the array.
        /// </summary>
        public static void ForEach<T>(this T[] source, System.Action<T> action)
        {
            for (int i = 0; i < source.Length; i++)
            {
                int index = i;
                action(source[index]);
            }
        }

        /// <summary>
        /// Converts a normal Vector2 to a Vector2Int.
        /// </summary>
        public static Vector2Int ToInt(this Vector2 v)
        {
            return new Vector2Int(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y));
        }

        /// <summary>
        /// Converts a normal Vector3 to a Vector3Int.
        /// </summary>
        public static Vector3Int ToInt(this Vector3 v)
        {
            return new Vector3Int(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y), Mathf.RoundToInt(v.z));
        }

        /// <summary>
        /// Tries to parse a string to a float with no culture.
        /// </summary>
        public static bool TryFloatParse(this string s, out float result)
        {
            return float.TryParse(s, NumberStyles.Float | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out result);
        }

        /// <summary>
        /// Parses a string to a float with no culture.
        /// </summary>
        public static float ParseFloat(this string s)
        {
#if NET_4_6 || (UNITY_2018_3_OR_NEWER && !NET_LEGACY)
            if (string.IsNullOrWhiteSpace(s))
                s = "0";
#else
            if (string.IsNullOrEmpty(s.Trim()))
                s = "0";
#endif

            float val;
            return s.TryFloatParse(out val) ? val : 0;
        }

        /// <summary>
        /// Tries to parse a string to a int with no culture.
        /// </summary>
        public static bool TryIntParse(this string s, out int result)
        {
            return int.TryParse(s, NumberStyles.Float | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out result);
        }

        /// <summary>
        /// Parses a string to a int with no culture.
        /// </summary>
        public static int ParseInt(this string s)
        {
#if NET_4_6 || (UNITY_2018_3_OR_NEWER && !NET_LEGACY)
            if (string.IsNullOrWhiteSpace(s))
                s = "0";
#else
            if (string.IsNullOrEmpty(s.Trim()))
                s = "0";
#endif

            int val;
            return s.TryIntParse(out val) ? val : 0;
        }

        /// <summary>
        /// Checks if this float is NaN.
        /// </summary>
        public static bool IsNaN(this float f)
        {
            return float.IsNaN(f);
        }

        /// <summary>
        /// Checks if this Vector2 is NaN.
        /// </summary>
        public static bool IsNaN(this Vector2 vector)
        {
            return float.IsNaN(vector.x) || float.IsNaN(vector.y);
        }

        /// <summary>
        /// Checks if this Vector3 is NaN.
        /// </summary>
        public static bool IsNaN(this Vector3 vector)
        {
            return float.IsNaN(vector.x) || float.IsNaN(vector.y) || float.IsNaN(vector.z);
        }

        /// <summary>
        /// Checks if this Vector4 is NaN.
        /// </summary>
        public static bool IsNaN(this Vector4 vector)
        {
            return float.IsNaN(vector.x) || float.IsNaN(vector.y) || float.IsNaN(vector.z) || float.IsNaN(vector.w);
        }

        /// <summary>
        /// Checks if this Quaternion is NaN.
        /// </summary>
        public static bool IsNaN(this Quaternion q)
        {
            return float.IsNaN(q.x) || float.IsNaN(q.y) || float.IsNaN(q.z) || float.IsNaN(q.w);
        }
    }
}
