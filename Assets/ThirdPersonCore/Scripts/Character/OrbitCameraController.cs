
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;


namespace B2BGTPC
{
    public class OrbitCameraController : MonoBehaviour
    {

        public Transform objective;                                     // Objecto a seguir

        public Vector3 pivotOffset = new Vector3(0f, 1f, 0f);           // Punto de origen al que la camara seguira 

        public Vector3 camOffset = new Vector3(0f, 0f, -2f);            // Offset para posicionar la camara basada en el pivote anterior

        public float smooth = 5f;                                       // Smooth damp para la camara
        public float horizontalAimingSpeed = 6f;                        // Velocidad de rotacion horizontal.
        public float verticalAimingSpeed = 6f;                          // Velocidad de rotacion vertical.
        public float maxVerticalAngle = 30f;                            // Maximo angulo vertical. 
        public float minVerticalAngle = -60f;                           // Minimo angulo vertical.

        private Transform cam;                                          // Cache de nuestro transform

        private float angleH = 0;                                       // angulo horizontal respecto al movimiento del raton.
        private float angleV = 0;                                       // angulo vertical respecto al movimiento del raton.
        private Vector3 smoothPivotOffset;                              // Offset de interpolacion del pivote respecto al transform del objetivo
        private Vector3 smoothCamOffset;                                // Offset de interpolacion de la camara respecto al pivote.
        private Vector3 targetPivotOffset;                              // Posicion de interpolacion del pivote hacia la camara.
        private Vector3 targetCamOffset;                                // Pocision de interpolacion de la camara hacia el pivote.
        private float targetMaxVerticalAngle;                           // Angulo maximo vertical de la camara usado internamente

        private void Awake()
        {
            // Hacemos cache de el transform
            cam = transform;

            // Posicionamos la camara en el punto por default haciendo que la rotation tanto del pivote como de la camara sean la identidad del Quaternion
            cam.position = objective.position + Quaternion.identity * pivotOffset + Quaternion.identity * camOffset;
            cam.rotation = Quaternion.identity;
            // Obtenemos una referencia del pivote y de la pocision de la camara en sus valores por defecto
            smoothPivotOffset = pivotOffset;
            smoothCamOffset = camOffset;
            // Se pone el vetor y del objetivo como referncia para el recorrido horizontal
            angleH = objective.eulerAngles.y;

            ResetTargetOffsets();
            ResetMaxVerticalAngle();
        }

        

        
        void Update()
        {
            // Input desde el mouse
            angleH += Mathf.Clamp(CrossPlatformInputManager.GetAxis("Mouse X"), -1, 1) * horizontalAimingSpeed;
            angleV += Mathf.Clamp(CrossPlatformInputManager.GetAxis("Mouse Y"), -1, 1) * verticalAimingSpeed;

            // Limite de angulo vertical.
            angleV = Mathf.Clamp(angleV, minVerticalAngle, targetMaxVerticalAngle);

            // Asignamos la aceleracion horizontal sobre el eje Y, esto es se asigna una rotacion respecto a la rotacion anterior si es que la hay 
            Quaternion camYRotation = Quaternion.Euler(0, angleH, 0);
            // Calculamos una rotacion la cual hara que la camara voltee en sentido inverso sobre el eje de las X usando el input vertical y dejando la rotacion en Y en su sentido normal
            Quaternion aimRotation = Quaternion.Euler(-angleV, angleH, 0);
            cam.rotation = aimRotation;

            // Re posicionamos la camara basado en la nueva posicion del jugador, calculamos nuevamente los offsets desde la posicion anterior se mueva o no el jugador
            smoothPivotOffset = Vector3.Lerp(smoothPivotOffset, targetPivotOffset, Time.deltaTime);
            smoothCamOffset = Vector3.Lerp(smoothCamOffset, targetCamOffset, Time.deltaTime);

            // la nueva posicion de la camara es la rotacion horizontl y rotacion vertical previamente calculadas manteniendo la posicion del pivote 
            // y de la camara calculados previemente para mantener la camara en el lugar correcto respecto al movimiento en x y z
            cam.position = objective.position + camYRotation * smoothPivotOffset + aimRotation * smoothCamOffset;

            //Debug.DrawLine(objective.position, objective.position + pivotOffset, Color.red);
            Debug.DrawLine(objective.position + pivotOffset + camOffset, objective.position + pivotOffset, Color.cyan);
            Debug.DrawLine(objective.position + smoothCamOffset, objective.position + pivotOffset + camOffset, Color.black);
            Debug.DrawLine(objective.position + smoothPivotOffset, transform.localPosition, Color.yellow);
            //Debug.DrawLine(transform.localPosition, objective.position + pivotOffset, Color.green);
        }


        // Regresar los objetivos a sus valores originales
        public void ResetTargetOffsets()
        {
            targetCamOffset = camOffset;
            targetPivotOffset = pivotOffset;
        }

        // Reset max vertical camera rotation angle to default value.
        public void ResetMaxVerticalAngle()
        {
            this.targetMaxVerticalAngle = maxVerticalAngle;
        }

    }
}


