using UnityEngine;

namespace SimpleCraft.Core{
    /// <summary>
    /// It will generate a item every interval and place it in the
    /// Settlement manager's inventory
    /// Author: Raul Souza
    /// </summary>
	public class ItemGenerator : MonoBehaviour {
		
		[SerializeField]
		private float _interval;

		[SerializeField]
		private float _amount = 1;

		[SerializeField]
		private Item _itemGenerated;

        [SerializeField]
        private bool _spawn;

        [SerializeField]
        private Transform _spawnPos;

        private ItemReference _craftableItem;

		void Start () {
			_craftableItem = this.GetComponent<ItemReference> ();
			InvokeRepeating ("GenerateItem", _interval, _interval);
            if (_spawnPos == null)
                _spawnPos = transform;
		}

		void GenerateItem(){
			if(_craftableItem.IsActive){
                if(!_spawn)
				    Manager.GetInventory ().Add (_itemGenerated, _amount);
                else
                    Manager.InstantiateItem (_itemGenerated,_spawnPos.position, _amount);
            }
		}
	}
}