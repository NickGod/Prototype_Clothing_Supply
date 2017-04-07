using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class button : MonoBehaviour {
    
    //[SerializeField] private Text m_CurrentSuccessRate;
    //[SerializeField] private Text m_SingleTestSuccessRate;
    //[SerializeField] private AudioClip m_SuccessSound;
    //[SerializeField] private AudioClip m_FailureSound;
    //[SerializeField] private AudioSource m_Audio;
    
    Transform _myButton;
    Transform _spot1;
    Transform _spot2;
    Transform _spotout;

    private module.ModuleClass preferredClass1;
    private module.ModuleClass preferredClass2;
    private module.ModuleClass targetClass;
    private module.ModuleType targetType;

    public Transform _robot;
    bool _isFinalSpot = false;

    bool _isWorking = false;
    Vector3 _originPos;
    Vector3 _factory;
    Vector3 _movingTarget;

    Vector3 originSize = Vector3.one * 8;
    public GameObject[] GenerateOBJs = new GameObject[5];
    //gameobject to generate should be in following order
    //spot1  index 0: human ; index 1: house
    //spot2  index 2: human ; index 3: house
    //spot3  index 4: human


    Transform child1 = null;
    Transform child2 = null;

    int targetObjGroupIndex;
    GameObject targetObj;
    void Start() {
        Transform _parent = transform.parent;
        foreach(Transform child in _parent) {
            if (child.name.Equals("Button")) {
                _myButton = child;
            } else if (child.name.Contains("Out")){
                _spotout = child;
            } else if (child.name.Contains("In")){
                if (child.name.EndsWith("_1")) {
                    _spot1 = child;
                } else {
                    _spot2 = child;
                }
            }
        }
        if (_parent.name.EndsWith("1")) {
            preferredClass1 = module.ModuleClass.Design;
            preferredClass2 = module.ModuleClass.Cube;
            targetClass = module.ModuleClass.Model;
            _robot = GameObject.Find("alien_engi1a (1)").transform;
            targetObjGroupIndex = 0;
        } else if (_parent.name.EndsWith("2")) {
            preferredClass1 = module.ModuleClass.Mesh;
            preferredClass2 = module.ModuleClass.Model;
            targetClass = module.ModuleClass.Human;
            _robot = GameObject.Find("alien_engi1a (2)").transform;
            targetObjGroupIndex = 1;
        } else {
            preferredClass1 = module.ModuleClass.Anim;
            preferredClass2 = module.ModuleClass.Human;
            targetClass = module.ModuleClass.AnimHuman;
            _robot = GameObject.Find("alien_engi1a (3)").transform;
            targetObjGroupIndex = 2;
            _isFinalSpot = true;
        }
        _originPos = _robot.position;
        _factory = _robot.position - _robot.up * 2.7f;
    }

    void Update() {
        if (_isWorking) {
            move_towards(_movingTarget);
            if (isClosed(_movingTarget)) {
                if (_movingTarget == _originPos) {
                    _isWorking = false;
                    _robot.GetChild(2).GetChild(0).position = _spotout.position;
                    _robot.GetChild(2).GetChild(0).parent = _spotout;
                } else {
                    _movingTarget = _originPos;
                    _robot.Rotate(0, 0, 180f);
                    Destroy(child1.gameObject);
                    Destroy(child2.gameObject);
                    child1 = null;
                    child2 = null;
                    Generate();
                    Debug.Log("Check this counter");
                }

            }
        }
    }

    bool Validate() {
        int validNum = 0;
        module.ModuleClass class1 = 0;
        module.ModuleClass class2 = 0;
        module.ModuleType type1 = 0;
        module.ModuleType type2 = 0;
        Debug.Log("Spot1: " + _spot1.name);
        Debug.Log("Spot2: " + _spot2.name);
        foreach (Transform child in _spot1) {
            if (!child.name.Equals("Cube")) {
                if (child.childCount == 0) {
                    continue;
                } else {
                    class1 = child.GetChild(0).GetComponent<module>()._myClass;
                    type1 = child.GetChild(0).GetComponent<module>()._myType;
                    validNum++;
                    child1 = child;
                }
            }
        }
        foreach (Transform child in _spot2) {

            if (!child.name.Equals("Cube")) {
                if (child.childCount == 0) {
                    continue;
                } else {
                    class2 = child.GetChild(0).GetComponent<module>()._myClass;
                    type2 = child.GetChild(0).GetComponent<module>()._myType;
                    validNum++;
                    child2 = child;
                }
            }
        }
        foreach (Transform child in _spotout) {
            if (!child.name.Equals("Cube")) {
                Destroy(child.gameObject);
            }
        }

        if (validNum == 2) {
            if (class1 == preferredClass1 && class2 == preferredClass2 ||
                class1 == preferredClass2 && class2 == preferredClass1) {
                if (type1 == type2) {
                    if (_isFinalSpot) {
                        if (type1 == module.ModuleType.Human) {
                            targetType = type1;
                            targetObj = GenerateOBJs[targetObjGroupIndex * 2 + 0];
                            child1.position = _robot.GetChild(0).position;
                            child2.position = _robot.GetChild(1).position;
                            child1.parent = _robot.GetChild(0);
                            child2.parent = _robot.GetChild(1);
                            return true;
                        } else {
                            Debug.Log("Err: wrong type for spot 3");
                        }
                    } else {
                        targetType = type1;
                        if (type1 == module.ModuleType.Human) {
                            targetObj = GenerateOBJs[targetObjGroupIndex * 2 + 0];
                        } else {
                            targetObj = GenerateOBJs[targetObjGroupIndex * 2 + 1];
                        }

                        child1.position = _robot.GetChild(0).position;
                        child2.position = _robot.GetChild(1).position;
                        child1.parent = _robot.GetChild(0);
                        child2.parent = _robot.GetChild(1);
                        return true;
                    }
                } else {
                    Debug.Log("Err: right class but wrong type");
                }
            } else {
                Debug.Log("Err: enough elements but wrong class");
            }
        } else {
            Debug.Log("Err: not enough elements");
        }
        return false;
    }

    void Generate() {
        
        Transform cloneOBJ = Instantiate(targetObj).transform;
        cloneOBJ.position = _robot.GetChild(2).position;
        cloneOBJ.parent = _robot.GetChild(2);
        cloneOBJ.rotation = Quaternion.identity;
        cloneOBJ.localScale = originSize;

        //initialize attribute value
        cloneOBJ.GetChild(0).GetComponent<module>()._myType = targetType;
        cloneOBJ.GetChild(0).GetComponent<module>()._myClass = targetClass;
        cloneOBJ.GetChild(0).GetComponent<module>()._myState = module.ModuleState.Real;
    }

    public void PressButton() {
        _myButton.position += _myButton.forward * 0.013f;
        if (!_isWorking) {
            if (Validate()) {
                _isWorking = true;
                _movingTarget = _factory;
                _robot.Rotate(0, 0, 180f);
            }
            targetType = 0;
        }
    }

    public void ReleaseButton() {
        _myButton.position -= _myButton.forward * 0.013f;
    }

    public bool isClosed(Vector3 target) {
        if (Vector3.Distance(target, _robot.position) < 0.05f) {
            return true;
        } else {
            return false;
        }
    }

    public void move_towards(Vector3 taret_pos) {
        _robot.position += (taret_pos - _robot.position) * Time.deltaTime;
    }
}
