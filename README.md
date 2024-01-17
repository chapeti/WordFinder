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

    Starting test for WordFinder.Find() method.
    
    Testing with 5 matrices of 64 of size with 100 words conforming the matrix and 212671 words as input.
    
    Test 1 of 5. Generating matrix of 64x64...
    
    Matrix generated: 
    
    vdjqyufgwighrjxhyffyikngjdymvvhmyomjseimrcetacushionlikeqcguvumz
    bcontingentlytgxvitiphqycbtqypqxqesfdfsexncbtfewvizmacjtfkbwtyvh
    djqbabuplomcepewwwuhomozygousvkyrBurneyvilleqpreshelterzubfbbbkv
    Burneyvillefijxomjmavoujcxxnxvsoyzncvdymxhxdhnqedsprpkaprcpuxtae
    hmgjavsi-adorpcpgtkffovermodestlyscvgsoft-lucentztodilipovaccine
    mzbyugiafbeywshmvfXcpkvwtpiivmwbuvihkzitjongkaj-kykgektadfsfuvtv
    jdeagabveasovermodestlyicrystallographykxqkesscbumasrhpqivsdswis
    udsgndxvntzveurkzmnhcdxbgkbcklnbpscozwcrofiyipmreotkmtabarichypq
    dxfmsckddttxudtsymoxpbpvcxhiwlpcvsukHarthacanuteyhrqdnkezkovibei
    gdbbfhmaeandroidqappwznhrocppmghttmwrebqyezjtmeaoaiurrieotnsojwp
    mtvgiasmdvqrzspfishypopteralvokuujsartremblementeegnzvszlt-znuio
    abwqfpokqwfgqtajdsaqbxqyrxrixutrchtzyphprxxcqpjhsqlcaatmeythlqsn
    trblong-spurredbownhfrobwaonrtrcjjayobtrBhrwbhkypwybkfakdzhaikmm
    iyuqnjomdpcaarare-entrainpveqhdhjfncrvmeuhyvdikkzxcxzdnxiqrxkawk
    civotaressesreauejssgfufzktwheqgmotueqbbrrutgardenershippjiyeemo
    adkdjgbohnxskonpeehpvkxhiefjudxofgip-cfinexygpunwgraxxsfgclebrte
    lfjeyxpbyyw-osbtksnwixuhoxixhbhimpaper-mendedbrhaaiiefvsmblbdstx
    lwevtaovdowevcgmlimesulphureznjnyelenvsayyfqiexquednbarkometerwq
    yxsqrcpjivuatowstyjlrwbmqqdxwtpgbplmtgdrvmeassyuchenzbzhwgdxarnq
    grieikfnvsftsproboulevarddpfffsxyoyartasitrhwaqvmpeqwydeniqqragp
    zhfecupzidoikeduout-bcbakpnocjwambhoamoslfhsacerdotalizegetrsbwy
    gohioqfcfbsnuhrbvwjfgvswnjgjsxmxiimwikcolavgyrczoswjspsmvmweevki
    re-entrainqgamyunprejudiceaqpyfvqmrenrdwell-horsedxazphoosuvnjvv
    pvrooueqecgpmiOgpscnbnveddqpxhziquhctxotogvxdsbewogcghukamkriytw
    cxzrds-rdizuiywkbksdbfurodiazoleozugtjddbpyxiabbmaqtgavfvtrecffh
    ujProbe-bibeleezvbvegoieeomasypvjcounterallianceusiojnbxarfqiszi
    zmcinynedddikrnwhvfdpcmqpgzsmhiqycsqfypvmekfpcmiajhnmttzaatuzosv
    uojitktjfkpkqfsxuqscaueajtqteuantilitteringbjtivkconsarcinatexem
    orOaoarrfdibodvaunburstableyedtwtvnsyhakhicshtkftyvaosqapsghhqnc
    jnwtixabuyamkjiabmefhshxemdhorgan-pipeqviMfqveqfogfvkmuqepoduhaa
    viekdtijrjxooslavyuHoehpfsqzdmbateunefeBrayporte-monnaiebaeicqgk
    hxnixendocellulardpoddnmsgmGavsapotm-upuzcbpdvevwfuybghckdrqbbpy
    gbsvnupudsrkdyewbrppbtHarthacanutehbcjhrrrjjyxynaupdntxocaqwsnrr
    ytvorticitiesqaitpbkjikjftzsumjeitkfuxwnpocapwefegcrkre-entrainw
    caiqeutvahvngkuppmiiiftbmwwtbynwyfxptpieorkdnuhadmiijmnxgeehybtu
    uylbsegzzyybvqcontingentlyiejbunderstoryfhsknXenopterygiicinwvqb
    nmlepanioqjhybzlyrisikzjofbrwxbksrxaixjvzanxuvetccprmstcmdrbmkmp
    ozehoyhblgxyxcqybqyiiwyprison-houseunwhivmxfuueaoya-msfsnpkhaouz
    eiaznvmeeuzwysftregaiscfsrtlmatvsyvkgeqlmpcrtjtsigrdgnrmovhccvth
    bsdasvjHoovnqkneyjhnonrecipiencebveagsrlxhstrdwihydoopennxtcrmoz
    xemvimbabhorrencyiqykycnwcicypxrrsrmrbaexozuhvrzipjwwpnyozysosgd
    wwmdbcnritbnzifhkxajeqpcdxjhchtsrv-kjobgcsycoczetygnsxafriansuna
    awvdiuqtvusksnunformulatedeeoootgynsgwhcourthouse'savironbfsppig
    fyetlwuhjzusbpdiemfoyonxzoinqpjradoriuUyfsrhngmtywodiursaguxepmu
    pcryimbadhbkrqksubprofitableakkacxbkqobnudzemyvwwbuzdsahmmdwcajs
    wshztzccstpxaiuteieyqmikdztskzgithltqwixzfgoswbisgsxektkexbiiyes
    mfezyagavhrkntsiyjmdhjyifujtaeqtboemrbqradbrmefqemcushionlikeohr
    ryiqqgsnevoykjwootopvulture-tornbjxohpuytariwluyzfvwitowthbqsxpj
    trxymsfutpff-yetggvbaenhnafzdkxezhxfovirardzylujdtivconpaplkcfvh
    szfvauftudicgcjugvwjbbtanaplerosisrymstick-at-itivegckgrluisxgau
    pgbfkphenrtcrvzZvzttwhjroatscrgsxgifehiui-utocqkuiwyahygljojsqev
    jpfvmennizazorcikixyuitphcivrgbtryhyyvsarhqiyotfgryjtgbbyqljmswb
    yrhqirmqjwbmweroidokhrakxgchhppymoqhrdtcuuaoyairzoahimugqwacbdhv
    hjzepusghklziennxkipremieresstaqafooqecqjlanwcpcjlfrvcgapytfryuo
    kzymqnorthernnessfkzojvxyozsyxzmaniwwdvkulh'hhuuuokkevawebeczzzm
    tistsibakzfbgspvtkxgmoxfmibanishmentsbwmkeasqexsmgjpbtaikmrjrgat
    ojoghtnpokvrniqikdimhwtuueokdnueugafkqfwrdbqxdshwiyoxomcqumdbcwa
    rrwgiychxnupwell-fendedfviqqecupwazgnmtmreqkuwwiysrqovmhjqkzumqv
    sftudpxosbjjqwmlncscmdaadenochrometodciidqymkccontingentlyeqsaax
    jeuvdxvstjmanifestantakbkejmttqcpacfhajgrbyknohnbswihnqiusjyojoa
    txfemkfcznxfbapxwtkewself-propellentiddrndivqjhldncuzidscctuyqco
    uqovermodestlynbjtkejxhevgofmjswbvazrejfxiefrkkiqOwensvillecjjif
    bvqeynkpiegasbnjvplenipotencyhwozzewugecgewaredkaygyurftdnouwgib
    isaBurneyvillenmakdyayuhtrshsjgawuszdkijjsqhgkwezcukcoleopteranx
    
    Starting processing of 212671 words...
    
    Process of 212671 words finished in 00:00:02.9370886:
    
    	 - Word "Burneyville" found 5 times: 
    	 - Top to bottom, from: [2,33] to [2,43]. 
    	 - Top to bottom, from: [3,0] to [3,10]. 
    	 - Left to right, from: [12,40] to [22,40]. 
    	 - Left to right, from: [30,39] to [40,39]. 
    	 - Top to bottom, from: [63,3] to [63,13]. 
    
    	 - Word "re-entrain" found 5 times: 
    	 - Top to bottom, from: [13,15] to [13,24]. 
    	 - Left to right, from: [13,36] to [22,36]. 
    	 - Top to bottom, from: [22,0] to [22,9]. 
    	 - Left to right, from: [22,6] to [31,6]. 
    	 - Top to bottom, from: [33,53] to [33,62]. 
    
    	 - Word "cushionlike" found 4 times: 
    	 - Top to bottom, from: [0,45] to [0,55]. 
    	 - Left to right, from: [4,60] to [14,60]. 
    	 - Top to bottom, from: [46,50] to [46,60]. 
    	 - Left to right, from: [53,47] to [63,47]. 
    
    	 - Word "contingent" found 3 times: 
    	 - Top to bottom, from: [1,1] to [1,10]. 
    	 - Top to bottom, from: [35,14] to [35,23]. 
    	 - Top to bottom, from: [58,46] to [58,55]. 
    
    	 - Word "contingently" found 3 times: 
    	 - Top to bottom, from: [1,1] to [1,12]. 
    	 - Top to bottom, from: [35,14] to [35,25]. 
    	 - Top to bottom, from: [58,46] to [58,57]. 
    
    	 - Word "furodiazole" found 3 times: 
    	 - Left to right, from: [1,56] to [11,56]. 
    	 - Top to bottom, from: [24,21] to [24,31]. 
    	 - Left to right, from: [28,8] to [38,8]. 
    
    	 - Word "Harthacanute" found 3 times: 
    	 - Top to bottom, from: [8,36] to [8,47]. 
    	 - Top to bottom, from: [32,22] to [32,33]. 
    	 - Left to right, from: [39,7] to [50,7]. 
    
    	 - Word "overmodest" found 3 times: 
    	 - Top to bottom, from: [4,21] to [4,30]. 
    	 - Top to bottom, from: [6,11] to [6,20]. 
    	 - Top to bottom, from: [61,2] to [61,11]. 
    
    	 - Word "overmodestly" found 3 times: 
    	 - Top to bottom, from: [4,21] to [4,32]. 
    	 - Top to bottom, from: [6,11] to [6,22]. 
    	 - Top to bottom, from: [61,2] to [61,13]. 
    
    	 - Word "Owensville" found 3 times: 
    	 - Left to right, from: [23,14] to [32,14]. 
    	 - Left to right, from: [28,2] to [37,2]. 
    	 - Top to bottom, from: [61,49] to [61,58]. 
    
    Expected results are present, processing next test.


Also, as you probably already noticed, I took the liberty to modify signature of method `WordFinder.Find()` to return a structure with more data than only the word string for the TOP 10 of results.

The test also generates an output TXT file with all logs for the results, here is an example: [WordFinderOutput-2024-1-17-202839.txt](https://github.com/chapeti/WordFinder/files/13968859/WordFinderOutput-2024-1-17-202839.txt)

You can find it on the output folder for test project (WordFinderTests\Bin\Debug). With every new execution a new file is generated. Logs are also present on the output window.

Let me know if you want to discuss something.

Regards,
Sebastian.
