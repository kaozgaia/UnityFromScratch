
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace B2BGTPC
{
    /// <summary>Clase encargada de el input sobre el character animado y controlado por el usuario.
    /// Author : Christian A Fernandez
    /// </summary>
    public class TPCharacterController : MonoBehaviour
    {

        public float inputDelay = 0.1f;                         // Espacio para retrasar la recepcion de input

        public float rotateVel = 100;                           // Velocidad de rotacion

        public Quaternion TargetRotation                        // Exponemos la rotacion actual
        {
            get { return targetRotation; }
        }

        public float smoothDamp = 5f;                           // Desplazamiento amortiguado

        private Quaternion targetRotation;                      // Rotacion para uso interno

        private float forwardInput, turnInput;                  // definicion del input para aceleracion y rotacion

        private Animator anim;                                  // el animator

        private int vSpeedHash;                                 // hash de la velocidad vertical

        private bool isRunning = false;                         // bandera a validar si se esta o no corriendo

        private float interpolateValue = 0f;                    // valor t para uso en la interpolacion lineal de la aceleracion horizontal de 1 a 2, ver animator en el editor


        // Methods

        private void Start()
        {
            // Cache de componentes
            targetRotation = transform.rotation;
            anim = GetComponent<Animator>();
            // Convertimos el string en un hash valido para el animator
            vSpeedHash = Animator.StringToHash("vSpeed");
            // Inicializacion
            forwardInput = 0;
            turnInput = 0;
            interpolateValue = 0f;
        }

        void GetInput()
        {
            // Input horizontal y vertical
            forwardInput = Mathf.Clamp(CrossPlatformInputManager.GetAxis("Vertical") * smoothDamp * Time.deltaTime, -1, 1); // Obtenemos el input vertical y como maximo aceptamos 1 y como minimo -1
            turnInput = Mathf.Clamp(CrossPlatformInputManager.GetAxis("Horizontal") * smoothDamp * Time.deltaTime, -1, 1); // Input horizontal con maximo y minimo

            // Validacion si el usuario quiere correr o no
            isRunning = Input.GetKey(KeyCode.LeftShift);
            if (isRunning && forwardInput > 0) {
                // Interpolacion lineal de 1 a 2
                interpolateValue += 0.1f;
                forwardInput = Mathf.Lerp(1,2, interpolateValue);
            }
            else
            {
                // Reiniciamos el valor t de la interpolacion
                interpolateValue = 0f;
            }
            // Se agrega al animator el input calculado
            if (anim) anim.SetFloat(vSpeedHash, forwardInput);
        }

        private void Update()
        {
            // Metodos de rotacion y translacion
            GetInput();
            Turn();
        }

        void Turn()
        {
            // Esperamos un poco antes de aceptar el input
            if (Mathf.Abs(turnInput) > inputDelay)
            {
                // Rotamos poco a poco sobre el vetor y
                targetRotation *= Quaternion.AngleAxis(rotateVel * turnInput * Time.deltaTime, Vector3.up);

            }
            // el resultante calculado lo asignamos a la rotacion actual
            transform.rotation = targetRotation;
        }


    }
}


