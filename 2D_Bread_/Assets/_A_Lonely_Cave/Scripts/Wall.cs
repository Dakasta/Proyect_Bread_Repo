using UnityEngine;

public class Wall : MonoBehaviour
{


    public bool wall = false;
    public GameObject puerta;
    public GameObject Boss;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        Boss.GetComponent<BoxCollider2D>().enabled = false;
        Boss.SetActive(false);

        puerta.GetComponent<BoxCollider2D>().enabled = false;
        puerta.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {

            puerta.GetComponent<BoxCollider2D>().enabled = true;
            puerta.SetActive(true);
            Boss.GetComponent<BoxCollider2D>().enabled = true;
            Boss.SetActive(true);
           
        }
    }
}
