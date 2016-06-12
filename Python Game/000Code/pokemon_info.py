from collections import namedtuple

Pokemon = namedtuple('Pokemon', ['number', 'type'])

Bulbasaur = Pokemon(1, ['Grass', 'Poison'])
Ivysaur = Pokemon(2, ['Grass', 'Poison'])
Venusaur = Pokemon(3, ['Grass', 'Poison'])
Charmander = Pokemon(4, ['Fire'])
Charmeleon = Pokemon(5, ['Fire'])
Charizard = Pokemon(6, ['Fire', 'Flying'])
Squirtle = Pokemon(7, ['Water'])
Wartortle = Pokemon(8, ['Water'])
Blastoise = Pokemon(9, ['Water'])
Caterpie = Pokemon(10, ['Bug'])
Metapod = Pokemon(11, ['Bug'])
Butterfree = Pokemon(12, ['Bug', 'Flying'])
Weedle = Pokemon(13, ['Bug', 'Poison'])
Kakuna = Pokemon(14, ['Bug', 'Poison'])
Beedrill = Pokemon(15, ['Bug', 'Poison'])
Pidgey = Pokemon(16, ['Normal', 'Flying'])
Pidgeotto = Pokemon(17, ['Normal', 'Flying'])
Pidgeot = Pokemon(18, ['Normal', 'Flying'])
Rattata = Pokemon(19, ['Normal'])
Raticate = Pokemon(20, ['Normal'])
Spearow = Pokemon(21, ['Normal', 'Flying'])
Fearow = Pokemon(22, ['Normal', 'Flying'])
Ekans = Pokemon(23, ['Poison'])
Arbok = Pokemon(24, ['Poison'])
Pikachu = Pokemon(25, ['Electric'])
Raichu = Pokemon(26, ['Electric'])
Sandshrew = Pokemon(27, ['Ground'])
Sandslash = Pokemon(28, ['Ground'])
NidoranF = Pokemon(29, ['Poison'])
Nidorina = Pokemon(30, ['Poison'])
Nidoqueen = Pokemon(31, ['Poison', 'Ground'])
NidoranM = Pokemon(32, ['Poison'])
Nidorino = Pokemon(33, ['Poison'])
Nidoking = Pokemon(34, ['Poison', 'Ground'])
Clefairy = Pokemon(35, ['Normal'])
Clefable = Pokemon(36, ['Normal'])
Vulpix = Pokemon(37, ['Fire'])
Ninetales = Pokemon(38, ['Fire'])
Jigglypuff = Pokemon(39, ['Normal'])
Wigglytuff = Pokemon(40, ['Normal'])
Zubat = Pokemon(41, ['Flying', 'Poison'])
Golbat = Pokemon(42, ['Flying', 'Poison'])
Oddish = Pokemon(43, ['Grass', 'Poison'])
Gloom = Pokemon(44, ['Grass', 'Poison'])
Vileplume = Pokemon(45, ['Grass', 'Poison'])
Paras = Pokemon(46, ['Bug', 'Grass'])
Parasect = Pokemon(47, ['Bug', 'Grass'])
Venonat = Pokemon(48, ['Bug', 'Poison'])
Venomoth = Pokemon(49, ['Bug', 'Poison'])
Diglett = Pokemon(50, ['Ground'])
Dugtrio = Pokemon(51, ['Ground'])
Meowth = Pokemon(52, ['Normal'])
Persian = Pokemon(53, ['Normal'])
Psyduck = Pokemon(54, ['Water'])
Golduck = Pokemon(55, ['Water'])
Mankey = Pokemon(56, ['Fighting'])
Primeape = Pokemon(57, ['Fighting'])
Growlithe = Pokemon(58, ['Fire'])
Arcanine = Pokemon(59, ['Fire'])
Poliwag = Pokemon(60, ['Water'])
Poliwhirl = Pokemon(61, ['Water'])
Poliwrath = Pokemon(62, ['Water', 'Fighting'])
Abra = Pokemon(63, ['Psychic'])
Kadabra = Pokemon(64, ['Psychic'])
Alakazam = Pokemon(65, ['Psychic'])
Machop = Pokemon(66, ['Fighting'])
Machoke = Pokemon(67, ['Fighting'])
Machamp = Pokemon(68, ['Fighting'])
Bellsprout = Pokemon(69, ['Grass', 'Poison'])
Weepinbell = Pokemon(70, ['Grass', 'Poison'])
Victreebel = Pokemon(71, ['Grass', 'Poison'])
Tentacool = Pokemon(72, ['Water', 'Poison'])
Tentacruel = Pokemon(73, ['Water', 'Poison'])
Geodude = Pokemon(74, ['Rock', 'Ground'])
Graveler = Pokemon(75, ['Rock', 'Ground'])
Golem = Pokemon(76, ['Rock', 'Ground'])
Ponyta = Pokemon(77, ['Fire'])
Rapidash = Pokemon(78, ['Fire'])
Slowpoke = Pokemon(79, ['Water', 'Psychic'])
Slowbro = Pokemon(80, ['Water', 'Psychic'])
Magnemite = Pokemon(81, ['Electric'])
Magneton = Pokemon(82, ['Electric'])
FarfetchD = Pokemon(83, ['Normal', 'Flying'])
Doduo = Pokemon(84, ['Normal', 'Flying'])
Dodrio = Pokemon(85, ['Normal', 'Flying'])
Seel = Pokemon(86, ['Water'])
Dewgong = Pokemon(87, ['Water', 'Ice'])
Grimer = Pokemon(88, ['Poison'])
Muk = Pokemon(89, ['Poison'])
Shellder = Pokemon(90, ['Water'])
Cloyster = Pokemon(91, ['Water', 'Ice'])
Gastly = Pokemon(92, ['Ghost', 'Poison'])
Haunter = Pokemon(93, ['Ghost', 'Poison'])
Gengar = Pokemon(94, ['Ghost', 'Poison'])
Onix = Pokemon(95, ['Rock', 'Ground'])
Drowzee = Pokemon(96, ['Psychic'])
Hypno = Pokemon(97, ['Psychic'])
Krabby = Pokemon(98, ['Water'])
Kingler = Pokemon(99, ['Water'])
Voltorb = Pokemon(100, ['Electric'])
Electrode = Pokemon(101, ['Electric'])
Exeggcute = Pokemon(102, ['Grass', 'Psychic'])
Exeggutor = Pokemon(103, ['Grass', 'Psychic'])
Cubone = Pokemon(104, ['Ground'])
Marowak = Pokemon(105, ['Ground'])
Hitmonlee = Pokemon(106, ['Fighting'])
Hitmonchan = Pokemon(107, ['Fighting'])
Lickitung = Pokemon(108, ['Normal'])
Koffing = Pokemon(109, ['Poison'])
Weezing = Pokemon(110, ['Poison'])
Rhyhorn = Pokemon(111, ['Ground', 'Rock'])
Rhydon = Pokemon(112, ['Ground', 'Rock'])
Chansey = Pokemon(113, ['Normal'])
Tangela = Pokemon(114, ['Grass'])
Kangaskhan = Pokemon(115, ['Normal'])
Horsea = Pokemon(116, ['Water'])
Seadra = Pokemon(117, ['Water'])
Goldeen = Pokemon(118, ['Water'])
Seaking = Pokemon(119, ['Water'])
Staryu = Pokemon(120, ['Water'])
Starmie = Pokemon(121, ['Water', 'Psychic'])
Mr_Mime = Pokemon(122, ['Psychic'])
Scyther = Pokemon(123, ['Bug', 'Flying'])
Jynx = Pokemon(124, ['Psychic', 'Ice'])
Electabuzz = Pokemon(125, ['Electric'])
Magmar = Pokemon(126, ['Fire'])
Pinsir = Pokemon(127, ['Bug'])
Tauros = Pokemon(128, ['Normal'])
Magikarp = Pokemon(129, ['Water'])
Gyarados = Pokemon(130, ['Water', 'Flying'])
Lapras = Pokemon(131, ['Water', 'Ice'])
Ditto = Pokemon(132, ['Normal'])
Eevee = Pokemon(133, ['Normal'])
Vaporeon = Pokemon(134, ['Water'])
Jolteon = Pokemon(135, ['Electric'])
Flareon = Pokemon(136, ['Fire'])
Porygon = Pokemon(137, ['Normal'])
Omanyte = Pokemon(138, ['Water', 'Rock'])
Omastar = Pokemon(139, ['Water', 'Rock'])
Kabuto = Pokemon(140, ['Water', 'Rock'])
Kabutops = Pokemon(141, ['Water', 'Rock'])
Aerodactyl = Pokemon(142, ['Rock', 'Flying'])
Snorlax = Pokemon(143, ['Normal'])
Articuno = Pokemon(144, ['Ice', 'Flying'])
Zapdos = Pokemon(145, ['Electric', 'Flying'])
Moltres = Pokemon(146, ['Fire', 'Flying'])
Dratini = Pokemon(147, ['Dragon'])
Dragonair = Pokemon(148, ['Dragon'])
Dragonite = Pokemon(149, ['Dragon', 'Flying'])
Mewtwo = Pokemon(150, ['Psychic'])
Mew = Pokemon(151, ['Psychic'])


All_Pokemon = [Bulbasaur, Ivysaur, Venusaur, Charmander, Charmeleon, Charizard,
               Squirtle, Wartortle, Blastoise, Caterpie, Metapod, Butterfree,
               Weedle, Kakuna, Beedrill, Pidgey, Pidgeotto, Pidgeot, Rattata,
               Raticate, Spearow, Fearow, Ekans, Arbok, Pikachu, Raichu,
               Sandshrew, Sandslash, NidoranF, Nidorina, Nidoqueen, NidoranM,
               Nidorino, Nidoking, Clefairy, Clefable, Vulpix, Ninetales,
               Jigglypuff, Wigglytuff, Zubat, Golbat, Oddish, Gloom,
               Vileplume, Paras, Parasect, Venonat, Venomoth, Diglett,
               Dugtrio, Meowth, Persian, Psyduck, Golduck, Mankey, Primeape,
               Growlithe, Arcanine, Poliwag, Poliwhirl, Poliwrath, Abra,
               Kadabra, Alakazam, Machop, Machoke, Machamp, Bellsprout,
               Weepinbell, Victreebel, Tentacool, Tentacruel, Geodude,
               Graveler, Golem, Ponyta, Rapidash, Slowpoke, Slowbro,
               Magnemite, Magneton, FarfetchD, Doduo, Dodrio, Seel, Dewgong,
               Grimer, Muk, Shellder, Cloyster, Gastly, Haunter, Gengar, Onix,
               Drowzee, Hypno, Krabby, Kingler, Voltorb, Electrode, Exeggcute,
               Exeggutor, Cubone, Marowak, Hitmonlee, Hitmonchan, Lickitung,
               Koffing, Weezing, Rhyhorn, Rhydon, Chansey, Tangela, Kangaskhan,
               Horsea, Seadra, Goldeen, Seaking, Staryu, Starmie, Mr_Mime,
               Scyther, Jynx, Electabuzz, Magmar, Pinsir, Tauros, Magikarp,
               Gyarados, Lapras, Ditto, Eevee, Vaporeon, Jolteon, Flareon,
               Porygon, Omanyte, Omastar, Kabuto, Kabutops, Aerodactyl,
               Snorlax, Articuno, Zapdos, Moltres, Dratini, Dragonair,
               Dragonite, Mewtwo, Mew]
teams = [[74, 95, 75], [121, 86, 55, 62],
         [125, 26, 101], [71, 45, 103, 44],
         [24, 42, 49,89, 109],[65, 64, 93, 122],
         [59,78, 126, 38, 136], [34, 76, 112, 51, 31, 111],
         [129, 72, 14, 37, 149, 21], [149, 94, 112, 3, 57, 59],
         [6, 9, 3, 38, 59, 149], [65, 82, 18, 139, 49, 34]] 

def _get_name(number):
    '''
    From a number, this function will return the name of the Pokemon.
    '''
    x = 'Bulbasaur, Ivysaur, Venusaur, Charmander, Charmeleon, Charizard, Squirtle, Wartortle, Blastoise, Caterpie, Metapod, Butterfree, Weedle, Kakuna, Beedrill, Pidgey, Pidgeotto, Pidgeot, Rattata,Raticate, Spearow, Fearow, Ekans, Arbok, Pikachu, Raichu,Sandshrew, Sandslash, NidoranF, Nidorina, Nidoqueen, NidoranM,Nidorino, Nidoking, Clefairy, Clefable, Vulpix, Ninetales,Jigglypuff, Wigglytuff, Zubat, Golbat, Oddish, Gloom,Vileplume, Paras, Parasect, Venonat, Venomoth, Diglett,Dugtrio, Meowth, Persian, Psyduck, Golduck, Mankey, Primeape,Growlithe, Arcanine, Poliwag, Poliwhirl, Poliwrath, Abra,Kadabra, Alakazam, Machop, Machoke, Machamp, Bellsprout,Weepinbell, Victreebel, Tentacool, Tentacruel, Geodude,Graveler, Golem, Ponyta, Rapidash, Slowpoke, Slowbro,Magnemite, Magneton, FarfetchD, Doduo, Dodrio, Seel, Dewgong,Grimer, Muk, Shellder, Cloyster, Gastly, Haunter, Gengar, Onix,Drowzee, Hypno, Krabby, Kingler, Voltorb, Electrode, Exeggcute,Exeggutor, Cubone, Marowak, Hitmonlee, Hitmonchan, Lickitung,Koffing, Weezing, Rhyhorn, Rhydon, Chansey, Tangela, Kangaskhan,Horsea, Seadra, Goldeen, Seaking, Staryu, Starmie, Mr_Mime,Scyther, Jynx, Electabuzz, Magmar, Pinsir, Tauros, Magikarp,Gyarados, Lapras, Ditto, Eevee, Vaporeon, Jolteon, Flareon,Porygon, Omanyte, Omastar, Kabuto, Kabutops, Aerodactyl,Snorlax, Articuno, Zapdos, Moltres, Dratini, Dragonair,Dragonite, Mewtwo, Mew'
    x = x.split(',')
    return (x[number-1]).replace(' ','')





