namespace Wholemy {
	public struct BugNum {
		public BugInt Numer;
		public BugInt Venom;
		public int Depth;
		public static int MaxDepth = 50;
		#region #new# (Value) 
		#region #through# 
#if TRACE
		[System.Diagnostics.DebuggerStepThrough]
#endif
		#endregion
		public BugNum(BugNum Value) : this(Value.Numer, Value.Venom) { }
		#endregion
		#region #new# (Numer, Venom, Depth) 
		/// <summary>Инициализация большого числа)</summary>
		/// <param name="Numer">Целое если Venom меньше или равен 0 и Depth равен 0, в остальных случаях это делимое)</param>
		/// <param name="Venom">Делитель если больше или равен 1 и дробное если меньше или равен 0)</param>
		/// <param name="Depth">Если 0 определяется из числа, длина дробной части, после установки реальная длина делителя, если меньше нуля бесконечное число)</param>
		public BugNum(BugInt Numer, BugInt Venom, int Depth = 0) {
			if (Venom == 0) {
				if (Depth > 0) {
					var P = +Numer.Zerone;
					if (P < Depth) { Depth -= P; } else { P = Depth; Depth = 0; }
					if (P > 0) { Numer /= BugInt.Pow(10, P); }
					if (Depth > 0) { Venom = BugInt.Pow(10, Depth); } else { Venom = 1; }
				} else { Venom = 1; }
			} else {
				if (Venom < 0 && Depth == 0) {
					Venom = -Venom;
					var P = Venom.Digits;
					var I = Numer;
					Numer = Venom;
					Venom = BugInt.Pow(10, P);
					Depth = P;
					if (I > 0) Numer += I * Venom;
				}
				if (Numer != 0 && Venom != 0) {
					BugInt N, V; uint NM = 0, VM = 0, DM = 1000000000;
					var D = Depth; var DE = 9;
					Next:
					N = BugInt.DivMod(Numer, DM, out NM);
					V = BugInt.DivMod(Venom, DM, out VM);
					D -= DE;
					if (NM == 0 && VM == 0) { Numer = N; Venom = V; Depth = D; goto Next; } else {
						while (DM > 10) { DM /= 10; DE--; if (NM % DM == 0 && VM % DM == 0) goto Next; }
					}
				}
				{
					var Int = BugInt.DivMod(Numer, Venom, out var Num);
					var Dec = Int;
					var End = Num;
					var Ven = Venom;
					var D = 0;
					var MD = MaxDepth + 1;
					while (Num > 0 && D < MD) {
						D++;
						var Pre = (uint)Ven;
						Ven = BugInt.DivMod(Ven, 10u, out var Mod);
						if (Ven == 0) { if (Mod > 0) D = -D; break; }
						Dec *= 10;
						Dec += BugInt.DivMod(Num, Ven, out Num);
					}
					MD--;
					if (D > MD || -D > MD) {
						var MM = false;
						if (D < 0) { D = -D; MM = true; }
						D--;
						var De = BugInt.Pow(10, D);
						if (Venom != De) {
							if(MM) Numer = Dec;
							else Numer = Dec / 10;
							Venom = De;
							Depth = D;
						}
					}
				}
			}
			this.Numer = Numer;
			this.Venom = Venom;
			this.Depth = Depth;
		}
		#endregion
		#region #new# (#string # value) 
		public BugNum(string value) {
			if (value == null) throw new System.ArgumentNullException("value");
			value = value.Trim();
			var T = System.Globalization.CultureInfo.InvariantCulture.NumberFormat;

			value = value.Replace(T.NumberGroupSeparator, "");
			int dot = value.IndexOf(T.NumberDecimalSeparator);
			if (dot < 0) {
				this.Numer = new BugInt(value);
				this.Venom = 1;
			} else {
				value = value.Replace(T.NumberDecimalSeparator, "");
				var Numer = new BugInt(value);
				var Depth = value.Length - dot;
				var Venom = BugInt.Pow(10, Depth);
				if (Numer != 0 && Venom != 0) GetGcd(ref Numer, ref Venom);
				this.Numer = Numer;
				this.Venom = Venom;
				this.Depth = Depth;
			}
		}
		#endregion
		#region #property# GcdNum 
		public BugNum GcdNum {
			get {
				var Numer = this.Numer;
				var Venom = this.Venom;
				var Depth = this.Depth;
				if (Numer != 0 && Venom != 0) {
					var Gcd = BugInt.Gcd(Numer, Venom);
					if (Gcd > 1) { Numer /= Gcd; Venom /= Gcd; }
				}
				return new BugNum(Numer, Venom, Depth);
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
			var MaxDecimalFraction = 100;
			var T = System.Globalization.CultureInfo.InvariantCulture.NumberFormat;
			var Sign = "";
			var Numer = this.Numer;
			var Venom = this.Venom;
			var Depth = this.Depth;
			if (Venom == 0) {
				var S = Numer.ToString();
				if (Depth == 0) return S;
				var L = S.Length;
				var C = 0;
				if (L > Depth) {
					C = L - Depth;
					return S.Substring(0, C) + "." + S.Substring(C, L - C);
				} else {
					C = Depth - L + 2;
					var I = C;
					var Chars = new char[I];
					while (--I >= 0) { Chars[I] = '0'; }
					Chars[1] = '.';
					return new string(Chars, 0, C) + S;
				}
			}
			if (Numer < 0) { Numer = -Numer; Sign = "-"; }
			var Divis = GetGcd(ref Numer, ref Venom);
			var Integer = Numer / Venom;
			var Renom = Venom;
			var Remin = Numer % Renom;
			if (Remin == 0) return Sign + Integer.ToString();
			System.Text.StringBuilder SB = new System.Text.StringBuilder();
			var pRemin = Remin;
			var pRenom = Renom;
			if (Renom % 10 == 0) { Renom /= 10; } else { Remin *= 10; }
			BugInt Nemin;

			var Infin = 0;
			var pInfin = 0;
			while (Remin > 0) {
				Nemin = Remin / Renom;
				Remin %= Renom;
				//if (Remin == pRemin && pRenom == Renom)
				//	return /*Numer.ToString() + ":" + Venom.ToString() + " = " +*/ Integer.ToString() + "." + SB.ToString() + Nemin.ToString() + "...";
				SB.Append(Nemin);
				if (SB.Length >= MaxDecimalFraction) {
					return /*Numer.ToString() + ":" + Venom.ToString() + " = " +*/ Sign + Integer.ToString() + "." + SB.ToString() + "......";
				}
				pRemin = Remin; pRenom = Renom;
				if (Renom % 10 == 0) { Renom /= 10; pInfin = Infin++; } else { Remin *= 10; }
				//if (Infin == pInfin && Nemin != 0) {
				//	var len = SB.Length - Infin;
				//	if (len > 0 && (len % 2) == 0) {
				//		var S = SB.ToString();

				//		if (Infin > 0) S.Substring(Infin);
				//		var hlen = len / 2;
				//		var hS = S.Substring(0, hlen);
				//		if (S.EndsWith(hS)) {
				//			SB.Length = Infin + hlen;
				//			return /*Numer.ToString() + ":" + Venom.ToString() + " = " +*/ Integer.ToString() + "." + SB.ToString() + "...";
				//		}
				//	}
				//}
			}
			return /*Numer.ToString() + ":" + Venom.ToString() + " = " +*/ Sign + Integer.ToString() + "." + SB.ToString();
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
			return L - Floor(L / R) * R;
		}
		public static BugNum operator %(BugNum L, int R) {
			return L - Floor(L / R) * R;
		}
		#endregion
		#region #method# Floor(L) 
		public static BugNum Floor(BugNum L) {
			var N = L.Numer;
			if (N < 0) { N += L.Venom - (N % L.Venom); } else { N -= (N % L.Venom); }
			return new BugNum(N, L.Venom);
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
			this.Depth = 0;
		}
		#endregion
		#region #new# (#uint # Value) 
		public BugNum(uint Value) {
			this.Numer = Value;
			this.Venom = 1;
			this.Depth = 0;
		}
		#endregion
		#region #new# (#long # Value) 
		public BugNum(long Value) {
			this.Numer = Value;
			this.Venom = 1;
			this.Depth = 0;
		}
		#endregion
		#region #new# (#ulong # Value) 
		public BugNum(ulong Value) {
			this.Numer = Value;
			this.Venom = 1;
			this.Depth = 0;
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
			this.Depth = Pow;
		}
		#endregion
		#endregion
		#region #method# Abs(value) 
		#region #through# 
#if TRACE
		[System.Diagnostics.DebuggerStepThrough]
#endif
		#endregion
		public static BugNum Abs(BugNum value) {
			return new BugNum(BugInt.Abs(value.Numer), value.Venom);
		}
		#endregion
		#region #operator# +(#struct # value)
		#region #through# 
#if TRACE
		[System.Diagnostics.DebuggerStepThrough]
#endif
		#endregion
		public static BugNum operator +(BugNum value) {
			return new BugNum(BugInt.Abs(value.Numer), value.Venom);
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
		#region #method# Neg(value) 
		#region #through# 
#if TRACE
		[System.Diagnostics.DebuggerStepThrough]
#endif
		#endregion
		public static BugNum Neg(BugNum value) {
			return new BugNum(-value.Numer, value.Venom);
		}
		#endregion
		#region #method# SqrtDebug(S, Bound) 
		public static BugNum SqrtDebug(BugNum S, int Depth) {
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
			var VVV = BugInt.Pow(10, Depth);
			var D = Depth;
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
		#region #method# SqrtDepth(X, Y, Depth) 
		public static BugNum SqrtDepth(BugNum X, BugNum Y, int Depth) {
			return SqrtDepth(X * X + Y * Y, Depth);
		}
		#endregion
		#region #method# SqrtDepth(S, Depth) 
		public static BugNum SqrtDepth(BugNum S, int Depth) {
			if (S == 0) return 0; if (S < 0) return 1;
			var SS = S.Numer;
			var VV = S.Venom;
			var SV = SS * BugInt.Pow(10, Depth * 2) / VV;
			if (SV > 1) {
				var T = SV;
				var X = SV / 2u;
				while (T != X) { T = X; X = (X + (SV / X)) / 2u; }
				SS = T;
			}
			return new BugNum(SS, BugInt.Pow(10, Depth));
		}
		#endregion
		#region #method# Sqrt(X) 
		public static double Sqrt(double X) {
			if (X == 0) return 0; if (X < 0) return 1;
			var PS = 0.0;
			var SS = X * 2 / X;
			var SSS = (SS - (X / SS)) / 2u;
			while (SSS != PS) {
				SS -= SSS;
				PS = SSS;
				SSS = (SS - (X / SS)) / 2u;
			}
			return SS;
		}
		#endregion
		#region #method# Sqrt(X, Y) 
		public static double Sqrt(double X, double Y) {
			var S = X * X + Y * Y;
			if (S == 0) return 0; if (S < 0) return 1;
			var PS = 0.0;
			var SS = S * 2 / S;
			var SSS = (SS - (S / SS)) / 2u;
			while (SSS != PS) {
				SS -= SSS;
				PS = SSS;
				SSS = (SS - (S / SS)) / 2u;
			}
			return SS;
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
		public static bool Rotate(BugNum CX, BugNum CY, ref BugNum BX, ref BugNum BY, BugNum AR, int ED = 56) {
			if (AR == 0) return false;
			var D = ED;
			var Len = SqrtDepth(CX - BX, CY - BY, D);
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
				var L = SqrtDepth(MX - EX, MY - EY, D);
				if (L == 0) break;
				var ll = L / 2;
				if (AR < new BugNum(1, 2)) {
					EX = MX + (EX - MX) / L * ll;
					EY = MY + (EY - MY) / L * ll;
					ll = SqrtDepth(CX - EX, CY - EY, D);
					EX = CX + (EX - CX) / ll * Len;
					EY = CY + (EY - CY) / ll * Len;
					AR = AR * 2;
					EX = EX.Round(D);
					EY = EY.Round(D);
					BX = EX;
					BY = EY;
				} else {
					MX = EX + (MX - EX) / L * ll;
					MY = EY + (MY - EY) / L * ll;
					ll = SqrtDepth(CX - MX, CY - MY, D);
					MX = CX + (MX - CX) / ll * Len;
					MY = CY + (MY - CY) / ll * Len;
					MX = MX.Round(D);
					MY = MY.Round(D);
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
		public static BugNum GetAR(BugNum CX, BugNum CY, BugNum BX, BugNum BY, BugNum AX, BugNum AY, int ED = 56) {
			var D = ED;
			var BL = SqrtDepth(CX - BX, CY - BY, D);
			if (BL == 0) return 0;
			var AL = SqrtDepth(CX - AX, CY - AY, D);
			if (AL == 0) return 0;
			AX = CX + (AX - CX) / AL * BL;
			AY = CY + (AY - CY) / AL * BL;
			AL = SqrtDepth(CX - AX, CY - AY, D);
			var X1 = CY - BY + CX; var Y1 = BX - CX + CY; // 90
			var X2 = CX - BX + CX; var Y2 = CY - BY + CY; // 180
			var X3 = BY - CY + CX; var Y3 = CX - BX + CY; // 270
			var L0 = SqrtDepth(BX - AX, BY - AY, D);
			var L1 = SqrtDepth(X1 - AX, Y1 - AY, D);
			var L2 = SqrtDepth(X2 - AX, Y2 - AY, D);
			var L3 = SqrtDepth(X3 - AX, Y3 - AY, D);
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
			while (L0 > 0 && (L2 = SqrtDepth(MX - EX, MY - EY, D)) > 0) {
				AR /= 2;
				L3 = L2 / 2;
				BX = MX + (EX - MX) / L2 * L3;
				BY = MY + (EY - MY) / L2 * L3;
				L2 = SqrtDepth(CX - BX, CY - BY, D);
				BX = CX + (BX - CX) / L2 * BL;
				BY = CY + (BY - CY) / L2 * BL;
				L3 = SqrtDepth(AX - BX, AY - BY, D);
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
		#region #method# TAtanS(X) 
		/// <summary>Функция возвращает обратный тангенс угла)</summary>
		/// <remarks>
		/// Вычисляет максимально близкие значения к System.Math.Atan)
		/// </remarks>
		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static BugNum TAtanS(BugNum X) {
			var M = false;
			if (X < 0) { X = -X; M = true; }
			var L = 0;
			var Y = 0;
			var XX = X * X;
			var C = (((13852575 * XX + 216602100) * XX + 891080190) * XX + 1332431100) * XX + 654729075;
			var B = ((((893025 * XX + 49116375) * XX + 425675250) * XX + 1277025750) * XX + 1550674125) * XX + 654729075;
			var R = (C / B) * X;
			return M ? -R : R;
		}
		#endregion
		#region #method# TAtan(X) 
		/// <summary>Функция возвращает обратный тангенс угла)</summary>
		/// <remarks>
		/// Вычисляет максимально близкие значения к System.Math.Atan)
		/// </remarks>
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
			BugNum XX = Y / 2;
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
				if (N == 0) N = AR[I] = TAtanS(Y * new BugNum(1, 2));
				R += N;
			}
			if (L != 0) R = (System.Math.PI / 2) - R;
			return M ? -R : R;
		}
		#endregion
		#region #method# TAtan2(Y, X) 
		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static BugNum TAtan2(BugNum Y, BugNum X) {
			if (X == 0) {
				if (Y == 0) return 0;
				else if (Y > 0) return System.Math.PI / 2; else return -(System.Math.PI / 2);
			}
			var A = TAtan(Y / X);
			if (X < 0) {
				if (Y >= 0) A += System.Math.PI; else A -= System.Math.PI;
			}
			return A;
		}
		#endregion
		#region #method# TAsin(X) 
		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static BugNum TAsin(BugNum X) {
			if (X < 0) X = -X;
			if (X > 1) return 1;
			return TAtan2(X, SqrtDepth(1 - X * X, 17));
		}
		#endregion
		#region #method# TCos(X) 
		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static BugNum TCos(BugNum X) {
			//var Test = System.Math.Cos(X);
			if (X < 0) { X = -X; }
			if (X > System.Math.PI * 2) {
				var P = X / (System.Math.PI * 2);
				X = System.Math.PI * 2 * (P - (int)P);
			}
			var M = (X > System.Math.PI / 2 && X <= System.Math.PI / 2 * 3);
			var XX = X * X;
			var XXX = XX;
			var R = 1 - (XX / 2);
			R += (XXX *= XX) / 24;
			R -= (XXX *= XX) / 720;
			R += (XXX *= XX) / 40320;
			R -= (XXX *= XX) / 3628800;
			R += (XXX *= XX) / 479001600;
			R -= (XXX *= XX) / 87178291200;
			R += (XXX *= XX) / 20922789888000;
			R -= (XXX *= XX) / 6402373705728000;
			R += (XXX *= XX) / 2432902008176640000;
			if (R < 0) R = -R;
			if (M) R = -R;
			//if (Test < 0 && !M) throw new System.InvalidOperationException();
			return R;
		}
		#endregion
		#region #method# TSin(X) 
		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static BugNum TSin(BugNum X) {
			//var Test = System.Math.Sin(X);
			X -= System.Math.PI / 2;
			if (X < 0) { X = -X; }
			if (X > System.Math.PI * 2) {
				var P = X / (System.Math.PI * 2);
				X = System.Math.PI * 2 * (P - (int)P);
			}
			var M = (X > System.Math.PI / 2 && X <= System.Math.PI / 2 * 3);
			var XX = X * X;
			var XXX = XX;
			var R = 1 - (XX / 2);
			R += (XXX *= XX) / 24;
			R -= (XXX *= XX) / 720;
			R += (XXX *= XX) / 40320;
			R -= (XXX *= XX) / 3628800;
			R += (XXX *= XX) / 479001600;
			R -= (XXX *= XX) / 87178291200;
			R += (XXX *= XX) / 20922789888000;
			R -= (XXX *= XX) / 6402373705728000;
			R += (XXX *= XX) / 2432902008176640000;
			if (R < 0) R = -R;
			if (M) R = -R;
			//if (Test < 0 && !M) throw new System.InvalidOperationException();
			return R;
		}
		#endregion
		#region #method# TSinCos(X) 
		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static void TSinCos(BugNum X, out BugNum Sin, out BugNum Cos) {
			var S = false;
			var C = new BugNum(0);
			Next:
			if (S) X -= System.Math.PI / 2;
			var x = X;
			if (x < 0) { x = -x; }
			if (x > System.Math.PI * 2) {
				var P = x / (System.Math.PI * 2);
				x = System.Math.PI * 2 * (P - (int)P);
			}
			var M = (x > System.Math.PI / 2 && x <= System.Math.PI / 2 * 3);
			var XX = x * x;
			var XXX = XX;
			var R = 1 - (XX / 2);
			R += (XXX *= XX) / 24;
			R -= (XXX *= XX) / 720;
			R += (XXX *= XX) / 40320;
			R -= (XXX *= XX) / 3628800;
			R += (XXX *= XX) / 479001600;
			R -= (XXX *= XX) / 87178291200;
			R += (XXX *= XX) / 20922789888000;
			R -= (XXX *= XX) / 6402373705728000;
			R += (XXX *= XX) / 2432902008176640000;
			if (R < 0) R = -R;
			if (M) R = -R;
			if (!S) { C = R; S = true; goto Next; }
			Cos = C;
			Sin = R;
		}
		#endregion
		#region #method# TTan(X) 
		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static BugNum TTan(BugNum X) {
			//var Test = System.Math.Tan(X);
			var S = false;
			BugNum C = 0;
			Next:
			if (S) X -= System.Math.PI / 2;
			var x = X;
			if (x < 0) { x = -x; }
			if (x > System.Math.PI * 2) {
				var P = x / (System.Math.PI * 2);
				x = System.Math.PI * 2 * (P - (int)P);
			}
			var M = (x > System.Math.PI / 2 && x <= System.Math.PI / 2 * 3);
			var XX = x * x;
			var XXX = XX;
			var R = 1 - (XX / 2);
			R += (XXX *= XX) / 24;
			R -= (XXX *= XX) / 720;
			R += (XXX *= XX) / 40320;
			R -= (XXX *= XX) / 3628800;
			R += (XXX *= XX) / 479001600;
			R -= (XXX *= XX) / 87178291200;
			R += (XXX *= XX) / 20922789888000;
			R -= (XXX *= XX) / 6402373705728000;
			R += (XXX *= XX) / 2432902008176640000;
			if (R < 0) R = -R;
			if (M) R = -R;
			if (!S) { C = R; S = true; goto Next; }
			R /= C;
			//if (Test < 0 && R >= 0) throw new System.InvalidOperationException();
			return R;
		}
		#endregion
		#region #method# TCot(x) 
		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static BugNum TCot(BugNum x) {
			return (1.0 / TTan(x));
		}
		#endregion
		#region #method# Cos(X) 
		public static BugNum Cos(BugNum X, int Depth = 5, int Count = 64) {
			var M = false;
			if (X < 0) { X = -X; M = true; }
			X = X.Round(Count);
			var XX = (X * X).Round(Count);
			var XXX = XX;
			var R = 1 - (XX / 2);
			XXX *= XX;
			XXX = XXX.Round(Count);
			R += XXX / 24;
			R = R.Round(Count);
			uint P = 5;
			var F = new BugInt(24);
			for (var I = 0; I < Depth; I++) {
				F *= (P++ * P++);
				XXX *= XX;
				XXX = XXX.Round(Count);
				R -= XXX / F;
				R = R.Round(Count);
				XXX = XXX.Round(Count);
				F *= (P++ * P++);
				XXX *= XX;
				XXX = XXX.Round(Count);
				R += XXX / F;
				R = R.Round(Count);
			}
			if (M) R = -R;
			return R;
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
				var Depth = this.Depth;
				if (Venom > 0 && Depth == 0) {
					BugInt.DivMod(Numer, Venom, out var Num);
					var Ven = Venom;
					var D = 0;
					var MD = MaxDepth;
					while (Num > 0 && D < MD) {
						D++;
						Ven = BugInt.DivMod(Ven, 10u, out var Mod);
						if (Ven == 0) { if (Mod > 0) D = -D; break; }
						BugInt.DivMod(Num, Ven, out Num);
					}
					Depth = D;
				}
				if (Depth != 0) {
					var D = BugInt.Pow(10, Depth < 0 ? MaxDepth : Depth);
					if (Venom == 0) { Venom = D; } else if (Venom != D) {
						Numer = D / Venom * Numer;
						Venom = D;
					}
					var Int = Numer;
					if (Venom > 0) {
						Int /= Venom;
						Numer -= Int * Venom;
					}
					var R = Numer.ToDouble();
					if (Int > 0) R += (ulong)Int;
					if (Minus) R = -R;
					return R;
				} else {
					var Int = Numer;
					if (Venom > 0) Int /= Venom;
					var R = 0.0;
					if (Int > 0) R += (ulong)Int;
					if (Minus) R = -R;
					return R;
				}
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
			var PI = System.Math.PI * 2 * AR;
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
			var R = (0.5 / System.Math.PI) * (TAtan2(AY - CY, AX - CX) - TAtan2(BY - CY, BX - CX));
			if (R < 0) R += 1;
			return R;
		}
		#endregion
	}
}
