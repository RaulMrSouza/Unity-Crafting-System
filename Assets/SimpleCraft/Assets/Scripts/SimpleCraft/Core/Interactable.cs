using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleCraft.Core{
    /// <summary>
    /// Performs some action when a key Item
    /// is used on it
    /// Author: Raul Souza
    /// </summary>
	public class Interactable : MonoBehaviour {
		[SerializeField]
        private Item _keyItem;

        [SerializeField]
        private string _animationTrigger;

        [SerializeField]
        private bool _playAnimation;

        [SerializeField]
        private bool _giveItem;

        [SerializeField]
        private bool _removeKeyItem;

        [SerializeField]
        private Item _itemGiven;

        [SerializeField]
        private string _successMessage;
        public string SuccessMessage{
            get { return _successMessage; }
            set { _successMessage = value; }
        }

        [SerializeField]
        private GameObject _returnItem;

        private Animator animator;

        void Start(){
            animator = GetComponent<Animator>();
        }

		public bool UseItem(Item item,Inventory inv){
            if (item == _keyItem){
                PerformAction(item,inv);
                return true;
            }
            return false;  
		}

		void PerformAction(Item item,Inventory inv){
            if (_playAnimation)
                animator.SetTrigger(_animationTrigger);

            if(_giveItem && _itemGiven!= null) 
                inv.Add(_itemGiven,1);

            if (_removeKeyItem)
                inv.Add(item, -1);
		}
	}
}
