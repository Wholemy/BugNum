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
		public static readonly BugNum Zer = new BugNum() { Numer = 0, Venom = 1 };
		public static readonly BugNum One = new BugNum() { Numer = 1, Venom = 1 };
		public static readonly BugNum Two = new BugNum() { Numer = 2, Venom = 1 };
		public static readonly BugNum Mon = new BugNum() { Numer = -1, Venom = 1 };
		public static readonly BugNum V05 = new BugNum() { Numer = 1, Venom = 2 };
		public static readonly BugNum Nan = new BugNum();
		public static readonly BugNum Pos = new BugNum() { Numer = 1 };
		public static readonly BugNum Neg = new BugNum() { Numer = -1 };
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
		#region #field# MaxDepth 
		/// <summary>Максимальная глубина числа для его округления при инициализации)</summary>
		public static int MaxDepth = 50;
		#endregion
		#region #field# MaxVenom 
		public static BugInt MaxVenom = BugInt.Pow(10, MaxDepth);
		#endregion
		#region #field# MaxChars 
		/// <summary>
		/// Максимальное количество выводимых в строку символов дроби) Значение больше MaxDepth не достижимо)
		/// </summary>
		/// <remarks>
		/// Как правило половина от дробной глубины числа эффективна при вычислении 
		/// значений сложных тригонометрических функций, затем и нужно это ограничение)
		/// Например, для вычисления квадратного корня, требуется вдвое больше число,
		/// но результат у него достоверный по всей вычисляемой длине дроби)
		/// </remarks>
		public static int MaxChars = MaxDepth / 2;
		#endregion
		#region #field# PI 
		public static BugNum PI = new BugNum("3.1415926535897932384626433832795028841971693993751058209749445923078164062862089986280348253421170679");
		#endregion
		#region #field# PId2 
		public static BugNum PId2 = PI / 2;
		#endregion
		#region #field# PId2x3 
		public static BugNum PId2x3 = PI + PId2;
		#endregion
		#region #field# PIx2 
		public static BugNum PIx2 = PI * 2;
		#endregion
		#region #field# PIx4 
		public static BugNum PIx4 = PI * 4;
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
				var MV = MaxVenom;
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
		#region #new# (#string # value) 
		public BugNum(string value) {
			if (value == null) throw new System.ArgumentNullException("value");
			value = value.Trim();
			value = value.Replace("_", "");
			value = value.Replace(" ", "");
			value = value.ToLower();
			if (value.Equals("nan")) { this.Venom = 0; this.Numer = 0; return; }
			if (value.Equals("neg")) { this.Venom = 0; this.Numer = -1; return; }
			if (value.Equals("pos")) { this.Venom = 0; this.Numer = 1; return; }
			var dotStr = ".";
			int dot = value.IndexOf(".");
			if (dot < 0) { dot = value.IndexOf(","); dotStr = ","; }
			if (dot < 0) { this.Numer = new BugInt(value); this.Venom = 1; return; }
			value = value.Replace(dotStr, "");
			var Depth = value.Length - dot;
			if (Depth > MaxDepth) { Depth = MaxDepth; value = value.Substring(0, Depth + dot); }
			var Numer = new BugInt(value);
			var Venom = BugInt.Pow(10, Depth);
			this.Numer = Numer;
			this.Venom = Venom;
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
				Numer = Num * MaxVenom / Venom;
				if (Numer > 0) {
					Sign += '.';
					var Decimal = "";
					var Zeros = MaxDepth - Numer.Digits;
					if (Zeros > 0) Decimal += new string('0', Zeros);
					Zeros = Numer.Zerone; if (Zeros < 0) Zeros = -Zeros;
					if (Zeros > 0) Numer /= BugInt.Pow(10u, Zeros);
					Decimal += Numer.ToString();
					if (Decimal.Length > MaxChars) Decimal = Decimal.Substring(0, MaxChars);
					Sign += Decimal;
				}
			} else {
				Venom = -Venom;
				Sign += Numer.ToString();
				Sign += '.';
				var Decimal = Venom.ToString();
				if (Decimal.Length > MaxChars) Decimal = Decimal.Substring(0, MaxChars);
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
		#region #implicit operator# (#double # value) 
		#region #through# 
#if TRACE
		[System.Diagnostics.DebuggerStepThrough]
#endif
		#endregion
		public static implicit operator BugNum(double value) {
			return new BugNum(value);
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
			var Int = (ulong)Value;
			var Num = new BugInt(Int);
			Value -= Int;
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
			var VVV = BugInt.Pow(10, MaxDepth);
			var D = MaxDepth;
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
			var SV = SS * BugInt.Pow(10, MaxDepth * 2) / VV;
			if (SV > 1) {
				var TT = SV;
				var XX = SV / 2u;
				while (TT != XX) { TT = XX; XX = (XX + (SV / XX)) / 2u; }
				SS = TT;
			}
			return new BugNum(SS, BugInt.Pow(10, MaxDepth));
		}
		#endregion
		#region #method# Sqrt(S, Depth = MaxDepth) 
		public static BugNum Sqrt(BugNum S) {
			if (S == 0) return 0; if (S < 0) return 1;
			var SS = S.Numer;
			var VV = S.Venom;
			var SV = SS * BugInt.Pow(10, MaxDepth * 2) / VV;
			if (SV > 1) {
				var TT = SV;
				var XX = SV / 2u;
				while (TT != XX) { TT = XX; XX = (XX + (SV / XX)) / 2u; }
				SS = TT;
			}
			return new BugNum(SS, BugInt.Pow(10, MaxDepth));
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
			var ED = MaxDepth * 2;
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
			var ED = MaxDepth * 2;
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
			var XX = X * X;
			var C = (((13852575 * XX + 216602100) * XX + 891080190) * XX + 1332431100) * XX + 654729075;
			var B = ((((893025 * XX + 49116375) * XX + 425675250) * XX + 1277025750) * XX + 1550674125) * XX + 654729075;
			var R = (C / B) * X;
			var I = new BugNum(1, 10);
			var c = 5u;
			var b = 3u;
			var a = 1u;
			var T = TOfTan(R);
			var P = R;
			while (T != X) {
				if (T < X) {
					var RI = R + I * c;
					var TT = TOfTan(RI);
					if (TT > X) {
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
					if (TT < X) {
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
			while (XXX != 0 && RR != R && F < MaxVenom) {
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
			var L = 0;
			var Y = 0;
			BugNum YY = 0;
			if (X >= 4) { L = -1; X = 1.0 / X; goto Next; } else if (X < new BugNum(1, 2)) goto Next;
			Y = (int)(X * 2);
			if (Y < 0) Y++;
			var XX = Y / new BugNum(2);
			X = (X - XX) / (X * XX + 1);
			Next:
			XX = X * X;
			var C = (((13852575 * XX + 216602100) * XX + 891080190) * XX + 1332431100) * XX + 654729075;
			var B = ((((893025 * XX + 49116375) * XX + 425675250) * XX + 1277025750) * XX + 1550674125) * XX + 654729075;
			var R = (C / B) * X;
			if (Y > 0) {
				var I = Y - 1;
				var AR = TAtanArray;
				if (AR == null) TAtanArray = AR = new BugNum[7];
				var N = AR[I];
				if (N == 0) N = AR[I] = TAtanOfTan(Y * new BugNum(1, 2)).GcdNum;
				R += N;
			}
			if (L != 0) R = PId2 - R;
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
			while (XXX != 0 && RR != R && F < MaxVenom) {
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
			while (XXX != 0 && RR != R && F < MaxVenom) {
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
			while (XXX != 0 && RR != R && F < MaxVenom) {
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
			while (XXX != 0 && RR != R && F < MaxVenom) {
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
		#region #method# TCot(x) 
		public static BugNum TCot(BugNum x) {
			return (1.0 / TTan(x));
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
					Numer = Num * MaxVenom / Venom;
					var Zeros = MaxDepth - Numer.Digits;
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
