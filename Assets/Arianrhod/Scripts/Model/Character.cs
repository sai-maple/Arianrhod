using System;
using System.Collections.Generic;
using Arianrhod.Entity;
using UniRx;
using Unity.Mathematics;
using UnityEngine;

namespace Arianrhod.Model
{
    public class Character : IDisposable
    {
        public void Initialize(int id, CharacterEntity entity, List<SkillEntity> skillEntity,Owner owner)
        {
            _hp = new ReactiveProperty<int>(entity.Hp);
            MaxHp = entity.Hp;
            _d3 = new ReactiveProperty<int>(0);
            _d6 = new ReactiveProperty<int>(0);
            _d8 = new ReactiveProperty<int>(0);
            _direction = new ReactiveProperty<Direction>();
            _damageSubject = new Subject<int>();
            _position = new ReactiveProperty<Vector3Int>();
            Id = id;
            CharacterEntity = entity;
            _skillEntity = skillEntity;
            Owner = owner;
        }

        private  List<SkillEntity> _skillEntity = default;
        private  ReactiveProperty<int> _hp = default;
        private  ReactiveProperty<int> _d3 = default;
        private  ReactiveProperty<int> _d6 = default;
        private  ReactiveProperty<int> _d8 = default;
        private  ReactiveProperty<Direction> _direction = default;
        private  Subject<int> _damageSubject = default;
        private  ReactiveProperty<Vector3Int> _position = default;

        public int Id { get; private set; }
        public int MaxHp { get; private set; }
        public IReadOnlyReactiveProperty<int> OnHpChanged() => _hp;
        public IReadOnlyReactiveProperty<int> OnD3Changed() => _d3;
        public IReadOnlyReactiveProperty<int> OnD6Changed() => _d6;
        public IReadOnlyReactiveProperty<int> OnD8Changed() => _d8;
        public IReadOnlyReactiveProperty<Direction> OnDirectionChanged() => _direction;
        
        public CharacterEntity CharacterEntity { get; private set; } = default;
        public Owner Owner { get; private set;} = default;

        public SkillEntity Skill(int index) => _skillEntity[index];
        public IEnumerable<SkillEntity> SkillEntities() => _skillEntity;
        public IReadOnlyReactiveProperty<Vector3Int> Position() => _position;


        public void SetDirection(Direction direction)
        {
            _direction.Value = direction;
        }
        
        public void AddDice(DiceType diceType)
        {
            switch (diceType)
            {
                case DiceType.D3:
                    _d3.Value++;
                    break;
                case DiceType.D6:
                    _d6.Value++;
                    break;
                case DiceType.D8:
                    _d8.Value++;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void SetPosition(int x,int y )
        {
            _position.Value = new Vector3Int(x, 1, y);
        }

        public int DiceNumCount(SkillEntity skillEntity)
        {
            switch (skillEntity.DiceType)
            {
                case DiceType.D3:
                    return _d3.Value;
                case DiceType.D6:
                    return _d6.Value;
                case DiceType.D8:
                    return _d8.Value;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void EmitDamage(int damage)
        {
            _hp.Value = math.max(0, _hp.Value - damage);
            _damageSubject.OnNext(damage);
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