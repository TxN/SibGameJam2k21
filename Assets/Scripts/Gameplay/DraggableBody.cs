using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
	public abstract class DraggableBody : MonoBehaviour, IInteractable {
		public float ActionCoooldown = 0.4f;

		public Camera TargetCam = null;

		public GameObject DecalFab;
		public Collider OwnCollider;
		public Transform RestZone;
		public GameObject StampObj;


		protected float _cooldown = 0f;
		protected bool _active = false;

		protected Vector3 _stampStartPos = Vector3.zero;

		protected virtual void Start() {
			_stampStartPos = StampObj.transform.localPosition;
		}

		protected virtual void Update() {
			if ( !_active ) {
				return;
			}

			_cooldown -= Time.deltaTime;

			var camRay = TargetCam.ScreenPointToRay(Input.mousePosition);
			Debug.DrawRay(camRay.origin, camRay.direction * 25f, Color.green);

			if ( Physics.Raycast(camRay, out var hit, 30) ) {
				transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.FromToRotation(Vector3.up, hit.normal), Time.deltaTime * 25f);
				transform.position = Vector3.Lerp(transform.position, hit.point + hit.normal * 0.4f, Time.deltaTime * 25f);

				if ( Input.GetMouseButtonDown(0) && IsOnDropZone(hit.collider) && _cooldown < 0 ) {
					Drop();
					return;
				}

				if ( Input.GetMouseButtonDown(0) && _cooldown < 0 && hit.collider.gameObject.tag != "IgnoreStamp" ) {
					Action(hit);
					_cooldown = ActionCoooldown;
				}
			}
		}

		protected abstract bool IsOnDropZone(Collider hitCol);

		protected abstract void Action(RaycastHit hit);

		protected virtual void OnDestroy() {

		}

		public virtual void Take() {
			_active = true;
			OwnCollider.enabled = false;
			_cooldown = ActionCoooldown;
		}

		public virtual void Drop() {
			transform.position = RestZone.position;
			transform.rotation = RestZone.rotation;
			_active = false;
			OwnCollider.enabled = true;
		}

		public virtual void Interact() {
			if ( !_active ) {
				Take();
			}
		}

		public virtual void StopInteraction() {
			if ( _active ) {
				Drop();
			}
		}

		public bool CanInteract() {
			return true;
		}
	}

}
