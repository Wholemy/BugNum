namespace Wholemy {
	public struct BugNum {
		public BugInt Numer;
		public BugInt Venom;
		#region #new# (Value) 
		public BugNum(BugNum Value) {
			BugInt Numer = Value.Numer; BugInt Venom = Value.Venom;
			BugInt N, V;
			uint NM = 0, VM = 0, DM = 1000000000;
			if (Numer != 0 && Venom != 0) {
			NextZero:
				N = BugInt.DivMod(Numer, DM, out NM);
				V = BugInt.DivMod(Venom, DM, out VM);
				if (NM == 0 && VM == 0) { Numer = N; Venom = V; goto NextZero; } else {
					while (DM > 10) { DM /= 10; if (NM % DM == 0 && VM % DM == 0) goto NextZero; }
				}
			}
			//if (Numer != 0 && Venom != 0) Gcd(ref Numer, ref Venom);
			this.Numer = Numer;
			this.Venom = Venom;
		}
		#endregion
		#region #new# (Numer, Venom, Bound) 
		public BugNum(BugInt Numer, BugInt Venom) {
			if (Venom < 1) throw new System.InvalidOperationException();
			if (Numer != 0 && Venom != 0) {
				BugInt N, V; uint NM = 0, VM = 0, DM = 1000000000;
			Next:
				N = BugInt.DivMod(Numer, DM, out NM);
				V = BugInt.DivMod(Venom, DM, out VM);
				if (NM == 0 && VM == 0) { Numer = N; Venom = V; goto Next; } else {
					while (DM > 10) { DM /= 10; if (NM % DM == 0 && VM % DM == 0) goto Next; }
				}
			}
			//if (Numer != 0 && Venom != 0) Gcd(ref Numer, ref Venom);
			this.Numer = Numer;
			this.Venom = Venom;
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
				var Venom = BugInt.Pow(10, value.Length - dot);
				if (Numer != 0 && Venom != 0) Gcd(ref Numer, ref Venom);
				this.Numer = Numer;
				this.Venom = Venom;
			}
		}
		#endregion
		#region #method# Gcd(Numer, Venom) 
		private static BugInt Gcd(ref BugInt Numer, ref BugInt Venom) {
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
				var Max = BugInt.Bits(-Count);
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
			var Numer = this.Numer;
			var Venom = this.Venom;
			var Divis = Gcd(ref Numer, ref Venom);
			var Integer = Numer / Venom;
			var Renom = Venom;
			var Remin = Numer % Renom;
			if (Remin == 0) return Integer.ToString();
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
					return /*Numer.ToString() + ":" + Venom.ToString() + " = " +*/ Integer.ToString() + "." + SB.ToString() + "......";
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
			return /*Numer.ToString() + ":" + Venom.ToString() + " = " +*/ Integer.ToString() + "." + SB.ToString();
		}
		#endregion
		#region #operator# / 
		public static BugNum operator /(BugNum L, BugNum R) {
			if (R.Numer == 0) throw new System.DivideByZeroException("R");
			return new BugNum(L.Numer * R.Venom, L.Venom * +R.Numer);
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
		#region #new# (#double # Value) 
		#region #through# 
#if TRACE
		[System.Diagnostics.DebuggerStepThrough]
#endif
		#endregion
		public BugNum(double Value, int Bound = 0) {
			var Minus = false;
			if (Value < 0) { Value = -Value; }
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
		public static bool operator ==(BugNum L, BugNum R) {
			return L.Numer * R.Venom == R.Numer * L.Venom;
		}
		public static bool operator !=(BugNum L, BugNum R) {
			return L.Numer * R.Venom != R.Numer * L.Venom;
		}
		public static bool operator >(BugNum L, BugNum R) {
			return L.Numer * R.Venom > R.Numer * L.Venom;
		}
		public static bool operator >=(BugNum L, BugNum R) {
			return L.Numer * R.Venom >= R.Numer * L.Venom;
		}
		public static bool operator <(BugNum L, BugNum R) {
			return L.Numer * R.Venom < R.Numer * L.Venom;
		}
		public static bool operator <=(BugNum L, BugNum R) {
			return L.Numer * R.Venom <= R.Numer * L.Venom;
		}
		public static explicit operator BugInt(BugNum L) {
			return L.Numer / L.Venom;
		}
		public static implicit operator BugNum(BugInt L) {
			return new BugNum(L, 1);
		}
		public static implicit operator BugNum(int L) {
			return new BugNum(L, 1);
		}
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
			BugInt R = (BugInt)AR;
			if (AR < 0) { AR = 1 + (AR - R); R %= 4; R += 3; } else { AR -= R; R %= 4; }
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
		#region #method# Cos(X) 
		public static double Cos(double X) {
			var M = false;
			if (X < 0) { X = -X; M = true; }
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
			if (M) R = -R;
			return R;
		}
		#endregion
		#region #method# Sin(X) 
		public static double Sin(double X) {
			var XX = -(X * X);
			var Q = X;
			var R = X;
			// var P = 1; var N = (P++ * P++ * 4);
			R += (Q *= XX / 8);
			R += (Q *= XX / 48);
			R += (Q *= XX / 120);
			R += (Q *= XX / 224);
			R += (Q *= XX / 360);
			R += (Q *= XX / 528);
			R += (Q *= XX / 728);
			R += (Q *= XX / 960);
			R += (Q *= XX / 1224);
			R += (Q *= XX / 1520);
			if (R >= 1.0) R = 1.0;
			if (R <= -1.0) R = -1.0;
			return R;
		}
		#endregion
		#region #method# Sin(X) 
		public static BugNum Sin(BugNum X) {
			var XX = -(X * X);
			var Q = X;
			var R = X;
			// var P = 1; var N = (P++ * P++ * 4);
			R += (Q *= XX / 8);
			R += (Q *= XX / 48);
			R += (Q *= XX / 120);
			R += (Q *= XX / 224);
			R += (Q *= XX / 360);
			R += (Q *= XX / 528);
			R += (Q *= XX / 728);
			R += (Q *= XX / 960);
			R += (Q *= XX / 1224);
			R += (Q *= XX / 1520);
			if (R >= 1.0) R = 1.0;
			if (R <= -1.0) R = -1.0;
			return R;
		}
		#endregion
		#region #method# Cos(X) 
		public static BugNum Cos(BugNum X) {
			var M = false;
			if (X < 0) { X = -X; M = true; }
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
			//uint P = 21;
			//var F = new BugInt(2432902008176640000);
			//F *= (P++ * P++);
			//R += (XXX *= XX) / F;
			//for (var I = 0; I < 5; I++) {
			//	F *= (P++ * P++);
			//	R -= (XXX *= XX) / F;
			//	F *= (P++ * P++);
			//	R += (XXX *= XX) / F;
			//}
			if (M) R = -R;
			return R;
		}
		#endregion
	}
}
