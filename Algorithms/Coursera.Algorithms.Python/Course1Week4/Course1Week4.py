"""Start of the program"""
from random import randint
import re
import sys
import copy
import math
import time

class GraphEdge(object):
    """Edge class for a graph"""
    def __init__(self, A, B):
        self.source_vertex = A
        self.destination_vertex = B

class Graph(object):
    """Graph class"""
    def __init__(self):
        self.vertices = []
        self.edges = []
        self.vertex_edge = {}

    def add_vertex(self, vertex):
        """Adds a vertex to the graph"""
        self.vertices.append(vertex)
        self.vertex_edge[vertex] = []

    def add_edge(self, source_vertex, destination_vertex):
        """Adds an edge to the graph"""
        self.vertex_edge[source_vertex].append(destination_vertex)
        new_edge = GraphEdge(source_vertex, destination_vertex)
        if destination_vertex not in self.vertex_edge:
            self.edges.append(new_edge)

    def add_vertex_edge(self, vertices):
        """Constructs the vertex and the edges from the list of vertices where the first /
        vertex is the source and all remaining vertices are the destinations in the graph edges"""
        if len(vertices) < 2:
            raise Exception('Cannot have a single vertex')
        self.add_vertex(vertices[0])
        length_array = len(vertices)
        for iterator in range(1, length_array):
            num = vertices[iterator]
            is_number = False
            try:
                int(num)
                is_number = True
            except ValueError:
                pass
            if is_number:
                self.add_edge(vertices[0], num)

    def contract_random_edge(self):
        """contracts a random edge"""
        edge_length = len(self.edges)
        edge_index = randint(0, edge_length-1)
        removed_edge = self.edges[edge_index]
        self.vertices.remove(removed_edge.destination_vertex)
        to_be_removed_edges = []
        for index, item in enumerate(self.edges):
            if (item.source_vertex == removed_edge.source_vertex and item.destination_vertex == removed_edge.destination_vertex) or (item.source_vertex == removed_edge.destination_vertex and item.destination_vertex == removed_edge.source_vertex):
                to_be_removed_edges.append(index)
            if item.source_vertex == removed_edge.destination_vertex:
                self.edges[index] = GraphEdge(removed_edge.source_vertex, item.destination_vertex)
            elif item.destination_vertex == removed_edge.destination_vertex:
                self.edges[index] = GraphEdge(item.source_vertex, removed_edge.source_vertex)
        for iterator in range(0, len(to_be_removed_edges)):
            self.edges.pop(to_be_removed_edges[iterator] - iterator)
        source_adjacent_vertices = self.vertex_edge[removed_edge.source_vertex]
        destination_adjacent_vertices = self.vertex_edge[removed_edge.destination_vertex]
        for item in destination_adjacent_vertices:
            if item != removed_edge.source_vertex:
                source_adjacent_vertices.append(item)
                self.vertex_edge[item].append(removed_edge.source_vertex)
            self.vertex_edge[item].remove(removed_edge.destination_vertex)
        del self.vertex_edge[removed_edge.destination_vertex]

def construct_graph():
    """Constructs the graph"""
    graph_object = Graph()
    filename = "c:/Users/v-sunvan/Documents/GitHub/Coursera/Algorithms/Coursera.Algorithms.Python/Course1Week4/KargerMinCut.txt"
    with open(filename, "r+") as textfile:
        for line_of_text in textfile:
            vertex_list = re.split('\t', line_of_text)
            graph_object.add_vertex_edge(vertex_list)
    if not textfile.closed:
        textfile.close()
    return graph_object

def main():
    """Main function"""
    min_cut_edges = sys.maxsize
    graph_object = construct_graph()
    vertices_count = len(graph_object.vertices)
    total_iterations = math.ceil(vertices_count * vertices_count * math.log(vertices_count))
    iteration_count = 0
    while iteration_count < total_iterations:
        #start_time = time.time()
        graph_object_copy = copy.deepcopy(graph_object)
        #copy_time = time.time()
        while len(graph_object_copy.vertices) > 2:
            graph_object_copy.contract_random_edge()
        #end_time = time.time()
        cut_edges = len(graph_object_copy.edges)
        if min_cut_edges > cut_edges:
            min_cut_edges = cut_edges
        iteration_count += 1
        end_time = time.time()
        # print("Copy time: {}".format((copy_time - start_time) * 100 /(end_time - start_time)))
        # print("Loop time: {}".format((end_time - copy_time) * 100 /(end_time - start_time)))
        # print("--------------------------------------------------")
        if iteration_count % 100 == 0:
            print("Iteration Count: {}; Min cut edges: {}".format(iteration_count, min_cut_edges))
    print("Final min cut edge length = " + str(min_cut_edges))

if __name__ == "__main__":
    main()
    sys.exit()
