# WordFinder

## About the solution:

I've separated the solution on three projects:

- **WordFinderContracts**: base classes, interfaces, enums, messages, etc.
- **WordFinderImplementation**: WordFinder implementation for the excercise.
- **WordFinderTests**: a small set of tests to prove the solution.

The implementation is basically composed by two steps:

- First I generate a map for all characters to then start searches direct on their spots, instead of iterate over the whole matrix to start testing if the word is present.
- Second a recursive function to check over the matrix if next character of the word is present.

## About the tests:

I've added a file with ~200k words with 10 or more characters to find into the matrices of 64x64. It's on **WordFinderTests** project with the name **words.txt**.
Also, there is a small class downloaded from the Internet to generate matrices using a subset of words from **words.txt**. 
On this way I can test the implementation with more data that provided on the exercise.

So, for example, some output for the test:

    Test 1 of 5. Generating matrix of 64x64...

    Matrix generated: 

    knghrysnkqutekueggedbkzEjdigknnjieuysiiynsutmwomsdssjeqaopynpkbx
    djjarvnxxgucrfenqxphvuagotkckwjendospermousdoniijkwswucijfmxkjnb
    xpeuqcadcuninferentiallyugteaxmggsgyiothjppdmetaxylenecfibaijucf
    fjuhqstwimrfcbatyfrpjcqpqbpuokxqqiytgbxwsraeztcgbhkddovcubrpcmhy
    ozcarefulnesskttyifgzumtnsentry-boxahidtuauiriaiixqufbytnmfhhhna
    bqkzutmpemcmzdmmvqwmsyaiexnudvmnauhaqbppm-ptqotiapreprohibitionj
    azmfwmvtbgtqangelinformalfacquaintednesszeqeapthoasbrcbgyntqbimj
    xfadozuqrgapevyuncxgzhyndmthvwnqgfhvspupgskpuhccpzdmetamorphygvu
    tshcoccbiinxobjectivisticspintermaxillarxoqcgytwuisqbzbengcwnxhr
    azqanamacmgmzcuizskgzsqzprozbszzyybgxnidwpnxalfjmtxready-formedk
    nznraumtihuogalloflavinehuyivnaihckacfjyghkfqlcqzrkytvqhnpsgpwqw
    gbfehcxhuolitghdrhqecbhdmconncjjupzoxpolyarchistgiatrogenicallys
    erpfgtgyayayyvseoghzncehvcugjcymmmicskzmhgghcngzbmrnotfbnginzchy
    zyrufous-brownfanvcykmjsflqmmono-iodomethanecshonor-thirstyezabt
    ikwlorsehqzxfepobzkunhfhviqonhmynnwrvrbyqlwjjqhjftdahhfbfottndfz
    daknhisixndhkwyfmfcarefulnesshmnzwfheqbgatcbChekhovianvhrfmnokmp
    brheqzasebtmihbhdcrmbfrnbkrzeousgspongeprooftjijedjjlaoyztmcnasr
    gdesgaqmjqyqdxusfhvadkvszenykqdpsknneodqojzebxscyuxqfsnphxvcasiu
    mnysjtnsdfcarefulnessofcxrigiaoramtozzsugemzfsjqstartmaohxvsdgzd
    rrufyecmsfqocfvfhsoyjxcaeigbcaxifohrytrareyownvbcprdspttppbdvahs
    hqitxghdadeformation'shnpnzskoqgxng-eydvhhcetovapjcfbocehsejeqan
    yrhshknxbdghrdkaszcdjjpddgyzstohgogtgrsdhghymuweudhiclinkeringzh
    ppaaxhupobjectivisticffansbdtcrthvihuahnicynnzjtjenthyfuiuoetqze
    hmvhiwpamtjadsmnhruciself-flagellationbdeep-damaskedwmwsidqiuxsj
    nqxonkiclinkeringxbafsxibusknsfibrbrgnderqoqugtwmdskvidatoesryux
    adonndftuxpethsubxylcuwzufkwdkknwipstihiowtywpbntkswacvlgszgoizj
    mwtosabbqwheat-raisingtegijusaqexamtvcstmqejcqcyeuesgrqwdqkjuhbq
    zuhrkuaugjaljigqgapsxyidrqwmzicspnkyhaykainwpmn-cvspsosbkufvsxvy
    gzp-xyracev-notedajtenntyjxhzdnsotppxlxercunadpbgarxosttpavonaby
    rhbtwdcjbesbabtspoliatorswabxjdebpwsblcctgsjunprejudicebimgaegnt
    bgbhaahgayguwjxcchycqanzbvbathyseismpytcynabvheogeywhocavajeswyv
    yujihfnutwplueduqqqccdoexhwnyommomcbxjmardlmazgwyqpafpsvqtxaszwa
    kdprhcefhpklvcdyamphzsupergloriouslyjydwuiwqktznbrzxkebzxepardts
    xssssgscytoypthagoroRutsfbaniceqqdtsakfvjwprakjgpatdnefcjaifgbtf
    pxatxpswswdqfirurtxuoalkyqiacpoicicdjzjapxChristophedyxouqmogytt
    hvCytherellaevjjspsopaaechoreographiczfvdgegtmnhftmrgfdgqemreqee
    cphccdsuiwkiwikztbokecwyzcpsohqnbzedegangelinformaldcuozfndepqgb
    fxgwehvysmrzusdeewubsnrhhypotenusalxnozqrvhqdowgftiswfncmdknjzur
    ynnonpermanentdytrqfvvyeukrodnfrbrsbathyseismv-spolysomatousudzj
    ytrpcnzieqyraijzhtssimhnwpeozcscgkubgypimdpuqcssbrkamgirmssidmjc
    jvkqyhizeifxxcwuormflfrzbiszgctatmIngaevonicbmcxhizmzdmeeppcrvtc
    dfdkcbnctthfioxppsshltaqafsfwoecsuywrdugwwtgwgaqparydkzfeekakwfe
    qfnpladqsmmftantalizedyggoiutwrzroqregcollophoreulokfiiugruljnxc
    qninocicudevdfhbrfzchkuhudvodacedxbngxisqmmbdnraxswdovvlsmhztmqy
    zanfpvcgbhmwzsdsauctorizatewejorthocarbonicfjyezepfkvevnjocgbarm
    jhdmaztpfpynskzdlusaenizrcnkgqruyvwbbtjerrzgxhdzkzagilhezuunfnnw
    osupermoralobathyseismmiaaexrxadddstlkmntfwufuhxarchnessesinuhyn
    pfckdjekonifxrctsfzofnrsnysneytmvinterminingcrwadqupichsosekudij
    uxtoicnoncontiguitiesdcmtdsseceqxsfeghobgbwyqhrxbpmgutequytinrdq
    dditaktuthfhhevuspqdtzvbeqkh-emqtriver-wornnrxipdtibarxdiarvrbsi
    wpoblj'pazxdgijqhvecijpteqdgdqqerkrbigetfoxbctxzqqnfxozmlvitegeo
    itnvrisalkwbnorptdqbckgudfbnantireactionarymzyznecabrskylsqmovme
    xyscvhirlitpost-bellumhnyuwpyxanbkpbfwrxueghcwcsxwttnctm-eubrtiy
    ifnkftgjyksbeukcrervrhacrdpbsqkajazwiunactivatedubegcogbcjebgfpg
    zsswaotasofwmpwzxvhabductor'sjvjdubozkpgiiooyjqgnvdnypbvontcaueh
    svsxxhzmfahwqegshkmbfhmcnfenrmjCycadofilicalesfjfbvmiicjnprrnndt
    rsrjneink-carryinggprforinsecalixtgaowwbcujcvrjwfbrcfccscnobiwak
    oypnghudpfnmomimcwwgzpcskoobqvqfyoowcwpyhloasqdhduyqazkyewuszsnt
    qhopqmjggyznrecantinglyypplfvwepzrkundeviationxtwghmvaccrescentj
    tkohsptwpgmernyrshippsugxvvnorgegihpjpbhntxpgtyyrjicwwvbtglxdfiy
    zgxtauctorizateewidysmorphismdahezpzvfcidermatoglyphicrvenypjdcd
    atywqairfmbbiapjanwydbwbhtnhpnyziaubujvgxisyapnjyfpxgyzzdxowzact
    octqunqqrqbbelly-worshipingwwygwgtkywwqmpzgrxfqywvpirsunjjxzhtus
    ifnjobwjaxuyubmmfgabixeoexpnyreveehrtfkuyfiuywsixkubttimayeqnjkj

    Starting processing of 212671 words...

    Process of 212671 words finished in 00:00:03.2612302:

	 - Word "bathyseism" found 5 times: 
	 - Top to bottom, from: [7,8] to [7,17]. 
	 - Top to bottom, from: [8,29] to [8,38]. 
	 - Left to right, from: [26,30] to [35,30]. 
	 - Left to right, from: [35,38] to [44,38]. 
	 - Left to right, from: [12,46] to [21,46]. 

	 - Word "carefulness" found 5 times: 
	 - Left to right, from: [2,4] to [12,4]. 
	 - Top to bottom, from: [3,8] to [3,18]. 
	 - Left to right, from: [18,15] to [28,15]. 
	 - Left to right, from: [10,18] to [20,18]. 
	 - Top to bottom, from: [55,37] to [55,47]. 

	 - Word "auctorizate" found 4 times: 
	 - Top to bottom, from: [5,9] to [5,19]. 
	 - Left to right, from: [16,44] to [26,44]. 
	 - Top to bottom, from: [33,53] to [33,63]. 
	 - Left to right, from: [4,60] to [14,60]. 

	 - Word "archnesses" found 3 times: 
	 - Top to bottom, from: [50,18] to [50,27]. 
	 - Top to bottom, from: [6,27] to [6,36]. 
	 - Left to right, from: [48,46] to [57,46]. 

	 - Word "clinkering" found 3 times: 
	 - Top to bottom, from: [25,12] to [25,21]. 
	 - Left to right, from: [52,21] to [61,21]. 
	 - Left to right, from: [7,24] to [16,24]. 

	 - Word "hypotenusal" found 3 times: 
	 - Top to bottom, from: [55,15] to [55,25]. 
	 - Top to bottom, from: [42,21] to [42,31]. 
	 - Left to right, from: [24,37] to [34,37]. 

	 - Word "honor-thirsty" found 3 times: 
	 - Left to right, from: [46,13] to [58,13]. 
	 - Top to bottom, from: [35,15] to [35,27]. 
	 - Top to bottom, from: [3,23] to [3,35]. 

	 - Word "objectivist" found 3 times: 
	 - Left to right, from: [12,8] to [22,8]. 
	 - Left to right, from: [8,22] to [18,22]. 
	 - Top to bottom, from: [13,28] to [13,38]. 

	 - Word "objectivistic" found 3 times: 
	 - Left to right, from: [12,8] to [24,8]. 
	 - Left to right, from: [8,22] to [20,22]. 
	 - Top to bottom, from: [13,28] to [13,40]. 

	 - Word "angelinformal" found 2 times: 
	 - Left to right, from: [12,6] to [24,6]. 
	 - Left to right, from: [38,36] to [50,36]. 

    Expected results are present, processing next test.


Also, as you probably already noticed, I took the liberty to modify signature of method `WordFinder.Find()` to return a structure with more data than only the word string for the TOP 10 of results.

The test also generates an output TXT file with all logs for the results, here is an example: [WordFinderOutput-2024-1-17-202839.txt](https://github.com/chapeti/WordFinder/files/13968859/WordFinderOutput-2024-1-17-202839.txt)

You can find it on the output folder for test project (WordFinderTests\Bin\Debug). With every new execution a new file is generated. Logs are also present on the output window.

Let me know if you want to discuss something.

Regards,
Sebastian.
