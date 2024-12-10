using UnityEngine;

public class colorChanging : MonoBehaviour
{
    public Color color, color2;
    public SpriteRenderer sprite;
    public float colorChangeSpeed;
    private void Update()//rengin iki renk arasýnda bir süre içinde deðiþme scripti fazla önemli deðil
    {
        sprite.color = Color.Lerp(color,color2,(Mathf.Sin(Time.time*colorChangeSpeed)+1)/2);
    }
}
