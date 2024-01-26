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
    [SerializeField] CharacterDisplayController displayController;

    private void Start()
    {
        // �ϥ� BoxCast �i��a���˴�
        detectionSize = col.bounds.size;
        groundCheckOffset.y = -col.bounds.size.y / 2;
        detectionSize.y = .05f;
        displayController = GetComponent<CharacterDisplayController>();
    }

    void Update()
    {
        float xInput = Input.GetAxis("Horizontal");
        Vector2 velocity = rigid.velocity;
        velocity.x = xInput * movementSpeed;
        rigid.velocity = velocity;

        bool isGrounded = IsGrounded();

        if (displayController != null)
        {
            displayController.SetBool("IsGrounded", isGrounded);
            displayController.SetBool("IsRunning", xInput != 0);
            if (xInput != 0)
                displayController.flipX = xInput < 0;
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rigid.AddForce(Vector2.up * jumpForce);
        }
    }
    private bool IsGrounded()
    {
        Vector2 position = transform.position;
        position += groundCheckOffset; // �p���˴��_�I

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
