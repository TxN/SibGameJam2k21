using UnityEngine;

using SMGCore.EventSys;
using Game.Events;

using DG.Tweening;

namespace Game {
	public sealed class FloodOverlay : MonoBehaviour {
		public RectTransform FloodParent = null;
		private void Start() {

			EventManager.Subscribe<Breach_Broken>(this,OnBreachBroken);
		}

		private void OnDestroy() {
			EventManager.Unsubscribe<Breach_Broken>(OnBreachBroken);
		}

		void OnBreachBroken(Breach_Broken e) {
			var seq = DOTween.Sequence();
			seq.AppendInterval(1.5f);
			seq.AppendCallback(() => {
				FloodParent.gameObject.SetActive(true);
			});
			seq.Append(FloodParent.DOAnchorPos(new Vector2(0, -30), 2f));
		}
	}
}
