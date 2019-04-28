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

    public bool colorAxis = true;
    public int mainAxisIndex = 0;
    public Vector3 mainAxis = new Vector3(0, 1, 0);
    private List<GameObject> temporaryObjects = new List<GameObject>();
    public List<List<string>> states = new List<List<string>>();
    private int N;
    private int currentState = 0;
    private int maxState;

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
        maxState = i - 1;
        reader.Close();

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
        cleanUp();  

        var firstLineElements = states[state][0].Split(',');
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

        int T = int.Parse(firstLineElements[3]);
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
            var projection = Mathf.Abs(Vector3.Dot(bases[mainAxisIndex], mainAxis));
            box.GetComponent<MeshGenerator>().color = new Color(projection, 0, 1 - projection);
            box.GetComponent<MeshGenerator>().clr = projection;
            box.GetComponent<MeshGenerator>().axis = bases[mainAxisIndex];

            temporaryObjects.Add(box);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            cleanUp();
            var path = StandaloneFileBrowser.OpenFilePanel("Open File", "Assets/Results", "", false);
            startReader(path[0]);          
        }

        if (Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.Minus) || Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            if (currentState == 0)
                return;

            currentState = currentState - 1;
            drawState(currentState);
        }

        if (Input.GetKeyDown(KeyCode.T) || Input.GetKeyDown(KeyCode.Plus) || Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            if (currentState == maxState)
                return;

            currentState = currentState + 1;
            drawState(currentState);
        }
    }
}
