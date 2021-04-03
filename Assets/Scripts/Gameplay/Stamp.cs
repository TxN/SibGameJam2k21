using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Game.Events;

using SMGCore.EventSys;

using DG.Tweening;

namespace Game {
	public enum StampType {
		Pass,
		Fail
	}

	public sealed class Stamp : DraggableBody {
		public StampType Type;

		protected override void Start() {
			base.Start();
			EventManager.Subscribe<Document_Stamped>(this, OnDocumentStamped);
		}

		protected override void OnDestroy() {
			base.OnDestroy();
			EventManager.Unsubscribe<Document_Stamped>(OnDocumentStamped);
		}

		public override void Take() {
			base.Take();
			EventManager.Fire(new Stamp_Taken());
		}

		public override void Drop() {
			base.Drop();
			EventManager.Fire(new Stamp_Dropped());
		}

		protected override bool IsOnDropZone(Collider hitCol) {
			return hitCol.GetComponentInParent<StampStand>() != null;
		}

		protected override void Action(RaycastHit hit) {
			var decal = Instantiate(DecalFab, hit.collider.transform);
			decal.transform.position = transform.position - hit.normal * 0.3f;
			decal.transform.rotation = transform.rotation;
			decal.SetActive(true);
			var stampSeq = DOTween.Sequence();
			stampSeq.Append(StampObj.transform.DOLocalMoveY(_stampStartPos.y - 0.3f, 0.2f));
			stampSeq.Append(StampObj.transform.DOLocalMoveY(_stampStartPos.y + 0.1f, 0.15f));
			stampSeq.Append(StampObj.transform.DOLocalMoveY(_stampStartPos.y, 0.15f));

			if ( hit.collider.GetComponent<IStampable>() != null ) {
				hit.collider.GetComponent<IStampable>().Stamp(Type);
			}
		}

		void OnDocumentStamped(Document_Stamped e) {
			if ( e.StampType != Type ) {
				return;
			}
			_active = false;
			var returnSeq = DOTween.Sequence();
			returnSeq.AppendInterval(0.5f);
			returnSeq.Append(transform.DOMove(RestZone.position, 0.5f));
			returnSeq.Join(transform.DORotate(RestZone.rotation.eulerAngles, 0.4f));
			returnSeq.AppendCallback(Drop);
		}
	}
}
