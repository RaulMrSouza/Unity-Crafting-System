using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleCraft.Core{
	public class ItemGenerator : MonoBehaviour {
		
		[SerializeField]
		private float _interval;

		[SerializeField]
		private float _amount = 1;

		[SerializeField]
		private GameObject _itemGenerated;

		private CraftableItem _craftableItem;

		// Use this for initialization
		void Start () {
			_craftableItem = this.GetComponent<CraftableItem> ();
			InvokeRepeating ("GenerateItem", _interval, _interval);
		}

		void GenerateItem(){
			if(_craftableItem.IsActive){
				Manager.GetInventory ().Add (_itemGenerated.GetComponent<Item>().ItemName, _amount);
			}
		}
	}
}