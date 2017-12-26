using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SimpleCraft{
	/// <summary>
	/// Manages the spawning of craftable itens
	/// by the itens and tools dictionaries
	/// it is used as a static instance.
	/// </summary>
	public class Manager : MonoBehaviour {

		[SerializeField] private  GameObject[] _itens;
		public GameObject[] Itens{
			get { return _itens; }
			set { _itens = value; }
		}

		private  GameObject[] _tools;
		public GameObject[] Tools{
			get { return _tools; }
			set { _tools = value; }
		}

		public static Manager _manager;

		[SerializeField] private List<CraftableItem> objectiveBuilding;

		private Dictionary<string, GameObject> toolDict = 
			new Dictionary<string, GameObject>();

		void Awake () {
			foreach(GameObject  g in Itens) {
				if(g.GetComponent<Tool>()!=null)
					toolDict.Add(g.GetComponent<Tool>().ItemName,g);
			}

			_tools = null;
			_manager = this;
		}

		public static GameObject getBuilding(int idx){
			if(_manager.Itens.Length > idx)
				return Instantiate(_manager.Itens[idx]);
			return null;
		}

		public static GameObject getTool(string toolName){
			if (_manager.toolDict.ContainsKey (toolName))
				return Instantiate (_manager.toolDict [toolName]);
			else
				Debug.Log ("Tool '" + toolName + "' is not in the Manager.");
			return null;
		}

		public static int getBuildingLength(){
				return _manager.Itens.Length;
		}

		public static bool CheckObjective(GameObject building){

			if (building.GetComponent<CraftableItem> () == null)
				return false;

			CraftableItem b = building.GetComponent<CraftableItem> ();

			foreach (CraftableItem objective in _manager.objectiveBuilding) {
				if (b.ItemName == objective.ItemName) {
					_manager.objectiveBuilding.Remove (objective);
					if (_manager.objectiveBuilding.Count == 0) 
						return true;
					else
						return false;
				}
			}
			return false;
		}
	}
}