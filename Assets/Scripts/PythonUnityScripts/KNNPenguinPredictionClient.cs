using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static KNNPenguinDataVisualizer;

public class KNNPenguinPredictionClient : MonoBehaviour
{
    private float[][] trainingData;
    private string[] trainingLabels;
    private float[][] testingData;
    private string[] testingLabels;
    private float accuracy;

    private PenguinDataPoint[] penguinDataPoints = new PenguinDataPoint[] {
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

    private void Start()
    {
        InitializeData();
    }

    private void InitializeData()
    {
        List<float[]> dataArrayList = new List<float[]>();
        List<string> labelList = new List<string>();

        foreach (PenguinDataPoint point in penguinDataPoints)
        {
            dataArrayList.Add(new float[] { point.bill_length_mm, point.bill_depth_mm, point.flipper_length_mm, point.body_mass_g });
            labelList.Add(point.species);
        }

        System.Random rand = new System.Random();
        var shuffledData = dataArrayList.Zip(labelList, (data, label) => new { data, label })
                                        .OrderBy(x => rand.Next())
                                        .ToArray();

        int splitIndex = (int)(0.75 * shuffledData.Length);

        trainingData = shuffledData.Take(splitIndex).Select(x => x.data).ToArray();
        trainingLabels = shuffledData.Take(splitIndex).Select(x => x.label).ToArray();
        testingData = shuffledData.Skip(splitIndex).Select(x => x.data).ToArray();
        testingLabels = shuffledData.Skip(splitIndex).Select(x => x.label).ToArray();

        CalculateAccuracy();
    }

    public void Predict(float[] input, Action<string> onOutputReceived, Action<Exception> fallback)
    {
        try
        {
            string prediction = KNNPredict(input, 5); // k-value = 5
            onOutputReceived(prediction);
        }
        catch (Exception ex)
        {
            fallback(ex);
        }
    }

    private string KNNPredict(float[] input, int k)
    {
        var neighbors = penguinDataPoints.OrderBy(p => CalculateDistance(input, p)).Take(k); // K = 5
        var prediction = neighbors.GroupBy(p => p.species).OrderByDescending(g => g.Count()).First().Key;
        return prediction;
    }

    private float CalculateDistance(float[] input, PenguinDataPoint point)
    {
        return Mathf.Sqrt(Mathf.Pow(input[0] - point.bill_length_mm, 2) +
                          Mathf.Pow(input[1] - point.bill_depth_mm, 2) +
                          Mathf.Pow(input[2] - point.flipper_length_mm, 2) +
                          Mathf.Pow(input[3] - point.body_mass_g, 2));
    }

    private void CalculateAccuracy()
    {
        int correctPredictions = 0;

        for (int i = 0; i < testingData.Length; i++)
        {
            float[] input = testingData[i];
            string actualLabel = testingLabels[i];

            string predictedLabel = KNNPredict(input, 3);

            if (predictedLabel == actualLabel)
            {
                correctPredictions++;
            }
        }

        accuracy = (float)correctPredictions / testingData.Length;
        Debug.Log($"Final Accuracy: {accuracy * 100}%");
    }

    public float GetAccuracy()
    {
        return accuracy;
    }
}
