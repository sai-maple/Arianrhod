using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Arianrhod.View.Game
{
	public class ActionEffectManager : MonoBehaviour
	{
		public bool isFar = true;
		public Vector3 pos;
		public List<ActionEffect> UltimateEffect = new List<ActionEffect>();
		public List<ActionEffect> MagicEffect = new List<ActionEffect>();
		public List<ActionEffect> AttackEffect = new List<ActionEffect>();
		public List<ActionEffect> Magic2Effect = new List<ActionEffect>();
	
		private readonly Subject<Unit> _actionStartSubject = new Subject<Unit>();
		private readonly Subject<Unit> _actionEndSubject = new Subject<Unit>();

		public IObservable<Unit> OnActionStart() => _actionStartSubject.Publish().RefCount();
		public IObservable<Unit> OnActionEnd() => _actionEndSubject.Publish().RefCount();

		public List<ActionEffect> getEffectByName(string str)
		{
			switch (str)
			{
				case AnimationName1.Ultimate:
					return UltimateEffect;
					break;
				case AnimationName1.Magic:
					return MagicEffect;
					break;
				case AnimationName1.Attack:
					return AttackEffect;
					break;
				case AnimationName1.Magic2:
					return Magic2Effect;
					break;
			}

			return null;
		}

		public void stopUltimateEffect()
		{
			for (int i = 0; i < UltimateEffect.Count; i++)
			{
				UltimateEffect[i].stop();
			}
		}

		private void ActionDone(string actionName)
		{
			_actionStartSubject.OnNext(Unit.Default);
		}

		private void ActionStart(string actionName)
		{
			List<ActionEffect> list = getEffectByName(actionName);
			if (list == null)
			{
				return;
			}

			for (int i = 0; i < list.Count; i++)
			{
				list[i].play();
			}

			_actionEndSubject.OnNext(Unit.Default);
		}

		private void OnDestroy()
		{
			_actionStartSubject.OnCompleted();
			_actionEndSubject.OnCompleted();
			_actionStartSubject.Dispose();
			_actionEndSubject.Dispose();
		}
	}
}
