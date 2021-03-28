using UnityEngine;
using UnityEngine.UI;

using SMGCore;
using SMGCore.EventSys;
using SMGCore.Windows;
using SMGCore.Windows.Events;

using DG.Tweening;

namespace KOZA.Windows {
	public sealed class PauseWindow : MonoBehaviour {

		[Header("Interactive Elements")]
		public Button HomeButton     = null;
		public Button ReplayButton   = null;
		public Button ContinueButton = null;

		WindowController _controller = null;
		Sequence         _hideSeq    = null;

		public void Show() {
			_controller = GetComponent<WindowController>();

			EventManager.Subscribe<Event_WindowHidden>(this, OnWindowHide);

			HomeButton.onClick.AddListener(OnHomeButton);
			ReplayButton.onClick.AddListener(OnReplayButton);
			ContinueButton.onClick.AddListener(OnContinueButton);

			_controller.Show();
			
		}

		void OnDestroy() {
			EventManager.Unsubscribe<Event_WindowHidden>(OnWindowHide);
			_hideSeq = TweenHelper.ResetSequence(_hideSeq);
		}

		void HideActions() {
			
		}

		void HideWithCallBack(System.Action cb, bool fade) {
			if ( fade ) {
				
			}
			_hideSeq = TweenHelper.ReplaceSequence(_hideSeq);
			if ( fade ) {
				_hideSeq.AppendInterval(0.1f);
				_hideSeq.AppendCallback(() => { _controller.Hide(); });
			} else {
				_controller.Hide();
			}

			_hideSeq.AppendInterval(0.4f);
			_hideSeq.AppendCallback(() => {
				HideActions();
				cb();
			});
		}

		void OnHomeButton() {
			HideWithCallBack(() => {
				
			}, true);
		}

		void OnReplayButton() {
			HideWithCallBack(() => {

			}, true);
		}

		void OnContinueButton() {
			_controller.Hide();
		}

		void OnWindowHide(Event_WindowHidden e) {
			if ( e.Window != _controller ) {
				return;
			}
			HideActions();
		}
	}
}
