using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class gameManager : MonoBehaviour
{

    public float leftTime;
    public GameObject OpenedPage;
    public GameObject[] pages;
    public Vector3 cameraPos1, cameraPos2;
    public float cameraScale1, cameraScale2;
    public Color noneColor;
    public bool zoomed,p1PlacingSolved, p1SolvingSolved;
    public GameObject square;
    public TextMeshProUGUI Timetxt;
    private void Update()
    {

        #region süredeðiþimi
        if (leftTime <= 0) { Timetxt.text = "0"; Lose(); }
        else { leftTime -= Time.deltaTime; Timetxt.text = leftTime.ToString("0"); }
        #endregion
        #region yeþilkareyetýklamakontrolü
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousepos, Vector2.zero);
            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("ZoomSquare"))
                {
                    if (!zoomed)
                    {
                        zoomed = true;
                        StartCoroutine(MoveCamera(cameraScale1, cameraPos1));
                        StartCoroutine(OpenPage(1));
                        square.SetActive(false);
                    }
                }
            }//ne olur ne olmaz diye ifleri ayrý býraktým lütfen dokunmayýn
        }
        #endregion

        else if (Input.GetMouseButtonDown(1) && zoomed)//saðtýklakapatma
        {
            CloseBtnPressed();
        }

    }
    public void CloseBtnPressed()
    {
        zoomed = false;
        StartCoroutine(MoveCamera(cameraScale2, cameraPos2));

        StartCoroutine(OpenPage(0));
    }
    public void Lose()
    {
        print("Lose");
    }
    public IEnumerator OpenPage(int id)
    {
        #region seçilen sayfanýn childlarýndaki SpriteRendererlara eriþme(varsa)
        List<GameObject> pagesChild = new List<GameObject>(); GetAllChildObjectsRecursively(pages[id].transform, pagesChild);
        List<SpriteRenderer> pagesRenderer = new List<SpriteRenderer>();
        for (int i = 0; i < pagesChild.Count; i++)
        {
            if (pagesChild[i].TryGetComponent(out SpriteRenderer c))
            {
                pagesRenderer.Add(pagesChild[i].GetComponent<SpriteRenderer>());
            }
        }
        List<GameObject> openedPagesChild = new List<GameObject>(); GetAllChildObjectsRecursively(OpenedPage.transform, openedPagesChild);
        List<SpriteRenderer> openedPagesRenderer = new List<SpriteRenderer>();
        for (int i = 0; i < openedPagesChild.Count; i++)
        {
            if (openedPagesChild[i].TryGetComponent(out SpriteRenderer d))
            {
                openedPagesRenderer.Add(openedPagesChild[i].GetComponent<SpriteRenderer>());
            }
        }
        #endregion
        #region objelerinrenginideðiþtirme
        pages[id].SetActive(true);


        if (OpenedPage.TryGetComponent(out SpriteRenderer a))
        {
            openedPagesRenderer.Add(a);


        }
        if (pages[id].TryGetComponent(out SpriteRenderer b))
        {
            pagesRenderer.Add(b);

        }




        for (float i = 0; i < 101; i++)
        {
            
            for (int j = 0; j < pagesRenderer.Count; j++)
            {
                pagesRenderer[j].color = Color.Lerp(noneColor, Color.white, i / 100);
            }
            for (int j = 0; j < openedPagesRenderer.Count; j++)
            {
                openedPagesRenderer[j].color = Color.Lerp(Color.white, noneColor, i / 100);
            }
            yield return new WaitForSecondsRealtime(0.01f);
        }
        #endregion
        OpenedPage.SetActive(false);
        OpenedPage = pages[id];
        square.SetActive(id == 0 && !p1SolvingSolved);
    }
    public IEnumerator MoveCamera(float cameraScale, Vector3 pos)//kamerayý verilen koordinat ve scale haline getirme animasyonu
    {

        Camera camera = Camera.main;
        float firstScale = camera.orthographicSize;

        camera.transform.DOMove(pos, 1).SetEase(Ease.Linear);
        for (float i = 0; i < 100; i++)
        {
            camera.orthographicSize = Mathf.Lerp(firstScale, cameraScale, i / 100);
            yield return new WaitForSecondsRealtime(0.01f);
        }

    }
    void GetAllChildObjectsRecursively(Transform parent, List<GameObject> childList)//bunu chatgptden çaldým :D
    {
        foreach (Transform child in parent)
        {
            childList.Add(child.gameObject); // Þu anki child'ý listeye ekle
            GetAllChildObjectsRecursively(child, childList); // Alt child'larý da tara
        }
    }
}
