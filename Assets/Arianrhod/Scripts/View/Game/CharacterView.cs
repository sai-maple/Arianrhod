using System;
using System.Collections.Generic;
using System.Linq;
using Arianrhod.Entity;
using DG.Tweening;
using UniRx;
using Unity.Mathematics;
using UnityEngine;
using Zenject;

namespace Arianrhod.View.Game
{
    public interface ICharacterView
    {
        IObservable<Unit> OnAnimationStarted();
        IObservable<Unit> OnAnimationEnded();
        void OnAnimation(AnimationState state);
        void OnDead();
        void DoMove(IEnumerable<Vector3> position);
        CharacterEntity GetEntity();
        void SetRotation(Direction direction);
    }
    
    public class CharacterView : MonoBehaviour, ICharacterView
    {
        [SerializeField] private Animator _animator = default;
        [SerializeField] private ActionEffectManager _actionEffectManager = default;

        private CharacterEntity _entity = default;
        
        private static readonly int[] Hash =
        {
            Animator.StringToHash("Stand"),
            Animator.StringToHash("Death")
        };

        public IObservable<Unit> OnAnimationStarted() => _actionEffectManager.OnActionStart();
        public IObservable<Unit> OnAnimationEnded() => _actionEffectManager.OnActionEnd();

        public async void DoMove(IEnumerable<Vector3> position)
        {
            await transform.DOLocalPath(position.ToArray(), 2f, PathType.Linear)
                .SetLookAt(1f, Vector3.forward)
                .SetEase(Ease.Linear);
        }

        public CharacterEntity GetEntity()
        {
            return _entity;
        }

        public void SetRotation(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    transform.localRotation = quaternion.RotateY(0);
                    break;
                case Direction.Down:
                    transform.localRotation = quaternion.RotateY(180);
                    break;
                case Direction.Right:
                    transform.localRotation = quaternion.RotateY(90);
                    break;
                case Direction.Left:
                    transform.localRotation = quaternion.RotateY(270);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
        }

        public void OnAnimation(AnimationState state)
        {
            _animator.SetTrigger(Hash[(int) state]);
        }

        public void OnDead()
        {
            _animator.SetTrigger(Hash[(int) AnimationState.Death]);
        }
    }
}