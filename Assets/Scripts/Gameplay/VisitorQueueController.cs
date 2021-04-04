using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

using Game.Events;
using SMGCore.EventSys;

using SMGCore;
using System.Linq;

namespace Game {
	public sealed class VisitorQueueController : MonoBehaviour {
		public int StartQueueSize = 20;

		[Range(0,100)]
		public int IdentityMismatchChance = 10;

		[Range(0, 100)]
		public int HatChance = 15;

		public VisitorSpawner Spawner = null;


		Queue<VisitorDescription> _visitorQueue = new Queue<VisitorDescription>();
		List<VisitorDescription> _currentExclusions = new List<VisitorDescription>();

		Dictionary<string, Visitor> _visitorFabs = new Dictionary<string, Visitor>();
		Dictionary<VisitorTrait, GameObject> _hats = new Dictionary<VisitorTrait, GameObject>();

		VisitorDescription _currentVisitor = null;

		VisitorMechanic _owner = null;

		public bool SpawnEnabled { get; set; } = true;

		public List<VisitorDescription> Exclusions => _currentExclusions;

		public void Setup(VisitorMechanic owner) {
			_owner = owner;
		}

		public void InitSpawn() {
			if ( _currentVisitor != null ) {
				return;
			}
			GenerateNewVisitor();
		}

		protected  void Awake() {
			
			foreach ( var visitorName in VisitorConstants.VisitorNames ) {
				var fab = Resources.Load<Visitor>($"Visitors/{visitorName}");
				if ( !fab ) {
					Debug.LogWarning($"No fab for {visitorName}");
					continue;
				}
				_visitorFabs.Add(visitorName, fab);
			}

			foreach ( var hatPair in VisitorConstants.HatTraitsToNames ) {
				var fab = Resources.Load<GameObject>($"Hats/{hatPair.Value}");
				if ( !fab ) {
					Debug.LogWarning($"No fab for {hatPair.Value}");
					continue;
				}
				_hats.Add(hatPair.Key, fab);
			}

			EventManager.Subscribe<VisitorPlace_Freed>(this, OnVisitorLeft);
		}

		private void OnDestroy() {
			EventManager.Unsubscribe<VisitorPlace_Freed>(OnVisitorLeft);
		}

		public bool IsInExclusionsList(VisitorDescription desc) {
			foreach ( var exclusion in _currentExclusions ) {
				if ( desc.IsEqual(exclusion) ) {
					return true;
				}
			}
			return false;
		}

		public void GenerateInitialQueue(int visitorCount, int exclusionCount) {
			_visitorQueue.Clear();
			_currentExclusions.Clear();

			var sb = new StringBuilder(2048);
			sb.Append($"Generate visitor queue (count {visitorCount}, exclusions {exclusionCount}:\nQueue: ");
			for ( int i = 0; i < visitorCount; i++ ) {
				var description = CreateRandomVisitor();
				_visitorQueue.Enqueue(description);
				sb.Append(description.ToString());
				sb.Append("\n");
			}
			sb.Append("excllusions queue:\n");
			for ( int i = 0; i < exclusionCount; i++ ) {
				var description = AddNewExclusion();
				sb.Append(description?.ToString());
				sb.Append("\n");		
			}
			Debug.Log(sb.ToString());
		}

		public VisitorDescription AddNewExclusion() {
			var description = PickExclusionFromList(_visitorQueue.ToList(), _currentExclusions);
			if ( description != null ) {
				_currentExclusions.Add(description);
			}
			return description;
		}

		VisitorDescription PickExclusionFromList(List<VisitorDescription> visitors, List<VisitorDescription> exclusions) {
			var suitableVisitors = new List<VisitorDescription>();
			foreach ( var visitor in visitors ) {
				if ( _owner.HasBannedTraits(visitor) && !visitor.Traits.Contains(VisitorTrait.IdentityMismatch) ) {
					var canAdd = true;
					foreach ( var ex in exclusions ) {
						if ( ex.IsEqual(visitor) ) {
							canAdd = false; //already in exclusion list
						}
					}
					if ( canAdd ) {
						suitableVisitors.Add(visitor);
					}					
				}
			}
			if ( suitableVisitors.Count <= 0) {
				return null;
			}
			return suitableVisitors[Random.Range(0, suitableVisitors.Count)];

		}

		VisitorDescription CreateRandomVisitor(VisitorTrait excludeTrait = VisitorTrait.None) {
			var description = new VisitorDescription();

			var tryCount = 10;
			var name = VisitorConstants.VisitorNames[0];
			while (tryCount > 0 ) {
				var randomIndex = Random.Range(0, VisitorConstants.VisitorNames.Count);
				name = VisitorConstants.VisitorNames[randomIndex];
				if ( IsVisitorHasStaticTrait(name, excludeTrait) ) {
					tryCount--;
					continue;
				}
				break;
			}
			var identityMismatch = Random.Range(0, 100) < IdentityMismatchChance;
			var hat = Random.Range(0, 100) < HatChance;
			var fab = _visitorFabs[name];
			description.Name = name;
			description.Archetype = fab.Description.Archetype;
			foreach ( var trait in fab.Description.Traits ) {
				description.Traits.Add(trait);
			}

			if ( hat ) {
				description.Traits.Add(VisitorConstants.HatTraitsToNames[Random.Range(0, VisitorConstants.HatTraitsToNames.Count)].Key);
			}
			if ( identityMismatch ) {
				description.Traits.Add(VisitorTrait.IdentityMismatch);
			}
			return description;
		}

		bool IsVisitorHasStaticTrait(string name, VisitorTrait trait) {
			var fab = _visitorFabs[name];
			if ( fab.Description.Traits.Contains(trait) ) {
				return true;
			}
			return false;
		}

		void GenerateNewVisitor() {

			if ( _visitorQueue.Count > 0 ) {
				var visitor = _visitorQueue.Dequeue();
				_currentVisitor = visitor;
				Spawner.SpawnVisitor(InstantiateVisitor(visitor));
				EventManager.Fire(new Visitor_Spawned(visitor));
			}
		}

		void OnVisitorLeft(VisitorPlace_Freed e) {
			_currentVisitor = null;
			if ( SpawnEnabled ) {
				GenerateNewVisitor();
			}
			
		}

		public Visitor InstantiateVisitor(VisitorDescription desc) {
			var inst = Instantiate<Visitor>(_visitorFabs[desc.Name]);
			inst.Description = desc;
			foreach ( var trait in desc.Traits ) {
				var hatObj = TryGetHatObject(trait);
				if ( hatObj ) {
					inst.SetupAttachement(trait, hatObj);
				}
			}

			return inst;
		}

		GameObject TryGetHatObject(VisitorTrait trait) {
			if ( _hats.ContainsKey(trait) ) {
				var inst = Instantiate(_hats[trait]);
				return inst;
			}
			return null;
		}
	}


	public static class VisitorConstants {
		public static List<string> VisitorNames = new List<string> {
			"VisitorShark0",
			"VisitorShark1",
			"VisitorShark2",
			"VisitorWhale0",
			"VisitorWhale1",
			"VisitorWhale2",
			"VisitorFrog0",
			"VisitorFrog1",
			"VisitorFrog2",
			"VisitorSquid0",
			"VisitorSquid1",
			"VisitorSquid2",
			"VisitorSad0",
			"VisitorSad1",
			"VisitorSad2",
			"VisitorShar0",
			"VisitorShar1",
			"VisitorShar2",
			"VisitorCrab0",
			"VisitorCrab1",
			"VisitorCrab2",
		};

		public static Dictionary<Archetype, string> ArchetypesToIdentites = new Dictionary<Archetype, string> {
			{Archetype.Shark, "Type2" },
			{Archetype.Squid, "Type7" },
			{Archetype.Crab, "Type1" },
			{Archetype.Whale, "Type5" },
			{Archetype.Sad, "Type4" },
			{Archetype.Shar, "Type3" },
			{Archetype.Frog, "Type6" }
		};

		public static List<KeyValuePair<VisitorTrait, string>> HatTraitsToNames = new List<KeyValuePair<VisitorTrait, string>> {
			new KeyValuePair<VisitorTrait, string>(VisitorTrait.Crown,   "HatCrown"),
			new KeyValuePair<VisitorTrait, string>(VisitorTrait.Fedora,   "HatFedora"),
			new KeyValuePair<VisitorTrait, string>(VisitorTrait.Plunger,   "HatPlunger")
		};

		public static bool IsHatTrait(this VisitorTrait trait) {
			foreach ( var t in HatTraitsToNames ) {
				return t.Key == trait;
			}
			return false;
		}
	}

	public enum VisitorTrait {
		None,
		Fish,
		Mechanical,
		Human,
		Demonic,
		Crown,
		Fedora,
		Plunger,
		IdentityMismatch
	}

	public enum Archetype {
		Shark,
		Squid,
		Frog,
		Crab,
		Sad,
		Whale,
		Shar,
	}
}

