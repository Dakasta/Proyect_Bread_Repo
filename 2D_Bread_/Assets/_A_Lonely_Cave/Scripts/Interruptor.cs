using UnityEngine;

public class Interruptor : MonoBehaviour
{


    public bool wall = false;
    public GameObject puerta;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        puerta.GetComponent<BoxCollider2D>().enabled = false;
        puerta.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Sword"))
        {

            puerta.GetComponent<BoxCollider2D>().enabled = true;
            puerta.SetActive(true);
           
            Debug.Log("MI BOMBOOOOO ");

        }
    }
}
