using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Hertzole.HertzLib
{
    [RequireComponent(typeof(Selectable))]
    [HelpURL("https://github.com/Hertzole/HertzLib/wiki/UI-Selectable-Sounds")]
    public class UISelectableSounds : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField]
        [Tooltip("If true, the sound will play from this Game Object. Else it will use a global audio source.")]
        [FormerlySerializedAs("m_PlayAudioOnThis")]
        private bool playAudioOnThis = false;

#if UNITY_EDITOR
        [Header("Sounds")]
#endif
        [SerializeField]
        [Tooltip("The sound that plays when the cursor enters the selectable.")]
        [FormerlySerializedAs("m_EnterSound")]
        private AudioClip enterSound = null;
        [SerializeField]
        [Tooltip("If true, the enter sound will play when the selectable isn't interactable.")]
        [FormerlySerializedAs("m_PlayEnterWhenDisabled")]
        private bool playEnterWhenDisabled = false;
        [SerializeField]
        [Tooltip("The sound that plays when the cursor exits the selectable.")]
        [FormerlySerializedAs("m_ExitSound")]
        private AudioClip exitSound = null;
        [SerializeField]
        [Tooltip("If true, the exit sound will play when the selectable isn't interactable.")]
        [FormerlySerializedAs("m_PlayExitWhenDisabled")]
        private bool playExitWhenDisabled = false;
        [SerializeField]
        [Tooltip("The sound that plays when the user presses the selectable.")]
        [FormerlySerializedAs("m_ClickSound")]
        private AudioClip clickSound = null;
        [SerializeField]
        [Tooltip("The sound that plays when the user presses the selectable and it isn't interactable.")]
        [FormerlySerializedAs("m_ClickDisabledSound")]
        private AudioClip clickDisabledSound = null;

#if UNITY_EDITOR
        [Header("Audio Settings")]
#endif
        [SerializeField]
        [Tooltip("The volume of the sound.")]
        [FormerlySerializedAs("m_Volume")]
        private float volume = 1f;
        [SerializeField]
        [Tooltip("The pitch of the sound.")]
        [FormerlySerializedAs("m_Pitch")]
        private float pitch = 1f;
        [SerializeField]
        [Tooltip("The mixer group of the sound.")]
        [FormerlySerializedAs("m_MixerGroup")]
        private AudioMixerGroup mixerGroup = null;

        // The selectable on this object.
        [SerializeField]
        [HideInInspector]
        private Selectable selectable;

        /// <summary> If true, the sound will play from this Game Object. Else it will use a global audio source. </summary>
        public bool PlayAudioOnThis { get { return playAudioOnThis; } set { playAudioOnThis = value; OnPlayAudioOnThis(); } }
        /// <summary> If true, the enter sound will play when the selectable isn't interactable. </summary>
        public bool PlayEnterWhenDisabled { get { return playEnterWhenDisabled; } set { playEnterWhenDisabled = value; } }
        /// <summary> If true, the exit sound will play when the selectable isn't interactable. </summary>
        public bool PlayExitWhenDisabled { get { return playExitWhenDisabled; } set { playExitWhenDisabled = value; } }
        /// <summary> The sound that plays when the cursor enters the selectable. </summary>
        public AudioClip EnterSound { get { return enterSound; } set { enterSound = value; } }
        /// <summary> The sound that plays when the cursor exits the selectable. </summary>
        public AudioClip ExitSound { get { return exitSound; } set { exitSound = value; } }
        /// <summary> The sound that plays when the user presses the selectable. </summary>
        public AudioClip ClickSound { get { return clickSound; } set { clickSound = value; } }
        /// <summary> The sound that plays when the user presses the selectable and it isn't interactable. </summary>
        public AudioClip ClickDisabled { get { return clickDisabledSound; } set { clickDisabledSound = value; } }
        /// <summary> The volume of the sound. </summary>
        public float Volume { get { return volume; } set { volume = value; } }
        /// <summary> The pitch of the sound. </summary>
        public float Pitch { get { return pitch; } set { pitch = value; } }
        /// <summary> The mixer group of the sound. </summary>
        public AudioMixerGroup MixerGroup { get { return mixerGroup; } set { mixerGroup = value; } }

        // The audio source on this object.
        private AudioSource thisAudioSource;

        // The global audio source.
        private static AudioSource globalSource;
        private static AudioSource GlobalSource
        {
            get
            {
                // If there's no global audio source, create it.
                if (globalSource == null)
                {
                    GameObject go = new GameObject("Global UI Sounds");
                    DontDestroyOnLoad(go);
                    globalSource = go.AddComponent<AudioSource>();
                }

                return globalSource;
            }
        }

        private void Awake()
        {
            // Get the selectable if required.
            if (selectable == null)
            {
                selectable = GetComponent<Selectable>();
            }

            // Set up the play audio on this if required.
            OnPlayAudioOnThis();
        }

        private void OnPlayAudioOnThis()
        {
            // If play on this is enabled, get the audio source, or add it.
            if (playAudioOnThis)
            {
                if (thisAudioSource == null)
                {
                    thisAudioSource = GetComponent<AudioSource>();

                    if (thisAudioSource == null)
                    {
                        thisAudioSource = gameObject.AddComponent<AudioSource>();
                    }
                }
            }
        }

        /// <summary>
        /// Plays the sound and sets all the values.
        /// </summary>
        /// <param name="clip"></param>
        private void PlaySound(AudioClip clip)
        {
            // Stop if there's no sound clip.
            if (clip == null)
            {
                return;
            }

            // If play on this is enabled, set all the values on this audio source.
            // Else set it on the global source.
            if (playAudioOnThis)
            {
                thisAudioSource.volume = volume;
                thisAudioSource.pitch = pitch;
                thisAudioSource.outputAudioMixerGroup = mixerGroup;

                thisAudioSource.clip = clip;
                thisAudioSource.Play();
            }
            else
            {
                GlobalSource.volume = volume;
                GlobalSource.pitch = pitch;
                GlobalSource.outputAudioMixerGroup = mixerGroup;

                GlobalSource.clip = clip;
                GlobalSource.Play();
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            // Make sure the user clicks with the left mouse button.
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                // If it's interactable, play the normal sound.
                // Else, play the disabled sound.
                if (selectable.interactable)
                {
                    PlaySound(clickSound);
                }
                else
                {
                    PlaySound(clickDisabledSound);
                }
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (selectable.interactable || playExitWhenDisabled)
            {
                PlaySound(exitSound);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (selectable.interactable || playEnterWhenDisabled)
            {
                PlaySound(enterSound);
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            // Cache the selectable in the editor if possible.
            if (selectable == null)
            {
                selectable = GetComponent<Selectable>();
            }

            // Make sure to do the play audio on this setup if changed in the editor at runtime.
            if (Application.isPlaying)
            {
                OnPlayAudioOnThis();
            }
        }

        private void Reset()
        {
            // Cache the selectable in the editor if possible.
            selectable = GetComponent<Selectable>();
        }
#endif
    }
}
