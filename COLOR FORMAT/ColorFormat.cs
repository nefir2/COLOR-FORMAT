using System;

namespace COLOR_FORMAT
{
	/// <summary>
	/// форматирование цвета в консоли.
	/// </summary>
	public static class ColorFormat
	{
		#region constants
		/// <summary>
		/// знак, который определяется как указание смены цвета текста.
		/// </summary>
		public const char FORE_SIGN = '%';
		/// <summary>
		/// знак, который определяется как указание смены цвета фона текста.
		/// </summary>
		public const char BACK_SIGN = '&';
		/// <summary>
		/// знак, после и до которого не меняется цвет текста.
		/// </summary>
		/// <remarks>
		/// всё что идёт после этого знака - <see cref="Writer(string, ConsoleColor[])"/> не преобразовывает цвета, в плоть до конца строки или этого же знака.
		/// </remarks>
		public const char STOP_SIGN = '~';
		/// <summary>
		/// открывающая скобка.
		/// </summary>
		/// <remarks>
		/// рассчитано на использование в случае если аргументов с цветом больше 9.
		/// </remarks>
		public const char OPEN_BRACKET_SIGN = '{';
		/// <summary>
		/// закрывающая скобка.
		/// </summary>
		/// <remarks>
		/// рассчитано на использование в случае если аргументов с цветом больше 9.
		/// </remarks>
		public const char CLOSE_BRACKET_SIGN = '}';
		/// <summary>
		/// стандартное значение цвета текста.
		/// </summary>
		/// <returns>цвет текста в консоли перед началом работы методов.</returns>
		public static ConsoleColor DefaultFore { get; private set; }
		/// <summary>
		/// стандартное значение цвета фона текста.
		/// </summary>
		/// <returns>цвет фона текста в консоли перед началом работы методов.</returns>
		public static ConsoleColor DefaultBack { get; private set; }

		/// <summary>
		/// статический конструктор для выдачи значений полям.
		/// </summary>
		static ColorFormat()
		{
			DefaultFore = Console.ForegroundColor;
			DefaultBack = Console.BackgroundColor;
		}
		#endregion cnsts
		#region parser and writer
		#region use writer
		/// <summary>
		/// выводит строку с указанными цветами и их позициями. <br/>
		/// пример: <example>"%0 %{10}"</example>
		/// </summary>
		/// <remarks>
		/// для определения места вывода используйте %{номер параметра цвета}. <br/>
		/// или амперсанд вместо процента для смены фона. <br/><br/>
		/// 
		/// после завершения работы восстанавливает цвет на тот, <br/>
		/// что был перед первым вызовом класса. <br/><br/>
		/// 
		/// переносит курсор на следующую строку после завершения работы метода.
		/// </remarks>
		/// <param name="value">строка, в которой используются цвета</param>
		/// <param name="colors">параметры подставляемых цветов, на которые ссылаются спец-знаки.</param>
		/// <exception cref="ArgumentException"/>
		public static void WriteLine(string value, params ConsoleColor[] colors)
		{
			Write(value, colors);
			Console.WriteLine();
		}
		/// <summary>
		///	выводит строку с указанными цветами и их позициями. <br/>
		///	пример: <example>"%0 %{10}"</example>
		/// </summary>
		/// <remarks>
		///	для определения места вывода используйте %{номер параметра цвета}. <br/>
		///	или амперсанд вместо процента для смены фона. <br/><br/>
		/// 
		///	переносит курсор на следующую строку после завершения работы метода.
		/// <example>
		///	<code>
		///	ColorFormat.WriteLine("%0text %1text1");
		///	</code>
		/// </example>
		/// </remarks>
		/// <param name="value">строка, в которой используются цвета.</param>
		/// <param name="doNotSetWithEndDefaultColor">
		///	если значение <see langword="true"/>, <br/>
		///	то после выведенной строки - <br/>
		///	цвет текста и фона возвращается на тот, <br/>
		///	что был перед первым вызовом метода.
		/// </param>
		/// <param name="colors">
		///	параметры подставляемых цветов, <br/>
		///	на которые ссылаются специальные знаки.
		/// </param>
		/// <exception cref="ArgumentException"/>
		public static void WriteLine(string value, bool doNotSetWithEndDefaultColor, params ConsoleColor[] colors)
		{
			Write(value, doNotSetWithEndDefaultColor, colors);
			Console.WriteLine();
		}
		/// <summary>
		/// выводит строку с указанными цветами и их позициями. <br/>
		/// пример: <example>"%0 %{10}"</example>
		/// </summary>
		/// <remarks>
		/// для определения места вывода используйте %{номер параметра цвета}. <br/>
		/// или амперсанд вместо процента для смены фона. <br/><br/>
		/// 
		/// после завершения работы восстанавливает цвет на тот, <br/>
		/// что был перед первым вызовом класса.
		/// </remarks>
		/// <param name="value">строка, в которой используются цвета</param>
		/// <param name="colors">параметры подставляемых цветов, на которые ссылаются спец-знаки.</param>
		/// <exception cref="ArgumentException"/>
		public static void Write(string value, params ConsoleColor[] colors)
		{
			Writer(value, colors);
			Console.ForegroundColor = DefaultFore; //возвращение цветов на те, что были до начала использования метода, после окончания работы метода.
			Console.BackgroundColor = DefaultBack;
		}
		/// <summary>
		/// выводит строку с указанными цветами и их позициями. <br/>
		/// пример: <example>"%0 %{10}"</example>
		/// </summary>
		/// <remarks>
		/// для определения места вывода используйте %{номер параметра цвета}. <br/>
		/// или амперсанд вместо процента для смены фона.
		/// </remarks>
		/// <param name="value">строка, в которой используются цвета.</param>
		/// <param name="doNotSetWithEndDefaultColor">
		/// если значение <see langword="true"/>, <br/>
		/// то после выведенной строки - <br/>
		/// цвет текста и фона возвращается на тот, <br/>
		/// что был перед первым вызовом метода.
		/// </param>
		/// <param name="colors">
		/// параметры подставляемых цветов, <br/>
		/// на которые ссылаются специальные знаки.
		/// </param>
		/// <exception cref="ArgumentException"/>
		public static void Write(string value, bool doNotSetWithEndDefaultColor, params ConsoleColor[] colors)
		{
			Writer(value, colors); //парс текста и подстановка цветов.
			if (doNotSetWithEndDefaultColor) return; //не возвращать цвет если true
			Console.ForegroundColor = DefaultFore; //возвращение цветов на те, что были до начала использования метода, после окончания работы метода.
			Console.BackgroundColor = DefaultBack;
		}
		#endregion
		/// <summary>
		/// скрытый метод парсера.
		/// </summary>
		/// <remarks>
		/// скрыт и переименован из-за совпадения сигнатуры с методом-оболочкой.
		/// </remarks>
		/// <param name="value">строка, в которой используются цвета.</param>
		/// <param name="colors">параметры подставляемых цветов, на которые ссылаются спец-знаки.</param>
		private static void Writer(string value, params ConsoleColor[] colors)
		{
			//сделать проверку на неуказанные позиции цветов, и если цвета всего два, то первый из них цвет текста, а второй - цвет фона.


			for (int i = 0; i < value.Length; i++) //начало парсера. проход по массиву строки
			{
				if (value[i] == STOP_SIGN) //парсер стоп знака.
				{
					if (i != value.Length - 1)
					{
						if (value[i] + 1 == STOP_SIGN)
						{
							Console.Write(STOP_SIGN);
							i++;
							continue;
						}

						bool isHasEnd = false;
						int endVal = -1;
						for (int j = i + 1; j < value.Length; j++)
						{
							if (value[j] != STOP_SIGN)
							{
								continue;
							}

							if (j == value.Length - 1)
							{
								isHasEnd = false;
								break;
							}
							isHasEnd = true;
							endVal = j + 1;
							break;
						}
						if (isHasEnd)
						{
							for (int j = i + 1; j < endVal - 1; j++) Console.Write(value[j]);
							i = endVal;
						}
						else
						{
							for (int j = i + 1; j < value.Length; j++) Console.Write(value[j]);
							break;
						}
					}
					else continue;
				}

				if (value[i] == FORE_SIGN || value[i] == BACK_SIGN) //поиск ключевых знаков
				{
					//как насчёт парсить отрицательные числа? чтобы не указывать явно после строки 'colorformat.defaultfore', а просто указать "%-1" в самой строке ?
					if (i + 1 != value.Length && int.TryParse(value[i + 1].ToString(), out int arg)) //если ЭТА итерация не равна последнему элементу полученной строки И следующий знак - число
					{
						try { ChangeColor(value[i] == FORE_SIGN, colors[arg]); } //попытка установить указанный цвет.
						catch { throw new ArgumentException($"указанный в строке параметр с номером '{arg}' не был найден.", nameof(colors)); } //при неудаче выбрасывается исключение с заданным сообщением.
						i++; //номер указанного цвета не выводится
						continue; //продолжается поиск ключевых знаков в строке.
					}
					else if (value[i + 1] == OPEN_BRACKET_SIGN && !(i + 3 >= value.Length)) //иначе если следующий знак - фигурная скобка И хватает места хотя бы на одно число и закрывающую скобку (пример: %{0})
					{
						//сделать отмену - на случай, если понадобиться вывести текст, выглядящий как форматированный цвет, но при этом им не являющимся.
						//идея для отмены - символ ~. 

						for (int j = i + 2; j < value.Length; j++) //цикл для получения параметра в фигурных скобках.
						{
							if (value[j] == CLOSE_BRACKET_SIGN) //если найдена закрывающая фигурная скобка
							{
								if (int.TryParse(Cut(value, i + 2, j), out arg)) //попытка получить число в фигурных скобках
								{
									try { ChangeColor(value[i] == FORE_SIGN, colors[arg]); } //смена цвета на указанный цвет в параметрах
									catch { throw new ArgumentException($"указанный в строке параметр с номером /{arg}/ не был найден.", nameof(colors)); } //если указанного цвета в параметрах нет - выбрасывается исключение с заданным текстом.
									i = j; //параметры цветов не выводятся
									break; //выход из цикла получения номера параметра в скобках
								}
								else throw new ArgumentException("указанный номер параметра не является числом."); //если не получилось - выбрасывается исключение с заданным текстом.
																												   //как насчёт не выкидывать исключение, а выводить то что могло быть форматированием как есть?
							}
							else if (j == value.Length - 1) throw new ArgumentException($"закрывающая скобка \"{CLOSE_BRACKET_SIGN}\" не была найдена."); //если цикл дошёл до конца, но закрывающая скобка была не найдена - выбрасывается исключение.
						}
					}
					else Console.Out.Write(value[i]); //если ключевой знак не найден - вывод знака итерации в консоль.
				}
				else Console.Out.Write(value[i]); //если никаких ключевых знаков не найдено - вывод знака в этой итерации в консоль.
			}
		}
		#endregion
		#region additional methods to parser and writer
		/// <summary> изменяет цвет последующих знаков на указанный в параметрах. </summary>
		/// <param name="isForeground">
		/// если <see langword="true"/> устанавливается цвет в <see cref="Console.ForegroundColor"/>, <br/>
		/// иначе цвет устанавливается в <see cref="Console.BackgroundColor"/>. 
		/// </param>
		/// <param name="color">цвет, на который меняется текст или фон текста.</param>
		public static void ChangeColor(bool isForeground, ConsoleColor color)
		{
			if (isForeground) Console.ForegroundColor = color;
			else Console.BackgroundColor = color;
		}
		/// <summary>
		/// метод вырезающий подстроку из общей строки по указанным позициям.
		/// </summary>
		/// <remarks>
		/// <paramref name="start"/>: знак под указанным номером возвращается в составе подстроки. <br/>
		/// <paramref name="end"/>: знак под указанным номером не возвращается в составе подстроки.
		/// </remarks>
		/// <param name="value">строка, из которой вырезается подстрока.</param>
		/// <param name="start">точка начала выреза.</param>
		/// <param name="end">точка окончания выреза.</param>
		/// <returns>вырезанная подстрока типа <see cref="string"/>.</returns>
		/// <exception cref="ArgumentException"/>
		/// <exception cref="IndexOutOfRangeException"/>
		private static string Cut(string value, int start, int end)
		{
			if (start > end) throw new ArgumentException("начальная позиция для вырезания подстроки не может быть больше конечной позиции.");
			if (start < 0 || end < 0) throw new IndexOutOfRangeException("начало или конец вырезания подстроки не может быть меньше нуля.");
			if (start > value.Length || end > value.Length) throw new IndexOutOfRangeException("начало или конец вырезания подстроки не может быть больше длины строки.");

			string cutted = "";
			for (int i = start; i < end; i++) cutted += value[i];
			return cutted;
		}
		#endregion
	}
}
