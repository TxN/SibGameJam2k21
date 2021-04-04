using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Game.Events;
using SMGCore.EventSys;
using DG.Tweening;

namespace Game {
	public sealed class Tape : DraggableBody {
		public GameObject GlassDecalFab = null;

		protected override void Start() {
			base.Start();
			EventManager.Subscribe<Breach_Sealed>(this, OnBreachSealed);
			transform.position = RestZone.position;
			transform.rotation = RestZone.rotation;
		}

		protected override void OnDestroy() {
			base.OnDestroy();
			EventManager.Unsubscribe< Breach_Sealed>(OnBreachSealed);
		}

		protected override void Action(RaycastHit hit) {
			if ( hit.collider.GetComponentInParent<Breach>() != null ) {
				hit.collider.GetComponentInParent<Breach>().Seal();
			} else if ( hit.collider.GetComponent<Glass>() ) {
				var fab = Instantiate(GlassDecalFab, hit.collider.transform);
				fab.gameObject.SetActive(true);
				fab.transform.position = hit.point;
				fab.transform.rotation = hit.collider.transform.rotation * Quaternion.Euler(0,0, Random.Range(-180, 180));
			} else {
				var decal = Instantiate(DecalFab, hit.collider.transform);
				decal.transform.position = transform.position - hit.normal * 0.3f;
				decal.transform.rotation = transform.rotation;
				decal.SetActive(true);
			}
		}

		protected override bool IsOnDropZone(Collider hitCol) {
			var flag = hitCol.GetComponent<TapeStand>() != null;
			return flag;
		}

		void OnBreachSealed(Breach_Sealed e) {
			_active = false;
			var returnSeq = DOTween.Sequence();
			returnSeq.AppendInterval(0.35f);
			returnSeq.Append(transform.DOMove(RestZone.position, 0.5f));
			returnSeq.Join(transform.DORotate(RestZone.rotation.eulerAngles, 0.4f));
			returnSeq.AppendCallback(Drop);
		}

		public override void Take() {
			base.Take();
			EventManager.Fire(new Tape_Taken());
		}

		public override void Drop() {
			base.Drop();
			EventManager.Fire(new Tape_Dropped());
		}

	}
}

