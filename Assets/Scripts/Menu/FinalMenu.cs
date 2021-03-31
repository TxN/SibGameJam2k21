using SMGCore;
using UnityEngine;
using UnityEngine.UI;

namespace Game {
	public sealed class FinalMenu : MonoSingleton<FinalMenu> {
		public GameObject WinObject = null;
		public GameObject LoseObject = null;

		public Button RestartButton = null;
		public Button QuitButton    = null;
		private void Start() {
			RestartButton.onClick.AddListener(Restart);
			QuitButton.onClick.AddListener(Quit);
			var isWin = ScenePersistence.Get<GamePersistence>().IsWin;

			WinObject.SetActive(isWin);
			LoseObject.SetActive(!isWin);
		}

		public void Quit() {
			GlobalController.Instance.Quit();
		}

		public void Restart() {
			GlobalController.Instance.StartGame(false);
		}
	}
}
