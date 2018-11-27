using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Hertzole.HertzLib
{
    [RequireComponent(typeof(Selectable))]
    [HelpURL("https://github.com/Hertzole/HertzLib/wiki/UI-Selectable-Sounds")]
    public class UISelectableSounds : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField]
        [Tooltip("If true, the sound will play from this Game Object. Else it will use a global audio source.")]
        private bool m_PlayAudioOnThis = false;

#if UNITY_EDITOR
        [Header("Sounds")]
#endif
        [SerializeField]
        [Tooltip("The sound that plays when the cursor enters the selectable.")]
        private AudioClip m_EnterSound = null;
        [SerializeField]
        [Tooltip("If true, the enter sound will play when the selectable isn't interactable.")]
        private bool m_PlayEnterWhenDisabled = false;
        [SerializeField]
        [Tooltip("The sound that plays when the cursor exits the selectable.")]
        private AudioClip m_ExitSound = null;
        [SerializeField]
        [Tooltip("If true, the exit sound will play when the selectable isn't interactable.")]
        private bool m_PlayExitWhenDisabled = false;
        [SerializeField]
        [Tooltip("The sound that plays when the user presses the selectable.")]
        private AudioClip m_ClickSound = null;
        [SerializeField]
        [Tooltip("The sound that plays when the user presses the selectable and it isn't interactable.")]
        private AudioClip m_ClickDisabledSound = null;

#if UNITY_EDITOR
        [Header("Audio Settings")]
#endif
        [SerializeField]
        [Tooltip("The volume of the sound.")]
        private float m_Volume = 1f;
        [SerializeField]
        [Tooltip("The pitch of the sound.")]
        private float m_Pitch = 1f;
        [SerializeField]
        [Tooltip("The mixer group of the sound.")]
        private AudioMixerGroup m_MixerGroup = null;

        /// <summary> If true, the sound will play from this Game Object. Else it will use a global audio source. </summary>
        public bool PlayAudioOnThis { get { return m_PlayAudioOnThis; } set { m_PlayAudioOnThis = value; OnPlayAudioOnThis(); } }
        /// <summary> If true, the enter sound will play when the selectable isn't interactable. </summary>
        public bool PlayEnterWhenDisabled { get { return m_PlayEnterWhenDisabled; } set { m_PlayEnterWhenDisabled = value; } }
        /// <summary> If true, the exit sound will play when the selectable isn't interactable. </summary>
        public bool PlayExitWhenDisabled { get { return m_PlayExitWhenDisabled; } set { m_PlayExitWhenDisabled = value; } }
        /// <summary> The sound that plays when the cursor enters the selectable. </summary>
        public AudioClip EnterSound { get { return m_EnterSound; } set { m_EnterSound = value; } }
        /// <summary> The sound that plays when the cursor exits the selectable. </summary>
        public AudioClip ExitSound { get { return m_ExitSound; } set { m_ExitSound = value; } }
        /// <summary> The sound that plays when the user presses the selectable. </summary>
        public AudioClip ClickSound { get { return m_ClickSound; } set { m_ClickSound = value; } }
        /// <summary> The sound that plays when the user presses the selectable and it isn't interactable. </summary>
        public AudioClip ClickDisabled { get { return m_ClickDisabledSound; } set { m_ClickDisabledSound = value; } }
        /// <summary> The volume of the sound. </summary>
        public float Volume { get { return m_Volume; } set { m_Volume = value; } }
        /// <summary> The pitch of the sound. </summary>
        public float Pitch { get { return m_Pitch; } set { m_Pitch = value; } }
        /// <summary> The mixer group of the sound. </summary>
        public AudioMixerGroup MixerGroup { get { return m_MixerGroup; } set { m_MixerGroup = value; } }

        // The audio source on this object.
        private AudioSource m_ThisAudioSource;

        // The selectable on this object.
        private Selectable m_Selectable;

        // The global audio source.
        private static AudioSource s_GlobalSource;
        private static AudioSource GlobalSource
        {
            get
            {
                // If there's no global audio source, create it.
                if (s_GlobalSource == null)
                {
                    GameObject go = new GameObject("Global UI Sounds");
                    DontDestroyOnLoad(go);
                    s_GlobalSource = go.AddComponent<AudioSource>();
                }

                return s_GlobalSource;
            }
        }

        private void Awake()
        {
            // Get the selectable.
            m_Selectable = GetComponent<Selectable>();

            // Set up the play audio on this if required.
            OnPlayAudioOnThis();
        }

        private void OnPlayAudioOnThis()
        {
            // If play on this is enabled, get the audio source, or add it.
            if (m_PlayAudioOnThis)
            {
                if (m_ThisAudioSource == null)
                {
                    m_ThisAudioSource = GetComponent<AudioSource>();

                    if (m_ThisAudioSource == null)
                        m_ThisAudioSource = gameObject.AddComponent<AudioSource>();
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
                return;

            // If play on this is enabled, set all the values on this audio source.
            // Else set it on the global source.
            if (m_PlayAudioOnThis)
            {
                m_ThisAudioSource.volume = m_Volume;
                m_ThisAudioSource.pitch = m_Pitch;
                m_ThisAudioSource.outputAudioMixerGroup = m_MixerGroup;

                m_ThisAudioSource.clip = clip;
                m_ThisAudioSource.Play();
            }
            else
            {
                GlobalSource.volume = m_Volume;
                GlobalSource.pitch = m_Pitch;
                GlobalSource.outputAudioMixerGroup = m_MixerGroup;

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
                if (m_Selectable.interactable)
                {
                    PlaySound(m_ClickSound);
                }
                else
                {
                    PlaySound(m_ClickDisabledSound);
                }
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (m_Selectable.interactable || (!m_Selectable.interactable && m_PlayExitWhenDisabled))
            {
                PlaySound(m_ExitSound);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (m_Selectable.interactable || (!m_Selectable.interactable && m_PlayEnterWhenDisabled))
            {
                PlaySound(m_EnterSound);
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            // Make sure to do the play audio on this setup if changed in the editor at runtime.
            if (Application.isPlaying)
            {
                OnPlayAudioOnThis();
            }
        }
#endif
    }
}
