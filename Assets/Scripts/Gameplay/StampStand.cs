using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Game.Events;

using SMGCore.EventSys;

namespace Game {
	public sealed class StampStand : MonoBehaviour {
		public Collider DropZone = null;
		

		private void Start() {
			EventManager.Subscribe<Stamp_Taken>(this, OnStampTake);
			EventManager.Subscribe<Stamp_Dropped>(this, OnStampDrop);
			DropZone.enabled = false;
		}

		private void OnDestroy() {
			EventManager.Unsubscribe<Stamp_Dropped>(OnStampDrop);
			EventManager.Unsubscribe<Stamp_Taken>(OnStampTake);
		}

		void OnStampTake(Stamp_Taken e) {
			DropZone.enabled = true;
		}

		void OnStampDrop(Stamp_Dropped e) {
			DropZone.enabled = false;
		}
	}
}

