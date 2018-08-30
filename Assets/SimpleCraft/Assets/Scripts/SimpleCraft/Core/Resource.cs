using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleCraft.Core{
    /// <summary>
    /// A resource has items can be gathered by a player
    /// Author: Raul Souza
    /// </summary>
    public class Resource : MonoBehaviour{

        [SerializeField] private GameObject _item;

        private Item _ItemScript { get; set; }
		public Item Item {
			get { return _ItemScript; }
			set { _ItemScript = value; }
		}

		[SerializeField] private float _amount;
		public float Amount {
			get { return _amount; }
			set { _amount = value; }
		}

		void Start(){
			_ItemScript = _item.GetComponent<Item>();

            if (this.gameObject.tag != "Resource")
                Debug.Log(this.name + " has a Resource script but isn't tagged as Resource!");

        }

        /// <summary>
        /// Called when the player uses a tool on this resource
        /// returns the amount of resource gathered
        /// </summary>
        /// <param name="AmountTaken">Amount taken.</param>
        public float Gather(float AmountTaken){
			if (_amount < AmountTaken)
				AmountTaken = _amount;
			
			_amount -= AmountTaken;

			if (_amount <= 0)
				Destroy (this.gameObject);

			return AmountTaken;
		}
	}
}