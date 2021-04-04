using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using SMGCore.Windows;
using KOZA.Windows;
using SMGCore;

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
			SoundManager.Instance.PlayMusic("music_menu");
		}

		void OnPlayButton() {
			GlobalController.Instance.StartGame();
		}

		void OnQuitButton() {
			ExitGame(); //TODO: transition
		}

		void OnAboutButton() {
			WindowManager.Instance.ShowWindow<PauseWindow>(w => w.Show());
		}

		void ExitGame() {
			GlobalController.Instance.Quit();
		}
	}
}
