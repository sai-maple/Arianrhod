using UnityEngine;
using Zenject;

namespace Arianrhod.View.Ui
{
    public class AudioPlayer : MonoBehaviour
    {
        [SerializeField] private AudioSource _source = default;
        [SerializeField] private AudioClip[] _soundEffects = default;
        
        private SignalBus _signalBus = default;
        
        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void Start()
        {
            _signalBus.Subscribe<SoundSignal>(PlayOneShot);
        }

        private void PlayOneShot(SoundSignal soundSignal)
        {
            _source.PlayOneShot(_soundEffects[(int)soundSignal.soundEffect]);
        }
    }
}