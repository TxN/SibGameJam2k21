using System;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace SMGCore {
	public sealed class LoadingController {
		const string TransitionsPath = "Prefabs/LoadingTransitions/{0}";

		LoadingTransition _currentTransition = null;
		string            _curLoadingScene   = null;
		Canvas            _loadingCanvas     = null;
		Action _onBeforeLoad = null;
		Action _onAfterLoad  = null;

		public void Setup(Canvas loadingCanvas, string transitionName, Action onBeforeLoad, Action onAfterLoad) {
			SceneManager.sceneLoaded += OnSceneLoaded;
			_onAfterLoad   = onAfterLoad;
			_onBeforeLoad  = onBeforeLoad;
			_loadingCanvas = loadingCanvas;
			SetupTransition(transitionName);
		}

		public void DeInit() {
			SceneManager.sceneLoaded -= OnSceneLoaded;
		}

		public void SetupTransition(string name) {
			var fab = Resources.Load<GameObject>(string.Format(TransitionsPath, name));
			if ( !fab ) {
				Debug.LogErrorFormat("LoadingController.SetupTransition: There is no transition prefab with name {0}", name);
			}
			var inst = UnityEngine.Object.Instantiate(fab, _loadingCanvas.transform, false);
			var tr = inst.GetComponent<LoadingTransition>();
			if ( !tr ) {
				Debug.LogErrorFormat("LoadingController.SetupTransition: There is no transition component on transition prefab with name {0}", name);
				UnityEngine.Object.Destroy(inst);
				return;
			}

			if ( _currentTransition ) {
				UnityEngine.Object.Destroy(_currentTransition);
			}

			_currentTransition = tr;
			_currentTransition.HideTransition(true, null);
		}

		public void LoadScene(string sceneName) {
			_onBeforeLoad?.Invoke();
			_curLoadingScene = sceneName;
			_currentTransition.ShowTransition(false, () => {
				OpenScene(sceneName);
			});
		}

		void OnSceneLoaded(Scene scene, LoadSceneMode loadMode) {
			if ( scene.name == _curLoadingScene ) {
				_currentTransition?.HideTransition(false, null);
				_curLoadingScene = null;
			}
		}

		void OpenScene(string sceneName) {
			SceneManager.LoadScene(sceneName);
			_onAfterLoad?.Invoke();
		}

	}
}
