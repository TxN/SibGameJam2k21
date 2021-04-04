using DG.Tweening;
using SMGCore;
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
			returnSeq.AppendCallback(() => {
				SoundManager.Instance.PlaySound("phone_hit");
			});
			returnSeq.Append(transform.DOMove(returnPos.position, 0.5f));
			returnSeq.Join(transform.DORotate(returnPos.rotation.eulerAngles, 0.4f));
			
		}

		private void Update() {
			if ( !_active ) {
				return;
			}

			var camRay = TargetCam.ScreenPointToRay(Input.mousePosition);
			Debug.DrawRay(camRay.origin, camRay.direction * 25f, Color.green);

			if ( Physics.Raycast(camRay, out var hit, 30) ) {
				transform.rotation = Quaternion.Euler(26, 51, -83);
				var nearPos = (camRay.direction* 3 + camRay.origin);
				transform.position = Vector3.Lerp(transform.position, nearPos, Time.deltaTime * 20f);
			}
		}
	}
}

