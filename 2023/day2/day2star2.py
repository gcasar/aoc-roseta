import sys
from functools import reduce

def process_input( lines ):
    for line in lines:
        # Split an example line "Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green"
        # into " 3 blue, 4 red", " 4 red", " 2 green, 6 blue", "2 green"
        sets = line.split(':')[1].split(';')
        # splits each set into dict with counts
        def extract_set( set_string ) -> dict:
            return {
                pair[1]: int(pair[0]) for pair
                in map(lambda it: it.strip().split(' '), set_string.split(','))
            }

        yield list(map(extract_set, sets))


def merge( draw1, draw2 ):
    return {
        'red': max(draw1.get('red', 0), draw2.get('red', 0)),
        'blue': max(draw1.get('blue', 0), draw2.get('blue', 0)),
        'green': max(draw1.get('green', 0), draw2.get('green', 0)),
    }


def power( draw ):
    return draw.get('red', 1) * draw.get('blue', 1) * draw.get('green', 1)


# Heh, dont hate me future me
# Returns a generator of [{'blue': 3, 'red': 4}, {'red': 1, 'green': 2, 'blue': 6}, {'green': 2}]
input_draws = process_input(sys.stdin)
sums = map(lambda l: reduce(merge, l), input_draws)
powers = map(power, sums)
print(sum(powers))