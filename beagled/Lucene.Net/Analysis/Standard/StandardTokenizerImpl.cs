﻿using System;
using Token = Lucene.Net.Analysis.Token;
namespace Lucene.Net.Analysis.Standard
{
	class StandardTokenizerImpl 
	{

		/** This character denotes the end of file */
		public static final int YYEOF = -1;

		/** initial size of the lookahead buffer */
		private static final int ZZ_BUFFERSIZE = 16384;

		/** lexical states */
		public static final int YYINITIAL = 0;

		/**
		 * ZZ_LEXSTATE[l] is the state in the DFA for the lexical state l
		 * ZZ_LEXSTATE[l+1] is the state in the DFA for the lexical state l
		 *                  at the beginning of a line
		 * l is of the form l = 2*k, k a non negative integer
		 */
		private static final int ZZ_LEXSTATE[] = 
			{ 
				0, 0
			};

		/** 
		 * Translates characters to character classes
		 */
		private static final String ZZ_CMAP_PACKED = 
			"\11\0\1\0\1\4\1\0\1\0\1\3\22\0\1\0\17\0\12\1"+
			"\7\0\32\1\6\0\32\1\57\0\1\1\12\0\1\1\4\0\1\1"+
			"\5\0\27\1\1\0\37\1\1\0\u013f\1\31\0\162\1\4\0\14\1"+
			"\16\0\5\1\11\0\1\1\213\0\1\1\13\0\1\1\1\0\3\1"+
			"\1\0\1\1\1\0\24\1\1\0\54\1\1\0\46\1\1\0\5\1"+
			"\4\0\202\1\10\0\105\1\1\0\46\1\2\0\2\1\6\0\20\1"+
			"\41\0\46\1\2\0\1\1\7\0\47\1\110\0\33\1\5\0\3\1"+
			"\56\0\32\1\5\0\13\1\25\0\12\1\4\0\2\1\1\0\143\1"+
			"\1\0\1\1\17\0\2\1\7\0\2\1\12\1\3\1\2\0\1\1"+
			"\20\0\1\1\1\0\36\1\35\0\3\1\60\0\46\1\13\0\1\1"+
			"\u0152\0\66\1\3\0\1\1\22\0\1\1\7\0\12\1\4\0\12\1"+
			"\25\0\10\1\2\0\2\1\2\0\26\1\1\0\7\1\1\0\1\1"+
			"\3\0\4\1\3\0\1\1\36\0\2\1\1\0\3\1\4\0\12\1"+
			"\2\1\23\0\6\1\4\0\2\1\2\0\26\1\1\0\7\1\1\0"+
			"\2\1\1\0\2\1\1\0\2\1\37\0\4\1\1\0\1\1\7\0"+
			"\12\1\2\0\3\1\20\0\11\1\1\0\3\1\1\0\26\1\1\0"+
			"\7\1\1\0\2\1\1\0\5\1\3\0\1\1\22\0\1\1\17\0"+
			"\2\1\4\0\12\1\25\0\10\1\2\0\2\1\2\0\26\1\1\0"+
			"\7\1\1\0\2\1\1\0\5\1\3\0\1\1\36\0\2\1\1\0"+
			"\3\1\4\0\12\1\1\0\1\1\21\0\1\1\1\0\6\1\3\0"+
			"\3\1\1\0\4\1\3\0\2\1\1\0\1\1\1\0\2\1\3\0"+
			"\2\1\3\0\3\1\3\0\10\1\1\0\3\1\55\0\11\1\25\0"+
			"\10\1\1\0\3\1\1\0\27\1\1\0\12\1\1\0\5\1\46\0"+
			"\2\1\4\0\12\1\25\0\10\1\1\0\3\1\1\0\27\1\1\0"+
			"\12\1\1\0\5\1\3\0\1\1\40\0\1\1\1\0\2\1\4\0"+
			"\12\1\25\0\10\1\1\0\3\1\1\0\27\1\1\0\20\1\46\0"+
			"\2\1\4\0\12\1\25\0\22\1\3\0\30\1\1\0\11\1\1\0"+
			"\1\1\2\0\7\1\71\0\1\1\60\1\1\1\2\1\14\1\7\1"+
			"\11\1\12\1\47\0\2\1\1\0\1\1\2\0\2\1\1\0\1\1"+
			"\2\0\1\1\6\0\4\1\1\0\7\1\1\0\3\1\1\0\1\1"+
			"\1\0\1\1\2\0\2\1\1\0\4\1\1\0\2\1\11\0\1\1"+
			"\2\0\5\1\1\0\1\1\11\0\12\1\2\0\2\1\42\0\1\1"+
			"\37\0\12\1\26\0\10\1\1\0\42\1\35\0\4\1\164\0\42\1"+
			"\1\0\5\1\1\0\2\1\25\0\12\1\6\0\6\1\112\0\46\1"+
			"\12\0\51\1\7\0\132\1\5\0\104\1\5\0\122\1\6\0\7\1"+
			"\1\0\77\1\1\0\1\1\1\0\4\1\2\0\7\1\1\0\1\1"+
			"\1\0\4\1\2\0\47\1\1\0\1\1\1\0\4\1\2\0\37\1"+
			"\1\0\1\1\1\0\4\1\2\0\7\1\1\0\1\1\1\0\4\1"+
			"\2\0\7\1\1\0\7\1\1\0\27\1\1\0\37\1\1\0\1\1"+
			"\1\0\4\1\2\0\7\1\1\0\47\1\1\0\23\1\16\0\11\1"+
			"\56\0\125\1\14\0\u026c\1\2\0\10\1\12\0\32\1\5\0\113\1"+
			"\25\0\15\1\1\0\4\1\16\0\22\1\16\0\22\1\16\0\15\1"+
			"\1\0\3\1\17\0\64\1\43\0\1\1\4\0\1\1\3\0\12\1"+
			"\46\0\12\1\6\0\130\1\10\0\51\1\127\0\35\1\51\0\12\1"+
			"\36\1\2\0\5\1\u038b\0\154\1\224\0\234\1\4\0\132\1\6\0"+
			"\26\1\2\0\6\1\2\0\46\1\2\0\6\1\2\0\10\1\1\0"+
			"\1\1\1\0\1\1\1\0\1\1\1\0\37\1\2\0\65\1\1\0"+
			"\7\1\1\0\1\1\3\0\3\1\1\0\7\1\3\0\4\1\2\0"+
			"\6\1\4\0\15\1\5\0\3\1\1\0\7\1\164\0\1\1\15\0"+
			"\1\1\202\0\1\1\4\0\1\1\2\0\12\1\1\0\1\1\3\0"+
			"\5\1\6\0\1\1\1\0\1\1\1\0\1\1\1\0\4\1\1\0"+
			"\3\1\1\0\7\1\3\0\3\1\5\0\5\1\u0ebb\0\2\1\52\0"+
			"\5\1\5\0\2\1\3\0\1\2\126\2\6\2\3\2\1\2\132\2"+
			"\1\2\4\2\5\2\50\2\3\2\1\0\136\1\21\0\30\1\70\0"+
			"\20\2\u0100\0\200\2\200\0\u19b6\2\12\2\100\0\u51a6\2\132\2\u048d\1"+
			"\u0773\0\u2ba4\1\u215c\0\u012e\2\2\2\73\2\225\2\7\1\14\0\5\1"+
			"\5\0\1\1\1\0\12\1\1\0\15\1\1\0\5\1\1\0\1\1"+
			"\1\0\2\1\1\0\2\1\1\0\154\1\41\0\u016b\1\22\0\100\1"+
			"\2\0\66\1\50\0\14\1\164\0\5\1\1\0\207\1\23\0\12\1"+
			"\7\0\32\1\6\0\32\1\12\0\1\2\72\2\37\1\3\0\6\1"+
			"\2\0\6\1\2\0\6\1\2\0\3\1\43\0";

		/** 
		 * Translates characters to character classes
		 */
		private static final char [] ZZ_CMAP = zzUnpackCMap(ZZ_CMAP_PACKED);

		/** 
		 * Translates DFA states to action switch labels.
		 */
		private static final int [] ZZ_ACTION = zzUnpackAction();

		private static final String ZZ_ACTION_PACKED_0 =
			"\1\0\1\1\1\2\1\3\1\1";

		private static int [] zzUnpackAction() 
		{
			int [] result = new int[5];
			int offset = 0;
			offset = zzUnpackAction(ZZ_ACTION_PACKED_0, offset, result);
			return result;
		}

		private static int zzUnpackAction(String packed, int offset, int [] result) 
		{
			int i = 0;       /* index in packed string  */
			int j = offset;  /* index in unpacked array */
			int l = packed.length();
			while (i < l) 
				{
					int count = packed.charAt(i++);
					int value = packed.charAt(i++);
					do result[j++] = value; while (--count > 0);
				}
			return j;
		}


		/** 
		 * Translates a state to a row index in the transition table
		 */
		private static final int [] ZZ_ROWMAP = zzUnpackRowMap();

		private static final String ZZ_ROWMAP_PACKED_0 =
			"\0\0\0\5\0\12\0\5\0\17";

		private static int [] zzUnpackRowMap() 
		{
			int [] result = new int[5];
			int offset = 0;
			offset = zzUnpackRowMap(ZZ_ROWMAP_PACKED_0, offset, result);
			return result;
		}

		private static int zzUnpackRowMap(String packed, int offset, int [] result) 
		{
			int i = 0;  /* index in packed string  */
			int j = offset;  /* index in unpacked array */
			int l = packed.length();
			while (i < l) 
				{
					int high = packed.charAt(i++) << 16;
					result[j++] = high | packed.charAt(i++);
				}
			return j;
		}

		/** 
		 * The transition table of the DFA
		 */
		private static final int [] ZZ_TRANS = zzUnpackTrans();

		private static final String ZZ_TRANS_PACKED_0 =
			"\1\2\1\3\1\4\1\5\1\2\6\0\1\3\7\0"+
			"\1\2";

		private static int [] zzUnpackTrans() 
		{
			int [] result = new int[20];
			int offset = 0;
			offset = zzUnpackTrans(ZZ_TRANS_PACKED_0, offset, result);
			return result;
		}

		private static int zzUnpackTrans(String packed, int offset, int [] result) 
		{
			int i = 0;       /* index in packed string  */
			int j = offset;  /* index in unpacked array */
			int l = packed.length();
			while (i < l) 
				{
					int count = packed.charAt(i++);
					int value = packed.charAt(i++);
					value--;
					do result[j++] = value; while (--count > 0);
				}
			return j;
		}


		/* error codes */
		private static final int ZZ_UNKNOWN_ERROR = 0;
		private static final int ZZ_NO_MATCH = 1;
		private static final int ZZ_PUSHBACK_2BIG = 2;

		/* error messages for the codes above */
		private static final String ZZ_ERROR_MSG[] = 
			{
				"Unkown internal scanner error",
				"Error: could not match input",
				"Error: pushback value was too large"
			};

		/**
		 * ZZ_ATTRIBUTE[aState] contains the attributes of state <code>aState</code>
		 */
		private static final int [] ZZ_ATTRIBUTE = zzUnpackAttribute();

		private static final String ZZ_ATTRIBUTE_PACKED_0 =
			"\1\0\1\11\1\1\1\11\1\1";

		private static int [] zzUnpackAttribute() 
		{
			int [] result = new int[5];
			int offset = 0;
			offset = zzUnpackAttribute(ZZ_ATTRIBUTE_PACKED_0, offset, result);
			return result;
		}

		private static int zzUnpackAttribute(String packed, int offset, int [] result) 
		{
			int i = 0;       /* index in packed string  */
			int j = offset;  /* index in unpacked array */
			int l = packed.length();
			while (i < l) 
				{
					int count = packed.charAt(i++);
					int value = packed.charAt(i++);
					do result[j++] = value; while (--count > 0);
				}
			return j;
		}

		/** the input device */
		private java.io.Reader zzReader;

		/** the current state of the DFA */
		private int zzState;

		/** the current lexical state */
		private int zzLexicalState = YYINITIAL;

		/** this buffer contains the current text to be matched and is
		    the source of the yytext() string */
		private char zzBuffer[] = new char[ZZ_BUFFERSIZE];

		/** the textposition at the last accepting state */
		private int zzMarkedPos;

		/** the current text position in the buffer */
		private int zzCurrentPos;

		/** startRead marks the beginning of the yytext() string in the buffer */
		private int zzStartRead;

		/** endRead marks the last character in the buffer, that has been read
		    from input */
		private int zzEndRead;

		/** number of newlines encountered up to the start of the matched text */
		private int yyline;

		/** the number of characters up to the start of the matched text */
		private int yychar;

		/**
		 * the number of characters from the last newline up to the start of the 
		 * matched text
		 */
		private int yycolumn;

		/** 
		 * zzAtBOL == true <=> the scanner is currently at the beginning of a line
		 */
		private boolean zzAtBOL = true;

		/** zzAtEOF == true <=> the scanner is at the EOF */
		private boolean zzAtEOF;

		/** denotes if the user-EOF-code has already been executed */
		private boolean zzEOFDone;

		/* user code: */

		public static final int ALPHANUM          = StandardTokenizer.ALPHANUM;
		public static final int APOSTROPHE        = StandardTokenizer.APOSTROPHE;
		public static final int ACRONYM           = StandardTokenizer.ACRONYM;
		public static final int COMPANY           = StandardTokenizer.COMPANY;
		public static final int EMAIL             = StandardTokenizer.EMAIL;
		public static final int HOST              = StandardTokenizer.HOST;
		public static final int NUM               = StandardTokenizer.NUM;
		public static final int CJ                = StandardTokenizer.CJ;
		/**
		 * @deprecated this solves a bug where HOSTs that end with '.' are identified
		 *             as ACRONYMs. It is deprecated and will be removed in the next
		 *             release.
		 */
		public static final int ACRONYM_DEP       = StandardTokenizer.ACRONYM_DEP;

		public static final String [] TOKEN_TYPES = StandardTokenizer.TOKEN_TYPES;

		public final int yychar()
	
		{
			return yychar;
		}

		/**
		 * Fills Lucene token with the current token text.
		 */
		final void getText(Token t) 
		{
			t.setTermBuffer(zzBuffer, zzStartRead, zzMarkedPos-zzStartRead);
		}

		/**
		 * Fills TermAttribute with the current token text.
		 */
		final void getText(TermAttribute t) 
		{
			t.setTermBuffer(zzBuffer, zzStartRead, zzMarkedPos-zzStartRead);
		}



		/**
		 * Creates a new scanner
		 * There is also a java.io.InputStream version of this constructor.
		 *
		 * @param   in  the java.io.Reader to read input from.
		 */
		StandardTokenizerImpl(java.io.Reader in) 
		{
			this.zzReader = in;
		}

		/**
		 * Creates a new scanner.
		 * There is also java.io.Reader version of this constructor.
		 *
		 * @param   in  the java.io.Inputstream to read input from.
		 */
		StandardTokenizerImpl(java.io.InputStream in) 
		{
			this(new java.io.InputStreamReader(in));
		}

		/** 
		 * Unpacks the compressed character translation table.
		 *
		 * @param packed   the packed character translation table
		 * @return         the unpacked character translation table
		 */
		private static char [] zzUnpackCMap(String packed) 
		{
			char [] map = new char[0x10000];
			int i = 0;  /* index in packed string  */
			int j = 0;  /* index in unpacked array */
			while (i < 1214) 
				{
					int  count = packed.charAt(i++);
					char value = packed.charAt(i++);
					do map[j++] = value; while (--count > 0);
				}
			return map;
		}


		/**
		 * Refills the input buffer.
		 *
		 * @return      <code>false</code>, iff there was new input.
		 * 
		 * @exception   java.io.IOException  if any I/O-Error occurs
		 */
		private boolean zzRefill() throws java.io.IOException 
		{

			/* first: make room (if you can) */
			if (zzStartRead > 0) 
				{
					System.arraycopy(zzBuffer, zzStartRead,
							 zzBuffer, 0,
							 zzEndRead-zzStartRead);

					/* translate stored positions */
					zzEndRead-= zzStartRead;
					zzCurrentPos-= zzStartRead;
					zzMarkedPos-= zzStartRead;
					zzStartRead = 0;
				}

			/* is the buffer big enough? */
			if (zzCurrentPos >= zzBuffer.length) 
				{
					/* if not: blow it up */
					char newBuffer[] = new char[zzCurrentPos*2];
					System.arraycopy(zzBuffer, 0, newBuffer, 0, zzBuffer.length);
					zzBuffer = newBuffer;
				}

			/* finally: fill the buffer with new input */
			int numRead = zzReader.read(zzBuffer, zzEndRead,
						    zzBuffer.length-zzEndRead);

			if (numRead > 0) 
				{
					zzEndRead+= numRead;
					return false;
				}
			// unlikely but not impossible: read 0 characters, but not at end of stream    
			if (numRead == 0) 
				{
					int c = zzReader.read();
					if (c == -1) 
						{
							return true;
						} else 
						{
							zzBuffer[zzEndRead++] = (char) c;
							return false;
						}     
				}

			// numRead < 0
			return true;
		}

    
		/**
		 * Closes the input stream.
		 */
		public final void yyclose() throws java.io.IOException 
		{
			zzAtEOF = true;            /* indicate end of file */
			zzEndRead = zzStartRead;  /* invalidate buffer    */

			if (zzReader != null)
				zzReader.close();
		}


		/**
		 * Resets the scanner to read from a new input stream.
		 * Does not close the old reader.
		 *
		 * All internal variables are reset, the old input stream 
		 * <b>cannot</b> be reused (internal buffer is discarded and lost).
		 * Lexical state is set to <tt>ZZ_INITIAL</tt>.
		 *
		 * @param reader   the new input stream 
		 */
		public final void yyreset(java.io.Reader reader) 
		{
			zzReader = reader;
			zzAtBOL  = true;
			zzAtEOF  = false;
			zzEOFDone = false;
			zzEndRead = zzStartRead = 0;
			zzCurrentPos = zzMarkedPos = 0;
			yyline = yychar = yycolumn = 0;
			zzLexicalState = YYINITIAL;
		}


		/**
		 * Returns the current lexical state.
		 */
		public final int yystate() 
		{
			return zzLexicalState;
		}


		/**
		 * Enters a new lexical state
		 *
		 * @param newState the new lexical state
		 */
		public final void yybegin(int newState) 
		{
			zzLexicalState = newState;
		}


		/**
		 * Returns the text matched by the current regular expression.
		 */
		public final String yytext() 
		{
			return new String( zzBuffer, zzStartRead, zzMarkedPos-zzStartRead );
		}


		/**
		 * Returns the character at position <tt>pos</tt> from the 
		 * matched text. 
		 * 
		 * It is equivalent to yytext().charAt(pos), but faster
		 *
		 * @param pos the position of the character to fetch. 
		 *            A value from 0 to yylength()-1.
		 *
		 * @return the character at position pos
		 */
		public final char yycharat(int pos) 
		{
			return zzBuffer[zzStartRead+pos];
		}


		/**
		 * Returns the length of the matched text region.
		 */
		public final int yylength() 
		{
			return zzMarkedPos-zzStartRead;
		}


		/**
		 * Reports an error that occured while scanning.
		 *
		 * In a wellformed scanner (no or only correct usage of 
		 * yypushback(int) and a match-all fallback rule) this method 
		 * will only be called with things that "Can't Possibly Happen".
		 * If this method is called, something is seriously wrong
		 * (e.g. a JFlex bug producing a faulty scanner etc.).
		 *
		 * Usual syntax/scanner level error handling should be done
		 * in error fallback rules.
		 *
		 * @param   errorCode  the code of the errormessage to display
		 */
		private void zzScanError(int errorCode) 
		{
			String message;
			try 
				{
					message = ZZ_ERROR_MSG[errorCode];
				}
			catch (ArrayIndexOutOfBoundsException e) 
				{
					message = ZZ_ERROR_MSG[ZZ_UNKNOWN_ERROR];
				}

			throw new Error(message);
		} 


		/**
		 * Pushes the specified amount of characters back into the input stream.
		 *
		 * They will be read again by then next call of the scanning method
		 *
		 * @param number  the number of characters to be read again.
		 *                This number must not be greater than yylength()!
		 */
		public void yypushback(int number)  
		{
			if ( number > yylength() )
				zzScanError(ZZ_PUSHBACK_2BIG);

			zzMarkedPos -= number;
		}


		/**
		 * Resumes scanning until the next regular expression is matched,
		 * the end of input is encountered or an I/O-Error occurs.
		 *
		 * @return      the next token
		 * @exception   java.io.IOException  if any I/O-Error occurs
		 */
		public int getNextToken() throws java.io.IOException 
		{
			int zzInput;
			int zzAction;

			// cached fields:
			int zzCurrentPosL;
			int zzMarkedPosL;
			int zzEndReadL = zzEndRead;
			char [] zzBufferL = zzBuffer;
			char [] zzCMapL = ZZ_CMAP;

			int [] zzTransL = ZZ_TRANS;
			int [] zzRowMapL = ZZ_ROWMAP;
			int [] zzAttrL = ZZ_ATTRIBUTE;

			while (true) 
				{
					zzMarkedPosL = zzMarkedPos;

					yychar+= zzMarkedPosL-zzStartRead;

					zzAction = -1;

					zzCurrentPosL = zzCurrentPos = zzStartRead = zzMarkedPosL;
  
					zzState = ZZ_LEXSTATE[zzLexicalState];


					zzForAction: 
					{
						while (true) 
							{
    
								if (zzCurrentPosL < zzEndReadL)
									zzInput = zzBufferL[zzCurrentPosL++];
								else if (zzAtEOF) 
									{
										zzInput = YYEOF;
										break zzForAction;
									}
								else 
									{
										// store back cached positions
										zzCurrentPos  = zzCurrentPosL;
										zzMarkedPos   = zzMarkedPosL;
										boolean eof = zzRefill();
										// get translated positions and possibly new buffer
										zzCurrentPosL  = zzCurrentPos;
										zzMarkedPosL   = zzMarkedPos;
										zzBufferL      = zzBuffer;
										zzEndReadL     = zzEndRead;
										if (eof) 
											{
												zzInput = YYEOF;
												break zzForAction;
											}
										else 
											{
												zzInput = zzBufferL[zzCurrentPosL++];
											}
									}
								int zzNext = zzTransL[ zzRowMapL[zzState] + zzCMapL[zzInput] ];
								if (zzNext == -1) break zzForAction;
								zzState = zzNext;

								int zzAttributes = zzAttrL[zzState];
								if ( (zzAttributes & 1) == 1 ) 
									{
										zzAction = zzState;
										zzMarkedPosL = zzCurrentPosL;
										if ( (zzAttributes & 8) == 8 ) break zzForAction;
									}

							}
					}

					// store back cached position
					zzMarkedPos = zzMarkedPosL;

					switch (zzAction < 0 ? zzAction : ZZ_ACTION[zzAction]) 
						{
						case 3: 
				
							{ return CJ;
							}
						case 4: break;
						case 1: 
				
							{ /* ignore */
							}
						case 5: break;
						case 2: 
				
							{ return ALPHANUM;
							}
						case 6: break;
						default: 
							if (zzInput == YYEOF && zzStartRead == zzCurrentPos) 
								{
									zzAtEOF = true;
									return YYEOF;
								} 
							else 
								{
									zzScanError(ZZ_NO_MATCH);
								}
						}
				}
		}


	}
}
