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
		[SerializeField] private GameObject _keyItem;
        [SerializeField] private string _animationTrigger;
        [SerializeField] private string _successMessage;

        public string SuccessMessage{
            get { return _successMessage; }
            set { _successMessage = value; }
        }

        private Animator animator;

        void Start(){
            animator = GetComponent<Animator>();
        }

		public bool UseItem(string item){
            Item keyItem = _keyItem.GetComponent<Item>();
            if (item == keyItem.ItemName){
                performeAction();
                return true;
            }
            return false;  
		}

		void performeAction(){
            animator.SetTrigger(_animationTrigger);
		}
	}
}
