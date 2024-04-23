using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    private const float DAMP_TIME = 0.2f;
    private const int RUNNING_SPEED = 4;
    private const int AIM_RUNNING_SPEED = 1;

    [SerializeField] private GameObject Paladin;
    [SerializeField] private GameObject Archer;
    [SerializeField] private GameObject Mage;

    [SerializeField] private GameObject UnfocusedVCamera;
    [SerializeField] private GameObject AimVCamera;

    [SerializeField] private GameObject Crosshair;

    public bool IsAiming { get; protected set; }
    public bool IsRunning { get; protected set; }
    public bool IsDefending { get; protected set; }

    private KeyCode currentKeyCodeOfCharacter;
    private Dictionary<KeyCode, GameObject> characterMap = new Dictionary<KeyCode, GameObject>(3);

    private Vector3 movement;
    private Rigidbody body;

    

    // Start is called before the first frame update
    private void Start()
    {
        Health = MaxHealth;
        Speed = RUNNING_SPEED;

        currentKeyCodeOfCharacter = KeyCode.Alpha2;
        characterMap.Add(KeyCode.Alpha1, Paladin);
        characterMap.Add(KeyCode.Alpha2, Archer);
        characterMap.Add(KeyCode.Alpha3, Mage);

        animator = characterMap[currentKeyCodeOfCharacter].GetComponent<Animator>();
        body = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    private void Update()
    {
        BoolUpdate();
        KeyboardUpdate();
    }

    private void FixedUpdate()
    {
        Move();
    }
    private void BoolUpdate()
    {
        IsDefending = animator.GetBool("IsDefending");
        IsAttacking = animator.GetBool("IsAttacking");
    }
    private void KeyboardUpdate()
    {
        MovementUpdate();
        
        if (Input.GetButtonDown("Fire1"))
        {
            Attack();
        }

        if (Input.GetButtonDown("Fire2"))
        {
            Defend();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            AimTransition(!IsAiming);
        }

        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            ChangeCharacter(KeyCode.Alpha1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChangeCharacter(KeyCode.Alpha2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ChangeCharacter(KeyCode.Alpha3);
        }
    }
    private void MovementUpdate()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.z = Input.GetAxisRaw("Vertical");

        IsRunning = ((movement.x != 0 || movement.z != 0) && !IsAttacking && !IsDefending);
    }
    private void ChangeCharacter(KeyCode numberPressed)
    {
        if(currentKeyCodeOfCharacter != numberPressed)
        {
            AimTransition(false);

            characterMap[currentKeyCodeOfCharacter].SetActive(false);
            characterMap[numberPressed].SetActive(true);
            currentKeyCodeOfCharacter = numberPressed;

            animator = characterMap[currentKeyCodeOfCharacter].GetComponent<Animator>();
        }
    }

    protected override void Die()
    {

    }

    private void Move()
    {
        if(IsRunning)
            body.MovePosition(body.position + movement * Speed * Time.fixedDeltaTime);
        else
            movement = Vector3.zero;
        animator.SetBool("IsRunning", IsRunning);
        animator.SetFloat("Horizontal", movement.x, DAMP_TIME, Time.deltaTime);
        animator.SetFloat("Vertical", movement.z, DAMP_TIME, Time.deltaTime);

    }
    protected override void Attack()
    {
        if(currentKeyCodeOfCharacter != KeyCode.Alpha2 || IsAiming)
        {
            animator.SetBool("IsAttacking", true);
            animator.SetBool("IsRunning", false);
        }
        
    }
    protected override void GetStunned()
    {

    }

    private void Defend()
    {
        animator.SetBool("IsDefending", true);
        animator.SetBool("IsRunning", false);
        animator.SetBool("IsAiminig", false);
    }

    private void AimTransition(bool IsSupposedToAim)
    {
        if(currentKeyCodeOfCharacter == KeyCode.Alpha2 && IsAiming != IsSupposedToAim)
        {
            
            IsAiming = IsSupposedToAim;
            characterMap[currentKeyCodeOfCharacter].GetComponent<Animator>().SetBool("IsAiming", IsAiming);
            Crosshair.SetActive(IsAiming);
            Speed = IsAiming ? AIM_RUNNING_SPEED : RUNNING_SPEED;

            CameraTransition();
        }
    }

    private void CameraTransition()
    {
        UnfocusedVCamera.SetActive(!IsAiming);
        AimVCamera.SetActive(IsAiming);    
    }
}
