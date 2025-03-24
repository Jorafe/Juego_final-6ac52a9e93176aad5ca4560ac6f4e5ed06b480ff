using UnityEngine;

public class PortalSpawner : MonoBehaviour
{
    public BoxCollider spawnArea;            // Área del BoxCollider donde se instanciarán los objetos
    public GameObject firstObjectPrefab;     // Objeto que se instanciará primero (cubo de 1x5x5)
    public GameObject secondObjectPrefab;    // Objeto que se instanciará 3 segundos después del primero (cubo de 5x5x5)
    public GameObject planePrefab;           // El plano que se instanciará encima del objeto con el tag "whatisground"

    // Escalas base para los objetos
    public Vector3 firstObjectScale = new Vector3(1f, 5f, 5f);   // Escala base del primer objeto (1x5x5)
    public Vector3 secondObjectScale = new Vector3(5f, 5f, 5f);  // Escala base del segundo objeto (5x5x5)
    public Vector3 planeScale = new Vector3(5f, 1f, 5f);        // Escala base del plano (5x1x5)

    public string groundTag = "whatisground"; // Tag del objeto que el raycast debe detectar
    public float raycastDistance = 1000f;    // Distancia del raycast (1 km)
    public float secondObjectDelay = 3f;     // Tiempo de retraso para instanciar el segundo objeto
    public float spawnInterval = 15f;        // Intervalo para instanciar el grupo (en segundos)
    public float scaleVariationFactor = 0.05f; // Factor de variación aleatoria (5%)
    public float lifeTime = 6f;               // Tiempo de vida total del grupo en segundos

    private GameObject firstObject;
    private GameObject secondObject;
    private GameObject plane;

    private void Start()
    {
        // Comenzar a instanciar el grupo de objetos cada 15 segundos
        InvokeRepeating("SpawnPortalGroup", 0f, spawnInterval);
    }

    private void SpawnPortalGroup()
    {
        // Instanciar el primer objeto en una posición aleatoria dentro del BoxCollider
        Vector3 randomPosition = new Vector3(
            Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x),
            Random.Range(spawnArea.bounds.min.y, spawnArea.bounds.max.y),
            Random.Range(spawnArea.bounds.min.z, spawnArea.bounds.max.z)
        );

        // Escalar aleatoriamente el primer objeto (con una variación del 5%)
        Vector3 scaledFirstObject = GetRandomScaledVector(firstObjectScale);

        // Instanciar el primer objeto
        firstObject = Instantiate(firstObjectPrefab, randomPosition, Quaternion.identity);
        firstObject.transform.localScale = scaledFirstObject; // Aplicar la escala aleatoria

        // Instanciar el segundo objeto 3 segundos después del primero
        Invoke("InstantiateSecondObject", secondObjectDelay);

        // Lanzar un raycast desde el primer objeto hacia abajo para encontrar el objeto "whatisground"
        RaycastHit hit;
        Ray ray = new Ray(firstObject.transform.position, Vector3.down); // Dirección hacia abajo
        if (Physics.Raycast(ray, out hit, raycastDistance))
        {
            // Si el raycast golpea un objeto con el tag especificado
            if (hit.transform.CompareTag(groundTag))
            {
                // Instanciar el plano 1 metro por encima del objeto "whatisground"
                Vector3 spawnPosition = hit.point + Vector3.up; // Un metro por encima del objeto
                plane = Instantiate(planePrefab, spawnPosition, Quaternion.identity);
                // Escalar el plano en relación al segundo objeto
                plane.transform.localScale = GetScaledPlaneForSecondObject();
            }
        }

        // Destruir todos los objetos después de 6 segundos
        Destroy(firstObject, lifeTime);
        Invoke("DestroySecondObject", secondObjectDelay + lifeTime);
        Invoke("DestroyPlane", secondObjectDelay + lifeTime);
    }

    private void InstantiateSecondObject()
    {
        // Instanciar el segundo objeto en una posición aleatoria
        Vector3 randomPosition = new Vector3(
            Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x),
            Random.Range(spawnArea.bounds.min.y, spawnArea.bounds.max.y),
            Random.Range(spawnArea.bounds.min.z, spawnArea.bounds.max.z)
        );

        // Escalar aleatoriamente el segundo objeto (con una variación del 5%)
        Vector3 scaledSecondObject = GetRandomScaledVector(secondObjectScale);

        // Instanciar el segundo objeto
        secondObject = Instantiate(secondObjectPrefab, randomPosition, Quaternion.identity);
        secondObject.transform.parent = firstObject.transform; // Hacer que sea hijo del primer objeto
        secondObject.transform.localScale = scaledSecondObject; // Aplicar la escala aleatoria
    }

    // Función para escalar aleatoriamente un objeto dentro de un 5% de su escala original
    private Vector3 GetRandomScaledVector(Vector3 originalScale)
    {
        return new Vector3(
            Random.Range(originalScale.x - originalScale.x * scaleVariationFactor, originalScale.x + originalScale.x * scaleVariationFactor),
            Random.Range(originalScale.y - originalScale.y * scaleVariationFactor, originalScale.y + originalScale.y * scaleVariationFactor),
            Random.Range(originalScale.z - originalScale.z * scaleVariationFactor, originalScale.z + originalScale.z * scaleVariationFactor)
        );
    }

    // Función para escalar el plano basado en el segundo objeto
    private Vector3 GetScaledPlaneForSecondObject()
    {
        // Escalar el plano de acuerdo con la escala del segundo cubo, manteniendo las proporciones
        return new Vector3(
            secondObjectScale.x, 
            planeScale.y, // Mantener el mismo valor de altura para el plano
            secondObjectScale.z
        );
    }

    // Funciones para destruir los objetos después del tiempo de vida
    private void DestroySecondObject()
    {
        if (secondObject != null)
            Destroy(secondObject);
    }

    private void DestroyPlane()
    {
        if (plane != null)
            Destroy(plane);
    }
}