using Hertzole.HertzLib;
using UnityEngine;

namespace Hertzole.Hertzlib.Tests
{
    public class HertzLibTest : MonoBehaviour
    {
        public string testString = "Test";
        [SerializeField]
        private string m_PrivateString;

        private string evenMorePrivateString;

        public RandomInt randomInt;
        public RandomFloat randomFloat;

        [SerializeField]
        private Layer m_LayerSelection;

        [SerializeField]
        private SceneObject m_ThisScene;
    }
}
