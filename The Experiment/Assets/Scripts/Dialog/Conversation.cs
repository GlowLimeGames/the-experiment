using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu()]
public class Conversation : ScriptableObject
{
    public bool teaganGoesFirst = false;
    public bool alignCharacters = true;
    public DialogCard[] teagan;
    public DialogCard[] tolstoy;
    
    // Returns all the cards in the order they're supposed to go in.
    public IEnumerable<DialogCard> Cards()
    {
        Queue<DialogCard> cards = new Queue<DialogCard>();

        int idx1 = 0, idx2 = 0;

        if (teaganGoesFirst && teagan.Length > 0)
        {
            yield return teagan[0];
            idx1 = 1;
        }

        while (idx2 < tolstoy.Length || idx1 < teagan.Length)
        {
            if (idx2 < tolstoy.Length)
            {
                yield return tolstoy[idx2];
                idx2++;
            }

            if (idx1 < teagan.Length)
            {
                yield return teagan[idx1];
                idx1++;
            }
        }
    }
}
