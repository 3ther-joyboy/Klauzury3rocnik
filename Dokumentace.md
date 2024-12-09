# Klauzurní práce 3 ročník 1 pololetí
## Lukáš valla

# O projektu 
Projekt byl inspirován hrou kterou jsme viděl na jednom game jamu, tuto hru jsem spětně už nenašel.
## Game loop
Mate se dostat z mistnosti A do mistnosti B a zaroveň se musite vyhibat svému minulému já ("Vzpomínkám"), s každým ciklem přibívá vzpomínek kterým se musíte vyhíbat.<br>
K obohaceni byla přidana dinamicka mapa (jak ma [Risk of rain 2](https://riskofrain2.fandom.com/wiki/Environments)) a přidáním předmětů které vám po dobu držení dávájí permanentí výhodu v statistikách nebo si můžete zvolit použít a využít jeho speciální efekt více impulznímzpůsobem.
## Hrač / objekt hrače
### Objekt Hrače (`movment`)
Tento objekt `movment`, sam o sobě nema ovladani a funguje poze jako kostra, ma v sobje pouze zakladni logiku a vola připadně funkce co řeši ostatni věci jako je použivani itemu ve objektu `hud`
Objekt se da ovladat přepisováním `public` proměných
#### Duležité Funkce
##### Invincible(bool)
přepina hurtbox a collisionbox objektu na to aby se navzajem nemohly jakkoliv zastavit či zabit
##### PlayStepSound()
vezme nahodny zvuk ze skolžky s zvukama kroků a přehraje ho
##### Jump()
můžete skočit opuze jednou za .2s, po dobu kdy tento časovač běží tak neůmžete skočit
vyhodnocování skákání jde v tomto pořadí:
###### Jump
řeší pouze skákání po době co jste na zemi
###### WallJump
jestli jste v dosahu stěny
`movment` uděla raycast a otočí ho kolem sebe (proměná resolution je jak přesně se otáčí `360/resolution` v případě kdyby `resolution` bylo 36, paprsek se otočí 36 o 10°)<br>
od strany která má nejmenčí vzdálenost raycastu se odrazí v pravém úhlu
###### Jump++
pokud má hráč víc využitelných skoků může skočit ve vzduchu, tento počet se restartuje po odrazu od země nebo stěny.
##### Duck(bool)
skrčí `movment`
pokud skusí vstát pod objektem kde se nevleze nic se nestane
##### Interact()
zavola funkci do objektu na ktery se `movment` momentalně dívá
##### KYS() (kill your self)
reší mazání objektu ze scény a spawnování rakdoll
jestli zabitý hráč, funkce spawne posmrtnou kamera
##### Marker3D Rakdoll()
na mistě kde se nachazi `movment` spawne rakdoll se stejnou rychlostí<br>
vrací objekt `Marker3D` aby posmrtna kamera se na ni mohla zaměřit
### Ovladani
vola reference na objekt `movment` ktery se zrovna označuje jako hrač, a vola do něj funkce podle inputu <br>
take zaznamenava vaše inputy do `RecordFrame` listu ktery předa nove vzpomince po konci konajícího se ciklu
#### Duležité Funkce
##### GetPlayerReady(bool)
Připraví `movment` pro používání hráčem, jako: zapne kameru, audio listener a UI 
## Vzpominka
Objekt Vzpominka v sobě drži veškere inormace k tomu aby mohl vytvořit novou instanci "hráce" (objěktu `movment`) a plně ji kontrolovat
### Spawn
Jedna z informaci ktere uchovava jsou infomrace ke spawnu, jako je aktualní inventář a startovní pozice.
### PlayBack
objekt `RecordFrame` ma v sobě všechni statusi ktere muže hrač vykonat každý snímek
#### Duležité Funkce
##### SetToCurrentItems()
Přida spawnutemu objektu `movment` ktery si drží vzpomínka všechy itemy které by měl mít
##### SpawnVzpominku(Node)
Spawne `movment` podle proměných a vytvočí si referenci na kterou zavola ostati připravne funkce
(tato vzpomínka se spawne do Nody předane funci
##### NextFrame()
vezme přvní `RecordFrame` z listu, sve referenci `movment` nastavi nastavajici inputy a tento `RecordFrame` vymaže z listu

#### Chyba
Godot nemá deterministikou fyziku, takže po několika kolizích se stěnou se to co dělá vzpomínka odlišovat od toho co jste dělal předtím
## Items
### usableObjet
objekt ktery se použiva pouze ke komunikaci<br>
Ruzne itemy potřebuji ruzne informace k fungování, tudíš jejednoduší vytvořit objekt který všechny tyto informace může mít, než vypisovat v každé funkci informace které stejně nevyužije<br>
objekt obsahuje:<br>
```
float Force;
Node executableNode
Vecotr3 Position
Vecotr3 Velocity
Vecotr3 Rotation
```
ma také speciální constructor co bere objekt `movment` a vyplní informace
### Items
struktura inventaře:

```
[`movment`]
    [`Node` - "Items"]
        [`Item` - "xxx"] // jednotlive itemy
        [`Item` - "xxx"]
        [`Item` - "xxx"]
```
Je abstraktni třida z `Node` ktera zaklda zakladni proměne ktere každi `Item` používá a tvoří abstraktní funkce pomocí kteřých může poté inventář komunikovat s `Item`ama ktere počitač nezna
#### Duležite Funkce 
##### SpawnItemCollectable(Vector3)
Spane collectable itemu na pozicich předane funkce a nastavi potřebe proměne jako je vyzual
##### abstract Copy()
Vytvoří kopii objektu, abstraktni je jelikož "nevím" jaký objekt chci zkopírovat takže nemohu využít constructor
##### abstract Use(usableObjet)
určena pro efekt toho kdž se `Item` zrvona použije, tato funkce by měla vždicky volat `KIS()`;
##### abstract PassivesAdd()
funkce se spouští při vzaní `Item`u, využiva se na permanenti přidani nějake vyhody (statů, neobo spawnutí něco okolo hráče)
##### abstract PassivesRemove()
funkce se spouští při mazání `Item`u, využiva se na odstranění permanentího efektu nebo vyhody 
##### KYS()
smaže objekt, přehraje zvuk použití a zavolá funkci `PassivesRemove`

Jestli pasivní schopnost je použe zlepšení statistik, doporučuju si vytvořit třetí funkci která se v nich bude volat:
```c#
	private void Stats(bool x){
		movment player = this.GetNode<movment>("../..");
		player.PermanentStat += (x?1:-1)*AdditionOfPermanentStat;
	}
```
#### Chyba
přište použe Interface a ne abstraktni objekt<br>
timhle sem musel pracovat s každým novím objektem jako s `Node` který nemá pozici, takže item jako `fireball` byl daleko složitější vytvořit<br>
A implementaci těch par funkci a proměných manualně by nebyl zas tak velký problém<br>
### Inventory / HUD (Heds up display)
zařizuje funkčnost inventáře jako je vybýrání/používání itemů a zborazování informací na obrazovce 
#### Duležité funkce
##### int UseItem(usableObject)
objekt si uchovává jaká slot je zrovna vybrán během použití najde tolikátý `Item` v inventáři a na daný objekt zavolá abstraktní funkci `Use(usableObjet)` do ktere předa komunikačni objěk. (strukturu a popsání kominikačniho objektu najdeme v sekci ##Items) <br>
funkce vraci intiger kolikaty `Item` byl zrovna použit aby to mohlo být zapsáno do `RecordFrame`, pokut nemá žádné `Item` tak vrátí -1<br>
##### MoveInInventory(int)
posune vas v inventaři o předany intiger<br>
pokud se pokusite dostat na N+1 `Item`, dá vást to na prví Item v inventáři (to samé i naopak)<br>
##### UpdateInventory()
Smaše všechny `Itemy` z listu a znova je tam všechny vypíše<br>
#### Chyba
tyto dvě funkčnosti by měli být rozděleny do 2 různých objektů, takhle se v tom dělá zmatek<br>
U vytvařeni inventaře mi došlo až jak moc objektově orientovany godot je, a itemy do toho inventaře nebylo instancovat vubec třeba a stačilo je mít v jednom Arrayi (neboď na pozadi je to v něm stejně jen musim zbytečně využivat funkce z 3tiho objektu)<br>
## Generace
vždi na začatku noveho ciklu cela scena se smaže a vygeneruje se zcela nová ze stejného seedu ve stejném pořadí, tudíš po požadavku na vygenerovani vice věci, začatek zustane stejný a nove věci se přidaji
### Start/Finish
generuje se tak že se vezme `seed` + ciklus, z toho se vybere první místnost, a pak se náhodně vybírá dokud se nevybere další, jiná mítnost
### Generovani prostředi
každý objekt, který není garantovaný být na mapě má 1/3 že na ni zustane pro daný seed
### Item
vybere se náhodná pozice mezi dvouma bodama v (`ItemSpawnLocation`) na ni se spawne item ktery se pote teleportuje k zemi<br>
tohle probjehne tolikrat kolikaty je ciklus
## QOL (quality of live)
### Audio settings
(po tom co sem pochopil jak funguje godot)<br>
V menu Každá kolonka s posuvníkem a jménem je vlastí objěkt `audioChanger`, každy objekt oblada jeden audio Bus, v menu se prodecuralně vygeneruje pro gaždy Bus ve hře
### Spawn Camp
Po dobu co hráč zustane v začatečni mistnosti tak ostatni vzpominky ktere se tam spawnuli s ním ho nemůžeou zabít<br>
ovšem v moment kdy opustí tuto zónu hurtbox hrače se aktivuje <br>
(hitbox -> co zabíjí, hurtbox -> co musí trefit aby zabili, collisionbox -> co interaguje s fyzikou)<br>
<br>
Aby se zabránilo dlouhavému vyčkávání v startovní místnosti tak na každí ciklus hráč má vyčleněný čas, který jestli vyprší hráč prohraje.<br>
### Ukazatel
cilova mistnost je vždi zvírazněna ikonou ktera jde vidět přez stěny<br>
kromě toho hrač při spawnutí se dívá přímo směrem na tuto značku
### Binds
v menu se dá přenastavit ovládání na libovolný druh vztupní periferie<br>
vždy po kliknuti an tlačitko, prvni dalši input se přida do listu těchto akci<br>
jestli chcete smazat, musíte restartovat celou akci<br>
### Smrt
když jakakoliv vzpominka zemře, necha po sobjě takzvanou "ragdoll" (mrtvolu) ktera smrt uděla plinulejši a rovnou něco nezmizí<br>
<br>
po smrti hráče hra dál běží, avšak hráč už nemůže interagovat s hrou<br>
kamera nasleduje ragdoll takže smrt neni jenom nahle zčernání obrazovky<br>
