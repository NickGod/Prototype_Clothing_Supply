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

    bool _isFinalSpot = false;

    Vector3 originSize = Vector3.one * 8;
    public GameObject[] GenerateOBJs = new GameObject[5];
    //gameobject to generate should be in following order
    //spot1  index 0: human ; index 1: house
    //spot2  index 2: human ; index 3: house
    //spot3  index 4: human

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
            targetObjGroupIndex = 0;
        } else if (_parent.name.EndsWith("2")) {
            preferredClass1 = module.ModuleClass.Mesh;
            preferredClass2 = module.ModuleClass.Model;
            targetClass = module.ModuleClass.Human;
            targetObjGroupIndex = 1;
        } else {
            preferredClass1 = module.ModuleClass.Anim;
            preferredClass2 = module.ModuleClass.Human;
            targetClass = module.ModuleClass.AnimHuman;
            targetObjGroupIndex = 2;
            _isFinalSpot = true;
        }
    }

    void Update() {

        
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
                    Destroy(child.gameObject);
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
                    Destroy(child.gameObject);
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
        cloneOBJ.position = _spotout.position;
        cloneOBJ.parent = _spotout;
        cloneOBJ.rotation = Quaternion.identity;
        cloneOBJ.localScale = originSize;

        //initialize attribute value
        cloneOBJ.GetChild(0).GetComponent<module>()._myType = targetType;
        cloneOBJ.GetChild(0).GetComponent<module>()._myClass = targetClass;
        cloneOBJ.GetChild(0).GetComponent<module>()._myState = module.ModuleState.Real;
    }

    public void PressButton() {
        _myButton.position += _myButton.forward * 0.013f;
        if (Validate()) {
            Generate();
        }
        targetType = 0;
    }

    public void ReleaseButton() {
        _myButton.position -= _myButton.forward * 0.013f;
    }
 }
