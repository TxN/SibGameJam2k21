using SMGCore;
using UnityEngine;
using UnityEngine.UI;

namespace Game {
	public sealed class FinalMenu : MonoSingleton<FinalMenu> {
		public GameObject WinObject = null;

		public GameObject FailCommon;
		public GameObject FailFlood;
		public GameObject FailCall;
		public GameObject FailWrong;

		public Button RestartButton = null;
		public Button QuitButton    = null;
		private void Start() {
			RestartButton.onClick.AddListener(Restart);
			QuitButton.onClick.AddListener(Quit);
			var p = ScenePersistence.Get<GamePersistence>();
			var isWin = p.IsWin;

			if ( isWin ) {
				SoundManager.Instance.PlaySound("victory");
			}
			Debug.LogWarning($"Game result is: {p.Result}");
			WinObject.SetActive(isWin);
			switch ( p.Result ) {
				case GameResult.Win:
					break;
				case GameResult.WrongIdentity:
					FailWrong.SetActive(true);
					break;
				case GameResult.ExclusionNotPassed:
					FailWrong.SetActive(true);
					break;
				case GameResult.BannedTraitPassed:
					FailWrong.SetActive(true);
					break;
				case GameResult.WronglyYeeted:
					FailWrong.SetActive(true);
					break;
				case GameResult.BreachBroken:
					FailFlood.SetActive(true);
					break;
				case GameResult.TimeIsOff:
					FailCommon.SetActive(true);
					break;
				case GameResult.MissedCall:
					FailCall.SetActive(true);
					break;
				default:
					break;
			}
		}

		public void Quit() {
			GlobalController.Instance.Quit();
		}

		public void Restart() {
			GlobalController.Instance.StartGame(false);
		}
	}
}
