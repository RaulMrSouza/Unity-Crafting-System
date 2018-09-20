using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SimpleCraft.Core{
    [CreateAssetMenu(fileName = "Item", menuName = "Simple Craft/Item", order = 1)]
    /// <summary>
    /// Base Class for Items that can be instantiated and stored
    /// in an inventory
    /// </summary>
    [System.Serializable]
    public class Item : ScriptableObject{
        [SerializeField]
        private string _itemName;
        public string ItemName{
            get { return _itemName; }
            set { _itemName = value; }
        }

        [SerializeField]
        private string _description;
        public string Description{
            get { return _description; }
            set { _description = value; }
        }

        [SerializeField]
        private GameObject _itemObject;
        public GameObject ItemObject{
            get { return _itemObject; }
            set { _itemObject = value; }
        }

        [SerializeField]
        private float _weight = 0.0f;
        public float Weight{
            get { return _weight; }
            set { _weight = value; }
        }

        [SerializeField]
        private int _price = 0;
        public int Price{
            get { return _price; }
            set { _price = value; }
        }

        [SerializeField]
        protected bool _canBePicked = true;
        public bool CanBePicked {
            get { return _canBePicked; }
            set { _canBePicked = value; }
        }
    }
}