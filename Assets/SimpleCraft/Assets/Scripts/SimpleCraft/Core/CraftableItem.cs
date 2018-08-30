using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using SimpleCraft.Physics;

namespace SimpleCraft.Core{
    /// <summary>
    /// A item that can be crafted by the Player using Items.
    /// Author: Raul Souza
    /// </summary>
    [RequireComponent(typeof(Collider))]
	public class CraftableItem : Item {

		[Serializable]
		public struct CraftCost {
			public GameObject item;
			public float amount;
		}

		[SerializeField]
        private CraftCost[] _craftCost;

        public struct Cost {
			public String item;
			public float amount;
		}

		private List<Cost> _cost = new List<Cost>();
		public List<Cost> BuildingCost{
			get { return _cost; }
			set { _cost = value; }
		}

		[SerializeField] 
		private bool _onlyOnGround = true;
		public bool OnlyOnGround {
			get { return _onlyOnGround; }
			set { _onlyOnGround = value; }
		}

		[SerializeField]
		private bool _isActive = false;
		public bool  IsActive{
			get { return _isActive; }
			set { _isActive = value; }
		}

        [Tooltip("if the item will be static like a building or dynamic like a tool")]
        [SerializeField]
        private bool _hasRigidBody = false;
        public bool HasRigidBody{
            get { return _hasRigidBody; }
            set { _hasRigidBody = value; }
        }

        /*The minimum space away from the player
         when it will be crafted*/
        [SerializeField]
        private float _offset = 2.0f;
        public float Offset{
            get { return _offset; }
            set { _offset = value; }
        }

        void Awake () {
			foreach (CraftCost it in _craftCost) {
				Item item = it.item.GetComponent<Item> ();
				Cost cost;
                if (item == null)
                    Debug.Log(it.item.name +" in "+ this.ItemName + " cost is not a valid item!");
                else{
                    cost.item = item.ItemName;
                    cost.amount = it.amount;
                    _cost.Add(cost);
                }
			}
			_craftCost = null;
			GC.Collect ();
		}

        void Start(){
            if (this.gameObject.layer != LayerMask.NameToLayer("CraftableItem"))
                Debug.Log(this.ItemName + " Craftable Items should be on the CraftableItem's layer!");
        }

		/// <summary>
		/// Check if there is any obstruction
		/// </summary>
		/// <returns><c>true</c> if this instance can build; otherwise, <c>false</c>.</returns>
		public bool CanBuild(){
            return SimplePhysics.CanPlaceItem(DetectionCollider);
		}
    }
}