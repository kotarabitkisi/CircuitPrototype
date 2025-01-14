using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ValueControlPuzzle1 : MonoBehaviour
{
    public gameManager gameManager;
    public SoundManager SoundManager;
    public GameObject glowingSquare;
    public GameObject[] notePages;
    public GameObject[] noteButton;
    [Header("Devre deðerleri ve üsleri")]
    public Complex Z0;
    public double Z0Re, Z0Im;
    public float R, G;
    public float L, C;
    public float Beta, Frequency;
    public float powOfL, powOfC, powOfG, powOfR, powOfFrequency;
    
    [Header("Devre objeleri")]
    [SerializeField] Color TextColor;
    public TextMeshProUGUI Z0Text;
    public GameObject[] devicetexts;
    public TMP_InputField InputFieldR, InputFieldG, InputFieldC, InputFieldL;
    public Slider LSlider, CSlider, GSlider, RSlider;
    [Header("Cevaplar")]
    public float diffWanted;
    public float AnsR, AnsG, AnsL, AnsC, AnsZ0;
    public bool[] ControlAns;//0:R 1:G 2:L 3:C 4:Z0
    [Header("Checkmarklar")]
    public GameObject DevicePlacingCheck;
    public GameObject Z0check;

    public void InitializeSliders(GameObject[] Devices, GameObject[] DevicePlaces)//düzenlenecek
    {
        bool first = true;
        for (int i = 0; i < 4; i++)
        {
            GameObject DaDevice = DevicePlaces[i].transform.GetChild(0).gameObject;
            if (Devices[0] == DaDevice || Devices[2] == DaDevice)
            {
                if (first)
                {
                    RSlider = DevicePlaces[i].transform.GetChild(0).GetChild(1).GetChild(0).gameObject.GetComponent<Slider>();
                    DevicePlaces[i].transform.GetChild(0).GetChild(2).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "R'";
                    InputFieldR = DevicePlaces[i].transform.GetChild(0).GetChild(1).GetChild(1).gameObject.GetComponent<TMP_InputField>();
                    first = false;
                }
                else
                {
                    GSlider = DevicePlaces[i].transform.GetChild(0).GetChild(1).GetChild(0).gameObject.GetComponent<Slider>();
                    DevicePlaces[i].transform.GetChild(0).GetChild(2).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "G'";
                    InputFieldG = DevicePlaces[i].transform.GetChild(0).GetChild(1).GetChild(1).gameObject.GetComponent<TMP_InputField>();
                }
            }
        }
        for (int i = 0; i < devicetexts.Length; i++)
        {
            devicetexts[i].SetActive(true);
            devicetexts[i].GetComponent<TextMeshProUGUI>().color = TextColor;
        }

    }
    public void SliderValueChanged()
    {
        R = RSlider.value;
        L = LSlider.value;
        G = GSlider.value;
        C = CSlider.value;
        InputFieldR.text = R.ToString("F2");
        InputFieldL.text = L.ToString("F2");
        InputFieldG.text = G.ToString("F2");
        InputFieldC.text = C.ToString("F2");
        float Hzfrequency = Frequency * Mathf.Pow(10, powOfFrequency);
        Complex Z0 = SolveZ0(G * Mathf.Pow(10, powOfG), C * Mathf.Pow(10, powOfC), L * Mathf.Pow(10, powOfL), R * Mathf.Pow(10, powOfR), Hzfrequency);
        Z0Text.text = " Z<voffset=-0.3em>0</voffset>= " + Z0.Real.ToString("F2");
        Z0Re = Z0.Real;
        Z0Im = Z0.Imaginary;
    }
    public void InputValueChanged()
    {
        R = float.Parse(InputFieldR.text);
        L = float.Parse(InputFieldL.text);
        G = float.Parse(InputFieldG.text);
        C = float.Parse(InputFieldC.text);
        LSlider.value = L;
        CSlider.value = C;
        RSlider.value = R;
        GSlider.value = G;

        float Hzfrequency = Frequency * Mathf.Pow(10, powOfFrequency);
        Complex Z0 = SolveZ0(G * Mathf.Pow(10, powOfG), C * Mathf.Pow(10, powOfC), L * Mathf.Pow(10, powOfL), R * Mathf.Pow(10, powOfR), Hzfrequency);
        Z0Text.text = " Z<voffset=-0.3em>0</voffset>= " + Z0.Real.ToString("F2");
        Z0Re = Z0.Real;
        Z0Im = Z0.Imaginary;
    }
    public void OpenNote(int a)
    {
        for (int i = 0; i < 3; i++)
        {
            notePages[i].SetActive(false);
            RectTransform ttransform = noteButton[i].GetComponent<RectTransform>();
            ttransform.position = new UnityEngine.Vector3(ttransform.position.x, 900, ttransform.position.z);
        }
        noteButton[a].GetComponent<RectTransform>().position = new UnityEngine.Vector3(noteButton[a].GetComponent<RectTransform>().position.x, 915, noteButton[a].GetComponent<RectTransform>().position.z);
        notePages[a].SetActive(true);
        SoundManager.Playsound(3);
    }
    public void ControlItsTrueOrNot()
    {
        float Hzfrequency = Frequency * Mathf.Pow(10, powOfFrequency);
        double diffC = Mathf.Abs(C - AnsC);
        double diffL = Mathf.Abs(L - AnsL);
        Complex Z0 = SolveZ0(G * Mathf.Pow(10, powOfG), C * Mathf.Pow(10, powOfC), L * Mathf.Pow(10, powOfL), R * Mathf.Pow(10, powOfR), Hzfrequency);
        Complex diffZ0 = Complex.Abs(Z0.Real - AnsZ0);
        bool istrueC = diffC <= diffWanted;
        bool istrueL = diffL <= diffWanted;
        bool istrueZ0 = diffZ0.Real <= diffWanted && diffZ0.Imaginary <= diffWanted;
        Z0Text.text = " Z<voffset=-0.3em>0</voffset>= " + Z0.Real.ToString("F2");
        print("Value");
        if ((istrueC || !ControlAns[3]) && (istrueL || !ControlAns[2]) && (istrueZ0 || !ControlAns[4])) { SoundManager.Playsound(1); Z0check.SetActive(true); print("Z0diff= " + diffZ0 + "true \n Ldiff=" + diffL + "\nCdiff = " + diffC); gameManager.p1SolvingSolved = true; gameManager.puzzNum = 2; gameManager.CloseBtnPressed(); }
        else { print("Z0diff= " + diffZ0 + "\n Ldiff= " + diffL + "\nCdiff = " + diffC); SoundManager.Playsound(0); }
    }
    public Complex SolveZ0(float G, float C, float L, float R, float frequency)
    {
        double omega = 2 * 3.14f * frequency;
        Complex pay = new Complex(R, omega * L);
        Complex payda = new Complex(G, omega * C);
        return Complex.Sqrt(pay / payda);
    }
    public double SolveR(Complex Z0, float G, float C, float frequency)
    {
        double omega = 2 * 3.14f * frequency;
        Complex denominator = new Complex(G, omega * C);
        return (Z0 * Z0 * denominator).Real;
    }

    public double SolveL(Complex Z0, float G, float C, float frequency)
    {
        double omega = 2 * 3.14f * frequency;
        Complex denominator = new Complex(G, omega * C);
        return (Z0 * Z0 * denominator).Imaginary / omega;
    }
    public double SolveG(Complex Z0, float R, float L, float frequency)
    {
        double omega = 2 * 3.14f * frequency;
        Complex numerator = new Complex(R, omega * L);
        return (numerator / (Z0 * Z0)).Real;
    }

    public double SolveC(Complex Z0, float R, float L, float frequency)
    {
        double omega = 2 * 3.14f * frequency;
        Complex numerator = new Complex(R, omega * L);
        return (numerator / (Z0 * Z0)).Imaginary / omega;
    }
}
