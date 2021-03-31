using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game {
	public sealed class MainMenu : MonoBehaviour {
		[Header("Buttons")]
		public Button PlayButton  = null;
		public Button QuitButton  = null;
		public Button AboutButton = null;

		void Start() {
			PlayButton.onClick. AddListener(OnPlayButton);
			QuitButton.onClick. AddListener(OnQuitButton);
			AboutButton.onClick.AddListener(OnAboutButton);

			if ( Application.platform == RuntimePlatform.WebGLPlayer ) {
				QuitButton.gameObject.SetActive(false);
			}
		}

		void OnPlayButton() {
			GlobalController.Instance.StartGame();
		}

		void OnQuitButton() {
			ExitGame(); //TODO: transition
		}

		void OnAboutButton() {

		}

		void ExitGame() {
			GlobalController.Instance.Quit();
		}
	}
}
