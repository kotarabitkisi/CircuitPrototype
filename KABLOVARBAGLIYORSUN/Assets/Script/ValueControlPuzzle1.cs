using System.Collections;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ValueControlPuzzle1 : MonoBehaviour
{
    public GameObject glowingSquare;
    [Header("Devre de�erleri ve �sleri")]
    public Complex Z0;
    public float Z0Re, Z0Im;
    public float R, G;
    public float L, C;
    public float Beta, Frequency;

    public float powOfL, powOfC, powOfG, powOfR, powOfFrequency;
    [Header("Devre objeleri")]
    public TextMeshProUGUI Z0Text;
    public TMP_InputField InputFieldR, InputFieldG, InputFieldC, InputFieldL;
    public Slider LSlider, CSlider, GSlider, RSlider;
    [Header("Cevaplar")]
    public float diffWanted;
    public float AnsR, AnsG, AnsL, AnsC, AnsZ0;
    [Header("Checkmarklar")]
    public GameObject Z0check;
    public GameObject DevicePlacingCheck;
    public void InitializeSliders(GameObject[] Devices, GameObject[] DevicePlaces)//d�zenlenecek
    {
        RSlider = DevicePlaces[0].transform.GetChild(0).GetChild(1).GetChild(0).gameObject.GetComponent<Slider>();
        DevicePlaces[0].transform.GetChild(0).GetChild(2).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "R'";
        if (Devices[3] != DevicePlaces[2].transform.GetChild(0).gameObject)
        {
            GSlider = DevicePlaces[2].transform.GetChild(0).GetChild(1).GetChild(0).gameObject.GetComponent<Slider>();
            DevicePlaces[2].transform.GetChild(0).GetChild(2).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "G'";

        }
        else { GSlider = DevicePlaces[3].transform.GetChild(0).GetChild(1).GetChild(0).gameObject.GetComponent<Slider>();
            DevicePlaces[3].transform.GetChild(0).GetChild(2).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "G'";
        }
    }


    private void Start()
    {
        Z0 = new Complex(Z0Re, Z0Im);
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
        Z0Text.text = "Z0= " + Z0.Real.ToString("F2");
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
        Z0Text.text = "Z0= " + Z0.Real.ToString("F2");
    }

    public IEnumerator ControlItsTrueOrNot()
    {
        float Hzfrequency = Frequency * Mathf.Pow(10, powOfFrequency);
        double diffC = Mathf.Abs(C - AnsC);
        double diffL = Mathf.Abs(L - AnsL);
        Complex Z0 = SolveZ0(G * Mathf.Pow(10, powOfG), C * Mathf.Pow(10, powOfC), L * Mathf.Pow(10, powOfL), R * Mathf.Pow(10, powOfR), Hzfrequency);
        Complex diffZ0 = Complex.Abs(Z0 - AnsZ0);
        bool istrueC = diffC <= diffWanted;
        bool istrueL = diffL <= diffWanted;
        bool istrueZ0 = diffZ0.Real <= diffWanted && diffZ0.Imaginary <= diffWanted;

        for (int i = 0; i < 101; i++)
        {
            Z0Text.text = "Z0= " + Mathf.Lerp(0, (float)Z0.Real, (float)i / 100).ToString("F2");
            yield return new WaitForSecondsRealtime(0.01f);
        }



        if (istrueC && istrueL && istrueZ0) { Z0check.SetActive(true); print("Z0diff= " + diffZ0 + "true \n Ldiff=" + diffL + "\nCdiff = " + diffC); GetComponent<gameManager>().p1SolvingSolved = true; GetComponent<gameManager>().CloseBtnPressed(); }
        else { print("Z0diff= " + diffZ0 + "\n Ldiff= " + diffL + "\nCdiff = " + diffC); }
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
