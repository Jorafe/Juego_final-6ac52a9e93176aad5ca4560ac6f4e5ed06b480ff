using System.Collections;
using UnityEngine;

public class PortalSpawner : MonoBehaviour
{
    public BoxCollider spawnArea;
    public GameObject firstObjectPrefab;
    public GameObject secondObjectPrefab;
    public GameObject planePrefab;

    // Escalas que ahora se pueden modificar desde el Inspector
    public Vector3 firstObjectScale = new Vector3(1f, 5f, 5f);
    public Vector3 secondObjectScale = new Vector3(5f, 5f, 5f);
    public Vector3 planeScale = new Vector3(5f, 1f, 5f);

    public string groundLayerName = "whatisGround"; // Nombre del layer en lugar de tag
    public float raycastDistance = 1000f;
    public float secondObjectDelay = 3f;
    public float spawnInterval = 15f;
    public float objectLifeTime = 5f; // Tiempo de vida para los objetos (en segundos)

    private void Start()
    {
        InvokeRepeating("SpawnRandomPortalGroups", 0f, spawnInterval);
    }

    private void SpawnRandomPortalGroups()
    {
        // Aleatoriamente decidimos cuántos grupos de portales instanciar
        int groupCount = Random.Range(1, 6);  // De 1 a 5 grupos de portales

        for (int i = 0; i < groupCount; i++)
        {
            StartCoroutine(SpawnPortalGroup());
        }
    }

    private IEnumerator SpawnPortalGroup()
    {
        // Obtener una posición aleatoria dentro del área del spawn
        Vector3 randomPosition = GetRandomPositionWithinCollider(spawnArea);

        // Instanciamos el primer objeto con la escala asignada desde el Inspector
        GameObject firstObject = Instantiate(firstObjectPrefab, randomPosition, Quaternion.identity);
        firstObject.transform.localScale = firstObjectScale;

        // Instanciamos el segundo objeto después del retraso
        StartCoroutine(InstantiateSecondObject(firstObject));

        // Instanciamos el plano de alerta tras detectar el suelo con raycast
        StartCoroutine(SpawnAlertMark(firstObject));

        // Destruimos el primer objeto después del tiempo de vida especificado
        Destroy(firstObject, objectLifeTime);

        // Asegurarnos de que los objetos con el tag "Enemy" no colisionen con nada
        GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemyObject in enemyObjects)
        {
            // Ignorar la colisión entre el "Enemy" y los otros objetos
            Collider enemyCollider = enemyObject.GetComponent<Collider>();
            if (enemyCollider != null)
            {
                Physics.IgnoreCollision(firstObject.GetComponent<Collider>(), enemyCollider);
                Physics.IgnoreCollision(secondObjectPrefab.GetComponent<Collider>(), enemyCollider);
                Physics.IgnoreCollision(planePrefab.GetComponent<Collider>(), enemyCollider);
            }
        }

        yield return null;
    }

    private IEnumerator InstantiateSecondObject(GameObject firstObject)
    {
        yield return new WaitForSeconds(secondObjectDelay);

        if (firstObject == null) yield break;

        // Calculamos la posición y escala para el segundo objeto
        Vector3 secondObjectPosition = firstObject.transform.position - new Vector3(0, firstObjectScale.y / 2 + secondObjectScale.y / 2, 0);
        GameObject secondObject = Instantiate(secondObjectPrefab, secondObjectPosition, Quaternion.identity);
        secondObject.transform.localScale = secondObjectScale;
        secondObject.transform.parent = firstObject.transform;

        // Destruimos el segundo objeto después del tiempo de vida especificado
        Destroy(secondObject, objectLifeTime);

        // Asegurarnos de que el segundo objeto no colisione con enemigos
        GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemyObject in enemyObjects)
        {
            // Ignorar la colisión entre el "Enemy" y el segundo objeto
            Collider enemyCollider = enemyObject.GetComponent<Collider>();
            if (enemyCollider != null)
            {
                Physics.IgnoreCollision(secondObject.GetComponent<Collider>(), enemyCollider);
            }
        }
    }

    private IEnumerator SpawnAlertMark(GameObject firstObject)
    {
        yield return new WaitForEndOfFrame();

        RaycastHit hit;
        Vector3 rayOrigin = firstObject.transform.position;

        // Lanzamos el raycast para detectar el suelo
        if (Physics.Raycast(rayOrigin, Vector3.down, out hit, raycastDistance))
        {
            Debug.Log("Raycast hit: " + hit.transform.gameObject.name);

            // Verificamos si el objeto golpeado está en el layer "whatisGround"
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer(groundLayerName))
            {
                Debug.Log("Raycast hit the ground layer");

                // Instanciamos el plano justo sobre el suelo detectado
                Vector3 planePosition = hit.point + Vector3.up * 0.01f; // Añadimos una pequeña altura para evitar solapamientos
                GameObject plane = Instantiate(planePrefab, planePosition, Quaternion.identity);
                plane.transform.localScale = planeScale;
                plane.transform.parent = hit.transform; // Adjuntamos al objeto del suelo

                // Destruimos el plano después del tiempo de vida especificado
                Destroy(plane, objectLifeTime);
            }
            else
            {
                Debug.Log("Raycast hit a non-ground layer: " + hit.transform.gameObject.layer);
            }
        }
        else
        {
            Debug.Log("Raycast did not hit anything.");
        }
    }

    private Vector3 GetRandomPositionWithinCollider(BoxCollider boxCollider)
    {
        Vector3 colliderCenter = boxCollider.bounds.center;
        Vector3 colliderSize = boxCollider.bounds.size;

        float randomX = Random.Range(colliderCenter.x - colliderSize.x / 2, colliderCenter.x + colliderSize.x / 2);
        float randomZ = Random.Range(colliderCenter.z - colliderSize.z / 2, colliderCenter.z + colliderSize.z / 2);

        return new Vector3(randomX, colliderCenter.y, randomZ);
    }
}