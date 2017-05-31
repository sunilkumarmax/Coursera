"""Start of the program"""

import sys

class ComparisionCalculator():
    """Calculates the comparisions in quick sort"""
    def __init__(self, integer_array):
        self.integer_array = integer_array
        self.size = len(integer_array)

    def swap(self, first, second):
        """Swaps the positions in an array"""
        temp = self.integer_array[first]
        self.integer_array[first] = self.integer_array[second]
        self.integer_array[second] = temp

    def partition_helper(self, start, end, func):
        separator = start + 1
        for iterator in range(start + 1, end + 1):
            if self.integer_array[iterator] < self.integer_array[start]:
                self.swap(iterator, separator)
                separator += 1
        self.swap(start, separator - 1)
        left_comparisions = func(start, separator - 2)
        right_comparisions = func(separator, end)
        return left_comparisions + right_comparisions + end - start

    def count_comparisions_pivot_first(self, start, end):
        """Counts the inversions in an array"""
        size = (end - start + 1)
        if size <= 1:
            return 0
        return self.partition_helper(start, end, self.count_comparisions_pivot_first)

    def count_comparisions_pivot_last(self, start, end):
        """Counts the inversions in an array"""
        size = (end - start + 1)
        if size <= 1:
            return 0
        self.swap(start, end)
        return self.partition_helper(start, end, self.count_comparisions_pivot_last)

    def count_comparisions_pivot_median(self, start, end):
        """Counts the inversions in an array"""
        size = (end - start + 1)
        if size <= 1:
            return 0
        mid = start + int((end - start) / 2)
        mini_list = []
        mini_list.append((start, self.integer_array[start]))
        mini_list.append((mid, self.integer_array[mid]))
        mini_list.append((end, self.integer_array[end]))
        mini_list.sort(key=lambda tup: tup[1])
        median = mini_list[1][0]
        self.swap(start, median)
        return self.partition_helper(start, end, self.count_comparisions_pivot_median)

def construct_array():
    input_array = []
    filename = "c:/Users/v-sunvan/Documents/GitHub/Coursera/Algorithms/Coursera.Algorithms.Python/Course1Week3/QuickSort.txt"
    with open(filename, "r+") as textfile:
        for line_of_text in textfile:
            input_array.append(int(line_of_text))
    if not textfile.closed:
        textfile.close()
    return input_array

def main(pivot):
    """Main function"""
    instance = ComparisionCalculator(construct_array())
    if pivot == "first":
        print(instance.count_comparisions_pivot_first(0, instance.size - 1))
    elif pivot == "last":
        print(instance.count_comparisions_pivot_last(0, instance.size - 1))
    elif pivot == "median":
        print(instance.count_comparisions_pivot_median(0, instance.size - 1))

if __name__ == "__main__":
    main("first")
    main("last")
    main("median")
    sys.exit()
