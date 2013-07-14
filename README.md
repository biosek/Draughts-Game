Draughts-Game
========
CZECH version<br>
Složky v Zapoctak->bin->Release->
Knihovny, Meshe, Textury, Zvuky
musí být ve stejném adresáři jako exe soubor.

Pro zkompilování je dále potřeba mít nainstalováno:
- .Net FrameWork 2.0 a výš
- Truevision (http://www.truevision3d.com/downloads/download.php?file=28)
- DirectX 8.0 a výš

Po instalaci Truevision je potřeba načíst do referencí knihovna z "cesta Truevision"\sdk\dotnet\MTV3D65.dll

Stolní hra „Dáma“
Programátorská dokumentace

Anotace
„Dáma“ je počítačová interpretace obyčejné stolní hry „Dámy“ programovanou pro Windows s .NET. Hra se hraje v jednookenní aplikaci bez možnosti fullscreenu.

Přesné zadání
Program bude simulovat reálnou stolní hru „Dámu“ pro dva hráče podle pravidel České dámy.
Schopnosti:
-	3D
-	Možnost označení aktuálních táhnutelných políček
-	Ozvučení
-	Ovládání pouze myší
Technologie:
-	C#
-	TrueVision knihovny (podpůrné knihovny pro vykreslování)
Platforma: Windows

Pravidla České dámy podle České federace dámy:
1.	Hraje se na šachovnici 8x8.
2.	Soupeři mají na začátku po 12 kamenech stojících na protilehlých stranách v prvních třech řadách na černých políčkách.
3.	Kameny se pohybují po diagonálách po černých políčkách, pouze vpřed, a nemohou přeskakovat kameny vlastní barvy.
4.	Pokud obyčejný kámen dojde na druhou stranu šachovnice, přemění se v dámu.
5.	Dáma se pohybuje diagonálně dopředu a dozadu o libovolný počet polí.
6.	Jestliže se kámen nachází na diagonále v sousedství soupeřovy figury, za kterou je volné pole, je povinen ji přeskočit, obsadit toto volné pole a odstranit přeskočenou soupeřovu figuru z desky.
7.	Skákání je povinné. Může-li hráč skákat jak dámou, tak obyčejným kamenem, musí skákat dámou. V případě, že může jedním kamenem provést více variant skoku, je na něm kterou si vybere, musí ovšem variantu doskákat (např. když může jedním kamenem skočit jeden nebo tři kameny, skočí tedy jeden, nebo tři. Nelze skočit jen dva z druhé varianty.)
8.	Při vícenásobném skoku se kameny odstraní až po dokončení celé sekvence. Přes jeden kámen nelze skákat vícekrát.
9.	Hráč který je na tahu a nemůže hrát (nemá kameny, nebo má všechny zablokované), prohrál. Partie končí remízou tehdy, když je teoreticky nemožné vzít soupeři při pozorné hře žádnou další figuru.
10.	Jestliže některý z hráčů zahraje tah v rozporu s pravidly (např. opomenutí skákání), je na jeho protihráči, jestli bude vyžadovat opravu tahu nebo ne.

Zvolený algoritmus
Na začátku je na tahu bílý, po táhnutí figurkou se přepne tah na černého, po táhnutí se vracíme na tah bílého a tak stále dokola, dokud nenastane konec hry.

Algoritmus vyhodnocení tahu je vskutku jednoduchý:
1.	Kliknutí na figurku hráče, který je právě na tahu.
2.	Kliknutí na jakékoliv prázdné políčko.
3.	Kontrola, jestli bylo kliknuto na políčko kam se může podle pravidel se zvolenou figurkou táhnout. Jestliže ano, provede se přesunutí figurky na zvolené políčko.
4.	Kontrola, zda jestli byla přeskočena nějaká z figurek protihráče, pokud ano, provede se kontrola, jestli se se skákající figurkou nedá skákat dále, pokud ano, pokračuje se bodem 2.
5.	Jestliže se mohlo s jakoukoliv figurkou hráče, který je na tahu, skákat a za tah se neprovedl žádný skok. Smaže se náhodná figurka, s kterou mohl skákat.

Konec hry je definován vítězstvím jednoho z hráčů a to takto:
-	Vítězem se stává bílý pokud:
o	Černému hráči nezůstali žádné figurky.
o	Černý hráč nemůže táhnout s žádnou z jeho figurek.
-	Vítězem se stává černý pokud:
o	Bílému hráči nezůstali žádné figurky.
o	Bílý hráč nemůže táhnout s žádnou z jeho figurek.

Diskuse výběru algoritmu
Algoritmus na hraní dámy je definován pravidly, podle kterých se hraje. Čili jsem si moc vybírat nemohl.

Program
Hlavní datovou strukturou programu je dvourozměrné pole, které definuje šachovnici a figurky na ní. 
Program je rozsáhlý a dostatečně okomentovaný, proto ho zde nebudu kopírovat.

Alternativní programová řešení
Program není moc pěkně napsaný, nejdříve jsem se snažil psát metody pouze s předávanými parametry, ale poté jsem měl kód výrazně nepřehledný a dlouhý, proto jsem ho rozkouskoval a vytvořil tak metody určené pouze ke zpřehlednění kódu.

Průběh prací
Největší problém byl začátek. Nasliboval jsem kupu věcí, se kterými jsem vživotě nepracoval. Dlouhou dobu strávil studiem 3D vykreslování a hledáním vhodného enginu, abych si ho nemusel psát sám. 
Když jsem vyřešil všechny problémy potřebné k funkčnosti hry (vykreslení objektů, získání souřadnic a objekt z kolize kliku), tak vlastní psaní kódu nebylo už tak složité, dokud nedošlo na tahy dámou, protože ty jsou velice komplexní co se týče pohybu po šachovnici. Nad nimi jsem přemýšlel, jak je co nejvíce zkrátit, což se povedlo. 
Každou funkci jsem testoval hned po jejím napsání, což mi ušetřilo mnoho času s řešením bugů a testováním na konci.

Co nebylo doděláno
Co se týče pravidel hry, tak jsem nedodělal dvě věci.
1.	Řešení remízy
2.	Pokud se mohlo skákat s običejnou figurkou nebo dámou, nevybere se primárně dáma, ale vezme se náhodná z nich.
Co se týče naslibovaných věcí, tak jsem nevyřešil hru po LAN.

Závěr
Programování je můj koníček, proto mě práce na tomto projektu velice bavila. Už i proto, že jsem počítačovou interpretaci dámy na PC dělal už na střední ve velmi strohé podobě :-). V budoucnu se budu zabývat buď doděláním umělé inteligence na tuto hru, nebo přejdu na uplné jiné téma her, protože řekněme si to narovinu, kdo v dnešní době hraje stolní hry na PC jen pro zabití času? :-) 
