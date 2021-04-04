using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;
using SMGCore;
using SMGCore.EventSys;
using Game.Events;

namespace Game {
	public sealed class VisitorSpawner : MonoBehaviour {
		public Transform CenterPoint;
		public Transform SpawnPoint;
		public Transform YeetPoint;
		public Transform PassPoint;

		public Transform ScoopTransform;
		public SlideShowAnim ScoopAnim;

		Visitor _activeVisitor  = null;
		bool    _enterCompleted = false;

		private void Start() {
			EventManager.Subscribe<Document_Stamped>(this, OnDocumentStamped);
		}

		private void OnDestroy() {
			EventManager.Unsubscribe<Document_Stamped>(OnDocumentStamped);
		}

		public bool CanSpawnVisitor() {
			return !_activeVisitor;
		}

		public bool CanStampVisitor() {
			return _activeVisitor && _enterCompleted;
		}

		public void SpawnVisitor(Visitor visitor) {
			if ( !CanSpawnVisitor() ) {
				return;
			}
			visitor.transform.position = SpawnPoint.position;
			_enterCompleted = false;
			_activeVisitor = visitor;
			var enterSeq = DOTween.Sequence();
			enterSeq.Append(visitor.transform.DOMove(CenterPoint.position, 2f));
			enterSeq.AppendCallback(() => {
				_enterCompleted = true;
				OnVisitorEntered(visitor.Description);
			});

		}

		public void YeetVisitor() {
			if ( !_activeVisitor ) {
				return;
			}
			
			var yeetSeq = DOTween.Sequence();
			//TODO: play effects and sound
			var visitor = _activeVisitor;
			_activeVisitor = null;
			yeetSeq.AppendInterval(0.5f);
			yeetSeq.AppendCallback( () => {
				ScoopAnim.gameObject.SetActive(true);
				SoundManager.Instance.PlaySound("suction");
			});
			yeetSeq.Append(visitor.transform.DOMove(YeetPoint.position, 0.75f));
			yeetSeq.InsertCallback(0.8f,() => {
				ScoopAnim.gameObject.SetActive(false);
			});
			yeetSeq.Join(visitor.transform.DOScaleY(1.4f, 0.3f));
			yeetSeq.Join(visitor.transform.DOScaleX(0.6f, 0.3f));
			yeetSeq.AppendCallback(() => {
				Destroy(visitor.gameObject);
			});
			OnVisitorPlaceFreed();
		}

		public void PassVisitor() {
			if ( !_activeVisitor ) {
				return;
			}
			var passSeq = DOTween.Sequence();
			//TODO: play effects and sound
			var visitor = _activeVisitor;
			_activeVisitor = null;
			passSeq.AppendInterval(0.75f);
			passSeq.Append(visitor.transform.DOMove(PassPoint.position, 2f));
			passSeq.AppendCallback(() => {
				Destroy(visitor.gameObject);
			});
			OnVisitorPlaceFreed();
		}

		void OnVisitorPlaceFreed() {
			EventManager.Fire(new VisitorPlace_Freed());
		}

		void OnVisitorEntered(VisitorDescription desc) {
			EventManager.Fire(new Visitor_Entered(desc));
		}

		void OnDocumentStamped(Document_Stamped e) {
			Debug.Log($"stamped: {e.StampType}");
			if ( e.StampType == StampType.Pass ) {
				PassVisitor();
			} else {
				YeetVisitor();
			}
		}
	}
}
