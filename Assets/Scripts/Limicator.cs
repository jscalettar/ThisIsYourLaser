using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Limicator : MonoBehaviour
{
    public static LimicatorObject limicatorObj;
    public GameObject limicator;
    public Sprite[] sprites1;
    public Sprite[] sprites2;
    public float scale;

    // Use this for initialization
    void Awake()
    {
        limicatorObj = new LimicatorObject(limicator, sprites1, sprites2, scale);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public class LimicatorObject 
    {
        public GameObject[,] stones = new GameObject[2, 10];
        //[HideInInspector]
        public int p1StonesPlaced;
        //[HideInInspector]
        public int p2StonesPlaced;
        public float scale;
        public Sprite[] sprites1;
        public Sprite[] sprites2;
        public GameObject limicator;

        public void reset()
        {
            for (int i = 0; i < stones.GetLength(0); i++) {
                for (int j = 0; j < stones.GetLength(1); j++) {
                    if (stones[i, j] != null) {
                        stones[i, j].GetComponent<SpriteRenderer>().sprite = limicator.GetComponent<SpriteRenderer>().sprite;
                        stones[i, j].transform.localScale = new Vector3(scale, scale, scale);
                    }
                }
            }
            p1StonesPlaced = 0;
            p2StonesPlaced = 0;
        }

        public LimicatorObject(GameObject limiter, Sprite[] sprits1, Sprite[] sprits2, float scal)
        {
            limicator = limiter;
            sprites1 = sprits1;
            sprites2 = sprits2;
            scale = scal;
            for(int i = 0; i< 2; i++){
                for (int j = 0; j< 10; j++)
                {
                    GameObject limit = Instantiate(limicator);    //makes transparent planes on each grid square
                    limit.transform.localPosition = i == 0 ? new Vector3(-6.6f, 0f, -3.45f + j* (.75f)) : new Vector3(6.8f, 0f, -3.45f + j* (.75f));
                    limit.transform.Rotate(90, 0, 0);
                    limit.transform.localScale = new Vector3(scale, scale, scale);
                    stones[i, j] = limit;
                }
            }
            p1StonesPlaced = 0;
            p2StonesPlaced = 0;
        }
        

        public void changeStones(int i, State state, Building build)
        {
            int animal = 0;
            switch (build)
            {
                case Building.Blocking: animal = 1; break;
                case Building.Reflecting: animal = 2; break;
                case Building.Refracting: animal = 3; break;
                case Building.Redirecting: animal = 4; break;
                case Building.Resource: animal = 5; break;
                default: animal = 0; break;
            }
            int stonesPlaced = i == 0 ? p1StonesPlaced : p2StonesPlaced;
            if(stones != null)
            if (state == State.placing)
            {
                if (i == 0)
                {
                    stones[i, p1StonesPlaced].transform.localScale = new Vector3(scale / 5, scale / 5, scale / 5);
                    stones[i, p1StonesPlaced].GetComponent<SpriteRenderer>().sprite = sprites1[animal];
                    p1StonesPlaced = Mathf.Min(10, p1StonesPlaced + 1);
                }
                else
                {
                    stones[i, p2StonesPlaced].transform.localScale = new Vector3(scale / 5, scale / 5, scale / 5);
                    stones[i, p2StonesPlaced].GetComponent<SpriteRenderer>().sprite = sprites2[animal];
                    p2StonesPlaced = Mathf.Min(10, p2StonesPlaced + 1);
                }
            }
            else if (state == State.removing)
            {
                if (i == 0)
                {
                    p1StonesPlaced = Mathf.Max(0, p1StonesPlaced - 1);
                    stones[i, p1StonesPlaced].transform.localScale = new Vector3(scale, scale, scale);
                    stones[i, p1StonesPlaced].GetComponent<SpriteRenderer>().sprite = sprites1[animal];
                }
                else
                {
                    p2StonesPlaced = Mathf.Max(0, p2StonesPlaced - 1);
                    stones[i, p2StonesPlaced].transform.localScale = new Vector3(scale, scale, scale);
                    stones[i, p2StonesPlaced].GetComponent<SpriteRenderer>().sprite = sprites2[animal];
                }
            }
        }
    }
}
