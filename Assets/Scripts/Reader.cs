using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System.Globalization;

using SFB;


public class Reader : MonoBehaviour
{

    public GameObject boxPrefab;
    public Text textBox;
    public Text textBoxRight;


    public bool colorAxis = true;
    public int mainAxisIndex = 0;
    public int coordinateAxisIdx = 0;
    public List<Vector3> coordinateAxisList = new List<Vector3> { new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, 0, 1) };
    private List<GameObject> temporaryObjects = new List<GameObject>();
    public List<List<string>> states = new List<List<string>>();

    public float maxDrawRadius = 40;

    private int N;
    private int currentState = 0;
    private int maxState;

    public string picturesFolder = "Pictures";

    // Use this for initialization

    void startReader(string path)
    {
        cleanUp();
        states = new List<List<string>>();
        currentState = 0;

        string line;
        StreamReader reader = new StreamReader(path);
        string firstLine = reader.ReadLine();
        var elements = firstLine.Split(',');
        N = int.Parse(elements[0]);
        mainAxisIndex = int.Parse(elements[1]);


        int i = 0;
        int j = 0;
        states.Add(new List<string>());
        while ((line = reader.ReadLine()) != null)
        {
            states[i].Add(line);
            j++;
            if (j > N)
            {
                i++;
                j = 0;
                states.Add(new List<string>());
            }
        }
        reader.Close();

        maxState = states.Count - 2;

        drawState(currentState);
    }


    void cleanUp()
    {
        if (temporaryObjects.Count > 0)
        {
            var nrOfObjectsToDestroy = temporaryObjects.Count;
            for (int i = 0; i < nrOfObjectsToDestroy; i++)
            {
                Destroy(temporaryObjects[0].gameObject);
                temporaryObjects.RemoveAt(0);
            }
        }
    }

    void drawState(int state)
    {
        currentState = state;
        cleanUp();

        var firstLineElements = states[state][0].Split(',');
        if (firstLineElements[0] == "cylinder")
        {
            var r = float.Parse(firstLineElements[1]);
            var h = float.Parse(firstLineElements[2]);

            GameObject circle1 = new GameObject();
            GameObject circle2 = new GameObject();
            circle1.transform.position = new Vector3(0, (h / 2), 0);
            circle2.transform.position = new Vector3(0, (-h / 2), 0);

            CircleDrawer c1 = circle1.AddComponent<CircleDrawer>();
            CircleDrawer c2 = circle2.AddComponent<CircleDrawer>();
            c1.radius = r;
            c2.radius = r;
            temporaryObjects.Add(c1.gameObject);
            temporaryObjects.Add(c2.gameObject);
        }
        else if (firstLineElements[0] == "cube")
        {
            var L = float.Parse(firstLineElements[1]);

            GameObject line1 = new GameObject();
            LineRenderer lineRenderer1 = line1.gameObject.AddComponent<LineRenderer>();
            lineRenderer1.positionCount = 5;

            lineRenderer1.SetPosition(0, new Vector3(L / 2, L/2, L / 2));
            lineRenderer1.SetPosition(1, new Vector3(-L / 2, L/2, L / 2));
            lineRenderer1.SetPosition(2, new Vector3(-L / 2, L/2, -L / 2));
            lineRenderer1.SetPosition(3, new Vector3(L / 2, L/2, -L / 2));
            lineRenderer1.SetPosition(4, new Vector3(L / 2, L / 2, L / 2));

            GameObject line2 = new GameObject();
            LineRenderer lineRenderer2 = line2.gameObject.AddComponent<LineRenderer>();
            lineRenderer2.positionCount = 5;

            lineRenderer2.SetPosition(0, new Vector3(L / 2, -L / 2, L / 2));
            lineRenderer2.SetPosition(1, new Vector3(-L / 2, -L / 2, L / 2));
            lineRenderer2.SetPosition(2, new Vector3(-L / 2, -L / 2, -L / 2));
            lineRenderer2.SetPosition(3, new Vector3(L / 2, -L / 2, -L / 2));
            lineRenderer2.SetPosition(4, new Vector3(L / 2, -L / 2, L / 2));

            temporaryObjects.Add(line1.gameObject);
            temporaryObjects.Add(line2.gameObject);
        }
        else if (firstLineElements[0] == "sphere")
        {
            var r = float.Parse(firstLineElements[1]);

            GameObject circle1 = new GameObject();
            GameObject circle2 = new GameObject();
            GameObject circle3 = new GameObject();

            circle1.transform.position = new Vector3(0, 0, 0);
            circle2.transform.position = new Vector3(0, 0, 0);
            circle3.transform.position = new Vector3(0, 0, 0);

            CircleDrawer c1 = circle1.AddComponent<CircleDrawer>();
            CircleDrawer c2 = circle2.AddComponent<CircleDrawer>();
            CircleDrawer c3 = circle3.AddComponent<CircleDrawer>();

            c1.radius = r;
            c1.rotation = 1;
            c2.radius = r;

            c3.radius = r;
            c3.rotation = 2;

            temporaryObjects.Add(c1.gameObject);
            temporaryObjects.Add(c2.gameObject);
            temporaryObjects.Add(c3.gameObject);

        }
        ulong T = ulong.Parse(firstLineElements[3]);
        double S1 = double.Parse(firstLineElements[4]);
        double S2 = double.Parse(firstLineElements[5]);
        double S3 = double.Parse(firstLineElements[6]);
        double B1 = double.Parse(firstLineElements[7]);
        double B2 = double.Parse(firstLineElements[8]);
        double B3 = double.Parse(firstLineElements[9]);
        double deltaS1 = double.Parse(firstLineElements[10]);
        double deltaS2 = double.Parse(firstLineElements[11]);
        double deltaS3 = double.Parse(firstLineElements[12]);
        double acceptedTPercentage = double.Parse(firstLineElements[13]);
        double acceptedRPercentage = double.Parse(firstLineElements[14]);

        textBox.text = "T = " + T.ToString("0.##E+00") +
            "\nS1 = " + S1.ToString("0.##E+00") + "\nS2 = " + S2.ToString("0.##E+00") + "\nS3 = " + S3.ToString("0.##E+00") +
            "\nΔS1 = " + deltaS1.ToString("0.##E+00") + "\nΔS2 = " + deltaS2.ToString("0.##E+00") + "\nΔS3 = " + deltaS3.ToString("0.##E+00") +
            "\nB1 = " + B1.ToString("0.##E+00") + "\nB2 = " + B2.ToString("0.##E+00") + "\nB3 = " + B3.ToString("0.##E+00") +
            "\nAccepted Translations = " + acceptedTPercentage.ToString("0.##") + "% \nAccepted Rotations = " + acceptedRPercentage.ToString("0.##") + "%";

        textBoxRight.text = coordinateAxisList[coordinateAxisIdx].ToString() + "\n";
        textBoxRight.text += mainAxisIndex.ToString();


        for (int i = 1; i < states[state].Count; i++)
        {
            string line = states[state][i];
            line = line.Replace(" ", "");
            line = line.Replace("),(", "@");
            line = line.Replace("(", "");
            line = line.Replace(")", "");

            var elements = line.Split('@');

            //CUBE CENTER
            var centerElements = elements[0].Split(',');
            Vector3 center = new Vector3(float.Parse(centerElements[0]), float.Parse(centerElements[1]), float.Parse(centerElements[2]));

            if (center.magnitude > maxDrawRadius)
                continue;

            //HALF LENGTHS
            var hLengthElements = elements[1].Split(',');
            Vector3 halfLengths = new Vector3(float.Parse(hLengthElements[0]), float.Parse(hLengthElements[1]), float.Parse(hLengthElements[2]));

            Vector3[] bases = new Vector3[3];

            //BASE 1
            var base1Elements = elements[2].Split(',');
            bases[0] = new Vector3(float.Parse(base1Elements[0]), float.Parse(base1Elements[1]), float.Parse(base1Elements[2]));

            //BASE 2
            var base2Elements = elements[3].Split(',');
            bases[1] = new Vector3(float.Parse(base2Elements[0]), float.Parse(base2Elements[1]), float.Parse(base2Elements[2]));

            //BASE 1
            var base3Elements = elements[4].Split(',');
            bases[2] = new Vector3(float.Parse(base3Elements[0]), float.Parse(base3Elements[1]), float.Parse(base3Elements[2]));

            Vector3[] vertices = {
            center - halfLengths[0]*bases[0] - halfLengths[1]*bases[1] - halfLengths[2]*bases[2],
            center + halfLengths[0]*bases[0] - halfLengths[1]*bases[1] - halfLengths[2]*bases[2],
            center + halfLengths[0]*bases[0] - halfLengths[1]*bases[1] + halfLengths[2]*bases[2],
            center - halfLengths[0]*bases[0] - halfLengths[1]*bases[1] + halfLengths[2]*bases[2],
            center - halfLengths[0]*bases[0] + halfLengths[1]*bases[1] + halfLengths[2]*bases[2],
            center + halfLengths[0]*bases[0] + halfLengths[1]*bases[1] + halfLengths[2]*bases[2],
            center + halfLengths[0]*bases[0] + halfLengths[1]*bases[1] - halfLengths[2]*bases[2],
            center - halfLengths[0]*bases[0] + halfLengths[1]*bases[1] - halfLengths[2]*bases[2],
            };

            GameObject box = (GameObject)Instantiate(boxPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            box.GetComponent<MeshGenerator>().vertices = vertices;
            var projection = Mathf.Acos(Mathf.Abs(Vector3.Dot(bases[mainAxisIndex], coordinateAxisList[coordinateAxisIdx]))) / 1.571f;
            box.GetComponent<MeshGenerator>().color = new Color(projection, 0, 1 - projection);
            //box.GetComponent<MeshGenerator>().color = new Color(1, 0, 1 - projection*0.3f);

            box.GetComponent<MeshGenerator>().clr = projection;
            box.GetComponent<MeshGenerator>().axis = bases[mainAxisIndex];

            temporaryObjects.Add(box);
        }
    }

    void screenshotSystem(string path)
    {
        cleanUp();
        startReader(path);

        var tempname = path.Split('\\');
        string name = tempname[tempname.Length - 1].Replace(".txt", "");

        FreeFloatingCamera camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<FreeFloatingCamera>();

        camera.radius = 90.63779f;
        camera.theta = 2 * Mathf.PI;

        for (int i = 0; i < 5; i++)
        {
            StartCoroutine(screenshotCoroutine(name + "_1_" + i.ToString(), 2 + 1 * i, 2, 1, i));
        }

        StartCoroutine(rotateCamera(13, 5.3f));

        for (int i = 0; i < 5; i++)
        {
            StartCoroutine(screenshotCoroutine(name + "_2_" + i.ToString(), 15 + 1 * i, 1, 0, i));
        }
    }

    IEnumerator screenshotCoroutine(string name, int seconds, int _coordinateAxisIdx, int _mainAxisIndex, int previousState = 0)
    {
        yield return new WaitForSeconds(seconds);
        coordinateAxisIdx = _coordinateAxisIdx;
        mainAxisIndex = _mainAxisIndex;
        drawState(maxState - previousState);

        ScreenCapture.CaptureScreenshot(picturesFolder + "/" + name + ".png");
    }

    IEnumerator rotateCamera(int seconds, float theta)
    {
        yield return new WaitForSeconds(seconds);

        FreeFloatingCamera camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<FreeFloatingCamera>();
        camera.theta = theta;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            var path = StandaloneFileBrowser.OpenFilePanel("Open File", "Assets/Results", "txt", false);
            if (path != null)
            {
                cleanUp();
                startReader(path[0]);
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (currentState == 0)
                return;

            currentState = currentState - 1;
            drawState(currentState);
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            if (currentState == maxState)
                return;

            currentState = currentState + 1;
            drawState(currentState);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            coordinateAxisIdx = (coordinateAxisIdx + 1) % 3;
            drawState(currentState);
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            mainAxisIndex = (mainAxisIndex + 1) % 3;
            drawState(currentState);
        }



        if (Input.GetKeyDown(KeyCode.P))
        {
            var paths = StandaloneFileBrowser.OpenFilePanel("Open File", "Assets/Results", "txt", true);
            int seconds = 0;
            if (paths != null)
            {
                foreach (string path in paths)
                {

                    StartCoroutine(screenshotSystemCoroutine(path, seconds));
                    seconds += 27;
                }

            }
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            int nr = 0;
            while(File.Exists(picturesFolder + "/screenshot" + nr.ToString() + ".png"))
            {
                nr++;
            }
            string name = picturesFolder + "/screenshot" + nr.ToString() + ".png";

            Debug.Log(picturesFolder + "/screenshot" + nr.ToString() + ".png");

            ScreenCapture.CaptureScreenshot(name);
        }

        if (Input.GetKeyDown(KeyCode.Minus) || Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            maxDrawRadius -= 1;
            drawState(currentState);
        }

        if (Input.GetKeyDown(KeyCode.Plus) || Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            maxDrawRadius += 1;
            drawState(currentState);
        }
    }

    IEnumerator screenshotSystemCoroutine(string path, int seconds)
    {
        yield return new WaitForSeconds(seconds);
        screenshotSystem(path);
    }

}



