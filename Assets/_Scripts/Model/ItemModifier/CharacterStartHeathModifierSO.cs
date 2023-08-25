using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CharacterStartHeathModifierSO : CharacterStartModifierSO
{
    public override void AffectCharacter(GameObject character, float value)
    {
        Health health = character.GetComponent<Health>();
        if (health)
        {
            health.AddHealth((int)value);
        }
    }
}
