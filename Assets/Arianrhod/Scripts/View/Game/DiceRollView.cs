using System;
using DG.Tweening;
using UniRx.Async;
using Unity.Mathematics;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Arianrhod.View.Game
{
    // 振るダイス
    public class DiceRollView : MonoBehaviour, IPoolable<int, IMemoryPool>, IDisposable
    {
        private IMemoryPool _pool = default;
        [SerializeField] private DiceStats _diceStats = default;
        [SerializeField] private Vector3[] _endRotates = default;

        private readonly Vector3[] _path =
        {
            new Vector3(0f, 1f, 0f),
            new Vector3(2f, 0f, 0f),
            new Vector3(2.5f, 0.5f, 0f),
            new Vector3(3f, 0f, 0f),
        };

        public void OnDespawned()
        {
            if (_pool == null) return;
            Dispose();
        }

        public async UniTask<int> OnRoll()
        {
            transform.DOLocalRotate(_endRotates[Random.Range(0, _endRotates.Length)], 1);
            await transform.DOJump(new Vector3(3f, 0f, 0f), 2f, 2, 1)
                .SetEase(Ease.Linear).SetRelative();

            return _diceStats.side;
        }

        public void OnSpawned(int index, IMemoryPool pool)
        {
            _pool = pool;
            transform.localPosition = new Vector3(index % 10, 1, math.floor(index / 10f));
        }

        public void Dispose()
        {
            _pool.Despawn(this);
        }

        public class Factory : PlaceholderFactory<int, DiceRollView>
        {
        }
    }
}