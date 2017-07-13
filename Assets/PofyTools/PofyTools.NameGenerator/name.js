/** v0.7
 Name Generator test

 **/
var ethicEnum = {
	LAWFUL: 0,
	NEUTRAL: 1,
	CHAOTIC: 2
};

var moralEnum = {
	EVIL: 0,
	NEUTRAL: 1,
	GOOD: 2
};

var firstName;
var lastName;
var title;

var gameObject = {
	maxSyllables: 3,
	minSyllables: 1,

	gameVersion: "0.7 Alpha",

	altmerNames: {
		firstNameStartMale: ["Aic", "Aica", "Aid", "Aide", "Ald", "Alda", "Ana", "Anar",
			"Anc", "Anca", "Anco", "And", "Andi", "Ang", "Anga", "Ango",
			"Ara", "Aran", "Are", "Arel", "Ari", "Arin", "Ark", "Arkv",
			"Arm", "Armi", "Arn", "Aro", "Aron", "Arr", "Arri", "Art",
			"Arte", "Asl", "Asli", "Ath", "Athe", "Cal", "Calc", "Cali",
			"Car", "Cara", "Care", "Cary", "Cel", "Cele", "Cir", "Ciri",
			"Core", "Corri", "Cyre", "Ear", "Eari", "Earm", "Eld", "Elda",
			"Eli", "Elid", "Enr", "Enri", "Era", "Eraa", "Err", "Erra",
			"Eru", "Erun", "Est", "Esto", "Fae", "Fael", "Fai", "Fain",
			"Fal", "Fala", "Falc", "Fan", "Fani", "Fas", "Fase", "Fii",
			"Fiir", "Gan", "Gil", "Gilg", "Gla", "Glad", "Gyr", "Gyrn",
			"Hal", "Hali", "Hec", "Hece", "Hen", "Hena", "Hid", "Hide",
			"Hin", "Hind", "Hir", "Hirt", "Hon", "Hond", "Hya", "Hyar",
			"Iac", "Iach", "Ilm", "Ilmi", "Ing", "Inga", "Iro", "Iror",
			"Ite", "Iter", "Jov", "Jovr", "Kae", "Kael", "Kala", "Kar",
			"Kard", "Kelkemme", "Kor", "Korn", "Lan", "Land", "Lilland",
			"Lin", "Linw", "Lit", "Lith", "Lovi", "Lyl", "Lyli", "Man",
			"Mank", "Mann", "Mea", "Mean", "Mel", "Mela", "Mer", "Mera",
			"Meri", "Mery", "Mith", "Mol", "Moll", "Mor", "Mora", "Moro",
			"Mos", "Moss", "Mur", "Muri", "Nae", "Naem", "Nan", "Nand",
			"Nel", "Nela", "Ner", "Neri", "Nor", "Nori", "Nur", "Nure",
			"Oca", "Ocat", "Oht", "Ohti", "Olq", "Olqu", "Ond", "Ondo",
			"Ori", "Orin", "Orm", "Ormi", "Ort", "Orth", "Pel", "Pell",
			"Qor", "Qorw", "Qua", "Quar", "Rav", "Rave", "Rim", "Rimi",
			"Rul", "Ruli", "Rum", "Ruma", "Run", "Runi", "Rya", "Ryai",
			"Sal", "Salm", "San", "Sany", "Saru", "Sau", "Sea", "Sean",
			"Ser", "Seri", "Sin", "Sind", "Siny", "Soli", "Sor", "Sorc",
			"Suu", "Suur", "Tan", "Tand", "Tau", "Taur", "Tel", "Teli",
			"Til", "Tilm", "Tra", "Trag", "Tre", "Trec", "Tum", "Tumi",
			"Tun", "Tune", "Tus", "Tusa", "Tye", "Tyer", "Ulu", "Ulun",
			"Umb", "Umba", "Und", "Undi", "Ung", "Unga", "Uul", "Uule",
			"Val", "Valm", "Via", "Viar", "Vin", "Ving", "Vol", "Vola",
			"Vor", "Vora", "Vori", "Yak", "Yako", "Yan", "Yann", "Yar",
			"Yarn"],

		firstNameEndMale: ["adilil", "alin", "almo", "alushorn", "an", "andial", "anirernil",
			"ano", "anonyaramen", "antar", "ar", "aramen", "aranyon", "arelenquar",
			"arengore", "aril", "arilmon", "arintil", "arion", "arnimarco", "asselimo",
			"athnertil", "calmoditar", "cano", "carryon", "celmo", "daenhesis", "dalf",
			"dil", "dilanaamo", "dildorume", "dilgondorin", "dur", "durcar", "ecalmo",
			"edaen", "edendil", "edil", "elde", "elerinde", "ellor", "elmoellith", "en",
			"enare", "enendor", "eninturco", "erion", "ertilim", "esisionil", "ewynn",
			"gothildil", "htus", "ianhnilian", "iel", "ielandil", "il", "ildroon", "ille",
			"ilroon", "imarcoindil", "imonwen", "indil", "ion", "ir", "iss", "itaracar",
			"lanar", "lanil", "las", "lcalin", "ldil", "ldilnen", "ldormo", "ldur", "le",
			"lemaramircil", "lian", "liongrim", "llenasse", "llithil", "llon", "llorantier",
			"lmoormo", "lndil", "maillin", "man", "mbarindil", "men", "mereluar", "milrmerel",
			"miondryn", "mircil", "mo", "mon", "naamokar", "nar", "narenquarien", "nargidur",
			"naro", "narto", "nath", "ndialtel", "ndil", "ndilaran", "ndildaril", "ndilgalmo",
			"ndilin", "ndilnalus", "ngore", "ngoth", "nian", "nil", "nilianranir", "nmil", "nmir",
			"noion", "ntar", "ntiersanon", "ntilrmo", "ntur", "nturco", "ochtus", "on", "onanarg",
			"ondorinildor", "onilmo", "oniril", "onl", "onniss", "onolemar", "onriel", "onrootan",
			"oonimbar", "ootan", "oov", "oril", "ornacano", "orron", "orumeladil", "otar", "ra",
			"ranil", "reanaro", "renen", "riil", "ril", "rilamil", "rilelion", "rillian", "rim",
			"rindeorurg", "rion", "rionamion", "rionion", "rmowe", "rnil", "ron", "roonyaran",
			"rurgderion", "rynil", "tarandil", "turrmaillin", "ve", "ved", "wennar", "yarel",
			"ynnarion", "yon"],

		firstNameStartFemale: ["Andra", "Ca", "Cymba", "Kori", "Lili", "Lora", "Mor", "Psy", "Saur", "Sha", "Valli", "Zeno", "Ani", "Ard", "Cal", "Cam", "Cel", "Cir", "Cul", "Cum", "Dha", "Ela", "Ela", "Eld", "Ele", "Eri", "Err", "Est", "Est", "Est", "Fai", "Fis", "Hel", "Hes", "Iir", "Ima", "Ini", "Iri", "Lor", "Mir", "Nal", "Sil", "Sir", "Son", "Taa", "Tar", "Ten", "Ter", "Vir", "Alw", "Ang", "Ara", "Ard", "Ari", "Arq", "Ary", "Ast", "Atr", "Aur", "Cam", "Car", "Car", "Cin", "Ear", "Eil", "Ela", "Eri", "Hli", "Kar", "Mir", "Muu", "Ner", "Oht", "Pal", "Ran", "Rum", "Son", "Tan", "Ter", "Var", "Ari", "Cur", "Ele", "End", "Far", "Min", "Nen", "Nir", "Nir", "Rel", "Taa", "Val", "Ayr", "Cam", "Gia", "Rav", "Tui", "Anir", "Arda", "Calm", "Cama", "Celr", "Cira", "Culu", "Cuma", "Dhau", "Elan", "Elan", "Elda", "Elen", "Eris", "Erra", "Esta", "Esti", "Esto", "Fair", "Fist", "Hele", "Hess", "Iire", "Imar", "Inie", "Irin", "Loru", "Mirk", "Nalc", "Silt", "Siri", "Sond", "Taar", "Tare", "Teny", "Term", "Vira", "Alwa", "Anga", "Aran", "Arda", "Arie", "Arqu", "Arya", "Asta", "Atra", "Aure", "Cami", "Cara", "Cara", "Cind", "Eara", "Eilo", "Elan", "Eris", "Hlid", "Kari", "Miri", "Muur", "Neru", "Ohte", "Palo", "Rana", "Ruma", "Sont", "Tand", "Term", "Varu", "Ariv", "Curw", "Elen", "Enda", "Fara", "Mino", "Neny", "Nira", "Niry", "Reld", "Taar", "Vali", "Ayre", "Cami", "Gial", "Rave", "Tuin"],
		firstNameEndFemale: ["aale", "ginia", "lia", "lina", "mia", "na", "sara", "sephona", "sha", "tha", "ae", "aen", "aena", "afire", "ahil", "aire", "alaure", "alda", "alenya", "alinde", "aline", "alsama", "ana", "ande", "andil", "anil", "aninde", "ante", "anwe", "anya", "anye", "ara", "arie", "arume", "arya", "ath", "aya", "carya", "daale", "danwe", "dara", "den", "dilwe", "dith", "ea", "el", "elinwae", "elle", "eminwe", "en", "ena", "ende", "ene", "enn", "enoore", "ente", "erane", "esse", "eth", "eya", "fire", "hil", "ia", "ie", "ilonwe", "ilwe", "inalda", "ine", "innarre", "ion", "irdalin", "ith", "krand", "lae", "lda", "lene", "lenya", "linde", "line", "linwae", "lonwe", "lsama", "maire", "maninde", "manwe", "na", "nae", "nalda", "nande", "nayne", "nde", "nden", "ndil", "ne", "nil", "ninde", "nirya", "nn", "nnarre", "noore", "nte", "nwe", "nwen", "nwy", "nya", "nye", "onirya", "onwy", "oril", "orne", "rae", "rand", "rane", "rdalin", "re", "ria", "rie", "ril", "rine", "rmend", "rne", "rume", "sare", "sion", "ssa", "ssare", "sse", "taire", "talaure", "te", "telle", "uen", "ulae", "umaire", "unayne", "urmend", "ussa", "vanya", "we", "wen", "ya", "yeminwe"],


		lastNameStart: ["Ad", "Caem", "Elsin", "Gae", "Gray", "High", "Jor", "Lareth", "Silin", "Spell",
			"Storm", "Throm", "Aed", "Cham", "Elsin", "Chae", "Gran", "Jaer", "Laraeth", "Saelin",
			"Thram", "Aed", "Cam", "Aelsin", "Kae", "Kor", "Larenh", "Sillon", "Khraem", "Ath",
			"Charm", "Aelson", "Gaeth", "Thaor", "Loraeth", "Saling", "Tahrom", "Anaed", "Laem",
			"Alkin", "Fae", "Lore", "Lareth", "Thilin", "Tahrom"],

		lastNameEnd: ["aire", "al", "binder", "ian", "ire", "ius", "lock", "or", "orin", "thar", "us", "watch",
			"aere", "ahl", "kaender", "aen", "ihre", "ious", "ore", "onin", "fhar", "eus", "aine",
			"ael", "iath", "ihle", "iuth", "our", "orinh", "thaer", "useus", "faere", "an", "iane",
			"ifeth", "thius", "oth", "aerith", "fhaer", "osin"],

		getFirstName: function () {
			if (gameObject.isMale) {
				var name = getRandomArrayIndex(this.firstNameStartMale) + getRandomArrayIndex(this.firstNameEndMale);
			} else {
				var name = getRandomArrayIndex(this.firstNameStartFemale) + getRandomArrayIndex(this.firstNameEndFemale);
			}
			return name;
		},

		getLastName: function () {
			var name = getRandomArrayIndex(this.lastNameStart) + getRandomArrayIndex(this.lastNameEnd);
			return name;
		},

		getFullName: function () {
			var name = this.getFirstName() + " " + this.getLastName();
			return name;
		},

	},
	amazonNames: {
		firstNameEnd: ["adia", "ameia", "anta", "asca", "cabe", "ce", "cleia", "cyone", "cyra", "da", "dae",
			"dia", "dice", "dora", "enice", "esia", "estra", "estris", "gea", "gone", "haedra",
			"hyia", "ippe", "isbe", "ises", "leia", "lene", "lete", "liope", "lipe", "lyte",
			"mache", "meia", "nache", "nara", "neira", "nestra", "nia", "nippe", "noe", "nousa",
			"ope", "padia", "pedo", "peia", "pesia", "phale", "pyle", "pyte", "rera", "reto",
			"roe", "scyra", "ses", "sippe", "sose", "tane", "thippe", "thoe", "thya", "thye",
			"thyia", "ybe", "yche", "yle", "yme", "yne", "yope", "yrbe", "ytie"],

		getFirstName: function () {
			var syllables = getNameSyllables();
			var syllablesLength = getSyllablesLength(syllables);
			var syllablesTypes = getSyllablesTypes(syllablesLength);
			var firstNameStart = getSyllablesStrings(syllablesTypes, syllablesLength, false);
			var name = concatName(firstNameStart) + getRandomArrayIndex(this.firstNameEnd);
			return name;
		},


	},

	argonianNames: {
		firstNameStartMale: ["Alex", "Antigon", "August", "Calig", "Claud", "Demer", "Dioclet", "German", "Her", "Jul", "Ner", "Pil", "Tib", "Asu", "Bun", "Bus", "Cha", "Chi", "Chu", "Chu", "Har", "Hat", "Hee", "Hul", "Huz", "Ine", "Ita", "Mee", "Mil", "Nee", "Oka", "Pee", "Ras", "Ree", "Ree", "See", "Ske", "Tan", "Tee", "Tul", "Uka", "Ula", "Uta", "Wee", "Wee", "Sis", "Yel", "Amu", "Dee", "Gee", "Jee", "Mah", "Otu", "Paj", "Ree", "Sak", "Sal", "Tee", "Tei", "Ush", "Wum", "Yin", "Dee", "Der", "Mad", "Nee", "Ush", "Vee", "Dee", "She", "Tsl", "Asum", "Buni", "Bush", "Chal", "Chiw", "Chul", "Chun", "Hara", "Hath", "Heed", "Hule", "Huze", "Inee", "Itan", "Meer", "Milo", "Neet", "Okaw", "Peer", "Rash", "Reem", "Rees", "Seew", "Skee", "Tana", "Teeg", "Tul", "Ukaw", "Ula", "Utad", "Weel", "Weer", "Siss", "Yeln", "Amus", "Deeh", "Geel", "Jeel", "Mahe", "Otum", "Paje", "Reen", "Sake", "Sali", "Teek", "Tein", "Ushe", "Wume", "Yinz", "Deek", "Derk", "Made", "Neet", "Usha", "Veez", "Deer", "Sheh", "Tsle"],
		firstNameEndMale: ["meya", "ish", "heeus", "lureel", "wish", "lz", "na", "an", "hei", "dul", "eeya", "ei", "erei", "neen", "raz", "os", "tinei", "wor", "radeeh", "ha", "mukeeus", "sa", "wul", "etul", "an", "gla", "wei", "deek", "ltul", "re", "sithik", "nicin", "sei", "haj", "leesh", "lius", "ei", "meel", "een", "num", "eepa", "iith", "keeus", "naava", "eeja", "eek", "zk", "kus", "keethus", "esi", "trenaza", "are", "zara", "rkaza", "hsi", "eeixth", "sh", "eeus", "ureel", "ish", "z"r","a"th", "nee", "ei", "ul", "eya", "il", "sehk", "inei", "adeeh", "akees", "ukeeus", "at", "ul", "tul", "nesh", "la", "ei", "eek", "tul", "ithik", "icin", "ei", "ius", "ieth", "eel", "en", "um", "epa", "ith", "eeus", "aava", "eja", "ek", "k"r","us","eethus","si","renaza","ara","kaza","s"r", "eixth", "acles", "andros", "ate", "erius", "ian", "iar", "icus", "ides", "ios", "ius", "os", "ula", "us"],

		lastNameStart: ["Andro", "Augus", "Ca", "Cae", "Cali", "Gal", "Mag", "Me", "Ni", "Per", "Theo", "Tiber", "Xer", "Andree", "Augees", "Caya", "Caree", "Calee", "Geel", "Nag", "Meree", "Nefe", "Peri", "Thefi", "Tikeer", "Xeir", "Endore", "Agius", "Cas", "Cay", "Cani", "Kay", "Laf", "Pe", "Neeth", "Pehr", "Theer", "Taier", "Xem"],
		lastNameEnd: ["cles", "des", "dorus", "gulus", "lus", "mean", "mus", "nes", "sar", "seus", "sion", "ssius", "tus", "calees", "desh", "dorees", "goulus", "lures", "mareen", "museeth", "mesh", "sareth", "sesh", "seene", "seus", "tius", "clesh", "daresh", "deseer", "galus", "leesh", "rean", "marush", "naresh", "sareeth", "teus", "sifon", "silus", "thees"],

		firstNameStartMale2: ["An", "Bun", "Bar", "Dan", "Effe", "Eleedal", "Gah", "Gam", "Geel", "Haj", "Han", "Heem", "Heir", "Im", "Jeelus", "Jeer", "J'Ram", "Junal", "Keerasa", "Miun", "Mush", "Okan", "Oleen", "Olink", "Reeh", "Silm", "Tee", "Tim", "Vistha", "Wanan", "Wih", "Wud", "Wuleen", "Ah", "Ajum", "Beem", "Dar", "Deetum", "Dreet", "Er", "Geem", "Gin", "Jee", "Jeetum", "Oleed", "Pad", "Tar", "Tun", "Weebam", "Beem", "Brand", "Gulum", "Ilas", "Jaree", "Talen", "Teeba", "Bah"],
		firstNameEndMale2: ["-Zaw", "-Teemeeta", "-Ru", "-Tei", "-Lei", "-Julan", "-Kur", "-Lah", "-Ei", "-Tulm", "-La", "-Zish", "-Kilaya", "-Tei", "-Maht", "-Dar", "-Lei", "-Tan", "-Gei", "-Mere", "-Shei", "-Gei", "-Nur", "-Jah", "-Dar", "-Lan", "-Jush", "-Kai", "-Dum", "-Eius", "-Neeus", "-Shei", "-Malz", "-Kajin", "-Kiurz", "-Jee", "-Ja", "-Ju", "-Lai", "-Teeus", "-Jasaiin", "-Wulm", "-Tah", "-Ze", "-Ei", "-Ei", "-Meena", "-Zeeus", "-Na", "-Ja", "-Shei", "-Ei", "-Tei", "-Ra", "-Jei", "-Ei"],

		firstNameStartFemale: ["Ahah", "Akis", "Bana", "Beek", "Eute", "Gilm", "Gish", "Hule", "Kasa", "Mila", "Naku", "Nees", "Nura", "Nush", "Okur", "Onas", "Shat", "Tash", "Wush", "Beel", "Beew", "Beje", "Deet", "Druj", "Marz", "Nume", "Oche", "Pash", "Rana", "Shal", "Skal", "Wits", "Deej", "Keer", "Shah", "Wuje", "Aha", "Aki", "Ban", "Bee", "Eut", "Gil", "Gis", "Hul", "Kas", "Mil", "Nak", "Nee", "Nur", "Nus", "Oku", "Ona", "Sha", "Tas", "Wus", "Bee", "Bee", "Bej", "Dee", "Dru", "Mar", "Num", "Och", "Pas", "Ran", "Sha", "Ska", "Wit", "Dee", "Kee", "Sha", "Wuj"],
		firstNameEndFemale: ["ht", "sh", "alz", "katan", "ei", "mee", "hee", "ayee", "ah", "uma", "sha", "alg", "h'r", "resh", "sha", "talg", "ha", "ha", "lei", "wos", "een", "tsan", "ja", "z"r","een","eeva","ha","aye","leez","leel","seidutsei","ja","rava","hvee","eeta","tvee","haz","lz","atan","ila","hota","ma","ha","lg","ha","alg","aree","aza","ei","os","en","san","araje","en","eva","a","eez","eel","eedutsee","ayo","ava","vee","eta"],
		
		firstNameStartFemale2 : ["Ah", "Am", "An", "Bur", "Chanil", "Cheesh", "Dar", "Deesh", "El", "Ereel", "Gih", "Hal", "Jeed", "Kal", "Keel", "Kud", "Mach", "Meeh", "Meen", "Mim", "Muz", "Nam", "Olank", "On", "Seed", "Seen", "Sheer ", "Tar", "Weedum", "Erh", "Amee", "Aney", "Bour", "Cheenal", "Chesoh", "Dur", "Deeth", "En", "Ereej", "Ginh", "Kahl", "Jid", "Kani", "Kelo", "Kudo", "Meech", "Meefh", "Mereen", "Meem", "Mez", "Nem", "Olink", "Oneer", "Sedir", "Tereen", "Sheef ", "Thari", "Wedum"],
			firstNameEndFemale2 : ["-Deesei", "-Ei", "-Ja", "-Jeen", "-La", "-Lee", "-Lei", "-Liurz", "-Lurasha", "-Ma", "-Meedish", "-Meema", "-Meena", "-Meesei", "-Meeus", "-Mei", "-Na", "-Neeus", "-Ra", "-Raniur", "-Rei", "-Sa", "-Wan", "-Wazei", "-Deese", "-Eji", "-Jazee", "-Jereen", "-Lari", "-Leef", "-Leith", "-Liruz", "-Lursha", "-Maki", "-Mideesh", "-Meemar", "-Menari", "-Meseif", "-Meefus", "-Meik", "-Nash", "-Nevus", "-Rafee", "-Ranier", "-Reij", "-Kajee", "-Wahn", "-Wareih", "-Deseith", "-Eijar", "-Jarlee", "-Jeleen", "-Lak", "-Leehp", "-Leish", "-Lirzee", "-Leesha", "-Majee", "-Meedish", "-Mena", "-Meeka", "-Meei", "-Neeus", "-Slei", "-Nha", "-Nefeus", "-Rajee", "-Raneer", "-Reiki", "-Sakee", "-Ran", "-Razei"],

			getFirstName: function () {
				if (gameObject.isMale) {
					var name = (Math.random() < 0.5) ? getRandomArrayIndex(this.firstNameStartMale) + getRandomArrayIndex(this.firstNameEndMale) : getRandomArrayIndex(this.firstNameStartMale2) + getRandomArrayIndex(this.firstNameEndMale2);
				} else {
					var name = (Math.random() < 0.5) ? getRandomArrayIndex(this.firstNameStartFemale) + getRandomArrayIndex(this.firstNameEndFemale) : getRandomArrayIndex(this.firstNameStartFemale2) + getRandomArrayIndex(this.firstNameEndFemale2);
				}
				return name;
			},

			getLastName: function () {
				var name = getRandomArrayIndex(this.lastNameStart) + getRandomArrayIndex(this.lastNameEnd);
				return name;
			},

			getFullName: function () {
				var name = this.getFirstName() + " " + this.getLastName();
				return name;
			},
	
	},
	serbianNames: {
		frstNameMale: ["Avram", "Adam", "Aksentije", "Aleksa", "Aleksandar", "Alimpije", "Anđelko",
			"Andrija", "Antonije", "Aranđel", "Arsenije", "Atanasije", "Aćim", "Aca",
			"Aco", "Bane", "Blagoje", "Blagomir", "Blaža", "Bogdan", "Bogoljub", "Bogomir", "Bogosav", "Borko",
			"Božidar", "Bojan", "Borivoje", "Borisav", "Borislav", "Boško", "Branimir", "Bratislav", "Branko",
			"Bratoljub", "Budimir", "Budislav", "Buda", "Vasilije", "Vekoslav", "Velibor", "Velimir", "Velislav", "Veljko",
			"Veroljub", "Veselin", "Vidak", "Vidoje", "Viktor", "Vitomir", "Višeslav", "Vladan", "Vladeta", "Vladimir", "Vladislav", "Vlajko",
			"Vlastimir", "Vojimir", "Vojin", "Vojislav", "Vujadin", "Vuk", "Vukadin", "Vukajlo", "Vukan", "Vukašin", "Vukoje", "Vukosav", "Vukota",
			"Vuksan", "Vučko", "Vuča", "Gavra", "Gavrilo", "Gvozden", "Georgije", "Gligorije", "Gojko", "Golub", "Goran", "Gradimir", "Gradiša", "Grigorije", "Grozdan", "Gruja", "Grujica",
			"David", "Dalibor", "Damjan", "Danilo", "Danojlo", "Darko", "Dejan", "Desimir", "Despot", "Dimitrije", "Dmitar",
			"Dobrašin", "Dobrivoj", "Dobrilo", "Dobrica", "Dobrosav", "Dositej", "Dragan", "Dragiša", "Drago", "Dragoje", "Dragojlo", "Dragoljub", "Dragomir", "Dragorad", "Dragoslav", "Dragutin", "Dražo", "Draško", "Dušan",
		],
		firstNameFemale: [],

		getFirstName: function () {
			var name = getRandomArrayIndex(this.frstNameMale);
			return name;
		},
		getLastName: function () {
			var lastname;
			var name = getRandomArrayIndex(this.frstNameMale);
			switch (name.charAt(name.length - 1)) {

				case "o":
					lastname = name + "vić";
					break;

				case "e":
					if (name.charAt(name.length - 2) == "j") {
						lastname = name + "vić";
					} else {
						lastname = name.slice(0, name.length - 1) + "ović";
					}
					break;

				case "a":
					if (name.charAt(name.length - 2) == "j") {
						lastname = name.slice(0, name.length - 1) + "ević";
					} else if (name.charAt(name.length - 2) == "c") {
						lastname = name.slice(0, name.length - 2) + "čić";
					} else {
						lastname = name.slice(0, name.length - 1) + "ić";
					}
					break;

				case "v":
					if (name.charAt(name.length - 2) == "a") {
						lastname = name + "ljević";
					}
					else {
						lastname = name + "ić"
					}
					break;
				case "r":
					if (name.charAt(name.length - 2) == "a") {
						lastname = name + "ević";
					}
					else {
						lastname = name + "ović"
					}
					break;

				default:
					lastname = name + "ović";
			}
			return lastname;
		},

		getFullName: function () {
			var fullname = this.getFirstName() + " " + this.getLastName();
			return fullname;
		},
	},

	vowelArray: ["a", "e", "i", "o", "u"],
	vowelPairArray: ["ee", "oo", "io", "ea", "ia", "oue", "ay", "ey"],


	consonantCloseArray: ["ck", "b", "d", "f", "g", "k", "m", "n", "p", "r", "ss", "t",
		"w", "zz", "ld", "nd", "nt", "th", "sh", "wn",
		"xt", "ff"],

	consonantOpenArray: ["b", "c", "d", "f", "g", "h", "k", "l", "m", "n", "p",
		"r", "s", "t", "v", "z", "qu",
		"ch", "gh", "ph", "sh", "th",
		"wh", "zh", "bl", "br", "cl", "cr",
		"dm", "dn", "dr", "ds", "dv", "dw", "f", "fl",
		"fr", "gd", "g", "gl", "gm", "gn", "gr",
		, "hr", "hy", "kl", "km",
		"kn", "kr", "ks", "kv", "kw", "ky", "ml", "mn",
		"mr", "mw", "pl", "pr", "sc",
		"sk", "sl", "sm", "sn", "sp", "squ", "sr", "st",
		"str", "sv", "tl", "tr",
		"wl", "wr", "xn", "xt", "xz",
	],

	consonantStartArray: ["b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "n", "p",
		"r", "s", "t", "v", "w", "z", "qu",
		"ch", "gh", "ph", "rh", "sh", "th",
		"wh", "zh", "bl", "br", "cl", "cr",
		"dm", "dn", "dr", "ds", "dv", "dw", "f", "fl",
		"fr", "gd", "g", "gl", "gm", "gn", "gr",
		"gy", "h", "hl", "hr", "hy", "kl", "km",
		"kn", "kr", "ks", "kv", "kw", "ky", "ml", "mn",
		"mr", "mw", "pl", "pr", "ps", "pw", "sc", "s",
		"sk", "sl", "sm", "sn", "sp", "squ", "sr", "st",
		"str", "sv", "tl", "tr", "vl", "vr", "wr",
		"xn", "xt", "xr", "xz", "gw", "sw", "tw", "zb", "zbr",
		"zd", "zdr", "zg", "zgr", "zl", "zv", "zvr", "zw", "zwr"],

	femaleEndSyllablesOpen: ["la", "neah", "na", "ly", "lay", "el", "bella", "ra", "da", "ta", "ca", "va"],
	femaleEndSyllablesClose: ["ela", "ana", "ea", "eah", "a", "y", "el", "ien", "ica", "da", "ida", "ita", "ira", "ia", "iva", "ova"],

	maleEndSyllablesOpen: ["llio", "mor", "mom", "mol", "nam", "nar", "nal", "vas",
		"mon", "mir", "sav", "ter", "mud", "slav", "rod", "van",
		"cco", "tar", "dor", "lan", "drag", "ler", "trick", "ck", "nad",
		"den", "las", "s", "la", "ran", "rin", "dos", "drew", "gan", "gunn", "ple", "tz"],

	maleEndSyllablesClose: ["ello", "on", "or", "om", "ol", "am", "ar", "al", "as", "mon",
		"mir", "mor", "av", "ub", "er", "cco", "ud", "slav", "an", "ack", "etz"],

	storyName: ["book", "tome", "scroll", "scripture", "writings", "advanture", "legend", "tale", "song", "story", "quest", "rhyme", "history", "memoars", "dairy"],
