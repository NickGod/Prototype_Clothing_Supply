using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class module : MonoBehaviour {
    public enum ModuleState {
        Inventory,
        Real
    };
    public enum ModuleClass {
        Design,
        Mesh,
        Anim,
        Cube,
        Model,
        Human,
        AnimHuman
    };
    public enum ModuleType {
        House,
        Human
    };

    Transform[] spot = new Transform[6];

    public ModuleState _myState;
    public ModuleClass _myClass;
    public ModuleType _myType;

    Vector3 originSize = Vector3.one * 8;
    
    // Use this for initialization
	void Start () {
        _myState = ModuleState.Inventory;
        
        spot[0] = GameObject.Find("slotIn1_1").transform;
        spot[1] = GameObject.Find("slotIn1_2").transform;
        spot[2] = GameObject.Find("slotIn2_1").transform;
        spot[3] = GameObject.Find("slotIn2_2").transform;
        spot[4] = GameObject.Find("slotIn3_1").transform;
        spot[5] = GameObject.Find("slotIn3_2").transform;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public Transform OnGrab() {
        // clone this game object so you can actually grab
        if (_myState == ModuleState.Inventory) {
            Transform me_clone = Instantiate(gameObject).transform;
            me_clone.parent = null;
            me_clone.position = transform.position;
            me_clone.rotation = transform.rotation;
            Transform _parent = transform;
            //multiply parents' scale
            while (_parent.parent != null) {
                me_clone.localScale *= _parent.parent.localScale.x;
                _parent = _parent.parent;
            }
            //initialize attribute value
            me_clone.GetComponent<module>()._myType = _myType;
            me_clone.GetComponent<module>()._myClass = _myClass;
            me_clone.GetComponent<module>()._myState = module.ModuleState.Real;
            return me_clone;
        } else {
            return transform;
        }
    }

    public void OnRelease(hand _playerHand) {
        transform.localScale = originSize;
        transform.rotation = Quaternion.identity;
        Transform target = GetClostestSpot();
        transform.position = target.position;
        transform.parent = target;
    }

    Transform GetClostestSpot() {
        float distance = float.MaxValue;
        Transform clostest = null;
        for (int i = 0; i < 6; i++) {
            if (Vector3.Distance(transform.position, spot[i].position) < distance) {
                distance = Vector3.Distance(transform.position, spot[i].position);
                clostest = spot[i];
            }
        }
        return clostest;
    }
    

    public void move_towards(Vector3 taret_pos) {
        transform.position += 0.05f * (taret_pos - transform.position);
    }
    
}
