using Game.Events;
using SMGCore.EventSys;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
	public class TapeStand : MonoBehaviour {
		public Collider DropZone = null;


		private void Start() {
			EventManager.Subscribe<Tape_Taken>(this, OnTapeTake);
			EventManager.Subscribe<Tape_Dropped>(this, OnTapeDrop);
			DropZone.enabled = false;
		}

		private void OnDestroy() {
			EventManager.Unsubscribe<Tape_Taken>(OnTapeTake);
			EventManager.Unsubscribe<Tape_Dropped>(OnTapeDrop);
		}

		void OnTapeTake(Tape_Taken e) {
			DropZone.enabled = true;
		}

		void OnTapeDrop(Tape_Dropped e) {
			DropZone.enabled = false;
		}
	}
}
