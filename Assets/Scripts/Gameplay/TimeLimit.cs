using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SMGCore.EventSys;
using Game.Events;

namespace Game {
	public class TimeLimit : MonoBehaviour {

		float _timeSpan = 0f;
		float _timePassed = 0f;
		bool _enabled = false;
		public void Setup(float timeLimit) {
			_enabled = true;
			_timeSpan = timeLimit;
			EventManager.Subscribe<Game_Ended>(this, OnGameEnd);
		}

		private void Update() {
			var tc = GameState.Instance.TimeController;
			if ( !_enabled ) {
				return;
			}
			_timePassed += tc.DeltaTime;
			if ( _timePassed > _timeSpan + 0.4f ) {
				_enabled = false;
				EventManager.Fire<Game_Ended>(new Game_Ended(false, GameResult.TimeIsOff));
			}
		}

		private void OnDestroy() {
			EventManager.Unsubscribe<Game_Ended>(OnGameEnd);
		}

		void OnGameEnd(Game_Ended e) {
			_enabled = false;
		}
	}
}

