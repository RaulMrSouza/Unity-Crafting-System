using System.Collections;
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

		/*private  GameObject[] _tools;
		public GameObject[] Tools{
			get { return _tools; }
			set { _tools = value; }
		}*/

        [Serializable]
        public struct Craftableitems{
            public string type;
            public GameObject[] items;
        }

        [SerializeField] public Craftableitems[] craftableitems;

        public static Manager _manager;

		[SerializeField] 
		private List<GameObject> objectiveItems;

		public Inventory inventory;

		void Awake () {
			_manager = this;
			inventory = this.GetComponent<Inventory> ();
		}

		public static Inventory GetInventory(){
			return _manager.inventory;
		}

        public static GameObject GetCraftableItem(int typeIdx,int itemIdx){
            return Instantiate(_manager.craftableitems[typeIdx].items[itemIdx]);
        }

        /// <summary>
        /// Instantiate the item class by the name
        /// without the GameObject
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
		public static Item GetInventoryItem(string name){
            GameObject g;
            try{
                g = Instantiate(Resources.Load("Items/" + name, typeof(GameObject))) as GameObject;
            }
            catch (System.Exception){
                Debug.Log("Invalid item! Every item must be placed on the SimpleCraft/Assets/Resources/Items/ folder!");
                Debug.Log("Certify that the item's name and Prefab's name are the same!");
                throw;
            }
            
			Item item = g.GetComponent<Item> ();
			Destroy (g);
			return item;
		}

        /// <summary>
        /// Instantiate the GameObject of a Item
        /// </summary>
        /// <param name="name"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
		public static GameObject getItem(string name,float amount){
			GameObject g;
            try{
                g = Instantiate(Resources.Load("Items/"+name, typeof(GameObject))) as GameObject;
            }
            catch (System.Exception){
                Debug.Log("Invalid item! Every item must be placed on the SimpleCraft/Assets/Resources/Items/ folder!");
                throw;
            }
            g.SetActive (true);
			g.GetComponent<Item> ().Amount = amount;
			return g;
		}

        /// <summary>
        /// Instantiate the item at a random nearby position
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pos"></param>
        /// <param name="amount"></param>
		public static bool InstantiateItem(string name,Vector3 pos,float amount){
			GameObject g;

			int x = UnityEngine.Random.Range (-1,1);
			int y = UnityEngine.Random.Range (-1,1);

            //Try to instantiate the item at a random nearby position
            try{
                g = Instantiate(Resources.Load("Items/" + name, typeof(GameObject)),
                new Vector3(pos.x + x, pos.y + y, pos.z + 1), Quaternion.identity) as GameObject;
            }
            catch (System.Exception){
                Debug.Log("Invalid item! Every item must be placed on the SimpleCraft/Items/ folder!");
                throw;
            }
            Collider col = g.GetComponent<Collider>();
            for (int i = 0; i < 10; i++){
                if (!SimplePhysics.CanPlaceItem(col)){
                    x = UnityEngine.Random.Range(-1, 1);
                    y = UnityEngine.Random.Range(-1, 1);
                    g.transform.position = new Vector3(pos.x + x, pos.y + y, pos.z + 1);
                }
                else{
                    g.SetActive(true);
                    g.GetComponent<Item>().Amount = amount;
                    return true;
                }
            }
            Destroy(g);
            return false;
        }

        public static int GetCraftableTypeLength(){
            return _manager.craftableitems.Length;
        }

        public static int GetCraftableitemsLength(int idx){
            return _manager.craftableitems[idx].items.Length;
        }

        public static string GetCraftableType(int idx){
            return _manager.craftableitems[idx].type;
        }


        public static bool CheckObjective(GameObject building){

			if (building.GetComponent<CraftableItem> () == null)
				return false;

			CraftableItem b = building.GetComponent<CraftableItem> ();

			foreach (GameObject objective in _manager.objectiveItems) {
                CraftableItem craftableItem = objective.GetComponent<CraftableItem>();
                    if (b.ItemName == craftableItem.ItemName) {
					_manager.objectiveItems.Remove (objective);
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