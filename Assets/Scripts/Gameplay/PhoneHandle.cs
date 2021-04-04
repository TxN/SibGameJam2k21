using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
	public sealed class PhoneHandle : MonoBehaviour {
		public Camera TargetCam;

		bool _active = false;

		public void Take() {
			_active = true;
			var col = GetComponentInChildren<Collider>();
			if ( col ) {
				col.enabled = false;
			}
		}

		public void Return(Transform returnPos) {
			_active = false;
			var col = GetComponentInChildren<Collider>();
			if ( col ) {
				col.enabled = true;
			}
			var returnSeq = DOTween.Sequence();
			returnSeq.AppendInterval(0.5f);
			returnSeq.Append(transform.DOMove(returnPos.position, 0.5f));
			returnSeq.Join(transform.DORotate(returnPos.rotation.eulerAngles, 0.4f));
			returnSeq.AppendCallback( () => {
				//TODO: drop sound
			});
		}

		private void Update() {
			if ( !_active ) {
				return;
			}

			var camRay = TargetCam.ScreenPointToRay(Input.mousePosition);
			Debug.DrawRay(camRay.origin, camRay.direction * 25f, Color.green);

			if ( Physics.Raycast(camRay, out var hit, 30) ) {
				transform.rotation = Quaternion.Euler(26, 51, -83);
				transform.position = Vector3.Lerp(transform.position, hit.point + hit.normal, Time.deltaTime * 20f);
			}
		}
	}
}

