using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Hertzole.HertzLib
{
    /// <summary>
    /// Class for layer selection in the editor.
    /// </summary>
    [System.Serializable]
    public struct Layer
    {
        [SerializeField]
        private int m_LayerID;
        [System.Obsolete("No longer needs to be used. You can directly reference the layer like a normal int.")]
        public int LayerID { get { return m_LayerID; } set { m_LayerID = value; } }

        public Layer(int layerId)
        {
            m_LayerID = layerId;
        }

        public static implicit operator int(Layer x)
        {
            return x.m_LayerID;
        }

        public static implicit operator Layer(int x)
        {
            return new Layer(x);
        }

        public static bool operator ==(Layer x, Layer y)
        {
            return x.m_LayerID == y.m_LayerID;
        }

        public static bool operator ==(Layer x, int y)
        {
            return x.m_LayerID == y;
        }

        public static bool operator ==(int x, Layer y)
        {
            return x == y.m_LayerID;
        }

        public static bool operator !=(Layer x, Layer y)
        {
            return x.m_LayerID != y.m_LayerID;
        }

        public static bool operator !=(Layer x, int y)
        {
            return x.m_LayerID != y;
        }

        public static bool operator !=(int x, Layer y)
        {
            return x != y.m_LayerID;
        }

        public override bool Equals(object obj)
        {
            try
            {
                return this == (Layer)obj;
            }
            catch
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return m_LayerID;
        }

        public override string ToString()
        {
            return LayerMask.LayerToName(m_LayerID);
        }
    }
}

#if UNITY_EDITOR
namespace Hertzole.HertzLib.Editor
{
    [CustomPropertyDrawer(typeof(Layer))]
    public class LayerPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GUIStyle labelStyle = new GUIStyle(EditorStyles.popup)
            {
                fontStyle = property.prefabOverride ? FontStyle.Bold : FontStyle.Normal
            };

            GUIContent newLabel = new GUIContent(property.displayName, property.tooltip);
            property.FindPropertyRelative("m_LayerID").intValue = EditorGUI.LayerField(position, newLabel, property.FindPropertyRelative("m_LayerID").intValue, labelStyle);
        }
    }
}
#endif
