using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleCraft.Core;

namespace SimpleCraft.UI{
    /// <summary>
    /// Handles the Crafying UI, showing
    /// the required crafting items and types
    /// Author: Raul Souza
    /// </summary>
    public class CraftUI : MonoBehaviour {
        [SerializeField]
        private GameObject _costScrollView;

        [SerializeField]
        private Text _costText;

        [SerializeField]
        private Text _typeText;

        [SerializeField]
        private Text _typeTextNext;

        [SerializeField]
        private Text _typeTextPrevious;

        [SerializeField]
        private Text _DescriptionText;

        private RectTransform _content;

        /// <summary>
        /// Shows the item's resource cost in a scrollView
        /// </summary>
        /// <param name="craftableItem">Building.</param>
        public void DrawCostView(CraftableItem craftableItem,Inventory inventory){
            _content = _costScrollView.GetComponent<ScrollRect>().content;

            _costText.text = craftableItem.ItemName;
            _costText.text += "\nCost (required/inventory)";

            _DescriptionText.text = craftableItem.ItemName;
            if (craftableItem.Description != "")
                _DescriptionText.text += " - " + craftableItem.Description;

            foreach (CraftableItem.CraftCost buildingCost in craftableItem.GetCraftCost){
                _costText.text += "\n" + buildingCost.item.name;
                string itemName = buildingCost.item.GetComponent<Item>().ItemName;
                if (!inventory.HasItem(itemName))
                    _costText.text += " <color=#ff0000ff> (" + buildingCost.amount + "/0)</color>";
                else{
                    if(buildingCost.amount > inventory.Items(itemName))
                        _costText.text += "<color=#ff0000ff> (" + buildingCost.amount + "/" + inventory.Items(itemName) + ")</color>";
                    else
                        _costText.text += " (" + buildingCost.amount + "/" + inventory.Items(itemName) + ")";
                }

            }
            _content.GetComponent<RectTransform>().sizeDelta = new Vector2(0, (craftableItem.GetCraftCost.Count + 2) * 30);
            _costText.GetComponent<RectTransform>().sizeDelta = new Vector2(160, (craftableItem.GetCraftCost.Count + 2) * 30);
        }
        
        public void setTypeText(int _craftTypeIdx){

            _typeText.text = Manager.GetCraftableType(_craftTypeIdx);

            if (Manager.GetCraftableTypeLength() > 1){
                _typeTextNext.text = "Press (R) for ";

                if (_craftTypeIdx < Manager.GetCraftableTypeLength() - 1)
                    _typeTextNext.text += " " + Manager.GetCraftableType(_craftTypeIdx + 1);
                else
                    _typeTextNext.text += " " + Manager.GetCraftableType(0);
                _typeTextNext.text += " >";

                if (_craftTypeIdx > 0)
                    _typeTextPrevious.text = "Press (Q) for "
                        + Manager.GetCraftableType(_craftTypeIdx - 1);
                else
                    _typeTextPrevious.text = "Press (Q) for "
                        + Manager.GetCraftableType(Manager.GetCraftableTypeLength() - 1);
                _typeTextPrevious.text = "< " + _typeTextPrevious.text;
            }
        }
    }
}