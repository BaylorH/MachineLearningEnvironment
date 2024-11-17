using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using System.Collections.Generic;
using System.Collections;

public class KMeansPlusPlusPenguinDataVisualizer : MonoBehaviour
{
    public GameObject dataPointPrefab;
    public GameObject centroidPrefab;
    public Material Material1, Material2, Material3, Material4, Material5, Material6;
    public int numClusters = 3;
    public int maxIterations = 100;
    public float iterationDelay = 0.2f;

    private GameObject pointsContainer;
    private GameObject centroidsContainer;
    private List<Vector3> selectedCentroids = new List<Vector3>();

    private KMeansPlusPlus kmeansPlusPlus;

    private int iteration = 0;
    private int currentCluster = 0;

    public Button clusterButton2;
    public Button clusterButton3;
    public Button clusterButton4;
    public Button clusterButton5;
    public Button clusterButton6;

    public Color selectedColor = Color.white;
    public Color unselectedColor = Color.gray;

    public Button randomCentroidsButton;
    public Button restartButton;
    public Button chooseCentroidsButton;
    public Button skipToEndButton;

    public TMP_Text iterationText;
    public TMP_Text convergedText;
    public TMP_Text orText;
    public TMP_Text centroidsLeftText;

    private bool isChoosingCentroids = false;

    [System.Serializable]
    public class PenguinDataPoint
    {
        public float body_mass_g;
        public float bill_length_mm;
        public float bill_depth_mm;
        public float flipper_length_mm;
        public string species;

        public Vector3 ToVector3()
        {
            return new Vector3(bill_length_mm, bill_depth_mm, flipper_length_mm);
        }
    }

    public enum KMeansState
    {
        InitializeFirstCentroid,
        InitializeRestOfCentroids,
        AssignPoints,
        RecalculateCentroids
    }

    private KMeansState currentState;


    private PenguinDataPoint[] dataPoints = new PenguinDataPoint[] {
        new PenguinDataPoint { body_mass_g=3750f, bill_length_mm=39.1f, bill_depth_mm=18.7f, flipper_length_mm=181f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3800f, bill_length_mm=39.5f, bill_depth_mm=17.4f, flipper_length_mm=186f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3250f, bill_length_mm=40.3f, bill_depth_mm=18f, flipper_length_mm=195f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3450f, bill_length_mm=36.7f, bill_depth_mm=19.3f, flipper_length_mm=193f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3650f, bill_length_mm=39.3f, bill_depth_mm=20.6f, flipper_length_mm=190f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3625f, bill_length_mm=38.9f, bill_depth_mm=17.8f, flipper_length_mm=181f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=4675f, bill_length_mm=39.2f, bill_depth_mm=19.6f, flipper_length_mm=195f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3475f, bill_length_mm=34.1f, bill_depth_mm=18.1f, flipper_length_mm=193f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=4250f, bill_length_mm=42f, bill_depth_mm=20.2f, flipper_length_mm=190f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3300f, bill_length_mm=37.8f, bill_depth_mm=17.1f, flipper_length_mm=186f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3700f, bill_length_mm=37.8f, bill_depth_mm=17.3f, flipper_length_mm=180f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3200f, bill_length_mm=41.1f, bill_depth_mm=17.6f, flipper_length_mm=182f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3800f, bill_length_mm=38.6f, bill_depth_mm=21.2f, flipper_length_mm=191f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=4400f, bill_length_mm=34.6f, bill_depth_mm=21.1f, flipper_length_mm=198f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3700f, bill_length_mm=36.6f, bill_depth_mm=17.8f, flipper_length_mm=185f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3450f, bill_length_mm=38.7f, bill_depth_mm=19f, flipper_length_mm=195f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=4500f, bill_length_mm=42.5f, bill_depth_mm=20.7f, flipper_length_mm=197f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3325f, bill_length_mm=34.4f, bill_depth_mm=18.4f, flipper_length_mm=184f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=4200f, bill_length_mm=46f, bill_depth_mm=21.5f, flipper_length_mm=194f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3400f, bill_length_mm=37.8f, bill_depth_mm=18.3f, flipper_length_mm=174f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3600f, bill_length_mm=37.7f, bill_depth_mm=18.7f, flipper_length_mm=180f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3800f, bill_length_mm=35.9f, bill_depth_mm=19.2f, flipper_length_mm=189f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3950f, bill_length_mm=38.2f, bill_depth_mm=18.1f, flipper_length_mm=185f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3800f, bill_length_mm=38.8f, bill_depth_mm=17.2f, flipper_length_mm=180f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3800f, bill_length_mm=35.3f, bill_depth_mm=18.9f, flipper_length_mm=187f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3550f, bill_length_mm=40.6f, bill_depth_mm=18.6f, flipper_length_mm=183f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3200f, bill_length_mm=40.5f, bill_depth_mm=17.9f, flipper_length_mm=187f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3150f, bill_length_mm=37.9f, bill_depth_mm=18.6f, flipper_length_mm=172f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3950f, bill_length_mm=40.5f, bill_depth_mm=18.9f, flipper_length_mm=180f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3250f, bill_length_mm=39.5f, bill_depth_mm=16.7f, flipper_length_mm=178f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3900f, bill_length_mm=37.2f, bill_depth_mm=18.1f, flipper_length_mm=178f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3300f, bill_length_mm=39.5f, bill_depth_mm=17.8f, flipper_length_mm=188f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3900f, bill_length_mm=40.9f, bill_depth_mm=18.9f, flipper_length_mm=184f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3325f, bill_length_mm=36.4f, bill_depth_mm=17f, flipper_length_mm=195f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=4150f, bill_length_mm=39.2f, bill_depth_mm=21.1f, flipper_length_mm=196f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3950f, bill_length_mm=38.8f, bill_depth_mm=20f, flipper_length_mm=190f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3550f, bill_length_mm=42.2f, bill_depth_mm=18.5f, flipper_length_mm=180f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3300f, bill_length_mm=37.6f, bill_depth_mm=19.3f, flipper_length_mm=181f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=4650f, bill_length_mm=39.8f, bill_depth_mm=19.1f, flipper_length_mm=184f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3150f, bill_length_mm=36.5f, bill_depth_mm=18f, flipper_length_mm=182f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3900f, bill_length_mm=40.8f, bill_depth_mm=18.4f, flipper_length_mm=195f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3100f, bill_length_mm=36f, bill_depth_mm=18.5f, flipper_length_mm=186f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=4400f, bill_length_mm=44.1f, bill_depth_mm=19.7f, flipper_length_mm=196f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3000f, bill_length_mm=37f, bill_depth_mm=16.9f, flipper_length_mm=185f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=4600f, bill_length_mm=39.6f, bill_depth_mm=18.8f, flipper_length_mm=190f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3425f, bill_length_mm=41.1f, bill_depth_mm=19f, flipper_length_mm=182f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=2975f, bill_length_mm=37.5f, bill_depth_mm=18.9f, flipper_length_mm=179f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3450f, bill_length_mm=36f, bill_depth_mm=17.9f, flipper_length_mm=190f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=4150f, bill_length_mm=42.3f, bill_depth_mm=21.2f, flipper_length_mm=191f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3500f, bill_length_mm=39.6f, bill_depth_mm=17.7f, flipper_length_mm=186f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=4300f, bill_length_mm=40.1f, bill_depth_mm=18.9f, flipper_length_mm=188f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3450f, bill_length_mm=35f, bill_depth_mm=17.9f, flipper_length_mm=190f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=4050f, bill_length_mm=42f, bill_depth_mm=19.5f, flipper_length_mm=200f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=2900f, bill_length_mm=34.5f, bill_depth_mm=18.1f, flipper_length_mm=187f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3700f, bill_length_mm=41.4f, bill_depth_mm=18.6f, flipper_length_mm=191f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3550f, bill_length_mm=39f, bill_depth_mm=17.5f, flipper_length_mm=186f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3800f, bill_length_mm=40.6f, bill_depth_mm=18.8f, flipper_length_mm=193f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=2850f, bill_length_mm=36.5f, bill_depth_mm=16.6f, flipper_length_mm=181f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3750f, bill_length_mm=37.6f, bill_depth_mm=19.1f, flipper_length_mm=194f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3150f, bill_length_mm=35.7f, bill_depth_mm=16.9f, flipper_length_mm=185f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=4400f, bill_length_mm=41.3f, bill_depth_mm=21.1f, flipper_length_mm=195f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3600f, bill_length_mm=37.6f, bill_depth_mm=17f, flipper_length_mm=185f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=4050f, bill_length_mm=41.1f, bill_depth_mm=18.2f, flipper_length_mm=192f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=2850f, bill_length_mm=36.4f, bill_depth_mm=17.1f, flipper_length_mm=184f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3950f, bill_length_mm=41.6f, bill_depth_mm=18f, flipper_length_mm=192f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3350f, bill_length_mm=35.5f, bill_depth_mm=16.2f, flipper_length_mm=195f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=4100f, bill_length_mm=41.1f, bill_depth_mm=19.1f, flipper_length_mm=188f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3050f, bill_length_mm=35.9f, bill_depth_mm=16.6f, flipper_length_mm=190f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=4450f, bill_length_mm=41.8f, bill_depth_mm=19.4f, flipper_length_mm=198f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3600f, bill_length_mm=33.5f, bill_depth_mm=19f, flipper_length_mm=190f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3900f, bill_length_mm=39.7f, bill_depth_mm=18.4f, flipper_length_mm=190f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3550f, bill_length_mm=39.6f, bill_depth_mm=17.2f, flipper_length_mm=196f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=4150f, bill_length_mm=45.8f, bill_depth_mm=18.9f, flipper_length_mm=197f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3700f, bill_length_mm=35.5f, bill_depth_mm=17.5f, flipper_length_mm=190f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=4250f, bill_length_mm=42.8f, bill_depth_mm=18.5f, flipper_length_mm=195f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3700f, bill_length_mm=40.9f, bill_depth_mm=16.8f, flipper_length_mm=191f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3900f, bill_length_mm=37.2f, bill_depth_mm=19.4f, flipper_length_mm=184f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3550f, bill_length_mm=36.2f, bill_depth_mm=16.1f, flipper_length_mm=187f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=4000f, bill_length_mm=42.1f, bill_depth_mm=19.1f, flipper_length_mm=195f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3200f, bill_length_mm=34.6f, bill_depth_mm=17.2f, flipper_length_mm=189f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=4700f, bill_length_mm=42.9f, bill_depth_mm=17.6f, flipper_length_mm=196f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3800f, bill_length_mm=36.7f, bill_depth_mm=18.8f, flipper_length_mm=187f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=4200f, bill_length_mm=35.1f, bill_depth_mm=19.4f, flipper_length_mm=193f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3350f, bill_length_mm=37.3f, bill_depth_mm=17.8f, flipper_length_mm=191f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3550f, bill_length_mm=41.3f, bill_depth_mm=20.3f, flipper_length_mm=194f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3800f, bill_length_mm=36.3f, bill_depth_mm=19.5f, flipper_length_mm=190f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3500f, bill_length_mm=36.9f, bill_depth_mm=18.6f, flipper_length_mm=189f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3950f, bill_length_mm=38.3f, bill_depth_mm=19.2f, flipper_length_mm=189f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3600f, bill_length_mm=38.9f, bill_depth_mm=18.8f, flipper_length_mm=190f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3550f, bill_length_mm=35.7f, bill_depth_mm=18f, flipper_length_mm=202f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=4300f, bill_length_mm=41.1f, bill_depth_mm=18.1f, flipper_length_mm=205f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3400f, bill_length_mm=34f, bill_depth_mm=17.1f, flipper_length_mm=185f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=4450f, bill_length_mm=39.6f, bill_depth_mm=18.1f, flipper_length_mm=186f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3300f, bill_length_mm=36.2f, bill_depth_mm=17.3f, flipper_length_mm=187f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=4300f, bill_length_mm=40.8f, bill_depth_mm=18.9f, flipper_length_mm=208f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3700f, bill_length_mm=38.1f, bill_depth_mm=18.6f, flipper_length_mm=190f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=4350f, bill_length_mm=40.3f, bill_depth_mm=18.5f, flipper_length_mm=196f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=2900f, bill_length_mm=33.1f, bill_depth_mm=16.1f, flipper_length_mm=178f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=4100f, bill_length_mm=43.2f, bill_depth_mm=18.5f, flipper_length_mm=192f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3725f, bill_length_mm=35f, bill_depth_mm=17.9f, flipper_length_mm=192f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=4725f, bill_length_mm=41f, bill_depth_mm=20f, flipper_length_mm=203f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3075f, bill_length_mm=37.7f, bill_depth_mm=16f, flipper_length_mm=183f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=4250f, bill_length_mm=37.8f, bill_depth_mm=20f, flipper_length_mm=190f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=2925f, bill_length_mm=37.9f, bill_depth_mm=18.6f, flipper_length_mm=193f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3550f, bill_length_mm=39.7f, bill_depth_mm=18.9f, flipper_length_mm=184f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3750f, bill_length_mm=38.6f, bill_depth_mm=17.2f, flipper_length_mm=199f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3900f, bill_length_mm=38.2f, bill_depth_mm=20f, flipper_length_mm=190f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3175f, bill_length_mm=38.1f, bill_depth_mm=17f, flipper_length_mm=181f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=4775f, bill_length_mm=43.2f, bill_depth_mm=19f, flipper_length_mm=197f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3825f, bill_length_mm=38.1f, bill_depth_mm=16.5f, flipper_length_mm=198f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=4600f, bill_length_mm=45.6f, bill_depth_mm=20.3f, flipper_length_mm=191f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3200f, bill_length_mm=39.7f, bill_depth_mm=17.7f, flipper_length_mm=193f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=4275f, bill_length_mm=42.2f, bill_depth_mm=19.5f, flipper_length_mm=197f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3900f, bill_length_mm=39.6f, bill_depth_mm=20.7f, flipper_length_mm=191f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=4075f, bill_length_mm=42.7f, bill_depth_mm=18.3f, flipper_length_mm=196f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=2900f, bill_length_mm=38.6f, bill_depth_mm=17f, flipper_length_mm=188f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3775f, bill_length_mm=37.3f, bill_depth_mm=20.5f, flipper_length_mm=199f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3350f, bill_length_mm=35.7f, bill_depth_mm=17f, flipper_length_mm=189f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3325f, bill_length_mm=41.1f, bill_depth_mm=18.6f, flipper_length_mm=189f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3150f, bill_length_mm=36.2f, bill_depth_mm=17.2f, flipper_length_mm=187f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3500f, bill_length_mm=37.7f, bill_depth_mm=19.8f, flipper_length_mm=198f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3450f, bill_length_mm=40.2f, bill_depth_mm=17f, flipper_length_mm=176f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3875f, bill_length_mm=41.4f, bill_depth_mm=18.5f, flipper_length_mm=202f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3050f, bill_length_mm=35.2f, bill_depth_mm=15.9f, flipper_length_mm=186f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=4000f, bill_length_mm=40.6f, bill_depth_mm=19f, flipper_length_mm=199f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3275f, bill_length_mm=38.8f, bill_depth_mm=17.6f, flipper_length_mm=191f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=4300f, bill_length_mm=41.5f, bill_depth_mm=18.3f, flipper_length_mm=195f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3050f, bill_length_mm=39f, bill_depth_mm=17.1f, flipper_length_mm=191f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=4000f, bill_length_mm=44.1f, bill_depth_mm=18f, flipper_length_mm=210f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3325f, bill_length_mm=38.5f, bill_depth_mm=17.9f, flipper_length_mm=190f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3500f, bill_length_mm=43.1f, bill_depth_mm=19.2f, flipper_length_mm=197f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3500f, bill_length_mm=36.8f, bill_depth_mm=18.5f, flipper_length_mm=193f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=4475f, bill_length_mm=37.5f, bill_depth_mm=18.5f, flipper_length_mm=199f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3425f, bill_length_mm=38.1f, bill_depth_mm=17.6f, flipper_length_mm=187f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3900f, bill_length_mm=41.1f, bill_depth_mm=17.5f, flipper_length_mm=190f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3175f, bill_length_mm=35.6f, bill_depth_mm=17.5f, flipper_length_mm=191f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3975f, bill_length_mm=40.2f, bill_depth_mm=20.1f, flipper_length_mm=200f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3400f, bill_length_mm=37f, bill_depth_mm=16.5f, flipper_length_mm=185f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=4250f, bill_length_mm=39.7f, bill_depth_mm=17.9f, flipper_length_mm=193f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3400f, bill_length_mm=40.2f, bill_depth_mm=17.1f, flipper_length_mm=193f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3475f, bill_length_mm=40.6f, bill_depth_mm=17.2f, flipper_length_mm=187f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3050f, bill_length_mm=32.1f, bill_depth_mm=15.5f, flipper_length_mm=188f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3725f, bill_length_mm=40.7f, bill_depth_mm=17f, flipper_length_mm=190f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3000f, bill_length_mm=37.3f, bill_depth_mm=16.8f, flipper_length_mm=192f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3650f, bill_length_mm=39f, bill_depth_mm=18.7f, flipper_length_mm=185f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=4250f, bill_length_mm=39.2f, bill_depth_mm=18.6f, flipper_length_mm=190f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3475f, bill_length_mm=36.6f, bill_depth_mm=18.4f, flipper_length_mm=184f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3450f, bill_length_mm=36f, bill_depth_mm=17.8f, flipper_length_mm=195f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3750f, bill_length_mm=37.8f, bill_depth_mm=18.1f, flipper_length_mm=193f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=3700f, bill_length_mm=36f, bill_depth_mm=17.1f, flipper_length_mm=187f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=4000f, bill_length_mm=41.5f, bill_depth_mm=18.5f, flipper_length_mm=201f, species= "Adelie" },
        new PenguinDataPoint { body_mass_g=4500f, bill_length_mm=46.1f, bill_depth_mm=13.2f, flipper_length_mm=211f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=5700f, bill_length_mm=50f, bill_depth_mm=16.3f, flipper_length_mm=230f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=4450f, bill_length_mm=48.7f, bill_depth_mm=14.1f, flipper_length_mm=210f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=5700f, bill_length_mm=50f, bill_depth_mm=15.2f, flipper_length_mm=218f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=5400f, bill_length_mm=47.6f, bill_depth_mm=14.5f, flipper_length_mm=215f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=4550f, bill_length_mm=46.5f, bill_depth_mm=13.5f, flipper_length_mm=210f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=4800f, bill_length_mm=45.4f, bill_depth_mm=14.6f, flipper_length_mm=211f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=5200f, bill_length_mm=46.7f, bill_depth_mm=15.3f, flipper_length_mm=219f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=4400f, bill_length_mm=43.3f, bill_depth_mm=13.4f, flipper_length_mm=209f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=5150f, bill_length_mm=46.8f, bill_depth_mm=15.4f, flipper_length_mm=215f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=4650f, bill_length_mm=40.9f, bill_depth_mm=13.7f, flipper_length_mm=214f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=5550f, bill_length_mm=49f, bill_depth_mm=16.1f, flipper_length_mm=216f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=4650f, bill_length_mm=45.5f, bill_depth_mm=13.7f, flipper_length_mm=214f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=5850f, bill_length_mm=48.4f, bill_depth_mm=14.6f, flipper_length_mm=213f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=4200f, bill_length_mm=45.8f, bill_depth_mm=14.6f, flipper_length_mm=210f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=5850f, bill_length_mm=49.3f, bill_depth_mm=15.7f, flipper_length_mm=217f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=4150f, bill_length_mm=42f, bill_depth_mm=13.5f, flipper_length_mm=210f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=6300f, bill_length_mm=49.2f, bill_depth_mm=15.2f, flipper_length_mm=221f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=4800f, bill_length_mm=46.2f, bill_depth_mm=14.5f, flipper_length_mm=209f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=5350f, bill_length_mm=48.7f, bill_depth_mm=15.1f, flipper_length_mm=222f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=5700f, bill_length_mm=50.2f, bill_depth_mm=14.3f, flipper_length_mm=218f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=5000f, bill_length_mm=45.1f, bill_depth_mm=14.5f, flipper_length_mm=215f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=4400f, bill_length_mm=46.5f, bill_depth_mm=14.5f, flipper_length_mm=213f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=5050f, bill_length_mm=46.3f, bill_depth_mm=15.8f, flipper_length_mm=215f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=5000f, bill_length_mm=42.9f, bill_depth_mm=13.1f, flipper_length_mm=215f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=5100f, bill_length_mm=46.1f, bill_depth_mm=15.1f, flipper_length_mm=215f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=4100f, bill_length_mm=44.5f, bill_depth_mm=14.3f, flipper_length_mm=216f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=5650f, bill_length_mm=47.8f, bill_depth_mm=15f, flipper_length_mm=215f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=4600f, bill_length_mm=48.2f, bill_depth_mm=14.3f, flipper_length_mm=210f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=5550f, bill_length_mm=50f, bill_depth_mm=15.3f, flipper_length_mm=220f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=5250f, bill_length_mm=47.3f, bill_depth_mm=15.3f, flipper_length_mm=222f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=4700f, bill_length_mm=42.8f, bill_depth_mm=14.2f, flipper_length_mm=209f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=5050f, bill_length_mm=45.1f, bill_depth_mm=14.5f, flipper_length_mm=207f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=6050f, bill_length_mm=59.6f, bill_depth_mm=17f, flipper_length_mm=230f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=5150f, bill_length_mm=49.1f, bill_depth_mm=14.8f, flipper_length_mm=220f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=5400f, bill_length_mm=48.4f, bill_depth_mm=16.3f, flipper_length_mm=220f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=4950f, bill_length_mm=42.6f, bill_depth_mm=13.7f, flipper_length_mm=213f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=5250f, bill_length_mm=44.4f, bill_depth_mm=17.3f, flipper_length_mm=219f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=4350f, bill_length_mm=44f, bill_depth_mm=13.6f, flipper_length_mm=208f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=5350f, bill_length_mm=48.7f, bill_depth_mm=15.7f, flipper_length_mm=208f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=3950f, bill_length_mm=42.7f, bill_depth_mm=13.7f, flipper_length_mm=208f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=5700f, bill_length_mm=49.6f, bill_depth_mm=16f, flipper_length_mm=225f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=4300f, bill_length_mm=45.3f, bill_depth_mm=13.7f, flipper_length_mm=210f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=4750f, bill_length_mm=49.6f, bill_depth_mm=15f, flipper_length_mm=216f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=5550f, bill_length_mm=50.5f, bill_depth_mm=15.9f, flipper_length_mm=222f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=4900f, bill_length_mm=43.6f, bill_depth_mm=13.9f, flipper_length_mm=217f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=4200f, bill_length_mm=45.5f, bill_depth_mm=13.9f, flipper_length_mm=210f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=5400f, bill_length_mm=50.5f, bill_depth_mm=15.9f, flipper_length_mm=225f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=5100f, bill_length_mm=44.9f, bill_depth_mm=13.3f, flipper_length_mm=213f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=5300f, bill_length_mm=45.2f, bill_depth_mm=15.8f, flipper_length_mm=215f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=4850f, bill_length_mm=46.6f, bill_depth_mm=14.2f, flipper_length_mm=210f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=5300f, bill_length_mm=48.5f, bill_depth_mm=14.1f, flipper_length_mm=220f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=4400f, bill_length_mm=45.1f, bill_depth_mm=14.4f, flipper_length_mm=210f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=5000f, bill_length_mm=50.1f, bill_depth_mm=15f, flipper_length_mm=225f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=4900f, bill_length_mm=46.5f, bill_depth_mm=14.4f, flipper_length_mm=217f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=5050f, bill_length_mm=45f, bill_depth_mm=15.4f, flipper_length_mm=220f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=4300f, bill_length_mm=43.8f, bill_depth_mm=13.9f, flipper_length_mm=208f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=5000f, bill_length_mm=45.5f, bill_depth_mm=15f, flipper_length_mm=220f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=4450f, bill_length_mm=43.2f, bill_depth_mm=14.5f, flipper_length_mm=208f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=5550f, bill_length_mm=50.4f, bill_depth_mm=15.3f, flipper_length_mm=224f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=4200f, bill_length_mm=45.3f, bill_depth_mm=13.8f, flipper_length_mm=208f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=5300f, bill_length_mm=46.2f, bill_depth_mm=14.9f, flipper_length_mm=221f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=4400f, bill_length_mm=45.7f, bill_depth_mm=13.9f, flipper_length_mm=214f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=5650f, bill_length_mm=54.3f, bill_depth_mm=15.7f, flipper_length_mm=231f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=4700f, bill_length_mm=45.8f, bill_depth_mm=14.2f, flipper_length_mm=219f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=5700f, bill_length_mm=49.8f, bill_depth_mm=16.8f, flipper_length_mm=230f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=4650f, bill_length_mm=46.2f, bill_depth_mm=14.4f, flipper_length_mm=214f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=5800f, bill_length_mm=49.5f, bill_depth_mm=16.2f, flipper_length_mm=229f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=4700f, bill_length_mm=43.5f, bill_depth_mm=14.2f, flipper_length_mm=220f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=5550f, bill_length_mm=50.7f, bill_depth_mm=15f, flipper_length_mm=223f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=4750f, bill_length_mm=47.7f, bill_depth_mm=15f, flipper_length_mm=216f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=5000f, bill_length_mm=46.4f, bill_depth_mm=15.6f, flipper_length_mm=221f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=5100f, bill_length_mm=48.2f, bill_depth_mm=15.6f, flipper_length_mm=221f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=5200f, bill_length_mm=46.5f, bill_depth_mm=14.8f, flipper_length_mm=217f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=4700f, bill_length_mm=46.4f, bill_depth_mm=15f, flipper_length_mm=216f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=5800f, bill_length_mm=48.6f, bill_depth_mm=16f, flipper_length_mm=230f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=4600f, bill_length_mm=47.5f, bill_depth_mm=14.2f, flipper_length_mm=209f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=6000f, bill_length_mm=51.1f, bill_depth_mm=16.3f, flipper_length_mm=220f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=4750f, bill_length_mm=45.2f, bill_depth_mm=13.8f, flipper_length_mm=215f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=5950f, bill_length_mm=45.2f, bill_depth_mm=16.4f, flipper_length_mm=223f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=4625f, bill_length_mm=49.1f, bill_depth_mm=14.5f, flipper_length_mm=212f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=5450f, bill_length_mm=52.5f, bill_depth_mm=15.6f, flipper_length_mm=221f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=4725f, bill_length_mm=47.4f, bill_depth_mm=14.6f, flipper_length_mm=212f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=5350f, bill_length_mm=50f, bill_depth_mm=15.9f, flipper_length_mm=224f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=4750f, bill_length_mm=44.9f, bill_depth_mm=13.8f, flipper_length_mm=212f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=5600f, bill_length_mm=50.8f, bill_depth_mm=17.3f, flipper_length_mm=228f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=4600f, bill_length_mm=43.4f, bill_depth_mm=14.4f, flipper_length_mm=218f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=5300f, bill_length_mm=51.3f, bill_depth_mm=14.2f, flipper_length_mm=218f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=4875f, bill_length_mm=47.5f, bill_depth_mm=14f, flipper_length_mm=212f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=5550f, bill_length_mm=52.1f, bill_depth_mm=17f, flipper_length_mm=230f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=4950f, bill_length_mm=47.5f, bill_depth_mm=15f, flipper_length_mm=218f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=5400f, bill_length_mm=52.2f, bill_depth_mm=17.1f, flipper_length_mm=228f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=4750f, bill_length_mm=45.5f, bill_depth_mm=14.5f, flipper_length_mm=212f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=5650f, bill_length_mm=49.5f, bill_depth_mm=16.1f, flipper_length_mm=224f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=4850f, bill_length_mm=44.5f, bill_depth_mm=14.7f, flipper_length_mm=214f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=5200f, bill_length_mm=50.8f, bill_depth_mm=15.7f, flipper_length_mm=226f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=4925f, bill_length_mm=49.4f, bill_depth_mm=15.8f, flipper_length_mm=216f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=4875f, bill_length_mm=46.9f, bill_depth_mm=14.6f, flipper_length_mm=222f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=4625f, bill_length_mm=48.4f, bill_depth_mm=14.4f, flipper_length_mm=203f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=5250f, bill_length_mm=51.1f, bill_depth_mm=16.5f, flipper_length_mm=225f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=4850f, bill_length_mm=48.5f, bill_depth_mm=15f, flipper_length_mm=219f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=5600f, bill_length_mm=55.9f, bill_depth_mm=17f, flipper_length_mm=228f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=4975f, bill_length_mm=47.2f, bill_depth_mm=15.5f, flipper_length_mm=215f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=5500f, bill_length_mm=49.1f, bill_depth_mm=15f, flipper_length_mm=228f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=4725f, bill_length_mm=47.3f, bill_depth_mm=13.8f, flipper_length_mm=216f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=5500f, bill_length_mm=46.8f, bill_depth_mm=16.1f, flipper_length_mm=215f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=4700f, bill_length_mm=41.7f, bill_depth_mm=14.7f, flipper_length_mm=210f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=5500f, bill_length_mm=53.4f, bill_depth_mm=15.8f, flipper_length_mm=219f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=4575f, bill_length_mm=43.3f, bill_depth_mm=14f, flipper_length_mm=208f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=5500f, bill_length_mm=48.1f, bill_depth_mm=15.1f, flipper_length_mm=209f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=5000f, bill_length_mm=50.5f, bill_depth_mm=15.2f, flipper_length_mm=216f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=5950f, bill_length_mm=49.8f, bill_depth_mm=15.9f, flipper_length_mm=229f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=4650f, bill_length_mm=43.5f, bill_depth_mm=15.2f, flipper_length_mm=213f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=5500f, bill_length_mm=51.5f, bill_depth_mm=16.3f, flipper_length_mm=230f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=4375f, bill_length_mm=46.2f, bill_depth_mm=14.1f, flipper_length_mm=217f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=5850f, bill_length_mm=55.1f, bill_depth_mm=16f, flipper_length_mm=230f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=4875f, bill_length_mm=44.5f, bill_depth_mm=15.7f, flipper_length_mm=217f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=6000f, bill_length_mm=48.8f, bill_depth_mm=16.2f, flipper_length_mm=222f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=4925f, bill_length_mm=47.2f, bill_depth_mm=13.7f, flipper_length_mm=214f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=4850f, bill_length_mm=46.8f, bill_depth_mm=14.3f, flipper_length_mm=215f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=5750f, bill_length_mm=50.4f, bill_depth_mm=15.7f, flipper_length_mm=222f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=5200f, bill_length_mm=45.2f, bill_depth_mm=14.8f, flipper_length_mm=212f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=5400f, bill_length_mm=49.9f, bill_depth_mm=16.1f, flipper_length_mm=213f, species= "Gentoo" },
        new PenguinDataPoint { body_mass_g=3500f, bill_length_mm=46.5f, bill_depth_mm=17.9f, flipper_length_mm=192f, species= "Chinstrap" },
        new PenguinDataPoint { body_mass_g=3900f, bill_length_mm=50f, bill_depth_mm=19.5f, flipper_length_mm=196f, species= "Chinstrap" },
        new PenguinDataPoint { body_mass_g=3650f, bill_length_mm=51.3f, bill_depth_mm=19.2f, flipper_length_mm=193f, species= "Chinstrap" },
        new PenguinDataPoint { body_mass_g=3525f, bill_length_mm=45.4f, bill_depth_mm=18.7f, flipper_length_mm=188f, species= "Chinstrap" },
        new PenguinDataPoint { body_mass_g=3725f, bill_length_mm=52.7f, bill_depth_mm=19.8f, flipper_length_mm=197f, species= "Chinstrap" },
        new PenguinDataPoint { body_mass_g=3950f, bill_length_mm=45.2f, bill_depth_mm=17.8f, flipper_length_mm=198f, species= "Chinstrap" },
        new PenguinDataPoint { body_mass_g=3250f, bill_length_mm=46.1f, bill_depth_mm=18.2f, flipper_length_mm=178f, species= "Chinstrap" },
        new PenguinDataPoint { body_mass_g=3750f, bill_length_mm=51.3f, bill_depth_mm=18.2f, flipper_length_mm=197f, species= "Chinstrap" },
        new PenguinDataPoint { body_mass_g=4150f, bill_length_mm=46f, bill_depth_mm=18.9f, flipper_length_mm=195f, species= "Chinstrap" },
        new PenguinDataPoint { body_mass_g=3700f, bill_length_mm=51.3f, bill_depth_mm=19.9f, flipper_length_mm=198f, species= "Chinstrap" },
        new PenguinDataPoint { body_mass_g=3800f, bill_length_mm=46.6f, bill_depth_mm=17.8f, flipper_length_mm=193f, species= "Chinstrap" },
        new PenguinDataPoint { body_mass_g=3775f, bill_length_mm=51.7f, bill_depth_mm=20.3f, flipper_length_mm=194f, species= "Chinstrap" },
        new PenguinDataPoint { body_mass_g=3700f, bill_length_mm=47f, bill_depth_mm=17.3f, flipper_length_mm=185f, species= "Chinstrap" },
        new PenguinDataPoint { body_mass_g=4050f, bill_length_mm=52f, bill_depth_mm=18.1f, flipper_length_mm=201f, species= "Chinstrap" },
        new PenguinDataPoint { body_mass_g=3575f, bill_length_mm=45.9f, bill_depth_mm=17.1f, flipper_length_mm=190f, species= "Chinstrap" },
        new PenguinDataPoint { body_mass_g=4050f, bill_length_mm=50.5f, bill_depth_mm=19.6f, flipper_length_mm=201f, species= "Chinstrap" },
        new PenguinDataPoint { body_mass_g=3300f, bill_length_mm=50.3f, bill_depth_mm=20f, flipper_length_mm=197f, species= "Chinstrap" },
        new PenguinDataPoint { body_mass_g=3700f, bill_length_mm=58f, bill_depth_mm=17.8f, flipper_length_mm=181f, species= "Chinstrap" },
        new PenguinDataPoint { body_mass_g=3450f, bill_length_mm=46.4f, bill_depth_mm=18.6f, flipper_length_mm=190f, species= "Chinstrap" },
        new PenguinDataPoint { body_mass_g=4400f, bill_length_mm=49.2f, bill_depth_mm=18.2f, flipper_length_mm=195f, species= "Chinstrap" },
        new PenguinDataPoint { body_mass_g=3600f, bill_length_mm=42.4f, bill_depth_mm=17.3f, flipper_length_mm=181f, species= "Chinstrap" },
        new PenguinDataPoint { body_mass_g=3400f, bill_length_mm=48.5f, bill_depth_mm=17.5f, flipper_length_mm=191f, species= "Chinstrap" },
        new PenguinDataPoint { body_mass_g=2900f, bill_length_mm=43.2f, bill_depth_mm=16.6f, flipper_length_mm=187f, species= "Chinstrap" },
        new PenguinDataPoint { body_mass_g=3800f, bill_length_mm=50.6f, bill_depth_mm=19.4f, flipper_length_mm=193f, species= "Chinstrap" },
        new PenguinDataPoint { body_mass_g=3300f, bill_length_mm=46.7f, bill_depth_mm=17.9f, flipper_length_mm=195f, species= "Chinstrap" },
        new PenguinDataPoint { body_mass_g=4150f, bill_length_mm=52f, bill_depth_mm=19f, flipper_length_mm=197f, species= "Chinstrap" },
        new PenguinDataPoint { body_mass_g=3400f, bill_length_mm=50.5f, bill_depth_mm=18.4f, flipper_length_mm=200f, species= "Chinstrap" },
        new PenguinDataPoint { body_mass_g=3800f, bill_length_mm=49.5f, bill_depth_mm=19f, flipper_length_mm=200f, species= "Chinstrap" },
        new PenguinDataPoint { body_mass_g=3700f, bill_length_mm=46.4f, bill_depth_mm=17.8f, flipper_length_mm=191f, species= "Chinstrap" },
        new PenguinDataPoint { body_mass_g=4550f, bill_length_mm=52.8f, bill_depth_mm=20f, flipper_length_mm=205f, species= "Chinstrap" },
        new PenguinDataPoint { body_mass_g=3200f, bill_length_mm=40.9f, bill_depth_mm=16.6f, flipper_length_mm=187f, species= "Chinstrap" },
        new PenguinDataPoint { body_mass_g=4300f, bill_length_mm=54.2f, bill_depth_mm=20.8f, flipper_length_mm=201f, species= "Chinstrap" },
        new PenguinDataPoint { body_mass_g=3350f, bill_length_mm=42.5f, bill_depth_mm=16.7f, flipper_length_mm=187f, species= "Chinstrap" },
        new PenguinDataPoint { body_mass_g=4100f, bill_length_mm=51f, bill_depth_mm=18.8f, flipper_length_mm=203f, species= "Chinstrap" },
        new PenguinDataPoint { body_mass_g=3600f, bill_length_mm=49.7f, bill_depth_mm=18.6f, flipper_length_mm=195f, species= "Chinstrap" },
        new PenguinDataPoint { body_mass_g=3900f, bill_length_mm=47.5f, bill_depth_mm=16.8f, flipper_length_mm=199f, species= "Chinstrap" },
        new PenguinDataPoint { body_mass_g=3850f, bill_length_mm=47.6f, bill_depth_mm=18.3f, flipper_length_mm=195f, species= "Chinstrap" },
        new PenguinDataPoint { body_mass_g=4800f, bill_length_mm=52f, bill_depth_mm=20.7f, flipper_length_mm=210f, species= "Chinstrap" },
        new PenguinDataPoint { body_mass_g=2700f, bill_length_mm=46.9f, bill_depth_mm=16.6f, flipper_length_mm=192f, species= "Chinstrap" },
        new PenguinDataPoint { body_mass_g=4500f, bill_length_mm=53.5f, bill_depth_mm=19.9f, flipper_length_mm=205f, species= "Chinstrap" },
        new PenguinDataPoint { body_mass_g=3950f, bill_length_mm=49f, bill_depth_mm=19.5f, flipper_length_mm=210f, species= "Chinstrap" },
        new PenguinDataPoint { body_mass_g=3650f, bill_length_mm=46.2f, bill_depth_mm=17.5f, flipper_length_mm=187f, species= "Chinstrap" },
        new PenguinDataPoint { body_mass_g=3550f, bill_length_mm=50.9f, bill_depth_mm=19.1f, flipper_length_mm=196f, species= "Chinstrap" },
        new PenguinDataPoint { body_mass_g=3500f, bill_length_mm=45.5f, bill_depth_mm=17f, flipper_length_mm=196f, species= "Chinstrap" },
        new PenguinDataPoint { body_mass_g=3675f, bill_length_mm=50.9f, bill_depth_mm=17.9f, flipper_length_mm=196f, species= "Chinstrap" },
        new PenguinDataPoint { body_mass_g=4450f, bill_length_mm=50.8f, bill_depth_mm=18.5f, flipper_length_mm=201f, species= "Chinstrap" },
        new PenguinDataPoint { body_mass_g=3400f, bill_length_mm=50.1f, bill_depth_mm=17.9f, flipper_length_mm=190f, species= "Chinstrap" },
        new PenguinDataPoint { body_mass_g=4300f, bill_length_mm=49f, bill_depth_mm=19.6f, flipper_length_mm=212f, species= "Chinstrap" },
        new PenguinDataPoint { body_mass_g=3250f, bill_length_mm=51.5f, bill_depth_mm=18.7f, flipper_length_mm=187f, species= "Chinstrap" },
        new PenguinDataPoint { body_mass_g=3675f, bill_length_mm=49.8f, bill_depth_mm=17.3f, flipper_length_mm=198f, species= "Chinstrap" },
        new PenguinDataPoint { body_mass_g=3325f, bill_length_mm=48.1f, bill_depth_mm=16.4f, flipper_length_mm=199f, species= "Chinstrap" },
        new PenguinDataPoint { body_mass_g=3950f, bill_length_mm=51.4f, bill_depth_mm=19f, flipper_length_mm=201f, species= "Chinstrap" },
        new PenguinDataPoint { body_mass_g=3600f, bill_length_mm=45.7f, bill_depth_mm=17.3f, flipper_length_mm=193f, species= "Chinstrap" },
        new PenguinDataPoint { body_mass_g=4050f, bill_length_mm=50.7f, bill_depth_mm=19.7f, flipper_length_mm=203f, species= "Chinstrap" },
        new PenguinDataPoint { body_mass_g=3350f, bill_length_mm=42.5f, bill_depth_mm=17.3f, flipper_length_mm=187f, species= "Chinstrap" },
        new PenguinDataPoint { body_mass_g=3450f, bill_length_mm=52.2f, bill_depth_mm=18.8f, flipper_length_mm=197f, species= "Chinstrap" },
        new PenguinDataPoint { body_mass_g=3250f, bill_length_mm=45.2f, bill_depth_mm=16.6f, flipper_length_mm=191f, species= "Chinstrap" },
        new PenguinDataPoint { body_mass_g=4050f, bill_length_mm=49.3f, bill_depth_mm=19.9f, flipper_length_mm=203f, species= "Chinstrap" },
        new PenguinDataPoint { body_mass_g=3800f, bill_length_mm=50.2f, bill_depth_mm=18.8f, flipper_length_mm=202f, species= "Chinstrap" },
        new PenguinDataPoint { body_mass_g=3525f, bill_length_mm=45.6f, bill_depth_mm=19.4f, flipper_length_mm=194f, species= "Chinstrap" },
        new PenguinDataPoint { body_mass_g=3950f, bill_length_mm=51.9f, bill_depth_mm=19.5f, flipper_length_mm=206f, species= "Chinstrap" },
        new PenguinDataPoint { body_mass_g=3650f, bill_length_mm=46.8f, bill_depth_mm=16.5f, flipper_length_mm=189f, species= "Chinstrap" },
        new PenguinDataPoint { body_mass_g=3650f, bill_length_mm=45.7f, bill_depth_mm=17f, flipper_length_mm=195f, species= "Chinstrap" },
        new PenguinDataPoint { body_mass_g=4000f, bill_length_mm=55.8f, bill_depth_mm=19.8f, flipper_length_mm=207f, species= "Chinstrap" },
        new PenguinDataPoint { body_mass_g=3400f, bill_length_mm=43.5f, bill_depth_mm=18.1f, flipper_length_mm=202f, species= "Chinstrap" },
        new PenguinDataPoint { body_mass_g=3775f, bill_length_mm=49.6f, bill_depth_mm=18.2f, flipper_length_mm=193f, species= "Chinstrap" },
        new PenguinDataPoint { body_mass_g=4100f, bill_length_mm=50.8f, bill_depth_mm=19f, flipper_length_mm=210f, species= "Chinstrap" },
        new PenguinDataPoint { body_mass_g=3775f, bill_length_mm=50.2f, bill_depth_mm=18.7f, flipper_length_mm=198f, species= "Chinstrap" },
    };

    void Start()
    {
        pointsContainer = new GameObject("PointsContainer");
        centroidsContainer = new GameObject("CentroidsContainer");
        SetupDataPoints();

        kmeansPlusPlus = new KMeansPlusPlus(numClusters, maxIterations, dataPoints.ToList());

        currentState = KMeansState.InitializeFirstCentroid;
        randomCentroidsButton.GetComponentInChildren<TMP_Text>().text = "Random First Centroid";

        convergedText.text = "";

        randomCentroidsButton.onClick.AddListener(OnRandomButtonClick);
        chooseCentroidsButton.onClick.AddListener(OnChooseCentroidsButtonClick);
        skipToEndButton.onClick.AddListener(OnSkipToEndButtonClick);

        restartButton.gameObject.SetActive(false);
        restartButton.onClick.AddListener(OnRestartButtonClick);

        skipToEndButton.gameObject.SetActive(false);

        clusterButton2.onClick.AddListener(() => OnClusterButtonClick(2));
        clusterButton3.onClick.AddListener(() => OnClusterButtonClick(3));
        clusterButton4.onClick.AddListener(() => OnClusterButtonClick(4));
        clusterButton5.onClick.AddListener(() => OnClusterButtonClick(5));
        clusterButton6.onClick.AddListener(() => OnClusterButtonClick(6));

        UpdateClusterButtonVisuals();
    }

    // set up and plot initial data points
    void SetupDataPoints()
    {
        // normalize and plot points based on attributes
        float scaler = 80f;
        foreach (PenguinDataPoint point in dataPoints)
        {
            Vector3 position = NormalizeAndScalePoint(point, scaler);
            CreateSphere(position, point.body_mass_g);
        }
    }

    // normalize penguin data
    Vector3 NormalizeAndScalePoint(PenguinDataPoint point, float scaler)
    {
        float xMin = dataPoints.Min(p => p.bill_length_mm), xMax = dataPoints.Max(p => p.bill_length_mm);
        float yMin = dataPoints.Min(p => p.bill_depth_mm), yMax = dataPoints.Max(p => p.bill_depth_mm);
        float zMin = dataPoints.Min(p => p.flipper_length_mm), zMax = dataPoints.Max(p => p.flipper_length_mm);

        float normalizedBillLength = (point.bill_length_mm - xMin) / (xMax - xMin);
        float normalizedBillDepth = (point.bill_depth_mm - yMin) / (yMax - yMin);
        float normalizedFlipperLength = (point.flipper_length_mm - zMin) / (zMax - zMin);

        return new Vector3(
            normalizedBillLength * scaler + 720,
            normalizedBillDepth * scaler + 547,
            -normalizedFlipperLength * scaler + 56
        );
    }

    // reverse above
    Vector3 ReverseNormalizeAndScalePoint(Vector3 normalizedPoint, float scaler)
    {
        // Get min and max values from data points for each attribute
        float xMin = dataPoints.Min(p => p.bill_length_mm), xMax = dataPoints.Max(p => p.bill_length_mm);
        float yMin = dataPoints.Min(p => p.bill_depth_mm), yMax = dataPoints.Max(p => p.bill_depth_mm);
        float zMin = dataPoints.Min(p => p.flipper_length_mm), zMax = dataPoints.Max(p => p.flipper_length_mm);

        // Reverse the normalization and scaling for each coordinate
        float originalBillLength = ((normalizedPoint.x - 720) / scaler) * (xMax - xMin) + xMin;
        float originalBillDepth = ((normalizedPoint.y - 547) / scaler) * (yMax - yMin) + yMin;
        float originalFlipperLength = ((-normalizedPoint.z + 56) / scaler) * (zMax - zMin) + zMin;

        return new Vector3(originalBillLength, originalBillDepth, originalFlipperLength);
    }


    // create a sphere for each penguin data point
    void CreateSphere(Vector3 position, float bodyMass)
    {
        GameObject sphere = Instantiate(dataPointPrefab, position, Quaternion.Euler(-90, 0, 0));
        sphere.transform.localScale = Vector3.one * (1.0f + bodyMass * 0.0005f);
        sphere.transform.SetParent(pointsContainer.transform);
        sphere.tag = "DataPoint";

        // collider to detect clicks
        if (sphere.GetComponent<Collider>() == null)
        {
            sphere.AddComponent<SphereCollider>();
        }
    }

    void OnRandomButtonClick()
    {
        RectTransform buttonRectTransform = randomCentroidsButton.GetComponent<RectTransform>();

        switch (currentState)
        {
            case KMeansState.InitializeFirstCentroid:
                PlaceFirstCentroid();
                orText.gameObject.SetActive(false);
                currentState = KMeansState.InitializeRestOfCentroids;
                randomCentroidsButton.GetComponentInChildren<TMP_Text>().text = "Initialize\nRest of\nCentroids";
                chooseCentroidsButton.gameObject.SetActive(false);
                // adjust width
                float preferredWidth = randomCentroidsButton.GetComponentInChildren<TMP_Text>().preferredWidth;
                //buttonRectTransform.sizeDelta = new Vector2(preferredWidth + 40f, buttonRectTransform.sizeDelta.y);
                break;

            case KMeansState.InitializeRestOfCentroids:
                PlaceRestOfCentroidsMaxDist();
                currentState = KMeansState.AssignPoints;
                randomCentroidsButton.GetComponentInChildren<TMP_Text>().text = "Assign\nPoints";
                skipToEndButton.gameObject.SetActive(true);
                skipToEndButton.interactable = true;
                // adjust width
                preferredWidth = randomCentroidsButton.GetComponentInChildren<TMP_Text>().preferredWidth;
                //buttonRectTransform.sizeDelta = new Vector2(preferredWidth + 40f, buttonRectTransform.sizeDelta.y);
                break;

            case KMeansState.AssignPoints:
                AssignPointsToCentroids();
                currentState = KMeansState.RecalculateCentroids;
                randomCentroidsButton.GetComponentInChildren<TMP_Text>().text = "Recalculate\nCentroids";
                // adjust width
                preferredWidth = randomCentroidsButton.GetComponentInChildren<TMP_Text>().preferredWidth;
                //buttonRectTransform.sizeDelta = new Vector2(preferredWidth + 40f, buttonRectTransform.sizeDelta.y);
                iteration++;
                iterationText.text = "Iteration #" + iteration;
                break;

            case KMeansState.RecalculateCentroids:
                RecalculateCentroids();

                if (kmeansPlusPlus.IsConverged())
                {
                    convergedText.text = "Converged";
                    // adjust width
                    preferredWidth = randomCentroidsButton.GetComponentInChildren<TMP_Text>().preferredWidth;
                    //buttonRectTransform.sizeDelta = new Vector2(preferredWidth + 40f, buttonRectTransform.sizeDelta.y);
                    randomCentroidsButton.interactable = false;  // disable the button to stop further recalculations
                    restartButton.gameObject.SetActive(true); // restart button
                    skipToEndButton.gameObject.SetActive(false);
                }
                else
                {
                    currentState = KMeansState.AssignPoints;
                    randomCentroidsButton.GetComponentInChildren<TMP_Text>().text = "Assign\nPoints";
                    // adjust width
                    preferredWidth = randomCentroidsButton.GetComponentInChildren<TMP_Text>().preferredWidth;
                    //buttonRectTransform.sizeDelta = new Vector2(preferredWidth + 40f, buttonRectTransform.sizeDelta.y);
                }
                break;
        }
    }


    void OnRestartButtonClick()
    {
        convergedText.text = "";
        iteration = 0;
        currentCluster = 0;
        isChoosingCentroids = false;
        iterationText.text = "Iteration #0";
        restartButton.gameObject.SetActive(false);
        randomCentroidsButton.gameObject.SetActive(true);
        chooseCentroidsButton.gameObject.SetActive(true);
        orText.gameObject.SetActive(true);
        randomCentroidsButton.interactable = true;
        chooseCentroidsButton.interactable = true;
        chooseCentroidsButton.GetComponent<Image>().color = selectedColor;
        randomCentroidsButton.GetComponent<Image>().color = selectedColor;
        selectedCentroids.Clear();

        // Re-initialize KMeans
        kmeansPlusPlus.InitializeFirstCentroid();
        currentState = KMeansState.InitializeFirstCentroid;
        chooseCentroidsButton.GetComponentInChildren<TMP_Text>().text = "User Choose\nFirst Centroid";
        randomCentroidsButton.GetComponentInChildren<TMP_Text>().text = "Random\nFirst\nCentroid";
        // adjust width
        float preferredWidth = randomCentroidsButton.GetComponentInChildren<TMP_Text>().preferredWidth;
        RectTransform buttonRectTransform = randomCentroidsButton.GetComponent<RectTransform>();
        //buttonRectTransform.sizeDelta = new Vector2(preferredWidth + 40f, buttonRectTransform.sizeDelta.y);

        ClearPreviousDataPoints();
        ClearPreviousCentroids();
        SetupDataPoints();
    }

    void OnClusterButtonClick(int selectedCluster)
    {
        numClusters = selectedCluster;
        kmeansPlusPlus = new KMeansPlusPlus(numClusters, maxIterations, dataPoints.ToList());  // reinitialize KMeans with new cluster count

        convergedText.text = "";
        centroidsLeftText.text = "";
        iteration = 0;
        currentCluster = 0;
        iterationText.text = "Iteration #0";
        restartButton.gameObject.SetActive(false);
        chooseCentroidsButton.gameObject.SetActive(true);
        randomCentroidsButton.gameObject.SetActive(true);
        skipToEndButton.gameObject.SetActive(false);
        randomCentroidsButton.interactable = true;
        chooseCentroidsButton.interactable = true;
        currentState = KMeansState.InitializeFirstCentroid;
        randomCentroidsButton.GetComponentInChildren<TMP_Text>().text = "Random\nFirst\nCentroid";
        chooseCentroidsButton.GetComponentInChildren<TMP_Text>().text = "User Choose\nFirst Centroid";
        selectedCentroids.Clear();        
        // width
        float preferredWidth = randomCentroidsButton.GetComponentInChildren<TMP_Text>().preferredWidth;
        RectTransform buttonRectTransform = randomCentroidsButton.GetComponent<RectTransform>();
        //buttonRectTransform.sizeDelta = new Vector2(preferredWidth + 40f, buttonRectTransform.sizeDelta.y);


        ClearPreviousDataPoints();
        ClearPreviousCentroids();
        SetupDataPoints();

        UpdateClusterButtonVisuals();  // update button visuals to reflect the new selection
    }

    void UpdateClusterButtonVisuals()
    {
        clusterButton2.GetComponent<Image>().color = unselectedColor;
        clusterButton3.GetComponent<Image>().color = unselectedColor;
        clusterButton4.GetComponent<Image>().color = unselectedColor;
        clusterButton5.GetComponent<Image>().color = unselectedColor;
        clusterButton6.GetComponent<Image>().color = unselectedColor;

        // set the selected button to a lighter color
        switch (numClusters)
        {
            case 2:
                clusterButton2.GetComponent<Image>().color = selectedColor;
                break;
            case 3:
                clusterButton3.GetComponent<Image>().color = selectedColor;
                break;
            case 4:
                clusterButton4.GetComponent<Image>().color = selectedColor;
                break;
            case 5:
                clusterButton5.GetComponent<Image>().color = selectedColor;
                break;
            case 6:
                clusterButton6.GetComponent<Image>().color = selectedColor;
                break;
        }
    }


    void PlaceFirstCentroid()
    {
        kmeansPlusPlus.InitializeFirstCentroid(); // initialize random centroids
        VisualizeCentroids(true); // visualize the centroids on the graph
    }

    void PlaceRestOfCentroidsMaxDist()
    {
        kmeansPlusPlus.InitializeRestOfCentroidsMaxDist(); // initialize maximally distant centroids
        VisualizeCentroids(false); // visualize the centroids on the graph
    }

    void VisualizeCentroids(bool first)
    {
        ClearPreviousCentroids();

        float scaler = 80f;
        Vector3[] centroids = kmeansPlusPlus.GetCentroids();

        Vector3 centroidPosition = NormalizeAndScaleCentroid(centroids[0], scaler);
        GameObject centroid = Instantiate(centroidPrefab, centroidPosition, Quaternion.Euler(-90, 0, 0));
        centroid.transform.localScale = Vector3.one * 2.0f;
        centroid.transform.SetParent(centroidsContainer.transform);
        centroid.tag = "Centroid";
        Renderer renderer = centroid.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.sharedMaterial = GetClusterMaterial(0);

            // add glow
            Material glowMaterial = new Material(renderer.sharedMaterial);
            glowMaterial.EnableKeyword("_EMISSION");
            glowMaterial.SetColor("_EmissionColor", GetClusterMaterial(0).color * 2.0f);
            renderer.sharedMaterial = glowMaterial;
        }

        if (!first)
        {
            for (int i = 1; i < centroids.Length; i++)
            {
                centroidPosition = NormalizeAndScaleCentroid(centroids[i], scaler);
                centroid = Instantiate(centroidPrefab, centroidPosition, Quaternion.Euler(-90, 0, 0));
                centroid.transform.localScale = Vector3.one * 2.0f;
                centroid.transform.SetParent(centroidsContainer.transform);
                centroid.tag = "Centroid";
                renderer = centroid.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.sharedMaterial = GetClusterMaterial(i);

                    // add glow
                    Material glowMaterial = new Material(renderer.sharedMaterial);
                    glowMaterial.EnableKeyword("_EMISSION");
                    glowMaterial.SetColor("_EmissionColor", GetClusterMaterial(i).color * 2.0f);
                    renderer.sharedMaterial = glowMaterial;
                }
            }
        }
        
    }

    void AssignPointsToCentroids()
    {
        kmeansPlusPlus.AssignPoints();
        VisualizeClusters();
    }

    void RecalculateCentroids()
    {
        kmeansPlusPlus.RecalculateCentroids(); // move the centroids based on point assignments
        VisualizeCentroids(false); // re-visualize the centroids after they move
    }

    // clear only data points
    void ClearPreviousDataPoints()
    {
        foreach (Transform child in pointsContainer.transform)
        {
            Destroy(child.gameObject);
        }
    }

    // clear only centroids
    void ClearPreviousCentroids()
    {
        foreach (Transform child in centroidsContainer.transform)
        {
            Destroy(child.gameObject);
        }
    }

    // visualize clusters with materials and print cluster info
    void VisualizeClusters()
    {
        ClearPreviousDataPoints();  // clear data points

        float scaler = 80f;

        // plot the data points
        for (int i = 0; i < dataPoints.Length; i++)
        {
            Vector3 position = NormalizeAndScalePoint(dataPoints[i], scaler);
            GameObject point = Instantiate(dataPointPrefab, position, Quaternion.Euler(-90, 0, 0));
            point.transform.localScale = Vector3.one * (1.0f + dataPoints[i].body_mass_g * 0.0005f);
            point.transform.SetParent(pointsContainer.transform);
            point.tag = "DataPoint";

            Renderer renderer = point.GetComponent<Renderer>();
            if (renderer != null)
            {
                int clusterLabel = kmeansPlusPlus.GetLabels()[i];
                renderer.sharedMaterial = GetClusterMaterial(clusterLabel);
            }
        }

        // plot the centroids again
        VisualizeCentroids(false);
    }

    // normalize and scale centroids similarly to data points
    Vector3 NormalizeAndScaleCentroid(Vector3 centroid, float scaler)
    {
        float xMin = dataPoints.Min(p => p.bill_length_mm), xMax = dataPoints.Max(p => p.bill_length_mm);
        float yMin = dataPoints.Min(p => p.bill_depth_mm), yMax = dataPoints.Max(p => p.bill_depth_mm);
        float zMin = dataPoints.Min(p => p.flipper_length_mm), zMax = dataPoints.Max(p => p.flipper_length_mm);

        float normalizedX = (centroid.x - xMin) / (xMax - xMin);
        float normalizedY = (centroid.y - yMin) / (yMax - yMin);
        float normalizedZ = (centroid.z - zMin) / (zMax - zMin);

        return new Vector3(
            normalizedX * scaler + 720,
            normalizedY * scaler + 547,
            -normalizedZ * scaler + 56
        );
    }


    // return the correct material based on cluster label
    Material GetClusterMaterial(int label)
    {
        switch (label)
        {
            case 0: return Material1;
            case 1: return Material2;
            case 2: return Material3;
            case 3: return Material4;
            case 4: return Material5;
            case 5: return Material6;
            default: return Material1; // default material if no valid label
        }
    }

    void Update()
    {
        if (isChoosingCentroids && Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("DataPoint"))
                {
                    OnDataPointClicked(hit.collider.gameObject);
                }
            }
        }
    }

    void OnDataPointClicked(GameObject dataPoint)
    {
        Debug.Log("Data Point clicked for centroid selection: " + dataPoint.name);

        dataPoint.GetComponent<Renderer>().material = GetClusterMaterial(currentCluster);
        currentCluster++;

        float scaler = 80f;

        selectedCentroids.Add(ReverseNormalizeAndScalePoint(dataPoint.transform.position, scaler));

        Debug.Log("All centroids selected.");
        isChoosingCentroids = false;
        kmeansPlusPlus.SetFirstCentroid(selectedCentroids);
        centroidsLeftText.text = "";
        chooseCentroidsButton.GetComponentInChildren<TMP_Text>().text = "Set\nRest Of\nCentroids";
        chooseCentroidsButton.GetComponent<Image>().color = selectedColor;
        chooseCentroidsButton.interactable = true;  // enable the button
    }


    void OnChooseCentroidsButtonClick()
    {
        //isChoosingCentroids = !isChoosingCentroids;

        //chooseCentroidsButton.GetComponentInChildren<TMP_Text>().text = isChoosingCentroids ? "Cancel Centroid Selection" : "I'll Choose Centroids";

        RectTransform buttonRectTransform = chooseCentroidsButton.GetComponent<RectTransform>();

        switch (currentState)
        {
            case KMeansState.InitializeFirstCentroid:
                currentState = KMeansState.InitializeRestOfCentroids;
                isChoosingCentroids = true;
                randomCentroidsButton.gameObject.SetActive(false);
                orText.gameObject.SetActive(false);
                chooseCentroidsButton.GetComponentInChildren<TMP_Text>().text = "(choosing centroids)";
                chooseCentroidsButton.GetComponent<Image>().color = unselectedColor;
                chooseCentroidsButton.interactable = false;  // disable the button
                centroidsLeftText.text = "Select 1 datapoint:";
                // adjust width
                float preferredWidth = chooseCentroidsButton.GetComponentInChildren<TMP_Text>().preferredWidth;
                //buttonRectTransform.sizeDelta = new Vector2(preferredWidth + 40f, buttonRectTransform.sizeDelta.y);
                break;

            case KMeansState.InitializeRestOfCentroids:
                PlaceRestOfCentroidsMaxDist();
                currentState = KMeansState.AssignPoints;
                chooseCentroidsButton.GetComponentInChildren<TMP_Text>().text = "Assign\nPoints";
                skipToEndButton.gameObject.SetActive(true);
                skipToEndButton.interactable = true;
                // adjust width
                preferredWidth = chooseCentroidsButton.GetComponentInChildren<TMP_Text>().preferredWidth;
                //buttonRectTransform.sizeDelta = new Vector2(preferredWidth + 40f, buttonRectTransform.sizeDelta.y);
                break;

            case KMeansState.AssignPoints:
                AssignPointsToCentroids();
                currentState = KMeansState.RecalculateCentroids;
                chooseCentroidsButton.GetComponentInChildren<TMP_Text>().text = "Recalculate\nCentroids";
                // adjust width
                preferredWidth = chooseCentroidsButton.GetComponentInChildren<TMP_Text>().preferredWidth;
                //buttonRectTransform.sizeDelta = new Vector2(preferredWidth + 40f, buttonRectTransform.sizeDelta.y);
                iteration++;
                iterationText.text = "Iteration #" + iteration;
                break;

            case KMeansState.RecalculateCentroids:
                RecalculateCentroids();

                if (kmeansPlusPlus.IsConverged())
                {
                    convergedText.text = "Converged";
                    // adjust width
                    preferredWidth = chooseCentroidsButton.GetComponentInChildren<TMP_Text>().preferredWidth;
                    //buttonRectTransform.sizeDelta = new Vector2(preferredWidth + 40f, buttonRectTransform.sizeDelta.y);
                    chooseCentroidsButton.interactable = false;  // disable the button to stop further recalculations
                    chooseCentroidsButton.GetComponent<Image>().color = unselectedColor;
                    restartButton.gameObject.SetActive(true); // restart button
                    skipToEndButton.gameObject.SetActive(false);
                }
                else
                {
                    currentState = KMeansState.AssignPoints;
                    chooseCentroidsButton.GetComponentInChildren<TMP_Text>().text = "Assign\nPoints";
                    // adjust width
                    preferredWidth = chooseCentroidsButton.GetComponentInChildren<TMP_Text>().preferredWidth;
                    //buttonRectTransform.sizeDelta = new Vector2(preferredWidth + 40f, buttonRectTransform.sizeDelta.y);

                }
                break;
        }
    }

    void OnSkipToEndButtonClick()
    {
        // Disable the Skip to End button to prevent multiple clicks
        skipToEndButton.interactable = false;

        // Start coroutine to animate the iterations until convergence
        StartCoroutine(SkipToEndAnimation());
    }

    IEnumerator SkipToEndAnimation()
    {
        bool converged = false;
        while (!converged && (currentState == KMeansState.AssignPoints || currentState == KMeansState.RecalculateCentroids))
        {
            switch (currentState)
            {
                // Perform one iteration of assigning points and recalculating centroids
                case KMeansState.AssignPoints:
                    AssignPointsToCentroids();
                    currentState = KMeansState.RecalculateCentroids;
                    chooseCentroidsButton.GetComponentInChildren<TMP_Text>().text = "Recalculate\nCentroids";
                    randomCentroidsButton.GetComponentInChildren<TMP_Text>().text = "Recalculate\nCentroids";
                    // adjust width
                    //buttonRectTransform.sizeDelta = new Vector2(preferredWidth + 40f, buttonRectTransform.sizeDelta.y);
                    iteration++;
                    iterationText.text = "Iteration #" + iteration;
                    break;

                case KMeansState.RecalculateCentroids:
                    RecalculateCentroids();

                    if (kmeansPlusPlus.IsConverged())
                    {
                        convergedText.text = "Converged";
                        // adjust width
                        //buttonRectTransform.sizeDelta = new Vector2(preferredWidth + 40f, buttonRectTransform.sizeDelta.y);
                        chooseCentroidsButton.interactable = false;  // disable the button to stop further recalculations
                        chooseCentroidsButton.GetComponent<Image>().color = unselectedColor;
                        randomCentroidsButton.interactable = false;  // disable the button to stop further recalculations
                        randomCentroidsButton.GetComponent<Image>().color = unselectedColor;
                        restartButton.gameObject.SetActive(true); // restart button
                        skipToEndButton.gameObject.SetActive(false);
                        converged = true;
                    }
                    else
                    {
                        currentState = KMeansState.AssignPoints;
                        chooseCentroidsButton.GetComponentInChildren<TMP_Text>().text = "Assign\nPoints";
                        randomCentroidsButton.GetComponentInChildren<TMP_Text>().text = "Assign\nPoints";
                        // adjust width
                        //buttonRectTransform.sizeDelta = new Vector2(preferredWidth + 40f, buttonRectTransform.sizeDelta.y); 
                    }
                    break;
            }

            // Wait for a short delay to create the animation effect
            yield return new WaitForSeconds(iterationDelay);
        }

    }

}
