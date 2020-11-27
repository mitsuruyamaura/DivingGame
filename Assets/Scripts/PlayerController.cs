using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;

    public float moveSpeed;

    public bool inWater;

    private int score;

    float x;
    float z;

    [SerializeField]
    private GameObject waterEffectPrefab = null;

    void Start()
    {
        transform.eulerAngles = new Vector3(180, 0, 0);
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");
        Debug.Log(x);
        Debug.Log(z);

        Vector3 moveDir = new Vector3(x, 0, z).normalized;

        rb.velocity = new Vector3(moveDir.x * moveSpeed, rb.velocity.y, moveDir.z * moveSpeed);
    }

    private void OnTriggerEnter(Collider col) {
        if (col.gameObject.tag == "Water" && inWater == false) {
            // エフェクト
            GameObject effect = Instantiate(waterEffectPrefab, transform.position, Quaternion.identity);
            effect.transform.position = new Vector3(effect.transform.position.x, effect.transform.position.y, effect.transform.position.z - 0.5f);


            Destroy(effect, 2.0f);
            //rb.angularDrag = waterDrag;
            //transform.eulerAngles = new Vector3(-30, 180, 0);
            //rb.isKinematic = true;
            inWater = true;

            StartCoroutine(OutOfWater());
            //transform.DORotate(Vector3.zero, 1.0f);
        }

        if (col.gameObject.tag == "Flower") {
            Debug.Log("ステージクリア");

            Destroy(col.gameObject, 0.5f);

            // エフェクト
        }

        if (col.gameObject.tag == "FlowerCircle") {
            score += col.transform.parent.GetComponent<FlowerCircleRotater>().point;
        }
    }

    private IEnumerator OutOfWater()
    {
        yield return new WaitForSeconds(1.0f);

        rb.isKinematic = true;

        transform.eulerAngles = new Vector3(-30, 180, 0);

        transform.DOMoveY(0, 1.0f);
    }
}
