using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
	public sealed class LimitsDisplay : MonoBehaviour {
		public GameObject HumanIcon = null;
		public GameObject MechIcon = null;
		public GameObject HatIcon = null;


		public void Setup(List<VisitorTrait> bannedTraits) {
			HumanIcon.SetActive(bannedTraits.Contains(VisitorTrait.Human));
			MechIcon.SetActive(bannedTraits.Contains(VisitorTrait.Mechanical));
			HatIcon.SetActive(bannedTraits.Contains(VisitorTrait.Fedora) || bannedTraits.Contains(VisitorTrait.Crown) || bannedTraits.Contains(VisitorTrait.Plunger));
		}
	}
}
