namespace Wholemy {
	/// <summary>
	/// Большое дробное число, на базе делимого и делителя)
	/// </summary>
	/// <remarks>
	/// При MaxDepth равном 100, эффективная точность в вычислениях
	/// тригонометрических функций будет чуть более 50 знаков после запятой)
	/// Единственный рациональный способ сделать разную глубину, дублировать все содержимое
	/// структуры с другим именованием полностью и дополнить методы конвертирования значений из других структур)
	/// </remarks>
	public struct BugNum {
		static BugNum() { InitDepthConsts(); }
		public static readonly BugNum Zer = new BugNum() { Numer = 0, Venom = 1 };
		public static readonly BugNum One = new BugNum() { Numer = 1, Venom = 1 };
		public static readonly BugNum Two = new BugNum() { Numer = 2, Venom = 1 };
		public static readonly BugNum Mon = new BugNum() { Numer = -1, Venom = 1 };
		public static readonly BugNum V05 = new BugNum() { Numer = 1, Venom = 2 };
		public static readonly BugNum Nan = new BugNum();
		public static readonly BugNum Pos = new BugNum() { Numer = 1 };
		public static readonly BugNum Neg = new BugNum() { Numer = -1 };
		#region #get# IsVal 
		public bool IsVal => Venom >= 1;
		#endregion
		#region #get# IsNan 
		/// <summary>Возвращает истину если значение не задано)</summary>
		#region #invisible# 
#if TRACE
		[System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
#endif
		#endregion
		public bool IsNan => Venom == 0 && Numer == 0;
		#endregion
		#region #get# IsInf 
		/// <summary>Возвращает истину если значение бесконечно)</summary>
		#region #invisible# 
#if TRACE
		[System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
#endif
		#endregion
		public bool IsInf => Venom == 0 && Numer != 0;
		#endregion
		#region #get# IsNeg 
		/// <summary>Возвращает истину если значение бесконечно негативно)</summary>
		#region #invisible# 
#if TRACE
		[System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
#endif
		#endregion
		public bool IsNeg => Venom == 0 && Numer < 0;
		#endregion
		#region #get# IsPos 
		/// <summary>Возвращает истину если значение бесконечно позитивно)</summary>
		#region #invisible# 
#if TRACE
		[System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
#endif
		#endregion
		public bool IsPos => Venom == 0 && Numer > 0;
		#endregion
		#region #field# Numer 
		public BugInt Numer;
		#endregion
		#region #field# Venom 
		public BugInt Venom;
		#endregion
		#region #property# MaxDepth 
		/// <summary>Максимальная глубина числа для его округления при инициализации)</summary>
		public static int MaxDepth {
			get { return maxDepth; }
			set { if (value < 0) value = -value; if (maxDepth != value) { maxDepth = value; InitDepthConsts(); } }
		}
		#endregion
		#region #method# InitDepthConsts 
		private static void InitDepthConsts() {
			maxVenom = BugInt.Pow(10, maxDepth);
			if (maxChars > maxDepth) maxChars = maxDepth;
			PI = new BugNum("3.1415926535897932384626433832795028841971693993751058209749445923078164062862089986280348253421170679821480865132823066470938446095505822317253594081284811174502841027019385211055596446229489549303819644288109756659334461284756482337867831652712019091456485669234603486104543266482133936072602491412737245870066063155881748815209209628292540917153643678925903600113305305488204665213841469519415116094330572703657595919530921861173819326117931051185480744623799627495673518857527248912279381830119491298336733624406566430860213949463952247371907021798609437027705392171762931767523846748184676694051320005681271452635608277857713427577896091736371787214684409012249534301465495853710507922796892589235420199561121290219608640344181598136297747713099605187072113499999983729780499510597317328160963185950244594553469083026425223082533446850352619311881710100031378387528865875332083814206171776691473035982534904287554687311595628638823537875937519577818577805321712268066130019278766111959092164201989380952572010654858632788659361533818279682303019520353018529689957736225994138912497217752834791315155748572424541506959508295331168617278558890750983817546374649393192550604009277016711390098488240128583616035637076601047101819429555961989467678374494482553797747268471040475346462080466842590694912933136770289891521047521620569660240580381501935112533824300355876402474964732639141992726042699227967823547816360093417216412199245863150302861829745557067498385054945885869269956909272107975093029553211653449872027559602364806654991198818347977535663698074265425278625518184175746728909777727938000816470600161452491921732172147723501414419735685481613611573525521334757418494684385233239073941433345477624168625189835694855620992192221842725502542568876717904946016534668049886272327917860857843838279679766814541009538837863609506800642251252051173929848960841284886269456042419652850222106611863067442786220391949450471237137869609563643719172874677646575739624138908658326459958133904780275900994657640789512694683983525957098258226205224894077267194782684826014769909026401363944374553050682034962524517493996514314298091906592509372216964615157098583874105978859597729754989301617539284681382686838689427741559918559252459539594310499725246808459872736446958486538367362226260991246080512438843904512441365497627807977156914359977001296160894416948685558484063534220722258284886481584560285060168427394522674676788952521385225499546667278239864565961163548862305774564980355936345681743241125150760694794510965960940252288797108931456691368672287489405601015033086179286809208747609178249385890097149096759852613655497818931297848216829989487226588048575640142704775551323796414515237462343645428584447952658678210511413547357395231134271661021359695362314429524849371871101457654035902799344037420073105785390621983874478084784896833214457138687519435064302184531910484810053706146806749192781911979399520614196634287544406437451237181921799983910159195618146751426912397489409071864942319615679452080951465502252316038819301420937621378559566389377870830390697920773467221825625996615014215030680384477345492026054146659252014974428507325186660021324340881907104863317346496514539057962685610055081066587969981635747363840525714591028970641401109712062804390397595156771577004203378699360072305587631763594218731251471205329281918261861258673215791984148488291644706095752706957220917567116722910981690915280173506712748583222871835209353965725121083579151369882091444210067510334671103141267111369908658516398315019701651511685171437657618351556508849099898599823873455283316355076479185358932261854896321329330898570642046752590709154814165498594616371802709819943099244889575712828905923233260972997120844335732654893823911932597463667305836041428138830320382490375898524374417029132765618093773444030707469211201913020330380197621101100449293215160842444859637669838952286847831235526582131449576857262433441893039686426243410773226978028073189154411010446823252716201052652272111660396665573092547110557853763466820653109896526918620564769312570586356620185581007293606598764861179104533488503461136576867532494416680396265797877185560845529654126654085306143444318586769751456614068007002378776591344017127494704205622305389945613140711270004078547332699390814546646458807972708266830634328587856983052358089330657574067954571637752542021149557615814002501262285941302164715509792592309907965473761255176567513575178296664547791745011299614890304639947132962107340437518957359614589019389713111790429782856475032031986915140287080859904801094121472213179476477726224142548545403321571853061422881375850430633217518297986622371721591607716692547487389866549494501146540628433663937900397692656721463853067360965712091807638327166416274888800786925602902284721040317211860820419000422966171196377921337575114959501566049631862947265473642523081770367515906735023507283540567040386743513622224771589150495309844489333096340878076932599397805419341447377441842631298608099888687413260472156951623965864573021631598193195167353812974167729478672422924654366800980676928238280689964004824354037014163149658979409243237896907069779422362508221688957383798623001593776471651228935786015881617557829735233446042815126272037343146531977774160319906655418763979293344195215413418994854447345673831624993419131814809277771038638773431772075456545322077709212019051660962804909263601975988281613323166636528619326686336062735676303544776280350450777235547105859548702790814356240145171806246436267945612753181340783303362542327839449753824372058353114771199260638133467768796959703098339130771098704085913374641442822772634659470474587847787201927715280731767907707157213444730605700733492436931138350493163128404251219256517980694113528013147013047816437885185290928545201165839341965621349143415956258658655705526904965209858033850722426482939728584783163057777560688876446248246857926039535277348030480290058760758251047470916439613626760449256274204208320856611906254543372131535958450687724602901618766795240616342522577195429162991930645537799140373404328752628889639958794757291746426357455254079091451357111369410911939325191076020825202618798531887705842972591677813149699009019211697173727847684726860849003377024242916513005005168323364350389517029893922334517220138128069650117844087451960121228599371623130171144484640903890644954440061986907548516026327505298349187407866808818338510228334508504860825039302133219715518430635455007668282949304137765527939751754613953984683393638304746119966538581538420568533862186725233402830871123282789212507712629463229563989898935821167456270102183564622013496715188190973038119800497340723961036854066431939509790190699639552453005450580685501956730229219139339185680344903982059551002263535361920419947455385938102343955449597783779023742161727111723643435439478221818528624085140066604433258885698670543154706965747458550332323342107301545940516553790686627333799585115625784322988273723198987571415957811196358330059408730681216028764962867446047746491599505497374256269010490377819868359381465741268049256487985561453723478673303904688383436346553794986419270563872931748723320837601123029911367938627089438799362016295154133714248928307220126901475466847653576164773794675200490757155527819653621323926406160136358155907422020203187277605277219005561484255518792530343513984425322341576233610642506390497500865627109535919465897514131034822769306247435363256916078154781811528436679570611086153315044521274739245449454236828860613408414863776700961207151249140430272538607648236341433462351897576645216413767969031495019108575984423919862916421939949072362346468441173940326591840443780513338945257423995082965912285085558215725031071257012668302402929525220118726767562204154205161841634847565169998116141010029960783869092916030288400269104140792886215078424516709087000699282120660418371806535567252532567532861291042487761825829765157959847035622262934860034158722980534989650226291748788202734209222245339856264766914905562842503912757710284027998066365825488926488025456610172967026640765590429099456815065265305371829412703369313785178609040708667114965583434347693385781711386455873678123014587687126603489139095620099393610310291616152881384379099042317473363948045759314931405297634757481193567091101377517210080315590248530906692037671922033229094334676851422144773793937517034436619910403375111735471918550464490263655128162288244625759163330391072253837421821408835086573917715096828874782656995995744906617583441375223970968340800535598491754173818839994469748676265516582765848358845314277568790029095170283529716344562129640435231176006651012412006597558512761785838292041974844236080071930457618932349229279650198751872127267507981255470958904556357921221033346697499235630254947802490114195212382815309114079073860251522742995818072471625916685451333123948049470791191532673430282441860414263639548000448002670496248201792896476697583183271314251702969234889627668440323260927524960357996469256504936818360900323809293459588970695365349406034021665443755890045632882250545255640564482465151875471196218443965825337543885690941130315095261793780029741207665147939425902989695946995565761218656196733786236256125216320862869222103274889218654364802296780705765615144632046927906821207388377814233562823608963208068222468012248261177185896381409183903673672220888321513755600372798394004152970028783076670944474560134556417254370906979396122571429894671543578468788614445812314593571984922528471605049221242470141214780573455105008019086996033027634787081081754501193071412233908663938339529425786905076431006383519834389341596131854347546495569781038293097164651438407007073604112373599843452251610507027056235266012764848308407611830130527932054274628654036036745328651057065874882256981579367897669742205750596834408697350201410206723585020072452256326513410559240190274216248439140359989535394590944070469120914093870012645600162374288021092764579310657922955249887275846101264836999892256959688159205600101655256375678");
			PId2 = PI / 2;
			PId4 = PI / 4;
			PId2x3 = PI + PId2;
			PIx2 = PI * 2;
			PIx4 = PI * 4;
		}
		#endregion
		#region #field# maxDepth 
		private static int maxDepth = 50;
		#endregion
		#region #field# maxVenom 
		private static BugInt maxVenom = BugInt.Pow(10, maxDepth);
		#endregion
		#region #field# maxChars 
		/// <summary>
		/// Максимальное количество выводимых в строку символов дроби) Значение больше MaxDepth не достижимо)
		/// </summary>
		/// <remarks>
		/// Как правило половина от дробной глубины числа эффективна при вычислении 
		/// значений сложных тригонометрических функций, затем и нужно это ограничение)
		/// Например, для вычисления квадратного корня, требуется вдвое больше число,
		/// но результат у него достоверный по всей вычисляемой длине дроби)
		/// </remarks>
		public static int MaxChars {
			get { return maxChars; }
			set { if (value < 0) value = -value; if (value > maxDepth) maxChars = maxDepth; else maxChars = value; }
		}
		private static int maxChars = maxDepth / 2;
		#endregion
		#region #field# PI 
		public static BugNum PI;
		#endregion
		#region #field# PId2 
		public static BugNum PId2;
		#endregion
		#region #field# PId4 
		public static BugNum PId4;
		#endregion
		#region #field# PId2x3 
		public static BugNum PId2x3;
		#endregion
		#region #field# PIx2 
		public static BugNum PIx2;
		#endregion
		#region #field# PIx4 
		public static BugNum PIx4;
		#endregion
		#region #new# (Value) 
		#region #through# 
#if TRACE
		[System.Diagnostics.DebuggerStepThrough]
#endif
		#endregion
		public BugNum(BugNum Value) : this(Value.Numer, Value.Venom) { }
		#endregion
		#region #new# (Numer, Venom) 
		/// <summary>Инициализация большого числа)</summary>
		/// <param name="Numer">Целое если Venom меньше или равен 0, в остальных случаях это делимое)</param>
		/// <param name="Venom">Делитель если больше или равен 1 и дробное если меньше или равен 0)</param>
		public BugNum(BugInt Numer, BugInt Venom) {
			if (Venom == 0) {
				Venom = 1;
			} else {
				var MV = maxVenom;
				if (Venom < 0) {
					if (Venom > MV) {
						var P = Venom / MV;
						if (P > 1) { Venom /= P; }
					}
					Numer = Venom + (Numer * MV);
					Venom = MV;
				} else if (Venom > MV) {
					var P = Venom / MV;
					if (P > 1) {
						Venom /= P;
						Numer /= P;
					}
				}
				if (Venom == 0) Venom = 1;
			}
			this.Numer = Numer;
			this.Venom = Venom;
		}
		#endregion
		#region #new# (#string # Value) 
		public BugNum(string Value) {
			if (Value == null || Value.Length == 0) { this.Venom = 0; this.Numer = 0; return; }
			char Char;
			var Length = Value.Length;
			if (Length == 3) {
				Char = Value[0];
				if (Char == 'n' || Char == 'N') {
					Char = Value[1];
					if (Char == 'a' || Char == 'A') {
						Char = Value[2];
						if (Char == 'n' || Char == 'N') { this.Venom = 0; this.Numer = 0; return; }
					} else if (Char == 'e' || Char == 'E') {
						Char = Value[2];
						if (Char == 'g' || Char == 'G') { this.Venom = 0; this.Numer = -1; return; }
					}
				} else if (Char == 'p' || Char == 'P') {
					Char = Value[1];
					if (Char == 'o' || Char == 'O') {
						Char = Value[2];
						if (Char == 's' || Char == 'S') { this.Venom = 0; this.Numer = 1; return; }
					}
				}
			}
			var Chars = new char[Length];
			var Split = -1;
			var Count = 0;
			var Minus = "";
			for (var Index = 0; Index < Length; Index++) {
				Char = Value[Index];
				if(Char == '-'&& Count==0) {
					Minus = "-";
				} else if (Char >= '0' && Char <= '9') {
					Chars[Count++] = Char;
				} else if (Split == -1 && (Char == '.' || Char == ',')) {
					Split = Count;
				}
			}
			if (Count == 0) { this.Venom = 0; this.Numer = 0; return; }
			if (Split == -1) { this.Numer = new BugInt(Minus+new string(Chars, 0, Count)); this.Venom = 1; return; }
			var Depth = Count - Split;
			while (Count > 0 && Depth > 0) { if (Chars[Count - 1] == '0') { Count--; Depth--; } else break; }
			if (Depth == 0) { this.Numer = new BugInt(Minus + new string(Chars, 0, Count)); this.Venom = 1; return; }
			if (Depth > maxDepth) { Depth = maxDepth; Count = Depth + Split; }
			this.Numer = new BugInt(Minus + new string(Chars, 0, Count)); this.Venom = BugInt.Pow(10, Depth);
		}
		#endregion
		#region #property# GcdNum 
		public BugNum GcdNum {
			get {
				var Numer = this.Numer;
				var Venom = this.Venom;
				if (Numer != 0 && Venom != 0) {
					var Gcd = BugInt.Gcd(Numer, Venom);
					if (Gcd > 1) { Numer /= Gcd; Venom /= Gcd; }
				}
				return new BugNum(Numer, Venom/*, Depth*/);
			}
		}
		public BugInt Gcd {
			get {
				return BugInt.Gcd(this.Numer, this.Venom);
			}
		}
		#endregion
		#region #method# GetGcd(Numer, Venom) 
		private static BugInt GetGcd(ref BugInt Numer, ref BugInt Venom) {
			var Num = Numer;
			var Ven = Venom;
			do { var X = Num % Ven; Num = Ven; Ven = X; } while (Ven != 0);
			Numer /= Num;
			Venom /= Num;
			return Num;
		}
		#endregion
		#region #method# Round(Count) 
		public BugNum Round(int Count) {
			var Numer = this.Numer;
			var Venom = this.Venom;
			if (Count == 0) return new BugNum(Numer / Venom, 1);
			if (Count < 0) {
				var Max = BugInt.Bit(-Count);
				if (Numer > Max) {
					var D = Venom / Max;
					if (D > 1) {
						Venom /= D;
						Numer /= D;
						if (Venom == 0) Venom = 1;
					}
				}
			} else {
				var Max = BugInt.Pow(10, Count);
				if (Venom > Max) {
					var D = Venom / Max;
					if (D > 1) {
						Venom /= D;
						Numer /= D;
					}
				}
			}
			return new BugNum(Numer, Venom);
		}
		#endregion
		#region #method# ToString 
		public override string ToString() {
			var Sign = "";
			var Numer = this.Numer;
			var Venom = this.Venom;
			if (Venom == 0) {
				if (Numer == 0) return "Nan";
				if (Numer < 0) return "Neg";
				return "Pos";
			}
			if (Numer < 0) { Numer = -Numer; Sign = "-"; }
			if (Venom > 0) {
				Sign += BugInt.DivMod(Numer, Venom, out var Num).ToString();
				Numer = Num * maxVenom / Venom;
				if (Numer > 0) {
					Sign += '.';
					var Decimal = "";
					var Zeros = maxDepth - Numer.Digits;
					if (Zeros > 0) Decimal += new string('0', Zeros);
					Zeros = Numer.Zerone; if (Zeros < 0) Zeros = -Zeros;
					if (Zeros > 0) Numer /= BugInt.Pow(10u, Zeros);
					Decimal += Numer.ToString();
					if (Decimal.Length > maxChars) Decimal = Decimal.Substring(0, maxChars);
					Sign += Decimal;
				}
			} else {
				Venom = -Venom;
				Sign += Numer.ToString();
				Sign += '.';
				var Decimal = Venom.ToString();
				if (Decimal.Length > maxChars) Decimal = Decimal.Substring(0, maxChars);
				Sign += Decimal;
			}
			return Sign;
		}
		#endregion
		#region #operator# / 
		public static BugNum operator /(BugNum L, BugNum R) {
			if (R.Numer == 0) throw new System.DivideByZeroException("R");
			return new BugNum(L.Numer * R.Venom, L.Venom * R.Numer);
		}
		public static BugNum operator /(BugNum L, int R) {
			if (R == 0) throw new System.DivideByZeroException("R");
			return new BugNum(L.Numer, L.Venom * R);
		}
		#endregion
		#region #operator# % 
		public static BugNum operator %(BugNum L, BugNum R) {
			if (R.Numer == 0) throw new System.DivideByZeroException("R");
			var D = new BugNum(L.Numer * R.Venom, L.Venom * R.Numer);
			var N = D.Numer;
			if (N < 0) { N += D.Venom - (N % D.Venom); } else { N -= (N % D.Venom); }
			return L - new BugNum(N, D.Venom) * R;
		}
		public static BugNum operator %(BugNum L, int R) {
			if (R == 0) throw new System.DivideByZeroException("R");
			var D = new BugNum(L.Numer, L.Venom * R);
			var N = D.Numer;
			if (N < 0) { N += D.Venom - (N % D.Venom); } else { N -= (N % D.Venom); }
			return L - new BugNum(N, D.Venom) * R;
		}
		#endregion
		#region #operator# * 
		public static BugNum operator *(BugNum L, BugNum R) {
			return new BugNum(L.Numer * R.Numer, R.Venom * L.Venom);
		}
		public static BugNum operator *(BugNum L, int R) {
			return new BugNum(L.Numer * R, L.Venom);
		}
		#endregion
		#region #operator# - 
		public static BugNum operator -(BugNum L, BugNum R) {
			return new BugNum(L.Numer * R.Venom - R.Numer * L.Venom, L.Venom * R.Venom);
		}
		#endregion
		#region #operator# + 
		public static BugNum operator +(BugNum L, BugNum R) {
			return new BugNum(L.Numer * R.Venom + R.Numer * L.Venom, L.Venom * R.Venom);
		}
		#endregion
		#region #method# Pow(L, E) 
		public static BugNum Pow(BugNum L, int E) {
			if (L.Numer == 0) return L;
			if (E < 0) {
				BugInt numerator = L.Numer;
				BugInt numerator2 = BugInt.Pow(L.Venom, -E);
				BugInt denominator = BugInt.Pow(numerator, -E);
				return new BugNum(numerator2, denominator);
			}
			BugInt numerator3 = BugInt.Pow(L.Numer, E);
			BugInt denominator2 = BugInt.Pow(L.Venom, E);
			return new BugNum(numerator3, denominator2);
		}
		#endregion
		#region #implicit operator# (#double # Value) 
		#region #through# 
#if TRACE
		[System.Diagnostics.DebuggerStepThrough]
#endif
		#endregion
		public static implicit operator BugNum(double Value) {
			return new BugNum(Value);
		}
		#endregion
		#region #new# (Value) 
		#region #new# (#int # Value) 
		public BugNum(int Value) {
			this.Numer = Value;
			this.Venom = 1;
		}
		#endregion
		#region #new# (#uint # Value) 
		public BugNum(uint Value) {
			this.Numer = Value;
			this.Venom = 1;
		}
		#endregion
		#region #new# (#long # Value) 
		public BugNum(long Value) {
			this.Numer = Value;
			this.Venom = 1;
		}
		#endregion
		#region #new# (#ulong # Value) 
		public BugNum(ulong Value) {
			this.Numer = Value;
			this.Venom = 1;
		}
		#endregion
		#region #new# (#double # Value) 
		public BugNum(double Value) {
			var Minus = false;
			if (Value < 0) { Value = -Value; Minus = true; }
			var Num = new BugInt(0);
			var Cnt = 0;
			while (Value >= 1) {
				Value /= 10;
				Cnt++;
			}
			while (Cnt > 0) {
				Value *= 10;
				var Int = (int)Value;
				Num *= 10;
				Num += Int;
				Value -= Int;
				Cnt--;
			}
			var Pow = 0;
			while (Value > 0) {
				Pow++;
				Num *= 10;
				Value *= 10;
				var I = (uint)Value;
				Num += I;
				Value -= I;
			}
			this.Numer = Minus ? -Num : Num;
			this.Venom = BugInt.Pow(10, Pow);
		}
		#endregion
		#endregion
		#region #operator# +(#struct # value)
		#region #through# 
#if TRACE
		[System.Diagnostics.DebuggerStepThrough]
#endif
		#endregion
		public static BugNum operator +(BugNum value) {
			return new BugNum(+value.Numer, value.Venom);
		}
		#endregion
		#region #operator# -(#struct # value)
		#region #through# 
#if TRACE
		[System.Diagnostics.DebuggerStepThrough]
#endif
		#endregion
		public static BugNum operator -(BugNum value) {
			return new BugNum(-value.Numer, value.Venom);
		}
		#endregion
		#region #method# SqrtDebug(S) 
		public static BugNum SqrtDebug(BugNum S) {
			if (S == 0) return 0; if (S < 0) return 1;
			var SS = S.Numer;
			var VV = S.Venom;
			var Ret = SS / VV;
			var V = Ret;
			if (V > 1) {
				var T = V;
				var X = V / 2u;
				while (T > X) { T = X; X = (X + (V / X)) / 2u; }
				Ret = T;
			}
			var VenomInt = ((BugNum)VV);
			var VVV = BugInt.Pow(10, maxDepth);
			var D = maxDepth;
			while (--D >= 0) {
				Ret *= 10;
				VenomInt /= 100;
				var SSS = (BugInt)(SS / VenomInt);
				var A = 5u;
				var B = 3u;
				var C = 1u;
				var M = 0u;
				for (var I = 0; I < 4; I++) {
					var MA = M + A;
					var RM = Ret + MA;
					if ((RM * RM) <= SSS) { M = MA; }
					A = B; B = C;
				}
				Ret += M;
			}
			return new BugNum(Ret, VVV);
		}
		#endregion
		#region #method# Sqrt(X, Y, Depth = MaxDepth) 
		public static BugNum Sqrt(BugNum X, BugNum Y) {
			var S = X * X + Y * Y;
			if (S == 0) return 0; if (S < 0) return 1;
			var SS = S.Numer;
			var VV = S.Venom;
			var SV = SS * BugInt.Pow(10, maxDepth * 2) / VV;
			if (SV > 1) {
				var TT = SV;
				var XX = SV / 2u;
				while (TT != XX) { TT = XX; XX = (XX + (SV / XX)) / 2u; }
				SS = TT;
			}
			return new BugNum(SS, BugInt.Pow(10, maxDepth));
		}
		#endregion
		#region #method# Sqrt(S, Depth = MaxDepth) 
		public static BugNum Sqrt(BugNum S) {
			if (S == 0) return 0; if (S < 0) return 1;
			var SS = S.Numer;
			var VV = S.Venom;
			var SV = SS * BugInt.Pow(10, maxDepth * 2) / VV;
			if (SV > 1) {
				var TT = SV;
				var XX = SV / 2u;
				while (TT != XX) { TT = XX; XX = (XX + (SV / XX)) / 2u; }
				SS = TT;
			}
			return new BugNum(SS, BugInt.Pow(10, maxDepth));
		}
		#endregion
		#region #operator # == (#struct # L, #struct # R) 
		public static bool operator ==(BugNum L, BugNum R) {
			return L.Numer * R.Venom == R.Numer * L.Venom;
		}
		#endregion
		#region #operator # != (#struct # L, #struct # R) 
		public static bool operator !=(BugNum L, BugNum R) {
			return L.Numer * R.Venom != R.Numer * L.Venom;
		}
		#endregion
		#region #operator # > (#struct # L, #struct # R) 
		public static bool operator >(BugNum L, BugNum R) {
			return L.Numer * R.Venom > R.Numer * L.Venom;
		}
		#endregion
		#region #operator # >= (#struct # L, #struct # R) 
		public static bool operator >=(BugNum L, BugNum R) {
			return L.Numer * R.Venom >= R.Numer * L.Venom;
		}
		#endregion
		#region #operator # < (#struct # L, #struct # R) 
		public static bool operator <(BugNum L, BugNum R) {
			return L.Numer * R.Venom < R.Numer * L.Venom;
		}
		#endregion
		#region #operator # <= (#struct # L, #struct # R) 
		public static bool operator <=(BugNum L, BugNum R) {
			return L.Numer * R.Venom <= R.Numer * L.Venom;
		}
		#endregion
		#region #explicit operator # BugInt(BugNum L) 
		public static explicit operator BugInt(BugNum L) {
			return L.Numer / L.Venom;
		}
		#endregion
		#region #explicit operator # BugNum(BugInt L)
		public static implicit operator BugNum(BugInt L) {
			return new BugNum(L, 1);
		}
		#endregion
		#region #explicit operator # BugNum(#int # L)
		public static implicit operator BugNum(int L) {
			return new BugNum(L, 1);
		}
		#endregion
		#region #method# Rotate(CX, CY, BX, BY, AR, ED) 
		/// <summary>Поворачивает координаты вокруг центра по корню четверти круга
		/// где 90 градусов равно значению 1.0 а 360 градусов равно значению 4.0)</summary>
		/// <param name="CX">Центр по оси X)</param>
		/// <param name="CY">Центр по оси Y)</param>
		/// <param name="BX">Старт и возвращаемый результат поворота по оси X)</param>
		/// <param name="BY">Старт и возвращаемый результат поворота по оси Y)</param>
		/// <param name="AR">Корень четверти от 0.0 до 4.0 отрицательная в обратную сторону)</param>
		public static bool Rotate(BugNum CX, BugNum CY, ref BugNum BX, ref BugNum BY, BugNum AR) {
			var ED = maxDepth * 2;
			if (AR == 0) return false;
			var D = ED;
			var Len = Sqrt(CX - BX, CY - BY);
			if (Len == 0) return false;
			int R = (int)AR;
			if (R < 0) { AR = R - AR; R = R % 4 + 4; } else { AR = AR - R; R = R % 4; }
			var MX = BX; var MY = BY;
			if (R == 1) { MX = CY - BY + CX; MY = BX - CX + CY; } // 90
			else if (R == 2) { MX = CX - BX + CX; MY = CY - BY + CY; } // 180
			else if (R == 3) { MX = BY - CY + CX; MY = CX - BX + CY; } // 270
			var EX = BX; var EY = BY; BX = MX; BY = MY;
			if (AR > 0 && R >= 0 && R < 3) { EX = CY - MY + CX; EY = MX - CX + CY; } // 90
			while (AR > 0 && AR < 1 && ED > 0) {
				var L = Sqrt(MX - EX, MY - EY);
				if (L == 0) break;
				var ll = L / 2;
				if (AR < new BugNum(1, 2)) {
					EX = MX + (EX - MX) / L * ll;
					EY = MY + (EY - MY) / L * ll;
					ll = Sqrt(CX - EX, CY - EY);
					EX = CX + (EX - CX) / ll * Len;
					EY = CY + (EY - CY) / ll * Len;
					AR = AR * 2;
					BX = EX;
					BY = EY;
				} else {
					MX = EX + (MX - EX) / L * ll;
					MY = EY + (MY - EY) / L * ll;
					ll = Sqrt(CX - MX, CY - MY);
					MX = CX + (MX - CX) / ll * Len;
					MY = CY + (MY - CY) / ll * Len;
					AR = (AR - new BugNum(1, 2)) * 2; BX = MX; BY = MY;
				}
				ED--;
			}
			return true;
		}
		#endregion
		#region #method# GetAR(CX, CY, BX, BY, AX, AY, ED) 
		/// <summary>Возвращает корень поворота от 0.0 до 4.0)</summary>
		/// <param name="CX">Центр по оси X)</param>
		/// <param name="CY">Центр по оси Y)</param>
		/// <param name="BX">Старт по оси X)</param>
		/// <param name="BY">Старт по оси Y)</param>
		/// <param name="AX">Конец по оси X)</param>
		/// <param name="AY">Конец по оси Y)</param>
		/// <returns>Возвращает корень поворота от 0.0 до 4.0)</returns>
		/// <exception cref="System.InvalidProgramException">
		/// Возникает в случае непредусмотренного состояния, требует исправления)</exception>
		public static BugNum GetAR(BugNum CX, BugNum CY, BugNum BX, BugNum BY, BugNum AX, BugNum AY) {
			var ED = maxDepth * 2;
			var D = ED;
			var BL = Sqrt(CX - BX, CY - BY);
			if (BL == 0) return 0;
			var AL = Sqrt(CX - AX, CY - AY);
			if (AL == 0) return 0;
			AX = CX + (AX - CX) / AL * BL;
			AY = CY + (AY - CY) / AL * BL;
			AL = Sqrt(CX - AX, CY - AY);
			var X1 = CY - BY + CX; var Y1 = BX - CX + CY; // 90
			var X2 = CX - BX + CX; var Y2 = CY - BY + CY; // 180
			var X3 = BY - CY + CX; var Y3 = CX - BX + CY; // 270
			var L0 = Sqrt(BX - AX, BY - AY);
			var L1 = Sqrt(X1 - AX, Y1 - AY);
			var L2 = Sqrt(X2 - AX, Y2 - AY);
			var L3 = Sqrt(X3 - AX, Y3 - AY);
			BugNum R = 0, MX = 0, MY = 0, EX = 0, EY = 0;
			if (L0 < L2 && L0 < L3 && L1 < L2 && L1 <= L3) {
				R = 0; MX = BX; MY = BY; EX = X1; EY = Y1;
			} else if (L1 < L3 && L1 < L0 && L2 < L3 && L2 <= L0) {
				R = 1; MX = X1; MY = Y1; EX = X2; EY = Y2; L0 = L1; L1 = L2;
			} else if (L2 < L0 && L2 < L1 && L3 < L0 && L3 <= L1) {
				R = 2; MX = X2; MY = Y2; EX = X3; EY = Y3; L0 = L2; L1 = L3;
			} else if (L3 < L1 && L3 < L2 && L0 < L1 && L0 <= L2) {
				R = 3; MX = X3; MY = Y3; EX = BX; EY = BY; L1 = L0; L0 = L3;
			} else { throw new System.InvalidProgramException(); }
			BugNum AR = 1;
			while (L0 > 0 && (L2 = Sqrt(MX - EX, MY - EY)) > 0) {
				AR /= 2;
				L3 = L2 / 2;
				BX = MX + (EX - MX) / L2 * L3;
				BY = MY + (EY - MY) / L2 * L3;
				L2 = Sqrt(CX - BX, CY - BY);
				BX = CX + (BX - CX) / L2 * BL;
				BY = CY + (BY - CY) / L2 * BL;
				L3 = Sqrt(AX - BX, AY - BY);
				if (L0 < L1) {
					if (EX == BX && EY == BY) break; if (L1 <= L3) break;
					EX = BX; EY = BY; L1 = L3;
				} else {
					if (MX == BX && MY == BY) break; if (L0 <= L3) break;
					MX = BX; MY = BY; L0 = L3; R += AR;
				}
				ED--;
			}
			return R;
		}
		#endregion
		#region #field# TAtanArray 
		public static BugNum[] TAtanArray;
		#endregion
		#region #method# TAtanOfTan(X) 
		/// <summary>Функция возвращает обратный тангенс угла с проверкой и уточнением тангенсом)</summary>
		public static BugNum TAtanOfTan(BugNum X) {
			var M = false;
			if (X < 0) { X = -X; M = true; }
			var RX = X;
			var L = false;
			var Y = 0;
			BugNum YY = 0;
			BugNum R = 0;
			if (X >= 2) { L = true; X = 1.0 / X; goto Next; }
			Y = (int)(X * 8);
			if (Y < 0) Y++;
			var XX = Y / new BugNum(8);
			X = (X - XX) / (X * XX + 1);
		Next:
			XX = X * X;
			var C = (((13852575 * XX + 216602100) * XX + 891080190) * XX + 1332431100) * XX + 654729075;
			var B = ((((893025 * XX + 49116375) * XX + 425675250) * XX + 1277025750) * XX + 1550674125) * XX + 654729075;
			R += (C / B) * X;
			if (Y > 0) {
				X = Y * new BugNum(1, 8);
				Y = 0; goto Next;
			}
			if (L) R = PId2 - R;
			var I = new BugNum(1, 10);
			var c = 5u;
			var b = 3u;
			var a = 1u;
			var T = TOfTan(R);
			var P = R;
			while (T != RX) {
				if (T < RX) {
					var RI = R + I * c;
					var TT = TOfTan(RI);
					if (TT > RX) {
						if (c == 1) {
							I /= 10; c = 5; b = 3;
						} else { c = b; b = a; }
					} else {
						R = RI;
						T = TT;
					}
				} else {
					var RI = R - I * c;
					var TT = TOfTan(RI);
					if (TT < RX) {
						if (c == 1) {
							I /= 10; c = 5; b = 3;
						} else { c = b; b = a; }
					} else {
						R = RI;
						T = TT;
					}
				}
				if (I == 0) { break; }
			}
			return M ? -R : R;
		}
		#endregion
		#region #method# TOfTan(X) 
		/// <summary>Возвращает положительный тангенс угла большей глубины)</summary>
		public static BugNum TOfTan(BugNum X) {
			var S = false;
			BugNum C = 0;
		Next:
			if (S) X -= PId2;
			var x = X;
			if (x < 0) { x = -x; }
			if (x > PIx2) {
				var XP = x / PIx2;
				x = PIx2 * (XP - (int)XP);
			}
			var M = (x > PId2 && x <= PId2x3);
			var XX = x * x;
			var XXX = XX;
			//var R = 1 - (XX / 2);
			//R += (XXX *= XX) / 24;
			//R -= (XXX *= XX) / 720;
			//R += (XXX *= XX) / 40320;
			//R -= (XXX *= XX) / 3628800;
			//R += (XXX *= XX) / 479001600;
			//R -= (XXX *= XX) / 87178291200;
			//R += (XXX *= XX) / 20922789888000;
			//R -= (XXX *= XX) / 6402373705728000;
			//R += (XXX *= XX) / 2432902008176640000;
			var F = new BugInt(2);
			var U = -1;
			var R = 1 + (XX / F * U);
			uint P = 3;
			BugNum RR = 0;
			while (XXX != 0 && RR != R && F < maxVenom) {
				RR = R;
				XXX *= XX;
				F *= (P++ * P++); U = -U;
				R += XXX / F * U;
			}
			if (R < 0) R = -R;
			//if (M) R = -R;
			if (!S) { C = R; S = true; goto Next; }
			R /= C;
			return R;
		}
		#endregion
		#region #method# TAtan(X) 
		/// <summary>Функция возвращает обратный тангенс угла)</summary>
		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static BugNum TAtan(BugNum X) {
			var M = false;
			if (X < 0) { X = -X; M = true; }
			var L = false;
			var Y = 0;
			BugNum YY = 0;
			BugNum R = 0;
			if (X >= 2) { L = true; X = 1.0 / X; goto Next; }
			Y = (int)(X * 8);
			if (Y < 0) Y++;
			var XX = Y / new BugNum(8);
			X = (X - XX) / (X * XX + 1);
		Next:
			XX = X * X;
			var C = (((13852575 * XX + 216602100) * XX + 891080190) * XX + 1332431100) * XX + 654729075;
			var B = ((((893025 * XX + 49116375) * XX + 425675250) * XX + 1277025750) * XX + 1550674125) * XX + 654729075;
			R += (C / B) * X;
			if (Y > 0) {
				X = Y * new BugNum(1, 8);
				Y = 0; goto Next;
			}
			if (L) R = PId2 - R;
			return M ? -R : R;
		}
		#endregion
		#region #method# TAtan2(Y, X) 
		public static BugNum TAtan2(BugNum Y, BugNum X) {
			if (X == 0) {
				if (Y == 0) return 0;
				else if (Y > 0) return PId2; else return -PId2;
			}
			var A = TAtan(Y / X);
			if (X < 0) {
				if (Y >= 0) A += PI; else A -= PI;
			}
			return A;
		}
		#endregion
		#region #method# TAsin(X) 
		public static BugNum TAsin(BugNum X) {
			if (X < 0) X = -X;
			if (X > 1) return 1;
			return TAtan2(X, Sqrt(1 - X * X, 17));
		}
		#endregion
		#region #method# TCos(X) 
		public static BugNum TCos(BugNum X) {
			if (X < 0) { X = -X; }
			if (X > PIx2) {
				var XP = X / PIx2;
				X = PIx2 * (XP - (int)XP);
			}
			var M = (X > PId2 && X <= PId2x3);
			var XX = X * X;
			var XXX = XX;
			//var R = 1 - (XX / 2);
			//R += (XXX *= XX) / 24;
			//R -= (XXX *= XX) / 720;
			//R += (XXX *= XX) / 40320;
			//R -= (XXX *= XX) / 3628800;
			//R += (XXX *= XX) / 479001600;
			//R -= (XXX *= XX) / 87178291200;
			//R += (XXX *= XX) / 20922789888000;
			//R -= (XXX *= XX) / 6402373705728000;
			//R += (XXX *= XX) / 2432902008176640000;
			var F = new BugInt(2);
			var U = -1;
			var R = 1 + (XX / F * U);
			uint P = 3;
			BugNum RR = 0;
			while (XXX != 0 && RR != R && F < maxVenom) {
				RR = R;
				XXX *= XX;
				F *= (P++ * P++); U = -U;
				R += XXX / F * U;
			}
			if (R < 0) R = -R;
			if (M) R = -R;
			return R;
		}
		#endregion
		#region #method# TSin(X) 
		public static BugNum TSin(BugNum X) {
			X -= PId2;
			if (X < 0) { X = -X; }
			if (X > PIx2) {
				var XP = X / PIx2;
				X = PIx2 * (XP - (int)XP);
			}
			var M = (X > PId2 && X <= PId2x3);
			var XX = X * X;
			var XXX = XX;
			//var R = 1 - (XX / 2);
			//R += (XXX *= XX) / 24;
			//R -= (XXX *= XX) / 720;
			//R += (XXX *= XX) / 40320;
			//R -= (XXX *= XX) / 3628800;
			//R += (XXX *= XX) / 479001600;
			//R -= (XXX *= XX) / 87178291200;
			//R += (XXX *= XX) / 20922789888000;
			//R -= (XXX *= XX) / 6402373705728000;
			//R += (XXX *= XX) / 2432902008176640000;
			var F = new BugInt(2);
			var U = -1;
			var R = 1 + (XX / F * U);
			uint P = 3;
			BugNum RR = 0;
			while (XXX != 0 && RR != R && F < maxVenom) {
				RR = R;
				XXX *= XX;
				F *= (P++ * P++); U = -U;
				R += XXX / F * U;
			}
			if (R < 0) R = -R;
			if (M) R = -R;
			return R;
		}
		#endregion
		#region #method# TSinCos(X) 
		public static void TSinCos(BugNum X, out BugNum Sin, out BugNum Cos) {
			var S = false;
			var C = new BugNum(0);
		Next:
			if (S) X -= PId2;
			var x = X;
			if (x < 0) { x = -x; }
			if (x > PIx2) {
				var XP = x / PIx2;
				x = PIx2 * (XP - (int)XP);
			}
			var M = (x > PId2 && x <= PId2x3);
			var XX = x * x;
			var XXX = XX;
			//var R = 1 - (XX / 2);
			//R += (XXX *= XX) / 24;
			//R -= (XXX *= XX) / 720;
			//R += (XXX *= XX) / 40320;
			//R -= (XXX *= XX) / 3628800;
			//R += (XXX *= XX) / 479001600;
			//R -= (XXX *= XX) / 87178291200;
			//R += (XXX *= XX) / 20922789888000;
			//R -= (XXX *= XX) / 6402373705728000;
			//R += (XXX *= XX) / 2432902008176640000;
			var F = new BugInt(2);
			var U = -1;
			var R = 1 + (XX / F * U);
			uint P = 3;
			BugNum RR = 0;
			while (XXX != 0 && RR != R && F < maxVenom) {
				RR = R;
				XXX *= XX;
				F *= (P++ * P++); U = -U;
				R += XXX / F * U;
			}
			if (R < 0) R = -R;
			if (M) R = -R;
			if (!S) { C = R; S = true; goto Next; }
			Cos = C;
			Sin = R;
		}
		#endregion
		#region #method# TTan(X) 
		public static BugNum TTan(BugNum X) {
			var S = false;
			BugNum C = 0;
		Next:
			if (S) X -= PId2;
			var x = X;
			if (x < 0) { x = -x; }
			if (x > PIx2) {
				var XP = x / PIx2;
				x = PIx2 * (XP - (int)XP);
			}
			var M = (x > PId2 && x <= PId2x3);
			var XX = x * x;
			var XXX = XX;
			//var R = 1 - (XX / 2);
			//R += (XXX *= XX) / 24;
			//R -= (XXX *= XX) / 720;
			//R += (XXX *= XX) / 40320;
			//R -= (XXX *= XX) / 3628800;
			//R += (XXX *= XX) / 479001600;
			//R -= (XXX *= XX) / 87178291200;
			//R += (XXX *= XX) / 20922789888000;
			//R -= (XXX *= XX) / 6402373705728000;
			//R += (XXX *= XX) / 2432902008176640000;
			var F = new BugInt(2);
			var U = -1;
			var R = 1 + (XX / F * U);
			uint P = 3;
			var I = 0;
			BugNum RR = 0;
			while (XXX != 0 && RR != R && F < maxVenom) {
				RR = R;
				XXX *= XX;
				F *= (P++ * P++); U = -U;
				R += XXX / F * U;
			}
			if (R < 0) R = -R;
			if (M) R = -R;
			if (!S) { C = R; S = true; goto Next; }
			R /= C;
			return R;
		}
		#endregion
		#region #method# TCot(X) 
		public static BugNum TCot(BugNum X) {
			return (1.0 / TTan(X));
		}
		#endregion
		#region #int # #explicit operator # (#struct # V)
		public static explicit operator int(BugNum V) {
			return (int)(V.Numer / V.Venom);
		}
		#endregion
		#region #uint # #explicit operator # (#struct # V)
		public static explicit operator uint(BugNum V) {
			return (uint)(V.Numer / V.Venom);
		}
		#endregion
		#region #long # #explicit operator # (#struct # V)
		public static explicit operator long(BugNum V) {
			return (long)(V.Numer / V.Venom);
		}
		#endregion
		#region #ulong # #explicit operator # (#struct # V)
		public static explicit operator ulong(BugNum V) {
			return (ulong)(V.Numer / V.Venom);
		}
		#endregion
		#region #double # #explicit operator # (#struct # V)
		public static explicit operator double(BugNum V) {
			return V.Double;
		}
		#endregion
		#region #get# Double 
		public double Double {
			get {
				var Minus = false;
				var Numer = this.Numer;
				if (Numer < 0) { Numer = -Numer; Minus = true; }
				var Venom = this.Venom;
				if (Venom == 0) {
					if (Numer == 0) return double.NaN;
					if (Numer < 0) return double.NegativeInfinity;
					return double.PositiveInfinity;
				}
				var R = 0.0;
				if (Venom > 0) {
					var Int = BugInt.DivMod(Numer, Venom, out var Num);
					Numer = Num * maxVenom / Venom;
					var Zeros = maxDepth - Numer.Digits;
					R = Numer.ToDouble();
					while (Zeros-- > 0) {
						R /= 10;
					}
					if (Int > 0) R += (ulong)Int;
				} else {
					R = (-Venom).ToDouble();
					if (Numer > 0) R += (ulong)Numer;
				}
				if (Minus) R = -R;
				return R;
			}
		}
		#endregion
		#region #get# DoubleNum 
		public BugNum DoubleNum {
			get {
				return (BugNum)Double;
			}
		}
		#endregion
		#region #method# Rotate1(CX, CY, BX, BY, AR) 
		/// <summary>Поворачивает координаты #double# вокруг центра по корню четверти круга
		/// где 90 градусов равно значению 0.25, а 360 градусов равно значению 1)</summary>
		/// <param name="CX">Центр по оси X)</param>
		/// <param name="CY">Центр по оси Y)</param>
		/// <param name="BX">Старт и возвращаемый результат поворота по оси X)</param>
		/// <param name="BY">Старт и возвращаемый результат поворота по оси Y)</param>
		/// <param name="AR">Корень четверти от 0.0 до 1.0 отрицательная в обратную сторону)</param>
		public static void Rotate1(BugNum CX, BugNum CY, ref BugNum BX, ref BugNum BY, BugNum AR) {
			if (AR == 0) return;
			var TX = BX - CX;
			var TY = BY - CY;
			if (TX == 0 && TY == 0) return;
			var PI = PIx2 * AR;
			TSinCos(PI, out var SiN, out var CoS);
			var X = (CoS * TX - SiN * TY + CX);
			var Y = (SiN * TX + CoS * TY + CY);
			BX = X;
			BY = Y;
		}
		#endregion
		#region #method# GetaR1(CX, CY, BX, BY, AX, AY) 
		/// <summary>Возвращает корень поворота от 0.0 до 1.0)</summary>
		/// <param name="CX">Центр по оси X)</param>
		/// <param name="CY">Центр по оси Y)</param>
		/// <param name="BX">Старт по оси X)</param>
		/// <param name="BY">Старт по оси Y)</param>
		/// <param name="AX">Конец по оси X)</param>
		/// <param name="AY">Конец по оси Y)</param>
		/// <returns>Возвращает корень поворота от 0.0 до 1.0)</returns>
		public static BugNum GetaR1(BugNum CX, BugNum CY, BugNum BX, BugNum BY, BugNum AX, BugNum AY) {
			var R = (new BugNum(1, 2) / PI) * (TAtan2(AY - CY, AX - CX) - TAtan2(BY - CY, BX - CX));
			if (R < 0) R += 1;
			return R;
		}
		#endregion
		#region #method# EqualDigits(L, R) 
		public static int EqualDigits(BugNum L, BugNum R) {
			var LS = L.ToString();
			var RS = R.ToString();
			var C = LS.Length;
			var RC = RS.Length;
			if (C > RC) C = RC;
			var I = 0;
			while (I < C) { if (LS[I] == RS[I]) I++; else break; }
			return I;
		}
		#endregion
	}
}