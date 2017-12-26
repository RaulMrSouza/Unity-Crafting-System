using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SimpleCraft{
	/// <summary>
	/// A Tool is a CraftableItem that can be used to gather Resource
	/// having the possibility of being better at some Resources than others.
	/// </summary>
	public class Tool : CraftableItem{

		[Tooltip("How much resource the tool will take each time it is used")] 
		[SerializeField] private float _gatherPower = 5;
		public float GatherPower {
			get { return _gatherPower; }
			set { _gatherPower = value; }
		}


		[Serializable]
		private struct GatherFactorSet{
			public Resource.Type resource;
			public float amount;
		}
		[Tooltip("This multiplies the gather power on a specific resource")]
		[SerializeField] private GatherFactorSet[] _gatherFactorSet;

		[SerializeField] private Dictionary<Resource.Type, float> _gatherFactor = 
			new Dictionary<Resource.Type, float> ();

		public float GatherFactor (Resource.Type type){
			if (_gatherFactor.ContainsKey (type))
				return _gatherFactor [type];
			else
				return -1;
		}

		void Start (){
			foreach (GatherFactorSet  gf in _gatherFactorSet)
				_gatherFactor.Add (gf.resource, gf.amount);

			_gatherFactorSet = null;
			GC.Collect ();
		}
	}
}