using System;

using UnityEngine;

using SMGCore;

using DG.Tweening;

namespace Game.UI {
	public sealed class LoadingTransition : MonoBehaviour {
		Sequence _seq = null;

		public RectTransform LeftPart = null;
		public RectTransform RightPart = null;

		Vector2 _leftStartPos = Vector2.zero;
		Vector2 _rightStartPos = Vector2.zero;

		void Awake() {
			
		}

		public void ShowTransition(Action onShown) {
			_seq = TweenHelper.ReplaceSequence(_seq);
			LeftPart.gameObject.SetActive(true);
			RightPart.gameObject.SetActive(true);

			_seq.AppendInterval(0.2f);
			_seq.Append(LeftPart.DOAnchorPos(_leftStartPos, 0.5f));
			_seq.Join(RightPart.DOAnchorPos(_rightStartPos, 0.5f));
			_seq.AppendCallback(()=> {
				onShown?.Invoke();
			});
		}

		public void HideTransition(Action onHidden) {
			LeftPart.gameObject.SetActive(true);
			RightPart.gameObject.SetActive(true);

			_seq = TweenHelper.ReplaceSequence(_seq);
			_seq.AppendInterval(0.2f);
			
			_seq.Append(LeftPart.DOAnchorPos(_leftStartPos - new Vector2(LeftPart.rect.width * 0.85f, 0), 0.7f));
			_seq.Join(RightPart.DOAnchorPos(_rightStartPos + new Vector2(RightPart.rect.width * 0.6f, 0), 0.7f));
			_seq.AppendCallback(() => {
				LeftPart.gameObject.SetActive(false);
				RightPart.gameObject.SetActive(false);
				onHidden?.Invoke();
			});
		}

		void OnDestroy() {
			_seq = TweenHelper.ResetSequence(_seq);
		}
	}
}
