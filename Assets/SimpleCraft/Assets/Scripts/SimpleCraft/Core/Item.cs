using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SimpleCraft.Core{
    /// <summary>
    /// Base class item, the items that go into the inventory
    /// Author: Raul Souza
    /// </summary>
	public class Item : MonoBehaviour {

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

        private GameObject _itemObject;
        public GameObject ItemObject{
            get { return _itemObject; }
            set { _itemObject = value; }
        }

        [SerializeField]
        private Collider _detectionCollider;
        public Collider DetectionCollider{
            get { return _detectionCollider; }
            set { _detectionCollider = value; }
        }

        [SerializeField] private float _weight = 0.0f;
		public float Weight{
			get{ return _weight;}
			set{ _weight = value;}
		}

		[SerializeField] private float _amount = 1;
		public float Amount{
			get{ return _amount;}
			set{ _amount = value;}
		}

		[SerializeField] private int _price = 0;
		public int Price{
			get{ return _price;}
			set{ _price = value;}
		}

		void Start () {
			ItemObject = this.gameObject;

            if (this.gameObject.tag != "Item")
                Debug.Log(this.name + " has a Item script but isn't tagged as Item!");
        }
    }
}
