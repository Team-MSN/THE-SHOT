using UnityEngine;

public class shoot : MonoBehaviour
{
    //ParticleSystem ps;
    public GameObject bullet;

    void Start() //スタート
    {
        //ps = GetComponent<ParticleSystem>();
    }

    void Update() // 常時実行
    {
        // 右クリック
        if (Input.GetMouseButtonDown(1))
        {
            GameObject obj = Instantiate(bullet, transform.position, transform.rotation);
            obj.SetActive(true);
            //ps.Emit(new ParticleSystem.EmitParams(), 1);
        }
    }
}
