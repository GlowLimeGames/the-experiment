using UnityEngine;
using System.Collections;

[CreateAssetMenu()]
public class OwnedDialogCard : DialogCard
{
    public bool isTeagan;

    public OwnedDialogCard(float textSpeed, string dialog) : base(textSpeed, dialog) { }
}
