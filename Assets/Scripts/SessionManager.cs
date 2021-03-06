using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GoogleMobileAds.Api;

public class SessionManager : MonoBehaviour
{
    [SerializeField] private GameObject welcominUI;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject playmodeUI;
    [SerializeField] private Text scoreText;
    [SerializeField] private GameObject sphere;
    [SerializeField] private GameObject caveTile;


    [SerializeField] private Button startGame;

    [SerializeField] private Dropdown dificultyLvl;

    private GameObject[] spawnedTiles;
    private float tileLength;


    private InterstitialAd interstitial;

    private void RequestInterstitial()
    {
#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-7037210179712297/5093616831";
#elif UNITY_IPHONE
        string adUnitId = "unexpected_platform";
#else
        string adUnitId = "unexpected_platform";
#endif

        // Initialize an InterstitialAd.
        this.interstitial = new InterstitialAd(adUnitId);

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        this.interstitial.LoadAd(request);
    }

    private void Start()
    {
        MobileAds.Initialize(initStatus => { });
        tileLength = GetTileLength();
        spawnedTiles = new GameObject[3];
        startGame.onClick.AddListener(delegate
        {
            welcominUI.SetActive(false);
            playmodeUI.SetActive(true);
            StartGame();
        });
        SimpleController.GameOver += OnGameOver;
    }



    private void StartGame()
    {
        RequestInterstitial();
        spawnedTiles[1] = Instantiate(caveTile, caveTile.transform.position, Quaternion.Euler(0f, 90f, 0f));

        spawnedTiles[0] = Instantiate(caveTile, spawnedTiles[1].transform.position - new Vector3(0f, 0f, tileLength), Quaternion.Euler(0f, 90f, 0f));
        spawnedTiles[2] = Instantiate(caveTile, spawnedTiles[1].transform.position + new Vector3(0f, 0f, tileLength), Quaternion.Euler(0f, 90f, 0f));
        GameObject go = Instantiate(sphere, sphere.transform.position, Quaternion.identity);
        go.GetComponent<SimpleController>().DificultyLvl = dificultyLvl.value;
        go.GetComponent<SimpleController>().ScoreText = scoreText;

        
    }

    private float GetTileLength()
    {
        float leftPoint = caveTile.GetComponent<MeshFilter>().sharedMesh.vertices[0].x;

        foreach (Vector3 point in caveTile.GetComponent<MeshFilter>().sharedMesh.vertices)
        {
            if (point.x < leftPoint)
            {
                leftPoint = point.x;
            }
        }

        float rightPoint = caveTile.GetComponent<MeshFilter>().sharedMesh.vertices[0].x;
        foreach (Vector3 point in caveTile.GetComponent<MeshFilter>().sharedMesh.vertices)
        {
            if (point.x > rightPoint)
            {
                rightPoint = point.x;
            }
        }



        return Mathf.Abs(rightPoint - leftPoint);

    }


    private void OnGameOver(int score)
    {
        playmodeUI.SetActive(false);
        gameOverUI.SetActive(true);
        foreach (GameObject i in spawnedTiles)
        {
            if (i != null)
            {
                Destroy(i);
            }
        }
        gameOverUI.GetComponentsInChildren<Text>()[1].text = score.ToString();
        gameOverUI.GetComponentsInChildren<Button>()[0].onClick.RemoveAllListeners();
        gameOverUI.GetComponentsInChildren<Button>()[0].onClick.AddListener(delegate
        {
            gameOverUI.SetActive(false);
            welcominUI.SetActive(true);
        });

        gameOverUI.GetComponentsInChildren<Button>()[1].onClick.RemoveAllListeners();
        gameOverUI.GetComponentsInChildren<Button>()[1].onClick.AddListener(delegate
        {
            Application.Quit();
        });

        if (this.interstitial.IsLoaded())
        {
            this.interstitial.Show();
        }
    }





}
