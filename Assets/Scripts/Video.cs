using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Video : MonoBehaviour {
    VideoPlayer video;
    public List<GameObject> active;

    // Start is called before the first frame update
    void Start() {
        video = GetComponent<VideoPlayer>();
    }

    // Update is called once per frame
    void Update() {
        if (video.isPlaying) {
            if (video.frame >= (long)video.frameCount - 1) {
                foreach (GameObject i in active) {
                    i.SetActive(true);
                }
                Destroy(this.gameObject);
            }
        }
    }
}
