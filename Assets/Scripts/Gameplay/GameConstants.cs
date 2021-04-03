using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
	public class LevelDescription {
		public float LevelTime = 150;
		public int InitialExclusions = 0;
		public int PlannedCount = 10;
		public List<VisitorTrait> BannedTraits = new List<VisitorTrait>();
	}
	public static class GameConstants  {
		public static List<LevelDescription> Levels = new List<LevelDescription> {
			new LevelDescription {
				LevelTime = 150f,
				InitialExclusions = 0,
				PlannedCount = 10,
				BannedTraits = new List<VisitorTrait> { VisitorTrait.Human}
			},
			new LevelDescription {
				LevelTime = 150f,
				InitialExclusions = 0,
				PlannedCount = 15,
				BannedTraits = new List<VisitorTrait> { VisitorTrait.Human, VisitorTrait.Mechanical}
			},
			new LevelDescription {
				LevelTime = 150f,
				InitialExclusions = 1,
				PlannedCount = 20,
				BannedTraits = new List<VisitorTrait> { VisitorTrait.Human, VisitorTrait.Mechanical, VisitorTrait.Demonic, VisitorTrait.Plunger, VisitorTrait.Crown, VisitorTrait.Fedora}
			}
		};
	}
}
