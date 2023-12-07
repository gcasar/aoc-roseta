import sys
import re

number_strings = ['one', 'two', 'three', 'four', 'five', 'six', 'seven', 'eight', 'nine']
replacements_dict = { 
    **{ str(i): i for i in range(1, 10)}, 
    ** { number_strings[i]: i+1 for i in range(9) }
}
okmatch = re.compile(fr"(\d|one|two|three|four|five|six|seven|eight|nine)")

def get_digits( line ):
    """Removes invalid chars one by one, left to right"""
    match = okmatch.search(line)
    while match:
        yield replacements_dict[match.group(1)]
        line = line[match.start()+1:]
        match = okmatch.search(line)


def process_input( stream ):
    for line in stream:
        digits = list(get_digits(line))
        yield int(digits[0] * 10 + digits[-1])


print(sum(process_input(sys.stdin)))
