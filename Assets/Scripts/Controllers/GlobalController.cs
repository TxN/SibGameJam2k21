using UnityEngine;

using SMGCore;
using Game.UI;

namespace Game {
	public sealed class GlobalController : MonoSingleton<GlobalController> {
		OverlayUI _overlayUI = null;
		LoadingController _loadingController = null;

		public bool IsControlsLocked {
			get {
				return _overlayUI.TapBlocker.activeInHierarchy;
			}
		}

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

			ScenePersistence.Instance.SetupHolder(new GamePersistence());
		}

		private void OnDestroy() {
			_loadingController?.DeInit();
		}

		public void LoadScene(string sceneName) {
			_loadingController?.LoadScene(sceneName);
		}

		public void StartGame(bool clearPersistence = true) {
			if ( clearPersistence ) {
				ScenePersistence.Instance.SetupHolder(new GamePersistence());
			}
			LoadScene("Gameplay");
		}

		public void OpenFinalScene() {
			LoadScene("FinalScene");
		}

		public void OpenMainMenu() {
			LoadScene("MainMenu");
		}

		public void Quit() {
			SetTapBlock(true);
			_loadingController.DryRunTransition(true, () => {
				Application.Quit();
#if UNITY_EDITOR
				UnityEditor.EditorApplication.isPlaying = false;
#endif
			});
		}

		public void SetTapBlock(bool enabled) {
			_overlayUI.SetTapBlock(enabled);
		}
	}
}
