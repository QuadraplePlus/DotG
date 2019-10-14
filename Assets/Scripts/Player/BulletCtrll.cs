using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrll : MonoBehaviour
{
    //총알의 파괴력
    public float damage = 20.0f;
    public float speed = 1000f;


    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody>().AddForce(transform.forward * speed);
    }
}
