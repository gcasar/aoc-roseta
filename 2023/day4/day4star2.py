import sys
from typing import Tuple, Set
from collections import Counter


def parse_input( line: str ) -> Tuple[Set[int], Set[int]]:
    left, right = line.split(':')[1].split('|')
    left_nums = {int(part) for part in left.split(' ') if part.strip()}
    right_nums = {int(part) for part in right.split(' ') if part.strip()}
    return left_nums, right_nums


input = map(parse_input, sys.stdin)
winning_nums = list(map(lambda it: it[0] & it[1], input))

card_counter: Counter = Counter()
# prefill by adding every original card
card_counter.update(range(len(winning_nums)))

# and now count subsequent cards
for idx, nums in enumerate(winning_nums):
    items = list(range(idx+1, idx+len(nums)+1))
    current_card_count = card_counter[idx]
    card_counter.update(items * current_card_count)  # not optimal but hey
 
print(sum(card_counter.values()))
