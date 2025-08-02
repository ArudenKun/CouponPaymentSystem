namespace Abp.Linq2Db.Common;

/// <summary>
/// Used to mark a DbContext as default for a multi db context project.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class DefaultDbContextAttribute : Attribute;
