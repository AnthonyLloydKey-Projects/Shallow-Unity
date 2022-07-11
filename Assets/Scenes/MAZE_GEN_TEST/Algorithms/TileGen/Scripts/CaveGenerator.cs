using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;


public class CaveGenerator : MonoBehaviour
{
    #region Tile Fields
    [Header("Tiles")]
    [SerializeField]
    GameObject[] entrancePrefabs;
    [SerializeField]
    GameObject[] tilePrefabs;
    [SerializeField]
    GameObject[] blockedAreaPrefabs;
    [SerializeField]
    GameObject[] exitPrefabs;
    [SerializeField]
    GameObject[] doorwayPrefabs;
    #endregion

    #region Generation Fields
    [Header("Generation Parameters")]
    [SerializeField]
    [Range(2,20)]
    int mainPathLength = 10;
    [Range(0, 30)]
    [SerializeField]
    int branchLength = 5;
    [SerializeField]
    [Range(0, 30)]
    int numberOfBranches = 10;
    [SerializeField]
    bool generateMainPathOnly = false;
    [SerializeField]
    int generationSeed = 0;
    #endregion

    #region Generation time debugging
    [Header("Generation-time Debugging")]
    [SerializeField]
    bool lightUpTiles = false;
    [SerializeField]
    bool fixRoomOverlap = false;
    [SerializeField]
    bool generateOnStart = false; 
    [SerializeField]
    Color mainPathColour = Color.red;
    [SerializeField]
    Color branchColour = Color.blue;
    #endregion

    #region Runtime Debugging Fields
    [Header("Runtime Debugging")]
    [SerializeField]
    List<Tile> generatedTiles = new List<Tile>();
    [SerializeField]
    KeyCode toggleMap = KeyCode.M;
    #endregion

    List<Connector> availableConnectors = new List<Connector>(); 

    Transform tileFrom, tileTo, tileRoot;
    Transform container;

    //Collision
    int attempts;
    readonly int maxAttempts = 50;

    void Start()
    {
        if (generateOnStart == false)
            return;

        BuildCave(generationSeed);

    }

    public void BuildCave(int seed)
    {
        Random.InitState(seed);

        //main path
        var goContainer = new GameObject("main path");
        container = goContainer.transform;
        container.SetParent(transform);
        tileRoot = CreateStartRoom();
        tileTo = tileRoot;

        for (int i = 0; i < mainPathLength-1; i++)
        {
            tileFrom = tileTo;
            tileTo = CreateTile();
            DebugTileLighting(tileTo, mainPathColour);
            ConnectTiles();

            if (fixRoomOverlap)
            {
                CollisionPass();

                if (attempts >= maxAttempts)
                {
                    break;
                }

            }
        }

        if (generateMainPathOnly)
            return;

        foreach (Connector connector in container.GetComponentsInChildren<Connector>())
        {
            if (!connector.isConnected)
            {
                if (!availableConnectors.Contains(connector))
                {
                    availableConnectors.Add(connector);
                }
            }
        }

        //branches
        for(int b = 0; b < numberOfBranches; b++)
        {
            if (availableConnectors.Count > 0)
            {

                goContainer = new GameObject("branch: "+ (b+1).ToString());

                container = goContainer.transform;
                container.SetParent(transform);
                int availI = Random.Range(0, availableConnectors.Count);
                tileRoot = availableConnectors[availI].transform.parent.parent;
                availableConnectors.RemoveAt(availI);
                tileTo = tileRoot;

                for (int i = 0; i < branchLength -1; i++)
                {
                    tileFrom = tileTo;
                    tileTo = CreateTile();
                    DebugTileLighting(tileTo, branchColour);
                    ConnectTiles();

                    if (fixRoomOverlap)
                    {

                        CollisionPass();

                        if (attempts >= maxAttempts)
                        {
                            break;
                        }

                    }
                }
            } else
            {
                break;
            }
        }

       


    }

    void OnDisableLightingDebug()
    {
        Debug.Log("lights_off");


    }


    void CollisionPass()
    {
        BoxCollider box = tileTo.GetComponent<BoxCollider>();
        if (box == null)
        {
            box = tileTo.gameObject.AddComponent<BoxCollider>();
            box.isTrigger = true;
        }

        Vector3 offset = (tileTo.right * box.center.x) + (tileTo.up * box.center.y) + (tileTo.forward * box.center.z);
        Vector3 halfExtents = box.bounds.extents; //3D box radius 

        List<Collider> hits = Physics.OverlapBox(tileTo.position + offset, halfExtents, Quaternion.identity, LayerMask.GetMask("Tile")).ToList();
        if (hits.Count > 0)
        {
            if(hits.Exists(x => x.transform != tileFrom && x.transform != tileTo))
            {
               
                attempts++;
                int toIndex = generatedTiles.FindIndex(x => x.tile == tileTo);
                if(generatedTiles[toIndex].connector != null)
                {
                    generatedTiles[toIndex].connector.isConnected = false;
                }
                generatedTiles.RemoveAt(toIndex);
                DestroyImmediate(tileTo.gameObject);


                //backtracking
                if(attempts >= maxAttempts)
                {
                    int fromI = generatedTiles.FindIndex(x => x.tile == tileFrom);
                    Tile myTileFrom = generatedTiles[fromI];

                    if(tileFrom != tileRoot)
                    {
                        if (myTileFrom.connector != null)
                        {
                            myTileFrom.connector.isConnected = false;
                        }
                        availableConnectors.RemoveAll(x => x.transform.parent.parent == tileFrom);
                        generatedTiles.RemoveAt(fromI);

                        DestroyImmediate(tileFrom.gameObject);

                        if (myTileFrom.origin != tileRoot)
                        {
                            tileFrom = myTileFrom.origin;

                        }
                        else if (container.name.Contains("Main"))
                        {
                            if (myTileFrom != null)
                            {
                                tileRoot = myTileFrom.origin;
                                tileFrom = tileRoot;
                            }
                        }
                        else if (availableConnectors.Count > 0)
                        {
                            int availI = Random.Range(0, availableConnectors.Count);
                            tileRoot = availableConnectors[availI].transform.parent.parent;
                            availableConnectors.RemoveAt(availI);
                            tileFrom = tileRoot;
                        }
                        else
                        {
                            return;
                        }

                    } else if (container.name.Contains("Main"))
                    {
                        if(myTileFrom.origin !=null)
                        {
                            tileRoot = myTileFrom.origin;
                            tileFrom = tileRoot;
                        }

                    } else if(availableConnectors.Count > 0)
                    {
                        int availI = Random.Range(0, availableConnectors.Count);
                        tileRoot = availableConnectors[availI].transform.parent.parent;
                        availableConnectors.RemoveAt(availI);
                        tileFrom = tileRoot;
                    } else
                    {
                        return;
                    }


                }
                //retry 
                if (tileFrom != null)
                {
                    tileTo = CreateTile();
                    Color retryCorrected = container.name.Contains("Branch") ? branchColour : mainPathColour;
                    DebugTileLighting(tileTo, retryCorrected);
                    ConnectTiles();
                    CollisionPass();
                }

            }
            else { attempts = 0; }

        }
        

    }

    #region Debug

    void DebugTileLighting(Transform tile, Color color)
    {
        if (lightUpTiles == false && Application.isEditor)
            return;

        Light[] lights = tile.GetComponentsInChildren<Light>();

        if(lights.Length > 0)
        {
            foreach(Light light in lights)
            {
                light.color = color;
            }
        }
    }
    

    #endregion 


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            SceneManager.LoadScene(2);
        }
    }

    Transform CreateStartRoom()
    {
        int index = Random.Range(0, entrancePrefabs.Length);
        GameObject gTile = Instantiate(entrancePrefabs[index], Vector3.zero, Quaternion.identity, container) as GameObject;
        gTile.name = "Start";
        float yRot = Random.Range(0, 4) * 90f;
        gTile.transform.Rotate(0, yRot, 0);
        generatedTiles.Add(new Tile(gTile.transform, null));
        return gTile.transform;
    }

    Transform CreateTile()
    {
        int index = Random.Range(0, tilePrefabs.Length);
        GameObject gTile = Instantiate(tilePrefabs[index], Vector3.zero, Quaternion.identity, container) as GameObject;
        gTile.name = tilePrefabs[index].name;
        var orign = generatedTiles[generatedTiles.FindIndex(x => x.tile == tileFrom)].tile;
        generatedTiles.Add(new Tile(gTile.transform, orign));

        return gTile.transform;

    }

    void ConnectTiles()
    {
        Transform connectFrom = GetRandomConnector(tileFrom);

        if (connectFrom == null)
            return;

        Transform connectTo = GetRandomConnector(tileTo);

        if (connectTo == null)
            return;

        connectTo.SetParent(connectFrom);
        tileTo.SetParent(connectTo);
        connectTo.localPosition = Vector3.zero;
        connectTo.localRotation = Quaternion.identity;
        connectTo.Rotate(0, 180f, 0);
        tileTo.SetParent(container);
        connectTo.SetParent(tileTo.Find("Connectors"));
        generatedTiles.Last().connector = connectFrom.GetComponent<Connector>();
    }

    Transform GetRandomConnector(Transform tile)
    {
        if (tile == null)
            return null;

        List<Connector> connectors = tile.GetComponentsInChildren<Connector>().ToList().FindAll(x => x.isConnected == false);
        if(connectors.Count > 0)
        {
            int connectorIndex = Random.Range(0, connectors.Count);
            connectors[connectorIndex].isConnected = true;
            
            if(tile == tileFrom)
            {
                BoxCollider BOX = tile.GetComponent<BoxCollider>();
                if(BOX == null)
                {
                    BOX = tile.gameObject.AddComponent<BoxCollider>();
                    BOX.isTrigger = true;
                }
                BOX.name = "/// TILE AREA MESH DATA ///";
            }

            return connectors[connectorIndex].transform;
        }
        return null;
    }
}
