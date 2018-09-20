using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleCraft.Core{
    [CreateAssetMenu(fileName = "CraftableItem", menuName = "Simple Craft/CraftableItem", order = 2)]
    /// <summary>
    /// Items that can be crafted using Component Items
    /// </summary>
    [System.Serializable]
    public class CraftableItem : Item {
        [System.Serializable]
        public struct CraftCost {
            public Item item;
            public float amount;
        }

        [SerializeField]
        private List<CraftCost> _craftCost = new List<CraftCost>();
        public List<CraftCost> GetCraftCost {
            get { return _craftCost; }
            set { _craftCost = value; }
        }

        [Tooltip("Can be crafted only on a valid point in a navigation mesh")]
        [SerializeField]
        private bool _onlyOnGround = true;
        public bool OnlyOnGround {
            get { return _onlyOnGround; }
            set { _onlyOnGround = value; }
        }

        [Tooltip("if the item will be static like a building or dynamic like a tool")]
        [SerializeField]
        protected bool _hasRigidBody = false;
        public bool HasRigidBody {
            get { return _hasRigidBody; }
            set { _hasRigidBody = value; }
        }

        [Tooltip("The minimum space away from the player when it will be crafted")]
        [SerializeField]
        private float _offset = 2.0f;
        public float Offset {
            get { return _offset; }
            set { _offset = value; }
        }

        [SerializeField]
        private float _yCraftCorrection = 0.0f;
        public float YCraftCorrection {
            get { return _yCraftCorrection; }
            set { _yCraftCorrection = value; }
        }

        public CraftableItem() {
            _canBePicked = false;
        }
    }
}
