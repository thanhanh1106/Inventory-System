using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterStartModifierSO : ScriptableObject
{
    public abstract void AffectCharacter(GameObject character,float value);
}
