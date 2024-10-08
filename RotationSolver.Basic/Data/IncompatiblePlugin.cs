﻿namespace RotationSolver.Basic.Data;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
public struct IncompatiblePlugin
{
    public string Name { get; set; }
    public string Icon { get; set; }
    public string Url { get; set; }
    public string Features { get; set; }
    public CompatibleType Type { get; set; }
}

[Flags]
public enum CompatibleType : byte
{
    Skill_Usage = 1 << 0,
    Skill_Selection = 1 << 1,
    Crash = 1 << 2,
}