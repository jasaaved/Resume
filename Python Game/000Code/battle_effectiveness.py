import pokemon_info
import random
from collections import namedtuple
def attack_effectiveness(attacker: namedtuple, defender: namedtuple) -> str:
    '''
    Determines the effectiveness of an attack. Some Pokemon are dual types so this function takes account of that
    '''
    result = 0
    for att_type in attacker.type:
        for def_type in defender.type:
            if def_type == 'Bug':
                if att_type == 'Fire' or att_type == 'Flying' or att_type == 'Rock':
                    result += 1
                elif att_type == 'Fighting' or att_type == 'Ground' or att_type == 'Grass':
                    result -= 1
            elif def_type == 'Dragon':
                if att_type == 'Ice' or att_type == 'Dragon':
                    result += 1
                elif att_type == 'Electric' or att_type == 'Fire' or att_type == 'Grass' or att_type == 'Water':
                    result -= 1
            elif def_type == 'Electric':
                if att_type == 'Ground':
                    result += 1
                elif att_type == 'Flying' or att_type == 'Electric':
                    result -= 1
            elif def_type == 'Fighting':
                if att_type == 'Flying' or att_type == 'Psychic':
                    result += 1
                elif att_type == 'Rock' or att_type == 'Bug':
                    result -= 1
            elif def_type == 'Fire':
                if att_type == 'Water' or att_type == 'Rock' or att_type == 'Ground':
                    result += 1
                elif att_type == 'Fire' or att_type == 'Grass' or att_type == 'Bug' or att_type == 'Ice':
                    result -= 1
            elif def_type == 'Flying':
                if att_type == 'Rock' or att_type == 'Electric' or att_type == 'Ice':
                    result += 1
                elif att_type == 'Fighting' or att_type == 'Ground' or att_type == 'Bug' or att_type == 'Grass':
                    result -= 1
            elif def_type == 'Ghost':
                if att_type == 'Ghost':
                    result += 1
                elif att_type == 'Poison' or att_type == 'Normal' or att_type == 'Fighting' or att_type == 'Bug':
                    result -= 1
            elif def_type == 'Grass':
                if att_type == 'Bug' or att_type == 'Fire' or att_type == 'Flying' or att_type == 'Ice' or att_type == 'Poison':
                    result += 1
                elif att_type == 'Electric' or att_type == 'Grass' or att_type == 'Water' or att_type == 'Ground':
                    result -= 1
            elif def_type == 'Ground':
                if att_type == 'Grass' or att_type == 'Ice' or att_type == 'Water':
                    result += 1
                elif att_type == 'Poison' or att_type == 'Rock' or att_type == 'Electric':
                    result -= 1
            elif def_type == 'Ice':
                if att_type == 'Fire' or att_type == 'Fighting' or att_type == 'Rock':
                    result += 1
                elif att_type == 'Ice':
                    result -= 1
            elif def_type == 'Normal':
                if att_type == 'Fighting':
                    result += 1
                elif att_type == 'Ghost':
                    result -= 1
            elif def_type == 'Poison':
                if att_type == 'Psychic' or att_type == 'Ground':
                    result += 1
                elif att_type == 'Fighting' or att_type == 'Poison' or att_type == 'Grass':
                    result -= 1
            elif def_type == 'Psychic':
                if att_type == 'Ghost' or att_type == 'Bug':
                    result += 1
                elif att_type == 'Fighting' or att_type == 'Psychic':
                    result -= 1
            elif def_type == 'Rock':
                if att_type == 'Fighting' or att_type == 'Grass' or att_type == 'Water' or att_type == 'Ground':
                    result += 1
                elif att_type == 'Normal' or att_type == 'Flying' or att_type == 'Poison' or att_type == 'Fire':
                    result -= 1
            elif def_type == 'Water':
                if att_type == 'Electric' or att_type == 'Grass':
                    result += 1
                elif att_type == 'Fire' or att_type == 'Water' or att_type == 'Ice':
                    result -= 1
    if result <= -1:
        return 'Not very effective'
    elif result == 0:
        return 'Normal damage'
    elif result >= 1:
        return 'Super effective'


def return_damage_amount(multiplier: str) -> str:
    '''
    From the previous function, this function will determine the exact damage the attack will make.
    '''
    random_count = random.randrange(0, 20)
    if multiplier == 'Not very effective':
        if random_count >= 0 and random_count <= 4:
            return '50'
        elif random_count >= 5 and random_count <= 14:
            return '25'
        elif random_count >= 15 and random_count <= 19:
            return '25'
    elif multiplier == 'Normal damage':
        if random_count >= 0 and random_count <= 4:
            return '100'
        elif random_count >= 5 and random_count <= 12:
            return '50'
        elif random_count >= 13 and random_count <= 17:
            return '25'
        elif random_count >= 18 and random_count <= 19:
            return '0'
    elif multiplier == 'Super effective':
        if random_count >= 0 and random_count <= 14:
            return '100'
        elif random_count >= 15 and random_count <= 19:
            return '50'




