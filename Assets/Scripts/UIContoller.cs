using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIContoller : MonoBehaviour {
 
    static int ui_index;

    public float distance_thread;

    /*please map the index of texture to the positions set above */
    public Texture[] indexed_texture;
    public Color[] indexed_color;

    public Transform right_hand;

    Transform UI_particle;

    static Vector3[] anchors = new Vector3[6] {
        new Vector3(0.557f, 1.32f, -0.641f),
        new Vector3(0.218f, 1.32f, -0.641f),
        new Vector3(-0.177f, 1.35f, 0.165f),
        new Vector3(-0.177f, 1.35f, -0.175f),
        new Vector3(0.218f, 1.32f, 0.631f),
        new Vector3(0.533f, 1.32f, 0.631f)
    };
  

    void Start () {
        UI_particle = transform.GetChild(0);   
	}
	
	
	void Update () {
        int i = triggered_ui();
        if (i >= 0){
            UI_particle.gameObject.SetActive(true);
            activate(i);
        }
        else {
            UI_particle.gameObject.SetActive(false);
        }
	}

    int triggered_ui() {
        int idx = -1;
        int cnt;
        float min_dis=distance_thread;
        for (cnt = 0; cnt < anchors.Length; cnt++) {
            float d = Vector3.Distance(anchors[cnt], right_hand.transform.position);
            if (d - min_dis < 1e-5) {
                idx = cnt;
                min_dis = d;
            }
        }
        return idx;
    }

    bool activate(int index) {
        if (index >= indexed_texture.Length) {
            Debug.LogWarning("Texture is not correctly assigned.");
            return false;
        }
        UI_particle.GetComponent<Renderer>().material.mainTexture = indexed_texture[index];
        ParticleSystem par = UI_particle.GetComponent<ParticleSystem>();
        var ma = par.main;
        ma.startColor = index< indexed_color.Length? indexed_color[index]:Color.white;
        ParticleSystem.Particle[] pars = new ParticleSystem.Particle[2];
        int i = par.GetParticles(pars);
        pars[0].startColor = index < indexed_color.Length ? indexed_color[index] : Color.white;
        par.SetParticles(pars,1);
        UI_particle.position = anchors[index];
        return true;
    }
}
