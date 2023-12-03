import sys
import re

schematic = []
gear_by_loc = dict()
number_by_loc = dict()

for y, line in enumerate(sys.stdin):
    line = line.strip()
    schematic.append(line)

    for x, char in enumerate(line):
        if char == "*":
            gear_by_loc[(x,y)] = char
    
    numbers = re.finditer(r"(\d+)", line)
    for number_match in numbers:
        for i in range(len(number_match.group())):
            # we store both starting point AND value because we might hit the same number in mulptile places
            # and we need to determine unique numbers
            number_by_loc[(number_match.start()+i, y)] = (number_match.start(), int(number_match.group()))


def get_all_adjecent_nums( x, y ):
    positions_to_check = []
    for xi in range(x-1, x+2):
        positions_to_check.append((xi, y-1))
        positions_to_check.append((xi, y  ))
        positions_to_check.append((xi, y+1))

    for position in positions_to_check:
        if position in number_by_loc:
            yield number_by_loc[position]


result = 0
for x, y in gear_by_loc.keys():
    # need to dedup numbers we hit multiple times
    adjacent_nums = list(set(get_all_adjecent_nums(x, y)))
    if len(adjacent_nums) == 2:
        result += adjacent_nums[0][1] * adjacent_nums[1][1]

print(result)