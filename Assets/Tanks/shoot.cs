using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shoot : MonoBehaviour
{
    ParticleSystem ps;

    void Start() //スタート
    {
        ps = GetComponent<ParticleSystem>();
    }

    void FixedUpdate() // 常時実行
    {
        // 右クリック
        if (Input.GetMouseButton(1) || Input.GetKey(KeyCode.F))
        {
            ps.Emit(new ParticleSystem.EmitParams(), 1);
        }
    }
}
