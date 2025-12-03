using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class PlayerMovement : MonoBehaviour
{
    
    public KeyUIManager keyUIManager;

    Animator m_Animator;
    public InputAction MoveAction;

    public float walkSpeed = 1.0f;
    public float turnSpeed = 20f;

    public bool isSprinting = false;
    public float sprintSpeed = 2.0f;
    public float stamina = 5.0f;
    public bool isTired = false;
    public float tiredSpeed = 0.5f;
    public Image StaminaBar;
    public Image TiredBar;

    
    public GameObject SB;
    public GameObject TB;

    Rigidbody m_Rigidbody;
    Vector3 m_Movement;
    Quaternion m_Rotation = Quaternion.identity;
    AudioSource m_AudioSource;

    private List<string> m_OwnedKeys = new List<string>();

    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        MoveAction.Enable();
        m_Animator = GetComponent<Animator>();
        m_AudioSource = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        var pos = MoveAction.ReadValue<Vector2>();

        float horizontal = pos.x;
        float vertical = pos.y;

        m_Movement.Set(horizontal, 0f, vertical);
        m_Movement.Normalize();

        bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f);
        bool hasVerticalInput = !Mathf.Approximately(vertical, 0f);
        bool isWalking = hasHorizontalInput || hasVerticalInput;
        m_Animator.SetBool("IsWalking", isWalking);

        Vector3 desiredForward = Vector3.RotateTowards(transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f);
        m_Rotation = Quaternion.LookRotation(desiredForward);

        m_Rigidbody.MoveRotation(m_Rotation);
        m_Rigidbody.MovePosition(m_Rigidbody.position + m_Movement * walkSpeed * Time.deltaTime);

        if (isWalking)
        {
            if (!m_AudioSource.isPlaying)
            {
                m_AudioSource.Play();
            }
        }
        else
        {
            m_AudioSource.Stop();
        }

        if(Input.GetKey(KeyCode.LeftShift))
        {
            if (isTired == false)
            {
                isSprinting = true;
            }
            else
            {
            }
        }
        else
        {
            isSprinting = false;
        }

        if (isSprinting == true)
        {
            walkSpeed = sprintSpeed;
            stamina -= Time.deltaTime;
        }
        else
        {
            walkSpeed = 1.0f;
            stamina += Time.deltaTime;
        }

        if (stamina <= 0f)
        {
            isTired = true;
        }

        if (isTired == true)
        {
            SB.SetActive(false);
            TB.SetActive(true);
            isSprinting = false;
            walkSpeed = tiredSpeed;
            stamina += Time.deltaTime;
        }
        else
        {
            SB.SetActive(true);
            TB.SetActive(false);
        }

        if (stamina >= 5.0f)
        {
            stamina = 5.0f;
            isTired = false;
        }

        StaminaBar.fillAmount = stamina / 5.0f;
        TiredBar.fillAmount = stamina / 5.0f;


    }

    public bool OwnKey(string keyName)
    {
        return m_OwnedKeys.Contains(keyName);
    }

   public void AddKey(string keyName, Sprite keySprite = null)
    {
        m_OwnedKeys.Add(keyName);
        if (keyUIManager != null && keySprite != null)
        {
            keyUIManager.AddKeyUI(keyName, keySprite);
        }
    }
}
