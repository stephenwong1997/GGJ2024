using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleController : MonoBehaviour
{
    [SerializeField] float movementSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] Vector2 groundCheckOffset;
    [SerializeField] Vector2 detectionSize;

    [Header("Ref")]
    [SerializeField] Rigidbody2D rigid;
    [SerializeField] Collider2D col;
    [SerializeField] LayerMask groundLayer;

    void Update()
    {
        float xInput = Input.GetAxis("Horizontal");
        Vector2 velocity = rigid.velocity;
        velocity.x = xInput * movementSpeed;
        rigid.velocity = velocity;

        print(IsGrounded());

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            rigid.AddForce(Vector2.up * jumpForce);
        }
    }
    private bool IsGrounded()
    {
        Vector2 position = transform.position;
        position += groundCheckOffset; // 計算檢測起點

        // 使用 BoxCast 進行地面檢測
        detectionSize = col.bounds.size;
        detectionSize.y = .05f;

        RaycastHit2D hit = Physics2D.BoxCast(position, detectionSize, 0f, Vector2.down, 0.05f, groundLayer);
        return hit.collider != null; // 如果碰到某物則視為在地面上
    }
    void OnDrawGizmos() // 新增 - 在 Unity 編輯器中繪製檢測範圍，幫助調試
    {
        Gizmos.color = Color.red;
        Vector2 position = transform.position;
        position += groundCheckOffset;
        Gizmos.DrawWireCube(position, detectionSize);
    }

}
