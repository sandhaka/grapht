namespace GraphT.Graph.Exceptions;

public sealed class NoHeuristicDefinedException() : Exception("Heuristic is mandatory for this search strategy")
{
    
}