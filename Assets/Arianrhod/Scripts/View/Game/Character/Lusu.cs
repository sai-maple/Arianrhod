using UniRx.Async;
using UnityEngine;

namespace Arianrhod.View.Game.Character
{
    public class Lusu : MonoBehaviour
    {
        [SerializeField] private Transform _target = default;

        [SerializeField] private PosBullet _attackBullet = default;
        [SerializeField] private PosBullet _magicBullet = default;
        [SerializeField] private CurvelBullet _magic2Bullet = default;
        [SerializeField] private PosBullet _ultimateBullet = default;
        [SerializeField] private ParticlesEffect _damageEffect1 = default;
        [SerializeField] private ParticlesEffect _damageEffect2 = default;
        [SerializeField] private ParticlesEffect _damageEffect3 = default;


        private async void DelayBullet()
        {
            await UniTask.Delay(2000);
        }

        private async void DelayBullet1()
        {
            const int count = 5;
            const float angle = -0 / 4f * 10f;
            for (var i = 0; i < count; i++)
            {
                var bullet = Instantiate(_attackBullet);
                bullet.player = transform;
                bullet.startPos = transform.position + new Vector3(0f, 0.01f, 0f);
                bullet.tarPos = MathUtil1.calcTargetPosByRotation(transform, angle + i * 10f, 10f);
                bullet.effectObj = _damageEffect1;
                bullet.bulleting();
                await UniTask.Delay(1);
                if (i % 6 != 0) continue;
                if (_damageEffect2 == null) continue;
                var effect = Instantiate(_damageEffect2);
                effect.transform.position = MathUtil1.findChild(_target, "attackedPivot").position;
                effect.play();
            }
        }

        private async void DelayBullet2()
        {
            const int count = 10;
            const float angle = -5 / 2f * 10f;
            for (var i = 0; i < count; i++)
            {
                var bullet = Instantiate(_magicBullet);
                bullet.player = transform;
                bullet.startPos = transform.position + new Vector3(0f, 0.01f, 0f);
                bullet.tarPos = MathUtil1.calcTargetPosByRotation(transform, angle + i * 10f, 10f);
                bullet.effectObj = _damageEffect1;
                bullet.bulleting();
                await UniTask.Delay(1);
                if (i % 6 != 0) continue;
                if (_damageEffect2 == null) continue;
                var effect = Instantiate(_damageEffect2);
                effect.transform.position = MathUtil1.findChild(_target, "attackedPivot").position;
                effect.play();
            }
        }

        private async void DelayBullet3()
        {
            const int count = 20;
            const float angle = -10 / 2f * 10f;
            for (var i = 0; i < count; i++)
            {
                var bullet = Instantiate(_ultimateBullet);
                bullet.player = transform;
                bullet.startPos = transform.position + new Vector3(0f, 0.01f, 0f);
                bullet.tarPos = MathUtil1.calcTargetPosByRotation(transform, angle + i * 10f, 10f);
                bullet.effectObj = _damageEffect1;
                bullet.bulleting();
                await UniTask.Delay(1);
                if (i % 6 != 0) continue;
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
                    DelayBullet1();
                    break;
                case AnimationName.Magic:
                    DelayBullet2();
                    break;
                case AnimationName.Magic2:
                    break;
                case AnimationName.Ultimate:
                    if (_damageEffect3 != null)
                    {
                        var effect = Instantiate(_damageEffect3);
                        effect.transform.position = MathUtil1.findChild(_target, "attackedPivot").position;
                        effect.play();
                    }
                    DelayBullet3();
                    DelayBullet();
                    break;
            }
        }
    }
}