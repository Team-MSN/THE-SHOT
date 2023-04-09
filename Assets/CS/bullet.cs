using System.Collections;
using UnityEngine;
using UnityEditor;

public class bullet : MonoBehaviour
{
    void Go() //前進
    {
        rb.velocity = transform.forward * speed;
    }

    private float bounciness = 1.0f;

    public float speed = 5.0f;

    private bool useContactOffset = true;

    private Rigidbody rb;
    private SphereCollider sphereCollider;
    private float defaultContactOffset;
    private const float sphereCastMargin = 0.01f;
    private Vector3? reboundVelocity;
    private Vector3 lastDirection;
    private bool canKeepSpeed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        sphereCollider = GetComponent<SphereCollider>();
    }

    private void Start()
    {
        Init();
        StartCoroutine(StartWaitForFixedUpdate());
        Go();
    }

    private void FixedUpdate()
    {
        // 重なりを解消
        OverlapDetection();

        // 前フレームで反射していたら反射後速度を反映
        ApplyReboundVelocity();

        // 進行方向に衝突対象があるかどうか確認
        ProcessForwardDetection();

        // 1フレーム前の進行方向を保存
        UpdateLastDirection();
    }


    private void Init()
    {
        // isTrigger=false で使用する場合はContinuous Dynamicsに設定
        if (
            !sphereCollider.isTrigger
            && rb.collisionDetectionMode != CollisionDetectionMode.ContinuousDynamic
        )
        {
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        }

        // 重力の使用は禁止
        if (rb.useGravity)
        {
            rb.useGravity = false;
        }

        defaultContactOffset = Physics.defaultContactOffset;
        canKeepSpeed = true;
    }

    private IEnumerator StartWaitForFixedUpdate()
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();

            WaitForFixedUpdate();
        }
    }

    private void WaitForFixedUpdate()
    {
        KeepConstantSpeed();
    }

    /// 速度を一定に保つ
    /// 衝突や引っかかりによる減速を上書きする役目
    private void KeepConstantSpeed()
    {
        if (!canKeepSpeed)
            return;

        var velocity = rb.velocity;
        var nowSqrSpeed = velocity.sqrMagnitude;
        var sqrSpeed = speed * speed;

        if (!Mathf.Approximately(nowSqrSpeed, sqrSpeed))
        {
            var dir = velocity != Vector3.zero ? velocity.normalized : lastDirection;
            rb.velocity = dir * speed;
        }
    }

    /// オブジェクトとの重なりを検知して解消するように位置補正する
    /// 主にTransform.positionで移動してきた外部オブジェクトを回避するのに使う
    private void OverlapDetection()
    {
        // Overlap
        var colliders = Physics.OverlapSphere(
            sphereCollider.transform.position,
            sphereCollider.radius
        );
        var isOverlap = 1 < colliders.Length;
        if (isOverlap)
        {
            var pushVec = Vector3.zero;
            var pushDistance = 0f;
            var totalPushPos = Vector3.zero;
            var pushCount = 0;

            foreach (var targetCollider in colliders)
            {
                // 自身のコライダーなら無視する
                if (targetCollider == sphereCollider)
                    continue;

                var isCollision = Physics.ComputePenetration(
                    sphereCollider,
                    sphereCollider.transform.position,
                    sphereCollider.transform.rotation,
                    targetCollider,
                    targetCollider.transform.position,
                    targetCollider.transform.rotation,
                    out pushVec,
                    out pushDistance
                );

                if (isCollision && pushDistance != 0)
                {
                    totalPushPos += pushDistance * pushVec;
                    pushCount++;
                }
            }

            if (pushCount != 0)
            {
                var pos = transform.position;
                pos += totalPushPos / pushCount;
                transform.position = pos;
            }
        }
    }

    /// 計算した反射ベクトルを反映する
    private void ApplyReboundVelocity()
    {
        if (reboundVelocity == null)
            return;

        rb.velocity = reboundVelocity.Value;
        speed *= bounciness;
        reboundVelocity = null;
        canKeepSpeed = true;
    }

    /// 前方方向を監視して1フレーム後に衝突している場合は反射ベクトルを計算する
    private void ProcessForwardDetection()
    {
        var velocity = rb.velocity;
        var direction = velocity.normalized;

        var offset = useContactOffset ? defaultContactOffset * 2 : 0;
        var origin = transform.position - direction * (sphereCastMargin + offset);
        var colliderRadius = sphereCollider.radius + offset;
        var isHit = Physics.SphereCast(origin, colliderRadius, direction, out var hitInfo);
        if (isHit)
        {
            var distance = hitInfo.distance - sphereCastMargin;
            var nextMoveDistance = velocity.magnitude * Time.fixedDeltaTime;
            if (distance <= nextMoveDistance)
            {
                // 次フレームに使う反射速度を計算
                var normal = hitInfo.normal;
                var inVecDir = direction;
                var outVecDir = Vector3.Reflect(inVecDir, normal);
                reboundVelocity = outVecDir * speed * bounciness;

                // 衝突予定先に接するように速度を調整
                var adjustVelocity = distance / Time.fixedDeltaTime * direction;
                rb.velocity = adjustVelocity;
                canKeepSpeed = false;
            }
        }
    }

    private void UpdateLastDirection()
    {
        var velocity = rb.velocity;
        if (velocity != Vector3.zero)
        {
            lastDirection = velocity.normalized;
        }
    }
}
