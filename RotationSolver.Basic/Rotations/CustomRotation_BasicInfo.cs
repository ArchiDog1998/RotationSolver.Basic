﻿using ECommons.DalamudServices;
using ECommons.ExcelServices;
using Lumina.Excel.GeneratedSheets;
using XIVConfigUI;
using XIVConfigUI.Attributes;

namespace RotationSolver.Basic.Rotations;

[Command("Rotation")]
partial class CustomRotation : ICustomRotation
{
    private Job? _job = null;

    /// <inheritdoc/>
    public Job Job => _job ??= this.GetType().GetCustomAttribute<JobsAttribute>()?.Jobs[0] ?? Job.ADV;

    private JobRole? _role = null;

    /// <inheritdoc/>
    public JobRole Role  => _role ??= Svc.Data.GetExcelSheet<ClassJob>()!.GetRow((uint)Job)!.GetJobRole();
    private string? _name = null;

    /// <inheritdoc/>
    public string Name
    {
        get
        {
            if (_name != null) return _name;

            var classJob = Svc.Data.GetExcelSheet<ClassJob>()?.GetRow((uint)Job)!;

            return _name = classJob.Abbreviation + " - " + classJob.Name;
        }
    }

    /// <inheritdoc/>
    public bool IsEnabled
    {
        get => !Service.Config.DisabledJobs.Contains(Job);
        set
        {
            if (value)
            {
                Service.Config.DisabledJobs.Remove(Job);
            }
            else
            {
                Service.Config.DisabledJobs.Add(Job);
            }
        }
    }

    /// <inheritdoc/>
    public uint IconID { get; }

    private readonly SearchableCollection _configs;

    /// <inheritdoc/>
    SearchableCollection ICustomRotation.Configs => _configs;

    /// <inheritdoc/>
    public static Vector3? MoveTarget { get; internal set; }

    /// <inheritdoc/>
    public string Description => this.GetType().GetCustomAttribute<RotationAttribute>()?.Description ?? string.Empty;

    /// <inheritdoc/>
    public IAction? ActionHealAreaGCD { get; private set; }

    /// <inheritdoc/>
    public IAction? ActionHealAreaAbility { get; private set; }

    /// <inheritdoc/>
    public IAction? ActionHealSingleGCD { get; private set; }

    /// <inheritdoc/>
    public IAction? ActionHealSingleAbility { get; private set; }

    /// <inheritdoc/>
    public IAction? ActionDefenseAreaGCD { get; private set; }

    /// <inheritdoc/>
    public IAction? ActionDefenseAreaAbility { get; private set; }

    /// <inheritdoc/>
    public IAction? ActionDefenseSingleGCD { get; private set; }

    /// <inheritdoc/>
    public IAction? ActionDefenseSingleAbility { get; private set; }

    /// <inheritdoc/>
    public IAction? ActionMoveForwardGCD { get; private set; }

    /// <inheritdoc/>
    public IAction? ActionMoveForwardAbility { get; private set; }

    /// <inheritdoc/>
    public IAction? ActionMoveBackAbility { get; private set; }

    /// <inheritdoc/>
    public IAction? ActionSpeedAbility { get; private set; }

    /// <inheritdoc/>
    public IAction? ActionDispelStancePositionalGCD { get; private set; }

    /// <inheritdoc/>
    public IAction? ActionDispelStancePositionalAbility { get; private set; }

    /// <inheritdoc/>
    public IAction? ActionRaiseShirkGCD { get; private set; }

    /// <inheritdoc/>
    public IAction? ActionRaiseShirkAbility { get; private set; }

    /// <inheritdoc/>
    public IAction? ActionAntiKnockbackAbility { get; private set; }

    /// <inheritdoc/>
    public IAction? ActionLimitBreak { get; private set; }

    /// <summary>
    /// Is this action valid.
    /// </summary>
    [Description("Is this rotation valid")]
    public bool IsValid { get; private set; } = true;

    /// <summary>
    /// Why this action is not valid.
    /// </summary>
    public string WhyNotValid { get; private set; } = string.Empty;

    /// <summary>
    /// Should show the status to the users.
    /// </summary>
    [Description("Show the status")]
    public virtual bool ShowStatus => false;

    private protected CustomRotation()
    {
        DataCenter.Job = Job;
        if (DataCenter.IsPvP)
        {
            Service.Config.PvPRotationChoice = GetType().FullName;
        }
        else
        {
            Service.Config.RotationChoice = GetType().FullName;
        }

        IconID = IconSet.GetJobIcon(Job);

        _configs = new SearchableCollection(this, new RotationSearchableConfig());

#if DEBUG
        Svc.Log.Info("Loading " + GetType().FullName);
#endif
        //Load from config.
        var savedConfigs = Service.Config.RotationConfigurations;

        foreach (var item in _configs)
        {
            item._default = item._property.GetValue(this)!;
            if (savedConfigs.TryGetValue(item._property.Name, out var value))
            {
                item.OnCommand(value);
            }
        }

        OnTerritoryChanged();
        Init();
    }

    /// <inheritdoc/>
    public override string ToString() => this.GetType().GetCustomAttribute<RotationAttribute>()?.Name ?? this.GetType().Name;

    /// <summary>
    /// Update your customized field.
    /// </summary>
    protected virtual void UpdateInfo() { }

    /// <summary>
    /// Some extra display things.
    /// </summary>
    public virtual void DisplayStatus()
    {
        ImGui.TextWrapped($"If you want to Display some extra information on this panel. Please override {nameof(DisplayStatus)} method!");
    }

    /// <summary>
    /// The things on territory changed.
    /// </summary>
    public virtual void OnTerritoryChanged() { }

    internal virtual void Init() { }

    /// <inheritdoc/>
    public void Dispose()
    {
        DataCenter.Job = Job.ADV;
        _configs.Dispose();
        GC.SuppressFinalize(this);
    }
}
