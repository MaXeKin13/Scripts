using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class ClickManager : MonoBehaviour
{
    public GameObject CTA;
    private int clickNum;
    public UnityEvent firstClick;

    public UnityEvent secondClick;

    public UnityEvent firstDelayClick;
    
    private float timePassed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    if(Input.GetMouseButton(0))
        timePassed = 0f;
    else
    {
        timePassed += Time.deltaTime;
    }
        
        
        if (Input.GetMouseButtonDown(0)&& clickNum == 0)
            FirstClick();
        if (Input.GetMouseButtonDown(0)&& clickNum == 0)
            StartCoroutine(firstClickWait());
        if (Input.GetMouseButtonDown(0)&& clickNum == 1)
            SecondClick();

        if (Input.GetMouseButtonDown(0))
            clickNum++;
        
        if(timePassed > 10f)
            CTA.SetActive(true);
    }
    
    public IEnumerator firstClickWait()
    {
        yield return new WaitForSeconds(1);
        firstDelayClick?.Invoke();
    }

    public void FirstClick()
    {
        firstClick?.Invoke();
    }
    public void SecondClick(){secondClick?.Invoke();}
}
