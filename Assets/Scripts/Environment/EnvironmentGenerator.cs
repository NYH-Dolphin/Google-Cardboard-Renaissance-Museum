using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentGenerator : MonoBehaviour
{
    public GameObject coinPrefab;
    public List<GameObject> carPrefab;
    public List<GameObject> treePrefabs;
    public List<GameObject> buildingPrefabs;

    private static float fEnd = -84;
    private static float fStart = 48;


    // left | middle | right
    private static float[] _coinPos = { -3f, 0f, 3f };
    private static float[] _treePos = { -6f, 6f };
    private static float[] _buildingPos = { -15f, 15f };


// Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CoinAndCarGeneration());
        StartCoroutine(TreeGeneration(0));
        StartCoroutine(TreeGeneration(1));
        StartCoroutine(BuildingGeneration(0));
        StartCoroutine(BuildingGeneration(1));
    }

    IEnumerator TreeGeneration(int side)
    {
        while (true)
        {
            float time = Random.Range(1, 3);
            GameObject tree = Instantiate(treePrefabs[Random.Range(0, treePrefabs.Count)]);
            tree.transform.position = new Vector3(_treePos[side], 0, fStart);
            yield return new WaitForSeconds(time);
        }
    }


    IEnumerator BuildingGeneration(int side)
    {
        while (true)
        {
            float time = Random.Range(1, 3);
            GameObject building = Instantiate(buildingPrefabs[Random.Range(0, buildingPrefabs.Count)]);
            building.transform.position = new Vector3(_buildingPos[side], 0, fStart);
            yield return new WaitForSeconds(time);
        }
    }

    IEnumerator CoinAndCarGeneration()
    {
        while (true)
        {
            float time = Random.Range(2, 5);
            int posCoin = Random.Range(0, _coinPos.Length);
            bool bcar = false;
            for (int i = 0; i < time; i++)
            {
                GameObject coin = Instantiate(coinPrefab);
                coin.transform.position = new Vector3(_coinPos[posCoin], 0.3f, fStart);

                bcar = !bcar;
                if (bcar)
                {
                    GameObject car = Instantiate(carPrefab[Random.Range(0, carPrefab.Count)]);
                    int posCar = (posCoin + Random.Range(1, 3)) % 3;
                    car.transform.position = new Vector3(_coinPos[posCar], -0.2f, fStart);
                }

                yield return new WaitForSeconds(0.5f);
            }
            
            yield return new WaitForSeconds(1f);
            
            time = Random.Range(2, 5);
            for (int i = 0; i < time * 2; i++)
            {
                bcar = !bcar;
                if (bcar)
                {
                    int posCar = Random.Range(0, _coinPos.Length);
                    GameObject car = Instantiate(carPrefab[Random.Range(0, carPrefab.Count)]);
                    car.transform.position = new Vector3(_coinPos[posCar], -0.2f, fStart);
                }

                yield return new WaitForSeconds(0.5f);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}