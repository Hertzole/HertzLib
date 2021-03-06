using UnityEngine;
using UnityEngine.Serialization;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Hertzole.HertzLib
{
    [System.Serializable]
    public struct RandomInt
    {
        [SerializeField]
        [FormerlySerializedAs("m_Min")]
        private int min;
        public int Min { get { return min; } set { min = value; } }
        [SerializeField]
        [FormerlySerializedAs("m_Max")]
        private int max;
        public int Max { get { return max; } set { max = value; } }

        public int Value { get { return Random.Range(Min, Max); } }

        public RandomInt(int min, int max)
        {
            this.min = min;
            this.max = max;
        }

        public static implicit operator int(RandomInt x)
        {
            return x.Value;
        }

        public static bool operator ==(int x, RandomInt y)
        {
            return x == y.Value;
        }

        public static bool operator ==(RandomInt x, int y)
        {
            return x.Value == y;
        }

        public static bool operator ==(RandomInt x, RandomInt y)
        {
            return x.Value == y.Value;
        }

        public static bool operator !=(int x, RandomInt y)
        {
            return x != y.Value;
        }

        public static bool operator !=(RandomInt x, int y)
        {
            return x.Value != y;
        }

        public static bool operator !=(RandomInt x, RandomInt y)
        {
            return x.Value != y.Value;
        }

        public static int operator +(int x, RandomInt y)
        {
            return x + y.Value;
        }

        public static int operator +(RandomInt x, int y)
        {
            return x.Value + y;
        }

        public static int operator +(RandomInt x, RandomInt y)
        {
            return x.Value + y.Value;
        }

        public static int operator -(int x, RandomInt y)
        {
            return x - y.Value;
        }

        public static int operator -(RandomInt x, int y)
        {
            return x.Value - y;
        }

        public static int operator -(RandomInt x, RandomInt y)
        {
            return x.Value - y.Value;
        }

        public static int operator /(int x, RandomInt y)
        {
            return x / y.Value;
        }

        public static int operator /(RandomInt x, int y)
        {
            return x.Value / y;
        }

        public static int operator /(RandomInt x, RandomInt y)
        {
            return x.Value / y.Value;
        }

        public static int operator *(int x, RandomInt y)
        {
            return x * y.Value;
        }

        public static int operator *(RandomInt x, int y)
        {
            return x.Value * y;
        }

        public static int operator *(RandomInt x, RandomInt y)
        {
            return x.Value * y.Value;
        }

        public static Vector2Int operator *(Vector2Int x, RandomInt y)
        {
            return x * y.Value;
        }

        public static Vector3Int operator *(Vector3Int x, RandomInt y)
        {
            return x * y.Value;
        }

        public static int operator %(int x, RandomInt y)
        {
            return x % y.Value;
        }

        public static int operator %(RandomInt x, int y)
        {
            return x.Value % y;
        }

        public static int operator %(RandomInt x, RandomInt y)
        {
            return x.Value % y.Value;
        }

        public override bool Equals(object obj)
        {
            try
            {
                return Value == ((RandomInt)obj).Value;
            }
            catch
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return Value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public string ToString(string format)
        {
            return Value.ToString(format);
        }

        public string ToString(System.IFormatProvider provider)
        {
            return Value.ToString(provider);
        }

        public string ToString(string format, System.IFormatProvider provider)
        {
            return Value.ToString(format, provider);
        }
    }
}

#if UNITY_EDITOR
namespace Hertzole.HertzLib.Editor
{
    [CustomPropertyDrawer(typeof(RandomInt))]
    public class RandomIntDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.LabelField(new Rect(position.x, position.y, EditorGUIUtility.labelWidth, position.height), label);
            EditorGUI.PrefixLabel(new Rect(position.x + EditorGUIUtility.labelWidth, position.y, 25, position.height), new GUIContent("Min"));
            EditorGUI.PropertyField(new Rect(position.x + EditorGUIUtility.labelWidth + 25, position.y, ((position.width - EditorGUIUtility.labelWidth) / 2) - 27, position.height), property.FindPropertyRelative("min"), GUIContent.none);
            EditorGUI.PrefixLabel(new Rect(position.x + EditorGUIUtility.labelWidth + ((position.width - EditorGUIUtility.labelWidth) / 2) + 2, position.y, 27, position.height), new GUIContent("Max"));
            EditorGUI.PropertyField(new Rect(position.x + EditorGUIUtility.labelWidth + ((position.width - EditorGUIUtility.labelWidth) / 2) + 31, position.y, ((position.width - EditorGUIUtility.labelWidth) / 2) - 31, position.height), property.FindPropertyRelative("max"), GUIContent.none);
        }
    }
}
#endif
