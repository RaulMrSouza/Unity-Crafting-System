using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SimpleCraft.UI{
    /// <summary>
    /// Show a mesage for some seconds
    /// Author: Raul Souza
    /// </summary>
    public class QuickMessage : MonoBehaviour{
        Text _Message;

        [SerializeField]
        private GameObject _panel;

        void Start(){
            _Message = this.GetComponent<Text>();
        }

        /// <summary>
        /// Show a message at the bottom left for some seconds
        /// </summary>
        /// <param name="message"></param>
        /// <param name="seconds"></param>
        public void ShowMessage(string message,int seconds = 2){
            _panel.SetActive(true);
            //Set message
            _Message.text = message;
            //Cancel some previous clear message invoke
            CancelInvoke();
            //Clear message after some time
            Invoke("clearMessage", seconds);
        }

        void clearMessage(){
            _panel.SetActive(false);
            _Message.text = "";
        }
    }	
}
