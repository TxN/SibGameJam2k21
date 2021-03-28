using UnityEngine;

using SMGCore;

namespace Game {
	public sealed class GameState : MonoSingleton<GameState> {
		public readonly TimeController TimeController = new TimeController();

		public bool IsDebug {
			get {
				return true; //Should be set to 'false' for release build.
			}
		}

		void Update() {
			TimeController.Update(Time.deltaTime);
			HandleInput();
		}

		void HandleInput() {
			if ( IsDebug ) {
				if ( Input.GetKeyDown(KeyCode.P) ) {
					var pauseFlag = TimeController.AddOrRemovePause(this);
					Debug.LogFormat("Pause cheat used, new pause state is: {0}", pauseFlag);
				}
			}
		}
	}
}
