using UniRx.Async;
using UnityEngine;

namespace Arianrhod.View.Game.Character
{
    public class Simayi : MonoBehaviour
    {
        [SerializeField] private Transform _target = default;

        [SerializeField] private NormalBullet _attackBullet = default;
        [SerializeField] private NormalBullet _magicBullet = default;
        [SerializeField] private NormalBullet _magic2Bullet = default;
        [SerializeField] private PosBullet _ultimateBullet = default;
        [SerializeField] private ParticlesEffect _damageEffect1 = default;
        [SerializeField] private ParticlesEffect _damageEffect2 = default;
        [SerializeField] private ParticlesEffect _damageEffect3 = default;
        [SerializeField] private ParticlesEffect _damageEffect4 = default;

        private const float padding = 3f;

        private async void DelayBullet1()
        {
            const int count = 2;
            for (var i = 0; i < count; i++)
            {
                var obj = Instantiate(_damageEffect4);

                obj.transform.position = _target.position + new Vector3(Random.Range(-padding, padding), 0.12f,
                                             Random.Range(-padding, padding));
                await UniTask.Delay(20);
            }
        }

        private async void DelayBullet2()
        {
            const int count = 4;
            for (var i = 0; i < count; i++)
            {
                var obj = Instantiate(_damageEffect4);
                obj.transform.position = _target.position + new Vector3(Random.Range(-padding, padding), 0.12f,
                                             Random.Range(-padding, padding));
                await UniTask.Delay(15);
            }
        }

        private async void DelayBullet3()
        {
            const int count = 12;
            for (var i = 0; i < count; i++)
            {
                var obj = Instantiate(_damageEffect4);
                obj.transform.position = _target.position + new Vector3(Random.Range(-padding, padding), 0.12f,
                                             Random.Range(-padding, padding));
                await UniTask.Delay(10);
            }
        }

        private async void DelayBullet()
        {
            const int count = 1;
            for (var i = 0; i < count; i++)
            {
                var obj = Instantiate(_damageEffect4);
                obj.transform.position = _target.position + new Vector3(Random.Range(-padding, padding), 0.12f,
                                             Random.Range(-padding, padding));
                await UniTask.Delay(10);
            }
        }

        private void preAction(string actionName)
        {
            string[] arr = actionName.Split('|');
            string name = arr[0];
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
                    DelayBullet2();
                    break;
                case AnimationName.Ultimate:
                    DelayBullet3();
                    break;
            }
        }


    }
}