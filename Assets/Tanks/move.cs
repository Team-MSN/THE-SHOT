using UnityEngine;

public class move : MonoBehaviour
{
    Rigidbody rb;
    public float speed = 3.0f;

    void Stop() //停止
    {
        rb.angularVelocity = transform.up * 0;
        rb.velocity = transform.forward * 0;
    }

    void rightRotate() //右回転
    {
        rb.angularVelocity = transform.up * speed;
    }

    void leftRotate() //左回転
    {
        rb.angularVelocity = -transform.up * speed;
    }

    void Go() //前進
    {
        rb.velocity = transform.forward * speed;
    }

    void Back() //後退
    {
        rb.velocity = -transform.forward * speed;
    }

    bool Move() //移動判別
    {
        // Wキー
        if (Input.GetKey(KeyCode.W))
        {
            // Sキー
            if (Input.GetKey(KeyCode.S))
            {
                Stop();
                return true;
            }
            Stop();
            Go();
            return true;
        }

        // Sキー
        if (Input.GetKey(KeyCode.S))
        {
            // Wキー
            if (Input.GetKey(KeyCode.W))
            {
                Stop();
                return true;
            }
            Stop();
            Back();
            return true;
        }
        return false;
    }

    void Start() //スタート
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate() // 常時実行
    {
        // Aキー
        if (Input.GetKey(KeyCode.A))
        {
            // Dキー
            if (Input.GetKey(KeyCode.D))
            {
                if (Move())
                    return;
                Stop();
                return;
            }
            Stop();
            leftRotate();
            return;
        }
        // Dキー
        if (Input.GetKey(KeyCode.D))
        {
            // Aキー
            if (Input.GetKey(KeyCode.A))
            {
                Stop();
                return;
            }
            Stop();
            rightRotate();
            return;
        }
        Move();
    }
}
