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
        public KeyValuePair<GameObject, int>[,] stones = new KeyValuePair<GameObject, int>[2, 10];
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
                    if (stones[i, j].Key != null) {
                        GameObject stone = stones[i,j].Key;
                        stone.transform.localScale = new Vector3(scale / 5, scale / 5, scale / 5);
                        stone.GetComponent<SpriteRenderer>().sprite = sprites1[0];
                        stones[i,j] = new KeyValuePair<GameObject, int>(stone, 0);
                    }
                }
            }
            drawStones();
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
                    GameObject limit = Instantiate(limicator);    //makes transparent planes on each grid squar
                    limit.transform.localPosition = i == 0 ? new Vector3(-6.6f, 0f, -3.45f + j* (.75f)) : new Vector3(6.8f, 0f, -3.45f + j* (.75f));
                    limit.transform.Rotate(90, 0, 0);
                    limit.transform.localScale = new Vector3(scale, scale, scale);
                    limit.GetComponent<Renderer>().material.color = new Vector4(1f, 1f, 1f, .5f);
                    KeyValuePair<GameObject, int> pair = new KeyValuePair<GameObject, int>(limit, 0);
                    stones[i, j] = pair;
                }
            }
            p1StonesPlaced = 0;
            p2StonesPlaced = 0;
        }
        
        public void drawStones()
        {
            for(int i = 0; i < 2; i++){
                for (int j = 0; j < 10; j++)
                {
                    GameObject stone = stones[i, j].Key;
                    int code = stones[i, j].Value;
                    stone.transform.localScale = code == 0 ? new Vector3(scale, scale, scale) : new Vector3(scale/5, scale/5, scale/5);
                    stone.GetComponent<SpriteRenderer>().sprite = i == 0 ? sprites1[code] : sprites2[code];
                    stone.GetComponent<Renderer>().material.color = code == 0 ? new Vector4(1f, 1f, 1f, .5f) : new Vector4(1f,1f,1f,1f);
                    stones[i,j] = new KeyValuePair<GameObject, int>(stone, code);
                }
            }
        }

        public void changeStones(int s, State state, Building build)
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
            int stonesPlaced = s == 0 ? p1StonesPlaced : p2StonesPlaced;
            //if(stones != null)
            if (state == State.placing)//could probably add the drawing part here to the drawStones function
            {
                int code = animal;
                if (s == 0)
                {
                    GameObject stone = stones[s, p1StonesPlaced].Key;
                    stone.transform.localScale = new Vector3(scale / 5, scale / 5, scale / 5);
                    stone.GetComponent<SpriteRenderer>().sprite = sprites1[animal];
                    stone.GetComponent<Renderer>().material.color = new Vector4(1f, 1f, 1f, 1f);
                    stones[s, p1StonesPlaced] = new KeyValuePair<GameObject, int>(stone, code);
                    p1StonesPlaced = Mathf.Min(10, p1StonesPlaced + 1);
                }
                else
                {
                    GameObject stone = stones[s, p2StonesPlaced].Key;
                    stone.transform.localScale = new Vector3(scale / 5, scale / 5, scale / 5);
                    stone.GetComponent<SpriteRenderer>().sprite = sprites2[animal];
                    stones[s, p2StonesPlaced] = new KeyValuePair<GameObject, int>(stone, code);
                    p2StonesPlaced = Mathf.Min(10, p2StonesPlaced + 1);
                }
            }
            else if (state == State.removing)
            {
                if (s == 0) p1StonesPlaced = Mathf.Max(0, p1StonesPlaced - 1);
                else p2StonesPlaced = Mathf.Max(0, p2StonesPlaced - 1);
                int order = -1;
                for(int i = stones.GetLength(1)-1; i >=0; i--) if(stones[s,i].Value == animal) order = i;
                for (int i = order; i < stones.GetLength(1); i++)
                {
                    if (i >= 0 && s >= 0 && stones.GetLength(0) > s && stones.GetLength(1) > i) { // added error check, might cause issues ?
                        GameObject go = stones[s, i].Key;
                        if (i == stones.GetLength(1) - 1) stones[s, i] = new KeyValuePair<GameObject, int>(go, 0);
                        else stones[s, i] = new KeyValuePair<GameObject, int>(go, stones[s, i + 1].Value);
                    }
                }
                drawStones();
            }
        }
    }
}
