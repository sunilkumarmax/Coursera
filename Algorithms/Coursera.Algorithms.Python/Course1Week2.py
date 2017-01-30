"""Start of the program"""

class Inversion():
    """Inversion holder class"""
    def __init__(self, array, inversion_count):
        self.data = array
        self.inversion_count = inversion_count

def merge_array_inversions(left_array, right_array):
    """Merges the two arrays and counts the inversions"""

    left_length = len(left_array)
    right_length = len(right_array)
    left_index = 0
    right_index = 0
    integer_array = []
    inversions_count = 0
    try:
        while True:
            if right_index == right_length:
                for index in range(left_length - left_index):
                    integer_array.append(left_array[left_index + index])
                return Inversion(integer_array, inversions_count)
            if left_index == left_length:
                for index in range(right_length - right_index):
                    integer_array.append(right_array[right_index + index])
                return Inversion(integer_array, inversions_count)
            if left_array[left_index] <= right_array[right_index]:
                integer_array.append(left_array[left_index])
                left_index += 1
            else:
                integer_array.append(right_array[right_index])
                right_index += 1
                inversions_count += left_length - left_index
    except Exception:
        print("left_index: " + str(left_index)
              + "; left_length: " + str(left_length)
              + "; right_index: " + str(right_index)
              + "; right_length: " + str(right_length))
        print(left_array)
        print(right_array)

def count_inversions(integer_array):
    """Counts the inversions in an array"""
    array_length = len(integer_array)
    if array_length == 1:
        return Inversion(integer_array, 0)
    half_length = int(array_length / 2)
    if array_length % 2 != 0:
        half_length += 1 #Ensures the left array to be bigger size always incase of odd lengths
    left_inversion = count_inversions(integer_array[:half_length])
    right_inversion = count_inversions(integer_array[half_length:])
    merge_inversion = merge_array_inversions(left_inversion.data, right_inversion.data)
    return Inversion(merge_inversion.data
                     , left_inversion.inversion_count
                     + right_inversion.inversion_count
                     + merge_inversion.inversion_count)

input_array = []
filename = "c:/Users/v-sunvan/Documents/GitHub/Coursera/Algorithms/Coursera.Algorithms.Python/IntegerArray.txt"
with open(filename, "r+") as textfile:
    for line_of_text in textfile:
        input_array.append(int(line_of_text))
if not textfile.closed:
    textfile.close()
print(count_inversions(input_array).inversion_count)
