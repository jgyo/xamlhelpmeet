using System;
using System.Text;

namespace XamlHelpmeet.Extensions
{
	/// <summary>
	/// 	String extentions.
	/// </summary>
	public static class StringExtensions
	{
		/// <summary>
		/// 	A char extension method that queries if a character is letter or digit.
		/// </summary>
		/// <param name="target">
		/// 	The target to act on.
		/// </param>
		/// <returns>
		/// 	true if the character is letter or digit, otherwise false.
		/// </returns>
		public static bool IsLetterOrDigit(this char target)
		{
			return char.IsLetterOrDigit(target);
		}

		/// <summary>
		/// 	A string extension method that queries if a specified character in a string is
		/// 	letter or digit.
		/// </summary>
		/// <param name="target">
		/// 	The target to act on.
		/// </param>
		/// <param name="index">
		/// 	Zero-based index of the character to test.
		/// </param>
		/// <returns>
		/// 	true if the character is a letter or digit, otherwise false.
		/// </returns>
		public static bool IsLetterOrDigit(this string target, int index)
		{
			return target[index].IsLetterOrDigit();
		}

		/// <summary>
		/// 	A string extension method that query if a string is not null.
		/// </summary>
		/// <param name="Target">
		/// 	The Target to act on.
		/// </param>
		/// <returns>
		/// 	true if not null, false if not.
		/// </returns>
		public static bool IsNotNull(this string Target)
		{
			return !Target.IsNull();
		}

		/// <summary>
		/// 	A string extension method that queries if a string is not null or empty.
		/// </summary>
		/// <param name="Target">
		/// 	The Target to act on.
		/// </param>
		/// <returns>
		/// 	true if a not null or empty, false if it is null or empty.
		/// </returns>
		public static bool IsNotNullOrEmpty(this string Target)
		{
			return !Target.IsNullOrEmpty();
		}

		/// <summary>
		/// 	A string extension method that queries if the string is null, empty or whitespace.
		/// </summary>
		/// <param name="Target">
		/// 	The Target to act on.
		/// </param>
		/// <returns>
		/// 	true if a null, empty, or whitespace, false if not.
		/// </returns>
		public static bool IsNotNullOrWhiteSpace(this string Target)
		{
			return !Target.IsNullOrWhiteSpace();
		}

		/// <summary>
		/// 	A char extension method that query if a character is not uppercase.
		/// </summary>
		/// <param name="c">
		/// 	The character to test.
		/// </param>
		/// <returns>
		/// 	true if not uppercase, otherwise false.
		/// </returns>
		public static bool IsNotUpper(this char c)
		{
			return !c.IsUpper();
		}

		/// <summary>
		/// 	A string extension method that queries if a specified character is not uppercase.
		/// </summary>
		/// <param name="target">
		/// 	The target to act on.
		/// </param>
		/// <param name="index">
		/// 	Zero-based index of the character to test.
		/// </param>
		/// <returns>
		/// 	true if not uppercase, otherwise false.
		/// </returns>
		public static bool IsNotUpper(this string target,
									  int index)
		{
			return !target.IsUpper(index);
		}

		/// <summary>
		/// 	A string extension method that queries if 'Target' is null.
		/// </summary>
		/// <param name="Target">
		/// 	The Target to act on.
		/// </param>
		/// <returns>
		/// 	true if null, otherwise false.
		/// </returns>
		public static bool IsNull(this string Target)
		{
			return Target == null;
		}

		/// <summary>
		/// 	A string extension method that queries if a null or is empty.
		/// </summary>
		/// <param name="target">
		/// 	The target to act on.
		/// </param>
		/// <returns>
		/// 	true if a null or is empty, otherwise false.
		/// </returns>
		public static bool IsNullOrEmpty(this string target)
		{
			return string.IsNullOrEmpty(target);
		}

		/// <summary>
		/// 	A string extension method that query if a string is is null.
		/// </summary>
		/// <summary>
		/// 	A string extension method that query if a string is null, empty or white space.
		/// </summary>
		/// <param name="Target">
		/// 	The Target to act on.
		/// </param>
		/// <returns>
		/// 	true if null, false if not.
		/// </returns>
		/// <returns>
		/// 	true if null or white space, false if not.
		/// </returns>
		public static bool IsNullOrWhiteSpace(this string Target)
		{
			return string.IsNullOrWhiteSpace(Target);
		}

		/// <summary>
		/// 	A string extension method that query if a specified character in a string is
		/// 	uppercase.
		/// </summary>
		/// <param name="target">
		/// 	The target to act on.
		/// </param>
		/// <param name="index">
		/// 	Zero-based index of the character to test.
		/// </param>
		/// <returns>
		/// 	true if uppercase, otherwise false.
		/// </returns>
		public static bool IsUpper(this string target, int index)
		{
			return target[index].IsUpper();
		}

		/// <summary>
		/// 	A character extension method that queries if a character is upcase.
		/// </summary>
		/// <param name="c">
		/// 	The character to test.
		/// </param>
		/// <returns>
		/// 	true if uppercase, otherwise false.
		/// </returns>
		public static bool IsUpper(this char c)
		{
			return char.IsUpper(c);
		}

		/// <summary>
		/// 	A char extension method that converts a Target character to a lowercase character.
		/// </summary>
		/// <param name="Target">
		/// 	The Target character to act on.
		/// </param>
		/// <returns>
		/// 	Target as a lowercase chararacter.
		/// </returns>
		public static char ToLower(this char Target)
		{
			if (Target >= 'A' && Target <= 'Z')
			{
				return (char)(Target - 'A' + 'a');
			}
			return Target;
		}
	}
}