import random
import csv
from sys import argv


class GraphMatrixModel:
    def __init__(self, number_of_vertices, max_weight, clusters):
        self.number_of_vertices = number_of_vertices
        self.adjacency_matrix = [[0] * number_of_vertices for _ in range(number_of_vertices)]
        self.generate_clustered_sparse_graph(max_weight, clusters)

    def generate_clustered_sparse_graph(self, max_weight, clusters):
        cluster_size = self.number_of_vertices // clusters

        # Connect vertices WITHIN each cluster (dense areas)
        for c in range(clusters):
            start = c * cluster_size
            end = self.number_of_vertices if c == clusters - 1 else start + cluster_size

            for i in range(start, end):
                for j in range(i + 1, end):
                    if random.random() < 0.7:  # 70% chance of connection within clusters
                        weight = random.randint(1, max_weight)
                        self.adjacency_matrix[i][j] = weight
                        self.adjacency_matrix[j][i] = weight

        # Connect clusters together (sparse interconnections)
        for c in range(clusters - 1):
            start_a = c * cluster_size
            end_a = start_a + cluster_size

            start_b = (c + 1) * cluster_size
            end_b = self.number_of_vertices if c == clusters - 2 else start_b + cluster_size

            for i in range(start_a, end_a):
                for j in range(start_b, end_b):
                    if random.random() < 0.3:  # 30% chance of connection between clusters
                        weight = random.randint(1, max_weight)
                        self.adjacency_matrix[i][j] = weight
                        self.adjacency_matrix[j][i] = weight

        # Ensure graph is connected
        self.ensure_graph_connectivity(max_weight)

    def ensure_graph_connectivity(self, max_weight):
        visited = set()
        stack = [0]

        # Perform DFS to check connectivity
        while stack:
            current = stack.pop()
            if current not in visited:
                visited.add(current)
                for neighbor, weight in enumerate(self.adjacency_matrix[current]):
                    if weight > 0 and neighbor not in visited:
                        stack.append(neighbor)

        # Add minimum edges if the graph is not fully connected
        while len(visited) < self.number_of_vertices:
            isolated_vertex = next(i for i in range(self.number_of_vertices) if i not in visited)
            connected_vertex = random.choice(list(visited))

            weight = random.randint(1, max_weight)
            self.adjacency_matrix[isolated_vertex][connected_vertex] = weight
            self.adjacency_matrix[connected_vertex][isolated_vertex] = weight
            visited.add(isolated_vertex)

    def print_adjacency_matrix(self, lang="plaintext"):
        """
        Print the adjacency matrix in the specified format.
    
        :param lang: Format of the output. Can be 'plaintext' or 'csharp'.
                     - 'plaintext': Prints the matrix as plain text.
                     - 'csharp': Prints the matrix as a C# static initialization.
        """
        if lang == "plaintext":
            print("Adjacency Matrix:")
            for row in self.adjacency_matrix:
                print(" ".join(map(str, row)))
    
        elif lang == "csharp":
            print("Adjacency Matrix in C# format:")
            csharp_code = "new decimal[,] {\n"
            for row in self.adjacency_matrix:
                csharp_code += "    { " + ", ".join(map(str, row)) + " },\n"
            csharp_code += "};"
            print(csharp_code)
    
        else:
            raise ValueError("Unsupported language option. Please choose 'plaintext' or 'csharp'.")


    def export_to_csv(self, filename):
        with open(filename, mode='w', newline='') as file:
            writer = csv.writer(file)
            writer.writerow(["Source", "Target", "Weight"])  # Headers for Gephi
            for i in range(self.number_of_vertices):
                for j in range(i + 1, self.number_of_vertices):  # Export only upper triangle
                    if self.adjacency_matrix[i][j] > 0:
                        writer.writerow([i, j, self.adjacency_matrix[i][j]])


# Example Usage
if __name__ == "__main__":
    if len(argv) < 3:
        print("Usage: python gen_sparse_g.py <number_of_vertices> <number_of_clusters>")
        exit(1)
        
    # pick from args
    number_of_vertices = int(argv[1])    # Total number of vertices
    clusters = int(argv[2])              # Number of clusters with different density

    max_weight = 10  # Maximum weight for edges

    print ("\nGenerating a sparse graph with {} vertices and {} clusters...".format(number_of_vertices, clusters))

    graph = GraphMatrixModel(number_of_vertices, max_weight, clusters)

    # Output adjacency matrix
    graph.print_adjacency_matrix('csharp')

    # Export to CSV for Gephi
    output_csv_file = "sparse_graph.csv"
    graph.export_to_csv(output_csv_file)
    print(f"\nGraph exported to '{output_csv_file}' for Gephi visualization.")
