using UnityEngine;

using SMGCore;
using Game.UI;
using UnityEngine.SceneManagement;

namespace Game {
	public sealed class GlobalController : MonoSingleton<GlobalController> {
		LoadingTransition LoadingAnimation = null;
		OverlayUI OverlayUI = null;

		string _curLoadingScene = null;
		protected override void Awake() {
			base.Awake();
			DontDestroyOnLoad(gameObject);
			var overlayUIFab = Resources.Load<GameObject>("Prefabs/OverlayUI");
			var overlayUIGo = Instantiate(overlayUIFab, null, false);
			DontDestroyOnLoad(overlayUIGo);
			OverlayUI = overlayUIGo.GetComponent<OverlayUI>();

			LoadingAnimation = OverlayUI.GetComponentInChildren<LoadingTransition>();

			LoadingAnimation.HideTransition(null);
			SceneManager.sceneLoaded += OnSceneLoaded;
		}

		private void OnDestroy() {
			SceneManager.sceneLoaded -= OnSceneLoaded;
		}

		public void LoadScene(string sceneName) {
			SetTapBlock(true);
			_curLoadingScene = sceneName;
			LoadingAnimation.ShowTransition(() => {
				OpenScene(sceneName);
			});
		}

		void OpenScene(string sceneName) {
			SceneManager.LoadScene(sceneName);
			SetTapBlock(false);
		}

		void OnSceneLoaded(Scene scene, LoadSceneMode loadMode) {
			if ( scene.name == _curLoadingScene ) {
				LoadingAnimation?.HideTransition(null);
				_curLoadingScene = null;
			}
		}

		public void StartGame() {
			LoadScene("Gameplay");
		}

		public void Quit() {

		}

		public void SetTapBlock(bool enabled) {
			OverlayUI.SetTapBlock(enabled);
		}
	}
}
