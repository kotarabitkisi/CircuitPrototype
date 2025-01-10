using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LatPuzzle : MonoBehaviour
{
    public GameObject[] notePages;
    public Complex ZL, Z0;
    public float Multiple, offset, Ycoor;
    public GameObject Left, Right;
    public Slider[] Sliders;
    public TextMeshProUGUI Zintext;
    public TMP_InputField Field;
    public valueControlPuzzle2 VCP2;
    [Header("Parametreler")]
    public float c;
    public double ZLRe, ZLIm;
    public double Z0Re, Z0Im;
    
    public float frequency, powOfFrequency;
    public float CableLength, powOfCableLength;
    public float phaseVelocityFactor;
    [Header("Cevap Parametreleri")]
    public float diffWanted;
    public double ZinAnsRe, ZinAnsIm;
    [Header("Checkmarklar")]
    public GameObject test1Check;

    public void OpenNote(int a)
    {
        for (int i = 0; i < 3; i++)
        {
            notePages[i].SetActive(false);
        }
        notePages[a].SetActive(true);
    }
    private void OnEnable()
    {
        Z0=VCP2.Z0;

        ZL = new Complex(ZLRe, ZLIm);
        for (int i = 0; i < Sliders.Length; i++) { Sliders[i].enabled = true; }
    }
    public void CableTouched(int a)
    {
        float Value = Sliders[a].value;
        for (int i = 0; i < 2; i++)
        {
            Sliders[i].value = Value;
        }
        CableLength = Value;
        Complex Zin = SolveZin(frequency * Mathf.Pow(10, powOfFrequency), phaseVelocityFactor, c, Mathf.Pow(10, powOfCableLength) * CableLength);
        Zintext.text = $"Z<voffset=-10f>in</voffset>=({Zin.Real:F2} + j{Zin.Imaginary:F2}) ohm";
        Right.transform.position = new UnityEngine.Vector3(offset + Multiple * (CableLength - 10), Ycoor, 0);
        Field.text = Sliders[a].value.ToString("F2");
    }
    public void InputTxtChanged()
    {
        string input = Field.text.Replace(',', '.');
        float Value = float.Parse(input);
        Value = Mathf.Clamp(Value, 10, 100);
        CableLength = Value;
        Sliders[0].value = Value;
        Complex Zin = SolveZin(frequency * Mathf.Pow(10, powOfFrequency), phaseVelocityFactor, c, Mathf.Pow(10, powOfCableLength) * CableLength);
        Zintext.text = $"Z<voffset=-10f>in</voffset>=({Zin.Real:F2} + j{Zin.Imaginary:F2}) ohm";
        Right.transform.position = new UnityEngine.Vector3(offset + Multiple * (CableLength - 10), Ycoor, 0);
    }
    public Complex SolveZin(float frequency, float phaseVelocityFactor, float c, float cableLength)
    {
        float phaseVelocity = phaseVelocityFactor * c;
        float wavelength = phaseVelocity / frequency;
        float betaL = 2 * Mathf.PI * cableLength / wavelength;
        betaL = Mathf.Repeat(betaL, 2 * Mathf.PI);
        Complex gamma = (ZL - Z0) / (ZL + Z0);
        Complex zin = Z0 * ((1 + gamma * Complex.Exp(-2 * betaL * Complex.ImaginaryOne)) /
                           (1 - gamma * Complex.Exp(-2 * betaL * Complex.ImaginaryOne)));
        return zin;
    }
    public void Run()
    {


        Complex Zin = SolveZin(frequency * Mathf.Pow(10, powOfFrequency), phaseVelocityFactor, c, Mathf.Pow(10, powOfCableLength) * CableLength);
        float diffRe = Mathf.Abs((float)(Zin.Real - ZinAnsRe));
        float diffIm = Mathf.Abs((float)(Zin.Imaginary - ZinAnsIm));
        print(diffRe);
        print(diffIm);
        if (diffRe <= diffWanted && diffIm <= diffWanted)
        {
            print("true");
            test1Check.SetActive(true);
        }
        else { print("false" + "\ndiffRe= " + diffRe.ToString("F2") + "\ndiffIm= " + diffIm.ToString("F2")); }

    }
}
