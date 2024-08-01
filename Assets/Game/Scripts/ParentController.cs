using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentController : MonoBehaviour
{
    public float speed = 6.0f;
    public float jumpForce = 8.0f;
    public float gravity = 20.0f;
    public float rotationSpeed = 10.0f; // 回転速度

    private Vector3 moveDirection = Vector3.zero;
    private CharacterController controller;
    private Animator animator;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (controller == null)
        {
            Debug.LogError("CharacterControllerが見つかりません");
            return;
        }

        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animatorが見つかりません");
            return;
        }
    }

    void Update()
    {
        if (controller == null || animator == null)
        {
            return; // もしCharacterControllerまたはAnimatorが見つからない場合、Update処理をしない
        }

        // 移動入力の取得
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        bool isDancing = Input.GetKey(KeyCode.F); // Fキーでダンス

        if (controller.isGrounded)
        {
            moveDirection = new Vector3(moveHorizontal, 0.0f, moveVertical);

            if (moveDirection != Vector3.zero && !isDancing)
            {
                // キャラクターの向きを移動方向に合わせる
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

                moveDirection *= speed;

                // アニメーションの切り替え
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

            // ジャンプ
            if (Input.GetButton("Jump"))
            {
                moveDirection.y = jumpForce;
                animator.SetBool("isJumping", true);
            }
        }

        // 重力の適用
        moveDirection.y -= gravity * Time.deltaTime;

        // 移動の実行
        controller.Move(moveDirection * Time.deltaTime);
    }
}
