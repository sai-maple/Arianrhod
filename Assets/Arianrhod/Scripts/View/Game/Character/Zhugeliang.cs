using UniRx.Async;
using UnityEngine;

namespace Arianrhod.View.Game.Character
{
    public class Zhugeliang : MonoBehaviour
    {
        [SerializeField] private Transform _target = default;

        [SerializeField] private NormalBullet _attackBullet = default;
        [SerializeField] private NormalBullet _magicBullet = default;
        [SerializeField] private NormalBullet _magic2Bullet = default;
        [SerializeField] private xuanzhuanBullet _ultimateBullet = default;
        [SerializeField] private GameObject _damageEffect1 = default;
        [SerializeField] private GameObject _damageEffect2 = default;
        [SerializeField] private GameObject _damageEffect3 = default;

        private async void DelayBullet()
        {
            if (_ultimateBullet == null)
            {
                return;
            }

            const int count = 10;
            for (var i = 0; i < count; i++)
            {
                var bullet = Instantiate(_ultimateBullet);
                bullet.player = transform;
                bullet.effectObj = _damageEffect1;
                bullet.target = _target;
                bullet.bulleting();
                await UniTask.Delay(1);
            }
        }

        private async void DelayBullet1()
        {
            if (_ultimateBullet == null)
            {
                return;
            }

            const int count = 10;
            for (var i = 0; i < count; i++)
            {
                var bullet = Instantiate(_ultimateBullet);
                bullet.player = transform;
                bullet.effectObj = _damageEffect1;
                bullet.target = _target;
                bullet.flag = -1f;
                bullet.bulleting();
                await UniTask.Delay(1);

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
                        if (bullet == null)
                        {
                            return;
                        }

                        bullet.player = transform;
                        bullet.target = _target;
                        bullet.effectObj = _damageEffect1;
                        bullet.bulleting();
                    }

                    break;
                case AnimationName.Magic:
                    if (_magicBullet != null)
                    {
                        var bullet = Instantiate(_magicBullet);
                        bullet.player = transform;
                        bullet.target = _target;
                        bullet.effectObj = _damageEffect2;
                        bullet.bulleting();


                    }

                    DelayBullet();
                    DelayBullet1();
                    break;
                case AnimationName.Magic2:
                    if (_magic2Bullet != null)
                    {
                        var bullet = Instantiate(_magic2Bullet);
                        bullet.player = transform;
                        bullet.target = _target;
                        bullet.effectObj = _damageEffect2;
                        bullet.bulleting();
                    }

                    break;
                case AnimationName.Ultimate:
                    if (_damageEffect3 != null)
                    {
                        var obj1 = Instantiate(_damageEffect3);
                        var effect = obj1.AddComponent<ParticlesEffect>();
                        effect.transform.position = MathUtil.findChild(_target, "attackedPivot").position;
                        effect.play();
                    }
                    break;
            }
        }
    }
}