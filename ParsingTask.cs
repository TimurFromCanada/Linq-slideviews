using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace linq_slideviews
{
	public class ParsingTask
	{
		/// <param name="lines">все строки файла, которые нужно распарсить. Первая строка заголовочная.</param>
		/// <returns>Словарь: ключ — идентификатор слайда, значение — информация о слайде</returns>
		/// <remarks>Метод должен пропускать некорректные строки, игнорируя их</remarks>
		public static IDictionary<int, SlideRecord> ParseSlideRecords(IEnumerable<string> lines)
		{
			if (lines == null)
				throw new InvalidOperationException();

			return lines
				.Skip(1)
				.Select(str => str.Split(';'))
				.Select(ParseStringWithSlides)
				.Where(slideRecord => slideRecord != null)
				.ToDictionary(slideRecord => slideRecord.SlideId);
		}

		public static SlideRecord ParseStringWithSlides(string[] array)
		{
			SlideType type;
			int id;

			if (array.Length != 3)
				return null;

			if (!Enum.TryParse(array[1], true, out type))
				return null;

			if (!int.TryParse(array[0], out id))
				return null;

			return new SlideRecord(id, type, array[2]);
		}

		/// <param name="lines">все строки файла, которые нужно распарсить. Первая строка — заголовочная.</param>
		/// <param name="slides">Словарь информации о слайдах по идентификатору слайда. 
		/// Такой словарь можно получить методом ParseSlideRecords</param>
		/// <returns>Список информации о посещениях</returns>
		/// <exception cref="FormatException">Если среди строк есть некорректные</exception>
		public static IEnumerable<VisitRecord> ParseVisitRecords(
			IEnumerable<string> lines, IDictionary<int, SlideRecord> slides)
		{
			return lines
				.Skip(1)
				.Select(line => ParseStringWithVisits(line, slides));
		}

		public static VisitRecord ParseStringWithVisits(string str, IDictionary<int, SlideRecord> slides)
		{
			try
			{
				var array = str.Split(';');
				int userID = int.Parse(array[0]);
				int slideID = int.Parse(array[1]);
				SlideType slideType = slides[slideID].SlideType;
				DateTime dateTime = DateTime.ParseExact(array[2] + ' ' + array[3], "yyyy-MM-dd HH:mm:ss",
					CultureInfo.InvariantCulture, DateTimeStyles.None);

				return new VisitRecord(userID, slideID, dateTime, slideType);
			}
			catch (Exception exc)
			{
				throw new FormatException($"Wrong line [{str}]", exc);
			}
		}
	}
}


