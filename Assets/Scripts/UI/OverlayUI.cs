using UnityEngine;

namespace Game.UI {
	public sealed class OverlayUI : MonoBehaviour {
		public GameObject TapBlocker = null;

		public void SetTapBlock(bool flag) {
			TapBlocker.SetActive(flag);
		}
	}
}

