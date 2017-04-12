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

    Transform[] spot = new Transform[7];

    public ModuleState _myState;
    public ModuleClass _myClass;
    public ModuleType _myType;

    Vector3 originSize = Vector3.one * 8;
    
    // Use this for initialization
	void Start () {
        
        spot[0] = GameObject.Find("slotIn1_1").transform;
        spot[1] = GameObject.Find("slotIn1_2").transform;
        spot[2] = GameObject.Find("slotIn2_1").transform;
        spot[3] = GameObject.Find("slotIn2_2").transform;
        spot[4] = GameObject.Find("slotIn3_1").transform;
        spot[5] = GameObject.Find("slotIn3_2").transform;
        spot[6] = GameObject.Find("Table").transform;

        string[] parts = transform.parent.name.Split('_');
        switch (parts[0]) {
            case "House":
                _myType = ModuleType.House;
                break;
            case "Human":
                _myType = ModuleType.Human;
                break;
            default:
                Debug.Log("Impossible");
                break;
        }

        switch (parts[1]) {
            case "Design":
                _myClass = ModuleClass.Design;
                break;
            case "Anim":
                _myClass = ModuleClass.Anim;
                break;
            case "AnimHuman":
                _myClass = ModuleClass.AnimHuman;
                break;
            case "Cube":
                _myClass = ModuleClass.Cube;
                break;
            case "Human":
                _myClass = ModuleClass.Human;
                break;
            case "Mesh":
                _myClass = ModuleClass.Mesh;
                break;
            case "Model":
                _myClass = ModuleClass.Model;
                break;
            default:
                Debug.Log("Impossible");
                break;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public Transform OnGrab() {
        // clone this game object so you can actually grab
        if (_myState == ModuleState.Inventory) {
            Transform me_clone = Instantiate(transform.parent.gameObject).transform;
            me_clone.parent = null;
            me_clone.position = transform.parent.position;
            me_clone.rotation = transform.parent.rotation;
            Transform _parent = transform.parent;
            //multiply parents' scale
            while (_parent.parent != null) {
                me_clone.localScale *= _parent.parent.localScale.x;
                _parent = _parent.parent;
            }
            //initialize attribute value
            me_clone.GetChild(0).GetComponent<module>()._myType = _myType;
            me_clone.GetChild(0).GetComponent<module>()._myClass = _myClass;
            me_clone.GetChild(0).GetComponent<module>()._myState = module.ModuleState.Real;
            return me_clone.GetChild(0);
        } else {
            return transform;
        }
    }

    public void OnRelease(hand _playerHand) {
        transform.parent.localScale = originSize;
        transform.parent.rotation = Quaternion.identity;
        if (_myClass == ModuleClass.AnimHuman) {
            if (WithinRange(spot[6], spot[6].lossyScale.x, spot[6].lossyScale.z, 0.0f)) {
                Vector3 pos = transform.parent.position;
                pos.y = spot[6].position.y + spot[6].lossyScale.y / 2;
                transform.parent.position = pos;
                // start jumping animation
                transform.parent.gameObject.GetComponent<Jump>().StartJumpAnimation();
                return;
            }
        }
        Transform target = GetClostestSpot();
        if (target.childCount > 0) {
            foreach (Transform child in target) {
                Destroy(child.gameObject);
            }
        }
        transform.parent.position = target.position;
        transform.parent.parent = target;
    }

    bool WithinRange(Transform target, float xLength, float zlength, float offset) {
        float xEdge1 = target.position.x + xLength / 2 - offset;
        float xEdge2 = target.position.x - xLength / 2 + offset;

        float zEdge1 = target.position.z + zlength / 2 - offset;
        float zEdge2 = target.position.z - zlength / 2 + offset;

        if ((transform.position.x - xEdge1) * (transform.position.x - xEdge2) < 0 &&
            (transform.position.z - zEdge1) * (transform.position.z - zEdge2) < 0) {
            return true;
        } else {
            return false;
        }
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
    
    
}
