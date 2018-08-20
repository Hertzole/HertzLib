using Hertzole.HertzLib;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        private SceneObject m_GameScene;

        private void Start()
        {
            int[] numbers = new int[6] { 0, 1, 2, 3, 4, 5 };
            numbers.ForEach(x =>
            {
                // x is the array item.
                Debug.Log(x);
            });

            List<int> numberList = new List<int>(6) { 0, 1, 2, 3, 4, 5 };
            numberList.ForEach(x =>
            {

            });

            Vector2 mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Vector2Int tilePosition = mousePosition.ToInt();

            string decmialNumbersString = "1234.45";
            float result = decmialNumbersString.ParseFloat();
            Vector3 position = transform.position;
            if (position.IsNaN())
                return;


            if (randomInt == 3)
            {

            }

            SceneManager.LoadScene(m_GameScene);
        }
    }
}
