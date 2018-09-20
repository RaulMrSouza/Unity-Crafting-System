using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using SimpleCraft.Core;

namespace SimpleCraft.UI{
    /// <summary>
    /// Button for the inventory UI
    /// Author: Raul Souza
    /// </summary>
	public class ItemButton : MonoBehaviour {

		[SerializeField] private Player _player;
		public Player Player {
			get { return _player; }
			set { _player = value; }
		}
			
		[SerializeField] private Inventory.Type _type;
		public Inventory.Type InventoryType {
			get { return _type; }
			set { _type = value; }
		}

		[SerializeField] private Item _item;
		public Item Item {
			get { return _item; }
			set { _item = value; }
		}

		public void Select(){
			_player.SelectItem (_item, _type);
		}
	}
}
