﻿using Content.Shared.Backmen.Surgery.CCVar;
using Content.Shared.Backmen.Surgery.Wounds.Components;
using Content.Shared.Body.Systems;
using Robust.Shared.Configuration;
using Robust.Shared.Containers;
using Robust.Shared.Network;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;
using Robust.Shared.Timing;

namespace Content.Shared.Backmen.Surgery.Wounds.Systems;

[Virtual]
public partial class WoundSystem : EntitySystem
{
    [Dependency] private readonly IPrototypeManager _prototype = default!;
    [Dependency] private readonly IComponentFactory _factory = default!;

    [Dependency] private readonly IRobustRandom _random = default!;

    [Dependency] private readonly INetManager _net = default!;
    [Dependency] private readonly IConfigurationManager _cfg = default!;
    [Dependency] private readonly IGameTiming _timing = default!;

    [Dependency] private readonly SharedBodySystem _body = default!;

    [Dependency] private readonly SharedAppearanceSystem _appearance = default!;
    [Dependency] private readonly SharedContainerSystem _container = default!;
    [Dependency] private readonly SharedTransformSystem _transform = default!;

    //todo: make this work actually
    //[Dependency] private readonly ConsciousnessSystem _consciousness = default!;
    //[Dependency] private readonly TraumaSystem _trauma = default!;

    private ISawmill _sawmill = default!;

    public override void Initialize()
    {
        base.Initialize();

        _sawmill = Logger.GetSawmill("wounds");

        InitWounding();
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);
        _woundableJobQueue.Process();

        if (!_timing.IsFirstTimePredicted)
            return;

        var timeToHeal = 1 / _cfg.GetCVar(SurgeryCvars.MedicalHealingTickrate);
        using var query = EntityQueryEnumerator<WoundableComponent>();
        while (query.MoveNext(out var ent, out var woundable))
        {
            woundable.HealingRateAccumulated += frameTime;
            if (woundable.HealingRateAccumulated < timeToHeal)
                continue;

            woundable.HealingRateAccumulated -= timeToHeal;
            _woundableJobQueue.EnqueueJob(new IntegrityJob(this, (ent, woundable), WoundableJobTime));
        }
    }
}
