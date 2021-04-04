using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

namespace Game {
	public sealed class ShiftIndicator : MonoBehaviour {
		public TMP_Text ServiceCountText = null;
		public TMP_Text TimeText = null;

		TimeLimit _timeLimiter = null;
		VisitorMechanic _visitorMechanic = null;

		public void Setup(TimeLimit timeLimiter, VisitorMechanic visitorMechanic) {
			_timeLimiter = timeLimiter;
			_visitorMechanic = visitorMechanic;

		}

		void Update() {
			ServiceCountText.text = $"Принято: {_visitorMechanic.Progress}/{_visitorMechanic.TargetCount}";
			var timeLeft = _timeLimiter.TimeLeft;
			var minutesLeft = Mathf.FloorToInt(timeLeft / 60);
			var secondsLeft = Mathf.FloorToInt(timeLeft - minutesLeft * 60);
			TimeText.text = $"{minutesLeft.ToString("00")}:{secondsLeft.ToString("00")}";
		}
	}
}

