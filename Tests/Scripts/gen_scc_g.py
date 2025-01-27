import random
import csv

def generate_adj_matrix(num_nodes, scc_sizes):
    """
    Generates an adjacency matrix for a graph containing strong connected components (SCCs).

    Args:
        num_nodes (int): Total number of nodes in the graph.
        scc_sizes (list of int): A list where each value represents the number of nodes in an SCC.

    Returns:
        list of list: The adjacency matrix of the graph.
    """
    # Initialize adjacency matrix with all zeros
    adj_matrix = [[0 for _ in range(num_nodes)] for _ in range(num_nodes)]

    # Helper function to add edges within an SCC
    def add_edges_within_scc(start, size):
        for i in range(start, start + size):
            for j in range(start, start + size):
                if i != j:  # avoid self-loops
                    adj_matrix[i][j] = 1

    # Create SCCs
    current_node = 0
    for size in scc_sizes:
        add_edges_within_scc(current_node, size)
        current_node += size

    # Randomly connect SCCs (at least one way)
    num_scc = len(scc_sizes)
    if num_scc > 1:
        start_nodes = [sum(scc_sizes[:i]) for i in range(num_scc)]
        for i in range(num_scc - 1):
            source_scc = start_nodes[i]
            target_scc = start_nodes[i + 1]
            adj_matrix[source_scc][target_scc] = 1  # Connect one node from SCC[i] → SCC[i+1]

    # Optionally: Add random edges between SCCs to make it interesting
    for _ in range(num_nodes // 2):  # Add ~num_nodes / 2 random edges
        u, v = random.sample(range(num_nodes), 2)  # Pick two different nodes
        adj_matrix[u][v] = 1

    return adj_matrix


def print_adj_matrix(adj_matrix):
    """Pretty print the adjacency matrix."""
    for row in adj_matrix:
        print(" ".join(map(str, row)))


def export_to_gephi(adj_matrix):
    """
    Exports the graph represented by the adjacency matrix into Gephi-compatible CSV files.

    Args:
        adj_matrix (list of list): Adjacency matrix of the graph.
    """
    num_nodes = len(adj_matrix)

    # Export Nodes (nodes.csv)
    with open('nodes.csv', mode='w', newline='', encoding='utf-8') as nodes_file:
        csv_writer = csv.writer(nodes_file)
        csv_writer.writerow(["Id", "Label"])  # Gephi requires 'Id' and 'Label' columns

        for node_id in range(num_nodes):
            csv_writer.writerow([node_id, f"Node {node_id}"])

    # Export Edges (edges.csv)
    with open('edges.csv', mode='w', newline='', encoding='utf-8') as edges_file:
        csv_writer = csv.writer(edges_file)
        csv_writer.writerow(["Source", "Target", "Type", "Weight"])  # Gephi requires these columns

        for source in range(num_nodes):
            for target in range(num_nodes):
                if adj_matrix[source][target] == 1:  # Edge exists
                    csv_writer.writerow([source, target, "Directed", 1])  # Default weight is 1


# Parameters
total_nodes = 10
scc_node_sizes = [3, 4, 3]  # Define SCC sizes (e.g., 3 nodes in SCC1, 4 in SCC2, 3 in SCC3)

# Generate the adjacency matrix
adj_matrix = generate_adj_matrix(total_nodes, scc_node_sizes)

# Output the adjacency matrix
print("Adjacency Matrix:")
print_adj_matrix(adj_matrix)

# Export data for Gephi
export_to_gephi(adj_matrix)
print("\nCSV files for Gephi have been generated: 'nodes.csv' and 'edges.csv'")
