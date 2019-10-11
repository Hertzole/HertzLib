using UnityEngine;
using UnityEngine.Serialization;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Hertzole.HertzLib
{
    [System.Serializable]
    public struct RandomFloat
    {
        [SerializeField]
        [FormerlySerializedAs("m_Min")]
        private float min;
        public float Min { get { return min; } set { min = value; } }
        [SerializeField]
        [FormerlySerializedAs("m_Max")]
        private float max;
        public float Max { get { return max; } set { max = value; } }

        public float Value { get { return Random.Range(Min, Max); } }

        public RandomFloat(float min, float max)
        {
            this.min = min;
            this.max = max;
        }

        public static implicit operator float(RandomFloat x)
        {
            return x.Value;
        }

        public static bool operator ==(float x, RandomFloat y)
        {
            return x == y.Value;
        }

        public static bool operator ==(RandomFloat x, float y)
        {
            return x.Value == y;
        }

        public static bool operator ==(RandomFloat x, RandomFloat y)
        {
            return x.Value == y.Value;
        }

        public static bool operator !=(float x, RandomFloat y)
        {
            return x != y.Value;
        }

        public static bool operator !=(RandomFloat x, float y)
        {
            return x.Value != y;
        }

        public static bool operator !=(RandomFloat x, RandomFloat y)
        {
            return x.Value != y.Value;
        }

        public static float operator +(float x, RandomFloat y)
        {
            return x + y.Value;
        }

        public static float operator +(RandomFloat x, float y)
        {
            return x.Value + y;
        }

        public static float operator +(RandomFloat x, RandomFloat y)
        {
            return x.Value + y.Value;
        }

        public static float operator -(float x, RandomFloat y)
        {
            return x - y.Value;
        }

        public static float operator -(RandomFloat x, float y)
        {
            return x.Value - y;
        }

        public static float operator -(RandomFloat x, RandomFloat y)
        {
            return x.Value - y.Value;
        }

        public static float operator /(float x, RandomFloat y)
        {
            return x / y.Value;
        }

        public static float operator /(RandomFloat x, float y)
        {
            return x.Value / y;
        }

        public static float operator /(RandomFloat x, RandomFloat y)
        {
            return x.Value / y.Value;
        }

        public static float operator *(float x, RandomFloat y)
        {
            return x * y.Value;
        }

        public static float operator *(RandomFloat x, float y)
        {
            return x.Value * y;
        }

        public static float operator *(RandomFloat x, RandomFloat y)
        {
            return x.Value * y.Value;
        }

        public static Vector2 operator *(RandomFloat x, Vector2 y)
        {
            return x.Value * y;
        }

        public static Vector2 operator *(Vector2 x, RandomFloat y)
        {
            return x * y.Value;
        }

        public static Vector3 operator *(RandomFloat x, Vector3 y)
        {
            return x.Value * y;
        }

        public static Vector3 operator *(Vector3 x, RandomFloat y)
        {
            return x * y.Value;
        }

        public static Vector4 operator *(RandomFloat x, Vector4 y)
        {
            return x.Value * y;
        }

        public static Vector4 operator *(Vector4 x, RandomFloat y)
        {
            return x * y.Value;
        }

        public static float operator %(float x, RandomFloat y)
        {
            return x % y.Value;
        }

        public static float operator %(RandomFloat x, float y)
        {
            return x.Value % y;
        }

        public static float operator %(RandomFloat x, RandomFloat y)
        {
            return x.Value % y.Value;
        }

        public override bool Equals(object obj)
        {
            try
            {
                return Value == ((RandomFloat)obj).Value;
            }
            catch
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
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
    [CustomPropertyDrawer(typeof(RandomFloat))]
    public class RandomFloatDrawer : PropertyDrawer
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
