using System;
using System.Collections.Generic;
using System.Linq;
using Arianrhod.Entity;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Arianrhod.View.Ui
{
    public class AttackScreen : ScreenBase
    {
        [SerializeField] private Toggle[] _skillToggles = default;
        [SerializeField] private TextMeshProUGUI[] _skillTexts = default;

        [SerializeField] private SoundButton _submitButton = default;
        [SerializeField] private SoundButton _skipButton = default;

        [SerializeField] private SoundButton[] _directionButton = default;
        
        public IObservable<int> OnSkillSelected()
        {
            var list = new List<IObservable<int>>();
            for (var i = 0; i < _skillToggles.Length; i++)
            {
                var index = i;
                list.Add(_skillToggles[i].OnValueChangedAsObservable()
                    .Where(value => value)
                    .Select(_ => index));
            }

            return list.Select(_ => _).Merge();
        }

        public IObservable<Direction> OnDirectionChanged()
        {
            var list = new List<IObservable<Direction>>();
            for (var i = 0; i < _directionButton.Length; i++)
            {
                var index = i;
                list.Add(_directionButton[i].OnClickObservable()
                    .Select(_ => (Direction)index));
            }

            return list.Select(_ => _).Merge();
        }

        public IObservable<Unit> OnSubmit() => _submitButton.OnClickObservable();
        public IObservable<Unit> OnSkip() => _submitButton.OnClickObservable();

        public void Initialize(List<SkillEntity> skills)
        {
            for (var i = 0; i < _skillToggles.Length; i++)
            {
                _skillTexts[i].text = skills[i].Name;
            }
        }
        
        public void IsEmitable(bool isEmitable)
        {
            _submitButton.interactable = isEmitable;
        }
    }
}