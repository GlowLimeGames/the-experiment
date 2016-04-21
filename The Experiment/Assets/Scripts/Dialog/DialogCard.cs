using UnityEngine;
using System.Collections;

[CreateAssetMenu()]
public class DialogCard : ScriptableObject
{
    public float textSpeed;
    [TextArea()]
    public string dialog;
    public Color backgroundColor;
    public Color textColor;

    /// <param name="textSpeed">Characters per second.</param>
    public DialogCard(float textSpeed, string dialog)
    {
        this.textSpeed = textSpeed;
        this.dialog = dialog;
        backgroundColor = Color.black;
        backgroundColor.a = 66;	//Set opacity at like 1/3
    }
}
