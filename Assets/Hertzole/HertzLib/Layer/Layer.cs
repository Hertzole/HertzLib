using UnityEngine;
using UnityEngine.Serialization;
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
        [FormerlySerializedAs("m_LayerID")]
        private int layerID;
        [System.Obsolete("No longer needs to be used. You can directly reference the layer like a normal int.")]
        public int LayerID { get { return layerID; } set { layerID = value; } }

        public Layer(int layerId)
        {
            layerID = layerId;
        }

        public static implicit operator int(Layer x)
        {
            return x.layerID;
        }

        public static implicit operator Layer(int x)
        {
            return new Layer(x);
        }

        public static bool operator ==(Layer x, Layer y)
        {
            return x.layerID == y.layerID;
        }

        public static bool operator ==(Layer x, int y)
        {
            return x.layerID == y;
        }

        public static bool operator ==(int x, Layer y)
        {
            return x == y.layerID;
        }

        public static bool operator !=(Layer x, Layer y)
        {
            return x.layerID != y.layerID;
        }

        public static bool operator !=(Layer x, int y)
        {
            return x.layerID != y;
        }

        public static bool operator !=(int x, Layer y)
        {
            return x != y.layerID;
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
            return layerID;
        }

        public override string ToString()
        {
            return LayerMask.LayerToName(layerID);
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
            property.FindPropertyRelative("layerID").intValue = EditorGUI.LayerField(position, newLabel, property.FindPropertyRelative("layerID").intValue, labelStyle);
        }
    }
}
#endif
