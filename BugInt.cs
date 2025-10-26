namespace Wholemy {
	/// <summary>Большое целое число)</summary>
	/// <remarks>
	/// Внимание! Основные операторы числа отличаются от принятых в C#) Знаки -(neg) +(pos) !(not) работают как должны были бы)
	/// </remarks>
	public struct BugInt {
		#region #field# Count 
		/// <summary>Количество элементов в массиве) Содержит значение, если массив не указан)</summary>
		#region #invisible# 
#if TRACE
		[System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
#endif
		#endregion
		private int Count;
		#endregion
		#region #field# Value 
		/// <summary>Массив значения, если нулевой, значение содержит Count)</summary>
		#region #invisible# 
#if TRACE
		[System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
#endif
		#endregion
		private uint[] Value;
		#endregion
		#region #property# Length 
		public int Length {
			get {
				if (Value == null) {
					if (Count == 0) return 0;
					return 1;
				}
				var C = Count;
				if (C < 0) return -C; return C;
			}
		}
		#endregion
		#region #new# (#uint #[] Value, #int # Count) 
		private BugInt(uint[] Value, int Count) {
			if (Value != null) {
				if (Value.Length == 0) {
					Value = null;
				} else if (Count == 1 || Count == -1) {
					var V = Value[0];
					if (V <= int.MaxValue) {
						Count = Count == -1 ? -(int)V : (int)V;
						Value = null;
					}
				}
			}
			this.Count = Count;
			this.Value = Value;
		}
		#endregion
		#region #new# (#bool # Minus, #uint #[] Value) 
		public BugInt(bool Minus, params uint[] Value) {
			if (Value != null && Value.Length > 0) {
				System.Array.Reverse(Value);
				this.Value = Value;
				var LCount = Value.Length;
				while (LCount > 0 && Value[LCount - 1] == 0) { LCount--; }
				this.Count = Minus ? -LCount : LCount;
			} else {
				this.Value = null;
				this.Count = Minus ? -1 : 0;
			}
		}
		#endregion
		#region #new# (#bool # Reverse, #bool # Minus, #uint #[] Value) 
		public BugInt(bool Reverse, bool Minus, params uint[] Value) {
			if (Value != null && Value.Length > 0) {
				if (Reverse) System.Array.Reverse(Value);
				this.Value = Value;
				var LCount = Value.Length;
				while (LCount > 0 && Value[LCount - 1] == 0) { LCount--; }
				this.Count = Minus ? -LCount : LCount;
			} else {
				this.Value = null;
				this.Count = Minus ? -1 : 0;
			}
		}
		#endregion
		#region #new# (#uint # Value) 
		public BugInt(uint Value) {
			if (Value != 0) {
				this.Value = new uint[] { Value };
				this.Count = 1;
			} else {
				this.Value = null;
				this.Count = 0;
			}
		}
		#endregion
		#region #new# (#int # Value) 
		public BugInt(int Value) {
			this.Value = null;
			this.Count = Value;
		}
		#endregion
		#region #new# (#ulong # Value) 
		public BugInt(ulong Value) {
			if (Value != 0) {
				var H = (uint)(Value >> 32);
				if (H != 0) {
					this.Value = new uint[] { (uint)Value, H };
					this.Count = 2;
				} else {
					if (Value <= int.MaxValue) {
						this.Value = null;
						this.Count = (int)Value;
					} else {
						this.Value = new uint[] { 1, (uint)Value };
						this.Count = 2;
					}
				}
			} else {
				this.Value = null;
				this.Count = 0;
			}
		}
		#endregion
		#region #new# (#long # Value) 
		public BugInt(long Value) {
			var Minus = false;
			if (Value < 0) { Minus = true; Value = -Value; }
			if (Value != 0) {
				var L = (uint)Value;
				var H = (uint)(Value >> 32);
				if (H != 0) {
					this.Value = new uint[] { L, H };
					this.Count = Minus ? -2 : 2;
				} else {
					if (L <= int.MaxValue) {
						this.Value = null;
						this.Count = Minus ? -(int)L : (int)L;
					} else {
						this.Value = new uint[] { 1, L };
						this.Count = Minus ? -2 : 2;
					}
				}
			} else {
				this.Value = null;
				this.Count = 0;
			}
		}
		#endregion
		#region #new# (#string # Value) 
		public BugInt(string Value) {
			var T = System.Globalization.CultureInfo.InvariantCulture.NumberFormat;
			Value = Value.Trim();
			Value = Value.Replace(T.NumberGroupSeparator, "");
			var Ret = new BugInt(0);
			var Inver = false;
			var Minus = false;
			var I = 0;
			var L = Value.Length;
			int Pow = 1;
			while (I < L) {
				var C = Value[I];
				if (C >= '0' && C <= '9') { break; } else if (C == '~') { Inver = true; I++; } else if (C == '-') { Minus = true; I++; } else { I++; }
			}
			while (I < L) {
				var C = Value[I];
				if (C >= '0' && C <= '9') {
					Ret *= 10;
					Ret += new BugInt(C - 48);
					Pow *= 10;
				}
				I++;
			}
			if (Minus) { Ret = -Ret; }
			if (Inver) { Ret = ~Ret; }
			this = Ret;
		}
		#endregion
		#region #method# ToString 
		/// <summary>Возвращает строковое значение числа в десятичном виде)</summary>
		/// <returns>Строковое значение числа в десятичном виде)</returns>
		public override string ToString() {
			var Count = this.Count;
			if (Count == 0) return "0";
			if (Value == null) return Count.ToString();
			var Minus = false;
			if (Count < 0) { Count = -Count; Minus = true; }
			var SArray = this.Value;
			var TArray = new uint[Count];
			var Index = 0;
			while (Index < Count) { TArray[Index] = SArray[Index]; Index++; }
			while (Count > 0 && TArray[Count - 1] == 0) { Count--; }
			if (Count == 0) return "0";
			var CCount = Count * 11;
			var CArray = new char[CCount];
			var CIndex = CCount - 1;
			var ACount = 0;
		Next:
			var A = 0u;
			while (Count-- > 0) {
				var OO = (ulong)A;
				var LL = OO << 32 | (ulong)TArray[Count];
				A = 0u;
				if (LL != 0) { A = (uint)(LL / 10); OO = LL - (A * 10); }
				TArray[Count] = A;
				if (ACount == 0 && A != 0) {
					ACount = Count + 1;
				}
				A = (uint)OO;
			}
			if (CIndex > 0)
				CArray[CIndex--] = (char)(((ushort)'0') + A);
			else
				throw new System.InvalidOperationException();
			if (ACount > 0) {
				Count = ACount;
				ACount = 0;
				goto Next;
			}
			if (Minus) { CArray[CIndex] = '-'; } else { CIndex++; }
			return new string(CArray, CIndex, CCount - CIndex);
		}
		#endregion
		#region #method# ToDouble 
		public double ToDouble() {
			var Count = this.Count;
			if (Count == 0) return 0;
			if (Value == null) return Count;
			var Minus = false;
			if (Count < 0) { Count = -Count; Minus = true; }
			var SArray = this.Value;
			var TArray = new uint[Count];
			var Index = 0;
			while (Index < Count) { TArray[Index] = SArray[Index]; Index++; }
			while (Count > 0 && TArray[Count - 1] == 0) { Count--; }
			if (Count == 0) return 0;
			var CCount = Count * 11;
			var ACount = 0;
			var R = 0.0;
		Next:
			var A = 0u;
			while (Count-- > 0) {
				var OO = (ulong)A;
				var LL = OO << 32 | (ulong)TArray[Count];
				A = 0u;
				if (LL != 0) { A = (uint)(LL / 10); OO = LL - (A * 10); }
				TArray[Count] = A;
				if (ACount == 0 && A != 0) {
					ACount = Count + 1;
				}
				A = (uint)OO;
			}
			R += A; R /= 10;
			if (ACount > 0) {
				Count = ACount;
				ACount = 0;
				goto Next;
			}
			if (Minus) { R = -R; }
			return R;
		}
		#endregion
		#region #get# Digits 
		/// <summary>Возвращает количество десятичных цифр в числе)</summary>
		public int Digits {
			get {
				var Count = this.Count;
				if (Count == 0) return 0;
				if (Count < 0) { Count = -Count; }
				var SArray = this.Value;
				if (SArray == null) { SArray = new uint[] { (uint)Count }; Count = 1; }
				var TArray = new uint[Count];
				var Index = 0;
				while (Index < Count) { TArray[Index] = SArray[Index]; Index++; }
				while (Count > 0 && TArray[Count - 1] == 0) { Count--; }
				if (Count == 0) return 0;
				var ACount = 0;
				var R = 0;
			Next:
				var A = 0u;
				while (Count-- > 0) {
					var OO = (ulong)A;
					var LL = OO << 32 | (ulong)TArray[Count];
					A = 0u;
					if (LL != 0) { A = (uint)(LL / 10); OO = LL - (A * 10); }
					TArray[Count] = A;
					if (ACount == 0 && A != 0) {
						ACount = Count + 1;
					}
					A = (uint)OO;
				}
				R++;
				if (ACount > 0) {
					Count = ACount;
					ACount = 0;
					goto Next;
				}
				return R;
			}
		}
		#endregion
		#region #get# Zerone 
		/// <summary>Возвращает количество десятичных нулей в конце числа, значение меньше нуля если число не заканчивается единицей)</summary>
		public int Zerone {
			get {
				var Count = this.Count;
				if (Count == 0) return 0;
				if (Count < 0) { Count = -Count; }
				var SArray = this.Value;
				if (SArray == null) { SArray = new uint[] { (uint)Count }; Count = 1; }
				var TArray = new uint[Count];
				var Index = 0;
				while (Index < Count) { TArray[Index] = SArray[Index]; Index++; }
				while (Count > 0 && TArray[Count - 1] == 0) { Count--; }
				if (Count == 0) return 0;
				var ACount = 0;
				var R = 0;
				var One = false;
			Next:
				var A = 0u;
				while (Count-- > 0) {
					var OO = (ulong)A;
					var LL = OO << 32 | (ulong)TArray[Count];
					A = 0u;
					if (LL != 0) { A = (uint)(LL / 10); OO = LL - (A * 10); }
					TArray[Count] = A;
					if (ACount == 0 && A != 0) {
						ACount = Count + 1;
					}
					A = (uint)OO;
				}
				if (One) {
					R = -R; return R;
				} else if (A == 0) {
					R++;
				} else {
					if (A != 1) {
						R = -R; return R;
					}
					One = true;
				}
				if (ACount > 0) {
					Count = ACount;
					ACount = 0;
					goto Next;
				}
				return R;
			}
		}
		#endregion
		#region #operator# * (L, R) 
		#region #operator# * (#struct # L, #uint # R) 
		public static BugInt operator *(BugInt L, uint R) {
			var LCount = L.Count;
			var Minus = false;
			if (LCount < 0) { Minus = true; LCount = -LCount; }
			var LArray = L.Value;
			if (LArray == null && LCount != 0) { LArray = new uint[] { (uint)LCount }; LCount = 1; }
			if (R == 1) { if (Minus) LCount = -LCount; return new BugInt(LArray, LCount); }
			if (LCount == 0) return new BugInt(null, Minus ? -1 : 0);
			var OArray = new uint[LCount + 1];
			var LIndex = 0;
			var Muled = ((ulong)LArray[LIndex++]) * R;
			var OCount = 0;
			var OBelow = OArray[OCount++] = (uint)Muled;
			var OAbove = (uint)(Muled >> 32);
			while (LIndex < LCount) {
				Muled = ((ulong)LArray[LIndex++]) * R + OAbove;
				OArray[OCount++] = (uint)(Muled);
				OAbove = (uint)(Muled >> 32);
			}
			OArray[OCount++] = OAbove;
			while (OCount > 0 && OArray[OCount - 1] == 0) { OCount--; }
			if (OCount == 0) { OArray = null; } else {
				if (Minus) OCount = -OCount;
			}
			return new BugInt(OArray, OCount);
		}
		#endregion
		#region #operator# * (#struct # L, #int # R) 
		public static BugInt operator *(BugInt L, int R) {
			var Minus = false;
			if (R < 0) { Minus = true; R = -R; }
			var LCount = L.Count;
			if (LCount < 0) { Minus = Minus ? false : true; LCount = -LCount; }
			var LArray = L.Value;
			if (LArray == null && LCount != 0) { LArray = new uint[] { (uint)LCount }; LCount = 1; }
			if (R == 1) { if (Minus) LCount = -LCount; return new BugInt(LArray, LCount); }
			if (LCount == 0) return new BugInt(null, Minus ? -1 : 0);
			var OArray = new uint[LCount + 1];
			var LIndex = 0;
			var Muled = ((ulong)LArray[LIndex++]) * (uint)R;
			var OCount = 0;
			var OBelow = OArray[OCount++] = (uint)Muled;
			var OAbove = (uint)(Muled >> 32);
			while (LIndex < LCount) {
				Muled = ((ulong)LArray[LIndex++]) * (uint)R + OAbove;
				OArray[OCount++] = (uint)(Muled);
				OAbove = (uint)(Muled >> 32);
			}
			OArray[OCount++] = OAbove;
			while (OCount > 0 && OArray[OCount - 1] == 0) { OCount--; }
			if (OCount == 0) { OArray = null; } else {
				if (Minus) OCount = -OCount;
			}
			return new BugInt(OArray, OCount);
		}
		#endregion
		#region #operator# * (#int # L, #struct # R) 
		public static BugInt operator *(int L, BugInt R) {
			return R * L;
		}
		#endregion
		#region #operator# * (#uint # L, #struct # R) 
		public static BugInt operator *(uint L, BugInt R) {
			return R * L;
		}
		#endregion
		#region #operator# * (#struct # L, #struct # R)
		public static BugInt operator *(BugInt L, BugInt R) {
			var Minus = false;
			var RCount = R.Count;
			if (RCount < 0) { Minus = true; RCount = -RCount; }
			var RArray = R.Value;
			if (RArray == null && RCount != 0) { RArray = new uint[] { (uint)RCount }; RCount = 1; }
			var LCount = L.Count;
			if (LCount < 0) { Minus = Minus ? false : true; LCount = -LCount; }
			var LArray = L.Value;
			if (LArray == null && LCount != 0) { LArray = new uint[] { (uint)LCount }; LCount = 1; }
			if (RCount == 1 && RArray[0] == 1) { if (Minus) LCount = -LCount; return new BugInt(LArray, LCount); }
			if (LCount == 1 && LArray[0] == 1) return R;
			if (LCount == 0 || RCount == 0) return new BugInt(null, 0);
			uint[] OArray = null;
			var OCount = 0;
			var OMinus = false;
			if (LCount < RCount) {
				OCount = RCount; RCount = LCount; LCount = OCount;
				OMinus = Minus; Minus = Minus; Minus = OMinus;
				OArray = RArray; RArray = LArray; LArray = OArray;
				OArray = null;
				OMinus = false;
				OCount = 0;
			}
			var LIndex = 0;
			ulong Muled;
			uint Above;
			uint Value;
			if (RCount == 1) {
				OArray = new uint[LCount + 1];
				Value = RArray[0];
				Muled = ((ulong)LArray[LIndex++]) * Value;
				OCount = 0;
				OArray[OCount++] = (uint)Muled;
				Above = (uint)(Muled >> 32);
				while (LIndex < LCount) {
					Muled = ((ulong)LArray[LIndex++]) * Value + Above;
					OArray[OCount++] = (uint)(Muled);
					Above = (uint)(Muled >> 32);
				}
				OArray[OCount++] = Above;
			} else {
				OArray = new uint[LCount + RCount];
				while (LIndex < LCount) {
					var OIndex = LIndex;
					var RIndex = 0;
					Value = LArray[LIndex++];
					Muled = ((ulong)RArray[RIndex++]) * Value + OArray[OIndex];
					OArray[OIndex++] = (uint)Muled;
					Above = (uint)(Muled >> 32);
					while (RIndex < RCount) {
						Muled = ((ulong)RArray[RIndex++]) * Value + OArray[OIndex] + Above;
						OArray[OIndex++] = (uint)(Muled);
						Above = (uint)(Muled >> 32);
					}
					while (Above != 0) {
						Muled = ((ulong)Above) + OArray[OIndex];
						OArray[OIndex++] = (uint)(Muled);
						Above = (uint)(Muled >> 32);
					}
					if (OIndex > OCount) OCount = OIndex;
				}
			}
			while (OCount > 0 && OArray[OCount - 1] == 0) { OCount--; }
			if (OCount == 0) { OArray = null; } else if (Minus) OCount = -OCount;
			return new BugInt(OArray, OCount);
		}
		#endregion
		#endregion
		#region #operator# / (L, R) 
		#region #operator# / (#struct # L, #uint # R) 
		public static BugInt operator /(BugInt L, uint R) {
			if (R == 0) { throw new System.DivideByZeroException(); }
			if (R == 1) return L;
			var LCount = L.Count;
			var Minus = false;
			if (LCount < 0) { Minus = true; LCount = -LCount; }
			var LArray = L.Value;
			if (LCount == 0) return new BugInt(null, 0);
			if (LArray == null && LCount != 0) { LArray = new uint[] { (uint)LCount }; LCount = 1; }
			var OCount = LCount;
			var OArray = new uint[LCount--];
			var Remnant = (ulong)LArray[LCount];
			var Divided = Remnant / R;
			Remnant -= Divided * R;
			OArray[LCount] = (uint)Divided;
			while (--LCount >= 0) {
				Remnant <<= 32;
				Remnant |= LArray[LCount];
				Divided = Remnant / R;
				Remnant -= Divided * R;
				OArray[LCount] = (uint)Divided;
			}
			while (OCount > 0 && OArray[OCount - 1] == 0) { OCount--; }
			if (OCount == 0) { OArray = null; } else if (Minus) OCount = -OCount;
			return new BugInt(OArray, OCount);
		}
		#endregion
		#region #operator# / (#struct # L, #int # R) 
		public static BugInt operator /(BugInt L, int R) {
			if (R == 0) { throw new System.DivideByZeroException(); }
			var Minus = false;
			if (R < 0) { R = -R; Minus = true; }
			var LCount = L.Count;
			if (LCount < 0) { Minus = Minus ? false : true; LCount = -LCount; }
			var LArray = L.Value;
			if (LArray == null && LCount != 0) { LArray = new uint[] { (uint)LCount }; LCount = 1; }
			if (R == 1) { if (Minus) { return new BugInt(LArray, -LCount); } return L; }
			if (LCount == 0) return new BugInt(null, 0);
			var OCount = LCount;
			var OArray = new uint[LCount--];
			var Remnant = (ulong)LArray[LCount];
			var Divided = Remnant / (uint)R;
			Remnant -= Divided * (uint)R;
			OArray[LCount] = (uint)Divided;
			while (--LCount >= 0) {
				Remnant <<= 32;
				Remnant |= LArray[LCount];
				Divided = Remnant / (uint)R;
				Remnant -= Divided * (uint)R;
				OArray[LCount] = (uint)Divided;
			}
			while (OCount > 0 && OArray[OCount - 1] == 0) { OCount--; }
			if (OCount == 0) OArray = null;
			if (Minus) OCount = -OCount;
			return new BugInt(OArray, OCount);
		}
		#endregion
		#region #operator# / (#struct # L, #struct # R) 
		public static BugInt operator /(BugInt L, BugInt R) {
			return DivMod(L, R, out var M);
		}
		#endregion
		#endregion
		#region #operator# % (#struct # L, #struct # R) 
		public static BugInt operator %(BugInt L, BugInt R) {
			DivMod(L, R, out var M);
			return M;
		}
		#endregion
		#region #method# DivMod(L, R, M) 
		#region #method# DivMod(#struct # L, #int # R, #int # M) 
		public static BugInt DivMod(BugInt L, int R, out int M) {
			if (R == 0) { throw new System.DivideByZeroException(); }
			var Minus = false;
			if (R < 0) { Minus = true; R = -R; }
			if (R == 1) { M = 0; if (Minus) { return new BugInt(L.Value, -L.Count); } return L; }
			var LCount = L.Count;
			if (LCount < 0) { Minus = Minus ? false : true; LCount = -LCount; }
			if (LCount == 0) { M = 0; return 0; }
			var LArray = L.Value;
			if (LArray == null && LCount != 0) { LArray = new uint[] { (uint)LCount }; LCount = 1; }
			if (LCount == 1 && LArray[0] < R) { if (Minus) { M = -(int)LArray[0]; } else { M = (int)LArray[0]; } return 0; }
			var OCount = LCount;
			var OArray = new uint[LCount--];
			var Remnant = (ulong)LArray[LCount];
			var Divided = Remnant / (uint)R;
			Remnant -= Divided * (uint)R;
			OArray[LCount] = (uint)Divided;
			while (--LCount >= 0) {
				Remnant <<= 32;
				Remnant |= LArray[LCount];
				Divided = Remnant / (uint)R;
				Remnant -= Divided * (uint)R;
				OArray[LCount] = (uint)Divided;
			}
			while (OCount > 0 && OArray[OCount - 1] == 0) { OCount--; }
			if (OCount == 0) { OArray = null; } else if (Minus) OCount = -OCount;
			M = (int)Remnant;
			return new BugInt(OArray, OCount);
		}
		#endregion
		#region #method# DivMod(#struct # L, #uint # R, #uint # M) 
		public static BugInt DivMod(BugInt L, uint R, out uint M) {
			if (R == 0) { throw new System.DivideByZeroException(); }
			if (R == 1) { M = 0u; return L; }
			var LCount = L.Count;
			if (LCount == 0) { }
			var Minus = false;
			if (LCount < 0) { Minus = true; LCount = -LCount; }
			if (LCount == 0) { M = 0; return 0; }
			var LArray = L.Value;
			if (LArray == null && LCount != 0) { LArray = new uint[] { (uint)LCount }; LCount = 1; }
			if (LCount == 1 && LArray[0] < R) { M = LArray[0]; return 0; }
			var OCount = LCount;
			var OArray = new uint[LCount--];
			var Remnant = (ulong)LArray[LCount];
			var Divided = Remnant / R;
			Remnant -= Divided * R;
			OArray[LCount] = (uint)Divided;
			while (--LCount >= 0) {
				Remnant <<= 32;
				Remnant |= LArray[LCount];
				Divided = Remnant / R;
				Remnant -= Divided * R;
				OArray[LCount] = (uint)Divided;
			}
			while (OCount > 0 && OArray[OCount - 1] == 0) { OCount--; }
			if (OCount == 0) { OArray = null; } else if (Minus) OCount = -OCount;
			M = (uint)Remnant;
			return new BugInt(OArray, OCount);
		}
		#endregion
		#region #method# DivMod(#struct # L, #struct # R, #struct # M) 
		public static BugInt DivMod(BugInt L, BugInt R, out BugInt M) {
			var RCount = R.Count;
			var Minus = false;
			if (RCount < 0) { RCount = -RCount; Minus = true; }
			var RArray = R.Value;
			if (RCount == 0) { throw new System.DivideByZeroException(); }
			if (RArray == null && RCount != 0) { RArray = new uint[] { (uint)RCount }; RCount = 1; }
			var RValue = RArray[RCount - 1];
			var OCount = 0;
			uint[] OArray = null;
			var LCount = L.Count;
			if (LCount < 0) { Minus = Minus ? false : true; LCount = -LCount; }
			var LArray = L.Value;
			if (LArray == null && LCount != 0) { LArray = new uint[] { (uint)LCount }; LCount = 1; }
			if (RCount == 1 && RValue == 1) { M = 0; if (Minus) { return new BugInt(LArray, -LCount); } return L; }
			if (LCount == 0) { M = 0; return 0; }
			if (RCount == 1) {
				if (LCount == 1 && LArray[0] < R) { if (Minus) { M = new BugInt(LArray, -LCount); } else { M = L; } return 0; }
				OCount = LCount--;
				OArray = new uint[OCount];
				var Remnant = (ulong)LArray[LCount];
				var Divided = Remnant / RValue;
				Remnant -= Divided * RValue;
				OArray[LCount] = (uint)Divided;
				while (--LCount >= 0) {
					Remnant <<= 32;
					Remnant |= LArray[LCount];
					Divided = Remnant / RValue;
					Remnant -= Divided * RValue;
					OArray[LCount] = (uint)Divided;
				}
				while (OCount > 0 && OArray[OCount - 1] == 0) { OCount--; }
				if (OCount == 0) { OArray = null; } else if (Minus) OCount = -OCount;
				M = new BugInt((uint)Remnant);
				return new BugInt(OArray, OCount);
			} else {
				if (LCount < RCount) { if (Minus) { M = new BugInt(L.Value, -L.Count); } else { M = L; } return 0; }
				if (LCount == RCount) {
					OCount = LCount;
					while (--OCount >= 0) {
						var LV = LArray[OCount];
						var RV = RArray[OCount];
						if (LV < RV) { if (Minus) { M = new BugInt(L.Value, -L.Count); } else { M = L; } return 0; }
						if (LV > RV) break;
					}
				}
				OArray = new uint[LCount];
				OCount = LCount;
				while (--OCount >= 0) { OArray[OCount] = LArray[OCount]; }
				LArray = OArray;
				OCount = LCount - RCount + 1;
				OArray = new uint[OCount];
			}
			var RBValue = RCount > 1 ? RArray[RCount - 2] : 0u;
			//var SHL = CbitHz(RValue);
			var SHL = 0;
			var V = RValue;
			if (V != 0) {
				if ((V & 0xFFFF0000u) == 0) { SHL += 16; V <<= 16; }
				if ((V & 0xFF000000u) == 0) { SHL += 8; V <<= 8; }
				if ((V & 0xF0000000u) == 0) { SHL += 4; V <<= 4; }
				if ((V & 0xC0000000u) == 0) { SHL += 2; V <<= 2; }
				if ((V & 0x80000000u) == 0) { SHL++; }
			} else { SHL = 32; }
			var SHR = 32 - SHL;
			if (SHL > 0) {
				var RBBValue = RCount > 2 ? RArray[RCount - 3] : 0u;
				RValue = (RValue << SHL) | (RBValue >> SHR);
				RBValue = (RBValue << SHL) | (RBBValue >> SHR);
			}
			for (int LI = LCount; LI >= RCount; LI--) {
				var LVIAV = 0u;
				if (LI < LCount) { LVIAV = LArray[LI]; LArray[LI] = 0; LCount--; }
				ulong LVIV = ((ulong)LVIAV << 32) | LArray[LI - 1];
				uint LVIBV = ((LI > 1) ? LArray[LI - 2] : 0u);
				if (SHL > 0) {
					uint LVIBBV = ((LI > 2) ? LArray[LI - 3] : 0u);
					LVIV = (LVIV << SHL) | (LVIBV >> SHR);
					LVIBV = (LVIBV << SHL) | (LVIBBV >> SHR);
				}
				ulong A = LVIV / RValue;
				if (A > uint.MaxValue) A = uint.MaxValue;
				reQ:
				ulong QL = RBValue * A;
				ulong QH = (RValue * A) + (QL >> 32);
				if (QH > LVIV || (QH == LVIV && ((uint)QL) > LVIBV)) { A--; goto reQ; }
				if (A != 0) {
					var aa = 0u;
					var RIndex = 0;
					var LIndex = LCount - RCount;
					while (RIndex < RCount) {
						var Muled = A * RArray[RIndex++];
						var Subed = ((ulong)(LArray[LIndex])) - (uint)Muled - aa;
						LArray[LIndex++] = (uint)Subed;
						aa = (uint)((-(int)(Subed >> 32)) + (uint)(Muled >> 32));
					}
					if (aa != LVIAV) {
						var aaa = aa;
						aa = 0;
						RIndex = 0;
						LIndex = LCount - RCount;
						while (RIndex < RCount) {
							ulong Added = ((ulong)(LArray[LIndex])) + ((ulong)RArray[RIndex++]) + aa;
							aa = (uint)(Added >> 32);
							LArray[LIndex++] = (uint)Added;
						}
						if (aa + LVIAV != aaa) {
							throw new System.InvalidOperationException();
						}
						A--;
					}
				}
				OArray[LI - RCount] = (uint)A;
			}
			while (LCount > 0 && LArray[LCount - 1] == 0) { LCount--; }
			while (OCount > 0 && OArray[OCount - 1] == 0) { OCount--; }
			if (LCount == 0) { LArray = null; } else if (Minus) LCount = -LCount;
			M = new BugInt(LArray, LCount);
			if (OCount == 0) { OArray = null; } else if (Minus) OCount = -OCount;
			return new BugInt(OArray, OCount);
		}
		#endregion
		#endregion
		#region #method# DivModSub(L, R, M) 
		public static BugInt DivModSub(BugInt L, BugInt R, out BugInt M) {
			var Minus = false;
			var RCount = R.Count;
			if (RCount < 0) { RCount = -RCount; Minus = true; }
			var RArray = R.Value;
			if (RArray == null && RCount != 0) { RArray = new uint[] { (uint)RCount }; RCount = 1; }
			var RValue = R.Value[RCount - 1];
			var LCount = L.Count;
			if (LCount < 0) { Minus = Minus ? false : true; LCount = -LCount; }
			var LArray = L.Value;
			if (LArray == null && LCount != 0) { LArray = new uint[] { (uint)LCount }; LCount = 1; }
			if (RCount == 1 && RValue == 1) { M = 0; if (Minus) { return new BugInt(LArray, -LCount); } return L; }
			if (L == R) { M = 0; if (Minus) { return -1; } return 1; }
			if (L < R) { if (Minus) { M = -L; } else { M = L; } return 0; }
			var Shift = L.Bits - R.Bits;
			var Remain = L;
			var Svalue = R << Shift;
			var Result = new BugInt();
			while (Shift-- >= 0) {
				Result <<= 1;
				if (Svalue <= Remain) { Result |= new BugInt(1); Remain -= Svalue; }
				Svalue >>= 1;
			}
			M = Remain;
			return Result;
		}
		#endregion
		#region #method# CbitHz(V) 
		public static int CbitHz(uint V) {
			if (V == 0) return 32;
			int R = 0;
			if ((V & 0xFFFF0000u) == 0) { R += 16; V <<= 16; }
			if ((V & 0xFF000000u) == 0) { R += 8; V <<= 8; }
			if ((V & 0xF0000000u) == 0) { R += 4; V <<= 4; }
			if ((V & 0xC0000000u) == 0) { R += 2; V <<= 2; }
			if ((V & 0x80000000u) == 0) { R++; }
			return R;
		}
		#endregion
		#region #get# Bits 
		/// <summary>Возвращает количество бит в числе)</summary>
		public int Bits {
			get {
				var Array = this.Value;
				var Count = this.Count;
				if (Count < 0) Count = -Count;
				while (Count > 0 && Array[Count - 1] == 0) { Count--; }
				if (Count > 0) {
					uint V = Array[Count - 1];
					int R = 0;
					if ((V & 0xFFFF0000u) == 0) { R += 16; V <<= 16; }
					if ((V & 0xFF000000u) == 0) { R += 8; V <<= 8; }
					if ((V & 0xF0000000u) == 0) { R += 4; V <<= 4; }
					if ((V & 0xC0000000u) == 0) { R += 2; V <<= 2; }
					if ((V & 0x80000000u) == 0) { R++; }
					return Count * 32 - R;
				}
				return 0;
			}
		}
		#endregion
		#region #operator# << 
		public static BugInt operator <<(BugInt L, int Count) {
			if (Count < 0) return L >> -Count;
			if (Count > 0) {
				var Minus = false;
				var LCount = L.Count;
				if (LCount < 0) { Minus = true; LCount = -LCount; }
				var LArray = L.Value;
				if (LArray == null && LCount != 0) { LArray = new uint[] { (uint)LCount }; LCount = 1; }
				var OCount = LCount;
				uint[] OArray = null;
				if (LCount > 0) {
					var ICount = Count / 32;
					Count -= ICount * 32;
					OCount += ICount + 1;
					OArray = new uint[OCount];
					var OIndex = ICount;
					var LIndex = 0;
					if (Count == 0) {
						while (LIndex < LCount) { OArray[OIndex++] = LArray[LIndex++]; }
					} else {
						var LBelow = LArray[LIndex++];
						OArray[OIndex++] = LBelow << Count;
						var RCount = 32 - Count;
						while (LIndex < LCount) {
							var LAbove = LArray[LIndex++];
							OArray[OIndex++] = LAbove << Count | LBelow >> RCount;
							LBelow = LAbove;
						}
						OArray[OIndex++] = LBelow >> RCount;
					}
					while (OCount > 0 && OArray[OCount - 1] == 0) { OCount--; }
					if (OCount == 0) OArray = null;
				}
				if (Minus) OCount = -OCount;
				return new BugInt(OArray, OCount);
			}
			return L;
		}
		#endregion
		#region #operator# >> 
		public static BugInt operator >>(BugInt L, int Count) {
			if (Count < 0) return L << -Count;
			if (Count > 0) {
				var Minus = false;
				var LCount = L.Count;
				if (LCount < 0) { Minus = true; LCount = -LCount; }
				var LArray = L.Value;
				if (LArray == null && LCount != 0) { LArray = new uint[] { (uint)LCount }; LCount = 1; }
				//while (LCount > 0 && LArray[LCount - 1] == 0) { LCount--; }
				var OCount = LCount;
				uint[] OArray = null;
				if (LCount > 0) {
					var ICount = Count / 32;
					Count -= ICount * 32;
					OCount -= ICount;
					OArray = new uint[OCount];
					var OIndex = 0;
					var LIndex = ICount;
					if (Count == 0) {
						while (LIndex < LCount) { OArray[OIndex++] = LArray[LIndex++]; }
					} else {
						var LBelow = LArray[LIndex++];
						var RCount = 32 - Count;
						while (LIndex < LCount) {
							var LAbove = LArray[LIndex++];
							OArray[OIndex++] = LAbove << RCount | LBelow >> Count;
							LBelow = LAbove;
						}
						OArray[OIndex++] = LBelow >> Count;
					}
					while (OCount > 0 && OArray[OCount - 1] == 0) { OCount--; }
					if (OCount == 0) OArray = null;
				}
				if (Minus) OCount = -OCount;
				return new BugInt(OArray, OCount);
			}
			return L;
		}
		#endregion
		#region #operator# !== 
		#region #operator# !== (#struct # L, #int # R) 
		public static bool operator !=(BugInt L, int R) {
			var LCount = L.Count;
			var LArray = L.Value;
			if (LArray == null && LCount != 0) { LArray = new uint[] { (uint)LCount }; LCount = LCount < 0 ? -1 : 1; }
			if (R != 0) {
				if (R < 0) {
					return LCount != -1 || LArray[0] != (-R);
				} else { return LCount != 1 || LArray[0] != R; }
			} else { return LCount != 0; }
		}
		public static bool operator ==(BugInt L, int R) {
			var LCount = L.Count;
			var LArray = L.Value;
			if (LArray == null && LCount != 0) { LArray = new uint[] { (uint)LCount }; LCount = LCount < 0 ? -1 : 1; }
			if (R != 0) { if (R < 0) { return LCount == -1 && LArray[0] == (-R); } else { return LCount == 1 && LArray[0] == R; } } else { return LCount == 0; }
		}
		#endregion
		#region #operator# !== (#struct # L, #uint # R) 
		public static bool operator !=(BugInt L, uint R) {
			var LCount = L.Count;
			var LArray = L.Value;
			if (LArray == null && LCount != 0) { LArray = new uint[] { (uint)LCount }; LCount = LCount < 0 ? -1 : 1; }
			if (R != 0) { return LCount != 1 || LArray[0] != R; } else { return LCount != 0; }
		}
		public static bool operator ==(BugInt L, uint R) {
			var LCount = L.Count;
			var LArray = L.Value;
			if (LArray == null && LCount != 0) { LArray = new uint[] { (uint)LCount }; LCount = LCount < 0 ? -1 : 1; }
			if (R != 0) { return LCount == 1 && LArray[0] == R; } else { return LCount == 0; }
		}
		#endregion
		#region #operator# !== (#int # L, #struct # R) 
		public static bool operator !=(int L, BugInt R) {
			var RCount = R.Count;
			var RArray = R.Value;
			if (RArray == null && RCount != 0) { RArray = new uint[] { (uint)RCount }; RCount = RCount < 0 ? -1 : 1; }
			if (L != 0) { if (L < 0) { return RCount != -1 || RArray[0] != (-L); } else { return RCount != 1 || RArray[0] != L; } } else { return RCount != 0; }
		}
		public static bool operator ==(int L, BugInt R) {
			var RCount = R.Count;
			var RArray = R.Value;
			if (RArray == null && RCount != 0) { RArray = new uint[] { (uint)RCount }; RCount = RCount < 0 ? -1 : 1; }
			if (L != 0) { if (L < 0) { return RCount == -1 && RArray[0] == (-L); } else { return RCount == 1 && RArray[0] == L; } } else { return RCount == 0; }
		}
		#endregion
		#region #operator# !== (#uint # L, #struct # R) 
		public static bool operator !=(uint L, BugInt R) {
			var RCount = R.Count;
			var RArray = R.Value;
			if (RArray == null && RCount != 0) { RArray = new uint[] { (uint)RCount }; RCount = RCount < 0 ? -1 : 1; }
			if (L != 0) { return RCount != 1 || RArray[0] != L; } else { return RCount != 0; }
		}
		public static bool operator ==(uint L, BugInt R) {
			var RCount = R.Count;
			var RArray = R.Value;
			if (RArray == null && RCount != 0) { RArray = new uint[] { (uint)RCount }; RCount = RCount < 0 ? -1 : 1; }
			if (L != 0) { return RCount == 1 && RArray[0] == L; } else { return RCount == 0; }
		}
		#endregion
		#region #operator# !== (#struct # L, #struct # R) 
		public static bool operator !=(BugInt L, BugInt R) {
			var LCount = L.Count;
			var LArray = L.Value;
			if (LArray == null && LCount != 0) { LArray = new uint[] { (uint)LCount }; LCount = LCount < 0 ? -1 : 1; }
			var RCount = R.Count;
			var RArray = R.Value;
			if (RArray == null && RCount != 0) { RArray = new uint[] { (uint)RCount }; RCount = RCount < 0 ? -1 : 1; }
			if (LCount != RCount) return true;
			if (LCount < 0) LCount = -LCount;
			if (LArray != RArray) {
				while (--LCount >= 0) { if (LArray[LCount] != RArray[LCount]) return true; }
			}
			return false;
		}
		public static bool operator ==(BugInt L, BugInt R) {
			var LCount = L.Count;
			var LArray = L.Value;
			if (LArray == null && LCount != 0) { LArray = new uint[] { (uint)LCount }; LCount = LCount < 0 ? -1 : 1; }
			var RCount = R.Count;
			var RArray = R.Value;
			if (RArray == null && RCount != 0) { RArray = new uint[] { (uint)RCount }; RCount = RCount < 0 ? -1 : 1; }
			if (LCount != RCount) return false;
			if (LCount < 0) LCount = -LCount;
			if (LArray != RArray) {
				while (--LCount >= 0) { if (LArray[LCount] != RArray[LCount]) return false; }
			}
			return true;
		}
		#endregion
		#endregion
		#region #operator# > 
		public static bool operator >(BugInt L, BugInt R) {
			var LCount = L.Count;
			var LArray = L.Value;
			if (LArray == null && LCount != 0) { LArray = new uint[] { (uint)LCount }; LCount = LCount < 0 ? -1 : 1; }
			var RCount = R.Count;
			var RArray = R.Value;
			if (RArray == null && RCount != 0) { RArray = new uint[] { (uint)RCount }; RCount = RCount < 0 ? -1 : 1; }
			if (LCount > RCount) return true;
			if (LCount < RCount) return false;
			if (LCount < 0) LCount = -LCount;
			if (LArray != RArray)
				while (--LCount >= 0) {
					var LValue = LArray[LCount];
					var RValue = RArray[LCount];
					if (LValue > RValue) return true;
					if (LValue < RValue) return false;
				}
			return false;
		}
		#endregion
		#region #operator# >= 
		public static bool operator >=(BugInt L, BugInt R) {
			var LCount = L.Count;
			var LArray = L.Value;
			if (LArray == null && LCount != 0) { LArray = new uint[] { (uint)LCount }; LCount = LCount < 0 ? -1 : 1; }
			var RCount = R.Count;
			var RArray = R.Value;
			if (RArray == null && RCount != 0) { RArray = new uint[] { (uint)RCount }; RCount = RCount < 0 ? -1 : 1; }
			if (LCount > RCount) return true;
			if (LCount < RCount) return false;
			if (LCount < 0) LCount = -LCount;
			if (LArray != RArray)
				while (--LCount >= 0) {
					var LValue = LArray[LCount];
					var RValue = RArray[LCount];
					if (LValue > RValue) return true;
					if (LValue < RValue) return false;
				}
			return true;
		}
		#endregion
		#region #operator# < 
		public static bool operator <(BugInt L, BugInt R) {
			var LCount = L.Count;
			var LArray = L.Value;
			if (LArray == null && LCount != 0) { LArray = new uint[] { (uint)LCount }; LCount = LCount < 0 ? -1 : 1; }
			var RCount = R.Count;
			var RArray = R.Value;
			if (RArray == null && RCount != 0) { RArray = new uint[] { (uint)RCount }; RCount = RCount < 0 ? -1 : 1; }
			if (LCount < RCount) return true;
			if (LCount > RCount) return false;
			if (LCount < 0) LCount = -LCount;
			if (LArray != RArray)
				while (--LCount >= 0) {
					var LValue = LArray[LCount];
					var RValue = RArray[LCount];
					if (LValue < RValue) return true;
					if (LValue > RValue) return false;
				}
			return false;
		}
		#endregion
		#region #operator# <= 
		public static bool operator <=(BugInt L, BugInt R) {
			var LCount = L.Count;
			var LArray = L.Value;
			if (LArray == null && LCount != 0) { LArray = new uint[] { (uint)LCount }; LCount = LCount < 0 ? -1 : 1; }
			var RCount = R.Count;
			var RArray = R.Value;
			if (RArray == null && RCount != 0) { RArray = new uint[] { (uint)RCount }; RCount = RCount < 0 ? -1 : 1; }
			if (LCount < RCount) return true;
			if (LCount > RCount) return false;
			if (LCount < 0) LCount = -LCount;
			if (LArray != RArray)
				while (--LCount >= 0) {
					var LValue = LArray[LCount];
					var RValue = RArray[LCount];
					if (LValue < RValue) return true;
					if (LValue > RValue) return false;
				}
			return true;
		}
		#endregion
		#region #operator# | 
		public static BugInt operator |(BugInt L, BugInt R) {
			var OCount = 0;
			var RCount = R.Count;
			var RArray = R.Value;
			if (RArray == null && RCount != 0) { RArray = new uint[] { (uint)RCount }; RCount = RCount < 0 ? -1 : 1; }
			var LCount = L.Count;
			var LArray = L.Value;
			if (LArray == null && LCount != 0) { LArray = new uint[] { (uint)LCount }; LCount = LCount < 0 ? -1 : 1; }
			uint[] OArray = new uint[System.Math.Max(LCount, RCount)];
			while (OCount < LCount || OCount < RCount) {
				OArray[OCount] = (OCount < LCount ? LArray[OCount] : 0u) | (OCount < RCount ? RArray[OCount] : 0u);
				OCount++;
			}
			while (OCount > 0 && OArray[OCount - 1] == 0) { OCount--; }
			return new BugInt(OArray, OCount);
		}
		#endregion
		#region #operator# & 
		public static BugInt operator &(BugInt L, BugInt R) {
			var OCount = 0;
			var RCount = R.Count;
			var RArray = R.Value;
			if (RArray == null && RCount != 0) { RArray = new uint[] { (uint)RCount }; RCount = RCount < 0 ? -1 : 1; }
			var LCount = L.Count;
			var LArray = L.Value;
			if (LArray == null && LCount != 0) { LArray = new uint[] { (uint)LCount }; LCount = LCount < 0 ? -1 : 1; }
			uint[] OArray = new uint[System.Math.Max(LCount, RCount)];
			while (OCount < LCount || OCount < RCount) {
				OArray[OCount] = (OCount < LCount ? LArray[OCount] : 0u) & (OCount < RCount ? RArray[OCount] : 0u);
				OCount++;
			}
			while (OCount > 0 && OArray[OCount - 1] == 0) { OCount--; }
			return new BugInt(OArray, OCount);
		}
		#endregion
		#region #operator# ^ 
		public static BugInt operator ^(BugInt L, BugInt R) {
			var OCount = 0;
			var RCount = R.Count;
			var RArray = R.Value;
			if (RArray == null && RCount != 0) { RArray = new uint[] { (uint)RCount }; RCount = RCount < 0 ? -1 : 1; }
			var LCount = L.Count;
			var LArray = L.Value;
			if (LArray == null && LCount != 0) { LArray = new uint[] { (uint)LCount }; LCount = LCount < 0 ? -1 : 1; }
			uint[] OArray = new uint[System.Math.Max(LCount, RCount)];
			while (OCount < LCount || OCount < RCount) {
				OArray[OCount] = (OCount < LCount ? LArray[OCount] : 0u) ^ (OCount < RCount ? RArray[OCount] : 0u);
				OCount++;
			}
			while (OCount > 0 && OArray[OCount - 1] == 0) { OCount--; }
			return new BugInt(OArray, OCount);
		}
		#endregion
		#region #operator# + (#struct # L, #struct # R) 
		public static BugInt operator +(BugInt L, BugInt R) {
			var RCount = R.Count;
			if (RCount == 0) return L;
			var LCount = L.Count;
			if (LCount == 0) return R;
			var RMinus = false;
			if (RCount < 0) { RMinus = true; RCount = -RCount; }
			var RArray = R.Value;
			if (RArray == null && RCount != 0) { RArray = new uint[] { (uint)RCount }; RCount = 1; }
			var LMinus = false;
			if (LCount < 0) { LMinus = true; LCount = -LCount; }
			var LArray = L.Value;
			if (LArray == null && LCount != 0) { LArray = new uint[] { (uint)LCount }; LCount = 1; }
			var OCount = LCount;
			var OMinus = LMinus;
			uint[] OArray = LArray;
			uint A = 0;
			if (LMinus == RMinus) {
				if (OCount < RCount) { OCount = RCount; }
				OArray = new uint[OCount + 1];
				OCount = 0;
				while (OCount < LCount || OCount < RCount) {
					long aa = 0;
					if (OCount < LCount) aa = LArray[OCount];
					if (OCount < RCount) aa += RArray[OCount];
					aa += A;
					A = (uint)aa;
					OArray[OCount++] = A;
					A = (uint)(aa >> 32);
				}
				if (A != 0u) OArray[OCount++] = A;
			} else {
				var LBelow = false;
				if (LCount < RCount) { LBelow = true; } else if (LCount == RCount) {
					while (--LCount >= 0) {
						var LValue = LArray[LCount]; var RValue = RArray[LCount];
						if (LValue != RValue) { if (LValue < RValue) LBelow = true; break; }
					}
					LCount = RCount;
				}
				if (LBelow) {
					OArray = new uint[RCount];
					OCount = 0;
					while (OCount < RCount) {
						long aa = 0;
						if (OCount < RCount) aa = RArray[OCount];
						if (OCount < LCount) aa -= LArray[OCount];
						aa -= A;
						A = (uint)aa;
						OArray[OCount++] = A;
						A = (uint)(-(int)(aa >> 32));
					}
					OMinus = !OMinus;
				} else {
					OArray = new uint[LCount];
					OCount = 0;
					while (OCount < LCount) {
						long aa = 0;
						if (OCount < LCount) aa = LArray[OCount];
						if (OCount < RCount) aa -= RArray[OCount];
						aa -= A;
						A = (uint)aa;
						OArray[OCount++] = A;
						A = (uint)(-(int)(aa >> 32));
					}
				}
			}
			while (OCount > 0 && OArray[OCount - 1] == 0) { OCount--; }
			if (OCount == 0) { OArray = null; } else if (OMinus) OCount = -OCount;
			return new BugInt(OArray, OCount);
		}
		#endregion
		#region #operator# + (#struct # L, #int # R) 
		public static BugInt operator +(BugInt L, int R) {
			if (R == 0) return L;
			var LCount = L.Count;
			if (LCount == 0) return R;
			var RMinus = false;
			if (R < 0) { RMinus = true; R = -R; }
			var LMinus = false;
			if (LCount < 0) { LMinus = true; LCount = -LCount; }
			var LArray = L.Value;
			if (LArray == null && LCount != 0) { LArray = new uint[] { (uint)LCount }; LCount = 1; }
			var OCount = LCount;
			var OMinus = LMinus;
			uint[] OArray = LArray;
			uint A = 0;
			if (LMinus == RMinus) {
				OArray = new uint[OCount + 1];
				OCount = 0;
				while (OCount < LCount || OCount < 1) {
					long aa = 0;
					if (OCount < LCount) aa = LArray[OCount];
					if (OCount < 1) aa += R;
					aa += A;
					A = (uint)aa;
					OArray[OCount++] = A;
					A = (uint)(aa >> 32);
				}
				if (A != 0u) OArray[OCount++] = A;
			} else {
				if (LCount == 1 && LArray[0] < R) {
					OArray = new uint[1];
					OCount = 0;
					long aa = 0;
					if (OCount < 1) aa = R;
					if (OCount < LCount) aa -= LArray[0];
					aa -= A;
					A = (uint)aa;
					OArray[OCount++] = A;
					A = (uint)(-(int)(aa >> 32));
					OMinus = !OMinus;
				} else {
					OArray = new uint[LCount];
					OCount = 0;
					while (OCount < LCount) {
						long aa = 0;
						if (OCount < LCount) aa = LArray[OCount];
						if (OCount < 1) aa -= R;
						aa -= A;
						A = (uint)aa;
						OArray[OCount++] = A;
						A = (uint)(-(int)(aa >> 32));
					}
				}
			}
			while (OCount > 0 && OArray[OCount - 1] == 0) { OCount--; }
			if (OCount == 0) { OArray = null; } else if (OMinus) OCount = -OCount;
			return new BugInt(OArray, OCount);
		}
		#endregion
		#region #operator# - (#struct # L, #struct # R) 
		public static BugInt operator -(BugInt L, BugInt R) {
			var RCount = R.Count;
			if (RCount == 0) return L;
			var LCount = L.Count;
			if (LCount == 0) return new BugInt(R.Value, -RCount);
			var RMinus = false;
			if (RCount < 0) { RMinus = true; RCount = -RCount; }
			var RArray = R.Value;
			if (RArray == null && RCount != 0) { RArray = new uint[] { (uint)RCount }; RCount = 1; }
			var LMinus = false;
			if (LCount < 0) { LMinus = true; LCount = -LCount; }
			var LArray = L.Value;
			if (LArray == null && LCount != 0) { LArray = new uint[] { (uint)LCount }; LCount = 1; }
			var OCount = LCount;
			var OMinus = LMinus;
			uint[] OArray = LArray;
			uint A = 0u;
			if (LMinus == RMinus) {
				var LBelow = false;
				if (LCount < RCount) { LBelow = true; } else if (LCount == RCount) {
					while (--LCount >= 0) {
						var LValue = LArray[LCount]; var RValue = RArray[LCount];
						if (LValue != RValue) { if (LValue < RValue) LBelow = true; break; }
					}
					LCount = RCount;
				}
				if (LBelow) {
					OArray = new uint[RCount];
					OCount = 0;
					while (OCount < RCount) {
						long aa = 0;
						if (OCount < RCount) aa = RArray[OCount];
						if (OCount < LCount) aa -= LArray[OCount];
						aa -= A; A = (uint)aa;
						OArray[OCount++] = A;
						A = (uint)(-(int)(aa >> 32));
					}
					OMinus = !OMinus;
				} else {
					OArray = new uint[LCount];
					OCount = 0;
					while (OCount < LCount) {
						long aa = 0;
						if (OCount < LCount) aa = LArray[OCount];
						if (OCount < RCount) aa -= RArray[OCount];
						aa -= A; A = (uint)aa;
						OArray[OCount++] = A;
						A = (uint)(-(int)(aa >> 32));
					}
				}
			} else {
				OCount = RCount;
				if (RCount < LCount) { OCount = LCount; }
				OArray = new uint[OCount + 1];
				OCount = 0;
				while (OCount < LCount || OCount < RCount) {
					long aa = 0;
					if (OCount < LCount) aa = LArray[OCount];
					if (OCount < RCount) aa += RArray[OCount];
					aa += A;
					A = (uint)aa;
					OArray[OCount++] = A;
					A = (uint)(aa >> 32);
				}
				if (A != 0u) OArray[OCount++] = A;
			}
			while (OCount > 0 && OArray[OCount - 1] == 0) { OCount--; }
			if (OCount == 0) { OArray = null; } else if (OMinus) OCount = -OCount;
			return new BugInt(OArray, OCount);
		}
		#endregion
		#region #operator# - (#struct # L, #int # R) 
		public static BugInt operator -(BugInt L, int R) {
			if (R == 0) return L;
			var LCount = L.Count;
			if (LCount == 0) return new BugInt(-1);
			var RMinus = false;
			if (R < 0) { RMinus = true; R = -R; }
			var LMinus = false;
			if (LCount < 0) { LMinus = true; LCount = -LCount; }
			var LArray = L.Value;
			if (LArray == null && LCount != 0) { LArray = new uint[] { (uint)LCount }; LCount = 1; }
			var OCount = LCount;
			var OMinus = LMinus;
			uint[] OArray = LArray;
			uint A = 0u;
			if (LMinus == RMinus) {
				if (LCount == 1 && LArray[0] < R) {
					OArray = new uint[1];
					OCount = 0;
					long aa = 0;
					if (OCount < 1) aa = R;
					if (OCount < LCount) aa -= LArray[OCount];
					aa -= A;
					A = (uint)aa;
					OArray[OCount++] = A;
					A = (uint)(-(int)(aa >> 32));
					OMinus = !OMinus;
				} else {
					OArray = new uint[LCount];
					OCount = 0;
					while (OCount < LCount) {
						long aa = 0;
						if (OCount < LCount) aa = LArray[OCount];
						if (OCount < 1) aa -= R;
						aa -= A;
						A = (uint)aa;
						OArray[OCount++] = A;
						A = (uint)(-(int)(aa >> 32));
					}
				}
			} else {
				OCount = 1;
				if (OCount < LCount) { OCount = LCount; }
				OArray = new uint[OCount + 1];
				OCount = 0;
				while (OCount < LCount) {
					long aa = 0;
					if (OCount < LCount) aa = LArray[OCount];
					if (OCount < R) aa += R;
					aa += A;
					A = (uint)aa;
					OArray[OCount++] = A;
					A = (uint)(aa >> 32);
				}
				if (A != 0u) OArray[OCount++] = A;
			}
			while (OCount > 0 && OArray[OCount - 1] == 0) { OCount--; }
			if (OCount == 0) { OArray = null; } else if (OMinus) OCount = -OCount;
			return new BugInt(OArray, OCount);
		}
		#endregion
		#region #operator# - (#struct # L) 
		/// <summary>Оператор возвращает отридцательное значение в любом случае)</summary>
		public static BugInt operator -(BugInt L) {
			var Count = L.Count;
			if (Count >= 0) return new BugInt(L.Value, -L.Count);
			return L;
		}
		#endregion
		#region #operator# ! (#struct # L) 
		/// <summary>Оператор возвращает значение с инвертированным знаком числа)</summary>
		public static BugInt operator !(BugInt L) {
			return new BugInt(L.Value, -L.Count);
		}
		#endregion
		#region #operator# + (#struct # L) 
		/// <summary>Оператор возвращает положительное значение в любом случае)</summary>
		public static BugInt operator +(BugInt L) {
			var Count = L.Count;
			if (Count < 0) return new BugInt(L.Value, -Count);
			return L;
		}
		#endregion
		#region #operator# ~ (#struct # L) 
		public static BugInt operator ~(BugInt L) {
			L += 1;
			return new BugInt(L.Value, -L.Count);
		}
		#endregion
		#region #operator# -- (#struct # L) 
		public static BugInt operator --(BugInt L) {
			return L - 1;
		}
		#endregion
		#region #operator# ++ (#struct # L) 
		public static BugInt operator ++(BugInt L) {
			return L + 1;
		}
		#endregion
		#region #method# Max(L, R) 
		public static BugInt Max(BugInt L, BugInt R) {
			var LCount = L.Count;
			var RCount = R.Count;
			if (LCount > RCount) return L;
			if (LCount < RCount) return R;
			if (LCount < 0) LCount = -LCount;
			var LArray = L.Value;
			if (LArray == null && LCount != 0) { LArray = new uint[] { (uint)LCount }; LCount = 1; }
			var RArray = R.Value;
			if (RArray == null && RCount != 0) { RArray = new uint[] { (uint)RCount }; RCount = 1; }
			if (LArray != RArray)
				while (--LCount >= 0) {
					var LValue = LArray[LCount];
					var RValue = RArray[LCount];
					if (LValue > RValue) return L;
					if (LValue < RValue) return R;
				}
			return R;
		}
		#endregion
		#region #method# Min(L, R) 
		public static BugInt Min(BugInt L, BugInt R) {
			var LCount = L.Count;
			var RCount = R.Count;
			if (LCount > RCount) return R;
			if (LCount < RCount) return L;
			if (LCount < 0) LCount = -LCount;
			var LArray = L.Value;
			if (LArray == null && LCount != 0) { LArray = new uint[] { (uint)LCount }; LCount = 1; }
			var RArray = R.Value;
			if (RArray == null && RCount != 0) { RArray = new uint[] { (uint)RCount }; RCount = 1; }
			if (LArray != RArray)
				while (--LCount >= 0) {
					var LValue = LArray[LCount];
					var RValue = RArray[LCount];
					if (LValue > RValue) return R;
					if (LValue < RValue) return L;
				}
			return L;
		}
		#endregion
		#region #method# Mul(L, R) 
		/// <summary>Умножение двух чисел, результат на фиксированной длине собственного массива)</summary>
		private void Mul(BugInt L, BugInt R) {
			var RCount = R.Count;
			var RMinus = false;
			if (RCount < 0) { RMinus = true; RCount = -RCount; }
			var RArray = R.Value;
			if (RArray == null && RCount != 0) { RArray = new uint[] { (uint)RCount }; RCount = 1; }
			var LCount = L.Count;
			var LMinus = false;
			if (LCount < 0) { LMinus = true; LCount = -LCount; }
			var LArray = L.Value;
			if (LArray == null && LCount != 0) { LArray = new uint[] { (uint)LCount }; LCount = 1; }
			uint[] OArray = this.Value;
			var OCount = this.Count;
			var OMinus = false; if (OCount < 0) { OMinus = true; OCount = -OCount; }
			if (RCount == 1 && RArray[0] == 1) { this.Count = LCount; while (--LCount >= 0) { OArray[LCount] = LArray[LCount]; } return; }
			if (RCount == 0) { this.Count = 0; return; }
			if (LCount == 1 && LArray[0] == 1) { this.Count = RCount; while (--RCount >= 0) { OArray[RCount] = RArray[RCount]; } return; }
			if (LCount == 0) { this.Count = 0; return; }
			while (OCount > 0) OArray[--OCount] = 0;
			OCount = 0;
			OMinus = false;
			if (LCount < RCount) {
				OCount = RCount; RCount = LCount; LCount = OCount;
				OMinus = RMinus; RMinus = LMinus; LMinus = OMinus;
				OArray = RArray; RArray = LArray; LArray = OArray;
				OArray = null;
				OMinus = false;
				OCount = 0;
			}
			var LIndex = 0;
			ulong Muled;
			uint Above;
			uint LValue;
			if (RCount == 1) {
				LValue = RArray[0];
				Muled = ((ulong)LArray[LIndex++]) * LValue;
				OCount = 0;
				OArray[OCount++] = (uint)Muled;
				Above = (uint)(Muled >> 32);
				while (LIndex < LCount) {
					Muled = ((ulong)LArray[LIndex++]) * LValue + Above;
					OArray[OCount++] = (uint)(Muled);
					Above = (uint)(Muled >> 32);
				}
				OArray[OCount++] = Above;
			} else {
				while (LIndex < LCount) {
					var OIndex = LIndex;
					var RIndex = 0;
					LValue = LArray[LIndex++];
					Muled = ((ulong)RArray[RIndex++]) * LValue + OArray[OIndex];
					OArray[OIndex++] = (uint)Muled;
					Above = (uint)(Muled >> 32);
					while (RIndex < RCount) {
						Muled = ((ulong)RArray[RIndex++]) * LValue + OArray[OIndex] + Above;
						OArray[OIndex++] = (uint)(Muled);
						Above = (uint)(Muled >> 32);
					}
					while (Above != 0) {
						Muled = ((ulong)Above) + OArray[OIndex];
						OArray[OIndex++] = (uint)(Muled);
						Above = (uint)(Muled >> 32);
					}
					if (OIndex > OCount) OCount = OIndex;
				}
			}
			while (OCount > 0 && OArray[OCount - 1] == 0) { OCount--; }
			if (OCount == 0) { OArray = null; } else if (LMinus) OCount = -OCount;
			this.Count = OCount;
		}
		#endregion
		#region #method# Pow(L, E) 
		public static BugInt Pow(BugInt L, int E) {
			#region #debug# 
#if DEBUG
			if (E < 0) throw new System.ArgumentOutOfRangeException();
#endif
			#endregion
			if (E == 0) return 1;
			if (E == 1) return L;
			if (L == 0) return L;
			if (L == 1) return L;
			if (L == -1) {
				if ((E & 1) == 0) return 1;
				return L;
			}
			bool Minus = false;
			var LCount = L.Count;
			if (LCount < 0) { Minus = true; LCount = -LCount; }
			var LArray = L.Value;
			if (LArray == null && LCount != 0) { LArray = new uint[] { (uint)LCount }; LCount = 1; }
			int cuRes1 = LCount;
			int cuRes2 = cuRes1;
			uint uHiRes1 = LArray[LCount - 1];
			uint uHiRes2 = uHiRes1 + 1;
			if (uHiRes2 == 0) {
				cuRes2++;
				uHiRes2 = 1u;
			}
			int cuRes3 = 1;
			int cuRes4 = 1;
			uint uHiRes3 = 1u;
			uint uHiRes4 = 1u;
			ulong uu;
			uint hi;
			int ee = E;
			while (true) {
				if ((ee & 1) != 0) {
					if ((hi = (uint)((uu = ((ulong)uHiRes4) * uHiRes2) >> 32)) != 0) { if (((uint)uu) != 0 && ++hi == 0) { hi = 1u; cuRes4 += cuRes2 + 1; } else { cuRes4 += cuRes2; } uHiRes4 = hi; } else { uHiRes4 = (uint)uu; cuRes4 += cuRes2 - 1; }
					if ((hi = (uint)((uu = ((ulong)uHiRes3) * uHiRes1) >> 32)) != 0) { uHiRes3 = hi; cuRes3 += cuRes1; } else { uHiRes3 = (uint)uu; cuRes3 += cuRes1 - 1; }
				}
				if ((ee >>= 1) == 0) break;
				if ((hi = (uint)((uu = ((ulong)uHiRes2) * uHiRes2) >> 32)) != 0) { if (((uint)uu) != 0 && ++hi == 0) { hi = 1u; cuRes2 += cuRes2 + 1; } else { cuRes2 += cuRes2; } uHiRes2 = hi; } else { uHiRes2 = (uint)uu; cuRes2 += cuRes2 - 1; }
				if ((hi = (uint)((uu = ((ulong)uHiRes1) * uHiRes1) >> 32)) != 0) { uHiRes1 = hi; cuRes1 += cuRes1; } else { uHiRes1 = (uint)uu; cuRes1 += cuRes1 - 1; }
			}
			cuRes4++;
			var OArray = new uint[cuRes4];
			var OCount = 0;
			while (OCount < LCount) { OArray[OCount] = LArray[OCount]; OCount++; }
			var r = new BugInt() { Value = OArray, Count = LCount };
			BugInt b = new BugInt() { Value = new uint[cuRes4], Count = 0 };
			var AArray = new uint[cuRes4]; AArray[0] = 1u;
			BugInt a = new BugInt() { Value = AArray, Count = 1 };
			BugInt t;
			if ((E & 1) == 0) Minus = false;
			ee = E;
			while (true) {
				if ((ee & 1) != 0) { t = a; a = b; b = t; a.Mul(r, b); }
				if ((ee >>= 1) == 0) break; t = r; r = b; b = t; r.Mul(b, b);
			}
			return new BugInt(a.Value, Minus ? -a.Count : a.Count);
		}
		#endregion
		#region #struct # #implicit operator # (#uint # V) 
		public static implicit operator BugInt(uint V) { return new BugInt(V); }
		#endregion
		#region #struct # #implicit operator # (#int # V) 
		public static implicit operator BugInt(int V) { return new BugInt(V); }
		#endregion
		#region #struct # #implicit operator # (#ulong # V) 
		public static implicit operator BugInt(ulong V) { return new BugInt(V); }
		#endregion
		#region #struct # #implicit operator # (#long # V) 
		public static implicit operator BugInt(long V) { return new BugInt(V); }
		#endregion
		#region #int # #explicit operator # (#struct # V)
		public static explicit operator int(BugInt V) {
			var C = V.Count;
			var A = V.Value;
			if (A == null) return C;
			if (C < 0) return -((int)A[0] & int.MaxValue);
			if (C != 0) return ((int)A[0] & int.MaxValue);
			return 0;
		}
		#endregion
		#region #uint # #explicit operator # (#struct # V)
		public static explicit operator uint(BugInt V) {
			var C = V.Count;
			var A = V.Value;
			if (A == null) return (uint)C;
			if (C != 0) return A[0];
			return 0;
		}
		#endregion
		#region #long # #explicit operator # (#struct # V)
		public static explicit operator long(BugInt V) {
			var C = V.Count;
			var A = V.Value;
			if (A == null) return C;
			var R = 0ul;
			if (C < 0) {
				if (C < -1) { R |= A[1]; R <<= 32; }
				R |= A[0];
				return -((long)R & long.MinValue);
			}
			if (C > 1) { R |= A[1]; R <<= 32; }
			if (C > 0) R |= A[0];
			return ((long)R & long.MinValue);
		}
		#endregion
		#region #ulong # #explicit operator # (#struct # V)
		public static explicit operator ulong(BugInt V) {
			var C = V.Count;
			var A = V.Value;
			if (A == null) return (uint)C;
			if (C < 0) C = -C;
			var R = 0ul;
			if (C > 1) { R |= A[1]; R <<= 32; }
			if (C > 0) R |= A[0];
			return R;
		}
		#endregion
		#region #uint # #get#[#int # I] 
		/// <summary>Возвращвет элемент числа, значение по указанному индексу)</summary>
		/// <param name="I">Индекс значения)</param>
		/// <returns>32 битное число без знака)</returns>
		public uint this[int I] {
			get {
				var C = this.Count;
				if (C < 0) C = -C;
				var V = this.Value;
				if (V == null) { if (I == 0) return (uint)C; } else {
					if (C > I && I >= 0) return V[I];
				}
				return 0;
			}
		}
		#endregion
		#region #uint#[] #get# Array 
		/// <summary>Возвращает копию массива значения)</summary>
		public uint[] Array {
			get {
				var C = this.Count;
				if (C < 0) C = -C;
				var V = this.Value;
				if (V == null) return new uint[] { (uint)C };
				var A = new uint[C];
				System.Array.Copy(V, A, C);
				return A;
			}
		}
		#endregion
		#region #bool# #get# Sign 
		/// <summary>Возвращает знак числа, положительный больше или равен нулю)</summary>
		public bool Sign => this.Count < 0;
		#endregion
		#region #method# Bit(Count) 
		public static BugInt Bit(int Count) {
			var Minus = false;
			uint[] Array = null;
			if (Count < 0) { Count = -Count; Minus = true; }
			if (Count > 0) {
				var Len = Count / 32;
				var Bit = Count - Len * 32;
				Count = Len;
				var Las = 0u;
				if (Bit > 0) {
					Las = uint.MaxValue >> 32 - Bit;
					Count++;
				}
				Array = new uint[Count];
				Bit = Count - 1;
				if (Las > 0) Array[Bit--] = Las;
				while (Bit >= 0) Array[Bit--] = uint.MaxValue;
			}
			if (Minus) Count = -Count;
			return new BugInt(Array, Count);
		}
		#endregion
		#region #method# Gcd(L, R) 
		public static BugInt Gcd(BugInt L, BugInt R) {
			var M = false;
			if (L < 0) { L = +L; M = true; }
			if (R < 0) { R = +R; M = true; }
			while (R != 0) { var X = L % R; L = R; R = X; }
			if (M) L = -L;
			return L;
		}
		#endregion
	}
}