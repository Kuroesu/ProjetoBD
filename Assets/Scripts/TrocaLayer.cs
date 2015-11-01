using UnityEngine;
using System.Collections;

public class TrocaLayer : MonoBehaviour {
    private SpriteRenderer render;
    private PlayerBehavior player;
    private EnemyBehavior enemy;

    // Use this for initialization
    void Start () {
        render = gameObject.GetComponent<SpriteRenderer>();
	}

    // Update is called once per frame
    void Update(){
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.transform.position.y < gameObject.transform.position.y-1)
        {
            render.sortingOrder = 1;
        }
        else
        {
            render.sortingOrder = 7;
        }
    }
}
