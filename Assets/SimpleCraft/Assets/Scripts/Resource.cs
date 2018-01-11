using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleCraft{
	/// <summary>
	/// Can be gathered using a Tool adding Resources to the Player
	/// </summary>
	public class Resource : MonoBehaviour{
		public enum Type{
			wood,
			stone,
			iron,
			gold,
			plank
		}

		[SerializeField] private Type _resType;
		public Type ResType {
			get { return _resType; }
			set { _resType = value; }
		}

		[SerializeField] private float _amount;
		public float Amount {
			get { return _amount; }
			set { _amount = value; }
		}

		[SerializeField] private bool _isPickable;
		public bool IsPickable {
			get { return _isPickable; }
			set { _isPickable = value; }
		}

		/// <summary>
		/// Called when the player uses a tool on this resource
		/// </summary>
		/// <param name="AmountTaken">Amount taken.</param>
		public float Gather(float AmountTaken){
			if (_amount < AmountTaken)
				AmountTaken = _amount;
			
			_amount -= AmountTaken;

			if (_amount <= 0)
				Destroy (this.gameObject);

			return AmountTaken;
		}
	}
}