using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class floatingText : MonoBehaviour
{
    private TextMeshPro tmp;
    private Color tmpColor;
    private float keepAlive = 1f;
    [SerializeField] float ySpeed = 5f;
    [SerializeField] float fadeSpeed = 3f;

    private void Awake() {
        tmp = transform.GetComponentInChildren<TextMeshPro>();
    }

    private void Update() {
        transform.position += new Vector3(0, ySpeed) * Time.deltaTime;

        keepAlive -= Time.deltaTime;
        if(keepAlive < 0) {
            tmpColor.a -= fadeSpeed * Time.deltaTime;
            tmp.color = tmpColor;
            if(tmpColor.a <= 0) Destroy(gameObject);
        }
    }

    public void Initiate(int dmgAmt) {
        tmp.SetText(dmgAmt.ToString());
        tmpColor = tmp.color;
    }


}
