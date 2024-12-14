using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class deviceControl : MonoBehaviour
{
    public gameManager GM;
    public ValueControlPuzzle1 VCP;
    public Button ControlBtn;
    public Vector3 firstPosition;
    public Transform firstParent;
    public GameObject ChosenDevice;
    public GameObject[] valueChangeCanvas;
    public GameObject[] devices;
    public GameObject[] selectedDevicePlaces;
    public Vector3[] DeviceTransforms;
    private void Start()
    {
        for (int i = 0; i < devices.Length; i++) { DeviceTransforms[i] = devices[i].transform.localPosition; }
    }
    void Update()
    {
        #region parcayadokunmaraycastkontrolu
        if (Input.GetMouseButtonDown(0)&&!GM.p1PlacingSolved)
        {
            Vector2 mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousepos, Vector2.zero);
            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("Device"))
                {
                    firstPosition = hit.collider.transform.position;
                    firstParent = hit.collider.transform.parent;
                    ChosenDevice = hit.collider.gameObject;
                    ChosenDevice.transform.parent = null;
                    ChosenDevice.GetComponent<BoxCollider2D>().enabled = false;
                    for (int i = 0; i < selectedDevicePlaces.Length; i++)//bug yaþanmamasý adýna yerlerin boxcolliderlarý geçici süreliðine kapanýyordu burada açýlýyor
                    {
                        selectedDevicePlaces[i].GetComponent<BoxCollider2D>().enabled = true;
                    }
                }
            }
        }
        #endregion
        #region parcayýsürükleme
        else if (Input.GetMouseButton(0) && ChosenDevice != null&&!GM.p1PlacingSolved)
        {







            Vector2 mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            ChosenDevice.transform.position = mousepos;
        }
        #endregion
        #region parçayýbýrakma
        else if (Input.GetMouseButtonUp(0) && ChosenDevice != null && !GM.p1PlacingSolved)
        {
            Vector2 mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousepos, Vector2.zero);
            #region deviceplacedebýrakmakontrolu
            if (hit.collider != null && hit.collider.CompareTag("DevicePlace") && hit.collider.gameObject.transform.childCount == 0)//3.koþul orada zaten aygýt olup olmadýðýnýn kontrolü
            {
                ChosenDevice.transform.position = hit.collider.transform.position;
                ChosenDevice.transform.GetChild(0).gameObject.transform.rotation = hit.collider.transform.rotation;
                ChosenDevice.transform.parent = hit.collider.transform;
            }
            else
            {
                ChosenDevice.transform.position = firstPosition;
                ChosenDevice.transform.parent = firstParent;
            }
            #endregion
            ChosenDevice.GetComponent<BoxCollider2D>().enabled = true;
            ChosenDevice = null;
            for (int i = 0; i < selectedDevicePlaces.Length; i++)//bug yaþanmamasý adýna yerlerin boxcolliderlarý geçici süreliðine kapanýyor
            {
                selectedDevicePlaces[i].GetComponent<BoxCollider2D>().enabled = false;
            }
        }
        else if(GM.p1PlacingSolved)
        {
            Vector2 mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousepos, Vector2.zero);

            if (hit.collider != null && hit.collider.CompareTag("Device"))
            {
                for (int i = 0; i < valueChangeCanvas.Length; i++)
                {
                    if (devices[i] == hit.collider.gameObject) { valueChangeCanvas[i].SetActive(true); }
                    else { valueChangeCanvas[i].SetActive(false); }
                }
            }
            else
            {
                for (int i = 0; i < valueChangeCanvas.Length; i++)
                {
                    valueChangeCanvas[i].SetActive(false);
                } 
            }

        }
        #endregion
    }
    public void ControlDevicesArePuttedCorrectly()//doðru eþleþmelerin elemanlarýnýn ayný indexte olmasý önemli :( 
    {
        for (int i = 0; i < selectedDevicePlaces.Length; i++)
        {
            if (selectedDevicePlaces[i].transform.childCount == 0)//önce bütün aygýtlarýn yerleþtirildiðini kontrol ediyor
            {
                print("false");
                for (int j = 0; j < devices.Length; j++)//eðer yanlýþlýk varsa aygýtlarý yerlerine geri koyup baþtan baþlatýyor
                {
                    devices[j].transform.parent = transform;
                    devices[j].transform.DOLocalMove(DeviceTransforms[j], 0.5f).SetEase(Ease.Linear);
                }
                return;
            }

        }
        //if (!(devices[0] == selectedDevicePlaces[0].transform.GetChild(0).gameObject || devices[0]== selectedDevicePlaces[3].transform.GetChild(0).gameObject || devices[0] == selectedDevicePlaces[2].transform.GetChild(0).gameObject)) {
        //    print("false");
        //    for (int j = 0; j < devices.Length; j++)//eðer yanlýþlýk varsa aygýtlarý yerlerine geri koyup baþtan baþlatýyor
        //    {
        //        devices[j].transform.parent = transform;
        //        devices[j].transform.DOLocalMove(DeviceTransforms[j], 0.5f).SetEase(Ease.Linear);
        //    }
        //    return;
        //}
        //else if (devices[1] != selectedDevicePlaces[1].transform.GetChild(0).gameObject)
        //{
        //    print("false");
        //    for (int j = 0; j < devices.Length; j++)//eðer yanlýþlýk varsa aygýtlarý yerlerine geri koyup baþtan baþlatýyor
        //    {
        //        devices[j].transform.parent = transform;
        //        devices[j].transform.DOLocalMove(DeviceTransforms[j], 0.5f).SetEase(Ease.Linear);
        //    }
        //    return;
        //}
        //else if (!(devices[2] == selectedDevicePlaces[3].transform.GetChild(0).gameObject || devices[2] == selectedDevicePlaces[2].transform.GetChild(0).gameObject || devices[3] == selectedDevicePlaces[2].transform.GetChild(0).gameObject || devices[3] == selectedDevicePlaces[3].transform.GetChild(0).gameObject))
        //{
        //    print("false");
        //    for (int j = 0; j < devices.Length; j++)//eðer yanlýþlýk varsa aygýtlarý yerlerine geri koyup baþtan baþlatýyor
        //    {
        //        devices[j].transform.parent = transform;
        //        devices[j].transform.DOLocalMove(DeviceTransforms[j], 0.5f).SetEase(Ease.Linear);
        //    }
        //    return;
        //}
        List<GameObject> deviceplaceschild = new List<GameObject>();
        for (int i = 0; i < 4; i++)
        {
            deviceplaceschild.Add(selectedDevicePlaces[i].transform.GetChild(0).gameObject);
            print(deviceplaceschild[i]);
        }
        if (!(devices[0] == deviceplaceschild[0] || devices[0] == deviceplaceschild[2] || devices[0] == deviceplaceschild[3])
            || !(devices[1] == deviceplaceschild[1])
            || !(devices[2] == deviceplaceschild[2] || devices[2] == deviceplaceschild[3])
            || !(devices[3] == deviceplaceschild[2] || devices[3] == deviceplaceschild[3]))
        {
            print("false");
            for (int j = 0; j < devices.Length; j++)//eðer yanlýþlýk varsa aygýtlarý yerlerine geri koyup baþtan baþlatýyor
            {
                devices[j].transform.parent = transform;
                devices[j].transform.DOLocalMove(DeviceTransforms[j], 0.5f).SetEase(Ease.Linear);
            }
            return;
        }


        for (int i = 0; i < devices.Length; i++)//yanlýþ eþleþtirme bulmazsa boxcolliderlarý kapatarak bir daha dokunulmamasýný saðlar
        {
            //buraya sliderlarý aktif etme satýrý gerek
            devices[i].GetComponent<BoxCollider2D>().enabled = false;
        }
        ControlBtn.GetComponent<Button>().onClick.RemoveAllListeners();
        ControlBtn.GetComponent<Button>().onClick.AddListener(() => StartCoroutine(VCP.ControlItsTrueOrNot()));
        for (int i = 0; i < selectedDevicePlaces.Length; i++)
        {
            selectedDevicePlaces[i].GetComponent<BoxCollider2D>().enabled = false;
        }
        for (int i = 0; i < devices.Length; i++)
        {
            devices[i].GetComponent<BoxCollider2D>().enabled = true;
        }
        GM.p1PlacingSolved = true;
        VCP.DevicePlacingCheck.SetActive(true);
        print("true");//for döngülerini birleþtirme hepsinin sýrasý önemli
    }

}

