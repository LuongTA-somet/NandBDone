using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guide : MonoBehaviour
{
    [SerializeField] private GameObject text;
    [SerializeField] private GameObject guide;
    [SerializeField] private GameObject hand;
    private Animation animGuide;
    private void Start()
    {
        animGuide = GetComponent<Animation>();
    }

    public void ActiveTut()
    {
        if(text != null) text.SetActive(true);
        this.gameObject.SetActive(true);

    }

    public void DisableTut()
    {
        if (text != null) text.SetActive(false);
        animGuide.Play("guidebreak");
        hand.SetActive(false);
        StartCoroutine(NextMove());
      
       
    }
    public IEnumerator NextMove()
    {
      yield return new WaitForSeconds(0.35f);
        guide.SetActive(false );
    }
}
