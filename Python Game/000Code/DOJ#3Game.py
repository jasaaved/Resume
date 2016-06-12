#Jonathan Saavedra 44714885
#Oscar Rosas 91857046
#David Jungman  22962656
from SpritesAndSounds import *
from battle_effectiveness import *
from InternetConnection import *
from InternetConnection import _get_data
import ast
import datetime
import fileinput
import random



blackcolor =(0,0,0)
whitecolor = (255, 255, 255)
bluecolor = (0, 0, 255)

fps = pygame.time.Clock()


font = pygame.font.Font("Pokemon DPPt.ttf", 30)
menu_font = pygame.font.Font("Pokemon DPPt.ttf", 23)

screen = pygame.display.set_mode((640,480))
pygame.display.set_caption('Monsters That Fit In Your Pocket')

def full_game():
    '''
    The main function the user is interacting with. This function will call other
    functions to run the game.
    '''

    while True:
        try:
            while True:
                data, UserID = _intro()
                if data != None:
                    break
            battles, pokemon, shinypokemon, team_pkmn, coins, games_completed, time_stamp = _save_data(UserID)
            current_time = datetime.datetime.now()
            if (current_time-time_stamp).total_seconds() >= 86400:
                gain = _update_coins(data, coins)
                coins += gain
                _line_replacer(4, coins, UserID)
                _line_replacer(6, current_time, UserID)
            _main_menu(battles, pokemon, shinypokemon, team_pkmn, coins, games_completed, 200, UserID)
            
        except:
            return



def _intro():
    '''
    This function asks the user for his/her student ID. It will then call
    another function to search for the account online.
    '''
    
    label1 = font.render('Enter School ID. Then Press Enter:', 1, whitecolor)
    UserID = ''
    
    while True:
        label2 = font.render(str(UserID), 2, whitecolor)
                
            
        events = pygame.event.get()
        for event in events: 
            if event.type == QUIT:
                pygame.quit()
                return '', ''
            if event.type == KEYDOWN:
                menu_sound.play()
                key = pygame.key.name(event.key)
                key = _corrector(key)
                if key == 'enter':
                    time =  pygame.time.get_ticks()
                    data = _data_grab(UserID, time)
                    return data, UserID

                    
                if key == 'backspace':
                    UserID = UserID[:-1]

                else:
                    UserID = UserID+key

        screen.fill(blackcolor)
        screen.blit(label1, (10, 100))

        screen.blit(label2, (370, 100))
        fps.tick(60)
        pygame.display.flip()

def _data_grab(UserID, time):
    '''
    This function will call another function from a different module to find out
    if the account the user has input exists. It will display on the screen the results.
    If an account is not found, this function will call the _intro() function to ask
    the user again for the ID.
    '''
    while True:
        events = pygame.event.get()
        for event in events: 
            if event.type == QUIT:
                pygame.quit()
                return
        label1 = font.render('Searching For Account',1,whitecolor)
        label2 = font.render('Searching For Account.',1,whitecolor)
        label3 = font.render('Searching For Account..',1,whitecolor)
        label4 = font.render('Searching For Account...',1,whitecolor)
        label5 = font.render('Account Found!',1,whitecolor)
        label6 = font.render('Account Not Found!', 1,whitecolor)
        
        screen.fill(blackcolor)
        new_time = pygame.time.get_ticks()
        if (new_time-time) < 5500:
            screen.blit(label1, (200,100))
            
            if (new_time-time) > 1000:
                screen.blit(label2, (200, 100))
            if (new_time-time) > 2500:
                screen.blit(label3, (200, 100))
            if (new_time-time) > 4500:
                screen.blit(label4, (200,100))

        if (new_time-time) > 5500:
            try:
                data = _get_data(UserID)
                screen.blit(label5, (240,100))
                if (new_time-time) > 7000:
                    return data
            except:
                screen.blit(label6, (240,100))
                if (new_time-time) > 7000:
                        return
                
        
        
        fps.tick(60)
        pygame.display.flip()

def _main_menu(battles, pokemon, shinypokemon, team_pkmn, coins, games_completed, lotto_cost, UserID):
    '''
    This function provides the user the main menu of the game. The user will be able to choose
    which part of the game he/she wants.
    '''
    menu_music.play(-1)
    battle_sprite = Battle_Sprite()
    slot_sprite = Slot_Sprite()
    pc_sprite = PC_Sprite()
    rule_sprite = Rule_Sprite()

    x1 = 145
    x2 = 170
    x3 = 157.5

    y1 = 50
    y2 = 70

    
    allsprites = pygame.sprite.RenderPlain((battle_sprite, slot_sprite, pc_sprite, rule_sprite))

    
    label1 = font.render('Battle', 1, blackcolor)
    label2 = font.render('Lotto', 1, blackcolor)
    label3 = font.render('PC', 1, blackcolor)
    label4 = font.render('Info', 1, blackcolor)
    
    while True:
        if battles < 8:
            battle_fee = 500
        if  11 > battles >= 8:
            battle_fee = 1000
        if battles >= 11:
            battle_fee = 2000
        label5 = menu_font.render('Battles: {}  Pkmn: {}  Shiny Pkmn: {}  Games Completed: {}  Coins: {}'.format(battles, len(pokemon), len(shinypokemon), games_completed, coins), 1, blackcolor)
        label6 = menu_font.render('Cost: {}'.format(lotto_cost), 1, blackcolor)
        label7 = menu_font.render('Cost: {}'.format(battle_fee), 1, blackcolor)

        events = pygame.event.get()
        for event in events: 
            if event.type == QUIT:
                pygame.quit()
                return
            elif event.type == pygame.KEYDOWN:
                if event.key == pygame.K_KP_ENTER or event.key == pygame.K_RETURN:
                    menu_sound.play()
                    if y1 == 250 and y2 == 270 and x1 == 470 and x2 == 495 and x3 == 482.5:
                        _info()
                    if y1 == 250 and y2 == 270 and x1 == 145 and x2 == 170 and x3 == 157.5:
                        _PC(pokemon, shinypokemon, team_pkmn, UserID)
                    if y1 == 50 and y2 == 70 and x1 == 145 and x2 == 170 and x3 == 157.5:
                        if coins < battle_fee and battles != 12:
                            _insufficient_funds()
                        if battles >= 12:
                            _battles_done()
                        if coins >= battle_fee and battles != 12:
                            menu_music.stop()
                            battle_start.play()
                            pygame.time.wait(2800)
                            winner = _battle(battles, team_pkmn, shinypokemon)
                            if winner != None:
                                battles += 1
                                _line_replacer(0, battles, UserID)
                            coins -= battle_fee
                            _line_replacer(4, coins, UserID)
                            if battles == 12 and len(pokemon) == 151 and len(shinypokemon) != 151:
                                battles = 0
                                pokemon = shinypokemon
                                team_pkmn = []
                                coins = 0
                                games_completed += 1
                                _line_replacer(4, coins, UserID)
                                _line_replacer(1, pokemon, UserID)
                                _line_replacer(2, shinypokemon, UserID)
                                _line_replacer(5, team_pkmn, UserID)
                                _line_replacer(3, games_completed, UserID)
                                _line_replacer(0, battles, UserID)
                                _resetter(1)
                                _lotto(pokemon, shinypokemon, team_pkmn, games_completed)
                                _line_replacer(1, pokemon, UserID)
                                _line_replacer(2, shinypokemon, UserID)
                                _line_replacer(5, team_pkmn, UserID)
                            if battles == 12 and len(pokemon) == 151 and len(shinypokemon) == 151:
                                _resetter(2)

                            
                    if y1 == 50 and y2 == 70 and x1 == 470 and x2 == 495 and x3 == 482.5:
                        if coins < lotto_cost:
                            _insufficient_funds()
                        if len(pokemon) == 151:
                            _insufficient_funds('full')
                        if coins >= lotto_cost and len(pokemon) != 151:
                            menu_music.stop()
                            _lotto(pokemon, shinypokemon, team_pkmn, games_completed)
                            menu_music.play(-1)
                            coins -= lotto_cost
                            _line_replacer(4, coins, UserID)
                            _line_replacer(1, pokemon, UserID)
                            _line_replacer(2, shinypokemon, UserID)
                            _line_replacer(5, team_pkmn, UserID)
                            if battles == 12 and len(pokemon) == 151 and len(shinypokemon) != 151:
                                battles = 0
                                pokemon = shinypokemon
                                team_pkmn = []
                                coins = 0
                                games_completed += 1
                                _line_replacer(4, coins, UserID)
                                _line_replacer(1, pokemon, UserID)
                                _line_replacer(2, shinypokemon, UserID)
                                _line_replacer(5, team_pkmn, UserID)
                                _line_replacer(3, games_completed, UserID)
                                _line_replacer(0, battles, UserID)
                                _resetter(1)
                                _lotto(pokemon, shinypokemon, team_pkmn, games_completed)
                                _line_replacer(1, pokemon, UserID)
                                _line_replacer(2, shinypokemon, UserID)
                                _line_replacer(5, team_pkmn, UserID)
                            if battles == 12 and len(pokemon) == 151 and len(shinypokemon) == 151:
                                _resetter(2)
                            


                if event.key == pygame.K_s and y1 != 250 and y2 != 270:
                    menu_sound.play()
                    y1 = 250
                    y2 = 270
                if event.key == pygame.K_w and y1 != 50 and y2 != 70:
                    menu_sound.play()
                    y1 = 50
                    y2 = 70
                if event.key == pygame.K_d and x1 != 470 and x2 != 495 and x3 != 482.5:
                    menu_sound.play()
                    x1 = 470
                    x2 = 495
                    x3 = 482.5
                if event.key == pygame.K_a and x1 != 145 and x2 != 170 and x3 != 157.5:
                    menu_sound.play()
                    x1 = 145
                    x2 = 170
                    x3 = 157.5


            

            
            
    
    
                
        screen.fill(whitecolor)

        pygame.draw.rect(screen, blackcolor, [80, 75, 150, 150], 2)
        pygame.draw.rect(screen, blackcolor, [400, 75, 150, 150], 2)
        pygame.draw.rect(screen, blackcolor, [80, 275, 150, 150], 2)
        pygame.draw.rect(screen, blackcolor, [400, 275, 150, 150], 2)

        pygame.draw.polygon(screen, blackcolor,((x1,y1),(x2,y1), (x3,y2)))
        screen.blit(label1, (125, 80))
        screen.blit(label2, (445, 80))
        screen.blit(label3, (145, 280))
        screen.blit(label4, (445, 280))
        screen.blit(label5, (0,0))
        screen.blit(label6, (450, 200))
        screen.blit(label7, (130, 200))
                    

        allsprites.draw(screen)



        fps.tick(60)
        pygame.display.flip()

def _info():
    '''
    If the player chooses Info from the main menu, the player will be presented by this
    function. This function shows the user information about the game. It is like a
    handbook.
    '''
    n = 0
    label1 = menu_font.render('Here you can learn everything about this game here. You can also look up', 1, blackcolor)
    label2 = menu_font.render('Poke... I mean Monster types. Okay, fine. Pokemon. Anyways, Use the a', 1, blackcolor)
    label3 = menu_font.render('and b keys to turn pages. Press b to return to the main menu.', 1, blackcolor)
    labeloak = menu_font.render('               -Prof. Poison Oak', 1, blackcolor)
    
    label4 = menu_font.render('Coins:', 1, blackcolor)
    label5 = menu_font.render('You can earn coins in 24 hour intervals. The amount of coins you earn depends', 1, blackcolor)                     
    label6 = menu_font.render('on the level of physcial activity you have performed in the real world.', 1, blackcolor)
    label7 = menu_font.render('Activity Levels: VERY HIGH = 5 coins, HIGH = 3, MEDIUM = 2, LOW = 1, VERY LOW = 0', 1, blackcolor)

    label8 = menu_font.render('Battle:', 1, blackcolor)
    label9 = menu_font.render('You can spend your coins to conduct battles. One of of your goals in this', 1, blackcolor)
    label10 = menu_font.render('game is to beat all 8 Gym leaders, the Elite Three, and the Champion.', 1, blackcolor)
    label11 = menu_font.render('Initially battles will cost 500 coins. However, once you face the Elite', 1, blackcolor)
    label12 = menu_font.render('Three and the Champion, the cost will raise significantly!', 1,blackcolor)

    label13 = menu_font.render('Battle System:', 1, blackcolor)
    label14 = menu_font.render('In a battle you can choose to attack or to switch Pokemon with another in', 1, blackcolor)
    label15 = menu_font.render('your team. You can only hold at most 6 Pokemon in your team at a time.', 1, blackcolor)
    label16 = menu_font.render('The effectiveness of an attack depends on the type of the Pokemon.', 1, blackcolor)
    label17 = menu_font.render('Here is a table that may help you understand attack effectiveness.', 1,blackcolor)
    label18 = menu_font.render('Damage Amount     Super Effective     Neutral        Not Very', 1, blackcolor)
    label19 = menu_font.render('    100%                      75%              25%              0%', 1, blackcolor)
    label20 = menu_font.render('     50%                       25%             40%              25%', 1, blackcolor)
    label21 = menu_font.render('     25%                        0%              25%             50%', 1, blackcolor)
    label22 = menu_font.render('      0%                        0%              10%             25%', 1, blackcolor)
    label23 = menu_font.render('So for example, a fire Pokemon has a 75% chance to inflict 100% damage or a 25%', 1, blackcolor)
    label24 = menu_font.render('chance to inflict 50% damage on an Ice Pokemon. Which types are effective', 1, blackcolor)
    label25 = menu_font.render('against other types? Well that is for you to find out! Or you can look ', 1,blackcolor)
    label26 = menu_font.render('it up on the internet. *sigh* What do you mean there are more than 151', 1, blackcolor)
    label27 = menu_font.render('Pokemon?!?! New types?!?! Different type effectiveness?!?! How dare you!', 1, blackcolor)
    label28 = menu_font.render("I have studied Pokemon for many years. I cannot just climb a mountain", 1, blackcolor)
    label29 = menu_font.render("and find a new region filled with...WHO ARE THESE PEOPLE?!?!", 1, blackcolor)

    label30 = menu_font.render("PC:", 1, blackcolor)
    label31 = menu_font.render("In your PC you can deposit Pokemon from you team into a box. No, it is", 1, blackcolor)
    label32 = menu_font.render('not animal cruelty. You can also withdraw Pokemon from you box into your', 1, blackcolor)
    label33 = menu_font.render('party, but only if you have space. All 151 Pokemon names appear here, but', 1, blackcolor)
    label34 = menu_font.render('only the Pokemon you have obtained will have a Pokeball symbol next to. And', 1, blackcolor)
    label35 = menu_font.render('only those are you able to withdraw. If you have a shiny version of a Pokemon, ', 1, blackcolor)
    label36 = menu_font.render('the Pokeball next to it will be in a different color. Shiny Pokemon are Pokemon', 1, blackcolor)
    label37 = menu_font.render('that are in a different color than are supposed to be.', 1, blackcolor)

    label38 = menu_font.render('Lotto:', 1, blackcolor)
    label39 = menu_font.render('You can also use your coins to get a new Pokemon. The Pokemon you will get', 1, blackcolor)
    label40 = menu_font.render('will be at random, but you will never get a repeat Pokemon. How wonderful!', 1, blackcolor)
    label41 = menu_font.render('The cost of the lotto will always be 200. It is a lot, but if you do plenty', 1, blackcolor)
    label42 = menu_font.render('of excercise, you will be fine. Also, there is slim chance that a Pokemon', 1, blackcolor)
    label43 = menu_font.render('you get will be shiny. Can you get all 151 Pokemon? Of course, I have seen', 1, blackcolor)
    label44 = menu_font.render('and own all 151.I AM a Professor of Pokemon. I need to know everything. I', 1, blackcolor)
    label45 = menu_font.render('am not like those other Professors that make children do their research. I', 1, blackcolor)
    label46 = menu_font.render('have even sent you all Pokemon names and types to your PC.', 1, blackcolor)

    label47 = menu_font.render('Game Completed:', 1, blackcolor)
    label48 = menu_font.render('After you beat all Pokemon Trainers and have collected all 151 pokemon, your', 1, blackcolor)
    label49 = menu_font.render('journey is not over yet! You can reset the game and play it all over again!', 1, blackcolor)
    label50 = menu_font.render('Why would you do that? Well, everytime you reset the game, the chance of', 1, blackcolor)
    label51 = menu_font.render('getting a shiny pokemon gets higher! Resetting makes you lose all of your', 1, blackcolor)
    label52 = menu_font.render('badges and Pokemon, but you get to keep your shiny Pokemon! If you get all', 1, blackcolor)
    label53 = menu_font.render('151 shiny Pokemon and beat all Pokemon trainers, you will get a special prize!', 1, blackcolor)

    label54 = menu_font.render('1-Bulbasaur: Grass Poison', 1, blackcolor)
    label55 = menu_font.render('2-Ivysaur: Grass Poison', 1, blackcolor)
    label56 = menu_font.render('3-Venusaur: Grass Poison', 1, blackcolor)
    label57 = menu_font.render('4-Charmander: Fire', 1, blackcolor)
    label58 = menu_font.render('5-Charmeleon: Fire', 1, blackcolor)
    label59 = menu_font.render('6-Charizard: Fire Flying', 1, blackcolor)
    label60 = menu_font.render('7-Squirtle: Water', 1, blackcolor)
    label61 = menu_font.render('8-Wartortle: Water', 1, blackcolor)
    label62 = menu_font.render('9-Blastoise: Water', 1, blackcolor)
    label63 = menu_font.render('10-Caterpie: Bug', 1, blackcolor)
    label64 = menu_font.render('11-Metapod: Bug', 1, blackcolor)
    label65 = menu_font.render('12-Butterfree: Bug Flying', 1, blackcolor)
    label66 = menu_font.render('13-Weedle: Bug Poison', 1, blackcolor)
    label67 = menu_font.render('14-Kakuna: Bug Poison', 1, blackcolor)
    label68 = menu_font.render('15-Beedrill: Bug Poison', 1, blackcolor)
    label69 = menu_font.render('16-Pidgey: Normal Flying', 1, blackcolor)
    label70 = menu_font.render('17-Pidgeotto: Normal Flying', 1, blackcolor)
    label71 = menu_font.render('18-Pidgeot: Normal Flying', 1, blackcolor)
    label72 = menu_font.render('19-Rattata: Normal', 1, blackcolor)
    label73 = menu_font.render('20-Raticate: Normal', 1, blackcolor)
    label74 = menu_font.render('21-Spearow: Normal Flying', 1, blackcolor)
    label75 = menu_font.render('22-Fearow: Normal Flying', 1, blackcolor)
    label76 = menu_font.render('23-Ekans: Poison', 1, blackcolor)
    label77 = menu_font.render('24-Arbok: Poison', 1, blackcolor)
    label78 = menu_font.render('25-Pikachu: Electric', 1, blackcolor)
    label79 = menu_font.render('26-Raichu: Electric', 1, blackcolor)
    label80 = menu_font.render('27-Sandshrew: Ground', 1, blackcolor)
    label81 = menu_font.render('28-Sandslash: Ground', 1, blackcolor)
    label82 = menu_font.render('29-NidoranF: Poison', 1, blackcolor)
    label83 = menu_font.render('30-Nidorina: Poison', 1, blackcolor)
    label84 = menu_font.render('31-Nidoqueen: Poison Ground', 1, blackcolor)
    label85 = menu_font.render('32-NidoranM: Poison', 1, blackcolor)
    label86 = menu_font.render('33-Nidorino: Poison', 1, blackcolor)
    label87 = menu_font.render('34-Nidoking: Poison Ground', 1, blackcolor)
    label88 = menu_font.render('35-Clefairy: Normal', 1, blackcolor)
    label89 = menu_font.render('36-Clefable: Normal', 1, blackcolor)
    label90 = menu_font.render('37-Vulpix: Fire', 1, blackcolor)
    label91 = menu_font.render('38-Ninetales: Fire', 1, blackcolor)
    label92 = menu_font.render('39-Jigglypuff: Normal', 1, blackcolor)
    label93 = menu_font.render('40-Wigglytuff: Normal', 1, blackcolor)
    label94 = menu_font.render('41-Zubat: Flying Poison', 1, blackcolor)
    label95 = menu_font.render('42-Golbat: Flying Poison', 1, blackcolor)
    label96 = menu_font.render('43-Oddish: Grass Poison', 1, blackcolor)
    label97 = menu_font.render('44-Gloom: Grass Poison', 1, blackcolor)
    label98 = menu_font.render('45-Vileplume: Grass Poison', 1, blackcolor)
    label99 = menu_font.render('46-Paras: Bug Grass', 1, blackcolor)
    label100 = menu_font.render('47-Parasect: Bug Grass', 1, blackcolor)
    label101 = menu_font.render('48-Venonat: Bug Poison', 1, blackcolor)
    label102 = menu_font.render('49-Venomoth: Bug Poison', 1, blackcolor)
    label103 = menu_font.render('50-Diglett: Ground', 1, blackcolor)
    label104 = menu_font.render('51-Dugtrio: Ground', 1, blackcolor)
    label105 = menu_font.render('52-Meowth: Normal', 1, blackcolor)
    label106 = menu_font.render('53-Persian: Normal', 1, blackcolor)
    label107 = menu_font.render('54-Psyduck: Water', 1, blackcolor)
    label108 = menu_font.render('55-Golduck: Water', 1, blackcolor)
    label109 = menu_font.render('56-Mankey: Fighting', 1, blackcolor)
    label110 = menu_font.render('57-Primeape: Fighting', 1, blackcolor)
    label111 = menu_font.render('58-Growlithe: Fire', 1, blackcolor)
    label112 = menu_font.render('59-Arcanine: Fire', 1, blackcolor)
    label113 = menu_font.render('60-Poliwag: Water', 1, blackcolor)
    label114 = menu_font.render('61-Poliwhirl: Water', 1, blackcolor)
    label115 = menu_font.render('62-Poliwrath: Water Fighting', 1, blackcolor)
    label116 = menu_font.render('63-Abra: Psychic', 1, blackcolor)
    label117 = menu_font.render('64-Kadabra: Psychic', 1, blackcolor)
    label118 = menu_font.render('65-Alakazam: Psychic', 1, blackcolor)
    label119 = menu_font.render('66-Machop: Fighting', 1, blackcolor)
    label120 = menu_font.render('67-Machoke: Fighting', 1, blackcolor)
    label121 = menu_font.render('68-Machamp: Fighting', 1, blackcolor)
    label122 = menu_font.render('69-Bellsprout: Grass Poison', 1, blackcolor)
    label123 = menu_font.render('70-Weepinbell: Grass Poison', 1, blackcolor)
    label124 = menu_font.render('71-Victreebel: Grass Poison', 1, blackcolor)
    label125 = menu_font.render('72-Tentacool: Water Poison', 1, blackcolor)
    label126 = menu_font.render('73-Tentacruel: Water Poison', 1, blackcolor)
    label127 = menu_font.render('74-Geodude: Rock Ground', 1, blackcolor)
    label128 = menu_font.render('75-Graveler: Rock Ground', 1, blackcolor)
    label129 = menu_font.render('76-Golem: Rock Ground', 1, blackcolor)
    label130 = menu_font.render('77-Ponyta: Fire', 1, blackcolor)
    label131 = menu_font.render('78-Rapidash: Fire', 1, blackcolor)
    label132 = menu_font.render('79-Slowpoke: Water Psychic', 1, blackcolor)
    label133 = menu_font.render('80-Slowbro: Water Psychic', 1, blackcolor)
    label134 = menu_font.render('81-Magnemite: Electric', 1, blackcolor)
    label135 = menu_font.render('82-Magneton: Electric', 1, blackcolor)
    label136 = menu_font.render('83-FarfetchD: Normal Flying', 1, blackcolor)
    label137 = menu_font.render('84-Doduo: Normal Flying', 1, blackcolor)
    label138 = menu_font.render('85-Dodrio: Normal Flying', 1, blackcolor)
    label139 = menu_font.render('86-Seel: Water', 1, blackcolor)
    label140 = menu_font.render('87-Dewgong: Water Ice', 1, blackcolor)
    label141 = menu_font.render('88-Grimer: Poison', 1, blackcolor)
    label142 = menu_font.render('89-Muk: Poison', 1, blackcolor)
    label143 = menu_font.render('90-Shellder: Water', 1, blackcolor)
    label144 = menu_font.render('91-Cloyster: Water Ice', 1, blackcolor)
    label145 = menu_font.render('92-Gastly: Ghost Poison', 1, blackcolor)
    label146 = menu_font.render('93-Haunter: Ghost Poison', 1, blackcolor)
    label147 = menu_font.render('94-Gengar: Ghost Poison', 1, blackcolor)
    label148 = menu_font.render('95-Onix: Rock Ground', 1, blackcolor)
    label149 = menu_font.render('96-Drowzee: Psychic', 1, blackcolor)
    label150 = menu_font.render('97-Hypno: Psychic', 1, blackcolor)
    label151 = menu_font.render('98-Krabby: Water', 1, blackcolor)
    label152 = menu_font.render('99-Kingler: Water', 1, blackcolor)
    label153 = menu_font.render('100-Voltorb: Electric', 1, blackcolor)
    label154 = menu_font.render('101-Electrode: Electric', 1, blackcolor)
    label155 = menu_font.render('102-Exeggcute: Grass Psychic', 1, blackcolor)
    label156 = menu_font.render('103-Exeggutor: Grass Psychic', 1, blackcolor)
    label157 = menu_font.render('104-Cubone: Ground', 1, blackcolor)
    label158 = menu_font.render('105-Marowak: Ground', 1, blackcolor)
    label159 = menu_font.render('106-Hitmonlee: Fighting', 1, blackcolor)
    label160 = menu_font.render('107-Hitmonchan: Fighting', 1, blackcolor)
    label161 = menu_font.render('108-Lickitung: Normal', 1, blackcolor)
    label162 = menu_font.render('109-Koffing: Poison', 1, blackcolor)
    label163 = menu_font.render('110-Weezing: Poison', 1, blackcolor)
    label164 = menu_font.render('111-Rhyhorn: Ground Rock', 1, blackcolor)
    label165 = menu_font.render('112-Rhydon: Ground Rock', 1, blackcolor)
    label166 = menu_font.render('113-Chansey: Normal', 1, blackcolor)
    label167 = menu_font.render('114-Tangela: Grass', 1, blackcolor)
    label168 = menu_font.render('115-Kangaskhan: Normal', 1, blackcolor)
    label169 = menu_font.render('116-Horsea: Water', 1, blackcolor)
    label170 = menu_font.render('117-Seadra: Water', 1, blackcolor)
    label171 = menu_font.render('118-Goldeen: Water', 1, blackcolor)
    label172 = menu_font.render('119-Seaking: Water', 1, blackcolor)
    label173 = menu_font.render('120-Staryu: Water', 1, blackcolor)
    label174 = menu_font.render('121-Starmie: Water Psychic', 1, blackcolor)
    label175 = menu_font.render('122-Mr_Mime: Psychic', 1, blackcolor)
    label176 = menu_font.render('123-Scyther: Bug Flying', 1, blackcolor)
    label177 = menu_font.render('124-Jynx: Psychic Ice', 1, blackcolor)
    label178 = menu_font.render('125-Electabuzz: Electric', 1, blackcolor)
    label179 = menu_font.render('126-Magmar: Fire', 1, blackcolor)
    label180 = menu_font.render('127-Pinsir: Bug', 1, blackcolor)
    label181 = menu_font.render('128-Tauros: Normal', 1, blackcolor)
    label182 = menu_font.render('129-Magikarp: Water', 1, blackcolor)
    label183 = menu_font.render('130-Gyarados: Water Flying', 1, blackcolor)
    label184 = menu_font.render('131-Lapras: Water Ice', 1, blackcolor)
    label185 = menu_font.render('132-Ditto: Normal', 1, blackcolor)
    label186 = menu_font.render('133-Eevee: Normal', 1, blackcolor)
    label187 = menu_font.render('134-Vaporeon: Water', 1, blackcolor)
    label188 = menu_font.render('135-Jolteon: Electric', 1, blackcolor)
    label189 = menu_font.render('136-Flareon: Fire', 1, blackcolor)
    label190 = menu_font.render('137-Porygon: Normal', 1, blackcolor)
    label191 = menu_font.render('138-Omanyte: Water Rock', 1, blackcolor)
    label192 = menu_font.render('139-Omastar: Water Rock', 1, blackcolor)
    label193 = menu_font.render('140-Kabuto: Water Rock', 1, blackcolor)
    label194 = menu_font.render('141-Kabutops: Water Rock', 1, blackcolor)
    label195 = menu_font.render('142-Aerodactyl: Rock Flying', 1, blackcolor)
    label196 = menu_font.render('143-Snorlax: Normal', 1, blackcolor)
    label197 = menu_font.render('144-Articuno: Ice Flying', 1, blackcolor)
    label198 = menu_font.render('145-Zapdos: Electric Flying', 1, blackcolor)
    label199 = menu_font.render('146-Moltres: Fire Flying', 1, blackcolor)
    label200 = menu_font.render('147-Dratini: Dragon', 1, blackcolor)
    label201 = menu_font.render('148-Dragonair: Dragon', 1, blackcolor)
    label202 = menu_font.render('149-Dragonite: Dragon Flying', 1, blackcolor)
    label203 = menu_font.render('150-Mewtwo: Psychic', 1, blackcolor)
    label204 = menu_font.render('151-Mew: Psychic', 1, blackcolor)
    labeloak2 = menu_font.render('Pokemon & Types:', 1, blackcolor) 

    while True:
        for event in pygame.event.get(): 
            if event.type == QUIT:
                pygame.quit()
                return
            elif event.type == pygame.KEYDOWN:
                if event.key == pygame.K_d:
                    if n != 13:
                        menu_sound.play()
                        n += 1
                if event.key == pygame.K_a:
                    if n != 0:
                        menu_sound.play()
                        n -= 1
                if event.key == pygame.K_b:
                    return
                    

        
        screen.fill(whitecolor)
        pressed = pygame.key.get_pressed()

        if n == 0:
            screen.blit(label1, (10, 0))
            screen.blit(label2,(10, 30))
            screen.blit(label3, (10, 60))
            screen.blit(label4, (10, 120))
            screen.blit(label5, (10, 150))
            screen.blit(label6,(10, 180))
            screen.blit(label7,(10, 210))
            screen.blit(label8, (10, 270))
            screen.blit(label9, (10, 300))
            screen.blit(label10, (10, 330))
            screen.blit(label11, (10, 360))
            screen.blit(label12, (10, 390))
            screen.blit(labeloak, (10, 90))

        if n == 1:
            screen.blit(label13, (10, 0))
            screen.blit(label14,(10, 30))
            screen.blit(label15, (10, 60))
            screen.blit(label16, (10, 90))
            screen.blit(label17, (10, 120))
            screen.blit(label18,(10, 150))
            screen.blit(label19,(10, 180))
            screen.blit(label20, (10, 210))
            screen.blit(label21, (10, 240))
            screen.blit(label22, (10, 270))
            screen.blit(label23, (10, 300))
            screen.blit(label24, (10, 330))
            screen.blit(label25, (10, 360))
            screen.blit(label26, (10, 390))
            screen.blit(label27, (10, 420))
            screen.blit(label28, (10, 450))
         

        if n == 2:
            screen.blit(label29, (10, 0))
            screen.blit(label30, (10, 60))
            screen.blit(label31, (10, 90))
            screen.blit(label32, (10, 120))
            screen.blit(label33, (10, 150))
            screen.blit(label34, (10, 180))
            screen.blit(label35, (10, 210))
            screen.blit(label36, (10, 240))
            screen.blit(label37, (10, 270))
            screen.blit(label38, (10, 330))
            screen.blit(label39, (10, 360))
            screen.blit(label40, (10, 390))
            screen.blit(label41, (10, 420))
            screen.blit(label42, (10, 450))

        if n == 3:
            screen.blit(label43, (10, 0))
            screen.blit(label44, (10, 30))
            screen.blit(label45, (10, 60))
            screen.blit(label46, (10, 90))
            screen.blit(label47, (10, 150))
            screen.blit(label48, (10, 180))
            screen.blit(label49, (10, 210))
            screen.blit(label50, (10, 240))
            screen.blit(label51, (10, 270))
            screen.blit(label52, (10, 300))
            screen.blit(label53, (10, 330))
        if n == 4:
            screen.blit(labeloak2, (10, 0))
            screen.blit(label54, (10, 30))
            screen.blit(label55, (10, 60))
            screen.blit(label56, (10, 90))
            screen.blit(label57, (10, 120))
            screen.blit(label58, (10, 150))
            screen.blit(label59, (10, 180))
            screen.blit(label60, (10, 210))
            screen.blit(label61, (10, 240))
            screen.blit(label62, (10, 270))
            screen.blit(label63, (10, 300))
            screen.blit(label64, (10, 330))
            screen.blit(label65, (10, 360))
            screen.blit(label66, (10, 390))
            screen.blit(label67, (10, 420))
            screen.blit(label68, (10, 450))
        if n == 5:
            screen.blit(label69, (10, 0))
            screen.blit(label70, (10, 30))
            screen.blit(label71, (10, 60))
            screen.blit(label72, (10, 90))
            screen.blit(label73, (10, 120))
            screen.blit(label74, (10, 150))
            screen.blit(label75, (10, 180))
            screen.blit(label76, (10, 210))
            screen.blit(label77, (10, 240))
            screen.blit(label78, (10, 270))
            screen.blit(label79, (10, 300))
            screen.blit(label80, (10, 330))
            screen.blit(label81, (10, 360))
            screen.blit(label82, (10, 390))
            screen.blit(label83, (10, 420))
            screen.blit(label84, (10, 450))
        if n == 6:
            screen.blit(label85, (10, 0))
            screen.blit(label86, (10, 30))
            screen.blit(label87, (10, 60))
            screen.blit(label88, (10, 90))
            screen.blit(label89, (10, 120))
            screen.blit(label90, (10, 150))
            screen.blit(label91, (10, 180))
            screen.blit(label92, (10, 210))
            screen.blit(label93, (10, 240))
            screen.blit(label94, (10, 270))
            screen.blit(label95, (10, 300))
            screen.blit(label96, (10, 330))
            screen.blit(label97, (10, 360))
            screen.blit(label98, (10, 390))
            screen.blit(label99, (10, 420))
            screen.blit(label100, (10, 450))
        if n == 7:
            screen.blit(label101, (10, 0))
            screen.blit(label102, (10, 30))
            screen.blit(label103, (10, 60))
            screen.blit(label104, (10, 90))
            screen.blit(label105, (10, 120))
            screen.blit(label106, (10, 150))
            screen.blit(label107, (10, 180))
            screen.blit(label108, (10, 210))
            screen.blit(label109, (10, 240))
            screen.blit(label110, (10, 270))
            screen.blit(label111, (10, 300))
            screen.blit(label112, (10, 330))
            screen.blit(label113, (10, 360))
            screen.blit(label114, (10, 390))
            screen.blit(label115, (10, 420))
            screen.blit(label116, (10, 450))
        if n == 8:
            screen.blit(label117, (10, 0))
            screen.blit(label118, (10, 30))
            screen.blit(label119, (10, 60))
            screen.blit(label120, (10, 90))
            screen.blit(label121, (10, 120))
            screen.blit(label122, (10, 150))
            screen.blit(label123, (10, 180))
            screen.blit(label124, (10, 210))
            screen.blit(label125, (10, 240))
            screen.blit(label126, (10, 270))
            screen.blit(label127, (10, 300))
            screen.blit(label128, (10, 330))
            screen.blit(label129, (10, 360))
            screen.blit(label130, (10, 390))
            screen.blit(label131, (10, 420))
            screen.blit(label132, (10, 450))
        if n == 9:
            screen.blit(label133, (10, 0))
            screen.blit(label134, (10, 30))
            screen.blit(label135, (10, 60))
            screen.blit(label136, (10, 90))
            screen.blit(label137, (10, 120))
            screen.blit(label138, (10, 150))
            screen.blit(label139, (10, 180))
            screen.blit(label140, (10, 210))
            screen.blit(label141, (10, 240))
            screen.blit(label142, (10, 270))
            screen.blit(label143, (10, 300))
            screen.blit(label144, (10, 330))
            screen.blit(label145, (10, 360))
            screen.blit(label146, (10, 390))
            screen.blit(label147, (10, 420))
            screen.blit(label148, (10, 450))
        if n == 10: 
            screen.blit(label149, (10, 0))
            screen.blit(label150, (10, 30))
            screen.blit(label151, (10, 60))
            screen.blit(label152, (10, 90))
            screen.blit(label153, (10, 120))
            screen.blit(label154, (10, 150))
            screen.blit(label155, (10, 180))
            screen.blit(label156, (10, 210))
            screen.blit(label157, (10, 240))
            screen.blit(label158, (10, 270))
            screen.blit(label159, (10, 300))
            screen.blit(label160, (10, 330))
            screen.blit(label161, (10, 360))
            screen.blit(label162, (10, 390))
            screen.blit(label163, (10, 420))
            screen.blit(label164, (10, 450))
        if n == 11:
            screen.blit(label165, (10, 0))
            screen.blit(label166, (10, 30))
            screen.blit(label167, (10, 60))
            screen.blit(label168, (10, 90))
            screen.blit(label169, (10, 120))
            screen.blit(label170, (10, 150))
            screen.blit(label171, (10, 180))
            screen.blit(label172, (10, 210))
            screen.blit(label173, (10, 240))
            screen.blit(label174, (10, 270))
            screen.blit(label175, (10, 300))
            screen.blit(label176, (10, 330))
            screen.blit(label177, (10, 360))
            screen.blit(label178, (10, 390))
            screen.blit(label179, (10, 420))
            screen.blit(label180, (10, 450))
        if n == 12:
            screen.blit(label181, (10, 0))
            screen.blit(label182, (10, 30))
            screen.blit(label183, (10, 60))
            screen.blit(label184, (10, 90))
            screen.blit(label185, (10, 120))
            screen.blit(label186, (10, 150))
            screen.blit(label187, (10, 180))
            screen.blit(label188, (10, 210))
            screen.blit(label189, (10, 240))
            screen.blit(label190, (10, 270))
            screen.blit(label191, (10, 300))
            screen.blit(label192, (10, 330))
            screen.blit(label193, (10, 360))
            screen.blit(label194, (10, 390))
            screen.blit(label195, (10, 420))
            screen.blit(label196, (10, 450))
        if n == 13:
            screen.blit(label197, (10, 0))
            screen.blit(label198, (10, 30))
            screen.blit(label199, (10, 60))
            screen.blit(label200, (10, 90))
            screen.blit(label201, (10, 120))
            screen.blit(label202, (10, 150))
            screen.blit(label203, (10, 180))
            screen.blit(label204, (10, 210))

            
            
            

        fps.tick(60)
        pygame.display.flip()

def _lotto(pokemon, shinypokemon, team_pkmn, games_completed,intro = None):
    '''
    If the user chooses Lotto from the main menu, this function will act.
    It will give the user a random pokemon back. It will put the pokemon in
    a team or the PC depending if there is space in the PC.
    '''

    n = 50 - games_completed
    if n <= 0:
        n = 1
    while True:
        number = random.randint(1,151)
        if number not in pokemon:
            break
    shiny_random = random.randint(1,n)
    if shiny_random == 1:
        pokemon.append(number)
        shinypokemon.append(number)
        lotto_sprite = Lotto_Sprite(number, "shiny")
    else:
        lotto_sprite = Lotto_Sprite(number)
        pokemon.append(number)
    pokemon_name = pokemon_info._get_name(number)
    if len(team_pkmn) < 6:
        team_pkmn.append(number)
        label1 = menu_font.render('You got {}! {} has been sent to your team.'.format(pokemon_name, pokemon_name), 1, blackcolor)
    else:
        label1 = menu_font.render('You got {}! {} has been sent to your PC.'.format(pokemon_name, pokemon_name), 1, blackcolor)
    allsprites = pygame.sprite.RenderPlain((lotto_sprite))
    all_cries[number-1].play()
    label2 = menu_font.render("It is a shiny!", 1, blackcolor)
    while True:

        events = pygame.event.get()
        for event in events: 
            if event.type == QUIT:
                pygame.quit()

            elif event.type == pygame.KEYDOWN:
                if event.key == pygame.K_KP_ENTER or event.key == pygame.K_RETURN:
                    if intro != None:
                        return pokemon, shinypokemon, team_pkmn
                    else:
                        return
            

        screen.fill(whitecolor)
        screen.blit(label1, (115, 240))
        if shiny_random == 1:
            screen.blit(label2, (260, 270))
        allsprites.draw(screen)

        fps.tick(60)
        pygame.display.flip()

    
def _insufficient_funds(full=None):
    '''
    If the player does not have enough funds for battle or the lottery, the player will see a message from Prof. Poison Oak.
    It also appears when the player already has all 151 pokemon.
    '''
    new_time = pygame.time.get_ticks()
    if full == None:
        label1 = menu_font.render('Insufficient Funds. Do More Excercise!', 1, blackcolor)
    if full != None:
        label1 = menu_font.render('You already have all Pokemon!', 1, blackcolor)
    label2 = menu_font.render('     -Prof. Poison Oak', 1, blackcolor)

    while True:
        events = pygame.event.get()
        for event in events: 
            if event.type == QUIT:
                pygame.quit()
                return
            elif event.type == pygame.KEYDOWN:
                if event.key == pygame.K_KP_ENTER or event.key == pygame.K_RETURN:
                    pass

        
        time = pygame.time.get_ticks()

        if (time-new_time) < 4000:
            screen.fill(whitecolor)
            screen.blit(label1, (180, 150))
            screen.blit(label2, (180, 180))
        if (time-new_time) > 4000:
            return

        fps.tick(60)
        pygame.display.flip()

def _battles_done():
    '''
    If the player does not have enough funds for battle or the lottery, the player will see a message from Prof. Poison Oak.
    It also appears when the player already has all 151 pokemon.
    '''
    new_time = pygame.time.get_ticks()
    label1 = menu_font.render('You already beat all the Pokemon trainers!', 1, blackcolor)
    label2 = menu_font.render('       -Prof. Poison Oak', 1, blackcolor)

    while True:
        events = pygame.event.get()
        for event in events: 
            if event.type == QUIT:
                pygame.quit()
                return
            elif event.type == pygame.KEYDOWN:
                if event.key == pygame.K_KP_ENTER or event.key == pygame.K_RETURN:
                    pass

        
        time = pygame.time.get_ticks()

        if (time-new_time) < 4000:
            screen.fill(whitecolor)
            screen.blit(label1, (180, 150))
            screen.blit(label2, (180, 180))
        else:
            return

        fps.tick(60)
        pygame.display.flip()
        
def _corrector(number:str)->str:
    '''
    If the user uses a keypad to enter his/her ID, this function will correct the string value
    of that key.
    '''
    if number == '[1]':
        return '1'
    if number == '[2]':
        return '2'
    if number == '[3]':
        return '3'
    if number == '[4]':
        return '4'
    if number == '[5]':
        return '5'
    if number == '[6]':
        return '6'
    if number == '[7]':
        return '7'
    if number == '[8]':
        return '8'
    if number == '[9]':
        return '9'
    if number == '[0]':
        return '0'
    if number == 'return':
        return 'enter'
    else:
        return str(number)

def _save_data(UserID):
    '''
    Using the UserID, this function will try to open the save file attached to
    the ID. If the save file does not exist, it will create one and take the
    player to the professor.
    '''
    new_time = pygame.time.get_ticks()
    label1 = font.render('Checking Save Data', 1, whitecolor)
    label2 = font.render('Checking Save Data.', 1, whitecolor)
    label3 = font.render('Checking Save Data..', 1,whitecolor)
    label4 = font.render('Checking Save Data...', 1, whitecolor)
    while True:
        time = pygame.time.get_ticks()
        events = pygame.event.get()
        for event in events: 
            if event.type == QUIT:
                pygame.quit()
                return
        screen.fill(blackcolor)
        if (time-new_time) < 5500:
            screen.blit(label1, (200,100))     
            if (time-new_time) > 1000:
                screen.blit(label2, (200, 100))
            if (time-new_time) > 2500:
                screen.blit(label3, (200, 100))
            if (time-new_time) > 4500:
                screen.blit(label4, (200,100))

        if (time-new_time) > 7500:
            try:
                file = open(str(UserID)+'.txt', 'r')
                file_data = file.readlines()
                battles = int(file_data[0])
                pokemon = ast.literal_eval(file_data[1])
                shinypokemon =ast.literal_eval(file_data[2])
                games_completed = int(file_data[3])
                coins = int(file_data[4])
                team_pkmn = ast.literal_eval(file_data[5])
                date_time = file_data[6]
                fixer_index = date_time.find('.')
                time_stamp = datetime.datetime.strptime(date_time[:fixer_index], '%Y-%m-%d %H:%M:%S')
                
                if (time-new_time) > 1000:
                    return (battles, pokemon, shinypokemon, team_pkmn, coins, games_completed, time_stamp)
            except:
                file = open(str(UserID)+'.txt', 'w')
                pokemon, shinypokemon, team_pkmn = _professor()
                file.write('0\n')
                file.write(str(pokemon)+'\n')
                file.write(str(shinypokemon)+'\n')
                file.write('0\n')
                file.write('500\n')
                file.write(str(team_pkmn)+'\n')
                time_stamp = datetime.datetime.now()
                file.write('{}\n'.format(time_stamp))
                file.close()

                return (0, pokemon, shinypokemon, team_pkmn, 500, 0, time_stamp)

                
                
                
        fps.tick(60)
        pygame.display.flip()

def _update_coins(data, coins):
    '''
    This function will take the data provided from the server
    and increase the player's coins accordingly.
    '''
    gain = 0
    item_sound.play()
    for each in data:
        if each == 'Low':
            gain += 1
        if each == 'Medium':
            gain += 2
        if each == 'High':
            gain += 3
        if each == 'VeryHigh':
            gain += 5
    label1 = font.render('Your activity has earned you {} coins.'.format(gain), 1, blackcolor)
    label2 = font.render('Total Coins: {}'.format(coins+gain), 1, blackcolor)
    time = pygame.time.get_ticks()
    while True:
        current_time = pygame.time.get_ticks()
        events = pygame.event.get()
        for event in events: 
            if event.type == QUIT:
                pygame.quit()
                return

        screen.fill(whitecolor)

        if (current_time-time) < 5000:
            screen.blit(label1, (120,160))
            screen.blit(label2, (230, 190))
        else:
            return gain

        fps.tick(60)
        pygame.display.flip()

def _line_replacer(line_change, new_entry, UserID):
    '''
    Everytime something affects the player's current progress, this function
    is called and records the data into the save file attached to the UserID.
    '''
    with open(str(UserID)+'.txt', 'r') as file:
        data = file.readlines()

    file.close()
    data[line_change] = str(new_entry)+'\n'

    with open(str(UserID)+'.txt', 'w') as file:
        file.writelines(data)

    file.close()

def _professor():
    '''
    If the player has no save file, the player will experience an introduction to the game.
    This function is the introduction.
    '''
    professor = Intro_Sprite()
    welcome_music.play(-1)
    allsprites = pygame.sprite.RenderPlain(professor)
    label1 = font.render('Welcome to the world of Monsters That Fit in Your Pocket!', 1, blackcolor)
    label2 = font.render('...erm what? Pokemon... never heard of it! Such a silly name!', 1, blackcolor)
    label3 = font.render('Anyways, my name is Prof. Poison Oak. I have studied these', 1, blackcolor)
    label4 = font.render('Monsters for many years, but now I want to try something', 1, blackcolor)
    label5 = font.render('new! (Press the a key to continue through dialogue)', 1, blackcolor)
    label6 = font.render('I want to be an excercise guru! So I have a task for you.', 1, blackcolor)
    label7 = font.render('I need you to do physical activity in your world. In return,', 1, blackcolor)
    label8 = font.render('I will award you coins for my world. You can use coins to buy', 1, blackcolor)
    label9 = font.render('Monsters or conduct battles with your Monsters. I want you', 1, blackcolor)
    label10 = font.render('to beat all 8 Gym Leaders, the Elite Three, and the Champion.', 1, blackcolor)
    label11 = font.render('I also want you to capture all 151 Monsters. Accomplishing', 1, blackcolor)
    label12 = font.render('these tasks will take a lot of physical activity, but I', 1, blackcolor)
    label13 = font.render('know you can do it! Well, to start you off I will give you', 1, blackcolor)
    label14 = font.render('500 coins and your first Pokemon! For more information go to', 1, blackcolor)
    label15 = font.render('Info in the main menu. Good Luck!', 1, blackcolor)
    n = 0
    while True:
        for event in pygame.event.get(): 
            if event.type == QUIT:
                pygame.quit()
                return
            elif event.type == pygame.KEYDOWN:
                if event.key == pygame.K_a:
                    menu_sound.play()
                    n += 1
        screen.fill(whitecolor)
        if n == 0:
            screen.blit(label1, (10, 310))
            screen.blit(label2, (10, 340))
            screen.blit(label3, (10, 370))
            screen.blit(label4, (10, 400))
            screen.blit(label5, (10, 430))
        if n == 1:
            screen.blit(label6, (10, 310))
            screen.blit(label7, (10, 340))
            screen.blit(label8, (10, 370))
            screen.blit(label9, (10, 400))
            screen.blit(label10, (10, 430))
        if n == 2:
            screen.blit(label11, (10, 310))
            screen.blit(label12, (10, 340))
            screen.blit(label13, (10, 370))
            screen.blit(label14, (10, 400))
            screen.blit(label15, (10, 430))
        if n == 3:
            welcome_music.stop()
            return _lotto([],[],[],0, 'intro')
        pygame.draw.rect(screen, blackcolor, (0, 300, 640, 180), 2)
        
        allsprites.draw(screen)

        fps.tick(60)
        pygame.display.flip()

def _PC(pokemon, shinypokemon, team_pkmn, UserID):
    '''
    This option allows the player to look ino their teams or storage.
    '''
    label1 = font.render('Press the b key to go back', 1, blackcolor)
    label2 = font.render('Team', 1, blackcolor)
    label3 = font.render('Storage', 1, blackcolor)

    n = 0
    x1 = 145
    x2 = 170
    x3 = 157.5

    y1 = 155
    y2 = 175
    while True:
        for event in pygame.event.get(): 
            if event.type == QUIT:
                pygame.quit()
                return
            elif event.type == pygame.KEYDOWN:
                if event.key == pygame.K_KP_ENTER or event.key == pygame.K_RETURN:
                        menu_sound.play()
                        if x1 == 470 and x2 == 495 and x3 == 482.5:
                            _storage(team_pkmn, pokemon, shinypokemon)
                            _line_replacer(5, team_pkmn, UserID)
                        if x1 == 145 and x2 == 170 and x3 == 157.5:
                            _team(team_pkmn, shinypokemon)
                            _line_replacer(5, team_pkmn, UserID)
                            
                if event.key == pygame.K_d and x1 != 470 and x2 != 495 and x3 != 482.5:
                        menu_sound.play()
                        x1 = 470
                        x2 = 495
                        x3 = 482.5
                if event.key == pygame.K_a and x1 != 145 and x2 != 170 and x3 != 157.5:
                    menu_sound.play()
                    x1 = 145
                    x2 = 170
                    x3 = 157.5
                if event.key == pygame.K_b:
                    return
        screen.fill(whitecolor)
        screen.blit(label1, (200, 30))
        screen.blit(label2, (130, 240))
        screen.blit(label3, (440, 240))
        pygame.draw.polygon(screen, blackcolor,((x1,y1),(x2,y1), (x3,y2)))
        pygame.draw.rect(screen, blackcolor, [80, 180, 150, 150], 2)
        pygame.draw.rect(screen, blackcolor, [400, 180, 150, 150], 2)
        
        fps.tick(60)
        pygame.display.flip()

def _team(team_pkmn, shinypokemon):
    '''
    Here the player can see his/her team make deposits.
    '''
    label0 = font.render('Team:', 1, blackcolor)
    label01 = font.render('Press b to return.', 1, blackcolor)
    label02 = font.render('Or Enter the # of the pokemon', 1, blackcolor)
    label04 = font.render('Press "enter" to send your', 1, blackcolor)
    label05 = font.render('request. At the least, you', 1, blackcolor)
    label06 = font.render('can have 1 pokemon.', 1, blackcolor)
    number = ''
    while True:
        
        label03 = font.render('you wish to deposit: {}'.format(number), 1, blackcolor)
        if team_pkmn[0] not in shinypokemon:
            label1 = font.render('{}-{}'.format(team_pkmn[0], pokemon_info._get_name(team_pkmn[0])), 1, blackcolor)
        else:
            label1 = font.render('{}-{}'.format(team_pkmn[0], pokemon_info._get_name(team_pkmn[0])), 1, bluecolor)
        if len(team_pkmn) >= 2:
            if team_pkmn[1] not in shinypokemon:
                label2 = font.render('{}-{}'.format(team_pkmn[1], pokemon_info._get_name(team_pkmn[1])), 1, blackcolor)
            else:
                label2 = font.render('{}-{}'.format(team_pkmn[1], pokemon_info._get_name(team_pkmn[1])), 1, bluecolor)
        if len(team_pkmn) >= 3:
            if team_pkmn[2] not in shinypokemon:
                label3 = font.render('{}-{}'.format(team_pkmn[2], pokemon_info._get_name(team_pkmn[2])), 1, blackcolor)
            else:
                label3 = font.render('{}-{}'.format(team_pkmn[2], pokemon_info._get_name(team_pkmn[2])), 1, bluecolor)
        if len(team_pkmn) >= 4:
            if team_pkmn[3] not in shinypokemon:
                label4 = font.render('{}-{}'.format(team_pkmn[3], pokemon_info._get_name(team_pkmn[3])), 1, blackcolor)
            else:
                label4 = font.render('{}-{}'.format(team_pkmn[3], pokemon_info._get_name(team_pkmn[3])), 1, bluecolor)
        if len(team_pkmn) >= 5:
            if team_pkmn[4] not in shinypokemon:
                label5 = font.render('{}-{}'.format(team_pkmn[4], pokemon_info._get_name(team_pkmn[4])), 1, blackcolor)
            else:
                label5 = font.render('{}-{}'.format(team_pkmn[4], pokemon_info._get_name(team_pkmn[4])), 1, bluecolor)
        if len(team_pkmn) >= 6:
            if team_pkmn[5] not in shinypokemon:
                label6 = font.render('{}-{}'.format(team_pkmn[5], pokemon_info._get_name(team_pkmn[5])), 1, blackcolor)
            else:
                label6 = font.render('{}-{}'.format(team_pkmn[5], pokemon_info._get_name(team_pkmn[5])), 1, bluecolor)
        for event in pygame.event.get(): 
            if event.type == QUIT:
                pygame.quit()
                return
            elif event.type == pygame.KEYDOWN:
                if event.key == pygame.K_b:
                    return
                else:
                    key = pygame.key.name(event.key)
                    key = _corrector(key)
                    if key == 'enter':
                        try:
                            if len(team_pkmn) > 1:
                                team_pkmn.remove(int(number))
                                number = ''
                            else:
                                number = ''
                        except:
                            number = ''
                            pass
                    if key == 'backspace':
                        number = number[:-1]

                    if key != 'enter' and key != 'backspace':
                        number = number+key
        screen.fill(whitecolor)
        screen.blit(label0, (0,0))
        screen.blit(label1, (10, 30))
        screen.blit(label01, (320,0))
        screen.blit(label02, (320, 30))
        screen.blit(label03, (320, 60))
        screen.blit(label04, (320, 90))
        screen.blit(label05, (320, 120))
        screen.blit(label06, (320, 150))
        if len(team_pkmn) >= 2:
            screen.blit(label2, (10, 90))
        if len(team_pkmn) >= 3:
            screen.blit(label3, (10, 150))
        if len(team_pkmn) >= 4:
            screen.blit(label4, (10, 210))
        if len(team_pkmn) >= 5:
            screen.blit(label5, (10, 270))
        if len(team_pkmn) >= 6:
            screen.blit(label6, (10, 330))
                                            

        
        fps.tick(60)
        pygame.display.flip()
    
        

            
def _storage(team_pkmn, pokemon, shinypokemon):
    '''
    Here the player can see the Pokemon stored in their storage and make withdraws.
    '''
    number = ''
    n = 0
    label01 = font.render('Press b to return.', 1, blackcolor)
    label02 = font.render('Or Enter the # of the pokemon', 1, blackcolor)
    label04 = font.render('Press "enter" to send your', 1, blackcolor)
    label05 = font.render('request.', 1, blackcolor)
    label02a = font.render('Your party is full.', 1, blackcolor)
    label03a = font.render('You cannot withdraw.', 1, blackcolor)

    
    
    while True:
        label0 = font.render('Storage(box {}):'.format(n+1), 1, blackcolor)
        label03 = font.render('you wish to withdraw: {}'.format(number), 1, blackcolor)
        for event in pygame.event.get(): 
            if event.type == QUIT:
                pygame.quit()
                return
            elif event.type == pygame.KEYDOWN:
                if event.key == pygame.K_b:
                    return
                else:
                    key = pygame.key.name(event.key)
                    key = _corrector(key)
                    if key == 'enter' and len(team_pkmn) < 6 :
                        try:
                            if int(number) in pokemon:
                                team_pkmn.append(int(number))
                                number = ''
                            else:
                                number = ''
                        except:
                            number = ''
                            pass
                    if key == 'backspace':
                        number = number[:-1]

                    if key != 'enter' and key != 'backspace'and key != 'd' and key != 'a':
                        number = number+key
                    if key == 'd' and n != 10:
                        menu_sound.play()
                        n += 1
                    if key == 'a' and n != 0:
                        menu_sound.play()
                        n -= 1
        screen.fill(whitecolor)
        screen.blit(label0, (0, 0))
        screen.blit(label01, (320,0))
        if len(team_pkmn) >= 6:
            screen.blit(label02a, (320, 30))
            screen.blit(label03a,(320,60))
            
        if len(team_pkmn) < 6:
            screen.blit(label02, (320, 30))
            screen.blit(label03, (320, 60))
            screen.blit(label04, (320, 90))
            screen.blit(label05, (320, 120))
        if n == 0:
            if 1 in pokemon and 1 not in team_pkmn:
                if 1 in shinypokemon:
                    label1 = menu_font.render('1-Bulbasaur', 1, bluecolor)
                else:
                    label1 = menu_font.render('1-Bulbasaur', 1, blackcolor)
                screen.blit(label1, (10, 30))
            if 2 in pokemon and 2 not in team_pkmn:
                if 2 in shinypokemon:
                        label2 = menu_font.render('2-Ivysaur', 1, bluecolor)
                else:
                        label2 = menu_font.render('2-Ivysaur', 1, blackcolor)
                screen.blit(label2, (10, 60))
            if 3 in pokemon and 3 not in team_pkmn:
                if 3 in shinypokemon:
                        label3 = menu_font.render('3-Venusaur', 1, bluecolor)
                else:
                        label3 = menu_font.render('3-Venusaur', 1, blackcolor)
                screen.blit(label3, (10, 90))
            if 4 in pokemon and 4 not in team_pkmn:
                if 4 in shinypokemon:
                        label4 = menu_font.render('4-Charmander', 1, bluecolor)
                else:
                        label4 = menu_font.render('4-Charmander', 1, blackcolor)
                screen.blit(label4, (10, 120))
            if 5 in pokemon and 5 not in team_pkmn:
                if 5 in shinypokemon:
                        label5 = menu_font.render('5-Charmeleon', 1, bluecolor)
                else:
                        label5 = menu_font.render('5-Charmeleon', 1, blackcolor)
                screen.blit(label5, (10, 150))
            if 6 in pokemon and 6 not in team_pkmn:
                if 6 in shinypokemon:
                        label6 = menu_font.render('6-Charizard', 1, bluecolor)
                else:
                        label6 = menu_font.render('6-Charizard', 1, blackcolor)
                screen.blit(label6, (10, 180))
            if 7 in pokemon and 7 not in team_pkmn:
                if 7 in shinypokemon:
                        label7 = menu_font.render('7-Squirtle', 1, bluecolor)
                else:
                        label7 = menu_font.render('7-Squirtle', 1, blackcolor)
                screen.blit(label7, (10, 210))
            if 8 in pokemon and 8 not in team_pkmn:
                if 8 in shinypokemon:
                        label8 = menu_font.render('8-Wartortle', 1, bluecolor)
                else:
                        label8 = menu_font.render('8-Wartortle', 1, blackcolor)
                screen.blit(label8, (10, 240))
            if 9 in pokemon and 9 not in team_pkmn:
                if 9 in shinypokemon:
                        label9 = menu_font.render('9-Blastoise', 1, bluecolor)
                else:
                        label9 = menu_font.render('9-Blastoise', 1, blackcolor)
                screen.blit(label9, (10, 270))
            if 10 in pokemon and 10 not in team_pkmn:
                if 10 in shinypokemon:
                        label10 = menu_font.render('10-Caterpie', 1, bluecolor)
                else:
                        label10 = menu_font.render('10-Caterpie', 1, blackcolor)
                screen.blit(label10, (10, 300))
            if 11 in pokemon and 11 not in team_pkmn:
                if 11 in shinypokemon:
                        label11 = menu_font.render('11-Metapod', 1, bluecolor)
                else:
                        label11 = menu_font.render('11-Metapod', 1, blackcolor)
                screen.blit(label11, (10, 330))
            if 12 in pokemon and 12 not in team_pkmn:
                if 12 in shinypokemon:
                        label12 = menu_font.render('12-Butterfree', 1, bluecolor)
                else:
                        label12 = menu_font.render('12-Butterfree', 1, blackcolor)
                screen.blit(label12, (10, 360))
            if 13 in pokemon and 13 not in team_pkmn:
                if 13 in shinypokemon:
                        label13 = menu_font.render('13-Weedle', 1, bluecolor)
                else:
                        label13 = menu_font.render('13-Weedle', 1, blackcolor)
                screen.blit(label13, (10, 390))
            if 14 in pokemon and 14 not in team_pkmn:
                if 14 in shinypokemon:
                        label14 = menu_font.render('14-Kakuna', 1, bluecolor)
                else:
                        label14 = menu_font.render('14-Kakuna', 1, blackcolor)
                screen.blit(label14, (10, 420))
            if 15 in pokemon and 15 not in team_pkmn:
                if 15 in shinypokemon:
                        label15 = menu_font.render('15-Beedrill', 1, bluecolor)
                else:
                        label15 = menu_font.render('15-Beedrill', 1, blackcolor)
                screen.blit(label15, (10, 450))
        if n == 1:
            if 16 in pokemon and 16 not in team_pkmn:
                if 16 in shinypokemon:
                        label16 = menu_font.render('16-Pidgey', 1, bluecolor)
                else:
                        label16 = menu_font.render('16-Pidgey', 1, blackcolor)
                screen.blit(label16, (10, 30))
            if 17 in pokemon and 17 not in team_pkmn:
                if 17 in shinypokemon:
                        label17 = menu_font.render('17-Pidgeotto', 1, bluecolor)
                else:
                        label17 = menu_font.render('17-Pidgeotto', 1, blackcolor)
                screen.blit(label17, (10, 60))
            if 18 in pokemon and 18 not in team_pkmn:
                if 18 in shinypokemon:
                        label18 = menu_font.render('18-Pidgeot', 1, bluecolor)
                else:
                        label18 = menu_font.render('18-Pidgeot', 1, blackcolor)
                screen.blit(label18, (10, 90))
            if 19 in pokemon and 19 not in team_pkmn:
                if 19 in shinypokemon:
                        label19 = menu_font.render('19-Rattata', 1, bluecolor)
                else:
                        label19 = menu_font.render('19-Rattata', 1, blackcolor)
                screen.blit(label19, (10, 120))
            if 20 in pokemon and 20 not in team_pkmn:
                if 20 in shinypokemon:
                        label20 = menu_font.render('20-Raticate', 1, bluecolor)
                else:
                        label20 = menu_font.render('20-Raticate', 1, blackcolor)
                screen.blit(label20, (10, 150))
            if 21 in pokemon and 21 not in team_pkmn:
                if 21 in shinypokemon:
                        label21 = menu_font.render('21-Spearow', 1, bluecolor)
                else:
                        label21 = menu_font.render('21-Spearow', 1, blackcolor)
                screen.blit(label21, (10, 180))
            if 22 in pokemon and 22 not in team_pkmn:
                if 22 in shinypokemon:
                        label22 = menu_font.render('22-Fearow', 1, bluecolor)
                else:
                        label22 = menu_font.render('22-Fearow', 1, blackcolor)
                screen.blit(label22, (10, 210))
            if 23 in pokemon and 23 not in team_pkmn:
                if 23 in shinypokemon:
                        label23 = menu_font.render('23-Ekans', 1, bluecolor)
                else:
                        label23 = menu_font.render('23-Ekans', 1, blackcolor)
                screen.blit(label23, (10, 240))
            if 24 in pokemon and 24 not in team_pkmn:
                if 24 in shinypokemon:
                        label24 = menu_font.render('24-Arbok', 1, bluecolor)
                else:
                        label24 = menu_font.render('24-Arbok', 1, blackcolor)
                screen.blit(label24, (10, 270))
            if 25 in pokemon and 25 not in team_pkmn:
                if 25 in shinypokemon:
                        label25 = menu_font.render('25-Pikachu', 1, bluecolor)
                else:
                        label25 = menu_font.render('25-Pikachu', 1, blackcolor)
                screen.blit(label25, (10, 300))
            if 26 in pokemon and 26 not in team_pkmn:
                if 26 in shinypokemon:
                        label26 = menu_font.render('26-Raichu', 1, bluecolor)
                else:
                        label26 = menu_font.render('26-Raichu', 1, blackcolor)
                screen.blit(label26, (10, 330))
            if 27 in pokemon and 27 not in team_pkmn:
                if 27 in shinypokemon:
                        label27 = menu_font.render('27-Sandshrew', 1, bluecolor)
                else:
                        label27 = menu_font.render('27-Sandshrew', 1, blackcolor)
                screen.blit(label27, (10, 360))
            if 28 in pokemon and 28 not in team_pkmn:
                if 28 in shinypokemon:
                        label28 = menu_font.render('28-Sandslash', 1, bluecolor)
                else:
                        label28 = menu_font.render('28-Sandslash', 1, blackcolor)
                screen.blit(label28, (10, 390))
            if 29 in pokemon and 29 not in team_pkmn:
                if 29 in shinypokemon:
                        label29 = menu_font.render('29-NidoranF', 1, bluecolor)
                else:
                        label29 = menu_font.render('29-NidoranF', 1, blackcolor)
                screen.blit(label29, (10, 420))
            if 30 in pokemon and 30 not in team_pkmn:
                if 30 in shinypokemon:
                        label30 = menu_font.render('30-Nidorina', 1, bluecolor)
                else:
                        label30 = menu_font.render('30-Nidorina', 1, blackcolor)
                screen.blit(label30, (10, 450))
        if n == 2:
            if 31 in pokemon and 31 not in team_pkmn:
                if 31 in shinypokemon:
                        label31 = menu_font.render('31-Nidoqueen', 1, bluecolor)
                else:
                        label31 = menu_font.render('31-Nidoqueen', 1, blackcolor)
                screen.blit(label31, (10, 30))
            if 32 in pokemon and 32 not in team_pkmn:
                if 32 in shinypokemon:
                        label32 = menu_font.render('32-NidoranM', 1, bluecolor)
                else:
                        label32 = menu_font.render('32-NidoranM', 1, blackcolor)
                screen.blit(label32, (10, 60))
            if 33 in pokemon and 33 not in team_pkmn:
                if 33 in shinypokemon:
                        label33 = menu_font.render('33-Nidorino', 1, bluecolor)
                else:
                        label33 = menu_font.render('33-Nidorino', 1, blackcolor)
                screen.blit(label33, (10, 90))
            if 34 in pokemon and 34 not in team_pkmn:
                if 34 in shinypokemon:
                        label34 = menu_font.render('34-Nidoking', 1, bluecolor)
                else:
                        label34 = menu_font.render('34-Nidoking', 1, blackcolor)
                screen.blit(label34, (10, 120))
            if 35 in pokemon and 35 not in team_pkmn:
                if 35 in shinypokemon:
                        label35 = menu_font.render('35-Clefairy', 1, bluecolor)
                else:
                        label35 = menu_font.render('35-Clefairy', 1, blackcolor)
                screen.blit(label35, (10, 150))
            if 36 in pokemon and 36 not in team_pkmn:
                if 36 in shinypokemon:
                        label36 = menu_font.render('36-Clefable', 1, bluecolor)
                else:
                        label36 = menu_font.render('36-Clefable', 1, blackcolor)
                screen.blit(label36, (10, 180))
            if 37 in pokemon and 37 not in team_pkmn:
                if 37 in shinypokemon:
                        label37 = menu_font.render('37-Vulpix', 1, bluecolor)
                else:
                        label37 = menu_font.render('37-Vulpix', 1, blackcolor)
                screen.blit(label37, (10, 210))
            if 38 in pokemon and 38 not in team_pkmn:
                if 38 in shinypokemon:
                        label38 = menu_font.render('38-Ninetales', 1, bluecolor)
                else:
                        label38 = menu_font.render('38-Ninetales', 1, blackcolor)
                screen.blit(label38, (10, 240))
            if 39 in pokemon and 39 not in team_pkmn:
                if 39 in shinypokemon:
                        label39 = menu_font.render('39-Jigglypuff', 1, bluecolor)
                else:
                        label39 = menu_font.render('39-Jigglypuff', 1, blackcolor)
                screen.blit(label39, (10, 270))
            if 40 in pokemon and 40 not in team_pkmn:
                if 40 in shinypokemon:
                        label40 = menu_font.render('40-Wigglytuff', 1, bluecolor)
                else:
                        label40 = menu_font.render('40-Wigglytuff', 1, blackcolor)
                screen.blit(label40, (10, 300))
            if 41 in pokemon and 41 not in team_pkmn:
                if 41 in shinypokemon:
                        label41 = menu_font.render('41-Zubat', 1, bluecolor)
                else:
                        label41 = menu_font.render('41-Zubat', 1, blackcolor)
                screen.blit(label41, (10, 330))
            if 42 in pokemon and 42 not in team_pkmn:
                if 42 in shinypokemon:
                        label42 = menu_font.render('42-Golbat', 1, bluecolor)
                else:
                        label42 = menu_font.render('42-Golbat', 1, blackcolor)
                screen.blit(label42, (10, 360))
            if 43 in pokemon and 43 not in team_pkmn:
                if 43 in shinypokemon:
                        label43 = menu_font.render('43-Oddish', 1, bluecolor)
                else:
                        label43 = menu_font.render('43-Oddish', 1, blackcolor)
                screen.blit(label43, (10, 390))
            if 44 in pokemon and 44 not in team_pkmn:
                if 44 in shinypokemon:
                        label44 = menu_font.render('44-Gloom', 1, bluecolor)
                else:
                        label44 = menu_font.render('44-Gloom', 1, blackcolor)
                screen.blit(label44, (10, 420))
            if 45 in pokemon and 45 not in team_pkmn:
                if 45 in shinypokemon:
                        label45 = menu_font.render('45-Vileplume', 1, bluecolor)
                else:
                        label45 = menu_font.render('45-Vileplume', 1, blackcolor)
                screen.blit(label45, (10, 450))
        if n == 3:
            if 46 in pokemon and 46 not in team_pkmn:
                if 46 in shinypokemon:
                        label46 = menu_font.render('46-Paras', 1, bluecolor)
                else:
                        label46 = menu_font.render('46-Paras', 1, blackcolor)
                screen.blit(label46, (10, 30))
            if 47 in pokemon and 47 not in team_pkmn:
                if 47 in shinypokemon:
                        label47 = menu_font.render('47-Parasect', 1, bluecolor)
                else:
                        label47 = menu_font.render('47-Parasect', 1, blackcolor)
                screen.blit(label47, (10, 60))
            if 48 in pokemon and 48 not in team_pkmn:
                if 48 in shinypokemon:
                        label48 = menu_font.render('48-Venonat', 1, bluecolor)
                else:
                        label48 = menu_font.render('48-Venonat', 1, blackcolor)
                screen.blit(label48, (10, 90))
            if 49 in pokemon and 49 not in team_pkmn:
                if 49 in shinypokemon:
                        label49 = menu_font.render('49-Venomoth', 1, bluecolor)
                else:
                        label49 = menu_font.render('49-Venomoth', 1, blackcolor)
                screen.blit(label49, (10, 120))
            if 50 in pokemon and 50 not in team_pkmn:
                if 50 in shinypokemon:
                        label50 = menu_font.render('50-Diglett', 1, bluecolor)
                else:
                        label50 = menu_font.render('50-Diglett', 1, blackcolor)
                screen.blit(label50, (10, 150))
            if 51 in pokemon and 51 not in team_pkmn:
                if 51 in shinypokemon:
                        label51 = menu_font.render('51-Dugtrio', 1, bluecolor)
                else:
                        label51 = menu_font.render('51-Dugtrio', 1, blackcolor)
                screen.blit(label51, (10, 180))
            if 52 in pokemon and 52 not in team_pkmn:
                if 52 in shinypokemon:
                        label52 = menu_font.render('52-Meowth', 1, bluecolor)
                else:
                        label52 = menu_font.render('52-Meowth', 1, blackcolor)
                screen.blit(label52, (10, 210))
            if 53 in pokemon and 53 not in team_pkmn:
                if 53 in shinypokemon:
                        label53 = menu_font.render('53-Persian', 1, bluecolor)
                else:
                        label53 = menu_font.render('53-Persian', 1, blackcolor)
                screen.blit(label53, (10, 240))
            if 54 in pokemon and 54 not in team_pkmn:
                if 54 in shinypokemon:
                        label54 = menu_font.render('54-Psyduck', 1, bluecolor)
                else:
                        label54 = menu_font.render('54-Psyduck', 1, blackcolor)
                screen.blit(label54, (10, 270))
            if 55 in pokemon and 55 not in team_pkmn:
                if 55 in shinypokemon:
                        label55 = menu_font.render('55-Golduck', 1, bluecolor)
                else:
                        label55 = menu_font.render('55-Golduck', 1, blackcolor)
                screen.blit(label55, (10, 300))
            if 56 in pokemon and 56 not in team_pkmn:
                if 56 in shinypokemon:
                        label56 = menu_font.render('56-Mankey', 1, bluecolor)
                else:
                        label56 = menu_font.render('56-Mankey', 1, blackcolor)
                screen.blit(label56, (10, 330))
            if 57 in pokemon and 57 not in team_pkmn:
                if 57 in shinypokemon:
                        label57 = menu_font.render('57-Primeape', 1, bluecolor)
                else:
                        label57 = menu_font.render('57-Primeape', 1, blackcolor)
                screen.blit(label57, (10, 360))
            if 58 in pokemon and 58 not in team_pkmn:
                if 58 in shinypokemon:
                        label58 = menu_font.render('58-Growlithe', 1, bluecolor)
                else:
                        label58 = menu_font.render('58-Growlithe', 1, blackcolor)
                screen.blit(label58, (10, 390))
            if 59 in pokemon and 59 not in team_pkmn:
                if 59 in shinypokemon:
                        label59 = menu_font.render('59-Arcanine', 1, bluecolor)
                else:
                        label59 = menu_font.render('59-Arcanine', 1, blackcolor)
                screen.blit(label59, (10, 420))
            if 60 in pokemon and 60 not in team_pkmn:
                if 60 in shinypokemon:
                        label60 = menu_font.render('60-Poliwag', 1, bluecolor)
                else:
                        label60 = menu_font.render('60-Poliwag', 1, blackcolor)
                screen.blit(label60, (10, 450))
        if n == 4:
            if 61 in pokemon and 61 not in team_pkmn:
                if 61 in shinypokemon:
                        label61 = menu_font.render('61-Poliwhirl', 1, bluecolor)
                else:
                        label61 = menu_font.render('61-Poliwhirl', 1, blackcolor)
                screen.blit(label61, (10, 30))
            if 62 in pokemon and 62 not in team_pkmn:
                if 62 in shinypokemon:
                        label62 = menu_font.render('62-Poliwrath', 1, bluecolor)
                else:
                        label62 = menu_font.render('62-Poliwrath', 1, blackcolor)
                screen.blit(label62, (10, 60))
            if 63 in pokemon and 63 not in team_pkmn:
                if 63 in shinypokemon:
                        label63 = menu_font.render('63-Abra', 1, bluecolor)
                else:
                        label63 = menu_font.render('63-Abra', 1, blackcolor)
                screen.blit(label63, (10, 90))
            if 64 in pokemon and 64 not in team_pkmn:
                if 64 in shinypokemon:
                        label64 = menu_font.render('64-Kadabra', 1, bluecolor)
                else:
                        label64 = menu_font.render('64-Kadabra', 1, blackcolor)
                screen.blit(label64, (10, 120))
            if 65 in pokemon and 65 not in team_pkmn:
                if 65 in shinypokemon:
                        label65 = menu_font.render('65-Alakazam', 1, bluecolor)
                else:
                        label65 = menu_font.render('65-Alakazam', 1, blackcolor)
                screen.blit(label65, (10, 150))
            if 66 in pokemon and 66 not in team_pkmn:
                if 66 in shinypokemon:
                        label66 = menu_font.render('66-Machop', 1, bluecolor)
                else:
                        label66 = menu_font.render('66-Machop', 1, blackcolor)
                screen.blit(label66, (10, 180))
            if 67 in pokemon and 67 not in team_pkmn:
                if 67 in shinypokemon:
                        label67 = menu_font.render('67-Machoke', 1, bluecolor)
                else:
                        label67 = menu_font.render('67-Machoke', 1, blackcolor)
                screen.blit(label67, (10, 210))
            if 68 in pokemon and 68 not in team_pkmn:
                if 68 in shinypokemon:
                        label68 = menu_font.render('68-Machamp', 1, bluecolor)
                else:
                        label68 = menu_font.render('68-Machamp', 1, blackcolor)
                screen.blit(label68, (10, 240))
            if 69 in pokemon and 69 not in team_pkmn:
                if 69 in shinypokemon:
                        label69 = menu_font.render('69-Bellsprout', 1, bluecolor)
                else:
                        label69 = menu_font.render('69-Bellsprout', 1, blackcolor)
                screen.blit(label69, (10, 270))
            if 70 in pokemon and 70 not in team_pkmn:
                if 70 in shinypokemon:
                        label70 = menu_font.render('70-Weepinbell', 1, bluecolor)
                else:
                        label70 = menu_font.render('70-Weepinbell', 1, blackcolor)
                screen.blit(label70, (10, 300))
            if 71 in pokemon and 71 not in team_pkmn:
                if 71 in shinypokemon:
                        label71 = menu_font.render('71-Victreebel', 1, bluecolor)
                else:
                        label71 = menu_font.render('71-Victreebel', 1, blackcolor)
                screen.blit(label71, (10, 330))
            if 72 in pokemon and 72 not in team_pkmn:
                if 72 in shinypokemon:
                        label72 = menu_font.render('72-Tentacool', 1, bluecolor)
                else:
                        label72 = menu_font.render('72-Tentacool', 1, blackcolor)
                screen.blit(label72, (10, 360))
            if 73 in pokemon and 73 not in team_pkmn:
                if 73 in shinypokemon:
                        label73 = menu_font.render('73-Tentacruel', 1, bluecolor)
                else:
                        label73 = menu_font.render('73-Tentacruel', 1, blackcolor)
                screen.blit(label73, (10, 390))
            if 74 in pokemon and 74 not in team_pkmn:
                if 74 in shinypokemon:
                        label74 = menu_font.render('74-Geodude', 1, bluecolor)
                else:
                        label74 = menu_font.render('74-Geodude', 1, blackcolor)
                screen.blit(label74, (10, 420))
            if 75 in pokemon and 75 not in team_pkmn:
                if 75 in shinypokemon:
                        label75 = menu_font.render('75-Graveler', 1, bluecolor)
                else:
                        label75 = menu_font.render('75-Graveler', 1, blackcolor)
                screen.blit(label75, (10, 450))
        if n == 5:
            if 76 in pokemon and 76 not in team_pkmn:
                if 76 in shinypokemon:
                        label76 = menu_font.render('76-Golem', 1, bluecolor)
                else:
                        label76 = menu_font.render('76-Golem', 1, blackcolor)
                screen.blit(label76, (10, 30))
            if 77 in pokemon and 77 not in team_pkmn:
                if 77 in shinypokemon:
                        label77 = menu_font.render('77-Ponyta', 1, bluecolor)
                else:
                        label77 = menu_font.render('77-Ponyta', 1, blackcolor)
                screen.blit(label77, (10, 60))
            if 78 in pokemon and 78 not in team_pkmn:
                if 78 in shinypokemon:
                        label78 = menu_font.render('78-Rapidash', 1, bluecolor)
                else:
                        label78 = menu_font.render('78-Rapidash', 1, blackcolor)
                screen.blit(label78, (10, 90))
            if 79 in pokemon and 79 not in team_pkmn:
                if 79 in shinypokemon:
                        label79 = menu_font.render('79-Slowpoke', 1, bluecolor)
                else:
                        label79 = menu_font.render('79-Slowpoke', 1, blackcolor)
                screen.blit(label79, (10, 120))
            if 80 in pokemon and 80 not in team_pkmn:
                if 80 in shinypokemon:
                        label80 = menu_font.render('80-Slowbro', 1, bluecolor)
                else:
                        label80 = menu_font.render('80-Slowbro', 1, blackcolor)
                screen.blit(label80, (10, 150))
            if 81 in pokemon and 81 not in team_pkmn:
                if 81 in shinypokemon:
                        label81 = menu_font.render('81-Magnemite', 1, bluecolor)
                else:
                        label81 = menu_font.render('81-Magnemite', 1, blackcolor)
                screen.blit(label81, (10, 180))
            if 82 in pokemon and 82 not in team_pkmn:
                if 82 in shinypokemon:
                        label82 = menu_font.render('82-Magneton', 1, bluecolor)
                else:
                        label82 = menu_font.render('82-Magneton', 1, blackcolor)
                screen.blit(label82, (10, 210))
            if 83 in pokemon and 83 not in team_pkmn:
                if 83 in shinypokemon:
                        label83 = menu_font.render('83-FarfetchD', 1, bluecolor)
                else:
                        label83 = menu_font.render('83-FarfetchD', 1, blackcolor)
                screen.blit(label83, (10, 240))
            if 84 in pokemon and 84 not in team_pkmn:
                if 84 in shinypokemon:
                        label84 = menu_font.render('84-Doduo', 1, bluecolor)
                else:
                        label84 = menu_font.render('84-Doduo', 1, blackcolor)
                screen.blit(label84, (10, 270))
            if 85 in pokemon and 85 not in team_pkmn:
                if 85 in shinypokemon:
                        label85 = menu_font.render('85-Dodrio', 1, bluecolor)
                else:
                        label85 = menu_font.render('85-Dodrio', 1, blackcolor)
                screen.blit(label85, (10, 300))
            if 86 in pokemon and 86 not in team_pkmn:
                if 86 in shinypokemon:
                        label86 = menu_font.render('86-Seel', 1, bluecolor)
                else:
                        label86 = menu_font.render('86-Seel', 1, blackcolor)
                screen.blit(label86, (10, 330))
            if 87 in pokemon and 87 not in team_pkmn:
                if 87 in shinypokemon:
                        label87 = menu_font.render('87-Dewgong', 1, bluecolor)
                else:
                        label87 = menu_font.render('87-Dewgong', 1, blackcolor)
                screen.blit(label87, (10, 360))
            if 88 in pokemon and 88 not in team_pkmn:
                if 88 in shinypokemon:
                        label88 = menu_font.render('88-Grimer', 1, bluecolor)
                else:
                        label88 = menu_font.render('88-Grimer', 1, blackcolor)
                screen.blit(label88, (10, 390))
            if 89 in pokemon and 89 not in team_pkmn:
                if 89 in shinypokemon:
                        label89 = menu_font.render('89-Muk', 1, bluecolor)
                else:
                        label89 = menu_font.render('89-Muk', 1, blackcolor)
                screen.blit(label89, (10, 420))
            if 90 in pokemon and 90 not in team_pkmn:
                if 90 in shinypokemon:
                        label90 = menu_font.render('90-Shellder', 1, bluecolor)
                else:
                        label90 = menu_font.render('90-Shellder', 1, blackcolor)
                screen.blit(label90, (10, 450))
        if n == 6:
            if 91 in pokemon and 91 not in team_pkmn:
                if 91 in shinypokemon:
                        label91 = menu_font.render('91-Cloyster', 1, bluecolor)
                else:
                        label91 = menu_font.render('91-Cloyster', 1, blackcolor)
                screen.blit(label91, (10, 30))
            if 92 in pokemon and 92 not in team_pkmn:
                if 92 in shinypokemon:
                        label92 = menu_font.render('92-Gastly', 1, bluecolor)
                else:
                        label92 = menu_font.render('92-Gastly', 1, blackcolor)
                screen.blit(label92, (10, 60))
            if 93 in pokemon and 93 not in team_pkmn:
                if 93 in shinypokemon:
                        label93 = menu_font.render('93-Haunter', 1, bluecolor)
                else:
                        label93 = menu_font.render('93-Haunter', 1, blackcolor)
                screen.blit(label93, (10, 90))
            if 94 in pokemon and 94 not in team_pkmn:
                if 94 in shinypokemon:
                        label94 = menu_font.render('94-Gengar', 1, bluecolor)
                else:
                        label94 = menu_font.render('94-Gengar', 1, blackcolor)
                screen.blit(label94, (10, 120))
            if 95 in pokemon and 95 not in team_pkmn:
                if 95 in shinypokemon:
                        label95 = menu_font.render('95-Onix', 1, bluecolor)
                else:
                        label95 = menu_font.render('95-Onix', 1, blackcolor)
                screen.blit(label95, (10, 150))
            if 96 in pokemon and 96 not in team_pkmn:
                if 96 in shinypokemon:
                        label96 = menu_font.render('96-Drowzee', 1, bluecolor)
                else:
                        label96 = menu_font.render('96-Drowzee', 1, blackcolor)
                screen.blit(label96, (10, 180))
            if 97 in pokemon and 97 not in team_pkmn:
                if 97 in shinypokemon:
                        label97 = menu_font.render('97-Hypno', 1, bluecolor)
                else:
                        label97 = menu_font.render('97-Hypno', 1, blackcolor)
                screen.blit(label97, (10, 210))
            if 98 in pokemon and 98 not in team_pkmn:
                if 98 in shinypokemon:
                        label98 = menu_font.render('98-Krabby', 1, bluecolor)
                else:
                        label98 = menu_font.render('98-Krabby', 1, blackcolor)
                screen.blit(label98, (10, 240))
            if 99 in pokemon and 99 not in team_pkmn:
                if 99 in shinypokemon:
                        label99 = menu_font.render('99-Kingler', 1, bluecolor)
                else:
                        label99 = menu_font.render('99-Kingler', 1, blackcolor)
                screen.blit(label99, (10, 270))
            if 100 in pokemon and 100 not in team_pkmn:
                if 100 in shinypokemon:
                        label100 = menu_font.render('100-Voltorb', 1, bluecolor)
                else:
                        label100 = menu_font.render('100-Voltorb', 1, blackcolor)
                screen.blit(label100, (10, 300))
            if 101 in pokemon and 101 not in team_pkmn:
                if 101 in shinypokemon:
                        label101 = menu_font.render('101-Electrode', 1, bluecolor)
                else:
                        label101 = menu_font.render('101-Electrode', 1, blackcolor)
                screen.blit(label101, (10, 330))
            if 102 in pokemon and 102 not in team_pkmn:
                if 102 in shinypokemon:
                        label102 = menu_font.render('102-Exeggcute', 1, bluecolor)
                else:
                        label102 = menu_font.render('102-Exeggcute', 1, blackcolor)
                screen.blit(label102, (10, 360))
            if 103 in pokemon and 103 not in team_pkmn:
                if 103 in shinypokemon:
                        label103 = menu_font.render('103-Exeggutor', 1, bluecolor)
                else:
                        label103 = menu_font.render('103-Exeggutor', 1, blackcolor)
                screen.blit(label103, (10, 390))
            if 104 in pokemon and 104 not in team_pkmn:
                if 104 in shinypokemon:
                        label104 = menu_font.render('104-Cubone', 1, bluecolor)
                else:
                        label104 = menu_font.render('104-Cubone', 1, blackcolor)
                screen.blit(label104, (10, 420))
            if 105 in pokemon and 105 not in team_pkmn:
                if 105 in shinypokemon:
                        label105 = menu_font.render('105-Marowak', 1, bluecolor)
                else:
                        label105 = menu_font.render('105-Marowak', 1, blackcolor)
                screen.blit(label105, (10, 450))
        if n == 7:
            if 106 in pokemon and 106 not in team_pkmn:
                if 106 in shinypokemon:
                        label106 = menu_font.render('106-Hitmonlee', 1, bluecolor)
                else:
                        label106 = menu_font.render('106-Hitmonlee', 1, blackcolor)
                screen.blit(label106, (10, 30))
            if 107 in pokemon and 107 not in team_pkmn:
                if 107 in shinypokemon:
                        label107 = menu_font.render('107-Hitmonchan', 1, bluecolor)
                else:
                        label107 = menu_font.render('107-Hitmonchan', 1, blackcolor)
                screen.blit(label107, (10, 60))
            if 108 in pokemon and 108 not in team_pkmn:
                if 108 in shinypokemon:
                        label108 = menu_font.render('108-Lickitung', 1, bluecolor)
                else:
                        label108 = menu_font.render('108-Lickitung', 1, blackcolor)
                screen.blit(label108, (10, 90))
            if 109 in pokemon and 109 not in team_pkmn:
                if 109 in shinypokemon:
                        label109 = menu_font.render('109-Koffing', 1, bluecolor)
                else:
                        label109 = menu_font.render('109-Koffing', 1, blackcolor)
                screen.blit(label109, (10, 120))
            if 110 in pokemon and 110 not in team_pkmn:
                if 110 in shinypokemon:
                        label110 = menu_font.render('110-Weezing', 1, bluecolor)
                else:
                        label110 = menu_font.render('110-Weezing', 1, blackcolor)
                screen.blit(label110, (10, 150))
            if 111 in pokemon and 111 not in team_pkmn:
                if 111 in shinypokemon:
                        label111 = menu_font.render('111-Rhyhorn', 1, bluecolor)
                else:
                        label111 = menu_font.render('111-Rhyhorn', 1, blackcolor)
                screen.blit(label111, (10, 180))
            if 112 in pokemon and 112 not in team_pkmn:
                if 112 in shinypokemon:
                        label112 = menu_font.render('112-Rhydon', 1, bluecolor)
                else:
                        label112 = menu_font.render('112-Rhydon', 1, blackcolor)
                screen.blit(label112, (10, 210))
            if 113 in pokemon and 113 not in team_pkmn:
                if 113 in shinypokemon:
                        label113 = menu_font.render('113-Chansey', 1, bluecolor)
                else:
                        label113 = menu_font.render('113-Chansey', 1, blackcolor)
                screen.blit(label113, (10, 240))
            if 114 in pokemon and 114 not in team_pkmn:
                if 114 in shinypokemon:
                        label114 = menu_font.render('114-Tangela', 1, bluecolor)
                else:
                        label114 = menu_font.render('114-Tangela', 1, blackcolor)
                screen.blit(label114, (10, 270))
            if 115 in pokemon and 115 not in team_pkmn:
                if 115 in shinypokemon:
                        label115 = menu_font.render('115-Kangaskhan', 1, bluecolor)
                else:
                        label115 = menu_font.render('115-Kangaskhan', 1, blackcolor)
                screen.blit(label115, (10, 300))
            if 116 in pokemon and 116 not in team_pkmn:
                if 116 in shinypokemon:
                        label116 = menu_font.render('116-Horsea', 1, bluecolor)
                else:
                        label116 = menu_font.render('116-Horsea', 1, blackcolor)
                screen.blit(label116, (10, 330))
            if 117 in pokemon and 117 not in team_pkmn:
                if 117 in shinypokemon:
                        label117 = menu_font.render('117-Seadra', 1, bluecolor)
                else:
                        label117 = menu_font.render('117-Seadra', 1, blackcolor)
                screen.blit(label117, (10, 360))
            if 118 in pokemon and 118 not in team_pkmn:
                if 118 in shinypokemon:
                        label118 = menu_font.render('118-Goldeen', 1, bluecolor)
                else:
                        label118 = menu_font.render('118-Goldeen', 1, blackcolor)
                screen.blit(label118, (10, 390))
            if 119 in pokemon and 119 not in team_pkmn:
                if 119 in shinypokemon:
                        label119 = menu_font.render('119-Seaking', 1, bluecolor)
                else:
                        label119 = menu_font.render('119-Seaking', 1, blackcolor)
                screen.blit(label119, (10, 420))
            if 120 in pokemon and 120 not in team_pkmn:
                if 120 in shinypokemon:
                        label120 = menu_font.render('120-Staryu', 1, bluecolor)
                else:
                        label120 = menu_font.render('120-Staryu', 1, blackcolor)
                screen.blit(label120, (10, 450))
        if n == 8:
            if 121 in pokemon and 121 not in team_pkmn:
                if 121 in shinypokemon:
                        label121 = menu_font.render('121-Starmie', 1, bluecolor)
                else:
                        label121 = menu_font.render('121-Starmie', 1, blackcolor)
                screen.blit(label121, (10, 30))
            if 122 in pokemon and 122 not in team_pkmn:
                if 122 in shinypokemon:
                        label122 = menu_font.render('122-Mr_Mime', 1, bluecolor)
                else:
                        label122 = menu_font.render('122-Mr_Mime', 1, blackcolor)
                screen.blit(label122, (10, 60))
            if 123 in pokemon and 123 not in team_pkmn:
                if 123 in shinypokemon:
                        label123 = menu_font.render('123-Scyther', 1, bluecolor)
                else:
                        label123 = menu_font.render('123-Scyther', 1, blackcolor)
                screen.blit(label123, (10, 90))
            if 124 in pokemon and 124 not in team_pkmn:
                if 124 in shinypokemon:
                        label124 = menu_font.render('124-Jynx', 1, bluecolor)
                else:
                        label124 = menu_font.render('124-Jynx', 1, blackcolor)
                screen.blit(label124, (10, 120))
            if 125 in pokemon and 125 not in team_pkmn:
                if 125 in shinypokemon:
                        label125 = menu_font.render('125-Electabuzz', 1, bluecolor)
                else:
                        label125 = menu_font.render('125-Electabuzz', 1, blackcolor)
                screen.blit(label125, (10, 150))
            if 126 in pokemon and 126 not in team_pkmn:
                if 126 in shinypokemon:
                        label126 = menu_font.render('126-Magmar', 1, bluecolor)
                else:
                        label126 = menu_font.render('126-Magmar', 1, blackcolor)
                screen.blit(label126, (10, 180))
            if 127 in pokemon and 127 not in team_pkmn:
                if 127 in shinypokemon:
                        label127 = menu_font.render('127-Pinsir', 1, bluecolor)
                else:
                        label127 = menu_font.render('127-Pinsir', 1, blackcolor)
                screen.blit(label127, (10, 210))
            if 128 in pokemon and 128 not in team_pkmn:
                if 128 in shinypokemon:
                        label128 = menu_font.render('128-Tauros', 1, bluecolor)
                else:
                        label128 = menu_font.render('128-Tauros', 1, blackcolor)
                screen.blit(label128, (10, 240))
            if 129 in pokemon and 129 not in team_pkmn:
                if 129 in shinypokemon:
                        label129 = menu_font.render('129-Magikarp', 1, bluecolor)
                else:
                        label129 = menu_font.render('129-Magikarp', 1, blackcolor)
                screen.blit(label129, (10, 270))
            if 130 in pokemon and 130 not in team_pkmn:
                if 130 in shinypokemon:
                        label130 = menu_font.render('130-Gyarados', 1, bluecolor)
                else:
                        label130 = menu_font.render('130-Gyarados', 1, blackcolor)
                screen.blit(label130, (10, 300))
            if 131 in pokemon and 131 not in team_pkmn:
                if 131 in shinypokemon:
                        label131 = menu_font.render('131-Lapras', 1, bluecolor)
                else:
                        label131 = menu_font.render('131-Lapras', 1, blackcolor)
                screen.blit(label131, (10, 330))
            if 132 in pokemon and 132 not in team_pkmn:
                if 132 in shinypokemon:
                        label132 = menu_font.render('132-Ditto', 1, bluecolor)
                else:
                        label132 = menu_font.render('132-Ditto', 1, blackcolor)
                screen.blit(label132, (10, 360))
            if 133 in pokemon and 133 not in team_pkmn:
                if 133 in shinypokemon:
                        label133 = menu_font.render('133-Eevee', 1, bluecolor)
                else:
                        label133 = menu_font.render('133-Eevee', 1, blackcolor)
                screen.blit(label133, (10, 390))
            if 134 in pokemon and 134 not in team_pkmn:
                if 134 in shinypokemon:
                        label134 = menu_font.render('134-Vaporeon', 1, bluecolor)
                else:
                        label134 = menu_font.render('134-Vaporeon', 1, blackcolor)
                screen.blit(label134, (10, 420))
            if 135 in pokemon and 135 not in team_pkmn:
                if 135 in shinypokemon:
                        label135 = menu_font.render('135-Jolteon', 1, bluecolor)
                else:
                        label135 = menu_font.render('135-Jolteon', 1, blackcolor)
                screen.blit(label135, (10, 450))
        if n == 9:
            if 136 in pokemon and 136 not in team_pkmn:
                if 136 in shinypokemon:
                        label136 = menu_font.render('136-Flareon', 1, bluecolor)
                else:
                        label136 = menu_font.render('136-Flareon', 1, blackcolor)
                screen.blit(label136, (10, 30))
            if 137 in pokemon and 137 not in team_pkmn:
                if 137 in shinypokemon:
                        label137 = menu_font.render('137-Porygon', 1, bluecolor)
                else:
                        label137 = menu_font.render('137-Porygon', 1, blackcolor)
                screen.blit(label137, (10, 60))
            if 138 in pokemon and 138 not in team_pkmn:
                if 138 in shinypokemon:
                        label138 = menu_font.render('138-Omanyte', 1, bluecolor)
                else:
                        label138 = menu_font.render('138-Omanyte', 1, blackcolor)
                screen.blit(label138, (10, 90))
            if 139 in pokemon and 139 not in team_pkmn:
                if 139 in shinypokemon:
                        label139 = menu_font.render('139-Omastar', 1, bluecolor)
                else:
                        label139 = menu_font.render('139-Omastar', 1, blackcolor)
                screen.blit(label139, (10, 120))
            if 140 in pokemon and 140 not in team_pkmn:
                if 140 in shinypokemon:
                        label140 = menu_font.render('140-Kabuto', 1, bluecolor)
                else:
                        label140 = menu_font.render('140-Kabuto', 1, blackcolor)
                screen.blit(label140, (10, 150))
            if 141 in pokemon and 141 not in team_pkmn:
                if 141 in shinypokemon:
                        label141 = menu_font.render('141-Kabutops', 1, bluecolor)
                else:
                        label141 = menu_font.render('141-Kabutops', 1, blackcolor)
                screen.blit(label141, (10, 180))
            if 142 in pokemon and 142 not in team_pkmn:
                if 142 in shinypokemon:
                        label142 = menu_font.render('142-Aerodactyl', 1, bluecolor)
                else:
                        label142 = menu_font.render('142-Aerodactyl', 1, blackcolor)
                screen.blit(label142, (10, 210))
            if 143 in pokemon and 143 not in team_pkmn:
                if 143 in shinypokemon:
                        label143 = menu_font.render('143-Snorlax', 1, bluecolor)
                else:
                        label143 = menu_font.render('143-Snorlax', 1, blackcolor)
                screen.blit(label143, (10, 240))
            if 144 in pokemon and 144 not in team_pkmn:
                if 144 in shinypokemon:
                        label144 = menu_font.render('144-Articuno', 1, bluecolor)
                else:
                        label144 = menu_font.render('144-Articuno', 1, blackcolor)
                screen.blit(label144, (10, 270))
            if 145 in pokemon and 145 not in team_pkmn:
                if 145 in shinypokemon:
                        label145 = menu_font.render('145-Zapdos', 1, bluecolor)
                else:
                        label145 = menu_font.render('145-Zapdos', 1, blackcolor)
                screen.blit(label145, (10, 300))
            if 146 in pokemon and 146 not in team_pkmn:
                if 146 in shinypokemon:
                        label146 = menu_font.render('146-Moltres', 1, bluecolor)
                else:
                        label146 = menu_font.render('146-Moltres', 1, blackcolor)
                screen.blit(label146, (10, 330))
            if 147 in pokemon and 147 not in team_pkmn:
                if 147 in shinypokemon:
                        label147 = menu_font.render('147-Dratini', 1, bluecolor)
                else:
                        label147 = menu_font.render('147-Dratini', 1, blackcolor)
                screen.blit(label147, (10, 360))
            if 148 in pokemon and 148 not in team_pkmn:
                if 148 in shinypokemon:
                        label148 = menu_font.render('148-Dragonair', 1, bluecolor)
                else:
                        label148 = menu_font.render('148-Dragonair', 1, blackcolor)
                screen.blit(label148, (10, 390))
            if 149 in pokemon and 149 not in team_pkmn:
                if 149 in shinypokemon:
                        label149 = menu_font.render('149-Dragonite', 1, bluecolor)
                else:
                        label149 = menu_font.render('149-Dragonite', 1, blackcolor)
                screen.blit(label149, (10, 420))
            if 150 in pokemon and 150 not in team_pkmn:
                if 150 in shinypokemon:
                        label150 = menu_font.render('150-Mewtwo', 1, bluecolor)
                else:
                        label150 = menu_font.render('150-Mewtwo', 1, blackcolor)
                screen.blit(label150, (10, 450))
        if n == 10:
            if 151 in pokemon and 151 not in team_pkmn:
                if 151 in shinypokemon:
                        label151 = menu_font.render('151-Mew', 1, bluecolor)
                else:
                        label151 = menu_font.render('151-Mew', 1, blackcolor)
                screen.blit(label151, (10, 30))
        
        fps.tick(60)
        pygame.display.flip()

def _battle(battles, team_pkmn, shinypokemon):
    '''
    This option allows the player to conduct a battle with a Pokemon Trainer.
    '''
    battle = 'start'
    attack = ''
    dead = 0
    nxt = 0
    battle_music.play(-1)
    labela = font.render('Attack', 1, blackcolor)
    labelb = font.render('Switch', 1, blackcolor)
    labelc = font.render('All of you Pokemon have fainted, and you take them to the', 1, blackcolor)
    labeld = font.render('Pokemon Center to restore them. The sadistic Nurse hopes', 1, blackcolor)
    labele = font.render('to see you and your Pokemon again.', 1, blackcolor)
    n = 0
    temp_team = {}
    enemy = pokemon_info.teams[battles]
    enemy_team = {}
    for each in enemy:
        enemy_team[each] = '100'
        
    for each in team_pkmn:
        temp_team[each] = '100'
    trainer_sprite = Trainer_Sprite(battles)
    trainer = pygame.sprite.RenderPlain((trainer_sprite))
    if battles == 0:     
        label1 = font.render('I am Barock Obama. I am the president of the Pokemon world!', 1, blackcolor)
        label2 = font.render('My Pokemon are like my approval ratings, rocky!', 1, blackcolor)
        label3 = font.render('Vote for me for re-election, but before that, lets battle!', 1, blackcolor)
        label4 = font.render('', 1, blackcolor)
        label5 = font.render('', 1, blackcolor)
        label6 = font.render('You were supposed to let the president win... well, if the', 1, blackcolor)
        label7 = font.render('Pokemon world actually did have a president. Anyways here', 1, blackcolor)
        label8 = font.render('is the Boulder badge. My career is going downhill from', 1, blackcolor)
        label9 = font.render('here.', 1, blackcolor)
        label10 = font.render('', 1, blackcolor)
    if battles == 1:
        label1 = font.render('...ugh..*burp*...name..Foggy...I...I...had enough...', 1, blackcolor)
        label2 = font.render('...Squirtle...water..ugh...but...must...drink...more...', 1, blackcolor)
        label3 = font.render('battle...wha?..', 1, blackcolor)
        label4 = font.render('', 1, blackcolor)
        label5 = font.render('', 1, blackcolor)
        label6 = font.render('*vomits up Cascade badge*...ugh...I.think..I..am...to...take', 1, blackcolor)
        label7 = font.render('a...nap..*ZZzzZZ*', 1, blackcolor)
        label8 = font.render('', 1, blackcolor)
        label9 = font.render('', 1, blackcolor)
        label10 = font.render('', 1, blackcolor)
    if battles == 2:
        label1 = font.render('I am Pvt. Lightning. I got kicked out of the army because I', 1, blackcolor)
        label2 = font.render('electrocuted my whole platoon. I guess it is pretty shocking', 1, blackcolor)
        label3 = font.render('that I am not in prison! Let us see what you got!', 1, blackcolor)
        label4 = font.render('', 1, blackcolor)
        label5 = font.render('', 1, blackcolor)
        label6 = font.render('Hmph...here is the Thunder badge. Leave before I strap', 1, blackcolor)
        label7 = font.render('you to an electric chair...', 1, blackcolor)
        label8 = font.render('', 1, blackcolor)
        label9 = font.render('', 1, blackcolor)
        label10 = font.render('', 1, blackcolor)
    if battles == 3:
        label1 = font.render('Ahhh... I feel high as a... OH! Hello there. My name is', 1, blackcolor)
        label2 = font.render('Mary Jane. Battle? Well, I was attending to my plants,', 1, blackcolor)
        label3 = font.render(' but okay!', 1, blackcolor)
        label4 = font.render('', 1, blackcolor)
        label5 = font.render('', 1, blackcolor)
        label6 = font.render('NO FAIR! You should battle people when they are clear-headed!', 1, blackcolor)
        label7 = font.render('But I guess you can have the Rainbow Badge', 1, blackcolor)
        label8 = font.render('', 1, blackcolor)
        label9 = font.render('', 1, blackcolor)
        label10 = font.render('', 1, blackcolor)
    if battles == 4:
        label1 = font.render('I am ninja and ninjas do not give out their names to people', 1, blackcolor)
        label2 = font.render('like you. Here is helpful tip: it is harmless to drink', 1, blackcolor)
        label3 = font.render('poison. Or was that venom? Well, there is only one', 1, blackcolor)
        label4 = font.render('way to find out.', 1, blackcolor)
        label5 = font.render('', 1, blackcolor)
        label6 = font.render('ugh...it was definitely venom...here is the Soul badge.', 1, blackcolor)
        label7 = font.render('No...do not go...call an...ambulance...please.', 1, blackcolor)
        label8 = font.render('Nooooooo...', 1, blackcolor)
        label9 = font.render('', 1, blackcolor)
        label10 = font.render('', 1, blackcolor)
    if battles == 5:
        label1 = font.render('I am Gemini. My psychic ablilties show me that your', 1, blackcolor)
        label2 = font.render('future is mostly clouded, but I do see that your future holds', 1, blackcolor)
        label3 = font.render('a lot of excercise. I also see great success for the makers', 1, blackcolor)
        label4 = font.render('of this video game!', 1, blackcolor)
        label5 = font.render('', 1, blackcolor)
        label6 = font.render('I...I... did not foresee this?!?! Your future...so...', 1, blackcolor)
        label7 = font.render('unrestrained. Here take the Marsh Badge and LEAVE!', 1, blackcolor)
        label8 = font.render('', 1, blackcolor)
        label9 = font.render('', 1, blackcolor)
        label10 = font.render('', 1, blackcolor)
    if battles == 6:
        label1 = font.render('Blaine is not my name. Do not blame me, it is just part', 1, blackcolor)
        label2 = font.render('of the game. Woah, kid look at that plane. I think it is', 1, blackcolor)
        label3 = font.render('covered in...flames?', 1, blackcolor)
        label4 = font.render('', 1, blackcolor)
        label5 = font.render('', 1, blackcolor)
        label6 = font.render('Here kid, here is the Volcano Badge...uhhh does anything', 1, blackcolor)
        label7 = font.render('rhyme with volcano? I am not going senile, am I? ', 1, blackcolor)
        label8 = font.render('', 1, blackcolor)
        label9 = font.render('', 1, blackcolor)
        label10 = font.render('', 1, blackcolor)
    if battles == 7:
        label1 = font.render('I am Mario. What? Leader of Team Rocket? What are you', 1, blackcolor)
        label2 = font.render('talking about? I am a legitimate business owner. You are', 1, blackcolor)
        label3 = font.render('making me mad. I am going to put you in your place. The', 1, blackcolor)
        label4 = font.render('ground!', 1, blackcolor)
        label5 = font.render('', 1, blackcolor)
        label6 = font.render('HaHaHa the Elite Three will show you! Here take the', 1, blackcolor)
        label7 = font.render('Earth Badge. Your doom draws closer!', 1, blackcolor)
        label8 = font.render('', 1, blackcolor)
        label9 = font.render('', 1, blackcolor)
        label10 = font.render('', 1, blackcolor)
    if battles == 8:
        label1 = font.render('My name is David. I am the Art Designer. I collected all the', 1, blackcolor)
        label2 = font.render('sprites for the game. I even did some of the progamming.', 1, blackcolor)
        label3 = font.render('The battle logic was all me!', 1, blackcolor)
        label4 = font.render('', 1, blackcolor)
        label5 = font.render('', 1, blackcolor)
        label6 = font.render('Hmmmm the battle logic must not be working correctly...', 1, blackcolor)
        label7 = font.render('', 1, blackcolor)
        label8 = font.render('', 1, blackcolor)
        label9 = font.render('', 1, blackcolor)
        label10 = font.render('', 1, blackcolor)
    if battles == 9:
        label1 = font.render('I am Oscar of the Elite Three and the Sound Designer.', 1, blackcolor)
        label2 = font.render('I got all the sound files for the game and I even', 1, blackcolor)
        label3 = font.render('wrote the dasldjasd *ahem* dialogue. Are you ready?', 1, blackcolor)
        label4 = font.render('My jams has destroyed all my competition! Ah-ha', 1, blackcolor)
        label5 = font.render('', 1, blackcolor)
        label6 = font.render('Looks like your team was louder...', 1, blackcolor)
        label7 = font.render('', 1, blackcolor)
        label8 = font.render('', 1, blackcolor)
        label9 = font.render('', 1, blackcolor)
        label10 = font.render('', 1, blackcolor)
    if battles == 10:
        label1 = font.render('I am Jonathan. I am the Lead Programmer. All that', 1, blackcolor)
        label2 = font.render('you see is MY WORK. It will be impossible to beat', 1, blackcolor)
        label3 = font.render('me! HAHAHA', 1, blackcolor)
        label4 = font.render('', 1, blackcolor)
        label5 = font.render('', 1, blackcolor)
        label6 = font.render('What?!?! I thought I programmed my invinciblity...', 1, blackcolor)
        label7 = font.render('Well, you are no match for the Champion!', 1, blackcolor)
        label8 = font.render('', 1, blackcolor)
        label9 = font.render('', 1, blackcolor)
        label10 = font.render('', 1, blackcolor)
    if battles == 11:
        label1 = font.render('I am TA Colin. I control the fates of many people...', 1, blackcolor)
        label2 = font.render('even yours...', 1, blackcolor)
        label3 = font.render('', 1, blackcolor)
        label4 = font.render('', 1, blackcolor)
        label5 = font.render('', 1, blackcolor)
        label6 = font.render('The stupid Elite Three forgot to make me unbeatable.', 1, blackcolor)
        label7 = font.render('They will FAIL for this! Anyways, congratulations,', 1, blackcolor)
        label8 = font.render('Champion.', 1, blackcolor)
        label9 = font.render('', 1, blackcolor)
        label10 = font.render('', 1, blackcolor)
    x1 = 180
    x2 = 195
    x3 = 187.5
    
    
    while True:
        for event in pygame.event.get(): 
            if event.type == QUIT:
                pygame.quit()
                return
            elif event.type == pygame.KEYDOWN:
                key = pygame.key.name(event.key) 
                if event.key == pygame.K_a:

                    if n == 0:
                        menu_sound.play()
                        n += 1
                    if n == 5 and x1 != 180 and x2 != 195 and x3 != 187.5:
                        menu_sound.play()
                        x1 = 180
                        x2 = 195
                        x3 = 187.5
                    if n == 10:
                        menu_sound.play()
                        victory_end.stop()
                        menu_music.play(-1)
                        return 1
                    if n == 12:
                        menu_sound.play()
                        menu_music.play(-1)
                        return
                if event.key == pygame.K_d and x1 != 380 and x2 != 395 and x3 != 387.5:
                    menu_sound.play()
                    x1 = 380
                    x2 = 395
                    x3 = 387.5
                if key == 'enter' and n == 5 and x1 == 380 and x2 == 395 and x3 == 387.5 or key == 'return' and n == 5 and x1 == 380 and x2 == 395 and x3 == 387.5:
                    n = 6
                if key == 'enter' and n == 5 and x1 == 180 and x2 == 195 and x3 == 187.5 or key == 'return' and n == 5 and x1 == 180 and x2 == 195 and x3 == 187.5:
                    result = attack_effectiveness(pokemon_info.All_Pokemon[choice-1], pokemon_info.All_Pokemon[enemy[nxt]-1])
                    damage = return_damage_amount(result)
                    if int(damage) > 0:
                        tackle_sound.play()
                        pygame.time.wait(2000)
                    enemy_team[enemy[nxt]] = str(int(enemy_team[enemy[nxt]])-int(damage))
                    if int(enemy_team[enemy[nxt]]) <= 0:
                        enemy_team[enemy[nxt]] = str(0)
                        all_cries[enemy[nxt]-1].play()
                        n = 8
                    else:
                        result = attack_effectiveness(pokemon_info.All_Pokemon[enemy[nxt]-1], pokemon_info.All_Pokemon[choice-1])
                        damage = return_damage_amount(result)
                        
                        if int(damage) > 0:
                            tackle_sound.play()
                            pygame.time.wait(2000)
                            temp_team[choice] = str(int(temp_team[choice])-int(damage))
                        if int(temp_team[choice]) <= 0:
                            temp_team[choice] = str(0)
                            all_cries[choice-1].play()
                            n = 7
                                                    
                        

        screen.fill(whitecolor)

        if n == 0:
            pygame.draw.rect(screen, blackcolor, (0, 300, 640, 180), 2)  
            trainer_sprite.add(10)
            trainer.draw(screen)
            screen.blit(label1, (10, 310))
            screen.blit(label2, (10, 340))
            screen.blit(label3, (10, 370))
            screen.blit(label4, (10, 400))
            screen.blit(label5, (10, 430))
        if n == 1:
            enemy_sprite = Enemy_Sprite(enemy[nxt])
            en_pkmn = pygame.sprite.RenderPlain((enemy_sprite))
            if battle == 'start':
                n = 2
            else:
                n = 5
        if n == 2:
            enemy_sprite.add()
            pygame.draw.rect(screen, blackcolor, (0, 300, 640, 180), 2)
            pygame.draw.rect(screen, whitecolor, (0,200, 640,100)) 
            en_pkmn.draw(screen)
            labelename = menu_font.render('{}'.format(pokemon_info._get_name(enemy[nxt])), 1, blackcolor)
            labeleHP = menu_font.render('HP:{}'.format(enemy_team[enemy[nxt]]), 1, blackcolor)
            screen.blit(labelename, (200, 80))
            screen.blit(labeleHP, (200, 110))
            if enemy_sprite.get_y() == 80:
                n = 3
            battle = 'started'
        if n == 3:
            pygame.time.wait(4000)
            n = 4
        if n == 4:
            choice = _choose_pokemon(temp_team, shinypokemon)
            friend_sprite = Team_Sprite(choice, shinypokemon)
            fr_pkmn = pygame.sprite.RenderPlain((friend_sprite))
            n = 5
        if n == 5:
            friend_sprite.add()
            enemy_sprite.add()
            fr_pkmn.draw(screen)
            en_pkmn.draw(screen)
            labelename = menu_font.render('{}'.format(pokemon_info._get_name(enemy[nxt])), 1, blackcolor)
            labeleHP = menu_font.render('HP:{}'.format(enemy_team[enemy[nxt]]), 1, blackcolor)
            labelname = menu_font.render('{}'.format(pokemon_info._get_name(choice)), 1, blackcolor)
            labelHP = menu_font.render('HP:{}'.format(temp_team[choice]), 1, blackcolor)
            screen.blit(labelename, (200, 80))
            screen.blit(labeleHP, (200, 110))
            screen.blit(labelname, (290, 210))
            screen.blit(labelHP, (290, 240))
            pygame.draw.rect(screen, blackcolor, (0, 300, 640, 180), 2)
            pygame.draw.rect(screen, whitecolor,(5, 302, 638, 175))
            pygame.draw.polygon(screen, blackcolor,((x1,325),(x2,325), (x3,340)))
            screen.blit(labela, (160, 350))
            screen.blit(labelb, (350, 350))
        if n == 6:
            old_choice = choice
            choice = _choose_pokemon(temp_team, shinypokemon, old_choice)
            if choice != None:
                friend_sprite = Team_Sprite(choice, shinypokemon)
                fr_pkmn = pygame.sprite.RenderPlain((friend_sprite))
            else:
                choice = old_choice
            n = 5
        if n == 7:
            friend_sprite.sub()
            fr_pkmn.draw(screen)
            en_pkmn.draw(screen)
            labelename = menu_font.render('{}'.format(pokemon_info._get_name(enemy[nxt])), 1, blackcolor)
            labeleHP = menu_font.render('HP:{}'.format(enemy_team[enemy[nxt]]), 1, blackcolor)
            labelname = menu_font.render('{}'.format(pokemon_info._get_name(choice)), 1, blackcolor)
            labelHP = menu_font.render('HP:{}'.format(temp_team[choice]), 1, blackcolor)
            screen.blit(labelename, (200, 80))
            screen.blit(labeleHP, (200, 110))
            screen.blit(labelname, (290, 210))
            screen.blit(labelHP, (290, 240))
            pygame.draw.rect(screen, blackcolor, (0, 300, 640, 180), 2)
            pygame.draw.rect(screen, whitecolor,(5, 302, 638, 175))
            pygame.draw.polygon(screen, blackcolor,((x1,325),(x2,325), (x3,340)))
            screen.blit(labela, (160, 350))
            screen.blit(labelb, (350, 350))

            if friend_sprite.get_y() >= 430:
                dead += 1
                if dead >= len(team_pkmn):
                    n = 11
                else:
                    n = 4
        if n == 8:
            enemy_sprite.sub()
            fr_pkmn.draw(screen)
            en_pkmn.draw(screen)
            labelename = menu_font.render('{}'.format(pokemon_info._get_name(enemy[nxt])), 1, blackcolor)
            labeleHP = menu_font.render('HP:{}'.format(enemy_team[enemy[nxt]]), 1, blackcolor)
            labelname = menu_font.render('{}'.format(pokemon_info._get_name(choice)), 1, blackcolor)
            labelHP = menu_font.render('HP:{}'.format(temp_team[choice]), 1, blackcolor)
            screen.blit(labelename, (200, 80))
            screen.blit(labeleHP, (200, 110))
            screen.blit(labelname, (290, 210))
            screen.blit(labelHP, (290, 240))
            pygame.draw.rect(screen, blackcolor, (0, 300, 640, 180), 2)
            pygame.draw.rect(screen, whitecolor,(5, 302, 638, 175))
            pygame.draw.polygon(screen, blackcolor,((x1,325),(x2,325), (x3,340)))
            screen.blit(labela, (160, 350))
            screen.blit(labelb, (350, 350))

            if enemy_sprite.get_y() >= 700:
                nxt += 1
                if nxt == len(enemy):
                    n = 9
                else:
                    enemy_sprite = Enemy_Sprite(enemy[nxt])
                    en_pkmn = pygame.sprite.RenderPlain((enemy_sprite))
                    n = 5
        if n == 9:
            battle_music.stop()
            victory_start.play()
            pygame.time.wait(4000)
            victory_end.play(-1)
            trainer_sprite = Trainer_Sprite(battles)
            trainer = pygame.sprite.RenderPlain((trainer_sprite))
            n = 10
        if n == 10:
            
            pygame.draw.rect(screen, blackcolor, (0, 300, 640, 180), 2)  
            trainer_sprite.add(10)
            trainer.draw(screen)
            screen.blit(label6, (10, 310))
            screen.blit(label7, (10, 340))
            screen.blit(label8, (10, 370))
            screen.blit(label9, (10, 400))
            screen.blit(label10, (10, 430))

        if n == 11:
           battle_music.stop()
           pygame.time.wait(3000)
           n = 12
        if n == 12:
            pygame.draw.rect(screen, blackcolor, (0, 300, 640, 180), 2)
            en_pkmn.draw(screen)
            labelename = menu_font.render('{}'.format(pokemon_info._get_name(enemy[nxt])), 1, blackcolor)
            labeleHP = menu_font.render('HP:{}'.format(enemy_team[enemy[nxt]]), 1, blackcolor)
            screen.blit(labelename, (200, 80))
            screen.blit(labeleHP, (200, 110))
            screen.blit(labelname, (290, 210))
            screen.blit(labelHP, (290, 240))
            pygame.draw.rect(screen, blackcolor, (0, 300, 640, 180), 2)
            pygame.draw.rect(screen, whitecolor,(5, 302, 638, 175))
            screen.blit(labelc, (10, 310))
            screen.blit(labeld, (10, 340))
            screen.blit(labele, (10, 370))
            
            
        
           
            
                  
        fps.tick(60)
        pygame.display.flip()

def _choose_pokemon(temp_team, shinypokemon, old_choice = None):
    '''
    This option allows the player to switch Pokemon.
    '''
    label0 = font.render('Team:', 1, blackcolor)
    label02 = font.render('Enter the # of the pokemon', 1, blackcolor)
    label04 = font.render('Press "enter" to send your', 1, blackcolor)
    label05 = font.render('request.', 1, blackcolor)
    number = ''
    friendly = list(temp_team.keys())
    while True:
        
        label03 = font.render('you wish to send to battle: {}'.format(number), 1, blackcolor)
        if friendly[0] not in shinypokemon:
            label1 = font.render('{}-{}:{}HP'.format(friendly[0], pokemon_info._get_name(friendly[0]),temp_team[friendly[0]]), 1, blackcolor)
        else:
            label1 = font.render('{}-{}:{}HP'.format(friendly[0], pokemon_info._get_name(friendly[0]),temp_team[friendly[0]]), 1, bluecolor)
        if len(friendly) >= 2:
            if friendly[1] not in shinypokemon:
                label2 = font.render('{}-{}:{}HP'.format(friendly[1], pokemon_info._get_name(friendly[1]),temp_team[friendly[1]]), 1, blackcolor)
            else:
                label2 = font.render('{}-{}:{}HP'.format(friendly[1], pokemon_info._get_name(friendly[1]),temp_team[friendly[1]]), 1, bluecolor)
        if len(friendly) >= 3:
            if friendly[2] not in shinypokemon:
                label3 = font.render('{}-{}:{}HP'.format(friendly[2], pokemon_info._get_name(friendly[2]),temp_team[friendly[2]]), 1, blackcolor)
            else:
                label3 = font.render('{}-{}:{}HP'.format(friendly[2], pokemon_info._get_name(friendly[2]),temp_team[friendly[2]]), 1, bluecolor)
        if len(friendly) >= 4:
            if friendly[3] not in shinypokemon:
                label4 = font.render('{}-{}:{}HP'.format(friendly[3], pokemon_info._get_name(friendly[3]),temp_team[friendly[3]]), 1, blackcolor)
            else:
                label4 = font.render('{}-{}:{}HP'.format(friendly[3], pokemon_info._get_name(friendly[3]),temp_team[friendly[3]]), 1, bluecolor)
        if len(friendly) >= 5:
            if friendly[4] not in shinypokemon:
                label5 = font.render('{}-{}:{}HP'.format(friendly[4], pokemon_info._get_name(friendly[4]),temp_team[friendly[4]]), 1, blackcolor)
            else:
                label5 = font.render('{}-{}:{}HP'.format(friendly[4], pokemon_info._get_name(friendly[4]),temp_team[friendly[4]]), 1, bluecolor)
        if len(friendly) >= 6:
            if friendly[5] not in shinypokemon:
                label6 = font.render('{}-{}:{}HP'.format(friendly[5], pokemon_info._get_name(friendly[5]),temp_team[friendly[5]]), 1, blackcolor)
            else:
                label6 = font.render('{}-{}:{}HP'.format(friendly[5], pokemon_info._get_name(friendly[5]),temp_team[friendly[5]]), 1, bluecolor)
        for event in pygame.event.get():
            if event.type == QUIT:
                pygame.quit()
                return
            elif event.type == pygame.KEYDOWN:
                key = pygame.key.name(event.key)
                key = _corrector(key)
                if key == 'enter' or key == 'return':
                    try:
                        if int(number) in friendly:
                            if temp_team[int(number)] == '0':
                                number = ''
                            if old_choice != None:
                                if old_choice == int(number):
                                    return
                                if old_choice != int(number):
                                    return int(number)
                            if old_choice == None:
                                return int(number)
                        else:
                            number = ''
                    except:
                        number = ''
                        pass
                if key == 'backspace':
                    number = number[:-1]

                if key != 'enter' and key != 'backspace' and key != 'return':
                    number = number+key
        screen.fill(whitecolor)
        screen.blit(label0, (0,0))
        screen.blit(label02, (320, 30))
        screen.blit(label03, (320, 60))
        screen.blit(label04, (320, 90))
        screen.blit(label05, (320, 120))
        screen.blit(label1, (10, 30))
        if len(friendly) >= 2:
            screen.blit(label2, (10, 90))
        if len(friendly) >= 3:
            screen.blit(label3, (10, 150))
        if len(friendly) >= 4:
            screen.blit(label4, (10, 210))
        if len(friendly) >= 5:
            screen.blit(label5, (10, 270))
        if len(friendly) >= 6:
            screen.blit(label6, (10, 330))
        fps.tick(60)
        pygame.display.flip()

def _resetter(condition):
    '''
    This function resets the save file depending on the condition. It also gives a player a message, if the player has completed a certain
    task.
    '''
    if condition == 1:                   
        label1 = menu_font.render('I must take your badges and non-shiny Pokemon for...uhh...research', 1, blackcolor)
        label2 = menu_font.render('purposes. Now you get to do everything over again! How wonderful!', 1, blackcolor)
        label3 = menu_font.render('But now you have higher chances of getting shiny Pokemon!', 1, blackcolor)
        label4 = menu_font.render('I will give you another pokemon.', 1, blackcolor)
        label5 = menu_font.render('    -Prof. Poison Oak', 1, blackcolor)

    if condition == 2:
        label1 = menu_font.render('Special Prize (A Message From The Lead Programmer):', 1, blackcolor)
        label2 = menu_font.render('Congratulations! If you are getting this message that means you ', 1, blackcolor)
        label3 = menu_font.render('got all 151 shiny Pokemon and beaten all 12 Pokemon Trainers once', 1, blackcolor)
        label4 = menu_font.render('again! You are a really active person! Unless you cheated, then you', 1, blackcolor)
        label5 = menu_font.render('are a cheater! Shame on you!', 1, blackcolor)
    new_time = pygame.time.get_ticks()
    while True:
        events = pygame.event.get()
        for event in events: 
            if event.type == QUIT:
                pygame.quit()
                return
            elif event.type == pygame.KEYDOWN:
                if event.key == pygame.K_KP_ENTER or event.key == pygame.K_RETURN:
                    pass

        
        time = pygame.time.get_ticks()

        if (time-new_time) < 8000:
            screen.fill(whitecolor)
            screen.blit(label1, (50, 150))
            screen.blit(label2, (50, 180))
            screen.blit(label3, (50, 210))
            screen.blit(label4, (50, 240))
            screen.blit(label5, (50, 270))
        if (time-new_time) > 8000:
            return

        fps.tick(60)
        pygame.display.flip()

        
        
                                
        
    

if __name__ == '__main__':
    full_game()
        

    
    

    






    
    
        
