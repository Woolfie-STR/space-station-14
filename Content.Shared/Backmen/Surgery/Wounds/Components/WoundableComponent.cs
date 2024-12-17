﻿using Content.Shared.FixedPoint;
using Robust.Shared.Containers;
using Robust.Shared.GameStates;

namespace Content.Shared.Backmen.Surgery.Wounds.Components;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class WoundableComponent : Component
{
    /// <summary>
    /// UID of the parent woundable entity. Can be null.
    /// </summary>
    [ViewVariables, AutoNetworkedField]
    public EntityUid? ParentWoundable;

    /// <summary>
    /// UID of the root woundable entity.
    /// </summary>
    [ViewVariables, AutoNetworkedField]
    public EntityUid RootWoundable;

    /// <summary>
    /// Set of UIDs representing child woundable entities.
    /// </summary>
    [ViewVariables, AutoNetworkedField]
    public HashSet<EntityUid> ChildWoundables = [];

    /// <summary>
    /// Indicates whether wounds are allowed.
    /// </summary>
    [DataField]
    [ViewVariables, AutoNetworkedField]
    public bool AllowWounds = true;

    /// <summary>
    /// Integrity points of this woundable.
    /// </summary>
    [DataField]
    [ViewVariables, AutoNetworkedField]
    public FixedPoint2 WoundableIntegrity;

    /// <summary>
    /// How much damage will be healed ACROSS all limb, for example if there are 2 wounds,
    /// Healing will be shared across those 2 wounds.
    /// </summary>
    [DataField]
    [ViewVariables, AutoNetworkedField]
    public FixedPoint2 HealAbility = 0.1;

    /// <summary>
    /// Multipliers of severity applied to this wound.
    /// </summary>
    public Dictionary<EntityUid, WoundableSeverityMultiplier> SeverityMultipliers = new();

    /// <summary>
    /// Multipliers applied to healing rate.
    /// </summary>
    public Dictionary<EntityUid, WoundableHealingMultiplier> HealingMultipliers = new();

    /// <summary>
    /// State of the woundable. Severity basically.
    /// </summary>
    [DataField]
    [ViewVariables, AutoNetworkedField]
    public WoundableSeverity WoundableSeverity;

    /// <summary>
    /// How much time in seconds had this woundable accumulated from the last healing tick.
    /// </summary>
    [ViewVariables, AutoNetworkedField]
    public float HealingRateAccumulated;

    /// <summary>
    /// DO NOT use OR I will break your knees.
    /// </summary>
    [ViewVariables, AutoNetworkedField]
    public bool ForceLoss = false;

    /// <summary>
    /// Container potentially holding wounds.
    /// </summary>
    [ViewVariables]
    public Container? Wounds;

    /// <summary>
    /// Container holding this woundables bone.
    /// </summary>
    [ViewVariables]
    public Container? Bone;
}