using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChooseLevel : MonoBehaviour {
    public List<GameObject> pages;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void Back() {
        SceneManager.LoadScene(0);
    }

    public void ChapterLoadSence(int scene) {
        SceneManager.LoadScene(scene);
    }

    public void PageUp() {

    }

    public void PageDown() {

    }
}
