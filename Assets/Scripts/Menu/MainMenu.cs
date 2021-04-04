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
		public Button SoundButton = null;

		public Sprite SoundOnSpr = null;
		public Sprite SoundOffSpr = null;

		void Start() {
			PlayButton.onClick. AddListener(OnPlayButton);
			QuitButton.onClick. AddListener(OnQuitButton);
			AboutButton.onClick.AddListener(OnAboutButton);
			SoundButton.onClick.AddListener(OnSoundButton);

			if ( Application.platform == RuntimePlatform.WebGLPlayer ) {
				QuitButton.gameObject.SetActive(false);
			}
			SoundManager.Instance.PlayMusic("music_menu");
			SetupSoundButton();
		}

		void OnPlayButton() {
			SoundManager.Instance.PlaySound("menuClick");
			GlobalController.Instance.StartGame();
		}

		void OnQuitButton() {
			SoundManager.Instance.PlaySound("menuClick");
			ExitGame(); //TODO: transition
		}

		void OnAboutButton() {
			SoundManager.Instance.PlaySound("menuClick");
		}

		void ExitGame() {
			GlobalController.Instance.Quit();
		}

		void SetupSoundButton() {
			var status = GetSoundStatus();
			SoundButton.image.sprite = status ? SoundOnSpr : SoundOffSpr;
		}

		bool GetSoundStatus() {
			return SoundManager.Instance.SoundEnabled;
		}

		void OnSoundButton() {
			var status = GetSoundStatus();
			SetSoundStatus(!status);
			SetupSoundButton();
		}

		void SetSoundStatus(bool on) {
			SoundManager.Instance.SoundEnabled = on;
			SoundManager.Instance.MusicEnabled = on;
		}
	}
}
