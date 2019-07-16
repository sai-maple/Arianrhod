using UniRx.Async;
using UnityEngine;

namespace Arianrhod.View.Game.Character
{
    public class Huanggai : MonoBehaviour
    {
        [SerializeField] private Transform _target = default;

        [SerializeField] private CurvelBullet _attackBullet = default;
        [SerializeField] private NormalBullet _magicBullet = default;
        [SerializeField] private NormalBullet _magic2Bullet = default;
        [SerializeField] private LightBullet _ultimateBullet = default;
        [SerializeField] private ParticlesEffect _damageEffect1 = default;
        [SerializeField] private ParticlesEffect _damageEffect2 = default;
        [SerializeField] private ParticlesEffect _damageEffect3 = default;
        [SerializeField] private ParticlesEffect _damageEffect4 = default;

        private async void DelayBullet()
        {
            const int count = 2;
            for (var i = 0; i < count; i++)
            {
                var bullet = Instantiate(_attackBullet);
                bullet.player = transform;
                bullet.target = _target;

                bullet.bulleting();
                await UniTask.Delay(100);
                if (i % 9 != 0) continue;
                bullet.effectObj = _damageEffect4;
                if (_damageEffect2 == null) continue;
                var effect = Instantiate(_damageEffect2);
                effect.transform.position = MathUtil1.findChild(_target, "attackedPivot").position;
                effect.play();
            }
        }


        private async void DelayBullet2()
        {
            const int count = 4;
            for (var i = 0; i < count; i++)
            {
                var bullet = Instantiate(_attackBullet);
                bullet.player = transform;
                bullet.target = _target;

                bullet.bulleting();
                await UniTask.Delay(100);
                if (i % 9 != 0) continue;
                bullet.effectObj = _damageEffect4;
                if (_damageEffect2 == null) continue;
                var effect = Instantiate(_damageEffect2);
                effect.transform.position = MathUtil1.findChild(_target, "attackedPivot").position;
                effect.play();
            }
        }

        private async void DelayBullet3()
        {
            const int count = 8;
            for (var i = 0; i < count; i++)
            {
                var bullet = GameObject.Instantiate(_attackBullet);
                bullet.player = transform;
                bullet.target = _target;
                
                bullet.effectObj = _damageEffect4;
                bullet.bulleting();
                await UniTask.Delay(100);
                if (i % 9 != 0) continue;
                bullet.effectObj = _damageEffect4;
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
                    if (_damageEffect1 != null)
                    {
                        var effect = Instantiate(_damageEffect1);

                        effect.transform.position = _target.position;
                        effect.play();
                    }

                    DelayBullet();
                    break;
                case AnimationName.Magic:
                    if (_magicBullet != null)
                    {
                        var bullet = Instantiate(_magicBullet);
                        bullet.player = transform;
                        bullet.target = _target;
                        bullet.effectObj = _damageEffect1;
                        bullet.bulleting();
                    }

                    if (_damageEffect2 != null)
                    {
                        var effect = Instantiate(_damageEffect2);

                        effect.transform.position = _target.position;
                        effect.play();
                    }

                    DelayBullet2();
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

                    if (_damageEffect2 != null)
                    {
                        var effect = GameObject.Instantiate(_damageEffect2);

                        effect.transform.position = _target.position;
                        effect.play();
                    }

                    DelayBullet3();
                    break;
                case AnimationName.Ultimate:
                    if (_ultimateBullet != null)
                    {
                        var bullet = GameObject.Instantiate(_ultimateBullet);
                        bullet.player = transform;
                        bullet.target = _target;
                        bullet.effectObj = _damageEffect3;
                        bullet.bulleting();
                    }

                    if (_damageEffect3 != null)
                    {
                        var effect = Instantiate(_damageEffect3);

                        effect.transform.position = GameObject.Find("bigzhangjiao (1)").transform.position;
                        effect.play();
                    }

                    DelayBullet3();
                    break;
            }
        }
    }
}