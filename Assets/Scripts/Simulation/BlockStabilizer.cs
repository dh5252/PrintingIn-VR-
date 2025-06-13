using UnityEngine;

public class BlockStabilizer : MonoBehaviour
{
    Rigidbody _rb;
    void Awake()
    {
        // Rigidbody 캐싱
        _rb = GetComponent<Rigidbody>();
        // 중력은 켜두고, Kinematic은 꺼두세요
        _rb.useGravity = true;
        _rb.isKinematic = false;
    }
    void OnCollisionEnter(Collision col)
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;
    }
    
    void OnEnable()
    {
        // 스폰되자마자 한 번 속도 세팅
        _rb.linearVelocity = Vector3.down * 10f;
    }
}