using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SMGCore.EventSys;
using Game.Events;

using DG.Tweening;

namespace Game {
	public sealed class ExclusionAdder : MonoBehaviour {
		public float TriggerTime = 25f;
		[Range(0,100)]
		public int TriggerChance = 40;

		public float RingTimeout = 4f;

		public VisitorMechanic VisitorMechanic;
		public PhoneController Phone;

		Sequence _ringSeq = null;
		bool _isRinging = false;
		bool _enabled = false;

		float _lastRingTime = 0f;

		public bool IsRinging => _isRinging;

		public void Setup(bool enable) {
			_enabled = enable;
			EventManager.Subscribe<Game_Ended>(this, OnGameEnd);
			Phone.Setup(this);
		}

		private void Update() {
			if ( Input.GetKeyDown(KeyCode.M) ) {
				TriggerCall();
			}
			if ( _enabled ) {
				return;
			}
			var ct = GameState.Instance.TimeController.CurrentTime;
			if ( ct - _lastRingTime > TriggerTime ) {
				_lastRingTime = ct;
				if ( Random.Range(0, 100) < TriggerChance && !_isRinging && VisitorMechanic.CanAddExclusion() ) {
					TriggerCall();
				}
			}
		}

		void TriggerCall() {
			Phone.StartRinging();
			_isRinging = true;
			_ringSeq = DOTween.Sequence();
			//TODO: ring sound
			_ringSeq.AppendInterval(RingTimeout);
			_ringSeq.AppendCallback(() => {
				Phone.StopRinging();
				TriggerGameEnd();
			});
		}

		void TriggerGameEnd() {
			if ( GameState.Instance.IsEnded ) {
				return;
			}
			EventManager.Fire<Game_Ended>(new Game_Ended(false, GameResult.MissedCall));
		}

		public void TakePhone() {
			_ringSeq.Kill();
			_isRinging = false;
			EventManager.Fire(new Phone_Taken());
			var exclusionSeq = DOTween.Sequence();
			exclusionSeq.AppendInterval(2f);
			exclusionSeq.AppendCallback(() => {
				VisitorMechanic.TryAddExclusion();
			});
		}

		private void OnDestroy() {
			EventManager.Unsubscribe<Game_Ended>(OnGameEnd);
		}

		void OnGameEnd(Game_Ended e) {
			_enabled = false;
		}
	}
}

