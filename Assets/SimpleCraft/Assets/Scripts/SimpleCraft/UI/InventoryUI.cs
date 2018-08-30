using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleCraft.Core;

namespace SimpleCraft.UI{
    /// <summary>
    /// The UI of the Player's Inventory
    /// and UI between player and containers inventory
    /// Author: Raul Souza
    /// </summary>
	public class InventoryUI : MonoBehaviour {

		[SerializeField] private  GameObject _invScrollView;

        //Used when accessing anoyher inventory
		[SerializeField] private  GameObject _secondInvScrollView;

		[SerializeField] private  Button _inventoryButton;
		[SerializeField] private  Button _secondInvButton;
		[SerializeField] private  InputField _inputAmount;

		[SerializeField] private  Text _descriptionText;
		[SerializeField] private  Text _itemNameText;

		[SerializeField] private  Text _inventoryWeight;

		Player _player;
		Button[] _but;
		Button[] _secondBut;

		void Start(){
			_player = this.GetComponent<Player> ();
			_invScrollView.SetActive (false);
			_secondInvScrollView.SetActive (false);
		}

        /// <summary>
        /// Set up the inventory UI by type
        /// </summary>
        /// <param name="inventory"></param>
		public void Draw(Inventory inventory){
			if (inventory.ButtonType == Inventory.Type.Inventory) {
				DrawInventoryItem (_invScrollView, _inventoryButton, inventory, ref _but);
				_inventoryWeight.text = "Weight: "+inventory.Weight + "/" + inventory.MaxWeight;
			}else
				DrawInventoryItem (_secondInvScrollView,_secondInvButton,inventory, ref _secondBut);
		}


        /// <summary>
        /// Update the item's description
        /// </summary>
        /// <param name="item"></param>
		public void SelectItem(Item item){
			_itemNameText.text = item.ItemName;
			_descriptionText.text = item.Description;

			_descriptionText.text += "\n\n" + "Weigth: " + item.Weight; 

			_descriptionText.text += "   Price: " + item.Price;
		}

        /// <summary>
        /// the amount of items to be transferred or dropped
        /// </summary>
        /// <returns></returns>
		public float GetAmount(){
			float amount = 1;

			if(_inputAmount.text != "")
				amount = float.Parse(_inputAmount.text);

			return amount;
		}

        /// <summary>
        /// Instantiate a button for each item, positioning one
        /// bellow the other and setting scrollview size according
        /// to the items
        /// </summary>
        /// <param name="inventoryScrollView"></param>
        /// <param name="inventoryButton"></param>
        /// <param name="inventory"></param>
        /// <param name="but"></param>
		void DrawInventoryItem (GameObject inventoryScrollView,
			Button inventoryButton,
			Inventory inventory,
			ref Button[] but){

			RectTransform Content;
			inventoryScrollView.SetActive(true);
			Inventory.Type buttonType = inventory.ButtonType;
			Content = _invScrollView.GetComponent<ScrollRect> ().content; 

			inventoryButton.gameObject.SetActive(true);

			Cursor.visible = true;
			Time.timeScale = 0;
			DestroyButtons (but);
			but = new Button[inventory.Items.Count];
			int i = 0;
			foreach (string name in inventory.Items.Keys) {
				but[i] = Instantiate (inventoryButton) as Button;

				but[i].image.rectTransform.sizeDelta = new Vector2 (160, 30);
                //position the button bellow the previous
                but[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(0, (-(i + 1) * 30));
                but[i].transform.localScale = new Vector3 (1,1,1);

				but[i].GetComponentInChildren <Text> ().text = name;

				if (inventory.Items [name] > 1)
					but [i].GetComponentInChildren <Text> ().text += "(" + inventory.Items [name] + ")";

				but[i].transform.SetParent (inventoryButton.transform.parent, false);
				but[i].enabled = true;

				ItemButton itemButton = but [i].gameObject.GetComponent<ItemButton> ();

				itemButton.Player = this.gameObject.GetComponent<Player>();
				itemButton.Name = name;
				itemButton.InventoryType = buttonType;

				i++;
			}

            Content.GetComponent<RectTransform>().sizeDelta = new Vector2 (0, (inventory.Items.Count+1)*30);
			inventoryButton.gameObject.SetActive(false);
		}

        /// <summary>
        /// Show/hide imvemtory
        /// </summary>
		public void Toogle(){
			_invScrollView.SetActive (!_invScrollView.activeSelf);
			_secondInvScrollView.SetActive (!_invScrollView.activeSelf);
            Cursor.visible = _invScrollView.activeSelf;
            if (_invScrollView.activeSelf){
                this.DrawInventoryItem(_invScrollView, _inventoryButton, _player.Inventory, ref _but);
                _inventoryWeight.text = "Weight: " + _player.Inventory.Weight + "/" + _player.Inventory.MaxWeight;
            }
            else
                Time.timeScale = 1.0f;
        }

        public bool IsActive(){
            return _invScrollView.activeSelf;
        }

        void DestroyButtons(Button[] but){
			if (but != null) {
				for (int i = 0; i < but.Length; i++) {
					Destroy (but [i].gameObject);
				}
			}
		}
	}
}