using System.Collections.Generic;
using UnityEngine;

using Game.Events;

using SMGCore.EventSys;

using DG.Tweening;

namespace Game {
	public sealed class DocumentController : MonoBehaviour {
		public SpriteRenderer PortraitSR   = null;
		public Transform    MainTransform = null;

		public Transform ExtendedPosition = null;
		public Transform RetractedPosition = null;

		Dictionary<Archetype, Sprite> _identityPortraits = new Dictionary<Archetype, Sprite>();

		bool _isStamped = false;
		VisitorDescription _desc;

		private void Start() {
			foreach ( var item in VisitorConstants.ArchetypesToIdentites ) {
				var res = Resources.Load<Sprite>($"Visitors/Identities/{item.Value}");
				if ( res ) {
					_identityPortraits.Add(item.Key, res);
				}
			}

			MainTransform.position = RetractedPosition.position;

			EventManager.Subscribe<Visitor_Entered>(this, OnVisitorEntered);
			EventManager.Subscribe<VisitorPlace_Freed>(this, OnVisitorLeft);
		}

		private void OnDestroy() {
			EventManager.Unsubscribe<Visitor_Entered>(OnVisitorEntered);
			EventManager.Unsubscribe<VisitorPlace_Freed>(OnVisitorLeft);
		}

		public void Stamp(StampType type) {
			if (_isStamped ) {
				return;
			}
			_isStamped = true;
			EventManager.Fire<Document_Stamped_Pre>(new Document_Stamped_Pre(type, _desc));
			EventManager.Fire<Document_Stamped>(new Document_Stamped(type, _desc));
		}

		void OnVisitorEntered(Visitor_Entered e) {
			_isStamped = false;
			_desc = e.Description;
			var portrait = _identityPortraits[e.Description.Archetype];
			if ( e.Description.Traits.Contains(VisitorTrait.IdentityMismatch) ) {
				var vals = new List<Sprite>(_identityPortraits.Values);
				portrait = vals[Random.Range(0, vals.Count)];
			}
			PortraitSR.sprite = portrait;

			var seq = DOTween.Sequence();
			MainTransform.position = RetractedPosition.position;
			seq.Append(MainTransform.DOMove(ExtendedPosition.position, 0.5f));
		}

		void OnVisitorLeft(VisitorPlace_Freed e) {
			var seq = DOTween.Sequence();
			seq.AppendInterval(0.5f);
			seq.Append(MainTransform.DOMove(RetractedPosition.position, 0.5f));
			seq.AppendCallback(() => {
				var decals = transform.GetComponentsInChildren<Decal>();
				foreach ( var item in decals ) {
					Destroy(item.gameObject);
				}
			});
		}
	}
}

