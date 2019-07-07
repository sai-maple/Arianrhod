using System;
using Arianrhod.Model;
using TMPro;
using UniRx;
using UnityEngine;

namespace Arianrhod.View.Ui
{
    public class MoveScreen : ScreenBase
    {
        [SerializeField] private TextMeshProUGUI _characterName = default;
        [SerializeField] private TextMeshProUGUI _characterHp = default;
        [SerializeField] private TextMeshProUGUI _characterRange = default;
        [SerializeField] private TextMeshProUGUI _characterD3Num = default;
        [SerializeField] private TextMeshProUGUI _characterD6Num = default;
        [SerializeField] private TextMeshProUGUI _characterD8Num = default;

        [SerializeField] private SoundButton _submitButton = default;
        [SerializeField] private SoundButton _skipButton = default;

        public IObservable<Unit> OnMoveSubmit() => _submitButton.OnClickObservable();
        public IObservable<Unit> OnMoveSkip() => _skipButton.OnClickObservable();

        public void Initialize(Character character)
        {
            _characterName.text = character.CharacterEntity.Name;
            _characterRange.text = $"移動距離:{character.CharacterEntity.Range}";
            _submitButton.interactable = false;
        }

        public void CharacterHp(int value, int max)
        {
            _characterHp.text = $"{value}/{max}";
        }

        public void CharacterD3Num(int num)
        {
            _characterD3Num.text = $"3面ダイス:{num}";
        }
        
        public void CharacterD6Num(int num)
        {
            _characterD6Num.text = $"6面ダイス:{num}";
        }
        
        public void CharacterD8Num(int num)
        {
            _characterD8Num.text = $"8面ダイス:{num}";
        }

        public void IsMovable(bool isMovable)
        {
            _submitButton.interactable = isMovable;
        }
    }
}