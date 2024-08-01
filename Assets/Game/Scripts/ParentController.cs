using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentController : MonoBehaviour
{
    public float speed = 6.0f;
    public float jumpForce = 8.0f;
    public float gravity = 20.0f;
    public float rotationSpeed = 10.0f; // ��]���x

    private Vector3 moveDirection = Vector3.zero;
    private CharacterController controller;
    private Animator animator;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (controller == null)
        {
            Debug.LogError("CharacterController��������܂���");
            return;
        }

        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator��������܂���");
            return;
        }
    }

    void Update()
    {
        if (controller == null || animator == null)
        {
            return; // ����CharacterController�܂���Animator��������Ȃ��ꍇ�AUpdate���������Ȃ�
        }

        // �ړ����͂̎擾
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        bool isDancing = Input.GetKey(KeyCode.F); // F�L�[�Ń_���X

        if (controller.isGrounded)
        {
            moveDirection = new Vector3(moveHorizontal, 0.0f, moveVertical);

            if (moveDirection != Vector3.zero && !isDancing)
            {
                // �L�����N�^�[�̌������ړ������ɍ��킹��
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

                moveDirection *= speed;

                // �A�j���[�V�����̐؂�ւ�
                animator.SetFloat("speed", moveDirection.magnitude);
                animator.SetBool("isJumping", false);
            }
            else if (isDancing)
            {
                animator.SetFloat("speed", 0);
                animator.SetBool("isJumping", false);
                animator.SetTrigger("dance");
            }
            else
            {
                animator.SetFloat("speed", 0);
            }

            // �W�����v
            if (Input.GetButton("Jump"))
            {
                moveDirection.y = jumpForce;
                animator.SetBool("isJumping", true);
            }
        }

        // �d�͂̓K�p
        moveDirection.y -= gravity * Time.deltaTime;

        // �ړ��̎��s
        controller.Move(moveDirection * Time.deltaTime);
    }
}
