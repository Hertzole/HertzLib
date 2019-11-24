using Hertzole.HertzLib;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.SceneManagement;

namespace Hertzole.Hertzlib.Tests
{
    public class HertzLibTest : MonoBehaviour
    {
        public string testString = "Test";
        [SerializeField]
        private string privateString;

        private float testFloat;
        private int testInt;

        private string evenMorePrivateString;

        public RandomInt randomInt;
        public RandomFloat randomFloat;

        [SerializeField]
        private Layer layerSelection;

        [SerializeField]
        private SceneObject gameScene = "";

        int[] numbers = new int[6] { 0, 1, 2, 3, 4, 5 };
        List<int> numberList = new List<int>(6) { 0, 1, 2, 3, 4, 5 };

        private void Start()
        {
            Profiler.BeginSample("Array for each");
            numbers.ForEach(x =>
            {
                // x is the array item.
            });
            Profiler.EndSample();

            Profiler.BeginSample("List for each");
            numberList.ForEach(x =>
            {
            });
            Profiler.EndSample();

            Vector2 mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Vector2Int tilePosition = mousePosition.ToInt();

            string decmialNumbersString = "1234.45";
            float result = decmialNumbersString.ParseFloatInvariant();
            Vector3 position = transform.position;
            if (position.IsNaN())
            {
                return;
            }

            if (randomFloat == 1f)
            {

            }

            if (randomInt == 3)
            {

            }

            if (randomFloat == testFloat)
            {

            }

            if (randomInt == testInt)
            {

            }

            if (gameObject.layer == layerSelection)
            {
            }
            layerSelection = gameObject.layer;

            testInt = randomInt;
            testFloat = randomFloat;

            Vector3 normalFloatMultiply = Vector3.one * testFloat;
            Vector3 normalFloatMultiplyInverse = testFloat * Vector3.one;
            Vector3 randomFloatMultiply = Vector3.one * randomFloat;
            Vector3 randomFloatMultiplyInverse = randomFloat * Vector3.one;

            Vector3Int normalIntMultiply = Vector3Int.one * testInt;
            Vector3Int randomIntMultiply = Vector3Int.one * testInt;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(gameScene);
            }

            Time.timeScale = 0;

            DelayedActions.ScheduleAction(() =>
            {
                Debug.Log("No timescale action!");
                Time.timeScale = 1;
            }, 5f, true);

            DelayedActions.ScheduleAction(() =>
            {
                Debug.Log("Delayed action after two seconds!");
            }, 2);

            DelayedActions.ScheduleAction(DelayedFunction, 5);
        }

        private void DelayedFunction()
        {
            Debug.Log("Hello world... after 5 seconds of wait!");
        }
    }
}
