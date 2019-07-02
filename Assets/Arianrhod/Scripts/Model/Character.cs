using System;
using System.Collections.Generic;
using Arianrhod.Entity;
using UniRx;
using UnityEngine;

namespace Arianrhod.Model
{
    public class Character : IDisposable
    {
        public Character()
        {
            _hp = new ReactiveProperty<int>();
            _d3 = new ReactiveProperty<int>(0);
            _d6 = new ReactiveProperty<int>(0);
            _d8 = new ReactiveProperty<int>(0);
            _direction = new ReactiveProperty<Direction>();
        }

        private readonly List<SkillEntity> _skillEntity = default;
        private readonly ReactiveProperty<int> _hp = default;
        private readonly ReactiveProperty<int> _d3 = default;
        private readonly ReactiveProperty<int> _d6 = default;
        private readonly ReactiveProperty<int> _d8 = default;
        private readonly ReactiveProperty<Direction> _direction = default;
        private PanelEntity _position = default;
        private Transform _transform = default;

        public int Id { get; }
        public int MaxHp { get; }
        public IReadOnlyReactiveProperty<int> OnHpChanged() => _hp;
        public IReadOnlyReactiveProperty<int> OnD3Changed() => _d3;
        public IReadOnlyReactiveProperty<int> OnD6Changed() => _d6;
        public IReadOnlyReactiveProperty<int> OnD8Changed() => _d8;
        public IReadOnlyReactiveProperty<Direction> OnDirectionChanged() => _direction;
        
        public CharacterEntity CharacterEntity { get; } = default;
        public Owner Owner { get; } = default;

        public SkillEntity Skill(int index) => _skillEntity[index];
        public (int, int) Position => (_position.X, _position.Y);


        public void SetDirection(Direction direction)
        {
            _direction.Value = direction;
        }
        
        public void AddDice(DiceType diceType)
        {
            
        }

        public void SetPosition(PanelEntity panelEntity)
        {
            _position = panelEntity;
        }

        public void Dispose()
        {
            _hp.Dispose();
            _d3.Dispose();
            _d6.Dispose();
            _d8.Dispose();
            _direction.Dispose();
        }
    }
}