using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleCraft.Core{
    /// <summary>
    /// Handles the Tools carried by the Player.
    /// Author: Raul Souza
    /// </summary>
    public class ToolHandler : MonoBehaviour{
		
		[SerializeField] private Player _player;

		[SerializeField] private Tool _currentTool;
		public Tool CurrentTool {
			get { return _currentTool; }
			set { _currentTool = value; }
		}

		private GameObject _toolObject;
		public GameObject ToolObject {
			get { return _toolObject; }
			set { _toolObject = value; }
		}

		private bool _OnAttack;
		public bool OnAttack {
			get { return _OnAttack; }
			set { _OnAttack = value; }
		}

		private Animator _Animator;

		void Start (){
			_Animator = GetComponent<Animator> ();
		}

		public void Attack (){
			_OnAttack = true;
			_Animator.SetTrigger ("Swing");
		}

		public void ChangeTool (GameObject newTool){
			if (_toolObject != null)
				_toolObject.SetActive (false);
			
			_toolObject = newTool;
			_currentTool = _toolObject.GetComponent<Tool> ();

			_currentTool.DetectionCollider.enabled = false;
			if (_toolObject.GetComponent<Rigidbody> () != null) {
				Destroy (_toolObject.GetComponent<Rigidbody> ());
			}

			_toolObject.transform.position = transform.position;
			_toolObject.transform.rotation = transform.rotation;
			_toolObject.transform.SetParent (transform);
			_toolObject.SetActive (true);
		}

		void OnTriggerEnter (Collider collider){
			if (_OnAttack)
				if (collider.gameObject.tag == "Resource") {
					Resource resource = collider.gameObject.GetComponent<Resource> ();

					if (resource != null) {
						_OnAttack = false;

						float gatherPower = _currentTool.GatherPower;

					    if (_currentTool.GatherFactor (resource.Item.ItemName) != -1)
						    gatherPower = gatherPower * _currentTool.GatherFactor (resource.Item.ItemName);

                        float amountGathered = resource.Gather(gatherPower);
                        float amount = _player.Inventory.Add(resource.Item.ItemName, amountGathered, _player);

                        if (amount > 0)
                            _player.QuickMessage.ShowMessage("Gathered "+ amount + " " + resource.Item.ItemName);

                        if (amount < amountGathered)
						    Manager.InstantiateItem (resource.Item.ItemName,this.transform.position,  amountGathered - amount);
					}
				}
		}
	}
}