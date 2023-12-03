import sys
import re

schematic = []
symbol_by_loc = dict()
number_by_loc = dict()

for y, line in enumerate(sys.stdin):
    line = line.strip()
    schematic.append(line)

    for x, char in enumerate(line):
        if (char not in ".0123456789"):
            symbol_by_loc[(x,y)] = char
    
    numbers = re.finditer(r"(\d+)", line)
    for number_match in numbers:
        number_by_loc[(number_match.start(), y)] = int(number_match.group())

print(set(symbol_by_loc.values()))

def has_adjecent( pos, number ):
    x, y = pos
    length = len(str(number))

    positions_to_check = []
    for xi in range(x-1, x+length+1):
        positions_to_check.append((xi, y-1))
        positions_to_check.append((xi, y  ))
        positions_to_check.append((xi, y+1))

    for position in positions_to_check:
        if position in symbol_by_loc:
            return True
    return False

numbers_adjecent_to_symbols = filter(lambda item: has_adjecent(item[0], item[1]), number_by_loc.items());
print(sum(map(lambda it: it[1], numbers_adjecent_to_symbols)))