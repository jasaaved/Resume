import os, sys
import pygame
from pygame.locals import *


pygame.mixer.pre_init(22050, -16, 2, 512)
pygame.init()


menu_sound = pygame.mixer.Sound("menusound.wav")
menu_music = pygame.mixer.Sound("menumusic.wav")
item_sound = pygame.mixer.Sound("item.wav")
welcome_music = pygame.mixer.Sound("welcome.wav")
battle_start = pygame.mixer.Sound("battlestart.wav")
battle_music = pygame.mixer.Sound("battle.wav")
tackle_sound = pygame.mixer.Sound("tackle.wav")
victory_start = pygame.mixer.Sound("victorystart.wav")
victory_end = pygame.mixer.Sound("victoryend.wav")

bulbasaur_sound = pygame.mixer.Sound("001.wav")
ivysaur_sound = pygame.mixer.Sound("002.wav")
venusaur_sound = pygame.mixer.Sound("003.wav")
charmander_sound = pygame.mixer.Sound("004.wav")
charmeleon_sound = pygame.mixer.Sound("005.wav")
charizard_sound = pygame.mixer.Sound("006.wav")
squirtle_sound = pygame.mixer.Sound("007.wav")
wartortle_sound = pygame.mixer.Sound("008.wav")
blastoise_sound = pygame.mixer.Sound("009.wav")
caterpie_sound = pygame.mixer.Sound("010.wav")
metapod_sound = pygame.mixer.Sound("011.wav")
butterfree_sound = pygame.mixer.Sound("012.wav")
weedle_sound = pygame.mixer.Sound("013.wav")
kakuna_sound = pygame.mixer.Sound("014.wav")
beedrill_sound = pygame.mixer.Sound("015.wav")
pidgey_sound = pygame.mixer.Sound("016.wav")
pidgeotto_sound = pygame.mixer.Sound("017.wav")
pidgeot_sound = pygame.mixer.Sound("018.wav")
rattata_sound = pygame.mixer.Sound("019.wav")
raticate_sound = pygame.mixer.Sound("020.wav")
spearow_sound = pygame.mixer.Sound("021.wav")
fearow_sound = pygame.mixer.Sound("022.wav")
ekans_sound = pygame.mixer.Sound("023.wav")
arbok_sound = pygame.mixer.Sound("024.wav")
pikachu_sound = pygame.mixer.Sound("025.wav")
raichu_sound = pygame.mixer.Sound("026.wav")
sandshrew_sound = pygame.mixer.Sound("027.wav")
sandslash_sound = pygame.mixer.Sound("028.wav")
nidoranf_sound = pygame.mixer.Sound("029.wav")
nidorina_sound = pygame.mixer.Sound("030.wav")
nidoqueen_sound = pygame.mixer.Sound("031.wav")
nidoranm_sound = pygame.mixer.Sound("032.wav")
nidorino_sound = pygame.mixer.Sound("033.wav")
nidoking_sound = pygame.mixer.Sound("034.wav")
clefairy_sound = pygame.mixer.Sound("035.wav")
clefable_sound = pygame.mixer.Sound("036.wav")
vulpix_sound = pygame.mixer.Sound("037.wav")
ninetales_sound = pygame.mixer.Sound("038.wav")
jigglypuff_sound = pygame.mixer.Sound("039.wav")
wigglytuff_sound = pygame.mixer.Sound("040.wav")
zubat_sound = pygame.mixer.Sound("041.wav")
golbat_sound = pygame.mixer.Sound("042.wav")
oddish_sound = pygame.mixer.Sound("043.wav")
gloom_sound = pygame.mixer.Sound("044.wav")
vileplume_sound = pygame.mixer.Sound("045.wav")
paras_sound = pygame.mixer.Sound("046.wav")
parasect_sound = pygame.mixer.Sound("047.wav")
venonat_sound = pygame.mixer.Sound("048.wav")
venomoth_sound = pygame.mixer.Sound("049.wav")
diglett_sound = pygame.mixer.Sound("050.wav")
dugtrio_sound = pygame.mixer.Sound("051.wav")
meowth_sound = pygame.mixer.Sound("052.wav")
persian_sound = pygame.mixer.Sound("053.wav")
psyduck_sound = pygame.mixer.Sound("054.wav")
golduck_sound = pygame.mixer.Sound("055.wav")
mankey_sound = pygame.mixer.Sound("056.wav")
primeape_sound = pygame.mixer.Sound("057.wav")
growlithe_sound = pygame.mixer.Sound("058.wav")
arcanine_sound = pygame.mixer.Sound("059.wav")
poliwag_sound = pygame.mixer.Sound("060.wav")
poliwhirl_sound = pygame.mixer.Sound("061.wav")
poliwrath_sound = pygame.mixer.Sound("062.wav")
abra_sound = pygame.mixer.Sound("063.wav")
kadabra_sound = pygame.mixer.Sound("064.wav")
alakazam_sound = pygame.mixer.Sound("065.wav")
machop_sound = pygame.mixer.Sound("066.wav")
machoke_sound = pygame.mixer.Sound("067.wav")
machamp_sound = pygame.mixer.Sound("068.wav")
bellsprout_sound = pygame.mixer.Sound("069.wav")
weepinbell_sound = pygame.mixer.Sound("070.wav")
victreebel_sound = pygame.mixer.Sound("071.wav")
tentacool_sound = pygame.mixer.Sound("072.wav")
tentacruel_sound = pygame.mixer.Sound("073.wav")
geodude_sound = pygame.mixer.Sound("074.wav")
graveler_sound = pygame.mixer.Sound("075.wav")
golem_sound = pygame.mixer.Sound("076.wav")
ponyta_sound = pygame.mixer.Sound("077.wav")
rapidash_sound = pygame.mixer.Sound("078.wav")
slowpoke_sound = pygame.mixer.Sound("079.wav")
slowbro_sound = pygame.mixer.Sound("080.wav")
magnemite_sound = pygame.mixer.Sound("081.wav")
magneton_sound = pygame.mixer.Sound("082.wav")
farfetchd_sound = pygame.mixer.Sound("083.wav")
doduo_sound = pygame.mixer.Sound("084.wav")
dodrio_sound = pygame.mixer.Sound("085.wav")
seel_sound = pygame.mixer.Sound("086.wav")
dewgong_sound = pygame.mixer.Sound("087.wav")
grimer_sound = pygame.mixer.Sound("088.wav")
muk_sound = pygame.mixer.Sound("089.wav")
shellder_sound = pygame.mixer.Sound("090.wav")
cloyster_sound = pygame.mixer.Sound("091.wav")
gastly_sound = pygame.mixer.Sound("092.wav")
haunter_sound = pygame.mixer.Sound("093.wav")
gengar_sound = pygame.mixer.Sound("094.wav")
onix_sound = pygame.mixer.Sound("095.wav")
drowzee_sound = pygame.mixer.Sound("096.wav")
hypno_sound = pygame.mixer.Sound("097.wav")
krabby_sound = pygame.mixer.Sound("098.wav")
kingler_sound = pygame.mixer.Sound("099.wav")
voltorb_sound = pygame.mixer.Sound("100.wav")
electrode_sound = pygame.mixer.Sound("101.wav")
exeggcute_sound = pygame.mixer.Sound("102.wav")
exeggutor_sound = pygame.mixer.Sound("103.wav")
cubone_sound = pygame.mixer.Sound("104.wav")
marowak_sound = pygame.mixer.Sound("105.wav")
hitmonlee_sound = pygame.mixer.Sound("106.wav")
hitmonchan_sound = pygame.mixer.Sound("107.wav")
lickitung_sound = pygame.mixer.Sound("108.wav")
koffing_sound = pygame.mixer.Sound("109.wav")
weezing_sound = pygame.mixer.Sound("110.wav")
rhyhorn_sound = pygame.mixer.Sound("111.wav")
rhydon_sound = pygame.mixer.Sound("112.wav")
chansey_sound = pygame.mixer.Sound("113.wav")
tangela_sound = pygame.mixer.Sound("114.wav")
kangaskhan_sound = pygame.mixer.Sound("115.wav")
horsea_sound = pygame.mixer.Sound("116.wav")
seadra_sound = pygame.mixer.Sound("117.wav")
goldeen_sound = pygame.mixer.Sound("118.wav")
seaking_sound = pygame.mixer.Sound("119.wav")
staryu_sound = pygame.mixer.Sound("120.wav")
starmie_sound = pygame.mixer.Sound("121.wav")
mr_mime_sound = pygame.mixer.Sound("122.wav")
scyther_sound = pygame.mixer.Sound("123.wav")
jynx_sound = pygame.mixer.Sound("124.wav")
electabuzz_sound = pygame.mixer.Sound("125.wav")
magmar_sound = pygame.mixer.Sound("126.wav")
pinsir_sound = pygame.mixer.Sound("127.wav")
tauros_sound = pygame.mixer.Sound("128.wav")
magikarp_sound = pygame.mixer.Sound("129.wav")
gyarados_sound = pygame.mixer.Sound("130.wav")
lapras_sound = pygame.mixer.Sound("131.wav")
ditto_sound = pygame.mixer.Sound("132.wav")
eevee_sound = pygame.mixer.Sound("133.wav")
vaporeon_sound = pygame.mixer.Sound("134.wav")
jolteon_sound = pygame.mixer.Sound("135.wav")
flareon_sound = pygame.mixer.Sound("136.wav")
porygon_sound = pygame.mixer.Sound("137.wav")
omanyte_sound = pygame.mixer.Sound("138.wav")
omastar_sound = pygame.mixer.Sound("139.wav")
kabuto_sound = pygame.mixer.Sound("140.wav")
kabutops_sound = pygame.mixer.Sound("141.wav")
aerodactyl_sound = pygame.mixer.Sound("142.wav")
snorlax_sound = pygame.mixer.Sound("143.wav")
articuno_sound = pygame.mixer.Sound("144.wav")
zapdos_sound = pygame.mixer.Sound("145.wav")
moltres_sound = pygame.mixer.Sound("146.wav")
dratini_sound = pygame.mixer.Sound("147.wav")
dragonair_sound = pygame.mixer.Sound("148.wav")
dragonite_sound = pygame.mixer.Sound("149.wav")
mewtwo_sound = pygame.mixer.Sound("150.wav")
mew_sound = pygame.mixer.Sound("151.wav")

bulbasaur = pygame.image.load("001-bulbasaur.png")
bulbasaur_back = pygame.image.load("001-bulbasaurback.png")
bulbasaur_shiny = pygame.image.load("001-bulbasaurshiny.png")
bulbasaur_shiny_back = pygame.image.load("001-bulbasaurshinyback.png")

ivysaur = pygame.image.load("002-ivysaur.png")
ivysaur_back = pygame.image.load("002-ivysaurback.png")
ivysaur_shiny = pygame.image.load("002-ivysaurshiny.png")
ivysaur_shiny_back = pygame.image.load("002-ivysaurshinyback.png")

venusaur = pygame.image.load("003-venusaur.png")
venusaur_back = pygame.image.load("003-venusaurback.png")
venusaur_shiny = pygame.image.load("003-venusaurshiny.png")
venusaur_shiny_back = pygame.image.load("003-venusaurshinyback.png")

charmander = pygame.image.load("004-charmander.png")
charmander_back = pygame.image.load("004-charmanderback.png")
charmander_shiny = pygame.image.load("004-charmandershiny.png")
charmander_shiny_back = pygame.image.load("004-charmandershinyback.png")

charmeleon = pygame.image.load("005-charmeleon.png")
charmeleon_back = pygame.image.load("005-charmeleonback.png")
charmeleon_shiny = pygame.image.load("005-charmeleonshiny.png")
charmeleon_shiny_back = pygame.image.load("005-charmeleonshinyback.png")

charizard = pygame.image.load("006-charizard.png")
charizard_back = pygame.image.load("006-charizardback.png")
charizard_shiny = pygame.image.load("006-charizardshiny.png")
charizard_shiny_back = pygame.image.load("006-charizardshinyback.png")

squirtle = pygame.image.load("007-squirtle.png")
squirtle_back = pygame.image.load("007-squirtleback.png")
squirtle_shiny = pygame.image.load("007-squirtleshiny.png")
squirtle_shiny_back = pygame.image.load("007-squirtleshinyback.png")

wartortle = pygame.image.load("008-wartortle.png")
wartortle_back = pygame.image.load("008-wartortleback.png")
wartortle_shiny = pygame.image.load("008-wartortleshiny.png")
wartortle_shiny_back = pygame.image.load("008-wartortleshinyback.png")

blastoise = pygame.image.load("009-blastoise.png")
blastoise_back = pygame.image.load("009-blastoiseback.png")
blastoise_shiny = pygame.image.load("009-blastoiseshiny.png")
blastoise_shiny_back = pygame.image.load("009-blastoiseshinyback.png")

caterpie = pygame.image.load("010-caterpie.png")
caterpie_back = pygame.image.load("010-caterpieback.png")
caterpie_shiny = pygame.image.load("010-caterpieshiny.png")
caterpie_shiny_back = pygame.image.load("010-caterpieshinyback.png")

metapod = pygame.image.load("011-metapod.png")
metapod_back = pygame.image.load("011-metapodback.png")
metapod_shiny = pygame.image.load("011-metapodshiny.png")
metapod_shiny_back = pygame.image.load("011-metapodshinyback.png")

butterfree = pygame.image.load("012-butterfree.png")
butterfree_back = pygame.image.load("012-butterfreeback.png")
butterfree_shiny = pygame.image.load("012-butterfreeshiny.png")
butterfree_shiny_back = pygame.image.load("012-butterfreeshinyback.png")

weedle = pygame.image.load("013-weedle.png")
weedle_back = pygame.image.load("013-weedleback.png")
weedle_shiny = pygame.image.load("013-weedleshiny.png")
weedle_shiny_back = pygame.image.load("013-weedleshinyback.png")

kakuna = pygame.image.load("014-kakuna.png")
kakuna_back = pygame.image.load("014-kakunaback.png")
kakuna_shiny = pygame.image.load("014-kakunashiny.png")
kakuna_shiny_back = pygame.image.load("014-kakunashinyback.png")

beedrill = pygame.image.load("015-beedrill.png")
beedrill_back = pygame.image.load("015-beedrillback.png")
beedrill_shiny = pygame.image.load("015-beedrillshiny.png")
beedrill_shiny_back = pygame.image.load("015-beedrillshinyback.png")

pidgey = pygame.image.load("016-pidgey.png")
pidgey_back = pygame.image.load("016-pidgeyback.png")
pidgey_shiny = pygame.image.load("016-pidgeyshiny.png")
pidgey_shiny_back = pygame.image.load("016-pidgeyshinyback.png")

pidgeotto = pygame.image.load("017-pidgeotto.png")
pidgeotto_back = pygame.image.load("017-pidgeottoback.png")
pidgeotto_shiny = pygame.image.load("017-pidgeottoshiny.png")
pidgeotto_shiny_back = pygame.image.load("017-pidgeottoshinyback.png")

pidgeot = pygame.image.load("018-pidgeot.png")
pidgeot_back = pygame.image.load("018-pidgeotback.png")
pidgeot_shiny = pygame.image.load("018-pidgeotshiny.png")
pidgeot_shiny_back = pygame.image.load("018-pidgeotshinyback.png")

rattata = pygame.image.load("019-rattata.png")
rattata_back = pygame.image.load("019-rattataback.png")
rattata_shiny = pygame.image.load("019-rattatashiny.png")
rattata_shiny_back = pygame.image.load("019-rattatashinyback.png")

raticate = pygame.image.load("020-raticate.png")
raticate_back = pygame.image.load("020-raticateback.png")
raticate_shiny = pygame.image.load("020-raticateshiny.png")
raticate_shiny_back = pygame.image.load("020-raticateshinyback.png")

spearow = pygame.image.load("021-spearow.png")
spearow_back = pygame.image.load("021-spearowback.png")
spearow_shiny = pygame.image.load("021-spearowshiny.png")
spearow_shiny_back = pygame.image.load("021-spearowshinyback.png")

fearow = pygame.image.load("022-fearow.png")
fearow_back = pygame.image.load("022-fearowback.png")
fearow_shiny = pygame.image.load("022-fearowshiny.png")
fearow_shiny_back = pygame.image.load("022-fearowshinyback.png")

ekans = pygame.image.load("023-ekans.png")
ekans_back = pygame.image.load("023-ekansback.png")
ekans_shiny = pygame.image.load("023-ekansshiny.png")
ekans_shiny_back = pygame.image.load("023-ekansshinyback.png")

arbok = pygame.image.load("024-arbok.png")
arbok_back = pygame.image.load("024-arbokback.png")
arbok_shiny = pygame.image.load("024-arbokshiny.png")
arbok_shiny_back = pygame.image.load("024-arbokshinyback.png")

pikachu = pygame.image.load("025-pikachu.png")
pikachu_back = pygame.image.load("025-pikachuback.png")
pikachu_shiny = pygame.image.load("025-pikachushiny.png")
pikachu_shiny_back = pygame.image.load("025-pikachushinyback.png")

raichu = pygame.image.load("026-raichu.png")
raichu_back = pygame.image.load("026-raichuback.png")
raichu_shiny = pygame.image.load("026-raichushiny.png")
raichu_shiny_back = pygame.image.load("026-raichushinyback.png")

sandshrew = pygame.image.load("027-sandshrew.png")
sandshrew_back = pygame.image.load("027-sandshrewback.png")
sandshrew_shiny = pygame.image.load("027-sandshrewshiny.png")
sandshrew_shiny_back = pygame.image.load("027-sandshrewshinyback.png")

sandslash = pygame.image.load("028-sandslash.png")
sandslash_back = pygame.image.load("028-sandslashback.png")
sandslash_shiny = pygame.image.load("028-sandslashshiny.png")
sandslash_shiny_back = pygame.image.load("028-sandslashshinyback.png")

nidoranf = pygame.image.load("029-nidoranf.png")
nidoranf_back = pygame.image.load("029-nidoranfback.png")
nidoranf_shiny = pygame.image.load("029-nidoranfshiny.png")
nidoranf_shiny_back = pygame.image.load("029-nidoranfshinyback.png")

nidorina = pygame.image.load("030-nidorina.png")
nidorina_back = pygame.image.load("030-nidorinaback.png")
nidorina_shiny = pygame.image.load("030-nidorinashiny.png")
nidorina_shiny_back = pygame.image.load("030-nidorinashinyback.png")

nidoqueen = pygame.image.load("031-nidoqueen.png")
nidoqueen_back = pygame.image.load("031-nidoqueenback.png")
nidoqueen_shiny = pygame.image.load("031-nidoqueenshiny.png")
nidoqueen_shiny_back = pygame.image.load("031-nidoqueenshinyback.png")

nidoranm = pygame.image.load("032-nidoranm.png")
nidoranm_back = pygame.image.load("032-nidoranmback.png")
nidoranm_shiny = pygame.image.load("032-nidoranmshiny.png")
nidoranm_shiny_back = pygame.image.load("032-nidoranmshinyback.png")

nidorino = pygame.image.load("033-nidorino.png")
nidorino_back = pygame.image.load("033-nidorinoback.png")
nidorino_shiny = pygame.image.load("033-nidorinoshiny.png")
nidorino_shiny_back = pygame.image.load("033-nidorinoshinyback.png")

nidoking = pygame.image.load("034-nidoking.png")
nidoking_back = pygame.image.load("034-nidokingback.png")
nidoking_shiny = pygame.image.load("034-nidokingshiny.png")
nidoking_shiny_back = pygame.image.load("034-nidokingshinyback.png")

clefairy = pygame.image.load("035-clefairy.png")
clefairy_back = pygame.image.load("035-clefairyback.png")
clefairy_shiny = pygame.image.load("035-clefairyshiny.png")
clefairy_shiny_back = pygame.image.load("035-clefairyshinyback.png")

clefable = pygame.image.load("036-clefable.png")
clefable_back = pygame.image.load("036-clefableback.png")
clefable_shiny = pygame.image.load("036-clefableshiny.png")
clefable_shiny_back = pygame.image.load("036-clefableshinyback.png")

vulpix = pygame.image.load("037-vulpix.png")
vulpix_back = pygame.image.load("037-vulpixback.png")
vulpix_shiny = pygame.image.load("037-vulpixshiny.png")
vulpix_shiny_back = pygame.image.load("037-vulpixshinyback.png")

ninetales = pygame.image.load("038-ninetales.png")
ninetales_back = pygame.image.load("038-ninetalesback.png")
ninetales_shiny = pygame.image.load("038-ninetalesshiny.png")
ninetales_shiny_back = pygame.image.load("038-ninetalesshinyback.png")

jigglypuff = pygame.image.load("039-jigglypuff.png")
jigglypuff_back = pygame.image.load("039-jigglypuffback.png")
jigglypuff_shiny = pygame.image.load("039-jigglypuffshiny.png")
jigglypuff_shiny_back = pygame.image.load("039-jigglypuffshinyback.png")

wigglytuff = pygame.image.load("040-wigglytuff.png")
wigglytuff_back = pygame.image.load("040-wigglytuffback.png")
wigglytuff_shiny = pygame.image.load("040-wigglytuffshiny.png")
wigglytuff_shiny_back = pygame.image.load("040-wigglytuffshinyback.png")

zubat = pygame.image.load("041-zubat.png")
zubat_back = pygame.image.load("041-zubatback.png")
zubat_shiny = pygame.image.load("041-zubatshiny.png")
zubat_shiny_back = pygame.image.load("041-zubatshinyback.png")

golbat = pygame.image.load("042-golbat.png")
golbat_back = pygame.image.load("042-golbatback.png")
golbat_shiny = pygame.image.load("042-golbatshiny.png")
golbat_shiny_back = pygame.image.load("042-golbatshinyback.png")

oddish = pygame.image.load("043-oddish.png")
oddish_back = pygame.image.load("043-oddishback.png")
oddish_shiny = pygame.image.load("043-oddishshiny.png")
oddish_shiny_back = pygame.image.load("043-oddishshinyback.png")

gloom = pygame.image.load("044-gloom.png")
gloom_back = pygame.image.load("044-gloomback.png")
gloom_shiny = pygame.image.load("044-gloomshiny.png")
gloom_shiny_back = pygame.image.load("044-gloomshinyback.png")

vileplume = pygame.image.load("045-vileplume.png")
vileplume_back = pygame.image.load("045-vileplumeback.png")
vileplume_shiny = pygame.image.load("045-vileplumeshiny.png")
vileplume_shiny_back = pygame.image.load("045-vileplumeshinyback.png")

paras = pygame.image.load("046-paras.png")
paras_back = pygame.image.load("046-parasback.png")
paras_shiny = pygame.image.load("046-parasshiny.png")
paras_shiny_back = pygame.image.load("046-parasshinyback.png")

parasect = pygame.image.load("047-parasect.png")
parasect_back = pygame.image.load("047-parasectback.png")
parasect_shiny = pygame.image.load("047-parasectshiny.png")
parasect_shiny_back = pygame.image.load("047-parasectshinyback.png")

venonat = pygame.image.load("048-venonat.png")
venonat_back = pygame.image.load("048-venonatback.png")
venonat_shiny = pygame.image.load("048-venonatshiny.png")
venonat_shiny_back = pygame.image.load("048-venonatshinyback.png")

venomoth = pygame.image.load("049-venomoth.png")
venomoth_back = pygame.image.load("049-venomothback.png")
venomoth_shiny = pygame.image.load("049-venomothshiny.png")
venomoth_shiny_back = pygame.image.load("049-venomothshinyback.png")

diglett = pygame.image.load("050-diglett.png")
diglett_back = pygame.image.load("050-diglettback.png")
diglett_shiny = pygame.image.load("050-diglettshiny.png")
diglett_shiny_back = pygame.image.load("050-diglettshinyback.png")

dugtrio = pygame.image.load("051-dugtrio.png")
dugtrio_back = pygame.image.load("051-dugtrioback.png")
dugtrio_shiny = pygame.image.load("051-dugtrioshiny.png")
dugtrio_shiny_back = pygame.image.load("051-dugtrioshinyback.png")

meowth = pygame.image.load("052-meowth.png")
meowth_back = pygame.image.load("052-meowthback.png")
meowth_shiny = pygame.image.load("052-meowthshiny.png")
meowth_shiny_back = pygame.image.load("052-meowthshinyback.png")

persian = pygame.image.load("053-persian.png")
persian_back = pygame.image.load("053-persianback.png")
persian_shiny = pygame.image.load("053-persianshiny.png")
persian_shiny_back = pygame.image.load("053-persianshinyback.png")

psyduck = pygame.image.load("054-psyduck.png")
psyduck_back = pygame.image.load("054-psyduckback.png")
psyduck_shiny = pygame.image.load("054-psyduckshiny.png")
psyduck_shiny_back = pygame.image.load("054-psyduckshinyback.png")

golduck = pygame.image.load("055-golduck.png")
golduck_back = pygame.image.load("055-golduckback.png")
golduck_shiny = pygame.image.load("055-golduckshiny.png")
golduck_shiny_back = pygame.image.load("055-golduckshinyback.png")

mankey = pygame.image.load("056-mankey.png")
mankey_back = pygame.image.load("056-mankeyback.png")
mankey_shiny = pygame.image.load("056-mankeyshiny.png")
mankey_shiny_back = pygame.image.load("056-mankeyshinyback.png")

primeape = pygame.image.load("057-primeape.png")
primeape_back = pygame.image.load("057-primeapeback.png")
primeape_shiny = pygame.image.load("057-primeapeshiny.png")
primeape_shiny_back = pygame.image.load("057-primeapeshinyback.png")

growlithe = pygame.image.load("058-growlithe.png")
growlithe_back = pygame.image.load("058-growlitheback.png")
growlithe_shiny = pygame.image.load("058-growlitheshiny.png")
growlithe_shiny_back = pygame.image.load("058-growlitheshinyback.png")

arcanine = pygame.image.load("059-arcanine.png")
arcanine_back = pygame.image.load("059-arcanineback.png")
arcanine_shiny = pygame.image.load("059-arcanineshiny.png")
arcanine_shiny_back = pygame.image.load("059-arcanineshinyback.png")

poliwag = pygame.image.load("060-poliwag.png")
poliwag_back = pygame.image.load("060-poliwagback.png")
poliwag_shiny = pygame.image.load("060-poliwagshiny.png")
poliwag_shiny_back = pygame.image.load("060-poliwagshinyback.png")

poliwhirl = pygame.image.load("061-poliwhirl.png")
poliwhirl_back = pygame.image.load("061-poliwhirlback.png")
poliwhirl_shiny = pygame.image.load("061-poliwhirlshiny.png")
poliwhirl_shiny_back = pygame.image.load("061-poliwhirlshinyback.png")

poliwrath = pygame.image.load("062-poliwrath.png")
poliwrath_back = pygame.image.load("062-poliwrathback.png")
poliwrath_shiny = pygame.image.load("062-poliwrathshiny.png")
poliwrath_shiny_back = pygame.image.load("062-poliwrathshinyback.png")

abra = pygame.image.load("063-abra.png")
abra_back = pygame.image.load("063-abraback.png")
abra_shiny = pygame.image.load("063-abrashiny.png")
abra_shiny_back = pygame.image.load("063-abrashinyback.png")

kadabra = pygame.image.load("064-kadabra.png")
kadabra_back = pygame.image.load("064-kadabraback.png")
kadabra_shiny = pygame.image.load("064-kadabrashiny.png")
kadabra_shiny_back = pygame.image.load("064-kadabrashinyback.png")

alakazam = pygame.image.load("065-alakazam.png")
alakazam_back = pygame.image.load("065-alakazamback.png")
alakazam_shiny = pygame.image.load("065-alakazamshiny.png")
alakazam_shiny_back = pygame.image.load("065-alakazamshinyback.png")

machop = pygame.image.load("066-machop.png")
machop_back = pygame.image.load("066-machopback.png")
machop_shiny = pygame.image.load("066-machopshiny.png")
machop_shiny_back = pygame.image.load("066-machopshinyback.png")

machoke = pygame.image.load("067-machoke.png")
machoke_back = pygame.image.load("067-machokeback.png")
machoke_shiny = pygame.image.load("067-machokeshiny.png")
machoke_shiny_back = pygame.image.load("067-machokeshinyback.png")

machamp = pygame.image.load("068-machamp.png")
machamp_back = pygame.image.load("068-machampback.png")
machamp_shiny = pygame.image.load("068-machampshiny.png")
machamp_shiny_back = pygame.image.load("068-machampshinyback.png")

bellsprout = pygame.image.load("069-bellsprout.png")
bellsprout_back = pygame.image.load("069-bellsproutback.png")
bellsprout_shiny = pygame.image.load("069-bellsproutshiny.png")
bellsprout_shiny_back = pygame.image.load("069-bellsproutshinyback.png")

weepinbell = pygame.image.load("070-weepinbell.png")
weepinbell_back = pygame.image.load("070-weepinbellback.png")
weepinbell_shiny = pygame.image.load("070-weepinbellshiny.png")
weepinbell_shiny_back = pygame.image.load("070-weepinbellshinyback.png")

victreebel = pygame.image.load("071-victreebel.png")
victreebel_back = pygame.image.load("071-victreebelback.png")
victreebel_shiny = pygame.image.load("071-victreebelshiny.png")
victreebel_shiny_back = pygame.image.load("071-victreebelshinyback.png")

tentacool = pygame.image.load("072-tentacool.png")
tentacool_back = pygame.image.load("072-tentacoolback.png")
tentacool_shiny = pygame.image.load("072-tentacoolshiny.png")
tentacool_shiny_back = pygame.image.load("072-tentacoolshinyback.png")

tentacruel = pygame.image.load("073-tentacruel.png")
tentacruel_back = pygame.image.load("073-tentacruelback.png")
tentacruel_shiny = pygame.image.load("073-tentacruelshiny.png")
tentacruel_shiny_back = pygame.image.load("073-tentacruelshinyback.png")

geodude = pygame.image.load("074-geodude.png")
geodude_back = pygame.image.load("074-geodudeback.png")
geodude_shiny = pygame.image.load("074-geodudeshiny.png")
geodude_shiny_back = pygame.image.load("074-geodudeshinyback.png")

graveler = pygame.image.load("075-graveler.png")
graveler_back = pygame.image.load("075-gravelerback.png")
graveler_shiny = pygame.image.load("075-gravelershiny.png")
graveler_shiny_back = pygame.image.load("075-gravelershinyback.png")

golem = pygame.image.load("076-golem.png")
golem_back = pygame.image.load("076-golemback.png")
golem_shiny = pygame.image.load("076-golemshiny.png")
golem_shiny_back = pygame.image.load("076-golemshinyback.png")

ponyta = pygame.image.load("077-ponyta.png")
ponyta_back = pygame.image.load("077-ponytaback.png")
ponyta_shiny = pygame.image.load("077-ponytashiny.png")
ponyta_shiny_back = pygame.image.load("077-ponytashinyback.png")

rapidash = pygame.image.load("078-rapidash.png")
rapidash_back = pygame.image.load("078-rapidashback.png")
rapidash_shiny = pygame.image.load("078-rapidashshiny.png")
rapidash_shiny_back = pygame.image.load("078-rapidashshinyback.png")

slowpoke = pygame.image.load("079-slowpoke.png")
slowpoke_back = pygame.image.load("079-slowpokeback.png")
slowpoke_shiny = pygame.image.load("079-slowpokeshiny.png")
slowpoke_shiny_back = pygame.image.load("079-slowpokeshinyback.png")

slowbro = pygame.image.load("080-slowbro.png")
slowbro_back = pygame.image.load("080-slowbroback.png")
slowbro_shiny = pygame.image.load("080-slowbroshiny.png")
slowbro_shiny_back = pygame.image.load("080-slowbroshinyback.png")

magnemite = pygame.image.load("081-magnemite.png")
magnemite_back = pygame.image.load("081-magnemiteback.png")
magnemite_shiny = pygame.image.load("081-magnemiteshiny.png")
magnemite_shiny_back = pygame.image.load("081-magnemiteshinyback.png")

magneton = pygame.image.load("082-magneton.png")
magneton_back = pygame.image.load("082-magnetonback.png")
magneton_shiny = pygame.image.load("082-magnetonshiny.png")
magneton_shiny_back = pygame.image.load("082-magnetonshinyback.png")

farfetchd = pygame.image.load("083-farfetchd.png")
farfetchd_back = pygame.image.load("083-farfetchdback.png")
farfetchd_shiny = pygame.image.load("083-farfetchdshiny.png")
farfetchd_shiny_back = pygame.image.load("083-farfetchdshinyback.png")

doduo = pygame.image.load("084-doduo.png")
doduo_back = pygame.image.load("084-doduoback.png")
doduo_shiny = pygame.image.load("084-doduoshiny.png")
doduo_shiny_back = pygame.image.load("084-doduoshinyback.png")

dodrio = pygame.image.load("085-dodrio.png")
dodrio_back = pygame.image.load("085-dodrioback.png")
dodrio_shiny = pygame.image.load("085-dodrioshiny.png")
dodrio_shiny_back = pygame.image.load("085-dodrioshinyback.png")

seel = pygame.image.load("086-seel.png")
seel_back = pygame.image.load("086-seelback.png")
seel_shiny = pygame.image.load("086-seelshiny.png")
seel_shiny_back = pygame.image.load("086-seelshinyback.png")

dewgong = pygame.image.load("087-dewgong.png")
dewgong_back = pygame.image.load("087-dewgongback.png")
dewgong_shiny = pygame.image.load("087-dewgongshiny.png")
dewgong_shiny_back = pygame.image.load("087-dewgongshinyback.png")

grimer = pygame.image.load("088-grimer.png")
grimer_back = pygame.image.load("088-grimerback.png")
grimer_shiny = pygame.image.load("088-grimershiny.png")
grimer_shiny_back = pygame.image.load("088-grimershinyback.png")

muk = pygame.image.load("089-muk.png")
muk_back = pygame.image.load("089-mukback.png")
muk_shiny = pygame.image.load("089-mukshiny.png")
muk_shiny_back = pygame.image.load("089-mukshinyback.png")

shellder = pygame.image.load("090-shellder.png")
shellder_back = pygame.image.load("090-shellderback.png")
shellder_shiny = pygame.image.load("090-shelldershiny.png")
shellder_shiny_back = pygame.image.load("090-shelldershinyback.png")

cloyster = pygame.image.load("091-cloyster.png")
cloyster_back = pygame.image.load("091-cloysterback.png")
cloyster_shiny = pygame.image.load("091-cloystershiny.png")
cloyster_shiny_back = pygame.image.load("091-cloystershinyback.png")

gastly = pygame.image.load("092-gastly.png")
gastly_back = pygame.image.load("092-gastlyback.png")
gastly_shiny = pygame.image.load("092-gastlyshiny.png")
gastly_shiny_back = pygame.image.load("092-gastlyshinyback.png")

haunter = pygame.image.load("093-haunter.png")
haunter_back = pygame.image.load("093-haunterback.png")
haunter_shiny = pygame.image.load("093-hauntershiny.png")
haunter_shiny_back = pygame.image.load("093-hauntershinyback.png")

gengar = pygame.image.load("094-gengar.png")
gengar_back = pygame.image.load("094-gengarback.png")
gengar_shiny = pygame.image.load("094-gengarshiny.png")
gengar_shiny_back = pygame.image.load("094-gengarshinyback.png")

onix = pygame.image.load("095-onix.png")
onix_back = pygame.image.load("095-onixback.png")
onix_shiny = pygame.image.load("095-onixshiny.png")
onix_shiny_back = pygame.image.load("095-onixshinyback.png")

drowzee = pygame.image.load("096-drowzee.png")
drowzee_back = pygame.image.load("096-drowzeeback.png")
drowzee_shiny = pygame.image.load("096-drowzeeshiny.png")
drowzee_shiny_back = pygame.image.load("096-drowzeeshinyback.png")

hypno = pygame.image.load("097-hypno.png")
hypno_back = pygame.image.load("097-hypnoback.png")
hypno_shiny = pygame.image.load("097-hypnoshiny.png")
hypno_shiny_back = pygame.image.load("097-hypnoshinyback.png")

krabby = pygame.image.load("098-krabby.png")
krabby_back = pygame.image.load("098-krabbyback.png")
krabby_shiny = pygame.image.load("098-krabbyshiny.png")
krabby_shiny_back = pygame.image.load("098-krabbyshinyback.png")

kingler = pygame.image.load("099-kingler.png")
kingler_back = pygame.image.load("099-kinglerback.png")
kingler_shiny = pygame.image.load("099-kinglershiny.png")
kingler_shiny_back = pygame.image.load("099-kinglershinyback.png")

voltorb = pygame.image.load("100-voltorb.png")
voltorb_back = pygame.image.load("100-voltorbback.png")
voltorb_shiny = pygame.image.load("100-voltorbshiny.png")
voltorb_shiny_back = pygame.image.load("100-voltorbshinyback.png")

electrode = pygame.image.load("101-electrode.png")
electrode_back = pygame.image.load("101-electrodeback.png")
electrode_shiny = pygame.image.load("101-electrodeshiny.png")
electrode_shiny_back = pygame.image.load("101-electrodeshinyback.png")

exeggcute = pygame.image.load("102-exeggcute.png")
exeggcute_back = pygame.image.load("102-exeggcuteback.png")
exeggcute_shiny = pygame.image.load("102-exeggcuteshiny.png")
exeggcute_shiny_back = pygame.image.load("102-exeggcuteshinyback.png")

exeggutor = pygame.image.load("103-exeggutor.png")
exeggutor_back = pygame.image.load("103-exeggutorback.png")
exeggutor_shiny = pygame.image.load("103-exeggutorshiny.png")
exeggutor_shiny_back = pygame.image.load("103-exeggutorshinyback.png")

cubone = pygame.image.load("104-cubone.png")
cubone_back = pygame.image.load("104-cuboneback.png")
cubone_shiny = pygame.image.load("104-cuboneshiny.png")
cubone_shiny_back = pygame.image.load("104-cuboneshinyback.png")

marowak = pygame.image.load("105-marowak.png")
marowak_back = pygame.image.load("105-marowakback.png")
marowak_shiny = pygame.image.load("105-marowakshiny.png")
marowak_shiny_back = pygame.image.load("105-marowakshinyback.png")

hitmonlee = pygame.image.load("106-hitmonlee.png")
hitmonlee_back = pygame.image.load("106-hitmonleeback.png")
hitmonlee_shiny = pygame.image.load("106-hitmonleeshiny.png")
hitmonlee_shiny_back = pygame.image.load("106-hitmonleeshinyback.png")

hitmonchan = pygame.image.load("107-hitmonchan.png")
hitmonchan_back = pygame.image.load("107-hitmonchanback.png")
hitmonchan_shiny = pygame.image.load("107-hitmonchanshiny.png")
hitmonchan_shiny_back = pygame.image.load("107-hitmonchanshinyback.png")

lickitung = pygame.image.load("108-lickitung.png")
lickitung_back = pygame.image.load("108-lickitungback.png")
lickitung_shiny = pygame.image.load("108-lickitungshiny.png")
lickitung_shiny_back = pygame.image.load("108-lickitungshinyback.png")

koffing = pygame.image.load("109-koffing.png")
koffing_back = pygame.image.load("109-koffingback.png")
koffing_shiny = pygame.image.load("109-koffingshiny.png")
koffing_shiny_back = pygame.image.load("109-koffingshinyback.png")

weezing = pygame.image.load("110-weezing.png")
weezing_back = pygame.image.load("110-weezingback.png")
weezing_shiny = pygame.image.load("110-weezingshiny.png")
weezing_shiny_back = pygame.image.load("110-weezingshinyback.png")

rhyhorn = pygame.image.load("111-rhyhorn.png")
rhyhorn_back = pygame.image.load("111-rhyhornback.png")
rhyhorn_shiny = pygame.image.load("111-rhyhornshiny.png")
rhyhorn_shiny_back = pygame.image.load("111-rhyhornshinyback.png")

rhydon = pygame.image.load("112-rhydon.png")
rhydon_back = pygame.image.load("112-rhydonback.png")
rhydon_shiny = pygame.image.load("112-rhydonshiny.png")
rhydon_shiny_back = pygame.image.load("112-rhydonshinyback.png")

chansey = pygame.image.load("113-chansey.png")
chansey_back = pygame.image.load("113-chanseyback.png")
chansey_shiny = pygame.image.load("113-chanseyshiny.png")
chansey_shiny_back = pygame.image.load("113-chanseyshinyback.png")

tangela = pygame.image.load("114-tangela.png")
tangela_back = pygame.image.load("114-tangelaback.png")
tangela_shiny = pygame.image.load("114-tangelashiny.png")
tangela_shiny_back = pygame.image.load("114-tangelashinyback.png")

kangaskhan = pygame.image.load("115-kangaskhan.png")
kangaskhan_back = pygame.image.load("115-kangaskhanback.png")
kangaskhan_shiny = pygame.image.load("115-kangaskhanshiny.png")
kangaskhan_shiny_back = pygame.image.load("115-kangaskhanshinyback.png")

horsea = pygame.image.load("116-horsea.png")
horsea_back = pygame.image.load("116-horseaback.png")
horsea_shiny = pygame.image.load("116-horseashiny.png")
horsea_shiny_back = pygame.image.load("116-horseashinyback.png")

seadra = pygame.image.load("117-seadra.png")
seadra_back = pygame.image.load("117-seadraback.png")
seadra_shiny = pygame.image.load("117-seadrashiny.png")
seadra_shiny_back = pygame.image.load("117-seadrashinyback.png")

goldeen = pygame.image.load("118-goldeen.png")
goldeen_back = pygame.image.load("118-goldeenback.png")
goldeen_shiny = pygame.image.load("118-goldeenshiny.png")
goldeen_shiny_back = pygame.image.load("118-goldeenshinyback.png")

seaking = pygame.image.load("119-seaking.png")
seaking_back = pygame.image.load("119-seakingback.png")
seaking_shiny = pygame.image.load("119-seakingshiny.png")
seaking_shiny_back = pygame.image.load("119-seakingshinyback.png")

staryu = pygame.image.load("120-staryu.png")
staryu_back = pygame.image.load("120-staryuback.png")
staryu_shiny = pygame.image.load("120-staryushiny.png")
staryu_shiny_back = pygame.image.load("120-staryushinyback.png")

starmie = pygame.image.load("121-starmie.png")
starmie_back = pygame.image.load("121-starmieback.png")
starmie_shiny = pygame.image.load("121-starmieshiny.png")
starmie_shiny_back = pygame.image.load("121-starmieshinyback.png")

mr_mime = pygame.image.load("122-mr_mime.png")
mr_mime_back = pygame.image.load("122-mr_mimeback.png")
mr_mime_shiny = pygame.image.load("122-mr_mimeshiny.png")
mr_mime_shiny_back = pygame.image.load("122-mr_mimeshinyback.png")

scyther = pygame.image.load("123-scyther.png")
scyther_back = pygame.image.load("123-scytherback.png")
scyther_shiny = pygame.image.load("123-scythershiny.png")
scyther_shiny_back = pygame.image.load("123-scythershinyback.png")

jynx = pygame.image.load("124-jynx.png")
jynx_back = pygame.image.load("124-jynxback.png")
jynx_shiny = pygame.image.load("124-jynxshiny.png")
jynx_shiny_back = pygame.image.load("124-jynxshinyback.png")

electabuzz = pygame.image.load("125-electabuzz.png")
electabuzz_back = pygame.image.load("125-electabuzzback.png")
electabuzz_shiny = pygame.image.load("125-electabuzzshiny.png")
electabuzz_shiny_back = pygame.image.load("125-electabuzzshinyback.png")

magmar = pygame.image.load("126-magmar.png")
magmar_back = pygame.image.load("126-magmarback.png")
magmar_shiny = pygame.image.load("126-magmarshiny.png")
magmar_shiny_back = pygame.image.load("126-magmarshinyback.png")

pinsir = pygame.image.load("127-pinsir.png")
pinsir_back = pygame.image.load("127-pinsirback.png")
pinsir_shiny = pygame.image.load("127-pinsirshiny.png")
pinsir_shiny_back = pygame.image.load("127-pinsirshinyback.png")

tauros = pygame.image.load("128-tauros.png")
tauros_back = pygame.image.load("128-taurosback.png")
tauros_shiny = pygame.image.load("128-taurosshiny.png")
tauros_shiny_back = pygame.image.load("128-taurosshinyback.png")

magikarp = pygame.image.load("129-magikarp.png")
magikarp_back = pygame.image.load("129-magikarpback.png")
magikarp_shiny = pygame.image.load("129-magikarpshiny.png")
magikarp_shiny_back = pygame.image.load("129-magikarpshinyback.png")

gyarados = pygame.image.load("130-gyarados.png")
gyarados_back = pygame.image.load("130-gyaradosback.png")
gyarados_shiny = pygame.image.load("130-gyaradosshiny.png")
gyarados_shiny_back = pygame.image.load("130-gyaradosshinyback.png")

lapras = pygame.image.load("131-lapras.png")
lapras_back = pygame.image.load("131-laprasback.png")
lapras_shiny = pygame.image.load("131-laprasshiny.png")
lapras_shiny_back = pygame.image.load("131-laprasshinyback.png")

ditto = pygame.image.load("132-ditto.png")
ditto_back = pygame.image.load("132-dittoback.png")
ditto_shiny = pygame.image.load("132-dittoshiny.png")
ditto_shiny_back = pygame.image.load("132-dittoshinyback.png")

eevee = pygame.image.load("133-eevee.png")
eevee_back = pygame.image.load("133-eeveeback.png")
eevee_shiny = pygame.image.load("133-eeveeshiny.png")
eevee_shiny_back = pygame.image.load("133-eeveeshinyback.png")

vaporeon = pygame.image.load("134-vaporeon.png")
vaporeon_back = pygame.image.load("134-vaporeonback.png")
vaporeon_shiny = pygame.image.load("134-vaporeonshiny.png")
vaporeon_shiny_back = pygame.image.load("134-vaporeonshinyback.png")

jolteon = pygame.image.load("135-jolteon.png")
jolteon_back = pygame.image.load("135-jolteonback.png")
jolteon_shiny = pygame.image.load("135-jolteonshiny.png")
jolteon_shiny_back = pygame.image.load("135-jolteonshinyback.png")

flareon = pygame.image.load("136-flareon.png")
flareon_back = pygame.image.load("136-flareonback.png")
flareon_shiny = pygame.image.load("136-flareonshiny.png")
flareon_shiny_back = pygame.image.load("136-flareonshinyback.png")

porygon = pygame.image.load("137-porygon.png")
porygon_back = pygame.image.load("137-porygonback.png")
porygon_shiny = pygame.image.load("137-porygonshiny.png")
porygon_shiny_back = pygame.image.load("137-porygonshinyback.png")

omanyte = pygame.image.load("138-omanyte.png")
omanyte_back = pygame.image.load("138-omanyteback.png")
omanyte_shiny = pygame.image.load("138-omanyteshiny.png")
omanyte_shiny_back = pygame.image.load("138-omanyteshinyback.png")

omastar = pygame.image.load("139-omastar.png")
omastar_back = pygame.image.load("139-omastarback.png")
omastar_shiny = pygame.image.load("139-omastarshiny.png")
omastar_shiny_back = pygame.image.load("139-omastarshinyback.png")

kabuto = pygame.image.load("140-kabuto.png")
kabuto_back = pygame.image.load("140-kabutoback.png")
kabuto_shiny = pygame.image.load("140-kabutoshiny.png")
kabuto_shiny_back = pygame.image.load("140-kabutoshinyback.png")

kabutops = pygame.image.load("141-kabutops.png")
kabutops_back = pygame.image.load("141-kabutopsback.png")
kabutops_shiny = pygame.image.load("141-kabutopsshiny.png")
kabutops_shiny_back = pygame.image.load("141-kabutopsshinyback.png")

aerodactyl = pygame.image.load("142-aerodactyl.png")
aerodactyl_back = pygame.image.load("142-aerodactylback.png")
aerodactyl_shiny = pygame.image.load("142-aerodactylshiny.png")
aerodactyl_shiny_back = pygame.image.load("142-aerodactylshinyback.png")

snorlax = pygame.image.load("143-snorlax.png")
snorlax_back = pygame.image.load("143-snorlaxback.png")
snorlax_shiny = pygame.image.load("143-snorlaxshiny.png")
snorlax_shiny_back = pygame.image.load("143-snorlaxshinyback.png")

articuno = pygame.image.load("144-articuno.png")
articuno_back = pygame.image.load("144-articunoback.png")
articuno_shiny = pygame.image.load("144-articunoshiny.png")
articuno_shiny_back = pygame.image.load("144-articunoshinyback.png")

zapdos = pygame.image.load("145-zapdos.png")
zapdos_back = pygame.image.load("145-zapdosback.png")
zapdos_shiny = pygame.image.load("145-zapdosshiny.png")
zapdos_shiny_back = pygame.image.load("145-zapdosshinyback.png")

moltres = pygame.image.load("146-moltres.png")
moltres_back = pygame.image.load("146-moltresback.png")
moltres_shiny = pygame.image.load("146-moltresshiny.png")
moltres_shiny_back = pygame.image.load("146-moltresshinyback.png")

dratini = pygame.image.load("147-dratini.png")
dratini_back = pygame.image.load("147-dratiniback.png")
dratini_shiny = pygame.image.load("147-dratinishiny.png")
dratini_shiny_back = pygame.image.load("147-dratinishinyback.png")

dragonair = pygame.image.load("148-dragonair.png")
dragonair_back = pygame.image.load("148-dragonairback.png")
dragonair_shiny = pygame.image.load("148-dragonairshiny.png")
dragonair_shiny_back = pygame.image.load("148-dragonairshinyback.png")

dragonite = pygame.image.load("149-dragonite.png")
dragonite_back = pygame.image.load("149-dragoniteback.png")
dragonite_shiny = pygame.image.load("149-dragoniteshiny.png")
dragonite_shiny_back = pygame.image.load("149-dragoniteshinyback.png")

mewtwo = pygame.image.load("150-mewtwo.png")
mewtwo_back = pygame.image.load("150-mewtwoback.png")
mewtwo_shiny = pygame.image.load("150-mewtwoshiny.png")
mewtwo_shiny_back = pygame.image.load("150-mewtwoshinyback.png")

mew = pygame.image.load("151-mew.png")
mew_back = pygame.image.load("151-mewback.png")
mew_shiny = pygame.image.load("151-mewshiny.png")
mew_shiny_back = pygame.image.load("151-mewshinyback.png")

Brock = pygame.image.load("Brock.png")
Misty = pygame.image.load("Misty.png")
Lt_Surge = pygame.image.load("Lt. Surge.png")
Erika = pygame.image.load("Erika.png")
Koga = pygame.image.load("Koga.png")
Sabrina = pygame.image.load("Sabrina.png")
Blaine = pygame.image.load("Blaine.png")
Giovanni = pygame.image.load("Giovanni.png")
Oscar = pygame.image.load("Youngster.png")
David = pygame.image.load("Bug Catcher.png")
Jonathan = pygame.image.load("Jonathan.png")
Champion = pygame.image.load("TA.png")

Prof_Oak = pygame.image.load("Prof Oak.png")


all_front_sprites = [bulbasaur, ivysaur, venusaur, charmander, charmeleon, charizard,
                     squirtle, wartortle, blastoise, caterpie, metapod, butterfree,
                     weedle, kakuna, beedrill, pidgey, pidgeotto, pidgeot, rattata,
                     raticate, spearow, fearow, ekans, arbok, pikachu, raichu,
                     sandshrew, sandslash, nidoranf, nidorina, nidoqueen, nidoranm,
                     nidorino, nidoking, clefairy, clefable, vulpix, ninetales,
                     jigglypuff, wigglytuff, zubat, golbat, oddish, gloom,
                     vileplume, paras, parasect, venonat, venomoth, diglett,
                     dugtrio, meowth, persian, psyduck, golduck, mankey, primeape,
                     growlithe, arcanine, poliwag, poliwhirl, poliwrath, abra,
                     kadabra, alakazam, machop, machoke, machamp, bellsprout,
                     weepinbell, victreebel, tentacool, tentacruel, geodude,
                     graveler, golem, ponyta, rapidash, slowpoke, slowbro,
                     magnemite, magneton, farfetchd, doduo, dodrio, seel, dewgong,
                     grimer, muk, shellder, cloyster, gastly, haunter, gengar, onix,
                     drowzee, hypno, krabby, kingler, voltorb, electrode, exeggcute,
                     exeggutor, cubone, marowak, hitmonlee, hitmonchan, lickitung,
                     koffing, weezing, rhyhorn, rhydon, chansey, tangela, kangaskhan,
                     horsea, seadra, goldeen, seaking, staryu, starmie, mr_mime,
                     scyther, jynx, electabuzz, magmar, pinsir, tauros, magikarp,
                     gyarados, lapras, ditto, eevee, vaporeon, jolteon, flareon,
                     porygon, omanyte, omastar, kabuto, kabutops, aerodactyl,
                     snorlax, articuno, zapdos, moltres, dratini, dragonair,
                     dragonite, mewtwo, mew]

all_back_sprites = [bulbasaur_back, ivysaur_back, venusaur_back, charmander_back, charmeleon_back, charizard_back,
                    squirtle_back, wartortle_back, blastoise_back, caterpie_back, metapod_back, butterfree_back,
                    weedle_back, kakuna_back, beedrill_back, pidgey_back, pidgeotto_back, pidgeot_back, rattata_back,
                    raticate_back, spearow_back, fearow_back, ekans_back, arbok_back, pikachu_back, raichu_back,
                    sandshrew_back, sandslash_back, nidoranf_back, nidorina_back, nidoqueen_back, nidoranm_back,
                    nidorino_back, nidoking_back, clefairy_back, clefable_back, vulpix_back, ninetales_back,
                    jigglypuff_back, wigglytuff_back, zubat_back, golbat_back, oddish_back, gloom_back,
                    vileplume_back, paras_back, parasect_back, venonat_back, venomoth_back, diglett_back,
                    dugtrio_back, meowth_back, persian_back, psyduck_back, golduck_back, mankey_back, primeape_back,
                    growlithe_back, arcanine_back, poliwag_back, poliwhirl_back, poliwrath_back, abra_back,
                    kadabra_back, alakazam_back, machop_back, machoke_back, machamp_back, bellsprout_back,
                    weepinbell_back, victreebel_back, tentacool_back, tentacruel_back, geodude_back,
                    graveler_back, golem_back, ponyta_back, rapidash_back, slowpoke_back, slowbro_back,
                    magnemite_back, magneton_back, farfetchd_back, doduo_back, dodrio_back, seel_back, dewgong_back,
                    grimer_back, muk_back, shellder_back, cloyster_back, gastly_back, haunter_back, gengar_back, onix_back,
                    drowzee_back, hypno_back, krabby_back, kingler_back, voltorb_back, electrode_back, exeggcute_back,
                    exeggutor_back, cubone_back, marowak_back, hitmonlee_back, hitmonchan_back, lickitung_back,
                    koffing_back, weezing_back, rhyhorn_back, rhydon_back, chansey_back, tangela_back, kangaskhan_back,
                    horsea_back, seadra_back, goldeen_back, seaking_back, staryu_back, starmie_back, mr_mime_back,
                    scyther_back, jynx_back, electabuzz_back, magmar_back, pinsir_back, tauros_back, magikarp_back,
                    gyarados_back, lapras_back, ditto_back, eevee_back, vaporeon_back, jolteon_back, flareon_back,
                    porygon_back, omanyte_back, omastar_back, kabuto_back, kabutops_back, aerodactyl_back,
                    snorlax_back, articuno_back, zapdos_back, moltres_back, dratini_back, dragonair_back,
                    dragonite_back, mewtwo_back, mew_back]

all_shinyfront_sprites = [bulbasaur_shiny, ivysaur_shiny, venusaur_shiny, charmander_shiny, charmeleon_shiny, charizard_shiny,
                         squirtle_shiny, wartortle_shiny, blastoise_shiny, caterpie_shiny, metapod_shiny, butterfree_shiny,
                         weedle_shiny, kakuna_shiny, beedrill_shiny, pidgey_shiny, pidgeotto_shiny, pidgeot_shiny, rattata_shiny,
                         raticate_shiny, spearow_shiny, fearow_shiny, ekans_shiny, arbok_shiny, pikachu_shiny, raichu_shiny,
                         sandshrew_shiny, sandslash_shiny, nidoranf_shiny, nidorina_shiny, nidoqueen_shiny, nidoranm_shiny,
                         nidorino_shiny,nidoking_shiny, clefairy_shiny, clefable_shiny, vulpix_shiny, ninetales_shiny,
                         jigglypuff_shiny, wigglytuff_shiny, zubat_shiny, golbat_shiny, oddish_shiny, gloom_shiny,
                         vileplume_shiny, paras_shiny, parasect_shiny, venonat_shiny, venomoth_shiny, diglett_shiny,
                         dugtrio_shiny, meowth_shiny, persian_shiny, psyduck_shiny, golduck_shiny, mankey_shiny, primeape_shiny,
                         growlithe_shiny, arcanine_shiny, poliwag_shiny, poliwhirl_shiny, poliwrath_shiny, abra_shiny,
                         kadabra_shiny, alakazam_shiny, machop_shiny, machoke_shiny, machamp_shiny, bellsprout_shiny,
                         weepinbell_shiny, victreebel_shiny, tentacool_shiny, tentacruel_shiny, geodude_shiny,
                         graveler_shiny, golem_shiny, ponyta_shiny, rapidash_shiny, slowpoke_shiny, slowbro_shiny,
                         magnemite_shiny, magneton_shiny, farfetchd_shiny, doduo_shiny, dodrio_shiny, seel_shiny, dewgong_shiny,
                         grimer_shiny, muk_shiny, shellder_shiny, cloyster_shiny, gastly_shiny, haunter_shiny, gengar_shiny, onix_shiny,
                         drowzee_shiny, hypno_shiny, krabby_shiny, kingler_shiny, voltorb_shiny, electrode_shiny, exeggcute_shiny,
                         exeggutor_shiny, cubone_shiny, marowak_shiny, hitmonlee_shiny, hitmonchan_shiny, lickitung_shiny,
                         koffing_shiny, weezing_shiny, rhyhorn_shiny, rhydon_shiny, chansey_shiny, tangela_shiny, kangaskhan_shiny,
                         horsea_shiny, seadra_shiny, goldeen_shiny, seaking_shiny, staryu_shiny, starmie_shiny, mr_mime_shiny,
                         scyther_shiny, jynx_shiny, electabuzz_shiny, magmar_shiny, pinsir_shiny, tauros_shiny, magikarp_shiny,
                         gyarados_shiny, lapras_shiny, ditto_shiny, eevee_shiny, vaporeon_shiny, jolteon_shiny, flareon_shiny,
                         porygon_shiny, omanyte_shiny, omastar_shiny, kabuto_shiny, kabutops_shiny, aerodactyl_shiny,
                         snorlax_shiny, articuno_shiny, zapdos_shiny, moltres_shiny, dratini_shiny, dragonair_shiny,
                         dragonite_shiny, mewtwo_shiny, mew_shiny]

all_shinyback_sprites = [bulbasaur_shiny_back, ivysaur_shiny_back, venusaur_shiny_back, charmander_shiny_back, charmeleon_shiny_back, charizard_shiny_back,
                         squirtle_shiny_back, wartortle_shiny_back, blastoise_shiny_back, caterpie_shiny_back, metapod_shiny_back, butterfree_shiny_back,
                         weedle_shiny_back, kakuna_shiny_back, beedrill_shiny_back, pidgey_shiny_back, pidgeotto_shiny_back, pidgeot_shiny_back, rattata_shiny_back,
                         raticate_shiny_back, spearow_shiny_back, fearow_shiny_back, ekans_shiny_back, arbok_shiny_back, pikachu_shiny_back, raichu_shiny_back,
                         sandshrew_shiny_back, sandslash_shiny_back, nidoranf_shiny_back, nidorina_shiny_back, nidoqueen_shiny_back, nidoranm_shiny_back,
                         nidorino_shiny_back,nidoking_shiny_back, clefairy_shiny_back, clefable_shiny_back, vulpix_shiny_back, ninetales_shiny_back,
                         jigglypuff_shiny_back, wigglytuff_shiny_back, zubat_shiny_back, golbat_shiny_back, oddish_shiny_back, gloom_shiny_back,
                         vileplume_shiny_back, paras_shiny_back, parasect_shiny_back, venonat_shiny_back, venomoth_shiny_back, diglett_shiny_back,
                         dugtrio_shiny_back, meowth_shiny_back, persian_shiny_back, psyduck_shiny_back, golduck_shiny_back, mankey_shiny_back, primeape_shiny_back,
                         growlithe_shiny_back, arcanine_shiny_back, poliwag_shiny_back, poliwhirl_shiny_back, poliwrath_shiny_back, abra_shiny_back,
                         kadabra_shiny_back, alakazam_shiny_back, machop_shiny_back, machoke_shiny_back, machamp_shiny_back, bellsprout_shiny_back,
                         weepinbell_shiny_back, victreebel_shiny_back, tentacool_shiny_back, tentacruel_shiny_back, geodude_shiny_back,
                         graveler_shiny_back, golem_shiny_back, ponyta_shiny_back, rapidash_shiny_back, slowpoke_shiny_back, slowbro_shiny_back,
                         magnemite_shiny_back, magneton_shiny_back, farfetchd_shiny_back, doduo_shiny_back, dodrio_shiny_back, seel_shiny_back, dewgong_shiny_back,
                         grimer_shiny_back, muk_shiny_back, shellder_shiny_back, cloyster_shiny_back, gastly_shiny_back, haunter_shiny_back, gengar_shiny_back, onix_shiny_back,
                         drowzee_shiny_back, hypno_shiny_back, krabby_shiny_back, kingler_shiny_back, voltorb_shiny_back, electrode_shiny_back, exeggcute_shiny_back,
                         exeggutor_shiny_back, cubone_shiny_back, marowak_shiny_back, hitmonlee_shiny_back, hitmonchan_shiny_back, lickitung_shiny_back,
                         koffing_shiny_back, weezing_shiny_back, rhyhorn_shiny_back, rhydon_shiny_back, chansey_shiny_back, tangela_shiny_back, kangaskhan_shiny_back,
                         horsea_shiny_back, seadra_shiny_back, goldeen_shiny_back, seaking_shiny_back, staryu_shiny_back, starmie_shiny_back, mr_mime_shiny_back,
                         scyther_shiny_back, jynx_shiny_back, electabuzz_shiny_back, magmar_shiny_back, pinsir_shiny_back, tauros_shiny_back, magikarp_shiny_back,
                         gyarados_shiny_back, lapras_shiny_back, ditto_shiny_back, eevee_shiny_back, vaporeon_shiny_back, jolteon_shiny_back, flareon_shiny_back,
                         porygon_shiny_back, omanyte_shiny_back, omastar_shiny_back, kabuto_shiny_back, kabutops_shiny_back, aerodactyl_shiny_back,
                         snorlax_shiny_back, articuno_shiny_back, zapdos_shiny_back, moltres_shiny_back, dratini_shiny_back, dragonair_shiny_back,
                         dragonite_shiny_back, mewtwo_shiny_back, mew_shiny_back]

all_cries = [bulbasaur_sound, ivysaur_sound, venusaur_sound, charmander_sound, charmeleon_sound, charizard_sound,
             squirtle_sound, wartortle_sound, blastoise_sound, caterpie_sound, metapod_sound, butterfree_sound,
             weedle_sound, kakuna_sound, beedrill_sound, pidgey_sound, pidgeotto_sound, pidgeot_sound, rattata_sound,
             raticate_sound, spearow_sound, fearow_sound, ekans_sound, arbok_sound, pikachu_sound, raichu_sound,
             sandshrew_sound, sandslash_sound, nidoranf_sound, nidorina_sound, nidoqueen_sound, nidoranm_sound,
             nidorino_sound, nidoking_sound, clefairy_sound, clefable_sound, vulpix_sound, ninetales_sound,
             jigglypuff_sound, wigglytuff_sound, zubat_sound, golbat_sound, oddish_sound, gloom_sound,
             vileplume_sound, paras_sound, parasect_sound, venonat_sound, venomoth_sound, diglett_sound,
             dugtrio_sound, meowth_sound, persian_sound, psyduck_sound, golduck_sound, mankey_sound, primeape_sound,
             growlithe_sound, arcanine_sound, poliwag_sound, poliwhirl_sound, poliwrath_sound, abra_sound,
             kadabra_sound, alakazam_sound, machop_sound, machoke_sound, machamp_sound, bellsprout_sound,
             weepinbell_sound, victreebel_sound, tentacool_sound, tentacruel_sound, geodude_sound,
             graveler_sound, golem_sound, ponyta_sound, rapidash_sound, slowpoke_sound, slowbro_sound,
             magnemite_sound, magneton_sound, farfetchd_sound, doduo_sound, dodrio_sound, seel_sound, dewgong_sound,
             grimer_sound, muk_sound, shellder_sound, cloyster_sound, gastly_sound, haunter_sound, gengar_sound, onix_sound,
             drowzee_sound, hypno_sound, krabby_sound, kingler_sound, voltorb_sound, electrode_sound, exeggcute_sound,
             exeggutor_sound, cubone_sound, marowak_sound, hitmonlee_sound, hitmonchan_sound, lickitung_sound,
             koffing_sound, weezing_sound, rhyhorn_sound, rhydon_sound, chansey_sound, tangela_sound, kangaskhan_sound,
             horsea_sound, seadra_sound, goldeen_sound, seaking_sound, staryu_sound, starmie_sound, mr_mime_sound,
             scyther_sound, jynx_sound, electabuzz_sound, magmar_sound, pinsir_sound, tauros_sound, magikarp_sound,
             gyarados_sound, lapras_sound, ditto_sound, eevee_sound, vaporeon_sound, jolteon_sound, flareon_sound,
             porygon_sound, omanyte_sound, omastar_sound, kabuto_sound, kabutops_sound, aerodactyl_sound,
             snorlax_sound, articuno_sound, zapdos_sound, moltres_sound, dratini_sound, dragonair_sound,
             dragonite_sound, mewtwo_sound, mew_sound]

all_trainers = [Brock, Misty, Lt_Surge, Erika, Koga, Sabrina,
                Blaine, Giovanni, Oscar, David, Jonathan, Champion]

class Battle_Sprite(pygame.sprite.Sprite):
    '''
    Controls the Battle Sprite on the main menu.
    '''
    def __init__(self):
        pygame.sprite.Sprite.__init__(self)
        self.image = pygame.image.load("BattleSymbol.png")
        self.rect = 120, 120, 0, 0
        

class Slot_Sprite(pygame.sprite.Sprite):
    '''
    Controls the Lotto Sprite on the main menu.
    '''
    def __init__(self):
        pygame.sprite.Sprite.__init__(self)
        self.image = pygame.image.load("SlotMachine.png")
        self.rect = 425, 120, 0, 0
        

class PC_Sprite(pygame.sprite.Sprite):
    '''
    Controls the PC Sprite on the main menu.
    '''
    def __init__(self):
        pygame.sprite.Sprite.__init__(self)
        self.image = pygame.image.load("PC.png")
        self.rect = 120, 320, 0, 0

class Rule_Sprite(pygame.sprite.Sprite):
    '''
    Controls the Rule Sprite on the main menu.
    '''
    def __init__(self):
        pygame.sprite.Sprite.__init__(self)
        self.image = pygame.image.load("Rules.png")
        self.rect = 440, 320, 0, 0

class Lotto_Sprite(pygame.sprite.Sprite):
    '''
    Controls the Pokemon Sprite when the lotto option is selected.
    '''
    def __init__(self, number, shiny = None):
        pygame.sprite.Sprite.__init__(self)
        if shiny == None:
            self.image = all_front_sprites[number-1]
            self.rect = 280, 120, 0, 0
            menu_sound.play()
        if shiny != None:
            self.image = all_shinyfront_sprites[number-1]
            self.rect = 280, 120, 0, 0

class Intro_Sprite(pygame.sprite.Sprite):
    '''
    Controls Prof. Oak Sprite in the introduction.
    '''
    def __init__(self):
        pygame.sprite.Sprite.__init__(self)
        self.image = Prof_Oak
        self.rect = 280, 120, 0, 0

class Trainer_Sprite(pygame.sprite.Sprite):
    '''
    Controls the Enemy Trainer Sprite during battle.
    '''
    def __init__(self, battles):
        pygame.sprite.Sprite.__init__(self)
        self.image = all_trainers[battles]
        self.x = 620
        self.rect = self.x,100, 0, 0
    def add(self, number):
        if self.x != 120:
            self.x -= number
            self.rect = self.x,100, 0, 0

class Enemy_Sprite(pygame.sprite.Sprite):
    '''
    Controls the Enemy Pokemon Sprite during battle
    '''
    def __init__(self, number):
        pygame.sprite.Sprite.__init__(self)
        self.number = number-1
        self.image = all_front_sprites[number-1]
        self.y = 250
        self.rect = 280, self.y, 0, 0
    def add(self):
        if self.y != 80:
            self.y -= 5
            self.rect = 280, self.y, 0, 0
            if self.y == 100:
                all_cries[self.number].play()
    def sub(self):
        self.y += 5
        self.rect = 280, self.y, 0, 0
        if self.y == 150:
            self.y = 480
            self.rect = 280, self.y, 0, 0
    def get_y(self):
        return self.y

class Team_Sprite(pygame.sprite.Sprite):
    '''
    controls the Team Pokemon Sprite during battle.
    '''
    def __init__(self, number, shinypokemon):
        pygame.sprite.Sprite.__init__(self)
        self.number = number-1
        if number in shinypokemon:
            self.image = all_shinyback_sprites[number-1]
        else:
            self.image = all_back_sprites[number-1]
        self.y = 450
        self.rect = 190, self.y, 0, 0
    def add(self):
        if self.y != 240:
            self.y -= 5
            self.rect = 190, self.y, 0, 0
            if self.y == 240:
                all_cries[self.number].play()
    def sub(self):
            self.y += 5
            self.rect = 190, self.y, 0, 0
    def get_y(self):
        return self.y
                
                
        
    
        
            
        


