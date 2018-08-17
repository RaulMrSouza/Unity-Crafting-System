using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SimpleCraft.Core{
	public class Inventory : MonoBehaviour {
        
		public Dictionary<string, float> Itens = 
			new Dictionary<string, float>();

		[Serializable]
		public struct InventoryStart {
			public GameObject item;
			public float amount;
		}

        public enum Type{
			Inventory, Container
		}

		[SerializeField] private Type _type;
		public Type ButtonType {
			get { return _type; }
			set { _type = value; }
		}


		[Tooltip("The resources that the inventory will have on start")]
		[SerializeField] private InventoryStart[] _inventoryStart;

		[SerializeField] private float _maxWeight;
		public float MaxWeight {
			get { return _maxWeight; }
			set { _maxWeight = value; }
		}

		private float _weight = 0.0f;
		public float Weight {
			get { return _weight; }
			set { _weight = value; }
		}

		// Use this for initialization
		void Start () {
			foreach (InventoryStart it in _inventoryStart) {
				Item item = it.item.GetComponent<Item> ();
				Itens.Add(item.ItemName, it.amount);
			}
			_inventoryStart = null;
			GC.Collect();
		}

        /// <summary>
        /// Take some item out of the inventory
        /// and instantiate it on the scene.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="amount"></param>
        public bool DropItem(string name, float amount) {
            if (Manager.InstantiateItem(name, transform.position, amount)){

                if (Itens[name] >= amount)
                    Itens[name] -= amount;
                else{
                    amount = Itens[name];
                    Itens[name] = 0;
                }

                _weight -= Manager.GetInventoryItem(name).Weight * amount;

                if (Itens[name] == 0)
                    Itens.Remove(name);
                return true;
            }
            return false;
        }

        /// <summary>
        /// The itens that fit into the inventory will be added
        /// returns the amount that was added.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="amount"></param>
        /// <param name="player"></param>
        /// <returns></returns>
		public float Add(string name, float amount,Player player = null){
            //When the inventory has limited capacity
            if (_maxWeight != 0.0f){
                Item item = Manager.GetInventoryItem(name);
                if (item.Weight != 0){
                    if ((item.Weight * amount + _weight) > _maxWeight){
                        amount = (float)Math.Floor((_maxWeight - _weight) / item.Weight);
						if(player != null)
                        	player.QuickMessage.ShowMessage("Inventory is full!");
                    }
                    _weight += item.Weight * amount;
                }
            }
            
            if (Itens.ContainsKey(name)){
                if (amount < 0 && amount * (-1) > Itens[name])
                    amount = -Itens[name];
                Itens[name] += amount;
            }
            else
                Itens.Add(name, amount);

            if (Itens[name] == 0)
                Itens.Remove(name);

            return amount;
        }

        /// <summary>
        /// if there is space in the inventory the item will
        /// be added and return true, false otherwise.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public bool TryAdd(string name,float amount){
            //When the inventory has limited capacity
			if (_maxWeight != 0.0f) {
				Item item = Manager.GetInventoryItem (name);
				if (item.Weight != 0) {
					if ((item.Weight * amount + _weight) > _maxWeight)
						return false;
					else
						_weight += item.Weight * amount;
				}
			}

			if (Itens.ContainsKey (name)) {
				if (amount < 0 && amount * (-1) > Itens [name])
					amount = -Itens [name];
				Itens [name] += amount;
			}else
				Itens.Add (name, amount);

			if (Itens [name] == 0)
				Itens.Remove (name);

			return true;
		}
	}
}
