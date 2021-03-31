using System;

using UnityEngine;

using DG.Tweening;

namespace SMGCore {
	public abstract class LoadingTransition : MonoBehaviour {
		protected Sequence _seq = null;

		public abstract void ShowTransition(bool force, Action onComplete);
		public abstract void HideTransition(bool force, Action onComplete);


		protected virtual void OnDestroy() {
			_seq = TweenHelper.ResetSequence(_seq);
		}
	}
}
