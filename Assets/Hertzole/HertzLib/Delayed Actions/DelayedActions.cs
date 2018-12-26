using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hertzole.HertzLib
{
    public static class DelayedActions
    {
#if HERTZLIB_UPDATE_MANAGER
        [DisallowMultipleComponent]
        [HelpURL("https://github.com/Hertzole/HertzLib/wiki/Delayed-Actions")]
        internal class DelayedActionsBehaviour : MonoBehaviour, ILateUpdate
#else
        internal class DelayedActionsBehaviour : MonoBehaviour
#endif
        {
            private struct ActionInfo
            {
                public float time;
                public Action action;

                public ActionInfo(float time, Action action)
                {
                    this.time = time;
                    this.action = action;
                }
            }

            private static DelayedActionsBehaviour instance;
            public static DelayedActionsBehaviour Instance
            {
                get
                {
                    if (!instance)
                    {
                        GameObject go = new GameObject("Delayed Action Manager");
                        instance = go.AddComponent<DelayedActionsBehaviour>();
                        DontDestroyOnLoad(go);
                    }

                    return instance;
                }
            }

            // Sets if the object is in the process of being destroyed.
            private static bool m_Destroying = false;

            // All the actions.
            private List<ActionInfo> m_Actions = new List<ActionInfo>();
            private static List<ActionInfo> Actions { get { return Instance.m_Actions; } }

            private void OnDestroy()
            {
                // Mark the object as destroyed.
                m_Destroying = true;
            }

            private void OnApplicationQuit()
            {
                // Mark the object as destroyed.
                m_Destroying = true;
            }

#if HERTZLIB_UPDATE_MANAGER
            private void OnEnable()
            {
                UpdateManager.AddLateUpdate(this);
            }

            private void OnDisable()
            {
                UpdateManager.RemoveLateUpdate(this);
            }

            public void OnLateUpdate()
#else
        private void LateUpdate()
#endif
            {
                // As long as this object isn't getting destroyed, run the check.
                if (!m_Destroying)
                {
                    // Reverse loop through the actions.
                    for (int i = m_Actions.Count - 1; i >= 0; i--)
                    {
                        // If the time is over the action's time, invoke it and then remove it.
                        if (Time.time >= m_Actions[i].time)
                        {
                            m_Actions[i].action.Invoke();
                            m_Actions.RemoveAt(i);
                        }
                    }
                }
            }

            /// <summary>
            /// Schedules a new action.
            /// </summary>
            /// <param name="action">The action you want to execute.</param>
            /// <param name="delay">The delay before the action is executed.</param>
            public void ScheduleAction(Action action, float delay)
            {
                // If the object isn't being destroyed, add the action.
                if (!m_Destroying)
                    Actions.Add(new ActionInfo(Time.time + delay, action));
            }
        }

        /// <summary>
        /// Schedules a new action.
        /// </summary>
        /// <param name="action">The action you want to execute.</param>
        /// <param name="delay">The delay before the action is executed.</param>
        public static void ScheduleAction(Action action, float delay)
        {
            // Make sure delayed actions isn't being used in the editor.
            if (!Application.isPlaying)
            {
                Debug.LogWarning("Delayed Actions can not be used outside the game!");
                return;
            }

            // Schedule a new action on the instance.
            if (DelayedActionsBehaviour.Instance)
                DelayedActionsBehaviour.Instance.ScheduleAction(action, delay);
        }
    }
}
