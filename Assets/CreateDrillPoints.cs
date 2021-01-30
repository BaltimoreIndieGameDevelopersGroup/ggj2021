using OptIn.Voxel;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CreateDrillPoints : MonoBehaviour
{
    // Start is called before the first frame update
    Collider m_Collider;
    public Vector3 m_Center;
    public Vector3 m_Size, m_Min, m_Max;
    public int numberOfBoxesPerRow = 6;
    public int yLevels = 10;
    public float yincrement = 0.2f;
    List<Vector3> listOfPoints;

    private void Start()
    {
        listOfPoints = new List<Vector3>();
        pointsAndCarve();
    }

    public void pointsAndCarve()
    {
        int yIndex = 0;
        while (yIndex < yLevels)
        {
            createPoints(yIndex * yincrement);
            yIndex++;
            listOfPoints.ForEach(point =>
            {
                Voxel voxel;

                if (TerrainGenerator.Instance.GetVoxel(point, out voxel))
                {

                    print(voxel.data);
                    TerrainGenerator.Instance.SetVoxel(point, Voxel.VoxelType.Air);

                }
            });
        }
    }

    public void createPoints(float y)
    {
        listOfPoints.Clear();
        //Fetch the Collider from the GameObject
        if (m_Collider == null)
        {
            m_Collider = GetComponent<Collider>();
        }
        //Fetch the center of the Collider volume
        m_Center = m_Collider.bounds.center;
        //drawCube(m_Center);
        //Fetch the size of the Collider volume
        m_Size = m_Collider.bounds.size;
        //Fetch the minimum and maximum bounds of the Collider volume
        m_Min = m_Collider.bounds.min;
        m_Max = m_Collider.bounds.max;
        // int i = 1;

        //float zPos = 0;
        int halfPos = numberOfBoxesPerRow / 2;
        int halfNeg = -numberOfBoxesPerRow / 2;
        while (halfNeg < halfPos)
        {
            var tempZ = ((m_Size.z) / numberOfBoxesPerRow) * halfNeg;

            createRow(tempZ, y);
            halfNeg++;
        }

    }

    private void createRow(float zPos, float y)
    {
        int halfPos = numberOfBoxesPerRow / 2;
        int halfNeg = -numberOfBoxesPerRow / 2;
        while (halfNeg < halfPos)
        {
            //   Console.WriteLine(i);
            var tempX = ((m_Size.x) / numberOfBoxesPerRow) * halfNeg;
            listOfPoints.Add(new Vector3(tempX, y, zPos) + m_Center);

            halfNeg++;
        }
    }

    void drawCube(Vector3 loc)
    {
        listOfPoints.Add(loc);
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(loc, new Vector3(1, 1, 1));
    }


}


// TODO , Use this instead of easy buttons 
#if UNITY_EDITOR
[CustomEditor(typeof(CreateDrillPoints))]
public class CreateDrillPointsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var myTarget = (CreateDrillPoints)target;


        DrawDefaultInspector();
        if (GUILayout.Button("Update Points"))
        {
            myTarget.pointsAndCarve();
        }

    }


}

#endif
