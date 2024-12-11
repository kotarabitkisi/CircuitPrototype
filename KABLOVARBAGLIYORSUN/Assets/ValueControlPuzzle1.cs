using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ValueControlPuzzle1 : MonoBehaviour
{
    public float R, G;
    public float L, C;
    public float powOfL, powOfC, powOfG, powOfR, powOfFrequency;
    public TMP_InputField InputFieldR, InputFieldG, InputFieldC,InputFieldL;
    public Slider LSlider, CSlider;
    public float Beta, Frequency;
    public Complex Z0;
    public float Z0Re, Z0Im;
    public GameObject glowingSquare;
    public float diffWanted;
    private void Start()
    {
        Z0 = new Complex(Z0Re,Z0Im);
    }
    public void SliderValueChanged()
    {
        L = LSlider.value;
        C = CSlider.value;
        InputFieldL.text = L.ToString();
        InputFieldC.text = C.ToString();
    }
    public void InputValueChanged()
    {
        L = float.Parse(InputFieldL.text);
        C = float.Parse(InputFieldC.text);
        LSlider.value = L;
        CSlider.value = C;
    }

    public void ControlItsTrueOrNot()
    {
        double CurC = SolveC(Z0,Mathf.Pow(R,powOfR), Mathf.Pow(L, powOfL), Frequency);
        double CurL = SolveL(Z0, Mathf.Pow(G, powOfG), Mathf.Pow(C, powOfC), Frequency);
        double diffC = Mathf.Abs((float)CurC - C * Mathf.Pow(10, powOfC));
        double diffL = Mathf.Abs((float)CurL - L * Mathf.Pow(10, powOfL));
        bool AnsC = diffC <= diffWanted;
        bool AnsL = diffL <= diffWanted;
        if (AnsC && AnsL) { print("true"); GetComponent<gameManager>().p1solved = true; GetComponent<gameManager>().CloseBtnPressed(); }
        else if (AnsC) { print("L is False \n Ldiff=" + diffL + "\nCdiff = " + diffC); }
        else if (AnsL) { print("C is False \n Cdiff=" + diffC+ "\nLdiff = " + diffL); }
        else { print("Both of them are False \n Cdiff=" + diffC + "\nLdiff = " + diffL); }
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
