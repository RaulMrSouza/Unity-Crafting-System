using System.Collections.Generic;
using UnityEngine;

namespace SimpleCraft.Core {
    [CreateAssetMenu(fileName = "Tool", menuName = "Simple Craft/Tool", order = 3)]
    /// <summary>
    /// Tools can be held and used to gather Resources
    /// </summary>
    [System.Serializable]
    public class Tool : CraftableItem{
        [Tooltip("How much resource the tool will take each time it is used")]
        [SerializeField]
        private float _gatherPower = 5;
        public float GatherPower{
            get { return _gatherPower; }
            set { _gatherPower = value; }
        }

        [System.Serializable]
        private struct GatherFactorSet{
            public Item resource;
            public float amount;
        }

        [Tooltip("This multiplies the gather power on a specific resource")]
        [SerializeField]
        private GatherFactorSet[] _gatherFactorSet;

        [SerializeField]
        public Dictionary<Item, float> _gatherFactor =
            new Dictionary<Item, float>();

        [SerializeField]
        private AudioSource _swingSound;
        public AudioSource SwingSound {
            get { return _swingSound; }
            set { _swingSound = value; }
        }

        [SerializeField]
        protected bool _destructionTool = false;
        public bool DestructionTool {
            get { return _destructionTool; }
            set { _destructionTool = value; }
        }

        public void InitializeDictionary(){
            _gatherFactor = new Dictionary<Item, float>();
            if (_gatherFactorSet != null)
                foreach (GatherFactorSet gf in _gatherFactorSet)
                    _gatherFactor.Add(gf.resource, gf.amount);
        }

        public float GatherFactor(Item item){
            if (_gatherFactor.ContainsKey(item))                
                return _gatherFactor[item];
            else
                return -1;
        }

        public Tool() {
            _canBePicked = true;
            _hasRigidBody = true;
        }
    }
}