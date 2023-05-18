using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class BossAttackDefinition
{
    public enum AttackName {
        Wait,
        PlayerCenteredSlam,
        SimultaneousProjectile,
        SequenceProjectile,
        SimultanoeusSlamShower,
        SequenceSlamShower
    }
    public AttackName attackName;
    public float delay;
    public float amount;
}
