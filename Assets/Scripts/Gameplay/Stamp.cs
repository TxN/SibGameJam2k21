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

	public sealed class Stamp : MonoBehaviour, IInteractable {
		public float ActionCoooldown = 0.4f;

		public Camera TargetCam = null;
		public StampType Type;
		public GameObject DecalFab;
		public Collider OwnCollider;
		public Transform RestZone;
		public GameObject StampObj;


		float _cooldown = 0f;
		bool _active = false;

		Vector3 _stampStartPos = Vector3.zero;

		void Start() {
			_stampStartPos = StampObj.transform.localPosition;
			EventManager.Subscribe<Document_Stamped>(this, OnDocumentStamped);
		}

		void OnDestroy() {
			EventManager.Unsubscribe<Document_Stamped>(OnDocumentStamped);
		}

		public void Take() {
			_active = true;
			OwnCollider.enabled = false;
			_cooldown = ActionCoooldown;
			EventManager.Fire(new Stamp_Taken());
		}

		public void Drop() {
			transform.position = RestZone.position;
			transform.rotation = RestZone.rotation;
			_active = false;
			OwnCollider.enabled = true;
			EventManager.Fire(new Stamp_Dropped());
		}

		void Update() {
			if ( !_active ) {
				return;
			}

			_cooldown -= Time.deltaTime;

			var camRay = TargetCam.ScreenPointToRay(Input.mousePosition);
			Debug.DrawRay(camRay.origin, camRay.direction, Color.green);

			if ( Physics.Raycast(camRay, out var hit, 30) ) {
				//transform.position = hit.point + hit.normal * 0.3f;
				transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.FromToRotation(Vector3.up, hit.normal), Time.deltaTime * 25f);
				transform.position = Vector3.Lerp(transform.position, hit.point + hit.normal * 0.4f, Time.deltaTime * 25f);

				if ( Input.GetMouseButtonDown(0) && hit.collider.GetComponentInParent<StampStand>() && _cooldown < 0 ) {
					Drop();
					return;
				}

				if ( Input.GetMouseButtonDown(0) && _cooldown < 0 && hit.collider.gameObject.tag != "IgnoreStamp" ) {
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

					_cooldown = ActionCoooldown;
				}
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

		public void Interact() {
			if ( !_active ) {
				Take();
			}
		}

		public void StopInteraction() {
			if ( _active ) {
				Drop();
			}
		}
	}
}

