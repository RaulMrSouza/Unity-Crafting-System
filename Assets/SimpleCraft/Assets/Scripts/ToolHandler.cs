using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleCraft{
	/// <summary>
	/// Handles the Tools carried by the Player.
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

		public void Drop (){
			if (_toolObject != null) {
				_toolObject.transform.parent = null;
				_toolObject.AddComponent<Rigidbody> ();
				_currentTool.DetectionCollider.enabled = true;
				_toolObject = null;
				_currentTool = null;
			}
		}

		void OnTriggerEnter (Collider collider){
			if (_OnAttack)
				if (collider.gameObject.tag == "Resource") {
					Resource resource = collider.gameObject.GetComponent<Resource> ();

					if (resource != null) {
						_OnAttack = false;

						float gatherPower = _currentTool.GatherPower;

						if (_currentTool.GatherFactor (resource.ResType) != -1)
							gatherPower = gatherPower * _currentTool.GatherFactor (resource.ResType);
						
						_player.addResource (resource.ResType, resource.Gather (gatherPower));
					}
				}
		}
	}
}