using System.Numerics;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class valueControlPuzzle2 : MonoBehaviour
{
    public GameObject[] notePages;
    public double CL, RL, frequency, Z0;
    public float powofCL, powofRL, powofFrequency;
    public double Magnitude, Phase;
    public Slider clSlider, rlSlider;
    public TMP_InputField clInputField, rlInputField;
    public double AnsMagnitude, AnsPhase;
    public TextMeshProUGUI MagnitudeText, PhaseText;
    public double diffWanted;
    public GameObject[] valueChangeCanvas;
    public GameObject[] devices;
    public gameManager GameManager;
    private void Update()
    {
        UnityEngine.Vector2 mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousepos, UnityEngine.Vector2.zero);

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
    public (double Magnitude, double Phase) SolveReflectionCoefficient(double RL, double CL, Complex Z0, double frequency)
    {
        Complex ZL = new Complex(RL, -1 / (2 * Mathf.PI * frequency * CL));

        Complex Z0Complex = new Complex(Z0.Real, 0);
        Complex Gamma = (ZL - Z0Complex) / (ZL + Z0Complex);

        // Büyüklük ve faz açýsý
        double magnitude = Gamma.Magnitude;
        double phase = Gamma.Phase * (180 / Mathf.PI);  // Radyan cinsinden dereceye çevirme
        return (magnitude, phase);
    }
    public void SliderValueChanged()
    {
        RL = rlSlider.value;
        CL = clSlider.value;
        rlInputField.text=RL.ToString("F2");
        clInputField.text = CL.ToString("F2");
        double Hzfrequency = frequency * Mathf.Pow(10, powofFrequency);

        (Magnitude, Phase) = SolveReflectionCoefficient(RL * Mathf.Pow(10, powofRL), CL * Mathf.Pow(10, powofCL), Z0, frequency * Mathf.Pow(10, powofFrequency));
        MagnitudeText.text = "|r|= " + Magnitude.ToString("F2");
        PhaseText.text = "Angle= " + Phase.ToString("F2");
    }
    public void InputValueChanged()
    {
        RL = float.Parse(rlInputField.text);
        CL = float.Parse(clInputField.text);
        rlSlider.value = (float)RL;
        clSlider.value = (float)CL;
        double Hzfrequency = frequency * Mathf.Pow(10, powofFrequency);

        (Magnitude, Phase) = SolveReflectionCoefficient(RL*Mathf.Pow(10, powofRL),CL*Mathf.Pow(10, powofCL), Z0,frequency* Mathf.Pow(10, powofFrequency));
        MagnitudeText.text = "|r|= " + Magnitude.ToString("F2");
        PhaseText.text = "Angle= " + Phase.ToString("F2");
    }

    public void Run()
    {
        (double magnitude, double phase) = SolveReflectionCoefficient(RL * Mathf.Pow(10, powofRL), CL * Mathf.Pow(10, powofCL), Z0, frequency * Mathf.Pow(10, powofFrequency));
        float diffphase = (float)(AnsPhase - phase);
        float diffmagnitude = (float)(AnsMagnitude - magnitude);
        if (Mathf.Abs(diffphase) <= diffWanted && Mathf.Abs(diffmagnitude) <= diffWanted)
        {
            print("true");  GameManager.puzzNum = 3; GameManager.CloseBtnPressed();
        }
        else { print("false" + "\ndiffphase= " + diffphase.ToString("F2") + "\ndiffmagnitude= " + diffmagnitude.ToString("F2")); }

    }
    public void OpenNote(int a)
    {
        for (int i = 0; i < 3; i++)
        {
            notePages[i].SetActive(false);
        }
        notePages[a].SetActive(true);
    }
}
