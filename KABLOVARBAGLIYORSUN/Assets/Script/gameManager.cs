using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class gameManager : MonoBehaviour
{
    public GameObject[] AllFormulaPages;
    public GameObject LosePanel;
    public float leftTime;
    public GameObject OpenedPage;
    public GameObject[] pages;
    public Vector3[] cameraPos;
    public float[] cameraScale;
    public Color noneColor;
    public bool zoomed, p1PlacingSolved, p1SolvingSolved;
    public int puzzNum;
    public GameObject[] squares;
    public TextMeshProUGUI Timetxt;
    [Header("LastPuzzle")]
    public bool LastPuzzStarted;
    public GameObject LastPuzzleCanvas;
    private void Update()
    {
        if (!LastPuzzStarted && puzzNum == 3)
        {
            LastPuzzStarted = true;
            Invoke("LastPuzzleStart", 2);
        }
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
                        StartCoroutine(MoveCamera(cameraScale[puzzNum], cameraPos[puzzNum]));
                        StartCoroutine(OpenPage(puzzNum));
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
        StartCoroutine(MoveCamera(cameraScale[0], cameraPos[0]));
        StartCoroutine(OpenPage(0));
    }
    public void Lose()
    {
        LosePanel.SetActive(true);
    }
    public void LastPuzzleStart()
    {
        StartCoroutine(MoveCamera(5, new Vector3(1.5f, -1, -10)));
        LastPuzzleCanvas.SetActive(true);
    }
    public IEnumerator OpenPage(int id)
    {
        for (int i = 0; i < squares.Length; i++)
        {
            squares[i].GetComponent<colorChanging>().enabled = false;
        }
        #region seçilen sayfanýn childlarýndaki SpriteRendererlara eriþme(varsa)
        List<GameObject> pagesChild = new List<GameObject>(); GetAllChildObjectsRecursively(pages[id].transform, pagesChild);
        List<SpriteRenderer> pagesRenderer = new List<SpriteRenderer>();
        List<Image> pagesImage = new List<Image>();
        List<TextMeshProUGUI> pagesText = new List<TextMeshProUGUI>();
        for (int i = 0; i < pagesChild.Count; i++)
        {
            if (pagesChild[i].TryGetComponent(out SpriteRenderer c))
            {
                pagesRenderer.Add(c);
            }
            if (pagesChild[i].TryGetComponent(out TextMeshProUGUI d))
            {
                pagesText.Add(d);
            }
            if (pagesChild[i].TryGetComponent(out Image e))
            {
                pagesImage.Add(e);
            }
        }
        List<GameObject> openedPagesChild = new List<GameObject>(); GetAllChildObjectsRecursively(OpenedPage.transform, openedPagesChild);
        List<SpriteRenderer> openedPagesRenderer = new List<SpriteRenderer>();
        List<Image> openedPagesImage = new List<Image>();
        List<TextMeshProUGUI> openedPagesText = new List<TextMeshProUGUI>();
        for (int i = 0; i < openedPagesChild.Count; i++)
        {
            if (openedPagesChild[i].TryGetComponent(out SpriteRenderer d))
            {
                openedPagesRenderer.Add(d);
            }
            else if (openedPagesChild[i].TryGetComponent(out TextMeshProUGUI e))
            {
                openedPagesText.Add(e);
            }
            else if (openedPagesChild[i].TryGetComponent(out Image f))
            {
                openedPagesImage.Add(f);
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


        float duration = 1f; // 1 saniye
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration; // Ýlerleme yüzdesi

            for (int j = 0; j < pagesRenderer.Count; j++)
                pagesRenderer[j].color = Color.Lerp(noneColor, Color.white, t);

            for (int j = 0; j < openedPagesRenderer.Count; j++)
                openedPagesRenderer[j].color = Color.Lerp(Color.white, noneColor, t);

            for (int j = 0; j < pagesImage.Count; j++)
                pagesImage[j].color = Color.Lerp(noneColor, Color.white, t);

            for (int j = 0; j < openedPagesImage.Count; j++)
                openedPagesImage[j].color = Color.Lerp(Color.white, noneColor, t);

            for (int j = 0; j < pagesText.Count; j++)
                pagesText[j].color = Color.Lerp(noneColor, Color.black, t);

            for (int j = 0; j < openedPagesText.Count; j++)
                openedPagesText[j].color = Color.Lerp(Color.black, noneColor, t);

            yield return null; // Bir sonraki kareyi bekle
        }


        //for (float i = 0; i < 101; i++)
        //{

        //    for (int j = 0; j < pagesRenderer.Count; j++)
        //    {
        //        pagesRenderer[j].color = Color.Lerp(noneColor, Color.white, i / 100);
        //    }
        //    for (int j = 0; j < openedPagesRenderer.Count; j++)
        //    {
        //        openedPagesRenderer[j].color = Color.Lerp(Color.white, noneColor, i / 100);
        //    }
        //    for (int j = 0; j < pagesImage.Count; j++)
        //    {
        //        pagesImage[j].color = Color.Lerp(noneColor, Color.white, i / 100);
        //    }
        //    for (int j = 0; j < openedPagesImage.Count; j++)
        //    {
        //        openedPagesImage[j].color = Color.Lerp(Color.white, noneColor, i / 100);
        //    }
        //    for (int j = 0; j < pagesText.Count; j++)
        //    {
        //        pagesText[j].color = Color.Lerp(noneColor, Color.black, i / 100);
        //    }
        //    for (int j = 0; j < openedPagesText.Count; j++)
        //    {
        //        openedPagesText[j].color = Color.Lerp(Color.black, noneColor, i / 100);
        //    }
        //    yield return new WaitForSecondsRealtime(0.01f);
        //}
        for (int i = 0; i < squares.Length; i++)
        {
            squares[i].GetComponent<colorChanging>().enabled = true;
        }
        #endregion
        OpenedPage.SetActive(false);
        OpenedPage = pages[id];
        for (int i = 0; i < squares.Length; i++)
        {
            squares[i].SetActive(i == puzzNum - 1 && id == 0);
        }

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

    public void openFormula(GameObject FormulaPage)
    {
        CloseAllFormulas();
        if (FormulaPage != null)
        {
            FormulaPage.SetActive(true);
        }
    }
    public void CloseAllFormulas()
    {
        for (int i = 0; i < AllFormulaPages.Length; i++)
        {
            AllFormulaPages[i].SetActive(false);
        }
    }
}
