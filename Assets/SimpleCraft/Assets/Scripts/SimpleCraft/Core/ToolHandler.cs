using UnityEngine;

namespace SimpleCraft.Core{
    /// <summary>
    /// Handles the Tools carried by the Player.
    /// Author: Raul Souza
    /// </summary>
    public class ToolHandler : MonoBehaviour{
		
		[SerializeField]
        private Player _player;

		[SerializeField]
        private Tool _currentTool;
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

		private Animator _Animator;

		void Start (){
			_Animator = GetComponent<Animator> ();
		}

		public void Attack (){
			_OnAttack = true;
			_Animator.SetTrigger ("Swing");

            if(_currentTool.SwingSound != null)
                _currentTool.SwingSound.Play();

            CancelInvoke();
            Invoke("SetAttack",2);
        }

        private void SetAttack() {
            _OnAttack = false;
        }

		public void ChangeTool (Tool newTool){
			if (_toolObject != null)
				_toolObject.SetActive (false);

            GameObject oldTool = _toolObject;

			_toolObject = Manager.getItem(newTool,1);

            Destroy(oldTool);

            _currentTool = newTool;

            _toolObject.GetComponent<ItemReference>().DetectionCollider.enabled = false;

			if (_toolObject.GetComponent<Rigidbody> () != null) 
				Destroy (_toolObject.GetComponent<Rigidbody> ());

            Vector3 pos = new Vector3(transform.position.x,
                                       transform.position.y,
                                       transform.position.z);

            _toolObject.SetActive(true);
            _toolObject.transform.rotation = transform.rotation;
            _toolObject.transform.position = pos;
            _toolObject.transform.SetParent (transform);
            _currentTool.InitializeDictionary();
        }

		void OnTriggerEnter (Collider collider){
            if (_OnAttack) {
                if (collider.gameObject.tag == "Resource") {
                    Resource resource = collider.gameObject.GetComponent<Resource>();

                    if (resource != null) {
                        _OnAttack = false;

                        float gatherPower = _currentTool.GatherPower;
                        Item item = resource.Item;

                        if (_currentTool.GatherFactor(item) != -1)
                            gatherPower = gatherPower * _currentTool.GatherFactor(item);

                        float amountGathered = resource.Gather(gatherPower);
                        float amount = _player.Inventory.Add(item, amountGathered, _player);

                        if (amount > 0)
                            _player.QuickMessage.ShowMessage("Gathered " + amount + " " + item.ItemName);

                        if (amount < amountGathered)
                            Manager.InstantiateItem(item, _player.transform.position, amountGathered - amount);
                    }
                }

                if (_currentTool.DestructionTool) {
                    _OnAttack = false;
                    if (collider.gameObject.GetComponent<ItemReference>() != null)
                        collider.gameObject.GetComponent<ItemReference>().DestroyItem();
                }
            }
		}
	}
}