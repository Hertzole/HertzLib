#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Hertzole.HertzLib
{
    [System.Serializable]
    public struct SceneObject
    {
        [SerializeField]
        [FormerlySerializedAs("m_SceneName")]
        private string sceneName;

        public static implicit operator string(SceneObject sceneObject)
        {
            return sceneObject.sceneName;
        }

        public static implicit operator SceneObject(string sceneName)
        {
            return new SceneObject() { sceneName = sceneName };
        }

        public static bool operator ==(string x, SceneObject y)
        {
            return x == y.sceneName;
        }

        public static bool operator ==(SceneObject x, string y)
        {
            return x.sceneName == y;
        }

        public static bool operator !=(string x, SceneObject y)
        {
            return x != y.sceneName;
        }

        public static bool operator !=(SceneObject x, string y)
        {
            return x.sceneName != y;
        }

        public static bool operator ==(Scene x, SceneObject y)
        {
            return x.name == y.sceneName;
        }

        public static bool operator ==(SceneObject x, Scene y)
        {
            return x.sceneName == y.name;
        }

        public static bool operator !=(Scene x, SceneObject y)
        {
            return x.name != y.sceneName;
        }

        public static bool operator !=(SceneObject x, Scene y)
        {
            return x.sceneName != y.name;
        }

        public override bool Equals(object obj)
        {
            if (obj is SceneObject sceneObj)
            {
                return sceneName == sceneObj.sceneName;
            }
            else if (obj is string sceneString)
            {
                return sceneName == sceneString;
            }
            else if (obj is Scene scene)
            {
                return sceneName == scene.name;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}

#if UNITY_EDITOR
namespace Hertzole.HertzLib.Editor
{
    [CustomPropertyDrawer(typeof(SceneObject))]
    public class SceneObjectEditor : PropertyDrawer
    {
        protected SceneAsset GetSceneObject(string sceneObjectName)
        {
            if (string.IsNullOrEmpty(sceneObjectName))
            {
                return null;
            }

            for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
            {
                EditorBuildSettingsScene scene = EditorBuildSettings.scenes[i];
                if (scene.path.IndexOf(sceneObjectName) != -1)
                {
                    return AssetDatabase.LoadAssetAtPath(scene.path, typeof(SceneAsset)) as SceneAsset;
                }
            }

            Debug.Log("Scene [" + sceneObjectName + "] cannot be used. Add this scene to the 'Scenes in the Build' in the build settings.");
            return null;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SceneAsset sceneObj = GetSceneObject(property.FindPropertyRelative("sceneName").stringValue);
            Object newScene = EditorGUI.ObjectField(position, label, sceneObj, typeof(SceneAsset), false);
            if (newScene == null)
            {
                SerializedProperty prop = property.FindPropertyRelative("sceneName");
                prop.stringValue = "";
            }
            else
            {
                if (newScene.name != property.FindPropertyRelative("sceneName").stringValue)
                {
                    SceneAsset scnObj = GetSceneObject(newScene.name);
                    if (scnObj == null)
                    {
                        Debug.LogWarning("The scene " + newScene.name + " cannot be used. To use this scene add it to the build settings for the project.");
                    }
                    else
                    {
                        SerializedProperty prop = property.FindPropertyRelative("sceneName");
                        prop.stringValue = newScene.name;
                    }
                }
            }
        }
    }
}
#endif
