using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Game.Events;
using SMGCore.EventSys;
using SMGCore;

namespace Game {
	public enum BreachStatus {
		Small,
		Large,
		Broken,
		Sealed
	}

	public sealed class Breach : MonoBehaviour {
		public float StateChangeTime = 6f;
		public GameObject NormalState;
		public GameObject LargeState;
		public GameObject SealedState;
		public GameObject BrokenState;

		BreachStatus _curStatus = BreachStatus.Small;
		float _lastStateChangeTime = 0f;
		private void Start() {
			UpdateState();
			_lastStateChangeTime = GameState.Instance.TimeController.CurrentTime;
			SoundManager.Instance.PlaySound("window_crack");

		}

		void Update() {
			var curTime = GameState.Instance.TimeController.CurrentTime;
			if ( curTime - _lastStateChangeTime > StateChangeTime ) {
				TryUpgradeBreach();
			}
		}

		void TryUpgradeBreach() {
			if ( _curStatus == BreachStatus.Broken || _curStatus == BreachStatus.Sealed) {
				return;
			}
			if ( _curStatus == BreachStatus.Large ) {
				_curStatus = BreachStatus.Broken;
				EventManager.Fire<Breach_Broken>(new Breach_Broken());
			}
			if ( _curStatus == BreachStatus.Small ) {
				_curStatus = BreachStatus.Large;
			}
			UpdateState();
			_lastStateChangeTime = GameState.Instance.TimeController.CurrentTime;

		}

		void UpdateState() {
			NormalState.SetActive(_curStatus == BreachStatus.Small);
			LargeState.SetActive(_curStatus == BreachStatus.Large);
			BrokenState.SetActive(_curStatus == BreachStatus.Broken);
			SealedState.SetActive(_curStatus == BreachStatus.Sealed);
		}

		public void Seal() {
			_curStatus = BreachStatus.Sealed;
			UpdateState();
			EventManager.Fire<Breach_Sealed>(new Breach_Sealed());
		}
	}

}
