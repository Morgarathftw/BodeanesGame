using UnityEngine;
using System.Collections;

public class MouseBehaviour : MonoBehaviour {

    private Vector3 target;

    // Use this for initialization
    void Start () {
        target = transform.position;
    }
	
	// Update is called once per frame
	void Update () {
        target = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        float _fx = Mathf.Floor(target.x + 0.5f);
        float _fy = Mathf.Floor(target.y + 0.5f);

        Debug.Log(_fx + " " + _fy);
    }
}
