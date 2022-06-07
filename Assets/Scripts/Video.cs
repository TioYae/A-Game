using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class Video : MonoBehaviour {
    VideoPlayer video;
    public Button skip;
    public List<GameObject> active;

    // Start is called before the first frame update
    void Start() {
        video = GetComponent<VideoPlayer>();
    }

    // Update is called once per frame
    void Update() {
        if (video.isPlaying) {
            if (video.frame >= (long)video.frameCount - 1) {
                VideoIsOver();
            }
        }
        if (!skip.IsActive() && video.time >= 3f) skip.gameObject.SetActive(true);
    }

    // ������Ƶ
    public void SkipVideo() {
        VideoIsOver();
    }

    // ��Ƶ��֮��Ļص�����
    void VideoIsOver() {
        if (active == null) return;
        foreach (GameObject i in active) {
            i.SetActive(true);
        }
        Destroy(skip.gameObject);
        Destroy(this.gameObject);
    }
}
