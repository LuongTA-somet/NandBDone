using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenSide : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject destroyer;
   [SerializeField] private GameObject darker;
    [SerializeField] private GameObject retryBtn;

    [SerializeField] private Raycast raycast;
  [SerializeField]  private SpriteRenderer darkerRenderer;
   [SerializeField] private Color dark = new Color(70, 70, 70);
   [SerializeField] private Color old = new Color(255, 255, 255);
    public GameObject firework;
    public bool isStart = false;
    private bool isOne = false;
    void Start()
    {

        StartCoroutine(CameraMove());
        StartCoroutine(DestroyerMove());
    }
    private void Update()
    {
        if (raycast.isEnd)
        {
            if (!isOne)
            {
                isOne = true;
                StartCoroutine(ReturnPos());
            }
        }
    }


    private void FadeToDarker(Color color)
    {
        
        if (darkerRenderer != null)
        {           
            darkerRenderer.DOColor(color, 0.5f); 
        }
    }

    private IEnumerator CameraMove()
    {
        yield return new WaitForSeconds(0.8f);
        mainCamera.DOOrthoSize(1.8f, 1f);
        mainCamera.transform.DOMove(new Vector3(0, -1.72f, -10), 1f);
        yield return new WaitForSeconds(1f);
        GameManager.instance.handStart.ActiveTut();
        FadeToDarker(dark);
        isStart = true;
        
    }
    private IEnumerator ReturnPos()
    {
        firework.SetActive(true);
        GameManager.instance.soundController.bg.Stop();
        GameManager.instance.soundController.fx.Stop();
        GameManager.instance.soundController.PlaySoundFX(TypeSound.Win);
        yield return new WaitForSeconds(1f);
        mainCamera.DOOrthoSize(3.5f, 1f);
        mainCamera.transform.DOMove(new Vector3(0, -0.098f, -10), 1f);
        destroyer.gameObject.transform.DOMove(new Vector3(0, -3.55f, 0), 1f);
        yield return new WaitForSeconds(1f);
        FadeToDarker(old);
        GameManager.instance.handTut.ActiveTut();
        GameManager.instance.EndGame();
    }
    private IEnumerator DestroyerMove()
    {
        yield return new WaitForSeconds(1.8f);
        destroyer.SetActive(true);
        destroyer.gameObject.transform.DOMove(new Vector3(0, -3.534f, 0), 1f).SetEase(Ease.OutBack);
    }
}
