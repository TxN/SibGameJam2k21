using System.Collections.Generic;
using UnityEngine;

using SMGCore.EventSys;

using Game.Events;

namespace Game {
	public sealed class VisitorMechanic : MonoBehaviour {
		public ExclusionDisplay ExclusionDisplay = null;
		public VisitorQueueController QueueController;

		List<VisitorTrait> _noGoTraits = new List<VisitorTrait>();
		int _progress = 0;
		bool _isFailed = false;

		public int TargetCount { get; private set; }

		private void Start() {
			EventManager.Subscribe<Document_Stamped_Pre>(this, OnDocumentStamped);
		}

		private void OnDestroy() {
			EventManager.Unsubscribe<Document_Stamped_Pre>(OnDocumentStamped);
		}

		public void Setup(int queueSize, int exclusionsCount, int targetCount, List<VisitorTrait> bannedTraits) {
			_noGoTraits.Clear();
			foreach ( var trait in bannedTraits ) {
				_noGoTraits.Add(trait);
			}
			QueueController.Setup(this);
			QueueController.GenerateInitialQueue(queueSize, exclusionsCount);
			QueueController.InitSpawn();
			TargetCount = targetCount;
			foreach ( var ex in QueueController.Exclusions ) {
				ExclusionDisplay.SetupExclusion(QueueController.InstantiateVisitor(ex));
			}
		}

		public bool CanAddExclusion() {
			return ExclusionDisplay.HasFreePlaces();
		}

		public void TryAddExclusion() {
			if ( !ExclusionDisplay.HasFreePlaces() ) {
				return;
			}
			var desc = QueueController.AddNewExclusion();
			if ( desc != null ) {
				ExclusionDisplay.SetupExclusion(QueueController.InstantiateVisitor(desc));
			}
			
		}

		void OnDocumentStamped(Document_Stamped_Pre e) {
			if ( IsStampedRight(e.VisitorDesc, e.StampType, out var stampResult) ) {
				_progress++;
				CheckProgress();
			} else {
				_isFailed = true;
				QueueController.SpawnEnabled = false;
				EventManager.Fire(new Game_Ended(false, stampResult));
			}
		}

		void CheckProgress() {
			if ( _progress >= TargetCount ) {
				_isFailed = false;
				QueueController.SpawnEnabled = false;
				EventManager.Fire(new Game_Ended(true, GameResult.Win));
			}
		}

		bool IsStampedRight(VisitorDescription desc, StampType type, out GameResult stampResult) {
			bool hasBannedTraits = HasBannedTraits(desc);

			if ( type == StampType.Pass ) {
				if ( desc.Traits.Contains(VisitorTrait.IdentityMismatch) ) {
					stampResult = GameResult.WrongIdentity;
					return false;
				}
				if ( hasBannedTraits ) {
					if ( !QueueController.IsInExclusionsList(desc) ) {
						stampResult = GameResult.BannedTraitPassed;
						return false;
					}
				}
				stampResult = GameResult.Win;
				return true;
			}

			if ( QueueController.IsInExclusionsList(desc) ) {
				stampResult = GameResult.ExclusionNotPassed;
				return false;
			}

			if ( !hasBannedTraits ) {
				stampResult = GameResult.WronglyYeeted;
				return false;
			}
			stampResult = GameResult.Win;
			return true;
		}

		public bool HasBannedTraits(VisitorDescription desc) {
			foreach ( var trait in _noGoTraits ) {
				if ( desc.Traits.Contains(trait) ) {
					return true;
				}
			}
			return false;
		}
	}

}
