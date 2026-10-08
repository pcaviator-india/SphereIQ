using System;

namespace EzyPlanData;

public static class Utility_Conversions
{
	public enum StringToAzimuthResult
	{
		ConversionSuccessful,
		NotValidCompassBearing,
		ValueOutOfDecimalRange,
		IncorectFormat,
		StringIsNull
	}

	public enum StringToDipResult
	{
		ConversionSuccessful,
		NotValidDip,
		ValueOutOfDecimalRange,
		IncorectFormat,
		StringIsNull
	}

	public enum StringToMDResult
	{
		ConversionSuccessful,
		NotValidMD,
		ValueOutOfDecimalRange,
		IncorectFormat,
		StringIsNull
	}

	public enum StringToDecimalResult
	{
		NoError,
		ValueOutOfDecimalRange,
		StringIsNull,
		IncorectFormat
	}

	public static StringToAzimuthResult StringToAzimuth(string s, out decimal azimuth)
	{
		switch (StringToDecimal(s, out azimuth))
		{
		case StringToDecimalResult.NoError:
			if ((azimuth >= 0m) & (azimuth <= 360m))
			{
				return StringToAzimuthResult.ConversionSuccessful;
			}
			return StringToAzimuthResult.NotValidCompassBearing;
		case StringToDecimalResult.ValueOutOfDecimalRange:
			return StringToAzimuthResult.ValueOutOfDecimalRange;
		case StringToDecimalResult.StringIsNull:
			return StringToAzimuthResult.StringIsNull;
		case StringToDecimalResult.IncorectFormat:
			return StringToAzimuthResult.IncorectFormat;
		default:
			return StringToAzimuthResult.ConversionSuccessful;
		}
	}

	public static string ToHumanReadable(StringToAzimuthResult result)
	{
		return result switch
		{
			StringToAzimuthResult.ConversionSuccessful => "The azimuth value entered was successfully converted into an azimuth", 
			StringToAzimuthResult.NotValidCompassBearing => "The azimuth value entered is outside the range of valid compass bearings (0 to 360 degrees inclusive)", 
			StringToAzimuthResult.ValueOutOfDecimalRange => "The azimuth value entered is outside the range of supported decimal numbers", 
			StringToAzimuthResult.IncorectFormat => "The azimuth value entered is incorrectly formated for a decimal number", 
			StringToAzimuthResult.StringIsNull => "No azimuth value has been entered", 
			_ => "Unknown Error Occured", 
		};
	}

	public static StringToDipResult StringToDip(string s, out decimal dip)
	{
		switch (StringToDecimal(s, out dip))
		{
		case StringToDecimalResult.NoError:
			if ((dip >= -90m) & (dip <= 90m))
			{
				if (Math.Sign(dip) == -1)
				{
					dip += 90m;
				}
				return StringToDipResult.ConversionSuccessful;
			}
			return StringToDipResult.NotValidDip;
		case StringToDecimalResult.ValueOutOfDecimalRange:
			return StringToDipResult.ValueOutOfDecimalRange;
		case StringToDecimalResult.StringIsNull:
			return StringToDipResult.StringIsNull;
		case StringToDecimalResult.IncorectFormat:
			return StringToDipResult.IncorectFormat;
		default:
			return StringToDipResult.ConversionSuccessful;
		}
	}

	public static string ToHumanReadable(StringToDipResult result)
	{
		return result switch
		{
			StringToDipResult.ConversionSuccessful => "The Dip value entered was successfully converted", 
			StringToDipResult.NotValidDip => "The Dip value entered is not a valid dip (0 to 90 inclusive or 0 to -90 inclusive)", 
			StringToDipResult.ValueOutOfDecimalRange => "The Dip value entered is outside the range of supported decimal numbers", 
			StringToDipResult.IncorectFormat => "The Dip value entered is incorrectly formated for a decimal number", 
			StringToDipResult.StringIsNull => "No Dip value has been entered", 
			_ => "Unknown Error Occured", 
		};
	}

	public static StringToMDResult StringToMD(string s, out decimal md)
	{
		switch (StringToDecimal(s, out md))
		{
		case StringToDecimalResult.NoError:
			if (md >= 0m)
			{
				return StringToMDResult.ConversionSuccessful;
			}
			return StringToMDResult.NotValidMD;
		case StringToDecimalResult.ValueOutOfDecimalRange:
			return StringToMDResult.ValueOutOfDecimalRange;
		case StringToDecimalResult.StringIsNull:
			return StringToMDResult.StringIsNull;
		case StringToDecimalResult.IncorectFormat:
			return StringToMDResult.IncorectFormat;
		default:
			return StringToMDResult.ConversionSuccessful;
		}
	}

	public static string ToHumanReadable(StringToMDResult result)
	{
		return result switch
		{
			StringToMDResult.ConversionSuccessful => "The MD value entered was successfully converted", 
			StringToMDResult.NotValidMD => "The MD value entered is not a valid MD (Positive number)", 
			StringToMDResult.ValueOutOfDecimalRange => "The MD value entered is outside the range of supported decimal numbers", 
			StringToMDResult.IncorectFormat => "The MD value entered is incorrectly formated for a decimal number", 
			StringToMDResult.StringIsNull => "No MD value has been entered", 
			_ => "Unknown Error Occured", 
		};
	}

	public static StringToDecimalResult StringToDecimal(string s, out decimal number)
	{
		try
		{
			number = decimal.Parse(s);
			return StringToDecimalResult.NoError;
		}
		catch (OverflowException)
		{
			number = 0m;
			return StringToDecimalResult.ValueOutOfDecimalRange;
		}
		catch (ArgumentNullException)
		{
			number = 0m;
			return StringToDecimalResult.StringIsNull;
		}
		catch (FormatException)
		{
			number = 0m;
			return StringToDecimalResult.IncorectFormat;
		}
	}

	public static string ToHumanReadable(StringToDecimalResult result)
	{
		return result switch
		{
			StringToDecimalResult.NoError => "Operation completed successfully", 
			StringToDecimalResult.ValueOutOfDecimalRange => "The number entered is too large", 
			StringToDecimalResult.StringIsNull => "No string has been provided", 
			StringToDecimalResult.IncorectFormat => "The number entered is incorectly formated", 
			_ => "Unexpected error", 
		};
	}
}
