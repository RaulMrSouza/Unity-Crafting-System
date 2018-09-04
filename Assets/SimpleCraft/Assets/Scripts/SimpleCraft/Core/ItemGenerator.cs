using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleCraft.Core{
    /// <summary>
    /// It will generate a item every interval and place it in the
    /// Settlement manager's inventory
    /// Author: Raul Souza
    /// </summary>
	public class ItemGenerator : MonoBehaviour {
		
		[SerializeField]
		private float _interval;

		[SerializeField]
		private float _amount = 1;

		[SerializeField]
		private GameObject _itemGenerated;

        [SerializeField]
        private bool _spawn;

        [SerializeField]
        private Transform _spawnPos;

        private CraftableItem _craftableItem;

		void Start () {
			_craftableItem = this.GetComponent<CraftableItem> ();
			InvokeRepeating ("GenerateItem", _interval, _interval);
            if (_spawnPos == null)
                _spawnPos = transform;
		}

		void GenerateItem(){
			if(_craftableItem.IsActive){
                if(!_spawn)
				    Manager.GetInventory ().Add (_itemGenerated.GetComponent<Item>().ItemName, _amount);
                else
                    Manager.InstantiateItem (_itemGenerated.GetComponent<Item>().ItemName,_spawnPos.position, _amount);
            }
		}
	}
}