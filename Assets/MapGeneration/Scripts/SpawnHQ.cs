// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class SpawnHQ : MonoBehaviour
// {
//     public int range = 0;
//     public int safe_range = 5;
//     public GameObject HQ;
//     public GameObject garage;
//     public int[] ore_radious;
//     LayerMask layer, layer2;
//     public GameObject[] ore_prefab;

//     Collider[] Validate(Collider[] to_check)
//     {
//         for (int i = 0; i < to_check.Length; i++)
//         {
//             if (to_check[i].GetComponent<CustomTag>().HasTag("stone"))
//                 return null;
//         }
//         return to_check;
//     }
//     GameObject CalculateSpawn(int range, int size)
//     {
//         float map_size_x = size * HexMetrics.inner_radious * 4;
//         float map_size_z = size * HexMetrics.outer_radious * 1.5f;
//         Vector3 centre = new Vector3(map_size_x / 2, 0, map_size_z / 2);
//        // Debug.Log(centre);
//         Collider[] neighbours;
//         neighbours = Physics.OverlapBox(new Vector3(map_size_x / 2, 0, map_size_z / 2), new Vector3(range, 10, range), Quaternion.Euler(0, 0, 0), layer);
//       // Debug.Log(neighbours.Length);
//         for(int i = 0; i < neighbours.Length; i++)
//         {
//             Collider[] check_if_correct;
//             centre = new Vector3(neighbours[i].transform.position.x, 0, neighbours[i].transform.position.z);
//             check_if_correct = Validate(Physics.OverlapSphere(centre, 3, layer));
//             if(check_if_correct != null)
//                 if (check_if_correct.Length >= 19)
//                     return neighbours[i].gameObject;
//         }
//         return null;
//     }

//     private void Start()
//     {
//         layer = LayerMask.GetMask("Default");
//         layer2 = LayerMask.GetMask("Oth_Terrain");
//     }

//     bool is_set = false;
//     GameObject spawn_location;

//     public static void SpawnPlayer()
//     {
//         GameObject[] hex = GameObject.FindGameObjectsWithTag("hex");
//         foreach(GameObject obj in hex)
//         {
//             if(obj.GetComponent<CustomTag>().HasTag("garage") && (obj.GetComponent<garagesc>().player != null))
//                 Instantiate(obj.GetComponent<garagesc>().player, obj.transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
//         }
//     }

//     void ClearSpawnArea(GameObject spawn_location)
//     {
//         for(int i = 0; i < ore_radious.Length; i++)
//         {
//             Collider[] colliders = Physics.OverlapSphere(spawn_location.transform.position, ore_radious[i], layer);
//             //Debug.Log(ore_radious.Length);
//             for(int j = 0; j < colliders.Length; j++)
//             {
//                 if(i == 0)
//                 {
//                     if(colliders[j].gameObject.GetComponent<CustomTag>().has_tree == true || colliders[j]. GetComponent<CustomTag>().has_ore == true)
//                     {
//                         Collider[] coll_to_destroy = Physics.OverlapSphere(colliders[j].transform.position, 0.5f, layer2);
//                         Destroy(coll_to_destroy[0].gameObject);
//                         colliders[j].GetComponent<CustomTag>().has_ore = false;
//                         colliders[j].GetComponent<CustomTag>().has_tree = false;
//                         colliders[j].GetComponent<CustomTag>().taken = false;
//                     }
//                 }
//                 else
//                 {
//                     if (colliders[j].GetComponent<CustomTag>().has_ore == true && Random.Range(0,100) > 70)
//                     {
//                         Collider[] coll_to_destroy = Physics.OverlapSphere(colliders[j].transform.position, 0.5f, layer2);
//                         Destroy(coll_to_destroy[0].gameObject);
//                         SpawnStr.ReplaceTile(colliders[j].gameObject, ore_prefab);
//                         colliders[j].GetComponent<CustomTag>().has_ore = false;
//                        // colliders[j].GetComponent<CustomTag>().taken = false;
//                     }
//                 }        
//             }
//         }

//     }
//     GameObject ins_obj;
//     public static EnemySpawn.Spawn_arr spawn_arr;
//     void Update()
//     {
//         if (is_set == false)
//         {
            
//             if(spawn_location == null)
//             {
//                 range++;
//                 spawn_location = CalculateSpawn(range);
//             }
//             else
//             {
//                 ClearSpawnArea(spawn_location);
//                 is_set = true;  
//                 ins_obj = Instantiate(HQ, spawn_location.transform.position, Quaternion.Euler(new Vector3(90, 0, 0)));
//                 Str_container.structures = new List<GameObject>();
//                 Str_container.taken = new List<GameObject>();
//                 Str_container.structures.Add(ins_obj);
//                 NoiseGrid.Safe_distance(ins_obj, safe_range, false);
                
//                 //*********************************************************************************

//                 PathFinding.InsRange(ins_obj.transform.position);

//                 for (int i = 0; i < HexGrid.size; i++)
//                 {
//                     for (int j = 0; j < HexGrid.size; j++)
//                     {
//                         if (HexGrid.hex_array[i, j] != null)
//                         {
//                             HexGrid.hex_array[i, j].GetComponent<CustomTag>().Rename(0, "None");
//                         }
//                     }
//                 }

//                 //*********************************************************************************

//                 HQ.GetComponent<CustomTag>().taken = true;
//                 HQ.GetComponent<Renderer>().sharedMaterial.color = spawn_location.GetComponent<Renderer>().material.color;
//                 HQ.transform.position = spawn_location.transform.position;
//                 Destroy(spawn_location);
//                 Collider[] neighbours = Physics.OverlapSphere(HQ.transform.position, 1f, layer);
//                 Vector3 HQ_position = new Vector3(HQ.transform.position.x, 0, HQ.transform.position.z);
//                 spawn_arr = EnemySpawn.Tiles_to_spawn(5, 0.5f, 0.5f, 20);
//                 foreach (Collider obj in neighbours)
//                 {
//                     Vector3 position = new Vector3(obj.transform.position.x, 0, obj.transform.position.z);
//                     if (position == HQ_position + HexMetrics.E)
//                     {
//                         Instantiate(garage, obj.transform.position, Quaternion.Euler(new Vector3(90, 0, 0)));
//                         garage.GetComponent<CustomTag>().taken = true;
//                         Destroy(obj.gameObject);
//                     }
//                     else if (position == HQ_position + HexMetrics.NW)
//                     {
//                         Instantiate(garage, obj.transform.position, Quaternion.Euler(new Vector3(90, 0, 0)));
//                         garage.GetComponent<CustomTag>().taken = true;
//                         Destroy(obj.gameObject);
//                     }                  
//                     else if (position == HQ_position + HexMetrics.SW)
//                     {
//                         Instantiate(garage, obj.transform.position, Quaternion.Euler(new Vector3(90, 0, 0)));
//                         garage.GetComponent<CustomTag>().taken = true;
//                         Destroy(obj.gameObject);
                        
//                     }
                        
//                 }

//             }
//         }
//         else
//         {
//             ins_obj.GetComponent<CustomTag>().noise = 50 + TurnEnd.week * 15;
//         }

//     }

// }
