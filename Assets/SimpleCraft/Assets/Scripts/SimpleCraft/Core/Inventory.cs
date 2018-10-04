using System.Collections.Generic;
using UnityEngine;
using System;

namespace SimpleCraft.Core {
    /// <summary>
    /// A collection of items
    /// Can be used as a Player inventory or a container.
    /// Author: Raul Souza
    /// </summary>
	public class Inventory : MonoBehaviour {
        
		private Dictionary<Item, float> _items = 
			new Dictionary<Item, float>();

		[Serializable]
		public struct InventoryStart {
			public Item item;
			public float amount;
		}

        public enum Type{
			PlayerInventory, Container, Store
		}

        public float Items(Item item){
            return _items[item];
        }

		[SerializeField] private Type _type;
		public Type InvType {
			get { return _type; }
			set { _type = value; }
		}


		[Tooltip("The resources that the inventory will have on start")]
		[SerializeField] private InventoryStart[] _inventoryStart;

        [Tooltip("Maximum weight, use zero for no limit")]
        [SerializeField] private float _maxWeight;
		public float MaxWeight {
			get { return _maxWeight; }
			set { _maxWeight = value; }
		}

        /// <summary>
        /// Current weight
        /// </summary>
		private float _weight = 0.0f;
		public float Weight {
			get { return _weight; }
			set { _weight = value; }
		}

		void Start () {
			foreach (InventoryStart it in _inventoryStart)
				_items.Add(it.item, it.amount);

			_inventoryStart = null;
			GC.Collect();
		}

        public bool HasItem(Item item,float amount = 1){
            if (_items.ContainsKey(item)){
                if (_items[item] >= amount)
                    return true;
            }
            return false;
        }

        public List<Item> ItemKeys(){
            return new List<Item>(_items.Keys);
        }

        public int ItemCount(){
            return _items.Count;
        }

        /// <summary>
        /// Take some item out of the inventory
        /// and instantiate it on the scene.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="amount"></param>
        public bool DropItem(Item item, float amount) {

            if (amount > _items[item])
                amount = _items[item];

            if (Manager.InstantiateItem(item, transform.position, amount)){
                _items[item] -= amount;

                _weight -= item.Weight * amount;

                if (_items[item] == 0)
                    _items.Remove(item);
                return true;
            }
            return false;
        }

        /// <summary>
        /// The items that fit into the inventory will be added,
        /// returns the amount that was added.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="amount"></param>
        /// <param name="player"></param>
        /// <returns></returns>
		public float Add(Item item, float amount,Player player = null){
            //When the inventory has limited capacity
            if (_maxWeight != 0.0f){
                if (item.Weight != 0){
                    if ((item.Weight * amount + _weight) > _maxWeight){
                        amount = (float)Math.Floor((_maxWeight - _weight) / item.Weight);
						if(player != null)
                        	player.QuickMessage.ShowMessage("Inventory is full!");
                    }
                    _weight += item.Weight * amount;
                }
            }
            
            if (_items.ContainsKey(item)){
                if (amount < 0 && amount * (-1) > _items[item])
                    amount = -_items[item];
                _items[item] += amount;
            }
            else
                _items.Add(item, amount);

            if (_items[item] == 0)
                _items.Remove(item);

            return amount;
        }

        /// <summary>
        /// if there is space in the inventory the item will
        /// be added and return true, false otherwise.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public bool TryAdd(Item item,float amount){
            //When the inventory has limited capacity
			if (_maxWeight != 0.0f) {
				if (item.Weight != 0) {
					if ((item.Weight * amount + _weight) > _maxWeight)
						return false;
					else
						_weight += item.Weight * amount;
				}
			}

			if (_items.ContainsKey (item)) {
				if (amount < 0 && amount * (-1) > _items [item])
					amount = -_items[item];
                _items[item] += amount;
			}else
                _items.Add (item, amount);

			if (_items[item] == 0)
                _items.Remove (item);

			return true;
		}
	}
}
