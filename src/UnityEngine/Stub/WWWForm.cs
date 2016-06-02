using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Collections;
using UnityEngineInternal;


public class WWWForm
{
/*
CSRAW private List<byte[]> formData; // <byte[]>
CSRAW private List<string> fieldNames; // <string>
CSRAW private List<string> fileNames; // <string>
CSRAW private List<string> types; // <string>
CSRAW private byte[] boundary;
CSRAW private bool containsFiles = false;

// Creates an empty WWWForm object.
CSRAW public WWWForm() {
	formData = new List<byte[]>();
	fieldNames = new List<string>();
	fileNames = new List<string>();
	types = new List<string>();

	// Generate a random boundary
	boundary=new byte[40];
	for(int i=0; i<40; i++) {
		int randomChar=Random.Range(48,110);
		if(randomChar > 57) // skip unprintable chars between 57 and 64 (inclusive)
			randomChar+=7;
		if(randomChar > 90) // and 91 and 96 (inclusive)
			randomChar+=6;
		boundary[i]=(byte)randomChar;
	}

}

// Add a simple field to the form.
CSRAW public void AddField(string fieldName, string value, Encoding e=System.Text.Encoding.UTF8) {
	fieldNames.Add(fieldName);
	fileNames.Add(null);
	formData.Add(e.GetBytes(value));
	types.Add("text/plain; charset=\"" + e.WebName + "\"");
}

// Adds a simple field to the form.
CSRAW public void AddField(string fieldName, int i) {
	AddField(fieldName, i.ToString());
}

// Add binary data to the form.
CSRAW public void AddBinaryData(string fieldName, byte[] contents, string fileName=null, string mimeType = null) {
	containsFiles=true;

	// We handle png files automatically as we suspect people will be uploading png files a lot due to the new
	// screen shot feature. If we want to add support for detecting other file types, we will need to do it in a more extensible way.
	bool isPng = contents.Length > 8 && contents[0] == 0x89 && contents[1] == 0x50 && contents[2] == 0x4e && contents[3] == 0x47
									&& contents[4] == 0x0d && contents[5] == 0x0a && contents[6] == 0x1a && contents[7] == 0x0a;
	if(fileName == null) {
		fileName = fieldName + (isPng?".png":".dat");
	}
	if(mimeType == null) {
		if(isPng)
			mimeType="image/png";
		else
			mimeType="application/octet-stream";
	}

	fieldNames.Add(fieldName);
	fileNames.Add(fileName);
	formData.Add(contents);
	types.Add(mimeType);
}

// (RO) Returns the correct request headers for posting the form using the [[WWW]] class.
CSRAW public Dictionary<string, string> headers { get {
		Dictionary<string, string> retval = new Dictionary<string, string>();
		if(containsFiles)
			retval["Content-Type"]="multipart/form-data; boundary=\"" + System.Text.Encoding.UTF8.GetString(boundary, 0, boundary.Length) + "\"";
		else
			retval["Content-Type"]="application/x-www-form-urlencoded";
		return retval;
	}
}

// (RO) The raw data to pass as the POST request body when sending the form.
CSRAW public byte[] data { get {

		if(containsFiles) {
			byte[] dDash = WWW.DefaultEncoding.GetBytes("--");
			byte[] crlf = WWW.DefaultEncoding.GetBytes("\r\n");
			byte[] contentTypeHeader = WWW.DefaultEncoding.GetBytes("Content-Type: ");
			byte[] dispositionHeader = WWW.DefaultEncoding.GetBytes("Content-disposition: form-data; name=\"");
			byte[] endQuote = WWW.DefaultEncoding.GetBytes("\"");
			byte[] fileNameField = WWW.DefaultEncoding.GetBytes("; filename=\"");


			using(MemoryStream memStream = new MemoryStream(1024))
			{
				for(int i=0; i < formData.Count; i++) {
					memStream.Write(crlf, 0, (int) crlf.Length);
					memStream.Write(dDash, 0, (int) dDash.Length);
					memStream.Write(boundary, 0, (int) boundary.Length);
					memStream.Write(crlf, 0, (int) crlf.Length);
					memStream.Write(contentTypeHeader, 0, (int) contentTypeHeader.Length);

					byte[] type=System.Text.Encoding.UTF8.GetBytes((string)types[i]);
					memStream.Write(type, 0, (int) type.Length);
					memStream.Write(crlf, 0, (int) crlf.Length);
					memStream.Write(dispositionHeader, 0, (int) dispositionHeader.Length);
					
					#if !UNITY_METRO_API && !UNITY_WP8_API
					string headerName = System.Text.Encoding.UTF8.HeaderName;
					#else
					string headerName = "";
					#endif
					// Headers must be 7 bit clean, so encode as per rfc1522 using quoted-printable if needed.
					string encodedFieldName=(string)fieldNames[i];
					if(!WWWTranscoder.SevenBitClean(encodedFieldName, System.Text.Encoding.UTF8) || encodedFieldName.IndexOf("=?") > -1) {
						encodedFieldName="=?"+headerName+"?Q?"+WWWTranscoder.QPEncode(encodedFieldName, System.Text.Encoding.UTF8) + "?=";
					}
					byte[] name=System.Text.Encoding.UTF8.GetBytes(encodedFieldName);
					memStream.Write(name, 0, (int) name.Length);
					memStream.Write(endQuote, 0, (int) endQuote.Length);

					if(fileNames[i] != null) {
						// Headers must be 7 bit clean, so encode as per rfc1522 using quoted-printable if needed.
						string encodedFileName=(string)fileNames[i];
						if(!WWWTranscoder.SevenBitClean(encodedFileName, System.Text.Encoding.UTF8) || encodedFileName.IndexOf("=?") > -1) {
							encodedFileName="=?"+headerName+"?Q?"+WWWTranscoder.QPEncode(encodedFileName, System.Text.Encoding.UTF8) + "?=";
						}
						byte[] fileName=System.Text.Encoding.UTF8.GetBytes(encodedFileName);

						memStream.Write(fileNameField, 0, (int) fileNameField.Length);
						memStream.Write(fileName, 0, (int) fileName.Length);
						memStream.Write(endQuote, 0, (int) endQuote.Length);

					}
					memStream.Write(crlf, 0, (int) crlf.Length);
					memStream.Write(crlf, 0, (int) crlf.Length);

					byte[] formBytes = (byte[])formData[i];
					memStream.Write(formBytes, 0, (int) formBytes.Length);
				}
				memStream.Write(crlf, 0, (int) crlf.Length);
				memStream.Write(dDash, 0, (int) dDash.Length);
				memStream.Write(boundary, 0, (int) boundary.Length);
				memStream.Write(dDash, 0, (int) dDash.Length);
				memStream.Write(crlf, 0, (int) crlf.Length);

				return memStream.ToArray();
			}
		}
		else {
			byte[] ampersand = WWW.DefaultEncoding.GetBytes("&");
			byte[] equal = WWW.DefaultEncoding.GetBytes("=");

			using(MemoryStream memStream = new MemoryStream(1024))
			{
				for(int i=0; i < formData.Count; i++) {
					byte[] name=WWWTranscoder.URLEncode(System.Text.Encoding.UTF8.GetBytes((string)fieldNames[i]));
					byte[] formBytes = (byte[])formData[i];
					byte[] value=WWWTranscoder.URLEncode(formBytes);

					if(i>0) memStream.Write(ampersand, 0, (int) ampersand.Length);
					memStream.Write(name, 0, (int) name.Length);
					memStream.Write(equal, 0, (int) equal.Length);
					memStream.Write(value, 0, (int) value.Length);
				}
				return memStream.ToArray();
			}
		}
	}
}


END
//*undocumented*
CLASS internal WWWTranscoder

	CSRAW private static byte [] ucHexChars = WWW.DefaultEncoding.GetBytes("0123456789ABCDEF");
	CSRAW private static byte [] lcHexChars = WWW.DefaultEncoding.GetBytes("0123456789abcdef");
	CSRAW private static byte urlEscapeChar=(byte)'%';
	CSRAW private static byte urlSpace=(byte)'+';
	CSRAW private static byte [] urlForbidden=WWW.DefaultEncoding.GetBytes("@&;:<>=?\"'/\\!#%+$,{}|^[]`");
	CSRAW private static byte qpEscapeChar=(byte)'=';
	CSRAW private static byte qpSpace=(byte)'_';
	CSRAW private static byte [] qpForbidden=WWW.DefaultEncoding.GetBytes("&;=?\"'%+_");

	CSRAW private static byte Hex2Byte (byte[] b, int offset) {
		byte result=(byte)0;

		for (int i = offset; i < offset+2; i++ ) {
			result *= 16;
			int d=b[i];

			if (d >= 48 && d <= 57) // 0 - 9
				d -= 48;
			else if (d >= 65 && d <= 75) // A -F
				d -= 55;
			else if (d >= 97 && d <= 102) // a - f
				d -= 87;
			if (d>15) {
				return 63; // ?
			}

			result += (byte)d;
		}

		return result;
	}

	CSRAW private static byte[] Byte2Hex (byte b, byte[] hexChars) {
		byte[] dest= new byte[2];
		dest[0]=hexChars[ b >> 4 ];
		dest[1]=hexChars[ b &0xf ];
		return dest;
	}


	CSRAW public static string URLEncode(string toEncode, Encoding e = Encoding.UTF8) {
		byte[] data = Encode(e.GetBytes(toEncode), urlEscapeChar, urlSpace, urlForbidden, false);
		return WWW.DefaultEncoding.GetString(data, 0, data.Length);
	}

	CSRAW public static byte[] URLEncode(byte[] toEncode) {
		return Encode(toEncode, urlEscapeChar, urlSpace, urlForbidden, false);
	}

	CSRAW public static string QPEncode(string toEncode, Encoding e = Encoding.UTF8) {
		byte[] data = Encode(e.GetBytes(toEncode), qpEscapeChar, qpSpace, qpForbidden, true);
		return WWW.DefaultEncoding.GetString(data, 0, data.Length);
	}

	CSRAW public static byte[] QPEncode(byte[] toEncode) {
		return Encode(toEncode, qpEscapeChar, qpSpace, qpForbidden, true);
	}

	CSRAW public static byte[] Encode(byte[] input, byte escapeChar, byte space, byte[] forbidden, bool uppercase) {
		using(MemoryStream memStream = new MemoryStream(input.Length*2)) {
			// encode
			for(int i=0; i < input.Length; i++) {
				if(input[i] == 32) {
					memStream.WriteByte(space);
				} else if(input[i] < 32 || input[i] > 126 || ByteArrayContains(forbidden, input[i])){
					memStream.WriteByte(escapeChar);
					memStream.Write(Byte2Hex(input[i],uppercase?ucHexChars:lcHexChars),0,2);
				} else {
					memStream.WriteByte(input[i]);
				}
			}

			return memStream.ToArray();
		}

	}

	CSRAW private static bool ByteArrayContains(byte[] array, byte b)
	{
		return (System.Array.IndexOf(array, b) != -1);
	}
	
	CSRAW public static string URLDecode(string toEncode, Encoding e = Encoding.UTF8) {
		byte []data = Decode(WWW.DefaultEncoding.GetBytes(toEncode), urlEscapeChar, urlSpace);
		return e.GetString(data, 0, data.Length);
	}

	CSRAW public static byte[] URLDecode(byte[] toEncode) {
		return Decode(toEncode, urlEscapeChar, urlSpace);
	}

	CSRAW public static string QPDecode(string toEncode, Encoding e = Encoding.UTF8) {
		byte[] data = Decode(WWW.DefaultEncoding.GetBytes(toEncode), qpEscapeChar, qpSpace);
		return e.GetString(data, 0, data.Length);
	}

	CSRAW public static byte[] QPDecode(byte[] toEncode){
		return Decode(toEncode, qpEscapeChar, qpSpace);
	}

	CSRAW public static byte[] Decode(byte[] input, byte escapeChar, byte space) {
		using(MemoryStream memStream = new MemoryStream(input.Length)) {
			// decode
			for(int i=0; i < input.Length; i++) {
				if(input[i] == space) {
					memStream.WriteByte((byte)32);
				} else if(input[i] == escapeChar && i+2 < input.Length) {
					i++;
					memStream.WriteByte(Hex2Byte(input,i++));
				} else {
					memStream.WriteByte(input[i]);
				}
			}

			return memStream.ToArray();
		}

	}

	CSRAW public static bool SevenBitClean(string s, Encoding e = Encoding.UTF8) {
		return SevenBitClean(e.GetBytes(s));
	}

	CSRAW public static bool SevenBitClean(byte[] input) {
		for(int i= 0; i<input.Length; i++) {
			if (input[i] < 32 || input[i] > 126 )
				return false;
		}

		return true;
	}

*/
}