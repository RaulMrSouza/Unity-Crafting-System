using System.Collections.Generic;
using UnityEngine;
using System;
using SimpleCraft.Physics;

namespace SimpleCraft.Core{
    /// <summary>
    /// Manages the spawning of craftable items
    /// and scene objective
    /// it is used as a static instance.
    /// Author: Raul Souza
    /// </summary>
    public class Manager : MonoBehaviour {

        [Serializable]
        public struct CraftableItems{
            public string type;
            public CraftableItem[] items;
        }

        [SerializeField]
        public CraftableItems[] craftableItems;

        public static Manager _manager;

		[SerializeField]
        private List<CraftableItem> objectiveItems;

        [SerializeField]
        private Item CurrencyItem;

        public Inventory inventory;

		void Awake () {
			_manager = this;
			inventory = this.GetComponent<Inventory> ();
		}

		public static Inventory GetInventory(){
			return _manager.inventory;
		}

        public static Item Currency(){
            return _manager.CurrencyItem;
        }

        public static CraftableItem GetCraftableItem(int typeIdx,int itemIdx){
            return Instantiate(_manager.craftableItems[typeIdx].items[itemIdx]);
        }

        public static GameObject GetCraftableItemObj(int typeIdx, int itemIdx){
            return Instantiate(_manager.craftableItems[typeIdx].items[itemIdx].ItemObject);
        }

        /// <summary>
        /// Instantiate the GameObject of a Item
        /// </summary>
        /// <param name="name"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
		public static GameObject getItem(Item item,float amount){
			GameObject g;
            g = Instantiate(item.ItemObject) as GameObject;
            g.SetActive (true);
			g.GetComponent<ItemReference> ().Amount = amount;
			return g;
		}

        /// <summary>
        /// Instantiate the item at a random nearby position
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pos"></param>
        /// <param name="amount"></param>
		public static bool InstantiateItem(Item item,Vector3 pos,float amount){
			GameObject g;

			int x = UnityEngine.Random.Range (-1,1);
			int z = UnityEngine.Random.Range (-1,1);

            //Try to instantiate the item at a random nearby position
            g = Instantiate(item.ItemObject,
            new Vector3(pos.x + x, pos.y + 0.2f, pos.z + z), Quaternion.identity) as GameObject;

            Collider col = g.GetComponent<Collider>();
            for (int i = 0; i < 10; i++){
                if (!SimplePhysics.CanPlaceItem(col)){
                    x = UnityEngine.Random.Range(-2, 2);
                    z = UnityEngine.Random.Range(-2, 2);
                    g.transform.position = new Vector3(pos.x + x, pos.y + 1, pos.z + z);
                }
                else{
                    g.SetActive(true);
                    g.GetComponent<ItemReference>().Amount = amount;
                    return true;
                }
            }
            Destroy(g);
            return false;
        }

        public static int GetCraftableTypeLength(){
            return _manager.craftableItems.Length;
        }

        public static int GetCraftableitemsLength(int idx){
            return _manager.craftableItems[idx].items.Length;
        }

        public static string GetCraftableType(int idx){
            return _manager.craftableItems[idx].type;
        }


        public static bool CheckObjective(CraftableItem craftableItem){

			if (craftableItem== null)
				return false;
            
			foreach (CraftableItem item in _manager.objectiveItems) {
                if (craftableItem.ItemName == item.ItemName) {
                    _manager.objectiveItems.Remove (item);
					    if (_manager.objectiveItems.Count == 0) 
						    return true;
					    else
						    return false;
				    }
			}
			return false;
		}
    }
}