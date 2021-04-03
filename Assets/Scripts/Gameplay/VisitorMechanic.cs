using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SMGCore.EventSys;

using Game.Events;

namespace Game {
	public sealed class VisitorMechanic : MonoBehaviour {
		public int TargetCount = 20;
		public int StartQueueSize = 25;
		public List<VisitorTrait> NoGoTraits = new List<VisitorTrait>();

		public VisitorQueueController QueueController;


		int _progress = 0;
		bool _isFailed = false;

		private void Start() {
			EventManager.Subscribe<Document_Stamped_Pre>(this, OnDocumentStamped);

			QueueController.GenerateInitialQueue(StartQueueSize, 0);
			QueueController.InitSpawn();
		}

		private void OnDestroy() {
			EventManager.Unsubscribe<Document_Stamped_Pre>(OnDocumentStamped);
		}

		void OnDocumentStamped(Document_Stamped_Pre e) {
			if ( IsStampedRight(e.VisitorDesc, e.StampType) ) {
				_progress++;
				CheckProgress();
			} else {
				_isFailed = true;
				QueueController.SpawnEnabled = false;
				EventManager.Fire(new Game_Ended(false));
			}
		}

		void CheckProgress() {
			if ( _progress >= TargetCount ) {
				_isFailed = false;
				QueueController.SpawnEnabled = false;
				EventManager.Fire(new Game_Ended(true));
			}
		}

		bool IsStampedRight(VisitorDescription desc, StampType type) {
			if ( desc.Traits.Contains(VisitorTrait.IdentityMismatch) && type == StampType.Pass ) {
				return false;
			}

			foreach ( var trait in NoGoTraits ) {
				if ( desc.Traits.Contains(trait) ) {
					if ( type == StampType.Pass ) {
						return false;
					}
				}
			}

			return true;
		}
	}

}
