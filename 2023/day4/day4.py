import sys
from typing import Tuple, Set


def parse_input( line: str ) -> Tuple[Set[int], Set[int]]:
    left, right = line.split(':')[1].split('|')
    left_nums = {int(part) for part in left.split(' ') if part.strip()}
    right_nums = {int(part) for part in right.split(' ') if part.strip()}
    return left_nums, right_nums


input = map(parse_input, sys.stdin)
winning_nums = filter(lambda it: it > 0, map(lambda it: len(it[0] & it[1]), input))
result = sum(map(lambda nums: 2**(nums-1), winning_nums))
print(result)
