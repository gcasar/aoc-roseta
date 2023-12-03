import sys
import re

# Matches either strings with first and last digit
twonums = re.compile(r'\D*(\d).*(\d)\D*')
# ... with only a single digit in string
onenum = re.compile(r'\D*(\d)\D*')
# Sets up a regex to only replace a single digit or the first and last one
number_strings = ['one', 'two', 'three', 'four', 'five', 'six', 'seven', 'eight', 'nine']
numbers_group = '|'.join(number_strings)
twostring = re.compile(fr"({numbers_group}).*({numbers_group})")
onestring = re.compile(fr"({numbers_group})")

def replace_span( input_string, span ):
    start, end = span
    string_to_replace = input_string[start:end]
    replacement = str(number_strings.index(string_to_replace)+1)
    return input_string[:start] + replacement + input_string[end:]

def preprocess_line( line ):
    """Replaces the first and numeric string with its digit representations"""
    # Replace the last number if it exists
    match = twostring.search(line)
    if match:
        line = replace_span(line, match.span(2))

    # Now the first one
    match = onestring.search(line)
    if match:
        line = replace_span(line, match.span(1))
    
    return line


buffer = list(map(lambda x: x, sys.stdin))
print(*list(map(preprocess_line, buffer)), sep='')

def process_input( stream ):
    for line in stream:
        line = preprocess_line(line)
        if twonums.match(line):
            yield int(twonums.sub(r'\1\2', line))
        elif onenum.match(line):
            yield int(onenum.sub(r'\1\1', line))


print(sum(process_input(buffer)))
