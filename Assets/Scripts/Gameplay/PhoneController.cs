using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using SMGCore.EventSys;
using Game.Events;

using DG.Tweening;
using SMGCore;

namespace Game {
	public sealed class PhoneController : MonoBehaviour, IInteractable {
		public Transform PhoneBody = null;
		public PhoneHandle PhoneHandle = null;

		public Transform HandleRestPosition;

		bool _isHandleLoose = false;

		ExclusionAdder _owner = null;

		Sequence _ringSeq = null;

		public bool CanInteract() {
			return !_isHandleLoose && _owner.IsRinging;
		}

		public void Interact() {
			Debug.Log("phone interact");
			if ( CanInteract() ) {
				TakeHandle();
			}
		}

		public void StopInteraction() {
			
		}

		public void Setup(ExclusionAdder owner) {
			_owner = owner;
		}

		public void StartRinging() {
			_ringSeq = TweenHelper.ReplaceSequence(_ringSeq);
			_ringSeq.AppendCallback(() => {
				SoundManager.Instance.PlaySound("ringtone");
			});
			_ringSeq.Append(PhoneBody.DOShakePosition(0.25f, new Vector3(0.05f, 0.05f, 0.05f), 10, 90, false, false));
			_ringSeq.Join(PhoneBody.DOShakeScale(0.25f, 0.1f));
			_ringSeq.AppendInterval(1f);
			_ringSeq.SetLoops(4);

		}		

		public void StopRinging() {
			_ringSeq = TweenHelper.ResetSequence(_ringSeq);
			if ( _isHandleLoose ) {
				ReturnHandle();
			}
		}

		public void TakeHandle() {
			StopRinging();
			_isHandleLoose = true;
			_owner.TakePhone();
			PhoneHandle.Take();
			SoundManager.Instance.PlaySound("phone_talk");
			var returnSeq = DOTween.Sequence();
			returnSeq.AppendInterval(4f);
			returnSeq.AppendCallback(() => {
				ReturnHandle();
				EventManager.Fire(new PhoneCall_Finished());
			});
		}

		void ReturnHandle() {
			_isHandleLoose = false;
			PhoneHandle.Return(HandleRestPosition);
		}
	}
}

