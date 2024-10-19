using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;
using System.Drawing;

public class KMeansPenguinDataVisualizer : MonoBehaviour
{
    public GameObject dataPointPrefab;
    public GameObject centroidPrefab;
    public Material Material1, Material2, Material3;
    public int numClusters = 3;
    public int maxIterations = 100;

    private GameObject pointsContainer;
    private GameObject centroidsContainer;

    private KMeans kmeans;

    private int iteration = 0;

    public Button recalculateButton;
    public Button restartButton;
    public TMP_Text iterationText;
    public TMP_Text convergedText;

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
        InitializeCentroids,
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

        kmeans = new KMeans(numClusters, maxIterations, dataPoints.ToList());

        currentState = KMeansState.InitializeCentroids;
        recalculateButton.GetComponentInChildren<TMP_Text>().text = "Place Random Centroids";

        convergedText.text = "";

        recalculateButton.onClick.AddListener(OnRecalculateButtonClick);

        restartButton.gameObject.SetActive(false);
        restartButton.onClick.AddListener(OnRestartButtonClick);
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

    // create a sphere for each penguin data point
    void CreateSphere(Vector3 position, float bodyMass)
    {
        GameObject sphere = Instantiate(dataPointPrefab, position, Quaternion.Euler(-90, 0, 0));
        sphere.transform.localScale = Vector3.one * (1.0f + bodyMass * 0.0005f);
        sphere.transform.SetParent(pointsContainer.transform);
        sphere.tag = "DataPoint";
    }

    void OnRecalculateButtonClick()
    {
        switch (currentState)
        {
            case KMeansState.InitializeCentroids:
                PlaceRandomCentroids();
                currentState = KMeansState.AssignPoints;
                recalculateButton.GetComponentInChildren<TMP_Text>().text = "Assign Points";
                break;

            case KMeansState.AssignPoints:
                AssignPointsToCentroids();
                currentState = KMeansState.RecalculateCentroids;
                recalculateButton.GetComponentInChildren<TMP_Text>().text = "Recalculate Centroids";
                iteration++;
                iterationText.text = "Iteration #" + iteration;
                break;

            case KMeansState.RecalculateCentroids:
                RecalculateCentroids();

                if (kmeans.IsConverged())
                {
                    convergedText.text = "Converged";
                    recalculateButton.interactable = false;  // disable the button to stop further recalculations
                    restartButton.gameObject.SetActive(true); // restart button
                }
                else
                {
                    currentState = KMeansState.AssignPoints;
                    recalculateButton.GetComponentInChildren<TMP_Text>().text = "Assign Points";
                }
                break;
        }
    }

    void OnRestartButtonClick()
    {
        convergedText.text = "";
        iteration = 0;
        iterationText.text = "Iteration #0";
        restartButton.gameObject.SetActive(false);
        recalculateButton.interactable = true;

        // Re-initialize KMeans
        kmeans.InitializeCentroids();
        currentState = KMeansState.InitializeCentroids;
        recalculateButton.GetComponentInChildren<TMP_Text>().text = "Place Random Centroids";

        ClearPreviousDataPoints();
        ClearPreviousCentroids();
        SetupDataPoints();
    }



    void PlaceRandomCentroids()
    {
        kmeans.InitializeCentroids(); // initialize random centroids
        VisualizeCentroids(); // visualize the centroids on the graph
    }

    void VisualizeCentroids()
    {
        ClearPreviousCentroids();

        float scaler = 80f;
        Vector3[] centroids = kmeans.GetCentroids();

        for (int i = 0; i < centroids.Length; i++)
        {
            Vector3 centroidPosition = NormalizeAndScaleCentroid(centroids[i], scaler);
            GameObject centroid = Instantiate(centroidPrefab, centroidPosition, Quaternion.Euler(-90, 0, 0));
            centroid.transform.localScale = Vector3.one * 2.0f;
            centroid.transform.SetParent(centroidsContainer.transform);
            centroid.tag = "Centroid";
            Renderer renderer = centroid.GetComponent<Renderer>();
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

    void AssignPointsToCentroids()
    {
        kmeans.AssignPoints();
        VisualizeClusters();
    }

    void RecalculateCentroids()
    {
        kmeans.RecalculateCentroids(); // move the centroids based on point assignments
        VisualizeCentroids(); // re-visualize the centroids after they move
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

    // visualize the clusters with materials and print cluster info
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
                int clusterLabel = kmeans.GetLabels()[i];
                renderer.sharedMaterial = GetClusterMaterial(clusterLabel);
            }
        }

        // plot the centroids again
        VisualizeCentroids();
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
            default: return Material1; // default material if no valid label
        }
    }

}
