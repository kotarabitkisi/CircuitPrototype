using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class deviceControl : MonoBehaviour
{
    public ValueControlPuzzle1 VCP;
    public Button ControlBtn;
    public Vector3 firstPosition;
    public Transform firstParent;
    public GameObject ChosenDevice;
    public GameObject[] valueChangeCanvas;
    public GameObject[] devices;
    public GameObject[] selectedDevicePlaces;
    public Vector3[] DeviceTransforms;
    void Update()
    {
        #region parcayadokunmaraycastkontrolu
        if (Input.GetMouseButtonDown(0))
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
                    for (int i = 0; i < selectedDevicePlaces.Length; i++)//bug ya�anmamas� ad�na yerlerin boxcolliderlar� ge�ici s�reli�ine kapan�yordu burada a��l�yor
                    {
                        selectedDevicePlaces[i].GetComponent<BoxCollider2D>().enabled = true;
                    }
                }
            }
        }
        #endregion
        #region parcay�s�r�kleme
        else if (Input.GetMouseButton(0) && ChosenDevice != null)
        {
            Vector2 mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            ChosenDevice.transform.position = mousepos;
        }
        #endregion
        #region par�ay�b�rakma
        else if (Input.GetMouseButtonUp(0) && ChosenDevice != null)
        {

            Vector2 mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousepos, Vector2.zero);
            #region deviceplacedeb�rakmakontrolu
            if (hit.collider != null && hit.collider.CompareTag("DevicePlace") && hit.collider.gameObject.transform.childCount == 0)//3.ko�ul orada zaten ayg�t olup olmad���n�n kontrol�
            {
                ChosenDevice.transform.position = hit.collider.transform.position;
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
            for (int i = 0; i < selectedDevicePlaces.Length; i++)//bug ya�anmamas� ad�na yerlerin boxcolliderlar� ge�ici s�reli�ine kapan�yor
            {
                selectedDevicePlaces[i].GetComponent<BoxCollider2D>().enabled = false;
            }
        }
        #endregion
    }
    public void ControlDevicesArePuttedCorrectly()//do�ru e�le�melerin elemanlar�n�n ayn� indexte olmas� �nemli :( 
    {
        for (int i = 0; i < selectedDevicePlaces.Length; i++)
        {
            if (selectedDevicePlaces[i].transform.childCount == 0)//�nce b�t�n ayg�tlar�n yerle�tirildi�ini kontrol ediyor
            {
                print("false");
                for (int j = 0; j < devices.Length; j++)//e�er yanl��l�k varsa ayg�tlar� yerlerine geri koyup ba�tan ba�lat�yor
                {
                    devices[j].transform.parent = transform;
                    devices[j].transform.DOLocalMove(DeviceTransforms[j], 0.5f).SetEase(Ease.Linear);
                }
                return;
            }

        }
        for (int i = 0; i < devices.Length - 2; i++)
        {
            if (devices[i] != selectedDevicePlaces[i].transform.GetChild(0).gameObject)
            {
                print(i);
                print("false");
                for (int j = 0; j < devices.Length; j++)//e�er yanl��l�k varsa ayg�tlar� yerlerine geri koyup ba�tan ba�lat�yor
                {
                    devices[j].transform.parent = transform;
                    devices[j].transform.DOLocalMove(DeviceTransforms[j], 0.5f).SetEase(Ease.Linear);
                }
                return;
            }
        }
        if (!(devices[2] == selectedDevicePlaces[3].transform.GetChild(0).gameObject || devices[2] == selectedDevicePlaces[2].transform.GetChild(0).gameObject))
        {
            print("false");
            for (int j = 0; j < devices.Length; j++)//e�er yanl��l�k varsa ayg�tlar� yerlerine geri koyup ba�tan ba�lat�yor
            {
                devices[j].transform.parent = transform;
                devices[j].transform.DOLocalMove(DeviceTransforms[j], 0.5f).SetEase(Ease.Linear);
            }
            return;
        }
        if (!(devices[3] == selectedDevicePlaces[2].transform.GetChild(0).gameObject || devices[3] == selectedDevicePlaces[3].transform.GetChild(0).gameObject))
        {
            print("false");
            for (int j = 0; j < devices.Length; j++)//e�er yanl��l�k varsa ayg�tlar� yerlerine geri koyup ba�tan ba�lat�yor
            {
                devices[j].transform.parent = transform;
                devices[j].transform.DOLocalMove(DeviceTransforms[j], 0.5f).SetEase(Ease.Linear);
            }
            return;
        }

        for (int i = 0; i < devices.Length; i++)//yanl�� e�le�tirme bulmazsa boxcolliderlar� kapatarak bir daha dokunulmamas�n� sa�lar
        {
            //buraya sliderlar� aktif etme sat�r� gerek
            devices[i].GetComponent<BoxCollider2D>().enabled = false;
        }

        for (int i = 0; i < valueChangeCanvas.Length; i++)
        {
            valueChangeCanvas[i].SetActive(true);
        }
        ControlBtn.GetComponent<Button>().onClick.RemoveAllListeners();
        ControlBtn.GetComponent<Button>().onClick.AddListener(VCP.ControlItsTrueOrNot);
        print("true");//for d�ng�lerini birle�tirme hepsinin s�ras� �nemli
    }
}
