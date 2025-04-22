using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalSpawner : MonoBehaviour
{
    public BoxCollider spawnArea;
    public GameObject firstObjectPrefab;
    public GameObject secondObjectPrefab;
    public GameObject planePrefab;

    public Vector3 firstObjectScale = new Vector3(1f, 5f, 5f);
    public Vector3 secondObjectScale = new Vector3(5f, 5f, 5f);
    public Vector3 planeScale = new Vector3(5f, 1f, 5f);

    public string groundLayerName = "whatisGround";
    public float raycastDistance = 1000f;
    public float secondObjectDelay = 3f;
    public float spawnInterval = 15f;
    public float objectLifeTime = 5f;

    public float minDistanceBetweenPortals = 1f; // Nueva variable para controlar distancia mínima

    private void Start()
    {
        InvokeRepeating("SpawnRandomPortalGroups", 0f, spawnInterval);
    }

    private void SpawnRandomPortalGroups()
    {
        int groupCount = Random.Range(4, 7); // De 4 a 6 grupos
        List<Vector3> usedPositions = new List<Vector3>();

        int attempts = 0;

        while (usedPositions.Count < groupCount && attempts < groupCount * 10)
        {
            Vector3 randomPosition = GetRandomPositionWithinCollider(spawnArea);
            bool tooClose = false;

            foreach (Vector3 pos in usedPositions)
            {
                if (Vector3.Distance(randomPosition, pos) < minDistanceBetweenPortals)
                {
                    tooClose = true;
                    break;
                }
            }

            if (!tooClose)
            {
                usedPositions.Add(randomPosition);
                StartCoroutine(SpawnPortalGroup(randomPosition));
            }

            attempts++;
        }
    }

    private IEnumerator SpawnPortalGroup(Vector3 position)
    {
        GameObject firstObject = Instantiate(firstObjectPrefab, position, Quaternion.identity);
        firstObject.transform.localScale = firstObjectScale;

        StartCoroutine(InstantiateSecondObject(firstObject));
        StartCoroutine(SpawnAlertMark(firstObject));

        Destroy(firstObject, objectLifeTime);

        GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemyObject in enemyObjects)
        {
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

        Vector3 secondObjectPosition = firstObject.transform.position - new Vector3(0, firstObjectScale.y / 2 + secondObjectScale.y / 2, 0);
        GameObject secondObject = Instantiate(secondObjectPrefab, secondObjectPosition, Quaternion.identity);
        secondObject.transform.localScale = secondObjectScale;
        secondObject.transform.parent = firstObject.transform;

        Destroy(secondObject, objectLifeTime);

        GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemyObject in enemyObjects)
        {
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

        int groundLayer = LayerMask.NameToLayer(groundLayerName);
        int layerMask = 1 << groundLayer;

        if (Physics.Raycast(rayOrigin, Vector3.down, out hit, raycastDistance, layerMask))
        {
            Debug.Log("Raycast hit: " + hit.transform.gameObject.name);

            Vector3 planePosition = hit.point + Vector3.up * 0.01f;
            GameObject plane = Instantiate(planePrefab, planePosition, Quaternion.identity);
            plane.transform.localScale = planeScale;
            plane.transform.parent = hit.transform;

            Destroy(plane, objectLifeTime);
        }
        else
        {
            Debug.Log("Raycast did not hit anything on the ground layer.");
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