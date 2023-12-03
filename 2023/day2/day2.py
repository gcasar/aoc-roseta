import sys

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


def is_valid( draw ) -> bool:
    totals = {'red': 12, 'green': 13, 'blue': 14}
    for key, drew_count in draw.items():
        if drew_count > totals[key]:
            return False
    return True


# Heh, dont hate me future me
# Returns a generator of [{'blue': 3, 'red': 4}, {'red': 1, 'green': 2, 'blue': 6}, {'green': 2}]
input_draws = process_input(sys.stdin)
# Reduces the above to True / False based on if any draw overflowed
draws_with_valid = map(lambda l: all(map(is_valid, l)), input_draws)
# Adds round numbers and removes invalid ones
weights = map(
    lambda it: it[0] * it[1], # Removes invalid ones by multiplying by False (=0)
    enumerate(draws_with_valid, 1) # Adds draw numbers
)
print(sum(weights))