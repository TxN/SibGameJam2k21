using UnityEngine;

using SMGCore;
using Game.UI;

namespace Game {
	public sealed class GlobalController : MonoSingleton<GlobalController> {
		OverlayUI _overlayUI = null;
		LoadingController _loadingController = null;

		protected override void Awake() {
			base.Awake();
			DontDestroyOnLoad(gameObject);
			var overlayUIFab = Resources.Load<GameObject>("Prefabs/OverlayUI");
			var overlayUIGo = Instantiate(overlayUIFab, null, false);
			DontDestroyOnLoad(overlayUIGo);
			_overlayUI = overlayUIGo.GetComponent<OverlayUI>();
			_loadingController = new LoadingController();
			_loadingController.Setup(_overlayUI.LoadingCanvas, "LateralSlideTransition", () => {
				SetTapBlock(true);
			}, () => {
				SetTapBlock(false);
			});
			
		}

		private void OnDestroy() {
			_loadingController?.DeInit();
		}

		public void LoadScene(string sceneName) {
			_loadingController?.LoadScene(sceneName);
		}

		public void StartGame() {
			LoadScene("Gameplay");
		}

		public void Quit() {

		}

		public void SetTapBlock(bool enabled) {
			_overlayUI.SetTapBlock(enabled);
		}
	}
}
