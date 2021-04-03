using UnityEngine;

using SMGCore;
using SMGCore.EventSys;

using Game.Events;

namespace Game {
	public sealed class GameState : MonoSingleton<GameState> {
		public readonly TimeController TimeController = new TimeController();

		public bool IsDebug {
			get {
				return true; //Should be set to 'false' for release build.
			}
		}

		protected override void Awake() {
			base.Awake();
			if ( ScenePersistence.Instance.Data == null ) {
				ScenePersistence.Instance.SetupHolder(new GamePersistence());
			}
			EventManager.Subscribe<Game_Ended>(this, OnGameEnded);
		}

		void Update() {
			TimeController.Update(Time.deltaTime);
			HandleInput();
		}

		private void OnDestroy() {
			EventManager.Unsubscribe<Game_Ended>(OnGameEnded);
		}

		public void EndGame(bool win) {
			ScenePersistence.Get<GamePersistence>().IsWin = win;
			Invoke("LoadFinishScene", 1f);
		}

		void LoadFinishScene() {
			GlobalController.Instance.OpenFinalScene();
		}

		void HandleInput() {
			if ( IsDebug ) {
				if ( Input.GetKeyDown(KeyCode.P) ) {
					var pauseFlag = TimeController.AddOrRemovePause(this);
					Debug.LogFormat("Pause cheat used, new pause state is: {0}", pauseFlag);
				}
				if ( Input.GetKeyDown(KeyCode.U) ) {
					EventManager.Fire<Game_Ended>(new Game_Ended(true));
				}
				if ( Input.GetKeyDown(KeyCode.I) ) {
					EventManager.Fire<Game_Ended>(new Game_Ended(false));
				}
			}
		}

		void OnGameEnded(Game_Ended e) {
			EndGame(e.Win);
		}
	}
}
