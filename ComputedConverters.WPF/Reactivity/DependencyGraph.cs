using System.Collections.Generic;

namespace ComputedConverters;

internal class DependencyGraph(IReadOnlyCollection<DependencyNode> dependencyNodes, IReadOnlyCollection<ConditionalNode> conditionalNodes)
{
    public IReadOnlyCollection<DependencyNode> DependencyRootNodes { get; } = dependencyNodes;

    public IReadOnlyCollection<ConditionalNode> ConditionalRootNodes { get; } = conditionalNodes;
}
