using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Main : MonoBehaviour {
    public static Main S;

    public GameObject[] prefabEnemies;
    public float enemySpawnPerSecond = 0.5f;  // # enemies/second
    public float enemySpawnPadding = 1.5f; // Padding for position

    [Space(10)]
    public float enemySpawnRate;

    void Awake()
    {
        S = this;
        Utils.SetCameraBounds(gameObject.GetComponent<Camera>());
        enemySpawnRate = 1f / enemySpawnPerSecond;
        Invoke("SpawnEnemy", enemySpawnRate);

    }

	public void SpawnEnemy()
    {
        int ndx = Random.Range(0, prefabEnemies.Length);
        GameObject go = Instantiate(prefabEnemies[ndx]) as GameObject;
        Vector3 pos = Vector3.zero;
        float xMin = Utils.cambounds.min.x + enemySpawnPadding;
        float xMax = Utils.cambounds.max.x + enemySpawnPadding;
        pos.x = Random.Range(xMin, xMax);
        pos.y = Utils.cambounds.max.y + enemySpawnPadding;
        go.transform.position = pos;
        Invoke("SpawnEnemy", enemySpawnRate);
    }

    public void DelayedRestart(float delay)
    {
        Invoke("Restart", delay);
    }

    public void Restart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("__Scene_0");
    }
}