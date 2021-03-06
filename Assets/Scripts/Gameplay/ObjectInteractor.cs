using UnityEngine;

using Game.Events;

using SMGCore.EventSys;

namespace Game {
	public class ObjectInteractor : MonoBehaviour {
		public Camera TargetCam = null;

		IInteractable _activeItem = null;

		private void Start() {
			EventManager.Subscribe<Stamp_Dropped>(this, OnStampDrop);
			EventManager.Subscribe<Tape_Dropped>(this, OnTapeDrop);
			EventManager.Subscribe<PhoneCall_Finished>(this, OnPhoneCallFinished);
		}

		private void OnDestroy() {
			EventManager.Unsubscribe<Stamp_Dropped>(OnStampDrop);
			EventManager.Unsubscribe<Tape_Dropped>(OnTapeDrop);
			EventManager.Unsubscribe<PhoneCall_Finished>(OnPhoneCallFinished);
		}

		void Update() {
			if ( _activeItem != null ) {
				return;
			}
			var camRay = TargetCam.ScreenPointToRay(Input.mousePosition);
			Debug.DrawRay(camRay.origin, camRay.direction, Color.red);

			if ( Physics.Raycast(camRay, out var hit, 10) ) {
				var interactable = hit.collider.gameObject.GetComponent<IInteractable>();
				if ( interactable == null ) {
					interactable = hit.collider.gameObject.GetComponentInParent<IInteractable>();
				}
				if ( interactable == null ) {
					return;
				}
				//Debug.Log(interactable);
				if ( interactable.CanInteract() && Input.GetMouseButtonDown(0) ) {
					interactable.Interact();
					_activeItem = interactable;
				}
			}
		}

		void OnStampDrop(Stamp_Dropped e) {
			_activeItem = null;
		}

		void OnTapeDrop(Tape_Dropped e) {
			_activeItem = null;
		}

		void OnPhoneCallFinished(PhoneCall_Finished e) {
			_activeItem = null;
		}
	}

}
