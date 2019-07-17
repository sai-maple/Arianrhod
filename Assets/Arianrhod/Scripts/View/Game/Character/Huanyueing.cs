using UniRx.Async;
using UnityEngine;

namespace Arianrhod.View.Game.Character
{
    public class Huanyueing : MonoBehaviour
    {
        [SerializeField] private Transform _target = default;

        [SerializeField] private NormalBullet _attackBullet = default;
        [SerializeField] private NormalBullet _magicBullet = default;
        [SerializeField] private CurvelBullet _magic2Bullet = default;
        [SerializeField] private CurvelBullet _ultimateBullet = default;
        [SerializeField] private ParticlesEffect _damageEffect1 = default;
        [SerializeField] private ParticlesEffect _damageEffect2 = default;
        [SerializeField] private ParticlesEffect _damageEffect3 = default;

        private async void DelayBullet()
        {
            const int count = 10;
            for (var i = 0; i < count; i++)
            {
                var bullet = Instantiate(_ultimateBullet);
                bullet.player = transform;
                bullet.target = _target;

                bullet.bulleting();
                await UniTask.Delay(1);
                if (i % 9 != 0) continue;
                bullet.effectObj = _damageEffect1;
                if (_damageEffect2 == null) continue;
                var effect = Instantiate(_damageEffect2);
                effect.transform.position = MathUtil.findChild(_target, "attackedPivot").position;
                effect.play();
            }
        }

        private async void DelayBullet1()
        {
            const int count = 10;
            for (var i = 0; i < count; i++)
            {
                var bullet = Instantiate(_magic2Bullet);
                bullet.player = transform;
                bullet.target = _target;

                bullet.bulleting();
                await UniTask.Delay(1);
                if (i % 9 != 0) continue;
                bullet.effectObj = _damageEffect1;
                if (_damageEffect2 == null) continue;
                var effect = Instantiate(_damageEffect2);
                effect.transform.position = MathUtil1.findChild(_target, "attackedPivot").position;
                effect.play();
            }
        }

        private void preAction(string actionName)
        {
            var arr = actionName.Split('|');
            var name = arr[0];
            switch (name)
            {
                case AnimationName.Attack:
                    if (_attackBullet != null)
                    {
                        var bullet = Instantiate(_attackBullet);
                        bullet.player = transform;
                        bullet.target = _target;
                        bullet.effectObj = _damageEffect1;
                        bullet.bulleting();
                    }
                    break;
                case AnimationName.Magic:
                    if (_attackBullet != null)
                    {
                        var bullet = Instantiate(_attackBullet);
                        bullet.player = transform;
                        bullet.target = _target;
                        bullet.effectObj = _damageEffect1;
                        bullet.bulleting();
                    }
                    break;
                case AnimationName.Magic2:
                    if (_attackBullet != null)
                    {
                        var bullet = Instantiate(_attackBullet);
                        bullet.player = transform;
                        bullet.target = _target;
                        bullet.effectObj = _damageEffect2;
                        bullet.bulleting();
                    }
                    break;
                case AnimationName.Ultimate:
                    if (_ultimateBullet != null)
                    {
                        DelayBullet();
                        DelayBullet1();
                    }

                    if (_damageEffect3 != null)
                    {
                        var effect = Instantiate(_damageEffect3);
                        effect.transform.position = MathUtil1.findChild(_target, "attackedPivot").position;
                        effect.play();
                    }
                    break;
            }
        }
    }
}