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
        position += groundCheckOffset; // �p���˴��_�I

        // �ϥ� BoxCast �i��a���˴�
        detectionSize = col.bounds.size;
        detectionSize.y = .05f;

        RaycastHit2D hit = Physics2D.BoxCast(position, detectionSize, 0f, Vector2.down, 0.05f, groundLayer);
        return hit.collider != null; // �p�G�I��Y���h�����b�a���W
    }
    void OnDrawGizmos() // �s�W - �b Unity �s�边��ø�s�˴��d��A���U�ո�
    {
        Gizmos.color = Color.red;
        Vector2 position = transform.position;
        position += groundCheckOffset;
        Gizmos.DrawWireCube(position, detectionSize);
    }

}
