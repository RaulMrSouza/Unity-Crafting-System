using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace SimpleCraft{
	public class Player : MonoBehaviour {
		[SerializeField] private  Text _actionText;
		[SerializeField] private  Text _objectiveText;
		[SerializeField] private  GameObject _scrollView;
		[SerializeField] private   Text _costText;
		[SerializeField] private  GameObject _costScrollView;
		[SerializeField] private  GameObject _pauseMenu;
		[SerializeField] private  Transform _raycaster;
		[SerializeField] private ToolHandler _toolHandler;
		[SerializeField] private int _maxTools = 0;
		[SerializeField] private List<GameObject> _tools;
		[SerializeField] private  Text _inventoryText;


		[Serializable]
		public struct InventoryStart {
			public Resource.Type resource;
			public float amount;
		}

		[Tooltip("The resources that the player will have on start")]
		[SerializeField] private InventoryStart[] _inventoryStart;

		private GameObject _itemObj;
		private CraftableItem _currentItem;
		private int _itemIdx = 0;
		private int _toolIdx = 0;

		private LayerMask _focusLayers;
		private LayerMask _CraftLayers;

		private bool _buildingMode = false;
		private bool _menuMode = false;

		private enum Interaction{
			GrabTool,GrabResource  
		}

		private Interaction _interaction;
		private GameObject _interactionObj;


		private Dictionary<Resource.Type, float> inventory = 
			new Dictionary<Resource.Type, float>();
		
		/// <summary>
		/// Check where the floor is on a navmesh to craft a item
		/// </summary>
		UnityEngine.AI.NavMeshHit _hitTerrain;


		RaycastHit _hit;
	    Transform _cam;

		void Start () {
			foreach (InventoryStart  inv in _inventoryStart) {
				inventory.Add(inv.resource, inv.amount);
			}

			_inventoryStart = null;
			GC.Collect();

			_cam =  Camera.main.transform;

			if (Manager.getBuildingLength() >= 1) {
				_itemIdx = 0;
				_itemObj = Manager.getBuilding(_itemIdx);
			}

			if (_tools.Count >= 1) {
				_toolIdx = 0;
				_toolHandler.ChangeTool (_tools[_toolIdx]);
			}

			_focusLayers = LayerMask.GetMask ("Default","CraftableItem");
			_CraftLayers = LayerMask.GetMask ("Default");
			Debug.Log("l"+_focusLayers+"s"+_CraftLayers);
			Time.timeScale = 1.0f;

			Cursor.visible = false;

			_pauseMenu.SetActive (_menuMode);
		}
			
		void Update () {
			if (Input.GetKeyDown (KeyCode.Escape)) {
				showPauseMenu ();
			}

			if (_menuMode)
				return;

			if (Input.GetKeyDown (KeyCode.B)) {
				_buildingMode = !_buildingMode;
				if (_itemObj == null)
					_itemObj = Manager.getBuilding (_itemIdx);
				if (_itemObj != null) {
					_itemObj.SetActive (_buildingMode);
					_actionText.text = "";
					_costScrollView.SetActive (_buildingMode);
					_currentItem = _itemObj.GetComponent<CraftableItem> ();
					this.drawCostView (_currentItem);
				}
			}

			if (_buildingMode)
				OnBuildingMode ();
			else{
				if (Input.GetMouseButtonDown (0) && _toolHandler.CurrentTool != null)
					_toolHandler.Attack ();

				if (Input.GetKeyDown (KeyCode.E) && _interactionObj != null) {
					if (_interaction == Interaction.GrabTool) {
						_tools.Add (Manager.getTool (_interactionObj.GetComponent<Tool> ().ItemName));
						Destroy (_interactionObj);
						_interactionObj = null;
					} else if (_interaction == Interaction.GrabResource) {
						Resource resource = _interactionObj.GetComponent<Resource> ();
						this.addResource (resource.ResType, resource.Amount);
						resource.Gather (resource.Amount);
						_interactionObj = null;
					}
				}

				if (Input.GetKeyDown (KeyCode.G)) {
					_tools.Remove (_toolHandler.ToolObject);
					_toolHandler.Drop ();
				}

				if (Input.GetAxis ("Mouse ScrollWheel") != 0) {
					if(_tools.Count != 0){
						if (Input.GetAxis ("Mouse ScrollWheel") > 0) {
							if (_toolIdx < _tools.Count - 1) {
								_toolIdx += 1;
							} else
								_toolIdx = 0;
						} else if (Input.GetAxis ("Mouse ScrollWheel") < 0) {
							if (_toolIdx > 0)
								_toolIdx -= 1;
							else
								_toolIdx = _tools.Count - 1;
						}

						_toolHandler.ChangeTool(_tools[_toolIdx]);
					}
				}

				if (Input.GetKeyDown (KeyCode.I)) {
					_scrollView.SetActive (!_scrollView.activeSelf);
					if (_scrollView.activeSelf)
						this.drawInventory ();
				}

				CheckPlayerFocus ();
			}
		}

		/// <summary>
		/// Checks where the player camera is focusing and
		/// display a message about it.
		/// </summary>
		void CheckPlayerFocus(){
			if (Physics.Raycast(_cam.position, _cam.forward, out _hit, 5,_focusLayers.value)) {
				if (_hit.transform.gameObject.tag == "Tool") {
					if (_maxTools != 0 && _tools.Count >= _maxTools)
						_actionText.text = "Can't hold more tools!";
					else {
						_actionText.text = "Press (E) to grab " + _hit.transform.gameObject.GetComponent<Tool> ().ItemName;
						_interaction = Interaction.GrabTool;
						_interactionObj = _hit.transform.gameObject;
					}
				} else if (_hit.transform.gameObject.tag == "Resource") {
					Resource resource = _hit.transform.gameObject.GetComponent<Resource> ();
					if (resource.IsPickable) {
						_actionText.text = "Press (E) to grab " + resource.ResType + " x " + resource.Amount;
						_interaction = Interaction.GrabResource;
						_interactionObj = _hit.transform.gameObject;
					} else
						_actionText.text = "Resource " + resource.ResType;
				} else {
					_interactionObj = null;
					_actionText.text = "";
				}
			} else
				_actionText.text = "";
		}

		void OnBuildingMode(){
			if(_currentItem == null)
				return;
			
			_raycaster.position =  transform.position + transform.forward * _currentItem.Offset;
			_raycaster.position = new Vector3(_raycaster.position.x, _raycaster.position.y+5, _raycaster.position.z);

			if (Physics.Raycast(_raycaster.position , _raycaster.forward, out _hit, 20,_CraftLayers.value)) {
				if (Vector3.Distance (_hit.point, this.gameObject.transform.position) >= _currentItem.Offset) {

					if (UnityEngine.AI.NavMesh.SamplePosition (_hit.point, out _hitTerrain, 100.0f, UnityEngine.AI.NavMesh.AllAreas)) {
						_itemObj.transform.position = new Vector3 (_hitTerrain.position.x, _hitTerrain.position.y, _hitTerrain.position.z);
					}
				}
			}

			if (_currentItem != null) {
				if (!_currentItem.CanBuild())
					_actionText.text = "Can't build there!";
				else if (!hasResources (_currentItem))
					_actionText.text = "Lack of the required resources!";
				else
					_actionText.text = "";
			}

			if (Input.GetAxis ("Mouse ScrollWheel") != 0) {
				if (Input.GetAxis ("Mouse ScrollWheel") > 0) {
					if (_itemIdx <  Manager.getBuildingLength() - 1) {
						_itemIdx += 1;
					} else
						_itemIdx = 0;
				} else if (Input.GetAxis ("Mouse ScrollWheel") < 0) {
					if (_itemIdx > 0)
						_itemIdx -= 1;
					else
						_itemIdx = Manager.getBuildingLength() - 1;
				}

				_itemObj.SetActive (false);
				Destroy (_itemObj);
				_itemObj = null;
				_itemObj = Manager.getBuilding (_itemIdx);
				_itemObj.SetActive (true);

				_currentItem = _itemObj.GetComponent<CraftableItem> ();
				this.drawCostView (_currentItem);

				if (_currentItem.HasRigidBody && _itemObj.GetComponent<Rigidbody> () != null)
					_itemObj.GetComponent<Rigidbody> ().detectCollisions = false;
			}

			//Rotate item on mouse click
			if (Input.GetMouseButton (1)) {
				_itemObj.transform.Rotate (0, 30 * Time.deltaTime, 0);
			} else if (Input.GetMouseButton (0)) {
				_itemObj.transform.Rotate (0, -30 * Time.deltaTime, 0);
			}

			//try to place the item 
			if (Input.GetKeyDown (KeyCode.E)) {
				if (this.hasResources (_currentItem)) {
					takeResources (_currentItem);
					GameObject g = Instantiate (_itemObj);

					if (Manager.CheckObjective (_itemObj)) {
						_objectiveText.text = "Objective Completed";
					}

					if (_currentItem.HasRigidBody && g.GetComponent<Rigidbody> () != null)
						g.GetComponent<Rigidbody> ().detectCollisions = true;
				}
			}
		}

		/// <summary>
		/// Add some resource to the inventory
		/// used when gathering
		/// </summary>
		/// <param name="resource">Resource.</param>
		/// <param name="amount">Amount.</param>
		public void addResource(Resource.Type resource,float amount){
			if (inventory.ContainsKey (resource)) 
				inventory [resource] += amount;
			else
				inventory.Add (resource, amount);

			if (inventory [resource] == 0)
				inventory.Remove (resource);
			
			if (_buildingMode)
				drawCostView (_currentItem);
		}

		/// <summary>
		/// Check if there are enough resources on the inventory in order
		/// to craft the item.
		/// </summary>
		/// <returns><c>true</c>, if resources was hased, <c>false</c> otherwise.</returns>
		/// <param name="building">Building.</param>
		bool hasResources(CraftableItem craftableItem){
			foreach (CraftableItem.Cost buildingCost in craftableItem.BuildingCost) {
				if (!inventory.ContainsKey (buildingCost.resource))
					return false;
				if (inventory [buildingCost.resource] < buildingCost.amount)
					return false;
			}
			return true;
		}

		/// <summary>
		/// Takes the item's cost resources out of the inventory.
		/// </summary>
		/// <param name="building">Building.</param>
		void takeResources(CraftableItem craftableItem){
			foreach (CraftableItem.Cost buildingCost in craftableItem.BuildingCost) {
				addResource (buildingCost.resource, -buildingCost.amount);
			}
			this.drawCostView (craftableItem);
		}

		/// <summary>
		/// Shows the item's resource cost in a scrollView
		/// </summary>
		/// <param name="building">Building.</param>
		void drawCostView (CraftableItem building){
			RectTransform Content;

			Content = _costScrollView.GetComponent<ScrollRect> ().content;

			_costText.text = building.ItemName;
			_costText.text += "\nCost (required/inventory)";

			foreach (CraftableItem.Cost buildingCost in building.BuildingCost) {
				if (!inventory.ContainsKey (buildingCost.resource)) {
					_costText.text += "\n" + Enum.GetName (typeof(Resource.Type), buildingCost.resource);
					_costText.text += " ("+buildingCost.amount+"/0)";
				}else{
					_costText.text += "\n" + Enum.GetName (typeof(Resource.Type), buildingCost.resource);
					_costText.text += " ("+buildingCost.amount+"/"+inventory[buildingCost.resource]+")";
				}
			}

			Content.GetComponent<RectTransform>().sizeDelta = new Vector2 (0, (building.BuildingCost.Length+2)*30);
			_costText.GetComponent<RectTransform>().sizeDelta = new Vector2 (160, (building.BuildingCost.Length+2)*30);
		}

		/// <summary>
		/// Shows the amount of each resource on the inventory
		/// </summary>
		void drawInventory (){
			RectTransform Content;
			_scrollView.SetActive(true);

			Content = _scrollView.GetComponent<ScrollRect> ().content;
			_inventoryText.text = "Inventory"; 

			foreach (Resource.Type resource in inventory.Keys) {
				_inventoryText.text += "\n" + Enum.GetName(typeof(Resource.Type),resource);
				_inventoryText.text += ": " + inventory [resource];
			}

			Content.GetComponent<RectTransform>().sizeDelta = new Vector2 (0, (inventory.Count+1)*30);
			_inventoryText.GetComponent<RectTransform>().sizeDelta = new Vector2 (160, (inventory.Count+1)*30);
		}

		void showPauseMenu(){
			_menuMode = !_menuMode;
			Cursor.visible = _menuMode;
			_pauseMenu.SetActive (_menuMode);
			if(_menuMode)
				Time.timeScale = 0.0f;
			else
				Time.timeScale = 1.0f;
		}
	}
}