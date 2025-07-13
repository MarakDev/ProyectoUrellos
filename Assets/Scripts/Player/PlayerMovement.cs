using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Debug mode")]
    public TMP_Text textJugador;
    public TMP_Text stamina;

    [Header("Movimiento")]
    private float moveSpeed;
    public float walkSpeed;
    public float runSpeed;
    public float crouchSpeed;

    public float exhaustedSpeed;
    public float staminaDebuff;

    public float groundDrag;

    [Header("Salto")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    [Header("Agacharse")]
    public float crouchYScale;
    private float startYScale;
    private bool justPressed;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;

    [Header("Cuesta Control")]
    public float maxAngle;
    private RaycastHit cuestaHit;
    private bool salirCuesta;

    bool grounded;
    bool crouchSmallSpace;
    bool isExhausted;
    private float exhaustCooldown = 0;

    [Header("Key Binds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode runKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;

    [Header("")]
    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;
    RaycastHit hit;

    public bool jugadorCaidaSuelo_Flag;
    public MovementState state;

    private float _timeSinceLastStep;
    private bool izqOrDer = true;
    private int nPaso = 0;

    private WaitForSeconds corrutineDelay;

    public enum MovementState
    {
        walk,
        run,
        crouch,
        exhausted,
        air
    }

    void Start()
    {
        corrutineDelay = new WaitForSeconds(staminaDebuff);

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;
        isExhausted = false;

        startYScale = transform.localScale.y;
    }
    /*
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position + new Vector3(0, -playerHeight * 0.5f, 0), 0.3f);
    }
    Esfera debug*/

    private void Update()
    {
        //Debug.DrawRay(transform.position,, Color.green);

        //grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);
        grounded = Physics.SphereCast(transform.position, 0.3f, Vector3.down, out hit, playerHeight * 0.5f, whatIsGround);

        CheckIfLand();
        PlayerInput();
        SpeedControl();
        StateHandler();
        PlayerSounds();

        if (state == MovementState.crouch)
            crouchSmallSpace = Physics.SphereCast(transform.position, 0.475f, Vector3.up, out hit, playerHeight * 0.475f, whatIsGround);
        else
            crouchSmallSpace = false;

        if (grounded) 
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }

    }

    private void PlayerSounds()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatVel.magnitude > 0 && state == MovementState.walk)
        {
            _timeSinceLastStep += Time.deltaTime +Time.deltaTime;
            if(_timeSinceLastStep > 1) 
            {
                _timeSinceLastStep = 0;

                if (izqOrDer)//paso izquierdo
                {
                    SoundManager.Instance.SeleeccionAudio(1 + nPaso, 0.4f, "Jugador");
                    izqOrDer = false;
                }
                else //paso derecho
                {
                    SoundManager.Instance.SeleeccionAudio(4 + nPaso, 0.4f, "Jugador");
                    izqOrDer = true;
                }

                nPaso++;
                if(nPaso > 2)
                {
                    nPaso = 0;
                }
            }

        }

        if (flatVel.magnitude > 0 && state == MovementState.run)
        {
            _timeSinceLastStep += Time.deltaTime + Time.deltaTime;
            if (_timeSinceLastStep > 0.6f)
            {
                _timeSinceLastStep = 0;

                if (izqOrDer)//paso izquierdo
                {
                    SoundManager.Instance.SeleeccionAudio(1 + nPaso, 1.5f, "Jugador");
                    izqOrDer = false;
                }
                else
                {
                    SoundManager.Instance.SeleeccionAudio(4 + nPaso, 1.5f, "Jugador");
                    izqOrDer = true;
                }

                nPaso++;
                if (nPaso > 2)
                {
                    nPaso = 0;
                }
            }
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();

        //textJugador.text = ("Estado = " + state.ToString());
        //stamina.text = ("Velocidad = " + rb.velocity.magnitude);

    }

    private void PlayerInput() 
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        //SALTAR

        if (Input.GetKey(jumpKey) && readyToJump && grounded && !isExhausted)
        {
            readyToJump = false;

            /*
             * 
             * 
             * Que el jugador no pudiera saltar es una decision deliberada. Ya que rompia los colliders del mapa y podia acceder a zonas que no deberia
             * 
             * 
             * 
             */
            //Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }

        //AGACHARSE

        if (Input.GetKey(crouchKey) && !isExhausted && state != MovementState.run)
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            if (justPressed)
            {
                SoundManager.Instance.SeleeccionAudio(7, 1.5f, "Jugador");
                rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
                justPressed = false;
            }
                
        }
        else if(!crouchSmallSpace)
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
            justPressed = true;
        }
    }

    private void StateHandler()
    {
        //Estado = Correr
        if (grounded && !crouchSmallSpace && !isExhausted && Input.GetKey(runKey))
        {
            state = MovementState.run;
            moveSpeed = runSpeed;
            StartCoroutine(RunCooldown());
        }
        //Estado = Agacharse
        else if (!isExhausted && grounded && Input.GetKey(crouchKey))
        {
            state = MovementState.crouch;
            moveSpeed = crouchSpeed;
            StaminaRegeneration();
        }
        //Estado = Andar
        else if (!isExhausted && grounded && !crouchSmallSpace)
        {
            state = MovementState.walk;
            moveSpeed = walkSpeed;
            StaminaRegeneration();
        }
        //Estado = Cansado
        else if (isExhausted)
        {
            state = MovementState.exhausted;
            moveSpeed = exhaustedSpeed;
        }
        //Estado = en el Aire
        else if (!isExhausted && !grounded)
        {
            StartCoroutine(RunCooldown());
            state = MovementState.air;
        }
        //Estado Idle, Regenerando stamina
        else
            StaminaRegeneration();

    }

    private void MovePlayer() 
    {
        //Calcular direccion del movimiento
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        //Movimiento del jugador en una cuesta
        if (OnCuesta() && !salirCuesta)
        {
            if(state == MovementState.crouch)
                rb.AddForce(DireccionCuesta() * moveSpeed * 35f, ForceMode.Force);
            else
                rb.AddForce(DireccionCuesta() * moveSpeed * 20f, ForceMode.Force);

            if (rb.velocity.y > 0)
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
        }
        
        //movimiento del jugador en el suelo plano
        else if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        //movimiento del jugador en el aire
        else if (!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);

        //si esta en una cuesta, desactiva la gravedad para que no se caiga
        rb.useGravity = !OnCuesta();

    }

    private void SpeedControl()
    {
        //limitacion de velocidad en cuesta
        if (OnCuesta() && !salirCuesta)
        {
            if (rb.velocity.magnitude > moveSpeed)
                rb.velocity = rb.velocity.normalized * moveSpeed;

            if (rb.velocity.magnitude < 1f)
                rb.velocity = Vector3.zero;

        }

        //limitacion de velocidad en el suelo y el aire
        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            //limitacion de velocidad
            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
        
        //SI el jugador esta parado velocidad = 0
        if (rb.velocity.magnitude < 0.01f)
            rb.velocity = Vector3.zero;
    }

    private void Jump()
    {
        salirCuesta = true;

        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

    }

    private void ResetJump()
    {
        readyToJump = true;

        salirCuesta = false;
    }

    private IEnumerator RunCooldown()
    {
        if(rb.velocity.y > 0f && state == MovementState.air)
            exhaustCooldown += Time.deltaTime * 2;
        else if (rb.velocity.magnitude > 0f && state == MovementState.run)
            exhaustCooldown += Time.deltaTime;
        else
            StaminaRegeneration();

        if (exhaustCooldown > 5)
        {
            exhaustCooldown = 0;
            isExhausted = true;
            yield return corrutineDelay;
            isExhausted = false;
        }
    }

    private void StaminaRegeneration()
    {
        exhaustCooldown -= Time.deltaTime * 0.5f;
        if (exhaustCooldown < 0)
            exhaustCooldown = 0;
    }

    private bool OnCuesta()
    {
        
        if (Physics.Raycast(transform.position, Vector3.down, out cuestaHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, cuestaHit.normal);
            return angle < maxAngle && angle > 10;
        }
        return false;
    }

    private Vector3 DireccionCuesta()
    {
        return Vector3.ProjectOnPlane(moveDirection, cuestaHit.normal).normalized;
    }

    private void CheckIfLand()
    {
        if (grounded && state == MovementState.air && rb.velocity.y < -4)
            jugadorCaidaSuelo_Flag = true;
        else
            jugadorCaidaSuelo_Flag = false;
    }
}
